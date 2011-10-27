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

                    //if (marker.SNOName.SNOId == (int)Markers.MarkerTypes.Start_Location_0 || marker.SNOName.SNOId == (int)Markers.MarkerTypes.Start_Location_Team_0)
                    // {
                    //    this.StartPosition = marker.PRTransform.Vector3D + this.Position;
                    //    continue; <-- this prevented startling locations to be loaded, portals need them
                    //}

                    Asset actorAsset = null;
                    Mooege.Common.MPQ.FileFormats.Actor actorData = null;
                    if (!MPQStorage.Data.Assets[SNOGroup.Actor].ContainsKey(marker.SNOName.SNOId)) continue;

                    actorAsset = MPQStorage.Data.Assets[SNOGroup.Actor][marker.SNOName.SNOId];
                    actorData = actorAsset.Data as Mooege.Common.MPQ.FileFormats.Actor;

                    // Since we are not loading all actors, make sure to load portals and portal destination actors
                    if (RandomHelper.Next(100) > 90 || tags.ContainsKey((int)MarkerTagTypes.DestinationWorld) || tags.ContainsKey((int)MarkerTagTypes.ActorTag))
                    {
                        if (marker.SNOName.Group == SNOGroup.Actor)
                        {
                            Actor newActor = null;

                            if (tags.ContainsKey((int)MarkerTagTypes.DestinationWorld))
                            {
                                newActor = new Portal(this.World);
                                (newActor as Portal).Destination.WorldSNO = tags[(int)MarkerTagTypes.DestinationWorld].Int2;

                                if (tags.ContainsKey((int)MarkerTagTypes.DestinationLevelArea))
                                    (newActor as Portal).Destination.DestLevelAreaSNO = tags[(int)MarkerTagTypes.DestinationLevelArea].Int2;

                                if (tags.ContainsKey((int)MarkerTagTypes.DestinationActorTag))
                                    newActor.Tag = tags[(int)MarkerTagTypes.DestinationActorTag].Int2;
                                else
                                    Logger.Warn("Found portal {0} in scene {1} without target location actor", newActor.ActorSNO, this.SceneSNO);
                            }
                            else
                                newActor = new Actor(this.World, this.World.NewActorID);

                            newActor.ActorSNO = marker.SNOName.SNOId;
                            newActor.Position = marker.PRTransform.Vector3D + this.Position;

                            if (tags.ContainsKey((int)MarkerTagTypes.ActorTag))
                                newActor.Tag = tags[(int)MarkerTagTypes.ActorTag].Int2;

                            if (tags.ContainsKey((int)MarkerTagTypes.Scale))
                                newActor.Scale = tags[(int)MarkerTagTypes.Scale].Float0;

                            newActor.RotationAmount = marker.PRTransform.Quaternion.W;
                            newActor.RotationAxis = marker.PRTransform.Quaternion.Vector3D;
                            newActor.Field2 = 16;
                            newActor.Field3 = 0;
                            newActor.Field7 = 0x00000001;
                            newActor.Field8 = newActor.ActorSNO;

                            if (!(newActor is Portal))
                                this.World.Enter(newActor);

                            // new actor loader
                            //------------------------------------------------------------------------------
                            // monter actors crashing for the code below /raist.
                            //var actor = ActorFactory.Create(marker.SNOName.SNOId, this.World, marker.PRTransform.Vector3D + this.Position);
                            //if (actor == null) continue;

                            //if (tags.ContainsKey(526852))
                            //    actor.Tag = tags[526852].Int2;

                            //if (tags.ContainsKey((int)Markers.MarkerTagTypes.Scale))
                            //    actor.Scale = tags[(int)Markers.MarkerTagTypes.Scale].Float0;

                            //actor.RotationAmount = marker.PRTransform.Quaternion.W;
                            //actor.RotationAxis = marker.PRTransform.Quaternion.Vector3D;

                            //if (!(actor is Portal))
                            //this.World.Enter(actor);
                            //------------------------------------------------------------------------------
                        }
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
