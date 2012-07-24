﻿/*
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
using TreasureClass = Mooege.Common.MPQ.FileFormats.TreasureClass;

namespace Mooege.Core.GS.Actors.Implementations
{
    /// <summary>
    /// Class that implements behaviour for clickable loot types.
    /// Play open animation on click, then set idle animation and drop loot
    /// </summary>
    class LootContainer : Gizmo
    {
        private TreasureClass _treasureClass;

        public LootContainer(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            try
            {
                _treasureClass = (TreasureClass)ActorData.TagMap[ActorKeys.LootTreasureClass].Target;
            }
            catch (Exception)
            {
                Logger.Warn("Could not load treasure class for loot actor because it is either not tagged or the treasure class is not available. Using standard treasure class instead");
                _treasureClass = TreasureClass.GenericTreasure;
            }
        }


        public override void OnTargeted(Players.Player player, Net.GS.Message.Definitions.World.TargetMessage message)
        {
            // TODO Are chests dropping loot for everyone or only for the one opening it
            foreach (var p in this.GetPlayersInRange(30))
                World.DropItem(this, null, _treasureClass.CreateDrop(player));

            World.BroadcastIfRevealed(new PlayAnimationMessage
            {
                ActorID = this.DynamicID,
                Field1 = 5,
                Field2 = 0,
                tAnim = new Net.GS.Message.Fields.PlayAnimationMessageSpec[]
                {
                    new Net.GS.Message.Fields.PlayAnimationMessageSpec()
                    {
                        Duration = 50,
                        AnimationSNO = AnimationSet.TagMapAnimDefault[AnimationSetKeys.Opening],
                        PermutationIndex = 0,
                        Speed = 1
                    }
                }

            }, this);

            World.BroadcastIfRevealed(new SetIdleAnimationMessage
            {
                ActorID = this.DynamicID,
                AnimationSNO = AnimationSetKeys.Open.ID
            }, this);

            this.Attributes[GameAttribute.Gizmo_Has_Been_Operated] = true;
            this.Attributes[GameAttribute.Gizmo_Operator_ACDID] = unchecked((int)player.DynamicID);
            this.Attributes[GameAttribute.Chest_Open, 0xFFFFFF] = true;
            Attributes.BroadcastChangedIfRevealed();

            base.OnTargeted(player, message);
        }
    }
}
