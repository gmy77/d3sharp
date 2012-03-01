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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Core.GS.Actors;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Net.GS.Message.Definitions.Animation;
using Mooege.Net.GS.Message.Fields;
using Mooege.Core.GS.Players;
using Mooege.Core.GS.Common.Types.TagMap;
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Net.GS.Message.Definitions.Player;
using Mooege.Net.GS.Message.Definitions.Trade;

namespace Mooege.Core.GS.Powers.Payloads
{
    public class DeathPayload : Payload
    {
        public DamageType DeathDamageType;

        public DeathPayload(PowerContext context, DamageType deathDamageType, Actor target)
            : base(context, target)
        {
            this.DeathDamageType = deathDamageType;
        }

        public void Apply()
        {
            if (this.Target.World == null) return;

            if (this.Target is Player)
            {
                DoPlayerDeath();
                return;
            }

            // HACK: add to hackish list thats used to defer deleting actor and filter it from powers targetting
            this.Target.World.PowerManager.AddDeletingActor(this.Target);

            // kill brain if monster
            if (this.Target is Monster)
            {
                Monster mon = (Monster)this.Target;
                if (mon.Brain != null)
                    mon.Brain.Kill();
            }

            // send this death payload to buffs
            this.Target.World.BuffManager.SendTargetPayload(this.Target, this);



            // wtf is this?
            this.Target.World.BroadcastIfRevealed(new Mooege.Net.GS.Message.Definitions.Effect.PlayEffectMessage()
            {
                ActorId = this.Target.DynamicID,
                Effect = Mooege.Net.GS.Message.Definitions.Effect.Effect.Unknown12,
            }, this.Target);

            this.Target.World.BroadcastIfRevealed(new ANNDataMessage(Opcodes.ANNDataMessage13)
            {
                ActorID = this.Target.DynamicID
            }, this.Target);

            // play main death animation
            this.Target.PlayAnimation(11, _FindBestDeathAnimationSNO(), 1f, 2);

            this.Target.World.BroadcastIfRevealed(new ANNDataMessage(Opcodes.ANNDataMessage24)
            {
                ActorID = this.Target.DynamicID,
            }, this.Target);

            // remove all buffs and running powers before deleting actor
            this.Target.World.BuffManager.RemoveAllBuffs(this.Target);
            this.Target.World.PowerManager.CancelAllPowers(this.Target);

            this.Target.Attributes[GameAttribute.Deleted_On_Server] = true;
            this.Target.Attributes[GameAttribute.Could_Have_Ragdolled] = true;
            this.Target.Attributes.BroadcastChangedIfRevealed();

            // Spawn Random item and give exp for each player in range
            List<Player> players = this.Target.GetPlayersInRange(26f);
            foreach (Player plr in players)
            {
                plr.UpdateExp(this.Target.Attributes[GameAttribute.Experience_Granted]);
                this.Target.World.SpawnRandomItemDrop(this.Target, plr);
            }

            if (this.Context.User is Player)
            {
                Player player = (Player)this.Context.User;

                player.ExpBonusData.Update(player.GBHandle.Type, this.Target.GBHandle.Type);
                this.Target.World.SpawnGold(this.Target, player);
                if (Mooege.Common.Helpers.Math.RandomHelper.Next(1, 100) < 20)
                    this.Target.World.SpawnHealthGlobe(this.Target, player, this.Target.Position);
            }

            if (this.Target is Monster)
                (this.Target as Monster).PlayLore();

            // HACK: instead of deleting actor right here, its added to a list (near the top of this function)
            //this.Target.Destroy();
        }

        private void DoPlayerDeath()
        {
            this.Target.PlayEffect(Net.GS.Message.Definitions.Effect.Effect.Unknown38);
            //not sure why count does not appear
            this.Target.Attributes[GameAttribute.Death_Count] += 1;

            //TODO: there seems to be a special trade message on death, maybe blocking trades with user...
            //TradeMessage:
            //{
            // Field0: 0x00000001 (1)
            // Field1: 0x789F005A (2023686234) ActorId
            // Field2: 0x44070500 (1141310720)
            // Field3: 0x00002AAB98C93000   First8: WorldId this.Target.World.DynamicID;
            // Field4: 0x98C93328 (-1731644632)
            // Field5:
            // {
            //  0x00002AAB, 0x44070530, 0x00000000, 0x44070500, 0x00000000, 
            // }

            //}

            //TradeMessage:
            //{
            // Field0: 0x00000001 (1)
            // Field1: 0x78AF004F (2024734799) ActorId
            // Field2: 0x45E73500 (1172780288)
            // Field3: 0x00002AABBB4BC000
            // Field4: 0xBB4BC490 (-1152662384)
            // Field5:
            // {
            //  0x00002AAB, 0x45E73530, 0x00000000, 0x45E73500, 0x00000000, 
            // }

            //}

            this.Target.Attributes[GameAttribute.Buff_Icon_End_Tick0, 0x00020C51] = 0;
            this.Target.Attributes[GameAttribute.Buff_Icon_End_Tick0, 0x00020CBE] = 0;
            this.Target.Attributes[GameAttribute.Buff_Icon_Count0, 0x00020CBE] = 0;
            this.Target.Attributes[GameAttribute.Buff_Active, 0x00020CBE] = false;
            this.Target.Attributes[GameAttribute.Buff_Icon_Start_Tick0, 0x00020CBE] = 0;
            this.Target.Attributes[GameAttribute.Buff_Icon_Start_Tick0, 0x00020C51] = 0;
            this.Target.Attributes[GameAttribute.Buff_Icon_Count0, 0x00020C51] = 0;
            this.Target.Attributes[GameAttribute.Banter_Cooldown, 0x000FFFFF] = -1;
            this.Target.Attributes[GameAttribute.Buff_Active, 0x00020C51] = false;

            ////[07.01.2012 09:25:27.103] [ Dump] [PacketReader]: [O] GameMessage(0x0073)
            ////ANNDataMessage:
            ////{
            //// ActorID: 0x78AF004F (2024734799)
            ////}

            this.Target.Attributes[GameAttribute.Look_Override] = 0x0782CAC5;
            this.Target.Attributes[GameAttribute.Buff_Icon_Count0, 0x0002F39E] = 1;
            this.Target.Attributes[GameAttribute.Buff_Active, 0x0002F39E] = true;
            this.Target.Attributes[GameAttribute.Buff_Icon_End_Tick0, 0x00033C40] = 0x00001733;
            this.Target.Attributes[GameAttribute.Resource_Cur, 0x00000003] = 150;
            this.Target.Attributes[GameAttribute.Buff_Icon_Start_Tick0, 0x00036D7F] = 0x000013AF;
            this.Target.Attributes[GameAttribute.Immobolize] = true;
            this.Target.Attributes[GameAttribute.Untargetable] = true;
            this.Target.Attributes[GameAttribute.Stealthed] = true;
            this.Target.Attributes[GameAttribute.Buff_Icon_Count0, 0x00036D7F] = 1;
            this.Target.Attributes[GameAttribute.Hitpoints_Cur] = 120;
            this.Target.Attributes[GameAttribute.Buff_Active, 0x00036D7F] = true;
            this.Target.Attributes[GameAttribute.Attacks_Per_Second_Percent] = 0.1499023F;
            this.Target.Attributes[GameAttribute.Buff_Icon_End_Tick0, 0x00036D7F] = 0x0000100B;
            this.Target.Attributes[GameAttribute.Attacks_Per_Second_Total] = 1.379883F;
            this.Target.Attributes[GameAttribute.Hidden] = true;
            this.Target.Attributes[GameAttribute.Loading] = true;
            this.Target.Attributes[GameAttribute.Invulnerable] = true;
            this.Target.Attributes[GameAttribute.Projectile_Speed] = 3.051758E-05F; //probably min speed
            this.Target.Attributes[GameAttribute.Headstone_Player_ANN] = -1;
            this.Target.Attributes[GameAttribute.Buff_Icon_Start_Tick0, 0x00020C51] = 0x00000905;
            this.Target.Attributes[GameAttribute.Buff_Icon_Count0, 0x0000CE11] = 1;
            this.Target.Attributes[GameAttribute.Buff_Icon_Start_Tick0, 0x00033C40] = 0x00000F57; //different value 
            this.Target.Attributes[GameAttribute.Buff_Active, 0x0000CE11] = true;
            this.Target.Attributes[GameAttribute.Power_Buff_0_Visual_Effect_None, 0x00036D7F] = true;
            this.Target.Attributes[GameAttribute.Buff_Icon_Count0, 0x00020C51] = 1;
            this.Target.Attributes[GameAttribute.Buff_Active, 0x00033C40] = true;
            this.Target.Attributes[GameAttribute.Buff_Icon_Count0, 0x00033C40] = 1;
            this.Target.Attributes[GameAttribute.CantStartDisplayedPowers] = true;
            this.Target.PlayEffect(Net.GS.Message.Definitions.Effect.Effect.Unknown22);
            this.Target.Attributes.BroadcastChangedIfRevealed();

            //TODO: There should be a 1-2 sec wait timer here so client shows the nice eeffect

            //move user to new position
            //TODO: Find last waypoint
            var lastWaypoint = new Common.Types.Math.Vector3D();
            lastWaypoint.X = 3143.75F;
            lastWaypoint.Y = 2828.75F;
            lastWaypoint.Z = 59.17559F;
            var deathWorld = new ACDWorldPositionMessage();
            deathWorld.ActorID = this.Target.DynamicID;
            deathWorld.WorldLocation = new WorldLocationMessageData();
            deathWorld.WorldLocation.Scale = 1.43F;
            deathWorld.WorldLocation.Transform = new Common.Types.Math.PRTransform();
            deathWorld.WorldLocation.Transform.Quaternion = new Common.Types.Math.Quaternion();
            deathWorld.WorldLocation.Transform.Quaternion.W = 0.05940545F;
            deathWorld.WorldLocation.Transform.Quaternion.Vector3D = new Common.Types.Math.Vector3D();
            deathWorld.WorldLocation.Transform.Quaternion.Vector3D.X = 0;
            deathWorld.WorldLocation.Transform.Quaternion.Vector3D.Y = 0;
            deathWorld.WorldLocation.Transform.Quaternion.Vector3D.Z = 0.9982339F;
            deathWorld.WorldLocation.Transform.Vector3D = lastWaypoint;
            deathWorld.WorldLocation.WorldID = this.Target.World.DynamicID;
            this.Target.World.BroadcastIfRevealed(deathWorld, this.Target);

            var snappedMessage = new ACDTranslateSnappedMessage();
            snappedMessage.Field0 = (int)this.Target.DynamicID;
            snappedMessage.Field1 = lastWaypoint;
            snappedMessage.Field2 = 3.022712F;
            snappedMessage.Field3 = true;
            snappedMessage.Field4 = 0x00000900; //constant in death
            this.Target.World.BroadcastIfRevealed(snappedMessage, this.Target);

            var playerWarpedMessage = new PlayerWarpedMessage();
            playerWarpedMessage.Field0 = 0x00000000; //sometimes 5
            playerWarpedMessage.Field1 = 0;

            this.Target.Attributes[GameAttribute.Buff_Visual_Effect, 0x000FFFFF] = true;
            this.Target.Attributes[GameAttribute.Hitpoints_Max_Total] = 80;
            this.Target.Attributes[GameAttribute.Hitpoints_Max] = 80;
            this.Target.Attributes[GameAttribute.Hitpoints_Healed_Target] = this.Target.Attributes[GameAttribute.Hitpoints_Max_Total]; //heal up -should be w/e needs to be
            ((Player)this.Target).AddPercentageHP(100); //heal up -should be w/e needs to be

            //start reverting buffs
            this.Target.Attributes[GameAttribute.Buff_Icon_End_Tick0, 0x00033C40] = 0;
            this.Target.Attributes[GameAttribute.Immobolize] = false;
            this.Target.Attributes[GameAttribute.Hidden] = false;
            this.Target.Attributes[GameAttribute.Loading] = false;
            this.Target.Attributes[GameAttribute.Buff_Icon_Start_Tick0, 0x00033C40] = 0;
            this.Target.Attributes[GameAttribute.Buff_Active, 0x00033C40] = false;
            this.Target.Attributes[GameAttribute.Buff_Icon_Count0, 0x00033C40] = 0;
            this.Target.Attributes[GameAttribute.Look_Override] = 0;
            this.Target.Attributes[GameAttribute.Buff_Icon_Start_Tick0, 0x00036D7F] = 0;
            this.Target.Attributes[GameAttribute.Untargetable] = false;
            this.Target.Attributes[GameAttribute.Stealthed] = false;
            this.Target.Attributes[GameAttribute.Buff_Icon_Count0, 0x00036D7F] = 0;
            this.Target.Attributes[GameAttribute.Buff_Active, 0x00036D7F] = false;
            this.Target.Attributes[GameAttribute.Buff_Icon_End_Tick0, 0x00036D7F] = 0;
            this.Target.Attributes[GameAttribute.Invulnerable] = false;
            this.Target.Attributes[GameAttribute.Power_Buff_0_Visual_Effect_None, 0x00036D7F] = false;
            this.Target.Attributes[GameAttribute.CantStartDisplayedPowers] = false;
            this.Target.Attributes.BroadcastChangedIfRevealed();

            // Update player.Position otherwise server treats you as your at old loc till you take a step - DarkLotus
            this.Target.Position = lastWaypoint;

            return;
        }

        private int _FindBestDeathAnimationSNO()
        {
            // check if power has special death animation, and roll chance to use it
            TagKeyInt specialDeathTag = _GetTagForSpecialDeath(this.Context.EvalTag(PowerKeys.SpecialDeathType));
            if (specialDeathTag != null)
            {
                float specialDeathChance = this.Context.EvalTag(PowerKeys.SpecialDeathChance);
                if (PowerContext.Rand.NextDouble() < specialDeathChance)
                {
                    int specialSNO = _GetSNOFromTag(specialDeathTag);
                    if (specialSNO != -1)
                        return specialSNO;
                }
                // decided not to use special death or actor doesn't have it, just fall back to normal death anis
            }

            int sno = _GetSNOFromTag(this.DeathDamageType.DeathAnimationTag);
            if (sno != -1)
                return sno;

            // load default ani if all else fails
            return _GetSNOFromTag(AnimationSetKeys.DeathDefault);
        }

        private int _GetSNOFromTag(TagKeyInt tag)
        {
            if (this.Target.AnimationSet != null && this.Target.AnimationSet.TagMapAnimDefault.ContainsKey(tag))
                return this.Target.AnimationSet.TagMapAnimDefault[tag];
            else
                return -1;
        }

        private static TagKeyInt _GetTagForSpecialDeath(int specialDeathType)
        {
            switch (specialDeathType)
            {
                default: return null;
                case 1: return AnimationSetKeys.DeathDisintegration;
                case 2: return AnimationSetKeys.DeathPulverise;
                case 3: return AnimationSetKeys.DeathPlague;
                case 4: return AnimationSetKeys.DeathDismember;
                case 5: return AnimationSetKeys.DeathDecap;
                case 6: return AnimationSetKeys.DeathAcid;
                case 7: return AnimationSetKeys.DeathLava;  // haven't seen lava used, but there's no other place for it
                case 8: return AnimationSetKeys.DeathSpirit;
            }
        }
    }
}