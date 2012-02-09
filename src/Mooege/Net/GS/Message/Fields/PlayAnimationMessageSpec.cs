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

namespace Mooege.Net.GS.Message.Fields
{

    public class PlayAnimationMessageSpec
    {
        /// <summary>
        /// Duration in ticks the animation plays. If set too short, animation just stop
        /// </summary>
        public int Duration;

        /// <summary>
        /// SNOId of the animation to play
        /// </summary>
        public int AnimationSNO;

        /// <summary>
        /// Inded of the permutation within the animation object
        /// </summary>
        public int PermutationIndex;

        /// <summary>
        /// Speed in which to play the animation
        /// </summary>
        public float Speed;

        public void Parse(GameBitBuffer buffer)
        {
            Duration = buffer.ReadInt(32);
            AnimationSNO = buffer.ReadInt(32);
            PermutationIndex = buffer.ReadInt(32);
            Speed = buffer.ReadFloat32();
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Duration);
            buffer.WriteInt(32, AnimationSNO);
            buffer.WriteInt(32, PermutationIndex);
            buffer.WriteFloat32(Speed);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayAnimationMessageSpec:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Duration: 0x" + Duration.ToString("X8") + " (" + Duration + " ticks)");
            b.Append(' ', pad);
            b.AppendLine("AnimationSNO: 0x" + AnimationSNO.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("PermutationIndex: 0x" + PermutationIndex.ToString("X8") + " (" + PermutationIndex + ")");
            b.Append(' ', pad);
            b.AppendLine("Speed: " + Speed.ToString("G"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}