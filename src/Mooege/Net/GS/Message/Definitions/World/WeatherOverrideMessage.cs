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

namespace Mooege.Net.GS.Message.Definitions.World
{
    [Message(Opcodes.WeatherOverrideMessage)]
    public class WeatherOverrideMessage : GameMessage
    {
        public int SNOWorld;
        public float Field1;
        public float Field2;

        public override void Parse(GameBitBuffer buffer)
        {
            SNOWorld = buffer.ReadInt(32);
            Field1 = buffer.ReadFloat32();
            Field2 = buffer.ReadFloat32();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, SNOWorld);
            buffer.WriteFloat32(Field1);
            buffer.WriteFloat32(Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("WeatherOverrideMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("SNOWorld: 0x" + SNOWorld.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field1: " + Field1.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field2: " + Field2.ToString("G"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}