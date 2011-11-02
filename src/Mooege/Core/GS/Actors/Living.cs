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
using System.Collections.Generic;
using Mooege.Common.Helpers;
using Mooege.Common.MPQ.FileFormats.Types;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Players;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Animation;

namespace Mooege.Core.GS.Actors
{
    public class Living : Actor
    {
        public override ActorType ActorType { get { return ActorType.Monster; } }

        // TODO: Setter needs to update world. Also, this is probably an ACD field. /komiga
        // TODO: not only Living have animations, put this in Actor? /fasbat
        //public int AnimationSNO { get; set; }
        private int snoAnimSet;
        private int snoMonster;
        public Mooege.Common.MPQ.FileFormats.AnimSet Animset = null;

        public Living(World world, int snoId, Dictionary<int, TagMapEntry> tags)
            : base(world, snoId, tags)
        {
            this.SNOId = snoId;
            var actor = (Mooege.Common.MPQ.FileFormats.Actor)Mooege.Common.MPQ.MPQStorage.Data.Assets[Common.Types.SNO.SNOGroup.Actor][snoId].Data;
            this.snoMonster = actor.MonsterSNO;
            if (actor.AnimSetSNO != -1)
            {
                this.Animset = (Mooege.Common.MPQ.FileFormats.AnimSet)Mooege.Common.MPQ.MPQStorage.Data.Assets[Common.Types.SNO.SNOGroup.AnimSet][actor.AnimSetSNO].Data;
            }
            // FIXME: This is hardcoded crap
            this.Field3 = 0x0;
            this.RotationAmount = (float)(RandomHelper.NextDouble() * 2.0f * Math.PI);
            this.RotationAxis.X = 0f; this.RotationAxis.Y = 0f; this.RotationAxis.Z = 1f;
            this.GBHandle.Type = -1; this.GBHandle.GBID = -1;
            this.Field7 = 0x00000001;
            this.Field8 = this.SNOId;
            this.Field10 = 0x0;
            this.Field11 = 0x0;
            this.Field12 = 0x0;
            this.Field13 = 0x0;
            //this.AnimationSNO = 0x11150;
            this.CollFlags = 1;

            this.Attributes[GameAttribute.Hitpoints_Max_Total] = 4.546875f;
            this.Attributes[GameAttribute.Hitpoints_Max] = 4.546875f;
            this.Attributes[GameAttribute.Hitpoints_Total_From_Level] = 0f;
            this.Attributes[GameAttribute.Hitpoints_Cur] = 4.546875f;

            this.Attributes[GameAttribute.Level] = 1;
        }

        public override void Update()
        {
            this.Brain(); // let him think. /raist 
        }

        public virtual void Brain()
        {
            // intellectual activities goes here ;) /raist
        }

        public override bool Reveal(Player player)
        {
            if (!base.Reveal(player))
                return false;
            if (Animset != null)
            {
                if (this.Animset.GetAnimationTag(Mooege.Common.MPQ.FileFormats.AnimationTags.Idle) != -1)
                {
                    player.InGameClient.SendMessage(new SetIdleAnimationMessage
                    {
                        ActorID = this.DynamicID,
                        AnimationSNO = this.Animset.GetAnimationTag(Mooege.Common.MPQ.FileFormats.AnimationTags.Idle)
                    });
                }
               
            }
            return true;
        }
    }
}
