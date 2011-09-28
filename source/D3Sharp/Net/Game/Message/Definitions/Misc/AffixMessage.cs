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

namespace D3Sharp.Net.Game.Message.Definitions.Misc
{
    public class AffixMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        // MaxLength = 32
        public int /* gbid */[] aAffixGBIDs;




        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(2);
            aAffixGBIDs = new int /* gbid */[buffer.ReadInt(6)];
            for (int i = 0; i < aAffixGBIDs.Length; i++) aAffixGBIDs[i] = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(2, Field1);
            buffer.WriteInt(6, aAffixGBIDs.Length);
            for (int i = 0; i < aAffixGBIDs.Length; i++) buffer.WriteInt(32, aAffixGBIDs[i]);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("AffixMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("aAffixGBIDs:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < aAffixGBIDs.Length; ) { b.Append(' ', pad + 1); for (int j = 0; j < 8 && i < aAffixGBIDs.Length; j++, i++) { b.Append("0x" + aAffixGBIDs[i].ToString("X8") + ", "); } b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}