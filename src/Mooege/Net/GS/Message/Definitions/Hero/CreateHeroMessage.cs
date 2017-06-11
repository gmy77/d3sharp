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

namespace Mooege.Net.GS.Message.Definitions.Hero
{
    [Message(Opcodes.CreateHeroMessage)]
    public class CreateHeroMessage : GameMessage
    {
        public string Name;
        public int /* gbid */ Field1;
        public int Field2;

        public override void Parse(GameBitBuffer buffer)
        {
            Name = buffer.ReadCharArray(49);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(30);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteCharArray(49, Name);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(30, Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("CreateHeroMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Name: \"" + Name + "\"");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
