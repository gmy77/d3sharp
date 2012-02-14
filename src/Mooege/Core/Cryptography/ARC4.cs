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

// Contains code from: https://wcell.svn.codeplex.com/svn/wcell/Services/WCell.RealmServer/Network/RealmClient.cs

namespace Mooege.Core.Cryptography
{
    public class ARC4
    {
        private readonly byte[] _state;
        private byte x, y;

        public ARC4(byte[] key)
        {
            _state = new byte[256];
            x = y = 0;
            KeySetup(key);
        }

        public int Process(byte[] buffer, int start, int count)
        {
            return InternalTransformBlock(buffer, start, count, buffer, start);
        }

        private void KeySetup(byte[] key)
        {
            byte index1 = 0;
            byte index2 = 0;

            for (int counter = 0; counter < 256; counter++)
            {
                _state[counter] = (byte)counter;
            }
            x = 0;
            y = 0;
            for (int counter = 0; counter < 256; counter++)
            {
                index2 = (byte)(key[index1] + _state[counter] + index2);
                // swap byte
                byte tmp = _state[counter];
                _state[counter] = _state[index2];
                _state[index2] = tmp;
                index1 = (byte)((index1 + 1) % key.Length);
            }
        }

        private int InternalTransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            for (int counter = 0; counter < inputCount; counter++)
            {
                x = (byte)(x + 1);
                y = (byte)(_state[x] + y);
                // swap byte
                byte tmp = _state[x];
                _state[x] = _state[y];
                _state[y] = tmp;

                byte xorIndex = (byte)(_state[x] + _state[y]);
                outputBuffer[outputOffset + counter] = (byte)(inputBuffer[inputOffset + counter] ^ _state[xorIndex]);
            }
            return inputCount;
        }
    }
}
