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

namespace Mooege.Net.GS.Message.Definitions.Artisan
{
    /// <summary>
    /// Shows Artisans UI window.
    /// </summary>
    [Message(Opcodes.OpenArtisanWindowMessage)]
    public class OpenArtisanWindowMessage : GameMessage
    {
        public uint ArtisanID;
        public OpenArtisanWindowMessage()
            : base(Opcodes.OpenArtisanWindowMessage)
        { }

        public override void Parse(GameBitBuffer buffer)
        {
            ArtisanID = buffer.ReadUInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteUInt(32, ArtisanID);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("OpenArtisanWindowMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ArtisanID: 0x" + ArtisanID.ToString("X8") + " (" + ArtisanID + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }

    }
}
