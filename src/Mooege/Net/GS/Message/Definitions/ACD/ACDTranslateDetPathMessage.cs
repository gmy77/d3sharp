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
using Mooege.Core.GS.Common.Types.Math;

namespace Mooege.Net.GS.Message.Definitions.ACD
{
    [Message(Opcodes.ACDTranslateDetPathMessage)]
    public class ACDTranslateDetPathMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int Field2;
        public int Field3;
        public Vector3D Field4;
        public float /* angle */ Field5;
        public Vector3D Field6;
        public int Field7;
        public int Field8;
        public int Field9;
        public int /* sno */ Field10;
        public int Field11;
        public float Field12;
        public float Field13;
        public float Field14;
        public float Field15;

        public ACDTranslateDetPathMessage() : base(Opcodes.ACDTranslateDetPathMessage) { }


        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(4);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(32);
            Field4 = new Vector3D();
            Field4.Parse(buffer);
            Field5 = buffer.ReadFloat32();
            Field6 = new Vector3D();
            Field6.Parse(buffer);
            Field7 = buffer.ReadInt(32);
            Field8 = buffer.ReadInt(32);
            Field9 = buffer.ReadInt(32);
            Field10 = buffer.ReadInt(32);
            Field11 = buffer.ReadInt(32);
            Field12 = buffer.ReadFloat32();
            Field13 = buffer.ReadFloat32();
            Field14 = buffer.ReadFloat32();
            Field15 = buffer.ReadFloat32();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(4, Field1);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(32, Field3);
            Field4.Encode(buffer);
            buffer.WriteFloat32(Field5);
            Field6.Encode(buffer);
            buffer.WriteInt(32, Field7);
            buffer.WriteInt(32, Field8);
            buffer.WriteInt(32, Field9);
            buffer.WriteInt(32, Field10);
            buffer.WriteInt(32, Field11);
            buffer.WriteFloat32(Field12);
            buffer.WriteFloat32(Field13);
            buffer.WriteFloat32(Field14);
            buffer.WriteFloat32(Field15);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDTranslateDetPathMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            Field4.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field5: " + Field5.ToString("G"));
            Field6.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field7: 0x" + Field7.ToString("X8") + " (" + Field7 + ")");
            b.Append(' ', pad); b.AppendLine("Field8: 0x" + Field8.ToString("X8") + " (" + Field8 + ")");
            b.Append(' ', pad); b.AppendLine("Field9: 0x" + Field9.ToString("X8") + " (" + Field9 + ")");
            b.Append(' ', pad); b.AppendLine("Field10: 0x" + Field10.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field11: 0x" + Field11.ToString("X8") + " (" + Field11 + ")");
            b.Append(' ', pad); b.AppendLine("Field12: " + Field12.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field13: " + Field13.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field14: " + Field14.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field15: " + Field15.ToString("G"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}