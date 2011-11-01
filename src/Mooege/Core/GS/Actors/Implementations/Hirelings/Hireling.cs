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

using System.Collections.Generic;
using Mooege.Common.MPQ.FileFormats.Types;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Players;
using Mooege.Net.GS.Message;
using Mooege.Common.Helpers;
using Mooege.Net.GS.Message.Definitions.World;
using Mooege.Core.GS.Actors.Interactions;

namespace Mooege.Core.GS.Actors.Implementations.Hirelings
{
    public class Hireling : InteractiveNPC
    {
        protected int hirelingSNO = -1;

        public Hireling(World world, int actorSNO, Vector3D position, Dictionary<int, TagMapEntry> tags)
            : base(world, actorSNO, position, tags)
        {
            this.Attributes[GameAttribute.TeamID] = 2;
            Interactions.Add(new HireInteraction());
            Interactions.Add(new InventoryInteraction());
        }

        public override void OnHire(Player player)
        {
            if (hirelingSNO == -1)
                return;

            this.Unreveal(player);
            var tmp = new Templar(this.World, hirelingSNO, this.Position, this.Tags);

            //tmp.GBHandle.Type = 4;
            //tmp.GBHandle.GBID = StringHashHelper.HashItemName("Templar");
            tmp.Field9 = 5;
            tmp.Field7 = 1;
            tmp.Attributes[GameAttribute.Pet_Creator] = player.PlayerIndex;
            tmp.Attributes[GameAttribute.Pet_Type] = 0;
            tmp.Attributes[GameAttribute.Pet_Owner] = player.PlayerIndex;

            tmp.RotationAmount = this.RotationAmount;
            tmp.RotationAxis = this.RotationAxis;
            tmp.World.Enter(tmp);
            tmp.Reveal(player);
            player.Attributes[GameAttribute.Hireling_Class] = tmp.Attributes[GameAttribute.Hireling_Class];
            player.Attributes.SendChangedMessage(player.InGameClient, player.DynamicID);

            player.InGameClient.SendMessage(new Mooege.Net.GS.Message.Definitions.Player.PetMessage()
            {
                Id = (int)Opcodes.PetMessage,
                Field2 = (int)tmp.DynamicID,
            });

            this.Destroy();
            player.SelectedNPC = null;
            
        }
    }
}
