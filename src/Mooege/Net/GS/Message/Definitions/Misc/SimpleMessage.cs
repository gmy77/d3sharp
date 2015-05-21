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

using System;
using System.Text;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Net.GS.Message.Definitions.Hireling;
using Mooege.Net.GS.Message.Fields;

namespace Mooege.Net.GS.Message.Definitions.Misc
{
    [Message(new[]{
        Opcodes.SimpleMessage1, Opcodes.SimpleMessage2, Opcodes.SimpleMessage5, Opcodes.SimpleMessage6, Opcodes.SimpleMessage7, Opcodes.SimpleMessage8,
        Opcodes.SimpleMessage10, Opcodes.SimpleMessage11, Opcodes.SimpleMessage12, Opcodes.SimpleMessage14, Opcodes.SimpleMessage16, 
        Opcodes.SimpleMessage18, Opcodes.SimpleMessage19, Opcodes.SimpleMessage20, Opcodes.SimpleMessage21, Opcodes.SimpleMessage22, Opcodes.SimpleMessage23, Opcodes.SimpleMessage24, 
        Opcodes.SimpleMessage25, Opcodes.SimpleMessage26, Opcodes.SimpleMessage27, Opcodes.SimpleMessage28, Opcodes.SimpleMessage29, Opcodes.SimpleMessage30, Opcodes.SimpleMessage31,
        Opcodes.SimpleMessage32, Opcodes.SimpleMessage34, Opcodes.SimpleMessage35, Opcodes.SimpleMessage36, Opcodes.SimpleMessage37, Opcodes.SimpleMessage38, 
        Opcodes.SimpleMessage39, Opcodes.SimpleMessage40, Opcodes.SimpleMessage41, Opcodes.SimpleMessage42,
        Opcodes.SimpleMessage46, Opcodes.SimpleMessage47})]
    public class SimpleMessage : GameMessage
    {

        public override void Parse(GameBitBuffer buffer)
        {
        }

        public override void Encode(GameBitBuffer buffer)
        {
            throw new NotImplementedException();
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
