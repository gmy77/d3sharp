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

// Credits goes to SharpNEAT project: http://sharpneat.sourceforge.net/ (GPL v2/LGPL).

using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Mooege.Common.Helpers.Math
{
    /// <summary>
    /// A fast random number generator.
    /// This class is by magnitude faster than System.Random and is preferred.
    /// </summary>
    [Serializable]
    public sealed class FastRandom : Random
    {
        /// <summary>
        /// Gets a FastRandom instance for the current thread.
        /// </summary>
        [ThreadStatic]
        [SuppressMessage("Microsoft.Security", "CA2104", Justification = "This field has to be public and read-only.")]
        public static readonly FastRandom Current = new FastRandom();

        private const double RealUnitInt32 = 1.0d / (int.MaxValue + 1.0d);

        private const double RealUnitUInt32 = 1.0d / (uint.MaxValue + 1.0d);

        private const uint Y = 842502087;

        private const uint Z = 3579807591;

        private const uint W = 273326509;

        private uint _x;

        private uint _y;

        private uint _z;

        private uint _w;

        private uint _bitBuffer;

        private uint _bitMask = 1;

        protected override double Sample()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Initialises a new instance using a time-dependent seed.
        /// </summary>
        public FastRandom()
            : this(Environment.TickCount)
        {
        }

        /// <summary>
        /// Initializes a new instance using an int value as seed.
        /// </summary>
        public FastRandom(int seed)
        {
            Initialize(seed);
        }

        /// <summary>
        /// Reinitializes using an int value as a seed.
        /// </summary>
        /// <param name="seed">Seed to initialize with.</param>
        public void Initialize(int seed)
        {
            _x = (uint)seed;
            _y = Y;
            _z = Z;
            _w = W;
        }

        /// <summary>
        /// Generates a random int over the range 0 to int.MaxValue - 1.
        /// </summary>
        public override int Next()
        {
            var t = (_x ^ (_x << 11));
            _x = _y;
            _y = _z;
            _z = _w;
            _w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8));

            // Handle the special case where the value int.MaxValue is generated. This is outside of 
            // the range of permitted values, so we therefore call Next() to try again.
            var rtn = _w & 0x7fffffff;
            if (rtn == 0x7fffffff)
                return Next();

            return (int)rtn;
        }

        /// <summary>
        /// Generates a random int over the range 0 to maxValue - 1, and not including maxValue.
        /// </summary>
        /// <param name="maxValue">The upper bound.</param>
        public override int Next(int maxValue)
        {
            var t = (_x ^ (_x << 11));
            _x = _y;
            _y = _z;
            _z = _w;

            // The explicit int cast before the first multiplication gives better performance.
            var value = (int)((RealUnitInt32 * (int)(0x7fffffff & (_w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8))))) * maxValue);
            Contract.Assume(value >= 0);
            Contract.Assume(value < maxValue);
            return value;
        }

        /// <summary>
        /// Generates a random int over the range minValue to maxValue - 1, and not including maxValue.
        /// 
        /// Note that maxValue must be >= minValue and minValue may be negative.
        /// </summary>
        /// <param name="minValue">The lower bound.</param>
        /// <param name="maxValue">The upper bound.</param>
        public override int Next(int minValue, int maxValue)
        {
            var t = (_x ^ (_x << 11));
            _x = _y;
            _y = _z;
            _z = _w;

            // The explicit int cast before the first multiplication gives better performance.
            var range = maxValue - minValue;
            if (range < 0)
            {
                // If range is < 0 then an overflow has occurred and we must resort to
                // using long integer arithmetic instead (slower). We also must use
                // all 32 bits of precision, instead of the normal 31, which again
                // is slower.
                return minValue + (int)((RealUnitUInt32 * (_w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8)))) *
                    ((long)maxValue - minValue));
            }

            // 31 bits of precision will suffice if range <= int.MaxValue. This allows us
            // to cast to an int and gain a little more performance.
            var value = minValue + (int)((RealUnitInt32 * (int)(0x7fffffff & (_w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8))))) * range);
            Contract.Assume(value >= minValue);
            Contract.Assume(value <= maxValue);
            Contract.Assume(value < maxValue || maxValue == minValue && value == minValue);
            return value;
        }

        /// <summary>
        /// Generates a random double. Values returned are from 0.0 up to, but not including, 1.0.
        /// </summary>
        public override double NextDouble()
        {
            var t = (_x ^ (_x << 11));
            _x = _y;
            _y = _z;
            _z = _w;

            // Here we can gain a 2x speed improvement by generating a value that can be casted to 
            // an int instead of the more easily available uint. If we then explicitly cast to an 
            // int the compiler will then cast the int to a double to perform the multiplication. 
            // This final cast is a lot faster than casting from an uint to a double. The extra cast
            // to an int is very fast (the allocated bits remain the same), and so the overall effect 
            // of the extra cast is a significant performance improvement.
            return (RealUnitInt32 * (int)(0x7fffffff & (_w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8)))));
        }

        /// <summary>
        /// Fills the provided byte array with random bytes.
        /// 
        /// This method is functionally equivalent to System.Random.NextBytes. 
        /// </summary>
        /// <param name="buffer">Buffer to fill.</param>
        public override unsafe void NextBytes(byte[] buffer)
        {
            if (buffer.Length % 8 != 0)
                throw new ArgumentException("Buffer length must be divisible by 8.");

            var x = _x;
            var y = _y;
            var z = _z;
            var w = _w;

            fixed (byte* pByte0 = buffer)
            {
                var pDWord = (uint*)pByte0;
                var len = buffer.Length >> 2;

                for (var i = 0; i < len; i += 2)
                {
                    var t = (x ^ (x << 11));
                    x = y;
                    y = z;
                    z = w;
                    pDWord[i] = w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));

                    t = (x ^ (x << 11));
                    x = y;
                    y = z;
                    z = w;
                    pDWord[i + 1] = w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));
                }
            }

            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }

        /// <summary>
        /// Generates a uint. Values returned are over the full range of a uint, 
        /// uint.MinValue to uint.MaxValue, inclusive.
        /// 
        /// This is the fastest method for generating a single random number because the underlying
        /// random number generator algorithm generates 32 random bits that can be cast directly to 
        /// a uint.
        /// </summary>
        [CLSCompliant(false)]
        public uint NextUInt32()
        {
            var t = (_x ^ (_x << 11));
            _x = _y;
            _y = _z;
            _z = _w;

            return (_w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8)));
        }

        /// <summary>
        /// Generates a random int over the range 0 to int.MaxValue, inclusive. 
        /// 
        /// This method differs from Next only in that the range is 0 to int.MaxValue
        /// and not 0 to int.MaxValue - 1.
        /// </summary>
        public int NextInt32()
        {
            var t = (_x ^ (_x << 11));
            _x = _y;
            _y = _z;
            _z = _w;

            return (int)(0x7fffffff & (_w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8))));
        }

        /// <summary>
        /// Generates a single random bit.
        /// </summary>
        public bool NextBoolean()
        {
            if (_bitMask == 1)
            {
                // Generate 32 more bits.
                var t = (_x ^ (_x << 11));
                _x = _y;
                _y = _z;
                _z = _w;

                _bitBuffer = _w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8));

                // Reset the bitmask that tells us which bit to read next.
                _bitMask = 0x80000000;
                return (_bitBuffer & _bitMask) == 0;
            }

            return (_bitBuffer & (_bitMask >>= 1)) == 0;
        }
    }
}
