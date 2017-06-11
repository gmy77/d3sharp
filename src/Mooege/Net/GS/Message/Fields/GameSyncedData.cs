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

namespace Mooege.Net.GS.Message.Fields
{
    public class GameSyncedData
    {
        public int Field0;
        public int Field1;
        public int Field2;
        public int Field3;
        public int Field4;
        public int Field5;
        public int Field6;
        // MaxLength = 2
        public int[] Field7;
        // MaxLength = 2
        public int[] Field8;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(2);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(32);
            Field4 = buffer.ReadInt(32);
            Field5 = buffer.ReadInt(32);
            Field6 = buffer.ReadInt(32);
            Field7 = new int[2];
            for (int i = 0; i < Field7.Length; i++) Field7[i] = buffer.ReadInt(32);
            Field8 = new int[2];
            for (int i = 0; i < Field8.Length; i++) Field8[i] = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(2,Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(32, Field3);
            buffer.WriteInt(32, Field4);
            buffer.WriteInt(32, Field5);
            buffer.WriteInt(32, Field6);
            for (int i = 0; i < Field7.Length; i++) buffer.WriteInt(32, Field7[i]);
            for (int i = 0; i < Field8.Length; i++) buffer.WriteInt(32, Field8[i]);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("GameSyncedData:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: 0x" + Field0);
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
            b.AppendLine("Field6: 0x" + Field6.ToString("X8") + " (" + Field6 + ")");
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
            b.Append(' ', pad);
            b.AppendLine("Field8:");
            b.Append(' ', pad);
            b.AppendLine("{");
            for (int i = 0; i < Field8.Length;)
            {
                b.Append(' ', pad + 1);
                for (int j = 0; j < 8 && i < Field8.Length; j++, i++)
                {
                    b.Append("0x" + Field8[i].ToString("X8") + ", ");
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
}