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
    public class LearnedLore
    {
        public int Count;
        // MaxLength = 256
        public int /* sno */[] m_snoLoreLearned;

        public void Parse(GameBitBuffer buffer)
        {
            Count = buffer.ReadInt(32);
            m_snoLoreLearned = new int /* sno */[256];
            for (int i = 0; i < m_snoLoreLearned.Length; i++) m_snoLoreLearned[i] = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Count);
            for (int i = 0; i < m_snoLoreLearned.Length; i++) buffer.WriteInt(32, m_snoLoreLearned[i]);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("LearnedLore:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Count: 0x" + Count.ToString("X8") + " (" + Count + ")");
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
}