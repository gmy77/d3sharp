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

namespace Mooege.Net.GS.Message.Definitions.ACD
{
    /// <summary>
    /// Sent by the server to translate an actor along an arc
    /// </summary>
    [Message(Opcodes.ACDTranslateArcMessage)]
    public class ACDTranslateArcMessage : GameMessage
    {
        public int ActorId;                 // DynamicID of the Actor to be moved
        public Vector3D Start;              // Starting position of the movement
        public Vector3D Velocity;           // Velocity vector i guess, exact math is unknown - farmy
        public int Field3;
        public int FlyingAnimationTagID;    // TagID of the flying animation or -1
        public int LandingAnimationTagID;   // TagID of the landing animation or -1
        public float Field6;                // some sort of fallof / individual gravity..always < 0...math is unknown - farmy
        public int /* sno */ Field7;        // its a power sno, like in knockback.pow. but i dont know what its used for -farmy
        public float Field8;
        public float Field9;

        public ACDTranslateArcMessage() 
            : base(Opcodes.ACDTranslateArcMessage) 
        { }

        public override void Parse(GameBitBuffer buffer)
        {
            ActorId = buffer.ReadInt(32);
            Start = new Vector3D();
            Start.Parse(buffer);
            Velocity = new Vector3D();
            Velocity.Parse(buffer);
            Field3 = buffer.ReadInt(25);
            FlyingAnimationTagID = buffer.ReadInt(21) + (-1);
            LandingAnimationTagID = buffer.ReadInt(21) + (-1);
            Field6 = buffer.ReadFloat32();
            Field7 = buffer.ReadInt(32);
            Field8 = buffer.ReadFloat32();
            Field9 = buffer.ReadFloat32();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, ActorId);
            Start.Encode(buffer);
            Velocity.Encode(buffer);
            buffer.WriteInt(25, Field3);
            buffer.WriteInt(21, FlyingAnimationTagID - (-1));
            buffer.WriteInt(21, LandingAnimationTagID - (-1));
            buffer.WriteFloat32(Field6);
            buffer.WriteInt(32, Field7);
            buffer.WriteFloat32(Field8);
            buffer.WriteFloat32(Field9);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDTranslateArcMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ActorId: 0x" + ActorId.ToString("X8"));
            Start.AsText(b, pad);
            Velocity.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', pad); b.AppendLine("FlyingAnimationTagID: 0x" + FlyingAnimationTagID.ToString("X8") + " (" + FlyingAnimationTagID + ")");
            b.Append(' ', pad); b.AppendLine("LandingAnimationTagID: 0x" + LandingAnimationTagID.ToString("X8") + " (" + LandingAnimationTagID + ")");
            b.Append(' ', pad); b.AppendLine("Field6: " + Field6.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field7: 0x" + Field7.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field8: " + Field8.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field9: " + Field9.ToString("G"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}