/*
 * Copyright (C) 2011 - 2012 mooege project - http://www.mooege.org
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
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Common.Types.TagMap;
using Mooege.Net.GS.Message.Definitions.Animation;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Misc;
using TreasureClass = Mooege.Common.MPQ.FileFormats.TreasureClass;

namespace Mooege.Core.GS.Actors.Implementations
{
    /// <summary>
    /// Class that implements behaviour for killable gizmos.
    /// Play die animation on click, then set idle animation, drop loot and remove from server
    /// </summary>
    class DesctructibleLootContainer : Gizmo
    {
        private TreasureClass TreasureClass;

        public DesctructibleLootContainer(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            if (ActorData.TagMap.ContainsKey(ActorKeys.LootTreasureClass))
                TreasureClass = (TreasureClass)ActorData.TagMap[ActorKeys.LootTreasureClass].Target;
        }

        public void ReceiveDamage(Actor source, float damage /* critical, type */)
        {
            World.BroadcastIfRevealed(new FloatingNumberMessage
            {
                Number = damage,
                ActorID = this.DynamicID,
                Type = FloatingNumberMessage.FloatType.White
            }, this);


            Attributes[GameAttribute.Hitpoints_Cur] = Math.Max(Attributes[GameAttribute.Hitpoints_Cur] - damage, 0);
            Attributes[GameAttribute.Last_Damage_ACD] = unchecked((int)source.DynamicID);

            Attributes.BroadcastChangedIfRevealed();

            if (Attributes[GameAttribute.Hitpoints_Cur] == 0)
            {
                Die();
            }
        }

        public void Die()
        {
            if (this.TreasureClass != null)
                foreach (var player in this.GetPlayersInRange(30))
                    World.DropItem(this, null, this.TreasureClass.CreateDrop(player));

            World.BroadcastIfRevealed(new PlayAnimationMessage
            {
                ActorID = this.DynamicID,
                Field1 = 11,
                Field2 = 0,
                tAnim = new Net.GS.Message.Fields.PlayAnimationMessageSpec[]
                {
                    new Net.GS.Message.Fields.PlayAnimationMessageSpec()
                    {
                        Duration = 10,
                        AnimationSNO = AnimationSet.TagMapAnimDefault[AnimationSetKeys.DeathDefault],
                        PermutationIndex = 0,
                        Speed = 1
                    }
                }

            }, this);

            this.Attributes[GameAttribute.Deleted_On_Server] = true;
            this.Attributes[GameAttribute.Could_Have_Ragdolled] = true;
            Attributes.BroadcastChangedIfRevealed();
            this.Destroy();
        }


        public override void OnTargeted(Players.Player player, Net.GS.Message.Definitions.World.TargetMessage message)
        {
            ReceiveDamage(player, 100);
        }
    }
}
