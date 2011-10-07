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

        // TODO: Put message fields into this class
        public MapRevealSceneMessage Map;
        public RevealSceneMessage SceneData;

        public int MiniMapVisibility;

        public readonly IVector2D Cell;
        public int SceneSNO;
        public uint ParentChunkID;
        public int SceneGroupSNO;
        public int /* gbid */[] AppliedLabels; // MaxLength = 256

        public readonly List<Scene> Subscenes;

        public PRTransform Transform
        {
            get { return new PRTransform { Rotation = new Quaternion { Amount = this.RotationAmount, Axis = this.RotationAxis }, ReferencePoint = this.Position }; }
        }

        public Scene(World world)
            : base(world, world.NewSceneID)
        {
            this.Scale = 1.0f;
            this.RotationAmount = 0.0f;
            this.Subscenes = new List<Scene>();

            this.World.AddScene(this);
        }

        public override void Reveal(Mooege.Core.GS.Player.Player player)
        {
            if (player.RevealedObjects.ContainsKey(this.DynamicID)) return; //already revealed
            if (SceneData != null)
            {
                player.InGameClient.SendMessage(SceneData);
                player.RevealedObjects.Add(this.DynamicID, this);
            }
            if (Map != null) player.InGameClient.SendMessage(Map);
            player.InGameClient.FlushOutgoingBuffer();
        }

        public override void Unreveal(Mooege.Core.GS.Player.Player player)
        {
            if (!player.RevealedObjects.ContainsKey(this.DynamicID)) return; //not revealed yet
            if (SceneData != null)
            {
                player.InGameClient.SendMessage(new DestroySceneMessage() { WorldID = this.World.DynamicID, SceneID = this.DynamicID });
                player.InGameClient.FlushOutgoingBuffer();
                player.RevealedObjects.Remove(this.DynamicID);
            }
        }
    }
}
