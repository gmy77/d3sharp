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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using Mooege.Common.Extensions;

// contains code from tomrus88 (https://github.com/tomrus88/d3proto/blob/master/Core/SRP.cs)

namespace Mooege.Core.MooNet.Authentication
{
    public class SRP6
    {
        private readonly SHA256Managed _sha256 = new SHA256Managed();

        public string Email { get; private set; }
        public string Password { get; private set; }
        public string AccountSalt { get; private set; }

        private readonly BigInteger s;
        private readonly BigInteger I;
        private readonly BigInteger v;
        private readonly BigInteger b;
        private readonly BigInteger B;

        private static readonly byte[] gBytes = new byte[] { 0x02 };
        private readonly BigInteger g = gBytes.ToBigInteger();

        private static readonly byte[] NBytes = new byte[]
            {
                0xAB, 0x24, 0x43, 0x63, 0xA9, 0xC2, 0xA6, 0xC3, 0x3B, 0x37, 0xE4, 0x61, 0x84, 0x25, 0x9F, 0x8B,
                0x3F, 0xCB, 0x8A, 0x85, 0x27, 0xFC, 0x3D, 0x87, 0xBE, 0xA0, 0x54, 0xD2, 0x38, 0x5D, 0x12, 0xB7,
                0x61, 0x44, 0x2E, 0x83, 0xFA, 0xC2, 0x21, 0xD9, 0x10, 0x9F, 0xC1, 0x9F, 0xEA, 0x50, 0xE3, 0x09,
                0xA6, 0xE5, 0x5E, 0x23, 0xA7, 0x77, 0xEB, 0x00, 0xC7, 0xBA, 0xBF, 0xF8, 0x55, 0x8A, 0x0E, 0x80,
                0x2B, 0x14, 0x1A, 0xA2, 0xD4, 0x43, 0xA9, 0xD4, 0xAF, 0xAD, 0xB5, 0xE1, 0xF5, 0xAC, 0xA6, 0x13,
                0x1C, 0x69, 0x78, 0x64, 0x0B, 0x7B, 0xAF, 0x9C, 0xC5, 0x50, 0x31, 0x8A, 0x23, 0x08, 0x01, 0xA1,
                0xF5, 0xFE, 0x31, 0x32, 0x7F, 0xE2, 0x05, 0x82, 0xD6, 0x0B, 0xED, 0x4D, 0x55, 0x32, 0x41, 0x94,
                0x29, 0x6F, 0x55, 0x7D, 0xE3, 0x0F, 0x77, 0x19, 0xE5, 0x6C, 0x30, 0xEB, 0xDE, 0xF6, 0xA7, 0x86
            };

        private readonly BigInteger N = NBytes.ToBigInteger();
        
        private BigInteger _secondChallenge;

        public SRP6(string email, string password)
        {
            this.Email = email;
            this.Password = password;

            this.AccountSalt = _sha256.ComputeHash(Encoding.ASCII.GetBytes(email)).ToHexString();
            var sBytes = GetRandomBytes(32);
            this.s = sBytes.ToBigInteger();

            var IBytes = _sha256.ComputeHash(Encoding.ASCII.GetBytes(this.AccountSalt.ToUpper() + ":" + password.ToUpper()));
            this.I = IBytes.ToBigInteger();

            var xBytes = _sha256.ComputeHash(new byte[0].Concat(sBytes).Concat(IBytes).ToArray());
            var x = xBytes.ToBigInteger();

            this.v = BigInteger.ModPow(g, x, N);
            this.b = GetRandomBytes(128).ToBigInteger();

            var gMod = BigInteger.ModPow(g, b, N);

            var kBytes = _sha256.ComputeHash(new byte[0].Concat(NBytes).Concat(gBytes).ToArray());
            var k = kBytes.ToBigInteger();

            this.B = BigInteger.Remainder((v * k) + gMod, N);

            this._secondChallenge = this.GetSecondChallenge();

            this.LogonChallenge = new byte[0]
                .Concat(new byte[] {0})
                .Concat(this.AccountSalt.ToByteArray()) // accountt-salt
                .Concat(sBytes) // password-salt
                .Concat(B.ToArray()) // server challenge
                .Concat(this._secondChallenge.ToArray()) // second challenge
                .ToArray();
        }

        private static byte[] GetRandomBytes(int count)
        {
            var rnd = new Random();
            var result = new byte[count];
            rnd.NextBytes(result);
            return result;
        }

        public BigInteger GetSecondChallenge()
        {
            var bytes = new byte[]
            {
                0x5B, 0xE8, 0xF1, 0x95, 0x54, 0x3C, 0x1E, 0xD2, 0xA2, 0x2D, 0x84, 0x88, 0xB0, 0x60, 0xA3, 0x94, 
                0x23, 0x68, 0x65, 0xD5, 0x00, 0xEC, 0x62, 0x92, 0x95, 0x82, 0xEB, 0xA6, 0x31, 0xEB, 0xF5, 0x0E, 
                0xFD, 0x1E, 0x14, 0x8E, 0x9C, 0x55, 0x9C, 0x62, 0x4B, 0x31, 0x72, 0xE8, 0x2E, 0xD4, 0xC2, 0x5D, 
                0x0A, 0x96, 0xF1, 0xA5, 0xFD, 0xE8, 0x04, 0xDA, 0xBE, 0x23, 0x72, 0x97, 0x09, 0xA6, 0xB2, 0x92, 
                0xD3, 0x67, 0xFF, 0xD8, 0x20, 0xC5, 0xCB, 0xC8, 0xF4, 0x8D, 0x16, 0xD7, 0xD0, 0x12, 0xF8, 0x48, 
                0xD1, 0x05, 0xAE, 0x03, 0xBA, 0x58, 0x49, 0x9C, 0x8A, 0xB7, 0x56, 0xAA, 0xC8, 0xFB, 0x18, 0x5E, 
                0x7E, 0x4E, 0x1B, 0x2C, 0xD0, 0x4C, 0xDA, 0xA3, 0xB7, 0x52, 0xDD, 0x89, 0x14, 0xE2, 0x1E, 0x73, 
                0xA3, 0x98, 0x5D, 0x5A, 0x41, 0xE8, 0x01, 0xDA, 0x90, 0xCD, 0x61, 0x9D, 0x6E, 0xDD, 0x41, 0x68
            };

            var sc = bytes.ToBigInteger();
            return sc;
        }

        /// byte accountSalt[32]; - static value per account
        /// byte passwordSalt[32]; - static value per account
        /// byte serverChallenge[128]; - changes every login
        /// byte secondChallenge[128]; - changes every login
        public byte[] LogonChallenge { get; private set; }
    }
}
