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

namespace Mooege.Net.GS.Message.Definitions.World
{
    [Message(Opcodes.TargetMessage,Consumers.Player)]
    public class TargetMessage : GameMessage
    {
        public int Field0;
        public uint TargetID; // Targeted actor's DynamicID
        public WorldPlace Field2;
        public int /* sno */ PowerSNO; // SNO of the power that was used on the targeted actor
        public int Field4;
        public int Field5;
        public AnimPreplayData Field6;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(3) + (-1);
            TargetID = buffer.ReadUInt(32);
            Field2 = new WorldPlace();
            Field2.Parse(buffer);
            PowerSNO = buffer.ReadInt(32);
            Field4 = buffer.ReadInt(32);
            Field5 = buffer.ReadInt(2);
            if (buffer.ReadBool())
            {
                Field6 = new AnimPreplayData();
                Field6.Parse(buffer);
            }
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(3, Field0 - (-1));
            buffer.WriteUInt(32, TargetID);
            Field2.Encode(buffer);
            buffer.WriteInt(32, PowerSNO);
            buffer.WriteInt(32, Field4);
            buffer.WriteInt(2, Field5);
            buffer.WriteBool(Field6 != null);
            if (Field6 != null)
            {
                Field6.Encode(buffer);
            }
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("TargetMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("TargetID: 0x" + TargetID.ToString("X8") + " (" + TargetID + ")");
            Field2.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("PowerSNO: 0x" + PowerSNO.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8") + " (" + Field4 + ")");
            b.Append(' ', pad); b.AppendLine("Field5: 0x" + Field5.ToString("X8") + " (" + Field5 + ")");
            if (Field6 != null)
            {
                Field6.AsText(b, pad);
            }
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }

}
