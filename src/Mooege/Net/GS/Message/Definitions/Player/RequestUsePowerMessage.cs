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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mooege.Net.GS.Message.Definitions.Player
{
    // uhm? - please comment about what should be fixed :) /raist.

    /* TODO: Fixme
    [Message(Opcodes.RequestUsePowerMessage, Consumers.Player)]
    public class RequestUsePowerMessage : GameMessage
    {
        public int PowerSNOId;

        public override void Parse(GameBitBuffer buffer)
        {
            PowerSNOId = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, PowerSNOId);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("RequestUsePowerMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("PowerSNOId: 0x" + PowerSNOId.ToString("X8") + " (" + PowerSNOId + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
     */
}
