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

namespace Mooege.Net.GS.Message.Definitions.Misc
{
    [Message(Opcodes.FloatingNumberMessage)]
    public class FloatingNumberMessage : GameMessage
    {
        // Enums members with a color in their name display a colored number
        // others display a localized string. FloatType.Dodged displays a
        // floating "Dodge"... crash sending (int)28 as type
        public enum FloatType
        {
            White = 0,
            WhiteCritical,
            Golden,
            Red2,              // GoldenCritical was expected
            Red,
            RedCritical,
            Dodge,
            Dodged,
            Block,
            Parry,
            Green,
            Absorbed,
            Rooted,
            Stunned,
            Blinded,
            Frozen,
            Feared,
            Charmed,
            Taunted,
            Snared,
            AttackSlowed,
            BrokeFreeze,
            BrokeBlind,
            BrokeStun,
            BrokeRoot,
            BrokeSnare,
            BrokeFear,
            Immune
        }

        public uint ActorID;
        public float Number;
        public FloatType Type;

        public FloatingNumberMessage() : base(Opcodes.FloatingNumberMessage) {}

        public override void Parse(GameBitBuffer buffer)
        {
            ActorID = buffer.ReadUInt(32);
            Number = buffer.ReadFloat32();
            Type = (FloatType)buffer.ReadInt(6);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteUInt(32, ActorID);
            buffer.WriteFloat32(Number);
            buffer.WriteInt(6, (int)Type);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("FloatingNumberMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ActorID: 0x" + ActorID.ToString("X8") + " (" + ActorID + ")");
            b.Append(' ', pad); b.AppendLine("Number: " + Number.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Type: 0x" + ((int)Type).ToString("X8") + " (" + Type + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
