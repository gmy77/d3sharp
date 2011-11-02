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
using Mooege.Core.GS.Players;
using Mooege.Net.GS.Message.Definitions.Map;
using Mooege.Net.GS.Message.Definitions.Scene;

namespace Mooege.Core.GS.Map
{

    public sealed class Scene : WorldObject
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        /// <summary>
        /// SNOId of the scene.
        /// </summary>
        public int SNOId { get; private set; }

        /// <summary>
        /// Scene group's SNOId.
        /// Not sure on usage /raist.
        /// </summary>
        public int SceneGroupSNO { get; set; }

        /// <summary>
        /// Subscenes.
        /// </summary>
        public List<Scene> Subscenes { get; private set; }

        /// <summary>
        /// Parent scene if any.
        /// </summary>
        public Scene Parent;

        /// <summary>
        /// Parent scene's chunk id.
        /// </summary>
        public uint ParentChunkID
        {
            get { return (this.Parent != null) ? this.Parent.DynamicID : 0xFFFFFFFF; }
        }

        /// <summary>
        /// Visibility in MiniMap.
        /// </summary>
        public SceneMiniMapVisibility MiniMapVisibility { get; set; }

        /// <summary>
        /// Scene Specification.
        /// </summary>
        public SceneSpecification Specification { get; set; }

        /// <summary>
        /// Applied labels.
        /// Not sure on usage /raist.
        /// </summary>
        public int[] AppliedLabels;       

        /// <summary>
        /// PRTransform for the scene.
        /// </summary>
        public PRTransform Transform
        {
            get { return new PRTransform { Quaternion = new Quaternion { W = this.RotationAmount, Vector3D = this.RotationAxis }, Vector3D = this.Position }; }
        }

        /// <summary>
        /// AABB bounds for the scene.
        /// </summary>
        public AABB AABBBounds { get; private set; }

        /// <summary>
        /// AABB bounds for MarketSet.
        /// </summary>
        public AABB AABBMarketSetBounds { get; private set; }

        /// <summary>
        /// NavMesh for the scene.
        /// </summary>
        public Mooege.Common.MPQ.FileFormats.Scene.NavMeshDef NavMesh { get; private set; }

        /// <summary>
        /// Markersets for the scene.
        /// </summary>
        public List<int> MarkerSets { get; private set; }

        /// <summary>
        /// LookLink - not sure on the usage /raist.
        /// </summary>
        public string LookLink { get; private set; }

        /// <summary>
        /// NavZone for the scene.
        /// </summary>
        public Mooege.Common.MPQ.FileFormats.Scene.NavZoneDef NavZone { get; private set; }

        /// <summary>
        /// Creates a new scene and adds it to given world.
        /// </summary>
        /// <param name="world">The parent world.</param>
        /// <param name="position">The position.</param>
        /// <param name="snoId">SNOId for the scene.</param>
        /// <param name="parent">The parent scene if any.</param>
        public Scene(World world, Vector3D position, int snoId, Scene parent)
            : base(world, world.NewSceneID)
        {
            this.SNOId = snoId;
            this.Parent = parent;
            this.Subscenes = new List<Scene>();
            this.Scale = 1.0f;                   
            this.AppliedLabels = new int[0];
            this.LoadSceneData(); // load data from mpqs.

            this.Size = new Size(this.NavZone.V0.X*this.NavZone.Float0, this.NavZone.V0.Y*this.NavZone.Float0);
            this.Position = position;
            this.World.AddScene(this); // add scene to the world.
        }

        #region mpq-data 

        /// <summary>
        /// Loads scene data from mpqs.
        /// </summary>
        private void LoadSceneData()
        {
            var data = MPQStorage.Data.Assets[SNOGroup.Scene][this.SNOId].Data as Mooege.Common.MPQ.FileFormats.Scene;
            if (data == null) return;

            this.AABBBounds = data.AABBBounds;
            this.AABBMarketSetBounds = data.AABBMarketSetBounds;
            this.NavMesh = data.NavMesh;
            this.MarkerSets = data.MarkerSets;
            this.LookLink = data.LookLink;
            this.NavZone = data.NavZone;
        }

        #endregion

        #region update & tick logic

        public override void Update()
        {
            if (!this.HasPlayers) // don't update actors if we have no players in scene.
                return;
            
            foreach(var actor in this.Actors)
            {
                actor.Update();
            }
            
            foreach(var player in this.Players)
            {
                player.Update();
            }
        }

        #endregion

        #region range-queries

        public List<Player> Players
        {
            get { return this.GetObjects<Player>(); }
        }

        public bool HasPlayers
        {
            get { return this.Players.Count > 0; }
        }

        public List<Actor> Actors
        {
            get { return this.GetObjects<Actor>(); }
        }

        public bool HasActors
        {
            get { return this.Actors.Count > 0; }
        }

        public List<T> GetObjects<T>() where T : WorldObject
        {
            return this.World.QuadTree.Query<T>(this.Bounds);
        }

        #endregion

        #region actor-loading

        /// <summary>
        /// Loads all actors for the scene.        
        /// </summary>
        public void LoadActors()
        {
            // TODO: We should be instead loading actors but let them get revealed based on quest/triggers/player proximity. /raist.

            foreach (var markerSet in this.MarkerSets)
            {
                var markerSetData = MPQStorage.Data.Assets[SNOGroup.MarkerSet][markerSet].Data as Mooege.Common.MPQ.FileFormats.MarkerSet;
                if (markerSetData == null) return;

                foreach (var marker in markerSetData.Markers)
                {
                    if (marker.SNOName.Group != SNOGroup.Actor) continue; // skip non-actor markers.
                    
                    var actor = ActorFactory.Create(this.World, marker.SNOName.SNOId, marker.TagMap); // try to create it.
                    if (actor == null) continue;

                    var position = marker.PRTransform.Vector3D + this.Position; // calculate the position for the actor.
                    actor.RotationAmount = marker.PRTransform.Quaternion.W;
                    actor.RotationAxis = marker.PRTransform.Quaternion.Vector3D;

                    actor.EnterWorld(position);
                }
            }
        }

        #endregion

        #region scene revealing & unrevealing 

        /// <summary>
        /// Returns true if the actor is revealed to player.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns><see cref="bool"/></returns>
        public bool IsRevealedToPlayer(Player player)
        {
            return player.RevealedObjects.ContainsKey(this.DynamicID);
        }

        /// <summary>
        /// Reveal the scene to given player.
        /// </summary>
        /// <param name="player">Player to reveal scene.</param>
        /// <returns></returns>
        public override bool Reveal(Player player)
        {
            if (player.RevealedObjects.ContainsKey(this.DynamicID)) return false; // return if already revealed.

            player.InGameClient.SendMessage(this.RevealMessage,true); // reveal the scene itself.
            player.InGameClient.SendMessage(this.MapRevealMessage,true); // reveal the scene in minimap.

            foreach (var sub in this.Subscenes) // reveal subscenes too.
            {
                sub.Reveal(player);
            }

            player.RevealedObjects.Add(this.DynamicID, this);
            return true;
        }

        /// <summary>
        /// Unreveals the scene to given player.
        /// </summary>
        /// <param name="player">Player to unreveal scene.</param>
        /// <returns></returns>
        public override bool Unreveal(Player player)
        {
            if (!player.RevealedObjects.ContainsKey(this.DynamicID)) return false; // if it's not revealed already, just return.

            player.InGameClient.SendMessage(new DestroySceneMessage() { WorldID = this.World.DynamicID, SceneID = this.DynamicID },true); // send the unreveal message.
            
            foreach (var subScene in this.Subscenes) // unreveal subscenes too.
            {
                subScene.Unreveal(player);
            }

            player.RevealedObjects.Remove(this.DynamicID);
            return true;
        }

        #endregion

        #region scene-related messages

        /// <summary>
        /// Returns a RevealSceneMessage.
        /// </summary>
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
                    SceneSNO = this.SNOId,
                    ParentChunkID = this.ParentChunkID,
                    SceneGroupSNO = this.SceneGroupSNO,
                    arAppliedLabels = this.AppliedLabels
                };
            }
        }

        /// <summary>
        /// Returns a MapRevealSceneMessage.
        /// </summary>
        public MapRevealSceneMessage MapRevealMessage
        {
            get
            {
                return new MapRevealSceneMessage
                {
                    ChunkID = this.DynamicID,
                    SceneSNO = this.SNOId,
                    Transform = this.Transform,
                    WorldID = this.World.DynamicID,
                    MiniMapVisibility = this.MiniMapVisibility
                };
            }
        }

        #endregion

        public override string ToString()
        {
            return string.Format("[Scene] SNOId: {0} DynamicId: {1} Position: {2}", this.SNOId, this.DynamicID, this.Position);
        }
    }

    /// <summary>
    /// Minimap visibility of the scene on map.
    /// </summary>
    public enum SceneMiniMapVisibility
    {
        /// <summary>
        /// Hidden.
        /// </summary>
        Hidden = 0,
        /// <summary>
        /// Revealed to player.
        /// </summary>
        Revealed = 1,
        /// <summary>
        /// Player already visited the scene.
        /// </summary>
        Visited = 2
    }
}
