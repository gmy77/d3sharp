/*
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

using System;
using Mooege.Common.Helpers;
using Mooege.Core.GS.Map;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Tick;
using Mooege.Net.GS.Message.Definitions.World;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message.Definitions.Animation;
using Mooege.Net.GS.Message.Definitions.Effect;
using Mooege.Net.GS.Message.Definitions.Misc;

namespace Mooege.Core.GS.Actors
{
    public class Monster : Actor
    {
        public override ActorType ActorType { get { return ActorType.Monster; } }

        // TODO: Setter needs to update world. Also, this is probably an ACD field. /komiga
        public int AnimationSNO { get; set; }

        public Monster(World world, int actorSNO, Vector3D position)
            : base(world, world.NewActorID)
        {
            this.ActorSNO = actorSNO;
            // FIXME: This is hardcoded crap
            this.Field2 = 0x8;
            this.Field3 = 0x0;
            this.Scale = 1.35f;
            this.Position.Set(position);
            this.RotationAmount = (float)(RandomHelper.NextDouble() * 2.0f * Math.PI);
            this.RotationAxis.X = 0f; this.RotationAxis.Y = 0f; this.RotationAxis.Z = 1f;
            this.GBHandle.Type = (int)GBHandleType.Monster; this.GBHandle.GBID = 1;
            this.Field7 = 0x00000001;
            this.Field8 = this.ActorSNO;
            this.Field10 = 0x0;
            this.Field11 = 0x0;
            this.Field12 = 0x0;
            this.Field13 = 0x0;
            this.AnimationSNO = 0x11150;

            this.Attributes[GameAttribute.Untargetable] = false;
            this.Attributes[GameAttribute.Uninterruptible] = true;
            this.Attributes[GameAttribute.Buff_Visual_Effect, 1048575] = true;
            this.Attributes[GameAttribute.Buff_Icon_Count0, 30582] = 1;
            this.Attributes[GameAttribute.Buff_Icon_Count0, 30286] = 1;
            this.Attributes[GameAttribute.Buff_Icon_Count0, 30285] = 1;
            this.Attributes[GameAttribute.Buff_Icon_Count0, 30284] = 1;
            this.Attributes[GameAttribute.Buff_Icon_Count0, 30283] = 1;
            this.Attributes[GameAttribute.Buff_Icon_Count0, 30290] = 1;
            this.Attributes[GameAttribute.Buff_Icon_Count0, 79486] = 1;
            this.Attributes[GameAttribute.Buff_Active, 30286] = true;
            this.Attributes[GameAttribute.Buff_Active, 30285] = true;
            this.Attributes[GameAttribute.Buff_Active, 30284] = true;
            this.Attributes[GameAttribute.Buff_Active, 30283] = true;
            this.Attributes[GameAttribute.Buff_Active, 30290] = true;

            this.Attributes[GameAttribute.Hitpoints_Max_Total] = 4.546875f;
            this.Attributes[GameAttribute.Buff_Active, 79486] = true;
            this.Attributes[GameAttribute.Hitpoints_Max] = 4.546875f;
            this.Attributes[GameAttribute.Hitpoints_Total_From_Level] = 0f;
            this.Attributes[GameAttribute.Hitpoints_Cur] = 4.546875f;
            this.Attributes[GameAttribute.Invulnerable] = true;
            this.Attributes[GameAttribute.Buff_Active, 30582] = true;
            this.Attributes[GameAttribute.TeamID] = 10;
            this.Attributes[GameAttribute.Level] = 1;
            this.Attributes[GameAttribute.Experience_Granted] = 125;

            this.World.Enter(this); // Enter only once all fields have been initialized to prevent a run condition
        }

        public override void Update()
        {
            this.Brain(); // let him think. /raist 
        }

        public virtual void Brain()
        {
            // intellectual activities goes here ;) /raist
        }

        public override void OnTargeted(Mooege.Core.GS.Player.Player player, TargetMessage message)
        {
            this.Die(player);
        }

        public override bool Reveal(Mooege.Core.GS.Player.Player player)
        {
            if (!base.Reveal(player))
                return false;

            /* Dont know what this does
            player.InGameClient.SendMessage(new ANNDataMessage(Opcodes.ANNDataMessage24)
            {
                ActorID = this.DynamicID
            });
            */

            player.InGameClient.SendMessage(new SetIdleAnimationMessage
            {
                ActorID = this.DynamicID,
                AnimationSNO = this.AnimationSNO
            });

            player.InGameClient.SendMessage(new EndOfTickMessage()
            {
                Field0 = player.InGameClient.Game.Tick,
                Field1 = player.InGameClient.Game.Tick + 20
            });

            return true;
        }

        // FIXME: Hardcoded hell. /komiga
        public void Die(Mooege.Core.GS.Player.Player player)
        {
            var killAni = new int[]{
                    0x2cd7,
                    0x2cd4,
                    0x01b378,
                    0x2cdc,
                    0x02f2,
                    0x2ccf,
                    0x2cd0,
                    0x2cd1,
                    0x2cd2,
                    0x2cd3,
                    0x2cd5,
                    0x01b144,
                    0x2cd6,
                    0x2cd8,
                    0x2cda,
                    0x2cd9
            };

            player.UpdateExp(this.Attributes[GameAttribute.Experience_Granted]);
            player.UpdateExpBonusData(player.GBHandle.Type, this.GBHandle.Type);

            this.World.BroadcastIfRevealed(new PlayEffectMessage()
            {
                ActorID = this.DynamicID,
                Field1 = 0x0,
                Field2 = 0x2,
            }, this);
            this.World.BroadcastIfRevealed(new PlayEffectMessage()
            {
                ActorID = this.DynamicID,
                Field1 = 0xc,
            }, this);
            this.World.BroadcastIfRevealed(new PlayHitEffectMessage()
            {
                ActorID = this.DynamicID,
                HitDealer = player.DynamicID,
                Field2 = 0x2,
                Field3 = false,
            }, this);

            this.World.BroadcastIfRevealed(new FloatingNumberMessage()
            {
                ActorID = this.DynamicID,
                Number = 9001.0f,
                Type = FloatingNumberMessage.FloatType.White,
            }, this);

            this.World.BroadcastIfRevealed(new ANNDataMessage(Opcodes.ANNDataMessage13)
            {
                ActorID = this.DynamicID
            }, this);

            this.World.BroadcastIfRevealed(new PlayAnimationMessage()
            {
                ActorID = this.DynamicID,
                Field1 = 0xb,
                Field2 = 0,
                tAnim = new PlayAnimationMessageSpec[1]
                {
                    new PlayAnimationMessageSpec()
                    {
                        Field0 = 0x2,
                        Field1 = killAni[RandomHelper.Next(killAni.Length)],
                        Field2 = 0x0,
                        Field3 = 1f
                    }
                }
            }, this);

            this.World.BroadcastIfRevealed(new ANNDataMessage(Opcodes.ANNDataMessage24)
            {
                ActorID = this.DynamicID,
            }, this);

            GameAttributeMap attribs = new GameAttributeMap();
            attribs[GameAttribute.Hitpoints_Cur] = 0f;
            attribs[GameAttribute.Could_Have_Ragdolled] = true;
            attribs[GameAttribute.Deleted_On_Server] = true;

            foreach (var msg in attribs.GetMessageList(this.DynamicID))
                this.World.BroadcastIfRevealed(msg, this);

            this.World.BroadcastIfRevealed(new PlayEffectMessage()
            {
                ActorID = this.DynamicID,
                Field1 = 0xc,
            }, this);
            this.World.BroadcastIfRevealed(new PlayEffectMessage()
            {
                ActorID = this.DynamicID,
                Field1 = 0x37,
            }, this);
            this.World.BroadcastIfRevealed(new PlayHitEffectMessage()
            {
                ActorID = this.DynamicID,
                HitDealer = player.DynamicID,
                Field2 = 0x2,
                Field3 = false,
            }, this);

            this.World.SpawnRandomDrop(player, this.Position);
            this.World.SpawnGold(player, this.Position);
            int rGlobes = RandomHelper.Next(1, 100);
            if (rGlobes < 20)
                this.World.SpawnGlobe(player, this.Position);
            this.Destroy();
        }
    }
}
