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
using System.Numerics;

namespace Mooege.Common.Extensions
{
    public static class BigIntegerExtensions
    {
        public static BigInteger ToBigInteger(this byte[] src)
        {
            var dst = new byte[src.Length + 1];
            Array.Copy(src, dst, src.Length);
            return new BigInteger(dst);
        }

        public static byte[] ToArray(this BigInteger b)
        {
            var result = b.ToByteArray();
            if (result[result.Length - 1] == 0 && (result.Length % 0x10) != 0)
                Array.Resize(ref result, result.Length - 1);
            return result;
        }

        public static byte[] ToArray(this BigInteger b, int size)
        {
            byte[] result = b.ToArray();
            if (result.Length > size)
                throw new ArgumentOutOfRangeException("size", size, "must be large enough to convert the BigInteger");

            // If the size is already correct, return the result.
            if (result.Length == size)
                return result;

            // Resize the array.
            int n = size - result.Length;
            Array.Resize(ref result, size);

            // Fill the extra bytes with 0x00.
            while (n > 0)
            {
                result[size - n] = 0x00;
                n--;
            }

            return result;
        }
    }
}
