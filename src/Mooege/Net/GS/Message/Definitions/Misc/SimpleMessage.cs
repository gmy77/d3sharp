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
using System.Text;

namespace Mooege.Net.GS.Message.Definitions.Misc
{
    // Mostly contains messages we don't know yet what they're used for.
    // When you discover a message's use and implement it (in a new file), please remove the associated opcode below. /raist.

    [Message(
        new[] {
        Opcodes.SimpleMessage1, Opcodes.SimpleMessage2, Opcodes.SimpleMessage3, Opcodes.SimpleMessage4, Opcodes.SimpleMessage5, Opcodes.SimpleMessage6,
        Opcodes.GameSetupMessageAck, Opcodes.SimpleMessage8, Opcodes.SimpleMessage10, Opcodes.SimpleMessage11, Opcodes.SimpleMessage14, Opcodes.SimpleMessage16,
        Opcodes.SimpleMessage18, Opcodes.SimpleMessage20, Opcodes.SimpleMessage21, Opcodes.SimpleMessage22, Opcodes.SimpleMessage23, Opcodes.SimpleMessage24,
        Opcodes.SimpleMessage25, Opcodes.SimpleMessage26, Opcodes.SimpleMessage27, Opcodes.SimpleMessage28, Opcodes.SimpleMessage29, Opcodes.SimpleMessage30,
        Opcodes.RepairAllMessage, Opcodes.RepairEquippedMessage, Opcodes.SimpleMessage36, Opcodes.SimpleMessage37, Opcodes.CancelCinematicsMessage,
        Opcodes.SimpleMessage39, Opcodes.SimpleMessage40, Opcodes.SimpleMessage42, Opcodes.SimpleMessage47, Opcodes.SimpleMessage48, Opcodes.SimpleMessage49,
        Opcodes.SimpleMessage50, Opcodes.SimpleMessage51, Opcodes.SimpleMessage52
    })]
    public class SimpleMessage : GameMessage
    {

        public override void Parse(GameBitBuffer buffer)
        {
            // do not return back a not-implemented exception! /raist.
        }

        public override void Encode(GameBitBuffer buffer)
        {
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("SimpleMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
