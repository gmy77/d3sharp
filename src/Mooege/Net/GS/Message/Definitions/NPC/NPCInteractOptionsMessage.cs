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

namespace Mooege.Net.GS.Message.Definitions.NPC
{
    public class NPCInteractOptionsMessage : GameMessage
    {
        public int Field0;
        // MaxLength = 20
        public NPCInteraction[] tNPCInteraction;
        public int Field2;




        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            tNPCInteraction = new NPCInteraction[buffer.ReadInt(5)];
            for (int i = 0; i < tNPCInteraction.Length; i++) { tNPCInteraction[i] = new NPCInteraction(); tNPCInteraction[i].Parse(buffer); }
            Field2 = buffer.ReadInt(2);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(5, tNPCInteraction.Length);
            for (int i = 0; i < tNPCInteraction.Length; i++) { tNPCInteraction[i].Encode(buffer); }
            buffer.WriteInt(2, Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("NPCInteractOptionsMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("tNPCInteraction:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < tNPCInteraction.Length; i++) { tNPCInteraction[i].AsText(b, pad + 1); b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}