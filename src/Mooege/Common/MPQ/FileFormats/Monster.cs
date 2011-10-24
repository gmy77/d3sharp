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

using CrystalMpq;
using Gibbed.IO;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.Monster)]
    public class Monster : FileFormat
    {
        public Header Header;
        public int MonsterSNO;
        public int ActorSNO;

        public int Race;
        public int Size;
        public int type;
        public string MonsterType // Probably should have an enum of these - DarkLotus
        {
            get
            {
                if (type == 0x00) { return "undead"; } // also 0x00 on .mon files like Power_Proxy_Seeker - DarkLotus
                if (type == 0x01) { return "demon"; }
                if (type == 0x02) { return "beast"; }
                if (type == 0x03) { return "humanoid"; }
                if (type == 0x06) { return "NPC"; }
                return "unknown";

            }
        }
        public Levels Level = new Levels();

        public Monster(MpqFile file)
        {
            var stream = file.Open();
            this.Header = new Header(stream);

            stream.Position = 0x20;
            this.ActorSNO = stream.ReadValueS32();
            stream.Position = 0x28;
            this.type = stream.ReadValueS32();
            this.Race = stream.ReadValueS32();
            this.Size = stream.ReadValueS32();
            stream.Position = 0x54;
            this.Level.Normal = stream.ReadValueS32();
            this.Level.Nightmare = stream.ReadValueS32();
            this.Level.Hell = stream.ReadValueS32();
            this.Level.Inferno = stream.ReadValueS32();            
            // 145 floats follow this according to chuanhsing, not sure what they are for - DarkLotus

            stream.Close();
        }

        public class Levels
        {
            public int Normal;
            public int Nightmare;
            public int Hell;
            public int Inferno;
        }
    }
}