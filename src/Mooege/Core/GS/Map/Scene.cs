﻿/*
 * Copyright (C) 2011 mooege project
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using System.Collections.Generic;
using Mooege.Common;
using Mooege.Common.Helpers;
using Mooege.Common.MPQ;
using Mooege.Common.MPQ.FileFormats.Types;
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Actors.Implementations;
using Mooege.Core.GS.Common.Types.Collusion;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Common.Types.SNO;
using Mooege.Core.GS.Common.Types.Scene;
using Mooege.Core.GS.Game;
using Mooege.Core.GS.Objects;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message.Definitions.Map;
using Mooege.Net.GS.Message.Definitions.Scene;
using Mooege.Core.GS.Markers;

// NOTE: Scenes are typically (always, hopefully) 240x240 in size. Cells are 60x60 with
//  subscenes able to be positioned relative to their parents

namespace Mooege.Core.GS.Map
{
    public enum MiniMapVisibility
    {
        Hidden = 0,
        Revealed = 1,
        Visited = 2
    }

    public sealed class Scene : WorldObject
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        public Vector3D StartPosition { get; set; }

        public override World World
        {
            get { return this._world; }
            set
            {
                if (this._world != value)
                {
                    if (this._world != null)
                        this._world.AddScene(this);
                    this._world = value;
                    if (this._world != null)
                        this._world.RemoveScene(this);
                }
            }
        }

        public uint ParentChunkID
        {
            get { return (this.Parent != null) ? this.Parent.DynamicID : 0xFFFFFFFF; }
        }

        public RevealSceneMessage RevealMessage
        {
            get
            {
                return new RevealSceneMessage
                {
                    WorldID = this.World.DynamicID,
                    SceneSpec = this.Specification,
                    ChunkID = this.DynamicID,
                    Transform = this.Transform,
                    SceneSNO = this.SceneSNO,
                    ParentChunkID = this.ParentChunkID,
                    SceneGroupSNO = this.SceneGroupSNO,
                    arAppliedLabels = this.AppliedLabels
                };
            }
        }

        public MapRevealSceneMessage MapRevealMessage
        {
            get
            {
                return new MapRevealSceneMessage
                {
                    ChunkID = this.DynamicID,
                    SceneSNO = this.SceneSNO,
                    Transform = this.Transform,
                    WorldID = this.World.DynamicID,
                    MiniMapVisibility = this.MiniMapVisibility
                };
            }
        }

        public SceneSpecification Specification;
        public int SceneSNO;
        public Scene Parent;
        public int SceneGroupSNO;
        public int /* gbid */[] AppliedLabels; // MaxLength = 256
        public MiniMapVisibility MiniMapVisibility;

        // read data from mpqs
        public AABB AABBBounds { get; private set; }
        public AABB AABBMarketSetBounds { get; private set; }
        public Mooege.Common.MPQ.FileFormats.Scene.NavMeshDef NavMesh { get; private set; }
        public List<int> MarkerSets = new List<int>();
        public string LookLink { get; private set; }
        public Mooege.Common.MPQ.FileFormats.Scene.NavZoneDef NavZone { get; private set; }

        public readonly List<Scene> Subscenes;

        public PRTransform Transform
        {
            get { return new PRTransform { Quaternion = new Quaternion { W = this.RotationAmount, Vector3D = this.RotationAxis }, Vector3D = this.Position }; }
        }

        public Scene(World world, int sceneSNO, Scene parent)
            : base(world, world.NewSceneID)
        {
            this.Scale = 1.0f;
            this.RotationAmount = 0.0f;
            this.Subscenes = new List<Scene>();

            this.SceneSNO = sceneSNO;
            this.Parent = parent;
            this.AppliedLabels = new int[0];

            this.World.AddScene(this);
            this.LoadSceneData();
        }

        private void LoadSceneData()
        {
            var data = MPQStorage.Data.Assets[SNOGroup.Scene][this.SceneSNO].Data as Mooege.Common.MPQ.FileFormats.Scene;
            if (data == null) return;

            this.AABBBounds = data.AABBBounds;
            this.AABBMarketSetBounds = data.AABBMarketSetBounds;
            this.NavMesh = data.NavMesh;
            this.MarkerSets = data.MarkerSets;
            this.LookLink = data.LookLink;
            this.NavZone = data.NavZone;
        }

        /// <summary>
        /// Helper method to print marker-sets for the scene.
        /// </summary>
        private void PrintMarkerSets()
        {
            foreach (var markerSet in this.MarkerSets)
            {
                var markerSetData = MPQStorage.Data.Assets[SNOGroup.MarkerSet][markerSet].Data as Mooege.Common.MPQ.FileFormats.MarkerSet;
                if (markerSetData == null) return;

                Logger.Trace("MarketSet: {0} [{1}]", markerSetData.Header.SNOId, markerSetData.FileName);
                foreach (var marker in markerSetData.Markers)
                {
                    Logger.Trace(marker.SNOName.ToString());
                }
            }
        }

        /// Loads all Actors for a scene chunk. TODO Remove hack that this method returns a vector for starting positions. Better to load all actors and search for the appropriate starting point afterwards
        public void LoadActors()
        {
            foreach (var markerSet in this.MarkerSets)
            {
                var markerSetData = MPQStorage.Data.Assets[SNOGroup.MarkerSet][markerSet].Data as Mooege.Common.MPQ.FileFormats.MarkerSet;
                if (markerSetData == null) return;

                foreach (var marker in markerSetData.Markers)
                {
                    // read tags.
                    var tags = new Dictionary<int, TagMapEntry>();
                    foreach (var tag in marker.TagMap.TagMapEntries)
                        tags.Add(tag.Int1, tag);

                    if (marker.SNOName.Group != SNOGroup.Actor) continue;
                    if (!MPQStorage.Data.Assets[SNOGroup.Actor].ContainsKey(marker.SNOName.SNOId)) continue;

                    // Since we are not loading all actors, make sure to load portals and portal destination actors - fix this /raist.
                    if ( ActorFactory.HasHandler(marker.SNOName.SNOId) 
                         || tags.ContainsKey((int)MarkerTagTypes.DestinationWorld) 
                         || tags.ContainsKey((int)MarkerTagTypes.ActorTag))
                         //|| RandomHelper.Next(100) > 90    
                    {                       
                        Actor newActor = null;

                        // This is ugly, because the ActorFactory does not differentiate between Gizmos, so when creating a portal, we have to do it manually
                        if (tags.ContainsKey((int)MarkerTagTypes.DestinationWorld))
                        {
                            newActor = new Portal(this.World);
                            newActor.ActorSNO = marker.SNOName.SNOId;
                            newActor.Field8 = marker.SNOName.SNOId;
                            newActor.Position = marker.PRTransform.Vector3D + this.Position;
                            (newActor as Portal).Destination.WorldSNO = tags[(int)MarkerTagTypes.DestinationWorld].Int2;

                            if (tags.ContainsKey((int)MarkerTagTypes.DestinationLevelArea))
                                (newActor as Portal).Destination.DestLevelAreaSNO = tags[(int)MarkerTagTypes.DestinationLevelArea].Int2;

                            if (tags.ContainsKey((int)MarkerTagTypes.DestinationActorTag))
                                (newActor as Portal).Destination.StartingPointActorTag = tags[(int)MarkerTagTypes.DestinationActorTag].Int2;
                            else
                                Logger.Warn("Found portal {0} in scene {1} without target location actor", newActor.ActorSNO, this.SceneSNO);
                        }
                        else
                            newActor = ActorFactory.Create(marker.SNOName.SNOId, this.World, marker.PRTransform.Vector3D + this.Position);

                        if (newActor != null)
                        {
                            if (tags.ContainsKey((int)MarkerTagTypes.ActorTag))
                                newActor.Tag = tags[(int)MarkerTagTypes.ActorTag].Int2;

                            if (tags.ContainsKey((int)MarkerTagTypes.Scale))
                                newActor.Scale = tags[(int)MarkerTagTypes.Scale].Float0;

                            newActor.RotationAmount = marker.PRTransform.Quaternion.W;
                            newActor.RotationAxis = marker.PRTransform.Quaternion.Vector3D;

                            System.Diagnostics.Debug.Assert(newActor.ActorSNO != -1);

                            if (newActor is Waypoint) //TODO Do it in waypoint construct once grid is done /fasbat
                            {
                                //TODO this is just act 1 ! /fasbat
                                var actData = (Mooege.Common.MPQ.FileFormats.Act)MPQStorage.Data.Assets[SNOGroup.Act][70015].Data;
                                var wayPointInfo = actData.WayPointInfo;
                                for (int i = 0; i < actData.WayPointInfo.Length; i++)
                                {
                                    if (wayPointInfo[i].SNOLevelArea == -1)
                                        continue;
                                    var levelAreaFile = MPQStorage.Data.Assets[SNOGroup.LevelArea][wayPointInfo[i].SNOLevelArea];
                                    var levelArea = (Mooege.Common.MPQ.FileFormats.LevelArea)levelAreaFile.Data;
                                    foreach (var area in Specification.SNOLevelAreas)
                                    {
                                        if (wayPointInfo[i].SNOWorld == World.WorldSNO && wayPointInfo[i].SNOLevelArea == area)
                                        {
                                            Waypoint.Waypoints[i] = newActor as Waypoint;
                                        }
                                    }
                                }
                            }

                            if (!(newActor is Portal))
                                this.World.Enter(newActor);
                        }
                    }

                    if (marker.SNOName.SNOId == (int)MarkerTypes.Start_Location_0 || marker.SNOName.SNOId == (int)MarkerTypes.Start_Location_Team_0)
                    {
                        this.StartPosition = marker.PRTransform.Vector3D + this.Position;
                    }
                }
            }
        }

        public override bool Reveal(Mooege.Core.GS.Player.Player player)
        {
            if (player.RevealedObjects.ContainsKey(this.DynamicID)) return false; // already revealed
            player.RevealedObjects.Add(this.DynamicID, this);
            player.InGameClient.SendMessage(this.RevealMessage);
            player.InGameClient.SendMessage(this.MapRevealMessage,true);
            foreach (var sub in this.Subscenes)
            {
                sub.Reveal(player);
            }
                        
            return true;
        }

        public override bool Unreveal(Mooege.Core.GS.Player.Player player)
        {
            if (!player.RevealedObjects.ContainsKey(this.DynamicID)) return false; // not revealed yet
            player.InGameClient.SendMessage(new DestroySceneMessage() { WorldID = this.World.DynamicID, SceneID = this.DynamicID },true);
            
            
            foreach (var sub in this.Subscenes)
            {
                sub.Unreveal(player);
            }
            player.RevealedObjects.Remove(this.DynamicID);
            return true;
        }
    }
}
