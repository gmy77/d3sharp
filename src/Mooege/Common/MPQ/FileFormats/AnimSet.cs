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

using System.Collections.Generic;
using System.Linq;
using CrystalMpq;
using Gibbed.IO;
using Mooege.Common.Helpers.Math;
using Mooege.Common.MPQ.FileFormats.Types;
using Mooege.Core.GS.Common.Types.SNO;
using System;
using Mooege.Common.Helpers;
using Mooege.Core.GS.Common.Types.TagMap;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.AnimSet)]
    public class AnimSet : FileFormat
    {
        public Header Header { get; private set; }
        public int SNOParentAnimSet { get; private set; }
        public TagMap TagMapAnimDefault { get; private set; }
        public TagMap[] AnimSetTagMaps;


        private Dictionary<int, int> _animations;
        public Dictionary<int, int> Animations
        {
            get
            {
                if (_animations == null)
                {
                    _animations = new Dictionary<int, int>();
                    foreach (var x in TagMapAnimDefault.TagMapEntries)
                    {
                        _animations.Add(x.TagID, x.Int);
                    }
                    //not sure how better to do this, cant load parents anims on init as they may not be loaded first. - DarkLotus
                    if (SNOParentAnimSet != -1)
                    {
                        var ani = (FileFormats.AnimSet)MPQStorage.Data.Assets[SNOGroup.AnimSet][SNOParentAnimSet].Data;
                        foreach (var x in ani.Animations)
                        {
                            if (!_animations.ContainsKey(x.Key))
                                _animations.Add(x.Key, x.Value);
                        }
                    }

                } return _animations;
            }
        }

        public AnimSet(MpqFile file)
        {
            var stream = file.Open();
            this.Header = new Header(stream);
            this.SNOParentAnimSet = stream.ReadValueS32();
            TagMapAnimDefault = stream.ReadSerializedItem<TagMap>();
            stream.Position += 8;
            AnimSetTagMaps = new TagMap[19];
            for (int i = 0; i < 19; i++)
            {
                AnimSetTagMaps[i] = stream.ReadSerializedItem<TagMap>();
                stream.Position += 8;
            }

            stream.Close();
        }
        public int GetAniSNO(AnimationTags type)
        {
            if (Animations.Keys.Contains((int)type))
            {
                if (Animations[(int)type] != -1)
                {
                    return Animations[(int)type];
                }
            }
            return -1;
        }
        public bool TagExists(AnimationTags type)
        {
            if (Animations.Keys.Contains((int)type))
            {
                return true;
            }
            return false;
        }
        public int GetAnimationTag(AnimationTags type)
        {
            if (Animations.Keys.Contains((int)type))
            {
                return (int)type;
            }
            return -1;
        }
        public int GetRandomDeath()
        {
            int ani = -1;
            if (!TagExists(AnimationTags.DeathDefault)) { return -1; }
            while (ani == -1)
            {
                Array values = Enum.GetValues(typeof(DeathTags));
                ani = GetAniSNO((AnimationTags)values.GetValue(RandomHelper.Next(0, values.Length - 1)));
            }
            return ani;
        }
        private enum DeathTags
        {
            Arcane = 73776,
            Fire = 73744,
            Lightning = 73760,
            Poison = 73792,
            Plague = 73856,
            Dismember = 73872,
            Default = 69712,
            Pulverise = 73824,
            Cold = 74016,
            Lava = 74032,
            Holy = 74048,
            Spirit = 74064,
            FlyingOrDefault = 71424
        }
    }
    public enum AnimationTags
    {
        GenericCast = 262144,
        Idle2 = 69632,
        Idle = 69968,
        Spawn = 70097,

        KnockBackLand = 71176,
        KnockBackMegaOuttro = 71218,
        KnockBack = 71168,
        KnockBackMegaIntro = 71216,
        RangedAttack = 69840,
        Stunned = 69648,
        GetHit = 69664,
        Dead1 = 79168,
        Dead2 = 79152,
        Dead3 = 77920,
        Dead4 = 77888,
        Dead5 = 77904,
        Dead6 = 77872,
        Dead7 = 77856,
        Dead8 = 77840,
        SpecialDead = 71440,
        Run = 69728,
        Walk = 69744,
        Attack = 69776,
        Attack2 = 69792,
        SpecialAttack = 69904,
        DeathArcane = 73776,
        DeathFire = 73744,
        DeathLightning = 73760,
        DeathPoison = 73792,
        DeathPlague = 73856,
        DeathDismember = 73872,
        DeathDefault = 69712,
        DeathPulverise = 73824,
        DeathCold = 74016,
        DeathLava = 74032,
        DeathHoly = 74048,
        DeathSpirit = 74064,
        DeathFlyingOrDefault = 71424
    }
}
