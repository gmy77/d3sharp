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
    [Message(Opcodes.SalvageResultsMessage)]
    public class SalvageResultsMessage : GameMessage
    {
        public int /* gbid */ gbidOriginalItem;
        public int Field1;
        public int Field2;
        // MaxLength = 10
        public int /* gbid */[] gbidNewItems;

        public SalvageResultsMessage() : base(Opcodes.SalvageResultsMessage) { }

        public override void Parse(GameBitBuffer buffer)
        {
            gbidOriginalItem = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(4) + (-1);
            Field2 = buffer.ReadInt(32);
            gbidNewItems = new int /* gbid */[10];
            for (int i = 0; i < gbidNewItems.Length; i++) gbidNewItems[i] = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, gbidOriginalItem);
            buffer.WriteInt(4, Field1 - (-1));
            buffer.WriteInt(32, Field2);
            for (int i = 0; i < gbidNewItems.Length; i++) buffer.WriteInt(32, gbidNewItems[i]);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("SalvageResultsMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("gbidOriginalItem: 0x" + gbidOriginalItem.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("gbidNewItems:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < gbidNewItems.Length; ) { b.Append(' ', pad + 1); for (int j = 0; j < 8 && i < gbidNewItems.Length; j++, i++) { b.Append("0x" + gbidNewItems[i].ToString("X8") + ", "); } b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}