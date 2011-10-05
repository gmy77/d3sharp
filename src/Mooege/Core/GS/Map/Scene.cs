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

using Mooege.Core.GS.Game;
using Mooege.Core.GS.Objects;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message.Definitions.Map;
using Mooege.Net.GS.Message.Definitions.Scene;

namespace Mooege.Core.GS.Map
{
    sealed public class Scene : IDynamicObject, IWorldObject
    {
        public Mooege.Core.GS.Game.Game Game { get; private set; }
        public uint DynamicID { get; private set; }

        public World World { get; set; }

        public float Scale { get; set; }
        public float RotationAmount { get; set; }
        public Vector3D RotationAxis { get; set; }
        public Vector3D Position { get; set; }

        // TODO: Put message fields into this class
        public MapRevealSceneMessage Map;
        public RevealSceneMessage SceneData;

        public int MiniMapVisibility;

        public int SceneSNO;
        public uint ParentChunkID;
        public int SceneGroupSNO;
        public int /* gbid */[] arAppliedLabels; // MaxLength = 256

        public PRTransform Transform
        {
            get { return new PRTransform { Rotation = new Quaternion { Amount = this.RotationAmount, Axis = this.RotationAxis }, ReferencePoint = this.Position }; }
        }

        public Scene(World world)
        {
            this.Game = world.Game;
            this.DynamicID = world.Game.NewSceneID;
            this.World = world;
            this.World.AddScene(this);

            this.Scale = 1.0f;
            this.RotationAmount = 0.0f;
            this.RotationAxis = new Vector3D();
            this.Position = new Vector3D();
        }

        public void Reveal(Player player)
        {
            if (player.RevealedScenes.ContainsKey(this.DynamicID)) return; //already revealed
            if (SceneData != null)
            {
                player.InGameClient.SendMessage(SceneData);
                player.RevealedScenes.Add(this.DynamicID, this);
            }
            if (Map != null) player.InGameClient.SendMessage(Map);
            player.InGameClient.FlushOutgoingBuffer();
        }

        public void Destroy(Player player)
        {
            if (!player.RevealedScenes.ContainsKey(this.DynamicID)) return; //not revealed yet
            if (SceneData != null)
            {
                player.InGameClient.SendMessage(new DestroySceneMessage() { WorldID = this.World.DynamicID, SceneID = this.DynamicID });
                player.InGameClient.FlushOutgoingBuffer();
                player.RevealedScenes.Remove(this.DynamicID);
            }
        }
    }
}
