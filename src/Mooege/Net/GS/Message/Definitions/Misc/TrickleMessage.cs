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
    /// Warning! Most of this documentation is speculation or just a description of
    /// what the fields are, not what purpose they actually have. For questions ask me - farmy
    /// 
    /// This message is periodically sent to the client for stuff like Players/NPC/Followers or the stash
    /// I guess that this message is used to keep the client updated about actors not in range for revealing
    /// (though the server happily sends TrickleMessages about actors currently revealed, so maybe its a general update.
    /// It also contains information about the levelarea of actors the client would not know otherwise)
    /// </summary>
    [Message(Opcodes.TrickleMessage)]
    public class TrickleMessage : GameMessage
    {
        public uint ActorId;
        public int ActorSNO;
        public WorldPlace WorldLocation;
        public int? PlayerIndex;        // only set for players, their lethal decoys and their spawns (companions like snake)
        public int LevelAreaSNO;        // sno of the level area the actor currently is in
        public float? Field5;           // ??? Mostly 0.99 < Field5 <= 1 but can have any value between 0 and 1. 0 for proxy actors
        public int Field6;
        public int Field7;
        public int? Field8;             // never seen it set
        public int? MinimapTextureSNO;  // SNO of the icon used to display that actor on the minimap if set
        public int? Field10;            // could that be a string hash used for the STL in field11? minimap markers appear without text nevertheless
        public int? Field11;            // Setting this to 0x21380817 makes a yellow exclamation mark appear (used for leah, cumford, imprisoned templar)
        public int? StringListSNO;      // ALWAYS?? 0x0000F063:Minimap.stl 
        public float? Field13;          // either 225 or null 
        public float? Field14;          // never seen != null
        public bool? Field15;

        public TrickleMessage() : base(Opcodes.TrickleMessage) {}

        public override void Parse(GameBitBuffer buffer)
        {
            ActorId = buffer.ReadUInt(32);
            ActorSNO = buffer.ReadInt(32);
            WorldLocation = new WorldPlace();
            WorldLocation.Parse(buffer);
            if (buffer.ReadBool())
            {
                PlayerIndex = buffer.ReadInt(4) + (-1);
            }
            LevelAreaSNO = buffer.ReadInt(32);
            if (buffer.ReadBool())
            {
                Field5 = buffer.ReadFloat32();
            }
            Field6 = buffer.ReadInt(4);
            Field7 = buffer.ReadInt(6);
            if (buffer.ReadBool())
            {
                Field8 = buffer.ReadInt(32);
            }
            if (buffer.ReadBool())
            {
                MinimapTextureSNO = buffer.ReadInt(32);
            }
            if (buffer.ReadBool())
            {
                Field10 = buffer.ReadInt(32);
            }
            if (buffer.ReadBool())
            {
                Field11 = buffer.ReadInt(32);
            }
            if (buffer.ReadBool())
            {
                StringListSNO = buffer.ReadInt(32);
            }
            if (buffer.ReadBool())
            {
                Field13 = buffer.ReadFloat32();
            }
            if (buffer.ReadBool())
            {
                Field14 = buffer.ReadFloat32();
            }
            if (buffer.ReadBool())
            {
                Field15 = buffer.ReadBool();
            }
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteUInt(32, ActorId);
            buffer.WriteInt(32, ActorSNO);
            WorldLocation.Encode(buffer);
            buffer.WriteBool(PlayerIndex.HasValue);
            if (PlayerIndex.HasValue)
            {
                buffer.WriteInt(4, PlayerIndex.Value - (-1));
            }
            buffer.WriteInt(32, LevelAreaSNO);
            buffer.WriteBool(Field5.HasValue);
            if (Field5.HasValue)
            {
                buffer.WriteFloat32(Field5.Value);
            }
            buffer.WriteInt(4, Field6);
            buffer.WriteInt(6, Field7);
            buffer.WriteBool(Field8.HasValue);
            if (Field8.HasValue)
            {
                buffer.WriteInt(32, Field8.Value);
            }
            buffer.WriteBool(MinimapTextureSNO.HasValue);
            if (MinimapTextureSNO.HasValue)
            {
                buffer.WriteInt(32, MinimapTextureSNO.Value);
            }
            buffer.WriteBool(Field10.HasValue);
            if (Field10.HasValue)
            {
                buffer.WriteInt(32, Field10.Value);
            }
            buffer.WriteBool(Field11.HasValue);
            if (Field11.HasValue)
            {
                buffer.WriteInt(32, Field11.Value);
            }
            buffer.WriteBool(StringListSNO.HasValue);
            if (StringListSNO.HasValue)
            {
                buffer.WriteInt(32, StringListSNO.Value);
            }
            buffer.WriteBool(Field13.HasValue);
            if (Field13.HasValue)
            {
                buffer.WriteFloat32(Field13.Value);
            }
            buffer.WriteBool(Field14.HasValue);
            if (Field14.HasValue)
            {
                buffer.WriteFloat32(Field14.Value);
            }
            buffer.WriteBool(Field15.HasValue);
            if (Field15.HasValue)
            {
                buffer.WriteBool(Field15.Value);
            }
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("TrickleMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ActorID: 0x" + ActorId.ToString("X8") + " (" + ActorId + ")");
            b.Append(' ', pad); b.AppendLine("ActorSNO: 0x" + ActorSNO.ToString("X8"));
            WorldLocation.AsText(b, pad);
            if (PlayerIndex.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("PlayerIndex.Value: 0x" + PlayerIndex.Value.ToString("X8") + " (" + PlayerIndex.Value + ")");
            }
            b.Append(' ', pad); b.AppendLine("LevelAreaSNO: 0x" + LevelAreaSNO.ToString("X8"));
            if (Field5.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field5.Value: " + Field5.Value.ToString("G"));
            }
            b.Append(' ', pad); b.AppendLine("Field6: 0x" + Field6.ToString("X8") + " (" + Field6 + ")");
            b.Append(' ', pad); b.AppendLine("Field7: 0x" + Field7.ToString("X8") + " (" + Field7 + ")");
            if (Field8.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field8.Value: 0x" + Field8.Value.ToString("X8") + " (" + Field8.Value + ")");
            }
            if (MinimapTextureSNO.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("MinimapTextureSNO.Value: 0x" + MinimapTextureSNO.Value.ToString("X8") + " (" + MinimapTextureSNO.Value + ")");
            }
            if (Field10.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field10.Value: 0x" + Field10.Value.ToString("X8") + " (" + Field10.Value + ")");
            }
            if (Field11.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field11.Value: 0x" + Field11.Value.ToString("X8") + " (" + Field11.Value + ")");
            }
            if (StringListSNO.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("StringListSNO.Value: 0x" + StringListSNO.Value.ToString("X8") + " (" + StringListSNO.Value + ")");
            }
            if (Field13.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field13.Value: " + Field13.Value.ToString("G"));
            }
            if (Field14.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field14.Value: " + Field14.Value.ToString("G"));
            }
            if (Field15.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field15.Value: " + (Field15.Value ? "true" : "false"));
            }
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
