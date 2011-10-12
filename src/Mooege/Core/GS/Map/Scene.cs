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
using Mooege.Core.GS.Game;
using Mooege.Core.GS.Objects;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message.Definitions.Map;
using Mooege.Net.GS.Message.Definitions.Scene;

// NOTE: Scenes are typically (always, hopefully) 240x240 in size. Cells are 60x60 with
//  subscenes able to be positioned relative to their parents

namespace Mooege.Core.GS.Map
{
    public sealed class Scene : WorldObject
    {
        public override World World
        {
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
                    SceneSpec = this.SceneSpec,
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

        public SceneSpecification SceneSpec;
        public int SceneSNO;
        public Scene Parent;
        public int SceneGroupSNO;
        public int /* gbid */[] AppliedLabels; // MaxLength = 256
        public int MiniMapVisibility;

        public readonly List<Scene> Subscenes;

        public PRTransform Transform
        {
            get { return new PRTransform { Rotation = new Quaternion { Amount = this.RotationAmount, Axis = this.RotationAxis }, ReferencePoint = this.Position }; }
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
        }

        public override bool Reveal(Mooege.Core.GS.Player.Player player)
        {
            if (player.RevealedObjects.ContainsKey(this.DynamicID)) return false; // already revealed
            player.RevealedObjects.Add(this.DynamicID, this);
            player.InGameClient.SendMessage(this.RevealMessage);
            player.InGameClient.SendMessage(this.MapRevealMessage);
            foreach (var sub in this.Subscenes)
            {
                sub.Reveal(player);
            }
            player.InGameClient.FlushOutgoingBuffer();
            return true;
        }

        public override bool Unreveal(Mooege.Core.GS.Player.Player player)
        {
            if (!player.RevealedObjects.ContainsKey(this.DynamicID)) return false; // not revealed yet
            player.InGameClient.SendMessage(new DestroySceneMessage() { WorldID = this.World.DynamicID, SceneID = this.DynamicID });
            player.InGameClient.FlushOutgoingBuffer();
            foreach (var sub in this.Subscenes)
            {
                sub.Unreveal(player);
            }
            player.RevealedObjects.Remove(this.DynamicID);
            return true;
        }
    }
}
