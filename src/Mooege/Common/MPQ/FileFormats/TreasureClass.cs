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

using System.Collections.Generic;
using Mooege.Common.MPQ.FileFormats.Types;
using Mooege.Core.GS.Common.Types.SNO;
using Mooege.Common.Logging;
using Mooege.Core.GS.Items;
using Mooege.Common.Helpers.Math;
using Mooege.Core.GS.Players;
using Mooege.Common.Storage;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.TreasureClass)]
    public class TreasureClass : FileFormat
    {
        Logger Logger = new Logger("TreasureClass");

        public static TreasureClass GenericTreasure
        {
            get
            {
                return new StandardTreasureClass();
            }
        }

        public class StandardTreasureClass : TreasureClass
        {
            public override Item CreateDrop(Player player)
            {
                return ItemGenerator.CreateGold(player, RandomHelper.Next(1000, 3000));
            }
        }

        [PersistentProperty("Percentage")]
        public float Percentage { get; private set; }

        [PersistentProperty("I0")]
        public int I0 { get; private set; }

        [PersistentProperty("LootDropModifiersCount")]
        public int LootDropModifiersCount { get; private set; }

        [PersistentProperty("LootDropModifiers")]
        public List<LootDropModifier> LootDropModifiers { get; private set; }

        public TreasureClass() { }

        public virtual Item CreateDrop(Player player)
        {
            Logger.Warn("Treasure classes not implemented, using generic treasure class");
            return TreasureClass.GenericTreasure.CreateDrop(player);
        }
    }

    public class LootDropModifier
    {
        [PersistentProperty("I0")]
        public int I0 { get; private set; }

        [PersistentProperty("SNOSubTreasureClass")]
        public int SNOSubTreasureClass { get; private set; }

        [PersistentProperty("Percentage")]
        public float Percentage { get; private set; }

        [PersistentProperty("I1")]
        public int I1 { get; private set; }

        [PersistentProperty("GBIdQualityClass")]
        public int GBIdQualityClass { get; private set; }

        [PersistentProperty("I2")]
        public int I2 { get; private set; }

        [PersistentProperty("I3")]
        public int I3 { get; private set; }

        [PersistentProperty("SNOCondition")]
        public int SNOCondition { get; private set; }

        [PersistentProperty("ItemSpecifier")]
        public ItemSpecifierData ItemSpecifier { get; private set; }

        [PersistentProperty("I5")]
        public int I5 { get; private set; }

        [PersistentProperty("I4", 4)]
        public int[] I4 { get; private set; }

        [PersistentProperty("I6")]
        public int I6 { get; private set; }

        public LootDropModifier() { }
    }
}
