using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Common.Helpers;
using System.Diagnostics;
using Mooege.Net.GS.Message;
using Mooege.Common;
using Mooege.Core.GS.Players;
using Mooege.Net.GS.Message.Fields;
using Mooege.Core.GS.Map;
using Mooege.Common.MPQ;
using Mooege.Net.GS.Message.Definitions.World;
using Mooege.Core.GS.Common.Types.SNO;
using Mooege.Core.GS.Markers;

namespace Mooege.Core.Common.Items.Implementations
{
    [HandledType("SpellRune")]
    public class SpellRune : Item
    {
        // type of rune is in Name
        // Attributes[GameAttribute.Rune_<x>] = <rank>; // on attuned runes ONLY
        // Attributes[GameAttribute.Rune_Rank] = <in spec>; // on unattuned rune ONLY, inititalized in creation
        // Attributes[GameAttribute.Rune_Attuned_Power] = 0; // need s to be 0 on unattuned or random value from all powers

        public static readonly Logger Logger = LogManager.CreateLogger();

        public SpellRune(World world, Mooege.Common.MPQ.FileFormats.ItemTable definition)
            : base(world, definition)
        {
            if (!definition.Name.Contains("X"))
            {
                // attuned rune, randomize power
                int classRnd = RandomHelper.Next(0, 5);
                int PowerSNOId = -1;
                switch (classRnd)
                {
                    case 0:
                        PowerSNOId = Mooege.Core.GS.Skills.Skills.Barbarian.AllActiveSkillsList.ElementAt(RandomHelper.Next(0, Mooege.Core.GS.Skills.Skills.Barbarian.AllActiveSkillsList.Count));
                        break;
                    case 1:
                        PowerSNOId = Mooege.Core.GS.Skills.Skills.DemonHunter.AllActiveSkillsList.ElementAt(RandomHelper.Next(0, Mooege.Core.GS.Skills.Skills.DemonHunter.AllActiveSkillsList.Count));
                        break;
                    case 2:
                        PowerSNOId = Mooege.Core.GS.Skills.Skills.Monk.AllActiveSkillsList.ElementAt(RandomHelper.Next(0, Mooege.Core.GS.Skills.Skills.Monk.AllActiveSkillsList.Count));
                        break;
                    case 3:
                        PowerSNOId = Mooege.Core.GS.Skills.Skills.WitchDoctor.AllActiveSkillsList.ElementAt(RandomHelper.Next(0, Mooege.Core.GS.Skills.Skills.WitchDoctor.AllActiveSkillsList.Count));
                        break;
                    case 4:
                        PowerSNOId = Mooege.Core.GS.Skills.Skills.Wizard.AllActiveSkillsList.ElementAt(RandomHelper.Next(0, Mooege.Core.GS.Skills.Skills.Wizard.AllActiveSkillsList.Count));
                        break;
                }
                this.Attributes[GameAttribute.Rune_Attuned_Power] = PowerSNOId;
            }
        }

        /// <summary>
        /// Re-attunes rune to player's class. Used for favoring.
        /// </summary>
        /// <param name="toonClass"></param>
        public void ReAttuneToClass(Toons.ToonClass toonClass)
        {
            int PowerSNOId = -1;
            switch (toonClass)
            {
                case Toons.ToonClass.Barbarian:
                    PowerSNOId = Mooege.Core.GS.Skills.Skills.Barbarian.AllActiveSkillsList.ElementAt(RandomHelper.Next(0, Mooege.Core.GS.Skills.Skills.Barbarian.AllActiveSkillsList.Count));
                    break;
                case Toons.ToonClass.DemonHunter:
                    PowerSNOId = Mooege.Core.GS.Skills.Skills.DemonHunter.AllActiveSkillsList.ElementAt(RandomHelper.Next(0, Mooege.Core.GS.Skills.Skills.DemonHunter.AllActiveSkillsList.Count));
                    break;
                case Toons.ToonClass.Monk:
                    PowerSNOId = Mooege.Core.GS.Skills.Skills.Monk.AllActiveSkillsList.ElementAt(RandomHelper.Next(0, Mooege.Core.GS.Skills.Skills.Monk.AllActiveSkillsList.Count));
                    break;
                case Toons.ToonClass.WitchDoctor:
                    PowerSNOId = Mooege.Core.GS.Skills.Skills.WitchDoctor.AllActiveSkillsList.ElementAt(RandomHelper.Next(0, Mooege.Core.GS.Skills.Skills.WitchDoctor.AllActiveSkillsList.Count));
                    break;
                case Toons.ToonClass.Wizard:
                    PowerSNOId = Mooege.Core.GS.Skills.Skills.Wizard.AllActiveSkillsList.ElementAt(RandomHelper.Next(0, Mooege.Core.GS.Skills.Skills.Wizard.AllActiveSkillsList.Count));
                    break;
            }
            this.Attributes[GameAttribute.Rune_Attuned_Power] = PowerSNOId;
        }
    }
}
