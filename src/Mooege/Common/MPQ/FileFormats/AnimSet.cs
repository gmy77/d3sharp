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

using System.Collections.Generic;
using System.Linq;
using CrystalMpq;
using Gibbed.IO;
using Mooege.Common.MPQ.FileFormats.Types;
using Mooege.Core.GS.Common.Types.SNO;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.AnimSet)]
    public class AnimSet : FileFormat
    {
        public Header Header { get; private set; }
        public int NumberOfAnimations { get; private set; }
        public List<AnimationDef> Animations = new List<AnimationDef>();

        public AnimSet(MpqFile file)
        {
            var stream = file.Open();
            this.Header = new Header(stream);

            stream.Position = 352;
            this.NumberOfAnimations = stream.ReadValueS32();
            for (int i = 0; i < this.NumberOfAnimations; i++)
            {
                stream.Position += 4;
                var animation = new AnimationDef { TagID = stream.ReadValueS32(), AnimationSNO = stream.ReadValueS32() };
                this.Animations.Add(animation);
            }

            stream.Close();
        }

        public int IdleTAG
        {
            get
            {
                if (Animations.Exists(ani => ani.TagID == 69968 && ani.AnimationSNO != -1))
                    return Animations.First(ani => ani.TagID == 69968 && ani.AnimationSNO != -1).TagID;

                if (Animations.Exists(ani => ani.TagID == 69632 && ani.AnimationSNO != -1))
                    return Animations.First(ani => ani.TagID == 69632 && ani.AnimationSNO != -1).TagID;

                //Logger.Trace("No Idle found for actor: " + this.ActorSNO + " Sending Zombies Idle");
                //Logger.Trace("using string matched ani: " + Animations.Single(ani => ani.name.Contains("idle") == true).AniTagID);
                //return Animations.Single(ani => ani.name.Contains("idle") == true).AniTagID;
                return 0x11150;
            }
        }

        public int WalkTAG
        {
            get
            {
                if (Animations.Exists(ani => ani.TagID == 69728 && ani.AnimationSNO != -1))
                    return Animations.Single(ani => ani.TagID == 69728 && ani.AnimationSNO != -1).TagID;

                if (Animations.Exists(ani => ani.TagID == 69744 && ani.AnimationSNO != -1))
                    return Animations.First(ani => ani.TagID == 69744 && ani.AnimationSNO != -1).TagID;

                // hackyness should never happen unless a actor trys to walk who has no walk animation
                //Logger.Trace("No Walk found for actor: " + this.ActorSNO + " Sending Zombies Walk");
                return 69728;
            }
        }

        public struct AnimationDef
        {
            public int TagID;
            public int AnimationSNO;
        }
    }
}
