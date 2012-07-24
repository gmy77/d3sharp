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

namespace Mooege.Net.GS.Message.Definitions.Hero
{
    [Message(Opcodes.CreateHeroResultMessage)]
    public class CreateHeroResultMessage : GameMessage
    {
        public int Field0;
        public EntityId Field1;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(2) + (-1);
            Field1 = new EntityId();
            Field1.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(2, Field0 - (-1));
            Field1.Encode(buffer);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("CreateHeroResultMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            Field1.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
