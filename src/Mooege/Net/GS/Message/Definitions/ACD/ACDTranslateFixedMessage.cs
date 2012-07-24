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
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Net.GS.Message.Fields;

namespace Mooege.Net.GS.Message.Definitions.ACD
{
    /// <summary>
    /// Sent to the client to indefinitly translate an actor in a given direction.
    /// </summary>
    [Message(Opcodes.ACDTranslateFixedMessage)]
    public class ACDTranslateFixedMessage : GameMessage
    {
        public int ActorId;
        public Vector3D Velocity;       // Velocity in game units per game tick
        public int Field2;
        public int AnimationTag;        // Animation used during movement
        public int /* sno */ Field4;

        public ACDTranslateFixedMessage() : base(Opcodes.ACDTranslateFixedMessage) { }

        public override void Parse(GameBitBuffer buffer)
        {
            ActorId = buffer.ReadInt(32);
            Velocity = new Vector3D();
            Velocity.Parse(buffer);
            Field2 = buffer.ReadInt(25);
            AnimationTag = buffer.ReadInt(21) + (-1);
            Field4 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, ActorId);
            Velocity.Encode(buffer);
            buffer.WriteInt(25, Field2);
            buffer.WriteInt(21, AnimationTag - (-1));
            buffer.WriteInt(32, Field4);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDTranslateFixedMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ActorId: 0x" + ActorId.ToString("X8"));
            Velocity.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("AnimationTag: 0x" + AnimationTag.ToString("X8") + " (" + AnimationTag + ")");
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}