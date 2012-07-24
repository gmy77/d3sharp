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

namespace Mooege.Net.GS.Message.Definitions.Game
{
    [Message(Opcodes.CreateBNetGameMessage)]
    public class CreateBNetGameMessage : GameMessage
    {
        public string Field0;
        public int Field1;
        public int Field2;
        public int /* sno */ Field3;
        public int Field4;
        public bool Field5;
        public int /* sno */ Field6;
        public int Field7;
        public int Field8;
        public int Field9;
        public int Field10;
        public short Field11;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadCharArray(33);
            Field1 = buffer.ReadInt(3) + (-1);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(32);
            Field4 = buffer.ReadInt(32);
            Field5 = buffer.ReadBool();
            Field6 = buffer.ReadInt(32);
            Field7 = buffer.ReadInt(16);
            Field8 = buffer.ReadInt(3) + (1);
            Field9 = buffer.ReadInt(32);
            Field10 = buffer.ReadInt(32);
            Field11 = (short)buffer.ReadInt(16);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteCharArray(33, Field0);
            buffer.WriteInt(3, Field1 - (-1));
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(32, Field3);
            buffer.WriteInt(32, Field4);
            buffer.WriteBool(Field5);
            buffer.WriteInt(32, Field6);
            buffer.WriteInt(16, Field7);
            buffer.WriteInt(3, Field8 - (1));
            buffer.WriteInt(32, Field9);
            buffer.WriteInt(32, Field10);
            buffer.WriteInt(16, Field11);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("CreateBNetGameMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: \"" + Field0 + "\"");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8") + " (" + Field4 + ")");
            b.Append(' ', pad); b.AppendLine("Field5: " + (Field5 ? "true" : "false"));
            b.Append(' ', pad); b.AppendLine("Field6: 0x" + Field6.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field7: 0x" + Field7.ToString("X8") + " (" + Field7 + ")");
            b.Append(' ', pad); b.AppendLine("Field8: 0x" + Field8.ToString("X8") + " (" + Field8 + ")");
            b.Append(' ', pad); b.AppendLine("Field9: 0x" + Field9.ToString("X8") + " (" + Field9 + ")");
            b.Append(' ', pad); b.AppendLine("Field10: 0x" + Field10.ToString("X8") + " (" + Field10 + ")");
            b.Append(' ', pad); b.AppendLine("Field11: 0x" + Field11.ToString("X4"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}