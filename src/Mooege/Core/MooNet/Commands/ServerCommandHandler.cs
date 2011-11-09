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

using Mooege.Common.Helpers;
using Mooege.Core.MooNet.Accounts;
using Mooege.Core.MooNet.Toons;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Commands
{
    // TODO: This a hackish way to enable server commands for players that are not in a party! As soon as we find a better way we should get rid of this /raist.

    /// <summary>
    /// CommandHandler account that server commands can be sent to.
    /// So with this we're able to trigger commands even we're not in party.
    /// </summary>
    public class CommandHandlerAccount:Account
    {
        private static readonly CommandHandlerAccount _instance = new CommandHandlerAccount();
        public static CommandHandlerAccount Instance { get { return _instance; } }

        private CommandHandlerAccount()
            : base(0, "server@mooege", new byte[] {}, new byte[] {}, UserLevels.User)
        {
            this.LoggedInClient = new CommandHandlerClient(this);
        }

        public void ParseCommand(bnet.protocol.notification.Notification request, MooNetClient client)
        {
            if (request.Type != "WHISPER") return;
            if (request.AttributeCount <= 0 || !request.AttributeList[0].HasValue)  return;

            CommandManager.TryParse(request.AttributeList[0].Value.StringValue, client, CommandManager.RespondOver.Whisper);
        }
    }

    public class CommandHandlerClient : MooNetClient
    {       
        public CommandHandlerClient(Account account):base(null)
        {
            this.Account = account;            
        }
    }

    public class CommandHandlerToon:Toon
    {
        private static readonly CommandHandlerToon _instance = new CommandHandlerToon();
        public static CommandHandlerToon Instance { get { return _instance; } }

        private CommandHandlerToon()
            : base("Server", 0, 0x1D4681B1, ToonFlags.Female, 99, CommandHandlerAccount.Instance)
        {
            this.Owner = CommandHandlerAccount.Instance;
            this.SetShinyEquipment(); // server needs shiny equipment! /raist.
        }

        public void SetShinyEquipment()
        {
            var visualItems = new[]
            {                                
                D3.Hero.VisualItem.CreateBuilder().SetGbid(0).SetDyeType(0).SetItemEffectType(0).SetEffectLevel(0).Build(), // Head
                D3.Hero.VisualItem.CreateBuilder().SetGbid(StringHashHelper.HashItemName("ChestArmor_203")).SetDyeType(0).SetItemEffectType(0).SetEffectLevel(0).Build(), // Chest
                D3.Hero.VisualItem.CreateBuilder().SetGbid(StringHashHelper.HashItemName("Boots_Unique_001")).SetDyeType(0).SetItemEffectType(0).SetEffectLevel(0).Build(), // Feet
                D3.Hero.VisualItem.CreateBuilder().SetGbid(StringHashHelper.HashItemName("Gloves_205")).SetDyeType(0).SetItemEffectType(0).SetEffectLevel(0).Build(), // Hands
                D3.Hero.VisualItem.CreateBuilder().SetGbid(StringHashHelper.HashItemName("Unique_Mighty_2H_001")).SetDyeType(0).SetItemEffectType(0).SetEffectLevel(0).Build(), // Weapon (1)
                D3.Hero.VisualItem.CreateBuilder().SetGbid(0).SetDyeType(0).SetItemEffectType(0).SetEffectLevel(0).Build(), // Weapon (2)
                D3.Hero.VisualItem.CreateBuilder().SetGbid(StringHashHelper.HashItemName("Shoulders_205")).SetDyeType(0).SetItemEffectType(0).SetEffectLevel(0).Build(), // Shoulders
                D3.Hero.VisualItem.CreateBuilder().SetGbid(0).SetDyeType(0).SetItemEffectType(0).SetEffectLevel(0).Build(), // Legs
            };

            this.Equipment = D3.Hero.VisualEquipment.CreateBuilder().AddRangeVisualItem(visualItems).Build();            
        }
    }
}
