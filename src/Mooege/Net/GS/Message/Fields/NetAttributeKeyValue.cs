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

using System;
using System.Text;

namespace Mooege.Net.GS.Message.Fields
{
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
                    Int = buffer.ReadInt(Attribute.BitCount) + Attribute.Min.Value;
                    break;
                case GameAttributeEncoding.Float16:
                    Float = buffer.ReadFloat16();
                    break;
                case GameAttributeEncoding.Float16Or32:
                    Float = buffer.ReadBool() ? buffer.ReadFloat16() : buffer.ReadFloat32();
                    break;
                case GameAttributeEncoding.Float32:
                    Float = buffer.ReadFloat32();
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
                    buffer.WriteInt(Attribute.BitCount, Int - Attribute.Min.Value);
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
                case GameAttributeEncoding.Float32:
                    buffer.WriteFloat32(Float);
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
}