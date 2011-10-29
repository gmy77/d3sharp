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
using System.Windows;
using Mooege.Common;
using Mooege.Common.MPQ;
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Common.Types.Collision;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Common.Types.SNO;
using Mooege.Core.GS.Common.Types.Scene;
using Mooege.Core.GS.Objects;
using Mooege.Net.GS.Message.Definitions.Map;
using Mooege.Net.GS.Message.Definitions.Scene;

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
                    if (this._world != null) // this is bugged i guess? /raist
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

        public Scene(World world, Vector3D position, int sceneSNO, Scene parent)
            : base(world, world.NewSceneID)
        {
            this.Scale = 1.0f;
            this.RotationAmount = 0.0f;
            this.Subscenes = new List<Scene>();

            this.SceneSNO = sceneSNO;
            this.Parent = parent;
            this.AppliedLabels = new int[0];

            this.LoadSceneData();
            this.Position = position;
            this.Bounds = new Rect(this.Position.X, this.Position.Y, this.NavZone.V0.X*this.NavZone.Float0, this.NavZone.V0.Y*this.NavZone.Float0);

            this.World.AddScene(this);
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

        /// Loads all Actors for a scene chunk. TODO Remove hack that this method returns a vector for starting positions. Better to load all actors and search for the appropriate starting point afterwards
        public void LoadActors()
        {
            foreach (var markerSet in this.MarkerSets)
            {
                var markerSetData = MPQStorage.Data.Assets[SNOGroup.MarkerSet][markerSet].Data as Mooege.Common.MPQ.FileFormats.MarkerSet;
                if (markerSetData == null) return;

                foreach (var marker in markerSetData.Markers)
                {
                    if (marker.SNOName.Group != SNOGroup.Actor) continue; // skip non-actor markers.

                    var position = marker.PRTransform.Vector3D + this.Position; // calculate the position for the actor.
                    var actor = ActorFactory.Create(this.World, marker.SNOName.SNOId, position, marker.TagMap); // try to create it.
                    if (actor == null) continue;

                    actor.RotationAmount = marker.PRTransform.Quaternion.W;
                    actor.RotationAxis = marker.PRTransform.Quaternion.Vector3D;

                    this.World.Enter(actor);
                }
            }
        }

        public override bool Reveal(Player.Player player)
        {
            if (player.RevealedObjects.ContainsKey(this.DynamicID)) return false; // already revealed
            player.RevealedObjects.Add(this.DynamicID, this);
            player.InGameClient.SendMessage(this.RevealMessage,true);
            player.InGameClient.SendMessage(this.MapRevealMessage,true);
            foreach (var sub in this.Subscenes)
            {
                sub.Reveal(player);
            }

            Logger.Trace("Revealing {0}", this);
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

        public override string ToString()
        {
            return string.Format("Scene SNOId: {0} Position: {1}", this.SceneSNO, this.Position);
        }
    }
}
