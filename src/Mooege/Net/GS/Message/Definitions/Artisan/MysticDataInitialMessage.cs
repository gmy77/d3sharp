﻿/*
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
using D3.ItemCrafting;

namespace Mooege.Net.GS.Message.Definitions.Artisan
{
    [Message(Opcodes.MysticDataInitialMessage)]
    public class MysticDataInitialMessage : GameMessage
    {
        public CrafterData CrafterData;

        public MysticDataInitialMessage() : base(Opcodes.MysticDataInitialMessage) { }

        public override void Parse(GameBitBuffer buffer)
        {
            CrafterData = CrafterData.ParseFrom(buffer.ReadBlob(32));
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteBlob(32, CrafterData.ToByteArray());
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("MysticDataInitialMessage:");
            b.Append(' ', pad++);
            b.Append(CrafterData.ToString());
        }

    }
}