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

﻿using Mooege.Common;
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Player;
using Mooege.Net.GS;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Net.GS.Message.Definitions.Attribute;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Net.GS.Message.Fields;

// TODO: What should the GBHandle for this actor type be?

namespace Mooege.Core.GS.Map
{
    public class Portal : Actor
    {
        static readonly Logger Logger = LogManager.CreateLogger();

        public override ActorType ActorType { get { return ActorType.Portal; } }

        public ResolvedPortalDestination Destination { get; private set; }
        public Vector3D TargetPos;

        public Portal(World world)
            : base(world, world.NewActorID)
        {
            this.Destination = new ResolvedPortalDestination();
            this.Destination.WorldSNO = -1;
            this.Destination.DestLevelAreaSNO = -1;
            this.TargetPos = new Vector3D();

            // FIXME: Hardcoded crap
            this.Attributes[GameAttribute.MinimapActive] = true;
            this.Attributes[GameAttribute.Hitpoints_Max_Total] = 1f;
            this.Attributes[GameAttribute.Hitpoints_Max] = 0.0009994507f;
            this.Attributes[GameAttribute.Hitpoints_Total_From_Level] = 3.051758E-05f;
            this.Attributes[GameAttribute.Hitpoints_Cur] = 0.0009994507f;
            this.Attributes[GameAttribute.TeamID] = 1;
            this.Attributes[GameAttribute.Level] = 1;

            this.World.Enter(this); // Enter only once all fields have been initialized to prevent a run condition
        }

        public override void Reveal(Mooege.Core.GS.Player.Player player)
        {
            if (TargetPos != null)
                // targetpos!=null in this case is used to detect if the portal has been completely initialized to have a target
                // if it doesn't have one, it won't be displayed - otherwise the client would crash from this.
            {
                //Logger.Info("Revealing portal {0}", PortalMessage.AsText());

                base.Reveal(player);

                // FIXME: Hardcoded crap
                player.InGameClient.SendMessage(new AffixMessage()
                {
                    ActorID = this.DynamicID,
                    Field1 = 1,
                    aAffixGBIDs = new int[0]
                });

                player.InGameClient.SendMessage(new AffixMessage()
                {
                    ActorID = this.DynamicID,
                    Field1 = 2,
                    aAffixGBIDs = new int[0]
                });

                player.InGameClient.SendMessage(new PortalSpecifierMessage()
                {
                    ActorID = this.DynamicID,
                    Destination = this.Destination
                });

                player.InGameClient.SendMessage(new ACDCollFlagsMessage()
                {
                    ActorID = this.DynamicID,
                    CollFlags = 0x00000001,
                });

                this.Attributes.SendMessage(player.InGameClient, this.DynamicID);

                player.InGameClient.SendMessage(new ACDGroupMessage()
                {
                    ActorID = this.DynamicID,
                    Field1 = -1,
                    Field2 = -1,
                });

                player.InGameClient.SendMessage(new ANNDataMessage(Opcodes.ANNDataMessage7)
                {
                    ActorID = this.DynamicID,
                });

                player.InGameClient.SendMessage(new ACDTranslateFacingMessage(Opcodes.ACDTranslateFacingMessage1)
                {
                    ActorID = this.DynamicID,
                    Angle = 0f,
                    Field2 = false,
                });
            }
            player.InGameClient.FlushOutgoingBuffer();
        }

        public override void Unreveal(Mooege.Core.GS.Player.Player player)
        {
        }

        public override void OnTargeted(Mooege.Core.GS.Player.Player player)
        {
            World world = this.World.Game.GetWorld(this.Destination.WorldSNO);
            if (world != null)
                player.TransferTo(world, this.TargetPos);
            else
                Logger.Warn("Portal's destination world does not exist (WorldSNO = {0})", this.Destination.WorldSNO);
        }
    }
}
