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

using System.Text;
using Mooege.Net.GS.Message.Fields;

namespace Mooege.Net.GS.Message.Definitions.Map
{
    [Message(Opcodes.MapMarkerInfoMessage)]
    public class MapMarkerInfoMessage : GameMessage
    {
        public int Field0;
        public WorldPlace Field1;
        public int Field2;
        public int Field3;
        public int /* sno */ m_snoStringList;
        public int Field5;
        public bool Field6;
        public bool Field7;
        public bool Field8;
        public float Field9;
        public float Field10;
        public float Field11;
        public int Field12;

        public MapMarkerInfoMessage()
            : base(Opcodes.MapMarkerInfoMessage)
        { }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = new WorldPlace();
            Field1.Parse(buffer);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(32);
            m_snoStringList = buffer.ReadInt(32);
            Field5 = buffer.ReadInt(32);
            Field6 = buffer.ReadBool();
            Field7 = buffer.ReadBool();
            Field8 = buffer.ReadBool();
            Field9 = buffer.ReadFloat32();
            Field10 = buffer.ReadFloat32();
            Field11 = buffer.ReadFloat32();
            Field12 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            Field1.Encode(buffer);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(32, Field3);
            buffer.WriteInt(32, m_snoStringList);
            buffer.WriteInt(32, Field5);
            buffer.WriteBool(Field6);
            buffer.WriteBool(Field7);
            buffer.WriteBool(Field8);
            buffer.WriteFloat32(Field9);
            buffer.WriteFloat32(Field10);
            buffer.WriteFloat32(Field11);
            buffer.WriteInt(32, Field12);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("MapMarkerInfoMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            Field1.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', pad); b.AppendLine("m_snoStringList: 0x" + m_snoStringList.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field5: 0x" + Field5.ToString("X8") + " (" + Field5 + ")");
            b.Append(' ', pad); b.AppendLine("Field6: " + (Field6 ? "true" : "false"));
            b.Append(' ', pad); b.AppendLine("Field7: " + (Field7 ? "true" : "false"));
            b.Append(' ', pad); b.AppendLine("Field8: " + (Field8 ? "true" : "false"));
            b.Append(' ', pad); b.AppendLine("Field9: " + Field9.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field10: " + Field10.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field11: " + Field11.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field12: 0x" + Field12.ToString("X8") + " (" + Field12 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}