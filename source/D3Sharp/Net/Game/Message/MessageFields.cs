/*
 * Copyright (C) 2011 D3Sharp Project
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

namespace D3Sharp.Net.Game.Messages
{
    public class HeroStateData
    {
        public int Field0;
        public int Field1;
        public int Field2;
        public int Field3;
        public PlayerSavedData Field4;
        public int Field5;
        // MaxLength = 100
        public PlayerQuestRewardHistoryEntry[] tQuestRewardHistory;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(29);
            Field4 = new PlayerSavedData();
            Field4.Parse(buffer);
            Field5 = buffer.ReadInt(32);
            tQuestRewardHistory = new PlayerQuestRewardHistoryEntry[buffer.ReadInt(7)];
            for (int i = 0; i < tQuestRewardHistory.Length; i++)
            {
                tQuestRewardHistory[i] = new PlayerQuestRewardHistoryEntry();
                tQuestRewardHistory[i].Parse(buffer);
            }
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(29, Field3);
            Field4.Encode(buffer);
            buffer.WriteInt(32, Field5);
            buffer.WriteInt(7, tQuestRewardHistory.Length);
            for (int i = 0; i < tQuestRewardHistory.Length; i++)
            {
                tQuestRewardHistory[i].Encode(buffer);
            }
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("HeroStateData:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            Field4.AsText(b, pad);
            b.Append(' ', pad);
            b.AppendLine("Field5: 0x" + Field5.ToString("X8") + " (" + Field5 + ")");
            b.Append(' ', pad);
            b.AppendLine("tQuestRewardHistory:");
            b.Append(' ', pad);
            b.AppendLine("{");
            for (int i = 0; i < tQuestRewardHistory.Length; i++)
            {
                tQuestRewardHistory[i].AsText(b, pad + 1);
                b.AppendLine();
            }
            b.Append(' ', pad);
            b.AppendLine("}");
            b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class Vector3D
    {
        public float Field0;
        public float Field1;
        public float Field2;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadFloat32();
            Field1 = buffer.ReadFloat32();
            Field2 = buffer.ReadFloat32();
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteFloat32(Field0);
            buffer.WriteFloat32(Field1);
            buffer.WriteFloat32(Field2);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("Vector3D:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: " + Field0.ToString("G"));
            b.Append(' ', pad);
            b.AppendLine("Field1: " + Field1.ToString("G"));
            b.Append(' ', pad);
            b.AppendLine("Field2: " + Field2.ToString("G"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class SceneSpecification
    {
        public int Field0;
        public IVector2D Field1;
        // MaxLength = 4
        public int /* sno */[] arSnoLevelAreas; //area names
        public int /* sno */ snoPrevWorld;
        public int Field4;
        public int /* sno */ snoPrevLevelArea;
        public int /* sno */ snoNextWorld;
        public int Field7;
        public int /* sno */ snoNextLevelArea;
        public int /* sno */ snoMusic;
        public int /* sno */ snoCombatMusic;
        public int /* sno */ snoAmbient;
        public int /* sno */ snoReverb;
        public int /* sno */ snoWeather;
        public int /* sno */ snoPresetWorld;
        public int Field15;
        public int Field16;
        public int Field17;
        public int Field18;
        public SceneCachedValues tCachedValues;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = new IVector2D();
            Field1.Parse(buffer);
            arSnoLevelAreas = new int /* sno */[4];
            for (int i = 0; i < arSnoLevelAreas.Length; i++) arSnoLevelAreas[i] = buffer.ReadInt(32);
            snoPrevWorld = buffer.ReadInt(32);
            Field4 = buffer.ReadInt(32);
            snoPrevLevelArea = buffer.ReadInt(32);
            snoNextWorld = buffer.ReadInt(32);
            Field7 = buffer.ReadInt(32);
            snoNextLevelArea = buffer.ReadInt(32);
            snoMusic = buffer.ReadInt(32);
            snoCombatMusic = buffer.ReadInt(32);
            snoAmbient = buffer.ReadInt(32);
            snoReverb = buffer.ReadInt(32);
            snoWeather = buffer.ReadInt(32);
            snoPresetWorld = buffer.ReadInt(32);
            Field15 = buffer.ReadInt(32);
            Field16 = buffer.ReadInt(32);
            Field17 = buffer.ReadInt(32);
            Field18 = buffer.ReadInt(32);
            tCachedValues = new SceneCachedValues();
            tCachedValues.Parse(buffer);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            Field1.Encode(buffer);
            for (int i = 0; i < arSnoLevelAreas.Length; i++) buffer.WriteInt(32, arSnoLevelAreas[i]);
            buffer.WriteInt(32, snoPrevWorld);
            buffer.WriteInt(32, Field4);
            buffer.WriteInt(32, snoPrevLevelArea);
            buffer.WriteInt(32, snoNextWorld);
            buffer.WriteInt(32, Field7);
            buffer.WriteInt(32, snoNextLevelArea);
            buffer.WriteInt(32, snoMusic);
            buffer.WriteInt(32, snoCombatMusic);
            buffer.WriteInt(32, snoAmbient);
            buffer.WriteInt(32, snoReverb);
            buffer.WriteInt(32, snoWeather);
            buffer.WriteInt(32, snoPresetWorld);
            buffer.WriteInt(32, Field15);
            buffer.WriteInt(32, Field16);
            buffer.WriteInt(32, Field17);
            buffer.WriteInt(32, Field18);
            tCachedValues.Encode(buffer);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("SceneSpecification:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            Field1.AsText(b, pad);
            b.Append(' ', pad);
            b.AppendLine("arSnoLevelAreas:");
            b.Append(' ', pad);
            b.AppendLine("{");
            for (int i = 0; i < arSnoLevelAreas.Length;)
            {
                b.Append(' ', pad + 1);
                for (int j = 0; j < 8 && i < arSnoLevelAreas.Length; j++, i++)
                {
                    b.Append("0x" + arSnoLevelAreas[i].ToString("X8") + ", ");
                }
                b.AppendLine();
            }
            b.Append(' ', pad);
            b.AppendLine("}");
            b.AppendLine();
            b.Append(' ', pad);
            b.AppendLine("snoPrevWorld: 0x" + snoPrevWorld.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("Field4: 0x" + Field4.ToString("X8") + " (" + Field4 + ")");
            b.Append(' ', pad);
            b.AppendLine("snoPrevLevelArea: 0x" + snoPrevLevelArea.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("snoNextWorld: 0x" + snoNextWorld.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("Field7: 0x" + Field7.ToString("X8") + " (" + Field7 + ")");
            b.Append(' ', pad);
            b.AppendLine("snoNextLevelArea: 0x" + snoNextLevelArea.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("snoMusic: 0x" + snoMusic.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("snoCombatMusic: 0x" + snoCombatMusic.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("snoAmbient: 0x" + snoAmbient.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("snoReverb: 0x" + snoReverb.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("snoWeather: 0x" + snoWeather.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("snoPresetWorld: 0x" + snoPresetWorld.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("Field15: 0x" + Field15.ToString("X8") + " (" + Field15 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field16: 0x" + Field16.ToString("X8") + " (" + Field16 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field17: 0x" + Field17.ToString("X8") + " (" + Field17 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field18: 0x" + Field18.ToString("X8") + " (" + Field18 + ")");
            tCachedValues.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class PRTransform
    {
        public Quaternion Field0;
        public Vector3D Field1;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = new Quaternion();
            Field0.Parse(buffer);
            Field1 = new Vector3D();
            Field1.Parse(buffer);
        }

        public void Encode(GameBitBuffer buffer)
        {
            Field0.Encode(buffer);
            Field1.Encode(buffer);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PRTransform:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            Field0.AsText(b, pad);
            Field1.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class WorldLocationMessageData
    {
        public float Field0;
        public PRTransform Field1;
        public int Field2;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadFloat32();
            Field1 = new PRTransform();
            Field1.Parse(buffer);
            Field2 = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteFloat32(Field0);
            Field1.Encode(buffer);
            buffer.WriteInt(32, Field2);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("WorldLocationMessageData:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: " + Field0.ToString("G"));
            Field1.AsText(b, pad);
            b.Append(' ', pad);
            b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class InventoryLocationMessageData
    {
        public int Field0;
        public int Field1;
        public IVector2D Field2;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(5) + (-1);
            Field2 = new IVector2D();
            Field2.Parse(buffer);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(5, Field1 - (-1));
            Field2.Encode(buffer);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("InventoryLocationMessageData:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            Field2.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class GBHandle
    {
        public int Field0;
        public int Field1;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(6) + (-2);
            Field1 = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(6, Field0 - (-2));
            buffer.WriteInt(32, Field1);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("GBHandle:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class VisualEquipment
    {
        // MaxLength = 8
        public VisualItem[] Field0;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = new VisualItem[8];
            for (int i = 0; i < Field0.Length; i++)
            {
                Field0[i] = new VisualItem();
                Field0[i].Parse(buffer);
            }
        }

        public void Encode(GameBitBuffer buffer)
        {
            for (int i = 0; i < Field0.Length; i++)
            {
                Field0[i].Encode(buffer);
            }
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("VisualEquipment:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0:");
            b.Append(' ', pad);
            b.AppendLine("{");
            for (int i = 0; i < Field0.Length; i++)
            {
                Field0[i].AsText(b, pad + 1);
                b.AppendLine();
            }
            b.Append(' ', pad);
            b.AppendLine("}");
            b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class ResolvedPortalDestination
    {
        public int /* sno */ snoWorld;
        public int Field1;
        public int /* sno */ snoDestLevelArea;

        public void Parse(GameBitBuffer buffer)
        {
            snoWorld = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            snoDestLevelArea = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoWorld);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, snoDestLevelArea);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ResolvedPortalDestination:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("snoWorld: 0x" + snoWorld.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad);
            b.AppendLine("snoDestLevelArea: 0x" + snoDestLevelArea.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class RareItemName
    {
        public bool Field0;
        public int /* sno */ snoAffixStringList;
        public int Field2;
        public int Field3;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadBool();
            snoAffixStringList = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteBool(Field0);
            buffer.WriteInt(32, snoAffixStringList);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(32, Field3);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("RareItemName:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: " + (Field0 ? "true" : "false"));
            b.Append(' ', pad);
            b.AppendLine("snoAffixStringList: 0x" + snoAffixStringList.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class NetAttributeKeyValue
    {
        public int? Field0;
        //public int Field1;
        public GameAttribute Attribute;
        public int Int;
        public float Float;

        public void Parse(GameBitBuffer buffer)
        {
            if (buffer.ReadBool())
            {
                Field0 = buffer.ReadInt(20);
            }
            int index = buffer.ReadInt(10) & 0xFFF;

            Attribute = GameAttribute.Attributes[index];
        }

        public void ParseValue(GameBitBuffer buffer)
        {
            switch (Attribute.EncodingType)
            {
                case GameAttributeEncoding.Int:
                    Int = buffer.ReadInt(Attribute.BitCount);
                    break;
                case GameAttributeEncoding.IntMinMax:
                    Int = buffer.ReadInt(Attribute.BitCount) + Attribute.Min;
                    break;
                case GameAttributeEncoding.Float16:
                    Float = buffer.ReadFloat16();
                    break;
                case GameAttributeEncoding.Float16Or32:
                    Float = buffer.ReadBool() ? buffer.ReadFloat16() : buffer.ReadFloat32();
                    break;
                default:
                    throw new Exception("bad voodoo");
            }
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteBool(Field0.HasValue);
            if (Field0.HasValue)
            {
                buffer.WriteInt(20, Field0.Value);
            }
            buffer.WriteInt(10, Attribute.Id);
        }

        public void EncodeValue(GameBitBuffer buffer)
        {
            switch (Attribute.EncodingType)
            {
                case GameAttributeEncoding.Int:
                    buffer.WriteInt(Attribute.BitCount, Int);
                    break;
                case GameAttributeEncoding.IntMinMax:
                    buffer.WriteInt(Attribute.BitCount, Int - Attribute.Min);
                    break;
                case GameAttributeEncoding.Float16:
                    buffer.WriteFloat16(Float);
                    break;
                case GameAttributeEncoding.Float16Or32:
                    if (Float >= 65536.0f || -65536.0f >= Float)
                    {
                        buffer.WriteBool(false);
                        buffer.WriteFloat32(Float);
                    }
                    else
                    {
                        buffer.WriteBool(true);
                        buffer.WriteFloat16(Float);
                    }
                    break;
                default:
                    throw new Exception("bad voodoo");
            }
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("NetAttributeKeyValue:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            if (Field0.HasValue)
            {
                b.Append(' ', pad);
                b.AppendLine("Field0.Value: 0x" + Field0.Value.ToString("X8") + " (" + Field0.Value + ")");
            }
            b.Append(' ', pad);
            b.Append(Attribute.Name);
            b.Append(" (" + Attribute.Id + "): ");

            if (Attribute.IsInteger)
                b.AppendLine("0x" + Int.ToString("X8") + " (" + Int + ")");
            else
                b.AppendLine(Float.ToString("G"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class PlayAnimationMessageSpec
    {
        public int Field0;
        public int /* sno */ Field1;
        public int Field2;
        public float Field3;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadFloat32();
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
            buffer.WriteFloat32(Field3);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayAnimationMessageSpec:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + Field1.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field3: " + Field3.ToString("G"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class DPathSinData
    {
        public float Field0;
        public float Field1;
        public float Field2;
        public float Field3;
        public float Field4;
        public float Field5;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadFloat32();
            Field1 = buffer.ReadFloat32();
            Field2 = buffer.ReadFloat32();
            Field3 = buffer.ReadFloat32();
            Field4 = buffer.ReadFloat32();
            Field5 = buffer.ReadFloat32();
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteFloat32(Field0);
            buffer.WriteFloat32(Field1);
            buffer.WriteFloat32(Field2);
            buffer.WriteFloat32(Field3);
            buffer.WriteFloat32(Field4);
            buffer.WriteFloat32(Field5);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("DPathSinData:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: " + Field0.ToString("G"));
            b.Append(' ', pad);
            b.AppendLine("Field1: " + Field1.ToString("G"));
            b.Append(' ', pad);
            b.AppendLine("Field2: " + Field2.ToString("G"));
            b.Append(' ', pad);
            b.AppendLine("Field3: " + Field3.ToString("G"));
            b.Append(' ', pad);
            b.AppendLine("Field4: " + Field4.ToString("G"));
            b.Append(' ', pad);
            b.AppendLine("Field5: " + Field5.ToString("G"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class NPCInteraction
    {
        public int Field0;
        public int Field1;
        public int Field2;
        public int Field3;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(4);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(2);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(4, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(2, Field3);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("NPCInteraction:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class WorldPlace
    {
        public Vector3D Field0;
        public int Field1;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = new Vector3D();
            Field0.Parse(buffer);
            Field1 = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            Field0.Encode(buffer);
            buffer.WriteInt(32, Field1);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("WorldPlace:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            Field0.AsText(b, pad);
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class GameSyncedData
    {
        public bool Field0;
        public int Field1;
        public int Field2;
        public int Field3;
        public int Field4;
        public int Field5;
        // MaxLength = 2
        public int[] Field6;
        // MaxLength = 2
        public int[] Field7;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadBool();
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(32);
            Field4 = buffer.ReadInt(32);
            Field5 = buffer.ReadInt(32);
            Field6 = new int[2];
            for (int i = 0; i < Field6.Length; i++) Field6[i] = buffer.ReadInt(32);
            Field7 = new int[2];
            for (int i = 0; i < Field7.Length; i++) Field7[i] = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteBool(Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(32, Field3);
            buffer.WriteInt(32, Field4);
            buffer.WriteInt(32, Field5);
            for (int i = 0; i < Field6.Length; i++) buffer.WriteInt(32, Field6[i]);
            for (int i = 0; i < Field7.Length; i++) buffer.WriteInt(32, Field7[i]);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("GameSyncedData:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: " + (Field0 ? "true" : "false"));
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field4: 0x" + Field4.ToString("X8") + " (" + Field4 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field5: 0x" + Field5.ToString("X8") + " (" + Field5 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field6:");
            b.Append(' ', pad);
            b.AppendLine("{");
            for (int i = 0; i < Field6.Length;)
            {
                b.Append(' ', pad + 1);
                for (int j = 0; j < 8 && i < Field6.Length; j++, i++)
                {
                    b.Append("0x" + Field6[i].ToString("X8") + ", ");
                }
                b.AppendLine();
            }
            b.Append(' ', pad);
            b.AppendLine("}");
            b.AppendLine();
            b.Append(' ', pad);
            b.AppendLine("Field7:");
            b.Append(' ', pad);
            b.AppendLine("{");
            for (int i = 0; i < Field7.Length;)
            {
                b.Append(' ', pad + 1);
                for (int j = 0; j < 8 && i < Field7.Length; j++, i++)
                {
                    b.Append("0x" + Field7[i].ToString("X8") + ", ");
                }
                b.AppendLine();
            }
            b.Append(' ', pad);
            b.AppendLine("}");
            b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class GameId
    {
        public long Field0;
        public long Field1;
        public long Field2;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt64(64);
            Field1 = buffer.ReadInt64(64);
            Field2 = buffer.ReadInt64(64);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt64(64, Field0);
            buffer.WriteInt64(64, Field1);
            buffer.WriteInt64(64, Field2);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("GameId:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: 0x" + Field0.ToString("X16"));
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + Field1.ToString("X16"));
            b.Append(' ', pad);
            b.AppendLine("Field2: 0x" + Field2.ToString("X16"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class EntityId
    {
        public long Field0;
        public long Field1;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt64(64);
            Field1 = buffer.ReadInt64(64);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt64(64, Field0);
            buffer.WriteInt64(64, Field1);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("EntityId:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: 0x" + Field0.ToString("X16"));
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + Field1.ToString("X16"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class AnimPreplayData
    {
        public int Field0;
        public int Field1;
        public int Field2;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("AnimPreplayData:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class InvLoc
    {
        public int Field0;
        public int Field1;
        public int Field2;
        public int Field3;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(5) + (-1);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(5, Field1 - (-1));
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(32, Field3);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("InvLoc:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class HotbarButtonData
    {
        public int /* sno */ m_snoPower;
        public int /* gbid */ m_gbidItem;

        public void Parse(GameBitBuffer buffer)
        {
            m_snoPower = buffer.ReadInt(32);
            m_gbidItem = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, m_snoPower);
            buffer.WriteInt(32, m_gbidItem);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("HotbarButtonData:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("m_snoPower: 0x" + m_snoPower.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("m_gbidItem: 0x" + m_gbidItem.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class PlayLineParams
    {
        public int /* sno */ snoConversation;
        public int Field1;
        public bool Field2;
        public int Field3;
        public int Field4;
        public int Field5;
        public int Field6;
        public int Field7;
        public int Field8;
        public int /* sno */ snoSpeakerActor;
        public string Field10;
        public int Field11;
        public int Field12;
        public int Field13;
        public int Field14;
        public int Field15;

        public void Parse(GameBitBuffer buffer)
        {
            snoConversation = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadBool();
            Field3 = buffer.ReadInt(32);
            Field4 = buffer.ReadInt(32);
            Field5 = buffer.ReadInt(32);
            Field6 = buffer.ReadInt(32);
            Field7 = buffer.ReadInt(32);
            Field8 = buffer.ReadInt(32);
            snoSpeakerActor = buffer.ReadInt(32);
            Field10 = buffer.ReadCharArray(49);
            Field11 = buffer.ReadInt(32);
            Field12 = buffer.ReadInt(32);
            Field13 = buffer.ReadInt(32);
            Field14 = buffer.ReadInt(32);
            Field15 = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoConversation);
            buffer.WriteInt(32, Field1);
            buffer.WriteBool(Field2);
            buffer.WriteInt(32, Field3);
            buffer.WriteInt(32, Field4);
            buffer.WriteInt(32, Field5);
            buffer.WriteInt(32, Field6);
            buffer.WriteInt(32, Field7);
            buffer.WriteInt(32, Field8);
            buffer.WriteInt(32, snoSpeakerActor);
            buffer.WriteCharArray(49, Field10);
            buffer.WriteInt(32, Field11);
            buffer.WriteInt(32, Field12);
            buffer.WriteInt(32, Field13);
            buffer.WriteInt(32, Field14);
            buffer.WriteInt(32, Field15);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayLineParams:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("snoConversation: 0x" + snoConversation.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field2: " + (Field2 ? "true" : "false"));
            b.Append(' ', pad);
            b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field4: 0x" + Field4.ToString("X8") + " (" + Field4 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field5: 0x" + Field5.ToString("X8") + " (" + Field5 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field6: 0x" + Field6.ToString("X8") + " (" + Field6 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field7: 0x" + Field7.ToString("X8") + " (" + Field7 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field8: 0x" + Field8.ToString("X8") + " (" + Field8 + ")");
            b.Append(' ', pad);
            b.AppendLine("snoSpeakerActor: 0x" + snoSpeakerActor.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("Field10: \"" + Field10 + "\"");
            b.Append(' ', pad);
            b.AppendLine("Field11: 0x" + Field11.ToString("X8") + " (" + Field11 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field12: 0x" + Field12.ToString("X8") + " (" + Field12 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field13: 0x" + Field13.ToString("X8") + " (" + Field13 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field14: 0x" + Field14.ToString("X8") + " (" + Field14 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field15: 0x" + Field15.ToString("X8") + " (" + Field15 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class SNOName
    {
        public int /* sno_group */ Field0;
        public int /* snoname_handle */ Field1;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("SNOName:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: 0x" + Field0.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + Field1.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class ActiveEvent
    {
        public int /* sno */ snoTimedEvent;
        public int Field1;
        public int Field2;

        public void Parse(GameBitBuffer buffer)
        {
            snoTimedEvent = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoTimedEvent);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ActiveEvent:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("snoTimedEvent: 0x" + snoTimedEvent.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class RGBAColor
    {
        public byte Field0;
        public byte Field1;
        public byte Field2;
        public byte Field3;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = (byte) buffer.ReadInt(8);
            Field1 = (byte) buffer.ReadInt(8);
            Field2 = (byte) buffer.ReadInt(8);
            Field3 = (byte) buffer.ReadInt(8);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(8, Field0);
            buffer.WriteInt(8, Field1);
            buffer.WriteInt(8, Field2);
            buffer.WriteInt(8, Field3);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("RGBAColor:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: 0x" + Field0.ToString("X2"));
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + Field1.ToString("X2"));
            b.Append(' ', pad);
            b.AppendLine("Field2: 0x" + Field2.ToString("X2"));
            b.Append(' ', pad);
            b.AppendLine("Field3: 0x" + Field3.ToString("X2"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class PlayerSavedData
    {
        // MaxLength = 9
        public HotbarButtonData[] Field0;
        // MaxLength = 15
        public SkillKeyMapping[] Field1;
        public int /* time */ Field2;
        public int Field3;
        public HirelingSavedData Field4;
        public int Field5;
        public LearnedLore Field6;
        // MaxLength = 6
        public int /* sno */[] snoActiveSkills;
        // MaxLength = 3
        public int /* sno */[] snoTraits;
        public SavePointData Field9;
        // MaxLength = 64
        public int /* sno */[] m_SeenTutorials;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = new HotbarButtonData[9];
            for (int i = 0; i < Field0.Length; i++)
            {
                Field0[i] = new HotbarButtonData();
                Field0[i].Parse(buffer);
            }
            Field1 = new SkillKeyMapping[15];
            for (int i = 0; i < Field1.Length; i++)
            {
                Field1[i] = new SkillKeyMapping();
                Field1[i].Parse(buffer);
            }
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(32);
            Field4 = new HirelingSavedData();
            Field4.Parse(buffer);
            Field5 = buffer.ReadInt(32);
            Field6 = new LearnedLore();
            Field6.Parse(buffer);
            snoActiveSkills = new int /* sno */[6];
            for (int i = 0; i < snoActiveSkills.Length; i++) snoActiveSkills[i] = buffer.ReadInt(32);
            snoTraits = new int /* sno */[3];
            for (int i = 0; i < snoTraits.Length; i++) snoTraits[i] = buffer.ReadInt(32);
            Field9 = new SavePointData();
            Field9.Parse(buffer);
            m_SeenTutorials = new int /* sno */[64];
            for (int i = 0; i < m_SeenTutorials.Length; i++) m_SeenTutorials[i] = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            for (int i = 0; i < Field0.Length; i++)
            {
                Field0[i].Encode(buffer);
            }
            for (int i = 0; i < Field1.Length; i++)
            {
                Field1[i].Encode(buffer);
            }
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(32, Field3);
            Field4.Encode(buffer);
            buffer.WriteInt(32, Field5);
            Field6.Encode(buffer);
            for (int i = 0; i < snoActiveSkills.Length; i++) buffer.WriteInt(32, snoActiveSkills[i]);
            for (int i = 0; i < snoTraits.Length; i++) buffer.WriteInt(32, snoTraits[i]);
            Field9.Encode(buffer);
            for (int i = 0; i < m_SeenTutorials.Length; i++) buffer.WriteInt(32, m_SeenTutorials[i]);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayerSavedData:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0:");
            b.Append(' ', pad);
            b.AppendLine("{");
            for (int i = 0; i < Field0.Length; i++)
            {
                Field0[i].AsText(b, pad + 1);
                b.AppendLine();
            }
            b.Append(' ', pad);
            b.AppendLine("}");
            b.AppendLine();
            b.Append(' ', pad);
            b.AppendLine("Field1:");
            b.Append(' ', pad);
            b.AppendLine("{");
            for (int i = 0; i < Field1.Length; i++)
            {
                Field1[i].AsText(b, pad + 1);
                b.AppendLine();
            }
            b.Append(' ', pad);
            b.AppendLine("}");
            b.AppendLine();
            b.Append(' ', pad);
            b.AppendLine("Field2: 0x" + Field2.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            Field4.AsText(b, pad);
            b.Append(' ', pad);
            b.AppendLine("Field5: 0x" + Field5.ToString("X8") + " (" + Field5 + ")");
            Field6.AsText(b, pad);
            b.Append(' ', pad);
            b.AppendLine("snoActiveSkills:");
            b.Append(' ', pad);
            b.AppendLine("{");
            for (int i = 0; i < snoActiveSkills.Length;)
            {
                b.Append(' ', pad + 1);
                for (int j = 0; j < 8 && i < snoActiveSkills.Length; j++, i++)
                {
                    b.Append("0x" + snoActiveSkills[i].ToString("X8") + ", ");
                }
                b.AppendLine();
            }
            b.Append(' ', pad);
            b.AppendLine("}");
            b.AppendLine();
            b.Append(' ', pad);
            b.AppendLine("snoTraits:");
            b.Append(' ', pad);
            b.AppendLine("{");
            for (int i = 0; i < snoTraits.Length;)
            {
                b.Append(' ', pad + 1);
                for (int j = 0; j < 8 && i < snoTraits.Length; j++, i++)
                {
                    b.Append("0x" + snoTraits[i].ToString("X8") + ", ");
                }
                b.AppendLine();
            }
            b.Append(' ', pad);
            b.AppendLine("}");
            b.AppendLine();
            Field9.AsText(b, pad);
            b.Append(' ', pad);
            b.AppendLine("m_SeenTutorials:");
            b.Append(' ', pad);
            b.AppendLine("{");
            for (int i = 0; i < m_SeenTutorials.Length;)
            {
                b.Append(' ', pad + 1);
                for (int j = 0; j < 8 && i < m_SeenTutorials.Length; j++, i++)
                {
                    b.Append("0x" + m_SeenTutorials[i].ToString("X8") + ", ");
                }
                b.AppendLine();
            }
            b.Append(' ', pad);
            b.AppendLine("}");
            b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class PlayerQuestRewardHistoryEntry
    {
        public int /* sno */ snoQuest;
        public int Field1;

        public enum eField2
        {
            Normal = 0,
            Nightmare = 1,
            Hell = 2,
            Inferno = 3,
        }

        public eField2 Field2;

        public void Parse(GameBitBuffer buffer)
        {
            snoQuest = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = (eField2) buffer.ReadInt(2);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoQuest);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(2, (int) Field2);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayerQuestRewardHistoryEntry:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("snoQuest: 0x" + snoQuest.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field2: " + Field2.ToString());
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class IVector2D
    {
        public int Field0;
        public int Field1;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("IVector2D:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class SceneCachedValues
    {
        public int Field0;
        public int Field1;
        public int Field2;
        public AABB Field3;
        public AABB Field4;
        // MaxLength = 4
        public int[] Field5;
        public int Field6;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
            Field3 = new AABB();
            Field3.Parse(buffer);
            Field4 = new AABB();
            Field4.Parse(buffer);
            Field5 = new int[4];
            for (int i = 0; i < Field5.Length; i++) Field5[i] = buffer.ReadInt(32);
            Field6 = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
            Field3.Encode(buffer);
            Field4.Encode(buffer);
            for (int i = 0; i < Field5.Length; i++) buffer.WriteInt(32, Field5[i]);
            buffer.WriteInt(32, Field6);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("SceneCachedValues:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            Field3.AsText(b, pad);
            Field4.AsText(b, pad);
            b.Append(' ', pad);
            b.AppendLine("Field5:");
            b.Append(' ', pad);
            b.AppendLine("{");
            for (int i = 0; i < Field5.Length;)
            {
                b.Append(' ', pad + 1);
                for (int j = 0; j < 8 && i < Field5.Length; j++, i++)
                {
                    b.Append("0x" + Field5[i].ToString("X8") + ", ");
                }
                b.AppendLine();
            }
            b.Append(' ', pad);
            b.AppendLine("}");
            b.AppendLine();
            b.Append(' ', pad);
            b.AppendLine("Field6: 0x" + Field6.ToString("X8") + " (" + Field6 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class Quaternion
    {
        public float Field0;
        public Vector3D Field1;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadFloat32();
            Field1 = new Vector3D();
            Field1.Parse(buffer);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteFloat32(Field0);
            Field1.Encode(buffer);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("Quaternion:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: " + Field0.ToString("G"));
            Field1.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class VisualItem
    {
        public int /* gbid */ Field0;
        public int Field1;
        public int Field2;
        public int Field3;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(5);
            Field2 = buffer.ReadInt(4);
            Field3 = buffer.ReadInt(5) + (-1);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(5, Field1);
            buffer.WriteInt(4, Field2);
            buffer.WriteInt(5, Field3 - (-1));
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("VisualItem:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: 0x" + Field0.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class SkillKeyMapping
    {
        public int /* sno */ Power;
        public int Field1;
        public int Field2;

        public void Parse(GameBitBuffer buffer)
        {
            Power = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(4);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Power);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(4, Field2);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("SkillKeyMapping:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Power: 0x" + Power.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class HirelingSavedData
    {
        // MaxLength = 4
        public HirelingInfo[] Field0;
        public int Field1;
        public int Field2;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = new HirelingInfo[4];
            for (int i = 0; i < Field0.Length; i++)
            {
                Field0[i] = new HirelingInfo();
                Field0[i].Parse(buffer);
            }
            Field1 = buffer.ReadInt(2);
            Field2 = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            for (int i = 0; i < Field0.Length; i++)
            {
                Field0[i].Encode(buffer);
            }
            buffer.WriteInt(2, Field1);
            buffer.WriteInt(32, Field2);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("HirelingSavedData:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0:");
            b.Append(' ', pad);
            b.AppendLine("{");
            for (int i = 0; i < Field0.Length; i++)
            {
                Field0[i].AsText(b, pad + 1);
                b.AppendLine();
            }
            b.Append(' ', pad);
            b.AppendLine("}");
            b.AppendLine();
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class LearnedLore
    {
        public int Field0;
        // MaxLength = 256
        public int /* sno */[] m_snoLoreLearned;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            m_snoLoreLearned = new int /* sno */[256];
            for (int i = 0; i < m_snoLoreLearned.Length; i++) m_snoLoreLearned[i] = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            for (int i = 0; i < m_snoLoreLearned.Length; i++) buffer.WriteInt(32, m_snoLoreLearned[i]);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("LearnedLore:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad);
            b.AppendLine("m_snoLoreLearned:");
            b.Append(' ', pad);
            b.AppendLine("{");
            for (int i = 0; i < m_snoLoreLearned.Length;)
            {
                b.Append(' ', pad + 1);
                for (int j = 0; j < 8 && i < m_snoLoreLearned.Length; j++, i++)
                {
                    b.Append("0x" + m_snoLoreLearned[i].ToString("X8") + ", ");
                }
                b.AppendLine();
            }
            b.Append(' ', pad);
            b.AppendLine("}");
            b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class SavePointData
    {
        public int /* sno */ snoWorld;
        public int Field1;

        public void Parse(GameBitBuffer buffer)
        {
            snoWorld = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoWorld);
            buffer.WriteInt(32, Field1);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("SavePointData:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("snoWorld: 0x" + snoWorld.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class AABB
    {
        public Vector3D Field0;
        public Vector3D Field1;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = new Vector3D();
            Field0.Parse(buffer);
            Field1 = new Vector3D();
            Field1.Parse(buffer);
        }

        public void Encode(GameBitBuffer buffer)
        {
            Field0.Encode(buffer);
            Field1.Encode(buffer);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("AABB:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            Field0.AsText(b, pad);
            Field1.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class HirelingInfo
    {
        public int Field0;
        public int Field1;
        public int Field2;
        public int Field3;
        public bool Field4;
        public int Field5;
        public int Field6;
        public int Field7;
        public int Field8;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(2);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(7);
            Field3 = buffer.ReadInt(32);
            Field4 = buffer.ReadBool();
            Field5 = buffer.ReadInt(32);
            Field6 = buffer.ReadInt(32);
            Field7 = buffer.ReadInt(32);
            Field8 = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(2, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(7, Field2);
            buffer.WriteInt(32, Field3);
            buffer.WriteBool(Field4);
            buffer.WriteInt(32, Field5);
            buffer.WriteInt(32, Field6);
            buffer.WriteInt(32, Field7);
            buffer.WriteInt(32, Field8);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("HirelingInfo:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field4: " + (Field4 ? "true" : "false"));
            b.Append(' ', pad);
            b.AppendLine("Field5: 0x" + Field5.ToString("X8") + " (" + Field5 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field6: 0x" + Field6.ToString("X8") + " (" + Field6 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field7: 0x" + Field7.ToString("X8") + " (" + Field7 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field8: 0x" + Field8.ToString("X8") + " (" + Field8 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
