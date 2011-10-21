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

using System.IO;
using CrystalMpq;
using Mooege.Common.Extensions;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.Monster)]
    public class Monster : FileFormat
    {
        public int MonsterSNO;
        public int ActorSNO;

        public int Race;
        public int Size;
        public int type;
        public string MonsterType // Probably should have an enum of these
        {
            get
            {
                if (type == 0x00) { return "undead"; } // also 0x00 on items like Power_Proxy_Seeker
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
            stream.Seek(16, SeekOrigin.Begin);
            this.MonsterSNO = stream.ReadInt32();
            stream.Position = 0x20;
            this.ActorSNO = stream.ReadInt32();
            stream.Position = 0x28;
            this.type = stream.ReadInt32();
            this.Race = stream.ReadInt32();
            this.Size = stream.ReadInt32();
            stream.Position = 0x54;
            this.Level.Normal = stream.ReadInt32();
            this.Level.Nightmare = stream.ReadInt32();
            this.Level.Hell = stream.ReadInt32();
            this.Level.Inferno = stream.ReadInt32();
            // 145floats follow this according to chuanhsing, not sure what their for - DarkLotus
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