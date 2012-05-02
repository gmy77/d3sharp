/*
 * Copyright (C) 2012 mooege project
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

using Mooege.Net.GS.Message;
using Mooege.Common.MPQ.FileFormats;

namespace Mooege.Core.GS.Powers.Implementations
{
    [ImplementsPowerSNO(30540)]
    public class SummonedBuff : TimedBuff
    {
        public override void Init()
        {
            base.Init();
            Timeout = WaitSeconds(3f);  // TODO: calculate this based on spawn animation length
        }

        public override bool Apply()
        {
            base.Apply();

            // lookup and play spawn animation, otherwise fail
            if (this.Target.AnimationSet != null && this.Target.AnimationSet.TagExists(AnimationTags.Spawn))
            {
                this.Target.PlayActionAnimation(this.Target.AnimationSet.GetAniSNO(AnimationTags.Spawn));
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
