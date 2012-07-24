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

namespace Mooege.Net.GS.Message.Definitions.Misc
{
    [Message(new[] { Opcodes.GenericBlobMessage1, Opcodes.GenericBlobMessage2, Opcodes.GenericBlobMessage3, Opcodes.GenericBlobMessage4, Opcodes.GenericBlobMessage5,
                     Opcodes.GenericBlobMessage15 })]
    public class GenericBlobMessage : GameMessage
    {
        public byte[] Data;

        public GenericBlobMessage(Opcodes opcode):base(opcode) { }

        public override void Parse(GameBitBuffer buffer)
        {
            Data = buffer.ReadBlob(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteBlob(32, Data);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("GenericBlobMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Length: 0x" + Data.Length.ToString("X8") + " (" + Data.Length + ")");
            for (int i = 0; i < Data.Length; i += 16)
            {

                b.Append(' ', pad);
                b.Append(i.ToString("X4"));
                b.Append(' ');

                int off = i;
                for (int j = 0; j < 8; j++, off++)
                {
                    if (off < Data.Length)
                    {
                        b.Append(Data[off].ToString("X2"));
                        b.Append(' ');
                    }
                    else
                    {
                        b.Append(' '); b.Append(' '); b.Append(' ');
                    }
                }
                b.Append(' ');
                off = i + 8;
                for (int j = 0; j < 8; j++, off++)
                {
                    if (off < Data.Length)
                    {
                        b.Append(Data[off].ToString("X2"));
                        b.Append(' ');
                    }
                    else
                    {
                        b.Append(' '); b.Append(' '); b.Append(' ');
                    }
                }

                b.Append(' ');

                off = i;
                for (int j = 0; j < 8; j++, off++)
                {
                    if (off < Data.Length)
                    {
                        if (Data[off] >= 20 && Data[off] < 128)
                            b.Append((char)Data[off]);
                        else
                            b.Append('.');
                    }
                    else
                        b.Append(' ');
                }
                b.Append(' ');
                off = i + 8;
                for (int j = 0; j < 8; j++, off++)
                {
                    if (off < Data.Length)
                    {
                        if (Data[off] >= 20 && Data[off] < 128)
                            b.Append((char)Data[off]);
                        else
                            b.Append('.');
                    }
                    else
                        b.Append(' ');
                }
                b.AppendLine();
            }
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}