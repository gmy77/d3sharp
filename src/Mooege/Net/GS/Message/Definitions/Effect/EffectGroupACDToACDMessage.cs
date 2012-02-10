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

namespace Mooege.Net.GS.Message.Definitions.Effect
{
	/// <summary>
    /// Sent to the client to play a special effect from an actor to another actor
    /// </summary>
    [Message(Opcodes.EffectGroupACDToACDMessage)]
    public class EffectGroupACDToACDMessage : GameMessage
    {
        public int? /* sno */ EffectSNOId; // the effect to play
        public uint ActorID; // where the effect starts
        public uint TargetID; // where the effect will travel to
		
		public EffectGroupACDToACDMessage() : base(Opcodes.EffectGroupACDToACDMessage) { }

        public override void Parse(GameBitBuffer buffer)
        {
            EffectSNOId = buffer.ReadInt(32);
            ActorID = buffer.ReadUInt(32);
            TargetID = buffer.ReadUInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, EffectSNOId.Value);
            buffer.WriteUInt(32, ActorID);
            buffer.WriteUInt(32, TargetID);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("EffectGroupACDToACDMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("EffectSNOId: 0x" + EffectSNOId.Value.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("ActorID: 0x" + ActorID.ToString("X8") + " (" + ActorID + ")");
            b.Append(' ', pad); b.AppendLine("TargetID: 0x" + TargetID.ToString("X8") + " (" + TargetID + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}