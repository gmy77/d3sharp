﻿/*
 * Copyright (C) 2011 D3Sharp Project
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
using System.IO;
using System.Linq;
using D3Sharp.Utils;
using D3Sharp.Utils.Extensions;
using Gibbed.Helpers;

namespace D3Sharp.Net.Game.Packets
{
    public class GameHeader
    {
        public byte[] Data { get; private set; }

        public UInt32 Length { get; set; }
        public byte opCode { get; set; }
        public byte Flags { get; set; }

        public GameHeader()
        {
            this.Length = 0;
            this.opCode = 0x00;
        }

        public GameHeader(UInt32 Length, byte opCode, byte Flags)
        {
            this.SetData(Length, opCode, Flags);
        }

        public GameHeader(MemoryStream stream)
        {
            var Length = stream.ReadValueU32(false);
            var opCode = stream.ReadValueU8();
            var Flags = stream.ReadValueU8();

            this.SetData(Length, opCode, Flags);
        }

        void SetData(UInt32 Length, byte opCode, byte Flags)
        {
            this.Length = Length;
            this.opCode = opCode;
            this.Flags = Flags;

            this.Data = new byte[6];

            using (var stream = new MemoryStream(this.Data))
            {
                stream.WriteValueU32(this.Length, false);
                stream.WriteValueU8(this.opCode);
                stream.WriteValueU8(this.Flags);

                this.Data = stream.ToArray();
                stream.Flush();
            }
        }

        public override string ToString()
        {
            return string.Format("[L]: {0}, [O]: 0x{1}, [F]: 0x{2}", this.Length.ToString(), this.opCode.ToString("X2"), this.Flags.ToString("X2"));
        }
    }
}