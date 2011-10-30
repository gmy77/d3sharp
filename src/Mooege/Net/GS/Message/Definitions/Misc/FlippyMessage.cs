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

using System.Text;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Net.GS.Message.Fields;

namespace Mooege.Net.GS.Message.Definitions.Misc
{
    /// <summary>
    /// Sent to the client to create a world animation. Like in gold flipping before gold dropping or
    /// highlighting the area around around important items.
    /// </summary>
    [Message(Opcodes.FlippyMessage)]
    public class FlippyMessage : GameMessage
    {
        // TODO Verify SNOs, there are to few samples to be sure - farmy
        public int ActorID;             // Effect is created at the actors location
        public int SNOParticleEffect;   // SNO for a particle effect or 0x6d82 (default_flippy) for an appearance effect
        public int SNOAppearance;       // -1 for a particle effect or SNO of the animation effect. eg Axe_flippy etc
        public Vector3D Field3;         // no idea ... my tests always take the actor position - farmy
        public FlippyMessage() : base(Opcodes.FlippyMessage) { }

        public override void Parse(GameBitBuffer buffer)
        {
            ActorID = buffer.ReadInt(32);
            SNOParticleEffect = buffer.ReadInt(32);
            SNOAppearance = buffer.ReadInt(32);
            Field3 = new Vector3D();
            Field3.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, ActorID);
            buffer.WriteInt(32, SNOParticleEffect);
            buffer.WriteInt(32, SNOAppearance);
            Field3.Encode(buffer);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("FlippyMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ActorID: 0x" + ActorID.ToString("X8") + " (" + ActorID + ")");
            b.Append(' ', pad); b.AppendLine("SNOParticleEffect: 0x" + SNOParticleEffect.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("SNOAppearance: 0x" + SNOAppearance.ToString("X8"));
            Field3.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}