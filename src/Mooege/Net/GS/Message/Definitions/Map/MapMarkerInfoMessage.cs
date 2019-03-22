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
        public int /* sno */ m_snoStringList;
        public int Field4;
        public float Field5;
        public float Field6;
        public float Field7;
        public int Field8;
        public bool Field9;
        public bool Field10;
        public bool Field11;
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
            m_snoStringList = buffer.ReadInt(32);
            Field4 = buffer.ReadInt(32);
            Field5 = buffer.ReadFloat32();
            Field6 = buffer.ReadFloat32();
            Field7 = buffer.ReadFloat32();
            Field8 = buffer.ReadInt(32);
            Field9 = buffer.ReadBool();
            Field10 = buffer.ReadBool();
            Field11 = buffer.ReadBool();
            Field12 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            Field1.Encode(buffer);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(32, m_snoStringList);
            buffer.WriteInt(32, Field4);
            buffer.WriteFloat32(Field5);
            buffer.WriteFloat32(Field6);
            buffer.WriteFloat32(Field7);
            buffer.WriteInt(32, Field8);
            buffer.WriteBool(Field9);
            buffer.WriteBool(Field10);
            buffer.WriteBool(Field11);
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
            b.Append(' ', pad); b.AppendLine("m_snoStringList: 0x" + m_snoStringList.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8") + " (" + Field4 + ")");
            b.Append(' ', pad); b.AppendLine("Field5: " + Field5.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field6: " + Field6.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field7: " + Field7.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field8: 0x" + Field8.ToString("X8") + " (" + Field8 + ")");
            b.Append(' ', pad); b.AppendLine("Field9: " + (Field9 ? "true" : "false"));
            b.Append(' ', pad); b.AppendLine("Field10: " + (Field10 ? "true" : "false"));
            b.Append(' ', pad); b.AppendLine("Field11: " + (Field11 ? "true" : "false"));
            b.Append(' ', pad); b.AppendLine("Field12: 0x" + Field12.ToString("X8") + " (" + Field12 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}