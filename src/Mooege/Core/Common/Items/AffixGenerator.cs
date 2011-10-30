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
using System.Text;
using Mooege.Common.Helpers;
using Mooege.Net.GS.Message;
using Mooege.Common;
using System.Reflection;
using Mooege.Common.MPQ.FileFormats;
using Mooege.Common.MPQ;

namespace Mooege.Core.Common.Items
{
    static class AffixGenerator
    {
        public static readonly Logger Logger = LogManager.CreateLogger();

        private static Dictionary<int, Dictionary<int, AffixTable>> affixList = new Dictionary<int, Dictionary<int, AffixTable>>();

        static AffixGenerator()
        {
            foreach (var asset in MPQStorage.Data.Assets[SNOGroup.GameBalance].Values)
            {
                GameBalance data = asset.Data as GameBalance;
                if (data != null && data.type == BalanceType.AffixList)
                {
                    foreach (var affixDef in data.Affixes)
                    {
                        Dictionary<int, AffixTable> list;
                        if (!affixList.TryGetValue(affixDef.AffixFamily0, out list))
                        {
                            list = new Dictionary<int, AffixTable>();
                            affixList.Add(affixDef.AffixFamily0, list);
                        }
                        list.Add(StringHashHelper.HashItemName(affixDef.Name), affixDef);
                    }
                }
            }
        }

        public static void Generate(Item item, int affixesCount)
        {
            if (!Item.IsWeapon(item.ItemType) && !Item.IsArmor(item.ItemType) && !Item.IsAccessory(item.ItemType))
                return;

            if(item.ItemType == ItemType.Orb)
                item.AffixList.Add(new Affix(0x17DC7B00));

            // set item level
            int itemLevel = RandomHelper.Next(1, 61);
            // item.Attributes[GameAttribute.Requirement, 38] = itemLevel;

            ItemRandomHelper irh = new ItemRandomHelper(item.Attributes[GameAttribute.Seed]);
            irh.Next(); // 1 random is always skipped
            if(Item.IsArmor(item.ItemType))
                irh.Next(); // next value is used but unknown if armor
            irh.ReinitSeed();
            if (Item.IsWeapon(item.ItemType) && item.ItemType != ItemType.Orb)
            {
                irh.Next(); // unknown
                irh.Next(); // unknown
            }


            var selected = affixList.OrderBy(x => RandomHelper.Next()).Take(affixesCount);
            foreach (var definitions in selected)
            {
                var bestDef = definitions.Value.Values.OrderByDescending(x => x.AffixLevel).Where(x => x.AffixLevel <= itemLevel).FirstOrDefault();

                if (bestDef != null)
                {
                    Logger.Debug("Generating affix " + bestDef.Name + " (aLvl:" + bestDef.AffixLevel + ")");
                    item.AffixList.Add(new Affix(StringHashHelper.HashItemName(bestDef.Name)));
                    foreach (var effect in bestDef.attributeSpecifier)
                    {
                        float result;
                        if (BBEScript.Evaluate(effect.formula.ToArray(), irh, out result))
                        {
                            var attr = GameAttribute.GameAttributeArray[effect.AttributeId] as GameAttributeF;
                            if (attr != null)
                            {
                                Logger.Debug("Randomized value for attribute " + attr.Name + " is " + result);
                                item.Attributes[attr] += result;
                            }
                        }
                    }
                }
            }
        }
    }

    public enum AffixType
    {
        Prefix,
        Suffix
    }

    public enum AffixEffectValueType
    {
        IntMinMax,
        IntFix,
        Percent,
    }

    // TODO: Use affixes associated with item type
    partial class AffixDefinition
    {


        public string Name;
        public int AffixGbid;
        public AffixType Type;
        public int MinLevel;
        public List<AffixEffect> Effects;

        public class AffixEffect
        {
            public string EffectAttribute;
            public AffixEffectValueType EffectValueType;
            public int MinI;
            public int MaxI;
            public float MinF;
            public float MaxF;

            public AffixEffect(string effectAttr, int min, int max)
            {
                EffectAttribute = effectAttr;
                EffectValueType = AffixEffectValueType.IntMinMax;
                MinI = min;
                MaxI = max;
            }

            public AffixEffect(string effectAttr, float min, float max)
            {
                EffectAttribute = effectAttr;
                EffectValueType = AffixEffectValueType.Percent;
                MinF = min;
                MaxF = max;
            }

            public AffixEffect(string effectAttr, int fix)
            {
                EffectAttribute = effectAttr;
                EffectValueType = AffixEffectValueType.IntFix;
                MinI = fix;
                MaxI = fix;
            }
        }

        public AffixDefinition(string a, AffixType t, int level)
        {
            Name = a;
            AffixGbid = StringHashHelper.HashItemName(Name);
            Type = t;
            MinLevel = level;
            Effects = new List<AffixEffect>();
        }

        public AffixDefinition AddEffect(string effectAttr, int min, int max)
        {
            Effects.Add(new AffixEffect(effectAttr, min, max));
            return this;
        }

        public AffixDefinition AddEffect(string effectAttr, int fix)
        {
            Effects.Add(new AffixEffect(effectAttr, fix));
            return this;
        }

        public AffixDefinition AddEffect(string effectAttr, float min, float max)
        {
            Effects.Add(new AffixEffect(effectAttr, min, max));
            return this;
        }
    }


    // temp
    static class BBEScript
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        private static float BinaryIntToFloat(int n)
        {
            byte[] array = BitConverter.GetBytes(n);
            return BitConverter.ToSingle(array, 0);
        }

        public static bool Evaluate(int[] script, ItemRandomHelper irh, out float result)
        {
            result = 0;
            Stack<float> stack = new Stack<float>(64);
            int pos = 0;
            float numb1, numb2, numb3;
            while (pos < script.Length)
            {
                switch ((byte)script[pos])
                {
                    case 0:
                        if (stack.Count < 1)
                        {
                            Logger.Error("Stack underflow");
                            return false;
                        }
                        result = stack.Pop();
                        return true;
                    case 1:
                        ++pos;
                        byte funcId = (byte)script[pos];
                        switch (funcId)
                        {
                            case 3:
                                if (stack.Count < 2)
                                {
                                    Logger.Error("Stack underflow");
                                    return false;
                                }
                                numb2 = stack.Pop();
                                numb1 = stack.Pop();
                                stack.Push(irh.Next(numb1, numb1+numb2));
                                break;
                            case 4:
                                if (stack.Count < 2)
                                {
                                    Logger.Error("Stack underflow");
                                    return false;
                                }
                                numb2 = stack.Pop();
                                numb1 = stack.Pop();
                                stack.Push(irh.Next(numb1, numb2));
                                break;
                            default:
                                Logger.Error("Unimplemented function");
                                return false;
                        }
                        break;
                    case 6:
                        ++pos;
                        stack.Push(BinaryIntToFloat(script[pos]));
                        break;
                    case 11:
                        if (stack.Count < 2)
                        {
                            Logger.Error("Stack underflow");
                            return false;
                        }
                        numb2 = stack.Pop();
                        numb1 = stack.Pop();
                        stack.Push(numb1 + numb2);
                        break;
                    case 12:
                        if (stack.Count < 2)
                        {
                            Logger.Error("Stack underflow");
                            return false;
                        }
                        numb2 = stack.Pop();
                        numb1 = stack.Pop();
                        stack.Push(numb1 - numb2);
                        break;
                    case 13:
                        if (stack.Count < 2)
                        {
                            Logger.Error("Stack underflow");
                            return false;
                        }
                        numb2 = stack.Pop();
                        numb1 = stack.Pop();
                        stack.Push(numb1 * numb2);
                        break;
                    case 14:
                        if (stack.Count < 2)
                        {
                            Logger.Error("Stack underflow");
                            return false;
                        }
                        numb2 = stack.Pop();
                        numb1 = stack.Pop();
                        if (numb1 == 0f)
                        {
                            Logger.Error("Division by zero");
                            return false;
                        }
                        stack.Push(numb1 / numb2);
                        break;
                    default:
                        Logger.Error("Unimplemented OpCode");
                        return false;
                }
                ++pos;
            }
            return false;
        }

        public static string ToString(int[] script)
        {
            StringBuilder b = new StringBuilder();
            int pos = 0;
            while (pos < script.Length)
            {
                switch ((byte)script[pos])
                {
                    case 0:
                        b.Append("return; ");
                        break;
                    case 1:
                        ++pos;
                        ToStringFunc(b, (byte)script[pos]);
                        break;
                    case 5:
                        ToStringIdentifierEval(
                            b,
                            script[pos + 1],
                            script[pos + 2],
                            script[pos + 3],
                            script[pos + 4]);
                        ++pos;
                        break;
                    case 6:
                        b.Append("push ");
                        ++pos;
                        b.Append(BinaryIntToFloat(script[pos]));
                        b.Append("; ");
                        break;
                    case 11:
                        b.Append("add; ");
                        break;
                    case 12:
                        b.Append("sub; ");
                        break;
                    case 13:
                        b.Append("mul; ");
                        break;
                    case 14:
                        b.Append("div; ");
                        break;
                    default:
                        b.Append("unknownOp (");
                        b.Append(script[pos]);
                        b.Append("); ");
                        break;
                }
                ++pos;
            }
            return b.ToString();
        }

        static void ToStringFunc(StringBuilder b, byte funcId)
        {
            switch (funcId)
            {
                case 0:
                    b.Append("func Min(stack1, stack0); ");
                    break;
                case 1:
                    b.Append("func Max(stack1, stack0); ");
                    break;
                case 2:
                    b.Append("func Pin(stack2, stack1, stack0); ");
                    break;
                case 3:
                    b.Append("func RandomIntMinRange(stack1, stack0); ");
                    break;
                case 4:
                    b.Append("func RandomIntMinMax(stack1, stack0); ");
                    break;
                case 5:
                    b.Append("func Floor(stack0); ");
                    break;
                case 6:
                    b.Append("func Dim(stack2, stack1, stack0); ");
                    break;
                case 7:
                    b.Append("func Pow(stack1, stack0); ");
                    break;
                case 8:
                    b.Append("func Log(stack0); ");
                    break;
                case 9:
                    b.Append("func RandomFloatMinRange(stack1, stack0); ");
                    break;
                case 10:
                    b.Append("func RandomFloatMinMax(stack1, stack0); ");
                    break;
                case 11:
                    b.Append("func TableLookup(stack1, stack0); ");
                    break;
                default:
                    b.Append("unknownFunc(");
                    b.Append(funcId);
                    b.Append("); ");
                    break;
            }
        }

        static void ToStringIdentifierEval(StringBuilder b, int numb1, int numb2, int numb3, int numb4)
        {
            switch (numb1)
            {
                case 0:
                    b.Append("GetAttribute ");
                    b.Append(GameAttribute.Attributes[numb2].Name);
                    b.Append(" ("); b.Append(numb2); b.Append("); ");
                    break;
                case 22:
                    b.Append("RunScript ... ; ");
                    break;
                default:
                    b.Append("unknown source on identifier function ; ");
                    break;
            }
        }
    }

}
