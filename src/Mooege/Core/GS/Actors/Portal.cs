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

using Mooege.Common.Helpers.Hash;
using Mooege.Common.Logging;
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Players;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Net.GS.Message.Definitions.World;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message.Definitions.Map;
using Mooege.Core.GS.Common.Types.TagMap;
using System.Collections.Generic;

namespace Mooege.Core.GS.Actors
{
    public class Portal : Actor
    {
        static readonly Logger Logger = LogManager.CreateLogger();

        public override ActorType ActorType { get { return ActorType.Gizmo; } }

        private ResolvedPortalDestination Destination { get; set; }
        private int MinimapIcon;

        public Portal(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            try
            {
                this.Destination = new ResolvedPortalDestination
                {
                    WorldSNO = tags[MarkerKeys.DestinationWorld].Id,
                    DestLevelAreaSNO = tags[MarkerKeys.DestinationLevelArea].Id,
                    StartingPointActorTag = tags[MarkerKeys.DestinationActorTag]
                };

                // Override minimap icon in merkerset tags
                if (tags.ContainsKey(MarkerKeys.MinimapTexture))
                {
                    MinimapIcon = tags[MarkerKeys.MinimapTexture].Id;
                }
                else
                {
                    MinimapIcon = ActorData.TagMap[ActorKeys.MinimapMarker].Id;
                }

            }
            catch (KeyNotFoundException)
            {
                Logger.Warn("Portal {0} has incomplete definition", this.ActorSNO.Id);
            }
            this.Field2 = 16;

            // FIXME: Hardcoded crap; probably don't need to set most of these. /komiga
            //this.Attributes[GameAttribute.MinimapActive] = true;
            //this.Attributes[GameAttribute.Hitpoints_Max_Total] = 1f;
            //this.Attributes[GameAttribute.Hitpoints_Max] = 0.0009994507f;
            //this.Attributes[GameAttribute.Hitpoints_Total_From_Level] = 3.051758E-05f;
            //this.Attributes[GameAttribute.Hitpoints_Cur] = 0.0009994507f;
            //this.Attributes[GameAttribute.Level] = 1;
        }

        public override bool Reveal(Player player)
        {
            if (!base.Reveal(player) || this.Destination == null)
                return false;

            player.InGameClient.SendMessage(new PortalSpecifierMessage()
            {
                ActorID = this.DynamicID,
                Destination = this.Destination
            });

            // Show a minimap icon
            Mooege.Common.MPQ.Asset asset;
            string markerName = "";

            if (Mooege.Common.MPQ.MPQStorage.Data.Assets[Common.Types.SNO.SNOGroup.LevelArea].TryGetValue(this.Destination.DestLevelAreaSNO, out asset))
                markerName = System.IO.Path.GetFileNameWithoutExtension(asset.FileName);

            player.InGameClient.SendMessage(new MapMarkerInfoMessage()
            {
                Field0 = (int)World.NewSceneID,    // TODO What is the correct id space for mapmarkers?
                Field1 = new WorldPlace()
                {
                    Position = this.Position,
                    WorldID = this.World.DynamicID
                },
                Field2 = MinimapIcon, //   0x00018FB0,  /* Marker_DungeonEntrance.tex */          // TODO Dont mark all portals as dungeon entrances... some may be exits too (although d3 does not necesarrily use the correct markers). Also i have found no hacky way to determine whether a portal is entrance or exit - farmy
                m_snoStringList = 0x0000CB2E, /* LevelAreaNames.stl */          // TODO Dont use hardcoded numbers

                Field3 = StringHashHelper.HashNormal(markerName),
                Field9 = 0,
                Field10 = 0,
                Field11 = 0,
                Field5 = 0,
                Field6 = true,
                Field7 = false,
                Field8 = true,
                Field12 = 0
            });
            return true;
        }

        public override void OnTargeted(Player player, TargetMessage message)
        {
            var world = this.World.Game.GetWorld(this.Destination.WorldSNO);

            if (world == null)
            {
                Logger.Warn("Portal's destination world does not exist (WorldSNO = {0})", this.Destination.WorldSNO);
                return;
            }

            var startingPoint = world.GetStartingPointById(this.Destination.StartingPointActorTag);

            if (startingPoint != null)
                player.ChangeWorld(world, startingPoint);
            else
                Logger.Warn("Portal's tagged starting point does not exist (Tag = {0})", this.Destination.StartingPointActorTag);
        }
    }
}
