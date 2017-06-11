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

namespace Mooege.Net.GS.Message.Definitions.Misc
{
    /// <summary>
    /// Displays either "+x Gold", "+x XP" or "+x Gold and +y XP" 
    /// </summary>

    [Message(Opcodes.FloatingAmountMessage)]
    public class FloatingAmountMessage : GameMessage
    {
        public enum FloatType : int
        {
            Gold = 0x1c,
            XP = 0x1d,
            Both = 0x1e         // XP in field1 and gold in field2 
        }

        public WorldPlace Place;
        public int Amount;
        public int? OptionalGoldAmount;
        public FloatType Type;

        public FloatingAmountMessage() : base(Opcodes.FloatingAmountMessage) { }

        public override void Parse(GameBitBuffer buffer)
        {
            Place = new WorldPlace();
            Place.Parse(buffer);
            Amount = buffer.ReadInt(32);
            if (buffer.ReadBool())
            {
                OptionalGoldAmount = buffer.ReadInt(32);
            }
            Type = (FloatType)buffer.ReadInt(6);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            Place.Encode(buffer);
            buffer.WriteInt(32, Amount);
            buffer.WriteBool(OptionalGoldAmount.HasValue);
            if (OptionalGoldAmount.HasValue)
            {
                buffer.WriteInt(32, OptionalGoldAmount.Value);
            }
            buffer.WriteInt(6, (int)Type);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("FloatingAmountMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            Place.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Amount: 0x" + Amount.ToString("X8") + " (" + Amount + ")");
            if (OptionalGoldAmount.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("OptionalGoldAmount.Value: 0x" + OptionalGoldAmount.Value.ToString("X8") + " (" + OptionalGoldAmount.Value + ")");
            }
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + ((int)Type).ToString("X8") + " (" + Type + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
