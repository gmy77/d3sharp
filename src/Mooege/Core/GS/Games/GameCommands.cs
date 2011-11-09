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
using System.Linq;
using Mooege.Common.Helpers;
using Mooege.Common.MPQ;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Common.Types.SNO;
using Mooege.Core.GS.Items;
using Mooege.Core.MooNet.Commands;
using Mooege.Net.MooNet;
using System.Text;

namespace Mooege.Core.GS.Games
{
    [CommandGroup("spawn", "Spawns a mob.\nUsage: spawn [amount] [actorSNO]")]
    public class SpawnCommand : CommandGroup
    {
        [DefaultCommand]
        public string Spawn(string[] @params, MooNetClient invokerClient)
        {
            if (invokerClient == null)
                return "You can not invoke this command from console.";

            if (invokerClient.InGameClient == null)
                return "You can only invoke this command while ingame.";

            var player = invokerClient.InGameClient.Player;
            var actorSNO = 6652; /* zombie */
            var amount = 1;


            if (@params != null)
            {
                if (!Int32.TryParse(@params[0], out amount))
                    amount = 1;

                if (amount > 100) amount = 100;

                if (@params.Count() > 1)
                    if (!Int32.TryParse(@params[1], out actorSNO))
                        actorSNO = 6652;
            }

            for (int i = 0; i < amount; i++)
            {
                var position = new Vector3D(player.Position.X + (float) RandomHelper.NextDouble()*20f,
                                            player.Position.Y + (float) RandomHelper.NextDouble()*20f,
                                            player.Position.Z);

                player.World.SpawnMonster(actorSNO, position);
            }

            return string.Format("Spawned {0} mobs with ActorSNO: {1}", amount, actorSNO);
        }
    }

    [CommandGroup("item", "Spawns an item (with a name or type).\nUsage: item [type <type>|<name>] [amount]")]
    public class ItemCommand : CommandGroup
    {
        [DefaultCommand]
        public string Spawn(string[] @params, MooNetClient invokerClient)
        {
            if (invokerClient == null)
                return "You can not invoke this command from console.";

            if (invokerClient.InGameClient == null)
                return "You can only invoke this command while ingame.";

            var player = invokerClient.InGameClient.Player;
            var name = "Dye_02";
            var amount = 1;


            if (@params == null)
                return this.Fallback();

            name = @params[0];

            if (!ItemGenerator.IsValidItem(name))
                return "You need to specify a valid item name!";


            if (@params.Count() == 1 || !Int32.TryParse(@params[1], out amount))
                amount = 1;

            if (amount > 100) amount = 100;

            for (int i = 0; i < amount; i++)
            {
                var position = new Vector3D(player.Position.X + (float)RandomHelper.NextDouble() * 20f,
                                            player.Position.Y + (float)RandomHelper.NextDouble() * 20f,
                                            player.Position.Z);

                var item = ItemGenerator.Cook(player, name);
                item.EnterWorld(position);
            }

            return string.Format("Spawned {0} items with name: {1}", amount, name);

        }

        [Command("type", "Spawns random items of a given type.\nUsage: item type <type> [amount]")]
        public string Type(string[] @params, MooNetClient invokerClient)
        {
            if (invokerClient == null)
                return "You can not invoke this command from console.";

            if (invokerClient.InGameClient == null)
                return "You can only invoke this command while ingame.";

            var player = invokerClient.InGameClient.Player;
            var name = "Dye";
            var amount = 1;


            if (@params == null)
                return "You need to specify a item type!";

            name = @params[0];

            var type = ItemGroup.FromString(name);

            if (type == null)
                return "The type given is not a valid item type.";

            if (@params.Count() == 1 || !Int32.TryParse(@params[1], out amount))
                amount = 1;

            if (amount > 100) amount = 100;

            for (int i = 0; i < amount; i++)
            {
                var position = new Vector3D(player.Position.X + (float)RandomHelper.NextDouble() * 20f,
                                            player.Position.Y + (float)RandomHelper.NextDouble() * 20f,
                                            player.Position.Z);

                var item = ItemGenerator.GenerateRandom(player, type);
                item.EnterWorld(position);
            }

            return string.Format("Spawned {0} items with type: {1}", amount, name);
        }
    }

    [CommandGroup("tp", "Transfers your character to another world.")]
    public class TeleportCommand : CommandGroup
    {
        [DefaultCommand]
        public string Portal(string[] @params, MooNetClient invokerClient)
        {
            if (invokerClient == null)
                return "You can not invoke this command from console.";

            if (invokerClient.InGameClient == null)
                return "You can only invoke this command while ingame.";

            if (@params != null && @params.Count() > 0)
            {
                var worldId = 0;
                Int32.TryParse(@params[0], out worldId);

                if(worldId==0)
                    return "Invalid arguments. Type 'help tp' to get help.";

                if(!MPQStorage.Data.Assets[SNOGroup.Worlds].ContainsKey(worldId))
                    return "There exist no world with SNOId: " + worldId;

                var world = invokerClient.InGameClient.Game.GetWorld(worldId);
                
                if(world==null)
                    return "Can't teleport you to world with snoId " + worldId;

                invokerClient.InGameClient.Player.ChangeWorld(world, world.StartingPoints.First().Position);
                return string.Format("Teleported to: {0} [id: {1}]", MPQStorage.Data.Assets[SNOGroup.Worlds][worldId].Name, worldId);
            }

            return "Invalid arguments. Type 'help tp' to get help.";
        }
    }

    [CommandGroup("conversation", "Starts a conversation. \n Usage: conversation snoConversation")]
    public class ConversationCommand : CommandGroup
    {
        [DefaultCommand]
        public string Conversation(string[] @params, MooNetClient invokerClient)
        {
            if (invokerClient == null)
                return "You can not invoke this command from console.";

            if (invokerClient.InGameClient == null)
                return "You can only invoke this command while ingame.";

            if (@params.Count() != 1)
                return "Invalid arguments. Type 'help conversation' to get help.";

            try
            {
                var conversation = MPQStorage.Data.Assets[SNOGroup.Conversation][Int32.Parse(@params[0])];
                invokerClient.InGameClient.Player.Conversations.StartConversation(Int32.Parse(@params[0]));
                return String.Format("Started conversation {0}", conversation.FileName);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }


    [CommandGroup("quest", "Retrieves information about quest states and manipulates quest progress.\n Usage: quest [triggers | trigger eventType eventValue | advance snoQuest]")]
    public class QuestCommand : CommandGroup
    {
        [DefaultCommand]
        public string Quest(string[] @params, MooNetClient invokerClient)
        {
            if (invokerClient == null)
                return "You can not invoke this command from console.";

            if (invokerClient.InGameClient == null)
                return "You can only invoke this command while ingame.";

             return "";
        }

        [Command("advance", "Advances a quest by a single step\n Usage advance snoQuest")]
        public string Advance(string[] @params, MooNetClient invokerClient)
        {
            if (@params == null)
                return this.Fallback();

            if (@params.Count() != 1)
                return "Invalid arguments. Type 'help lookup advance' to get help.";

            try
            {
                var quest = MPQStorage.Data.Assets[SNOGroup.Quest][Int32.Parse(@params[0])];
                invokerClient.InGameClient.Game.Quests.Advance(Int32.Parse(@params[0]));
                return String.Format("Advancing quest {0}", quest.FileName);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        [Command("trigger", "Triggers a single quest objective\n Usage trigger type value")]
        public string Trigger(string[] @params, MooNetClient invokerClient)
        {
            if (@params == null)
                return this.Fallback();

            if (@params.Count() < 2)
                return "Invalid arguments. Type 'help lookup trigger' to get help.";

            invokerClient.InGameClient.Game.Quests.Notify((Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType)Int32.Parse(@params[0]), Int32.Parse(@params[1]));
            return "Triggered";
        }

        [Command("triggers", "lists all current quest triggers")]
        public string Triggers(string[] @params, MooNetClient invokerClient)
        {
            StringBuilder returnValue = new StringBuilder();

            foreach (var quest in invokerClient.InGameClient.Game.Quests)
                foreach (var objectiveSet in quest.CurrentStep.ObjectivesSets)
                    foreach (var objective in objectiveSet.Objectives)
                        returnValue.AppendLine(String.Format("{0}, {1} ({2}) - {3}", quest.SNOName.ToString(), objective.ObjectiveType, (int)objective.ObjectiveType, objective.ObjectiveValue));

            return returnValue.ToString();
        }

    }


    [CommandGroup("town", "Transfers your character back to town.")]
    public class TownCommand : CommandGroup
    {
        [DefaultCommand]
        public string Portal(string[] @params, MooNetClient invokerClient)
        {
            if (invokerClient == null)
                return "You can not invoke this command from console.";

            if (invokerClient.InGameClient == null)
                return "You can only invoke this command while ingame.";

            var world = invokerClient.InGameClient.Game.GetWorld(71150);

            if (world != invokerClient.InGameClient.Player.World)
                invokerClient.InGameClient.Player.ChangeWorld(world, world.StartingPoints.First().Position);
            else
                invokerClient.InGameClient.Player.Teleport(world.StartingPoints.First().Position);

            return string.Format("Teleported back to town.");
        }
    }

    [CommandGroup("lookup", "Searches in sno databases.\nUsage: lookup [actor|npc|mob|power|scene] <pattern>")]
    public class LookupCommand : CommandGroup
    {
        [DefaultCommand]
        public string Search(string[] @params, MooNetClient invokerClient)
        {
            if (@params == null) 
                return this.Fallback();

            var matches = new List<Asset>();

            if (@params.Count() < 1)
                return "Invalid arguments. Type 'help lookup actor' to get help.";

            var pattern = @params[0].ToLower();

            foreach (var groupPair in MPQStorage.Data.Assets)
            {
                foreach(var pair in groupPair.Value)
                {
                    if (pair.Value.Name.ToLower().Contains(pattern))
                        matches.Add(pair.Value);   
                }
            }

            return matches.Aggregate(matches.Count >= 1 ? "Matches:\n" : "No matches found.", (current, match) => current + string.Format("[{0}] [{1}] {2}\n", match.SNOId.ToString("D6"), match.Group, match.Name));
        }

        [Command("actor", "Allows you to search for an actor.\nUsage: lookup actor <pattern>")]
        public string Actor(string[] @params, MooNetClient invokerClient)
        {
            var matches = new List<Asset>();

            if (@params.Count() < 1)
                return "Invalid arguments. Type 'help lookup actor' to get help.";

            var pattern = @params[0].ToLower();

            foreach (var pair in MPQStorage.Data.Assets[SNOGroup.Actor])
            {
                if (pair.Value.Name.ToLower().Contains(pattern))
                    matches.Add(pair.Value);
            }

            return matches.Aggregate(matches.Count >= 1 ? "Actor Matches:\n" : "No match found.", (current, match) => current + string.Format("[{0}] {1}\n", match.SNOId.ToString("D6"), match.Name));
        }

        [Command("monster", "Allows you to search for a monster.\nUsage: lookup monster <pattern>")]
        public string Monster(string[] @params, MooNetClient invokerClient)
        {
            var matches = new List<Asset>();

            if (@params.Count() < 1)
                return "Invalid arguments. Type 'help lookup monster' to get help.";

            var pattern = @params[0].ToLower();

            foreach (var pair in MPQStorage.Data.Assets[SNOGroup.Monster])
            {
                if (pair.Value.Name.ToLower().Contains(pattern))
                    matches.Add(pair.Value);
            }

            return matches.Aggregate(matches.Count >= 1 ? "Monster Matches:\n" : "No match found.", (current, match) => current + string.Format("[{0}] {1}\n", match.SNOId.ToString("D6"), match.Name));
        }

        [Command("power", "Allows you to search for a power.\nUsage: lookup power <pattern>")]
        public string Power(string[] @params, MooNetClient invokerClient)
        {
            var matches = new List<Asset>();

            if (@params.Count() < 1)
                return "Invalid arguments. Type 'help lookup power' to get help.";

            var pattern = @params[0].ToLower();

            foreach (var pair in MPQStorage.Data.Assets[SNOGroup.Power])
            {
                if (pair.Value.Name.ToLower().Contains(pattern))
                    matches.Add(pair.Value);
            }

            return matches.Aggregate(matches.Count >= 1 ? "Power Matches:\n" : "No match found.", (current, match) => current + string.Format("[{0}] {1}\n", match.SNOId.ToString("D6"), match.Name));
        }

        [Command("world", "Allows you to search for a world.\nUsage: lookup world <pattern>")]
        public string World(string[] @params, MooNetClient invokerClient)
        {
            var matches = new List<Asset>();

            if (@params.Count() < 1)
                return "Invalid arguments. Type 'help lookup world' to get help.";

            var pattern = @params[0].ToLower();

            foreach (var pair in MPQStorage.Data.Assets[SNOGroup.Worlds])
            {
                if (pair.Value.Name.ToLower().Contains(pattern))
                    matches.Add(pair.Value);
            }

            return matches.Aggregate(matches.Count >= 1 ? "World Matches:\n" : "No match found.", (current, match) => current + string.Format("[{0}] {1}\n", match.SNOId.ToString("D6"), match.Name));
        }

        [Command("scene", "Allows you to search for a scene.\nUsage: lookup scene <pattern>")]
        public string Scene(string[] @params, MooNetClient invokerClient)
        {
            var matches = new List<Asset>();

            if (@params.Count() < 1)
                return "Invalid arguments. Type 'help lookup scene' to get help.";

            var pattern = @params[0].ToLower();

            foreach (var pair in MPQStorage.Data.Assets[SNOGroup.Monster])
            {
                if (pair.Value.Name.ToLower().Contains(pattern))
                    matches.Add(pair.Value);
            }

            return matches.Aggregate(matches.Count >= 1 ? "Scene Matches:\n" : "No match found.", (current, match) => current + string.Format("[{0}] {1}\n", match.SNOId.ToString("D6"), match.Name));
        }
    }

    [CommandGroup("effect", "Play an effect above your character.\nUsage: effect [FromUser] <efgSNO>")]
    public class EffectCommand : CommandGroup
    {
        [DefaultCommand]
        public string Effect(string[] @params, MooNetClient invokerClient)
        {
            if (@params == null)
                return this.Fallback();

            if (invokerClient == null)
                return "You can not invoke this command from console.";

            if (invokerClient.InGameClient == null)
                return "You can only invoke this command while ingame.";

            bool fromUser = false;
            if (@params.Length >= 1 && @params[0] == "FromUser")
                fromUser = true;

            string efgParam;
            if (fromUser && @params.Length >= 2)
                efgParam = @params[1];
            else if (@params.Length >= 1)
                efgParam = @params[0];
            else
                return "Invalid arguments. Type 'help effect' to get help.";

            int effectSNO;
            if (!int.TryParse(efgParam, out effectSNO))
                return "Invalid argument. Type 'help effect' to get help.";

            int actorSNO = fromUser ? 6652 : 187359;

            _TestActorWithEffect(invokerClient.InGameClient.Player, actorSNO, effectSNO, fromUser);

            return string.Format("Playing effect {0} on actor.", effectSNO);
        }

        private void _TestActorWithEffect(Mooege.Core.GS.Actors.Actor user, int actorSNO, int effectSNO, bool fromUser)
        {
            var position = new Vector3D(user.Position);
            position.Y += 10f;
            var actor = new Mooege.Core.GS.Powers.EffectActor(user.World, actorSNO, position, 0,
                                                              new Ticker.Helpers.TickSecondsTimer(user.World.Game, 5f));
            if (fromUser)
                user.PlayEffectGroup(effectSNO, actor);
            else
                actor.PlayEffectGroup(effectSNO);
        }
    }
}

