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

namespace Mooege.Net.GS.Message.Definitions.ACD
{
    /// <summary>
    /// Server -> Client
    /// 
    /// Sent to rotate an actor (most likely around the world z axis, not the actor rotation axis?)
    /// </summary>
    [Message(Opcodes.ACDTranslateFacingMessage)]
    public class ACDTranslateFacingMessage : GameMessage
    {
        /// <summary>
        /// Id of the player actor
        /// </summary>
        public uint ActorId;

        /// <summary>
        /// Angle between actor X axis and world X axis in radians
        /// </summary>
        public float Angle;

        /// <summary>
        /// Sets whether the player turned immediatly or smoothly
        /// </summary>
        public bool TurnImmediately;

        public ACDTranslateFacingMessage() : base(Opcodes.ACDTranslateFacingMessage) { }

        public override void Parse(GameBitBuffer buffer)
        {
            ActorId = buffer.ReadUInt(32);
            Angle = buffer.ReadFloat32();
            TurnImmediately = buffer.ReadBool();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteUInt(32, ActorId);
            buffer.WriteFloat32(Angle);
            buffer.WriteBool(TurnImmediately);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDTranslateFacingMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ActorID: 0x" + ActorId.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Angle: " + Angle.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Immediately: " + (TurnImmediately ? "true" : "false"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }

    }
}
