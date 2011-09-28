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
using System.Text;
using D3Sharp.Net.Game.Message.Fields;

namespace D3Sharp.Net.Game.Message.Definitions.Misc
{
    public class TrickleMessage : GameMessage
    {
        public int Field0;
        public int /* sno */ Field1;
        public WorldPlace Field2;
        public int? Field3;
        public int /* sno */ Field4;
        public float? Field5;
        public int Field6;
        public int Field7;
        public int? Field8;
        public int? Field9;
        public int? Field10;
        public int? Field11;
        public int? Field12;
        public float? Field13;
        public float? Field14;




        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = new WorldPlace();
            Field2.Parse(buffer);
            if (buffer.ReadBool())
            {
                Field3 = buffer.ReadInt(4) + (-1);
            }
            Field4 = buffer.ReadInt(32);
            if (buffer.ReadBool())
            {
                Field5 = buffer.ReadFloat32();
            }
            Field6 = buffer.ReadInt(4);
            Field7 = buffer.ReadInt(6);
            if (buffer.ReadBool())
            {
                Field8 = buffer.ReadInt(32);
            }
            if (buffer.ReadBool())
            {
                Field9 = buffer.ReadInt(32);
            }
            if (buffer.ReadBool())
            {
                Field10 = buffer.ReadInt(32);
            }
            if (buffer.ReadBool())
            {
                Field11 = buffer.ReadInt(32);
            }
            if (buffer.ReadBool())
            {
                Field12 = buffer.ReadInt(32);
            }
            if (buffer.ReadBool())
            {
                Field13 = buffer.ReadFloat32();
            }
            if (buffer.ReadBool())
            {
                Field14 = buffer.ReadFloat32();
            }
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            Field2.Encode(buffer);
            buffer.WriteBool(Field3.HasValue);
            if (Field3.HasValue)
            {
                buffer.WriteInt(4, Field3.Value - (-1));
            }
            buffer.WriteInt(32, Field4);
            buffer.WriteBool(Field5.HasValue);
            if (Field5.HasValue)
            {
                buffer.WriteFloat32(Field5.Value);
            }
            buffer.WriteInt(4, Field6);
            buffer.WriteInt(6, Field7);
            buffer.WriteBool(Field8.HasValue);
            if (Field8.HasValue)
            {
                buffer.WriteInt(32, Field8.Value);
            }
            buffer.WriteBool(Field9.HasValue);
            if (Field9.HasValue)
            {
                buffer.WriteInt(32, Field9.Value);
            }
            buffer.WriteBool(Field10.HasValue);
            if (Field10.HasValue)
            {
                buffer.WriteInt(32, Field10.Value);
            }
            buffer.WriteBool(Field11.HasValue);
            if (Field11.HasValue)
            {
                buffer.WriteInt(32, Field11.Value);
            }
            buffer.WriteBool(Field12.HasValue);
            if (Field12.HasValue)
            {
                buffer.WriteInt(32, Field12.Value);
            }
            buffer.WriteBool(Field13.HasValue);
            if (Field13.HasValue)
            {
                buffer.WriteFloat32(Field13.Value);
            }
            buffer.WriteBool(Field14.HasValue);
            if (Field14.HasValue)
            {
                buffer.WriteFloat32(Field14.Value);
            }
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("TrickleMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8"));
            Field2.AsText(b, pad);
            if (Field3.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field3.Value: 0x" + Field3.Value.ToString("X8") + " (" + Field3.Value + ")");
            }
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8"));
            if (Field5.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field5.Value: " + Field5.Value.ToString("G"));
            }
            b.Append(' ', pad); b.AppendLine("Field6: 0x" + Field6.ToString("X8") + " (" + Field6 + ")");
            b.Append(' ', pad); b.AppendLine("Field7: 0x" + Field7.ToString("X8") + " (" + Field7 + ")");
            if (Field8.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field8.Value: 0x" + Field8.Value.ToString("X8") + " (" + Field8.Value + ")");
            }
            if (Field9.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field9.Value: 0x" + Field9.Value.ToString("X8") + " (" + Field9.Value + ")");
            }
            if (Field10.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field10.Value: 0x" + Field10.Value.ToString("X8") + " (" + Field10.Value + ")");
            }
            if (Field11.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field11.Value: 0x" + Field11.Value.ToString("X8") + " (" + Field11.Value + ")");
            }
            if (Field12.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field12.Value: 0x" + Field12.Value.ToString("X8") + " (" + Field12.Value + ")");
            }
            if (Field13.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field13.Value: " + Field13.Value.ToString("G"));
            }
            if (Field14.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field14.Value: " + Field14.Value.ToString("G"));
            }
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}