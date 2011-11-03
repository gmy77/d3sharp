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
using Mooege.Net.GS.Message.Definitions.Inventory;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message.Definitions.Hireling;

namespace Mooege.Core.GS.Actors.Implementations.Hirelings
{
    public class Hireling : InteractiveNPC
    {
        protected int hirelingSNO = -1;
        protected int proxySNO = -1;

        private Hireling proxyActor = null;

        protected Player _owner = null;

        public Hireling(World world, int snoId, Dictionary<int, TagMapEntry> tags)
            : base(world, snoId, tags)
        {
            this.Attributes[GameAttribute.TeamID] = 2;
            Interactions.Add(new HireInteraction());
            Interactions.Add(new InventoryInteraction());
        }

        public override void OnTargeted(Player player, TargetMessage message)
        {
            base.OnTargeted(player, message);

          /*  if (proxyActor != null)
                return;

            if (this.SNOId != proxySNO && this.SNOId != hirelingSNO)
            {
                proxyActor = new Templar(World, proxySNO, this.Tags);
                proxyActor.EnterWorld(this.Position);
            } */
        }

        public override void OnHire(Player player)
        {
            if (hirelingSNO == -1)
                return;

            //this doesn't fully work, and is for now disabled. /fasbat
            this.Unreveal(player);
            var tmp = new Templar(this.World, hirelingSNO, this.Tags);
            tmp._owner = player;
            tmp.Position = this.Position;
            tmp.GBHandle.Type = 4;
            tmp.GBHandle.GBID = StringHashHelper.HashItemName("Templar");
            tmp.Field9 = 5;
            tmp.Field7 = 1;
            tmp.Attributes[GameAttribute.Pet_Creator] = player.PlayerIndex;
            tmp.Attributes[GameAttribute.Pet_Type] = 0;
            tmp.Attributes[GameAttribute.Pet_Owner] = player.PlayerIndex;

            tmp.RotationAmount = this.RotationAmount;
            tmp.RotationAxis = this.RotationAxis;

            player.InGameClient.SendMessage(new HirelingInfoUpdateMessage()
            {
                Field0 = 1,
                Field1 = false,
                Field2 = -1,
                Field3 = 10,
            });

            tmp.World.Enter(tmp);

            player.InGameClient.SendMessage(new Mooege.Net.GS.Message.Definitions.Player.PetMessage()
            {
                Field0 = 0,
                Field1 = 0,
                PetId = tmp.DynamicID,
                Field3 = (tmp.SNOId == proxySNO ? 22 : 0),
            });

            this.Destroy();
            player.SelectedNPC = null;            
        }

        public override void OnEnter(World world)
        {
            base.OnEnter(world);

            if (_owner == null)
                return;

            if (this.SNOId != hirelingSNO || this.SNOId != proxySNO)
                return;


        }

        public override bool Reveal(Player player)
        {
            if(!base.Reveal(player))
                return false;

            player.InGameClient.SendMessage(new VisualInventoryMessage()
            {
                ActorID = this.DynamicID,
                EquipmentList = new VisualEquipment()
                {
                    Equipment = new VisualItem[]
                    {
                        new VisualItem()
                        {
                            GbId = -1,
                            Field1 = 0,
                            Field2 = 0,
                            Field3 = 0,
                        },
                        new VisualItem()
                        {
                            GbId = -1,
                            Field1 = 0,
                            Field2 = 0,
                            Field3 = 0,
                        },
                        new VisualItem()
                        {
                            GbId = -1,
                            Field1 = 0,
                            Field2 = 0,
                            Field3 = 0,
                        },
                        new VisualItem()
                        {
                            GbId = -1,
                            Field1 = 0,
                            Field2 = 0,
                            Field3 = 0,
                        },
                        new VisualItem()
                        {
                            GbId = unchecked((int)0xF9F6170B),
                            Field1 = 0,
                            Field2 = 0,
                            Field3 = -1,
                        },
                        new VisualItem()
                        {
                            GbId = 0x6C3B0389,
                            Field1 = 0,
                            Field2 = 0,
                            Field3 = -1,
                        },
                        new VisualItem()
                        {
                            GbId = -1,
                            Field1 = 0,
                            Field2 = 0,
                            Field3 = 0,
                        },
                        new VisualItem()
                        {
                            GbId = -1,
                            Field1 = 0,
                            Field2 = 0,
                            Field3 = 0,
                        },
                    }
                }
            });

            return true;
        }
    }
}
