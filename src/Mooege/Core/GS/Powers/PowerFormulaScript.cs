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
using Mooege.Common;
using Mooege.Common.Helpers;
using Mooege.Common.MPQ;
using Mooege.Common.MPQ.FileFormats;
using Mooege.Common.MPQ.FileFormats.Types;
using Mooege.Core.GS.Common.Types.SNO;
using Mooege.Core.GS.Common.Types.TagMap;
using Mooege.Net.GS.Message;

namespace Mooege.Core.GS.Powers
{
    // Based off Items.FormulaScript class, modified to read game attributes and fully execute power script formulas.
    static class PowerFormulaScript
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        public static TagKeyScript GenerateTagForScriptFormula(int SF_N)
        {
            return new TagKeyScript(266496 + 256 * (SF_N / 10) + 16 * (SF_N % 10));
        }

        public static bool Evaluate(int powerSNO, TagKeyScript scriptTag, GameAttributeMap attributes, Random rand, out float result)
        {
            result = 0;

            ScriptFormula scriptFormula = FindScriptFormula(powerSNO, scriptTag);
            if (scriptFormula == null)
            {
                Logger.Error("could not find script tag {0} in power {1}", scriptTag.ID, powerSNO);
                return false;
            }

            // load script from byte[] into int[]
            int[] script = new int[scriptFormula.OpCodeArray.Length / 4];
            for (int i = 0; i * 4 < scriptFormula.OpCodeArray.Length; ++i)
                script[i] = BitConverter.ToInt32(scriptFormula.OpCodeArray, i * 4);

            Stack<float> stack = new Stack<float>(64);
            int pos = 0;
            float numb1, numb2, numb3;
            float temp;
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
                            case 0: // Min()
                                if (stack.Count < 2)
                                {
                                    Logger.Error("Stack underflow");
                                    return false;
                                }
                                numb2 = stack.Pop();
                                numb1 = stack.Pop();
                                stack.Push(Math.Min(numb1, numb2));
                                break;
                            case 1: // Max()
                                if (stack.Count < 2)
                                {
                                    Logger.Error("Stack underflow");
                                    return false;
                                }
                                numb2 = stack.Pop();
                                numb1 = stack.Pop();
                                stack.Push(Math.Max(numb1, numb2));
                                break;
                            case 2: // Pin()
                                if (stack.Count < 3)
                                {
                                    Logger.Error("Stack underflow");
                                    return false;
                                }
                                numb3 = stack.Pop();
                                numb2 = stack.Pop();
                                numb1 = stack.Pop();
                                if (numb2 > numb1)
                                    stack.Push(numb2);
                                else if (numb1 > numb3)
                                    stack.Push(numb3);
                                else
                                    stack.Push(numb1);

                                break;
                            case 3:
                                if (stack.Count < 2)
                                {
                                    Logger.Error("Stack underflow");
                                    return false;
                                }
                                numb2 = stack.Pop();
                                numb1 = stack.Pop();
                                stack.Push(numb1 + (float)rand.NextDouble() * numb2); // TODO: should these be int rounded?
                                break;
                            case 4:
                                if (stack.Count < 2)
                                {
                                    Logger.Error("Stack underflow");
                                    return false;
                                }
                                numb2 = stack.Pop();
                                numb1 = stack.Pop();
                                stack.Push(numb1 + (float)rand.NextDouble() * (numb2 - numb1));
                                break;
                            case 5: // Floor()
                                if (stack.Count < 1)
                                {
                                    Logger.Error("Stack underflow");
                                    return false;
                                }
                                numb1 = stack.Pop();
                                stack.Push((float)Math.Floor(numb1));
                                break;
                            case 9: // RandomFloatMinRange()
                                if (stack.Count < 2)
                                {
                                    Logger.Error("Stack underflow");
                                    return false;
                                }
                                numb2 = stack.Pop();
                                numb1 = stack.Pop();
                                stack.Push(numb1 + (float)rand.NextDouble() * numb2);
                                break;
                            case 10: // RandomFloatMinMax()
                                if (stack.Count < 2)
                                {
                                    Logger.Error("Stack underflow");
                                    return false;
                                }
                                numb2 = stack.Pop();
                                numb1 = stack.Pop();
                                stack.Push(numb1 + (float)rand.NextDouble() * (numb2 - numb1));
                                break;
                            case 11: // Table()
                                if (stack.Count < 2)
                                {
                                    Logger.Error("Stack underflow");
                                    return false;
                                }
                                float index = stack.Pop();
                                float tableID = stack.Pop();
                                if (!LookupBalanceTable(tableID, index, out temp))
                                    return false;
                                stack.Push(temp);
                                break;
                            default:
                                Logger.Error("Unimplemented function");
                                return false;
                        }
                        break;
                    case 5:
                        if (!LoadIdentifier(powerSNO, scriptTag, attributes, rand,
                                            script[pos + 1],
                                            script[pos + 2],
                                            script[pos + 3],
                                            script[pos + 4],
                                            out temp))
                            return false;

                        stack.Push(temp);
                        pos += 4;
                        break;
                    case 6:
                        ++pos;
                        stack.Push(BinaryIntToFloat(script[pos]));
                        break;
                    case 8: // operator >
                        if (stack.Count < 2)
                        {
                            Logger.Error("Stack underflow");
                            return false;
                        }
                        numb2 = stack.Pop();
                        numb1 = stack.Pop();
                        stack.Push(numb1 > numb2 ? 1 : 0);
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
                    case 16: // operator -(unary)
                        if (stack.Count < 1)
                        {
                            Logger.Error("Stack underflow");
                            return false;
                        }
                        numb1 = stack.Pop();
                        stack.Push(-numb1);
                        break;
                    case 17: // operator ?:
                        if (stack.Count < 3)
                        {
                            Logger.Error("Stack underflow");
                            return false;
                        }
                        numb3 = stack.Pop();
                        numb2 = stack.Pop();
                        numb1 = stack.Pop();
                        stack.Push(numb1 != 0 ? numb2 : numb3);
                        break;
                    default:
                        Logger.Error("Unimplemented OpCode({0})", (byte)script[pos]);
                        return false;
                }
                ++pos;
            }
            return false;
        }

        private static float BinaryIntToFloat(int n)
        {
            byte[] array = BitConverter.GetBytes(n);
            return BitConverter.ToSingle(array, 0);
        }

        private static bool LoadIdentifier(int powerSNO, TagKeyScript scriptTag, GameAttributeMap attributes, Random rand, 
                                           int numb1, int numb2, int numb3, int numb4,
                                           out float result)
        {
            switch (numb1)
            {
                case 0:
                    return LoadAttribute(powerSNO, attributes, numb2, out result);
                case 1: // slevel
                    result = attributes[GameAttribute.Skill, powerSNO];
                    return true;
                case 22: // absolute power formula ref
                    return Evaluate(numb2, new TagKeyScript(numb3), attributes, rand, out result);
                default:
                    if (numb1 >= 23 && numb1 <= 62) // SF_N, relative power formula ref
                    {
                        int SF_N = numb1 - 23;
                        TagKeyScript relativeTag = GenerateTagForScriptFormula(SF_N);
                        return Evaluate(powerSNO, relativeTag, attributes, rand, out result);
                    }
                    else if (numb1 >= 63 && numb1 <= 71) // known gamebalance power table id range
                    {
                        result = BinaryIntToFloat(numb1); // simply store id, used later by Table()
                        return true;
                    }
                    else
                    {
                        Logger.Error("unknown identifier");
                        result = 0;
                        return false;
                    }
            }
        }

        // this lists the attributes that need to be keyed with the powerSNO to work
        private static readonly SortedSet<int> _powerKeyedAttributes = new SortedSet<int>()
        {
            GameAttribute.Rune_A.Id,
            GameAttribute.Rune_B.Id,
            GameAttribute.Rune_C.Id,
            GameAttribute.Rune_D.Id,
            GameAttribute.Rune_E.Id
        };

        private static bool LoadAttribute(int powerSNO, GameAttributeMap attributes, int attributeId, out float result)
        {
            GameAttribute attr = GameAttribute.Attributes[attributeId];
            bool needs_key = _powerKeyedAttributes.Contains(attributeId);

            if (attr is GameAttributeF)
            {
                if (needs_key) result = attributes[(GameAttributeF)attr, powerSNO];
                else result = attributes[(GameAttributeF)attr];

                return true;
            }            
            else if (attr is GameAttributeI)
            {
                if (needs_key) result = (float)attributes[(GameAttributeI)attr, powerSNO];
                else result = (float)attributes[(GameAttributeI)attr];
                
                return true;
            }
            else if (attr is GameAttributeB)
            {
                if (needs_key) result = attributes[(GameAttributeB)attr, powerSNO] ? 1 : 0;
                else result = attributes[(GameAttributeB)attr] ? 1 : 0;

                return true;
            }
            else
            {
                Logger.Error("invalid attribute {0}", attributeId);
                result = 0;
                return false;
            }
        }

        private static ScriptFormula FindScriptFormula(int powerSNO, TagKeyScript scriptTag)
        {
            Power power = (Power)MPQStorage.Data.Assets[SNOGroup.Power][powerSNO].Data;

            // TODO: figure out which tagmaps to search and in what order
            TagMap[] tagMaps = new TagMap[]
            {
                power.Powerdef.GeneralTagMap,
                power.Powerdef.TagMap,
                power.Powerdef.ContactTagMap0,
                power.Powerdef.ContactTagMap1,
                power.Powerdef.ContactTagMap2,
                power.Powerdef.ContactTagMap3,
                power.Powerdef.PVPGeneralTagMap,
                power.Powerdef.PVPContactTagMap0,
                power.Powerdef.PVPContactTagMap1,
                power.Powerdef.PVPContactTagMap2,
                power.Powerdef.PVPContactTagMap3,
            };

            foreach (TagMap tagmap in tagMaps)
            {
                if (tagmap.ContainsKey(scriptTag))
                    return tagmap[scriptTag];
            }

            return null;
        }

        private static bool LookupBalanceTable(float tableId, float index, out float result)
        {
            result = 0;

            int tableByte = BitConverter.GetBytes(tableId)[0];
            string tableName = GetTableName(tableByte);
            if (tableName == null)
                return false;

            foreach (GameBalance gb in MPQStorage.Data.Assets[SNOGroup.GameBalance].Values.Where(a => a.Data != null)
                                                                                          .Select(a => a.Data)
                                                                                          .Cast<GameBalance>())
            {
                foreach (var powerEntry in gb.PowerFormula)
                {
                    if (powerEntry.S0 == tableName)
                    {
                        result = powerEntry.F0[(int)index];
                        return true;
                    }
                }
            }

            Logger.Error("could not find table {0}", tableName);
            return false;
        }

        private static string GetTableName(int tableId)
        {
            switch (tableId)
            {
                case 63:
                    return "DmgTier1";
                case 64:
                    return "DmgTier2";
                case 65:
                    return "DmgTier3";
                case 66:
                    return "DmgTier4";
                case 67:
                    return "DmgTier5";
                case 68:
                    return "DmgTier6";
                case 69:
                    return "DmgTier7";
                case 70:
                    return "Healing";
                case 71:
                    return "WDCost";
                default:
                    Logger.Error("Unknown table id {0}", tableId);
                    return null;
            }
        }

        // TODO: disassembler is completely out of date

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
