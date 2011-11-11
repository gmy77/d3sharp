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
using CrystalMpq;
using Gibbed.IO;
using System.Reflection;
using Mooege.Core.GS.Common.Types.SNO;

namespace Mooege.Common.MPQ.FileFormats.Types
{
    /// <summary>
    /// Implementation of a dictionary like tag map.
    /// You can access elements either using a key object that identifies which of the TagKey
    /// fields holds your value or by accessing the entry directly with the interger tag id.
    /// </summary>
    public class TagMap : ISerializableData, IEnumerable<TagMapEntry>
    {
        public int TagMapSize { get; private set; }
        private readonly Dictionary<int, TagMapEntry> _tagMapEntries;

        [Obsolete("Use TagKeys instead. If it is missing create it.")]
        public List<TagMapEntry> TagMapEntries
        {
            get
            {
                return _tagMapEntries.Values.ToList();
            }
        }

        public TagMap()
        {
            this._tagMapEntries = new Dictionary<int, TagMapEntry>();
        }

        [Obsolete("Use TagKeys instead. If it is missing create it.")]
        public bool ContainsKey(int key) { return _tagMapEntries.ContainsKey(key); }

        public bool ContainsKey(TagKeys.TagKey key) { return _tagMapEntries.ContainsKey(key.ID); }

        public void Add(TagKeys.TagKey key, TagMapEntry entry) { _tagMapEntries.Add(key.ID, entry); }

        public void Read(MpqFileStream stream)
        {
            this.TagMapSize = stream.ReadValueS32();

            for (int i = 0; i < TagMapSize; i++)
            {
                var entry = new TagMapEntry(stream);
                this._tagMapEntries.Add(entry.TagID, entry);
            }
        }

        #region accessors

        public int this[TagKeys.TagKeyInt key]
        {
            get
            {
                return _tagMapEntries[key.ID].Int;
            }
        }

        public float this[TagKeys.TagKeyFloat key]
        {
            get
            {
                return _tagMapEntries[key.ID].Float;
            }
        }

        public ScriptFormula this[TagKeys.TagKeyScript key]
        {
            get
            {
                return _tagMapEntries[key.ID].ScriptFormula;
            }
        }

        public SNOHandle this[TagKeys.TagKeySNO key]
        {
            get
            {
                return new SNOHandle(_tagMapEntries[key.ID].Int);
            }
        }

        [Obsolete("Use TagKeys instead. If it is missing create it")]
        public TagMapEntry this[int key]
        {
            get
            {
                return _tagMapEntries[key];
            }
        }

        #endregion

        #region enumurators

        public IEnumerator<TagMapEntry> GetEnumerator()
        {
            return _tagMapEntries.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _tagMapEntries.Values.GetEnumerator();
        }

        #endregion
    }

    /// <summary>
    /// This class holds all currently defined keys as well as a way to get the matching key for a TagID
    /// </summary>
    public class TagKeys
    {
        public class TagKey
        {
            public int ID;
            public string Name;
        }

        public class TagKeyInt : TagKey { }
        public class TagKeyFloat : TagKey { }
        public class TagKeyScript : TagKey { }
        public class TagKeySNO : TagKey { }

        # region compile a dictionary to access keys from ids. If you need a readable name for a TagID, look up its key and get its name

        private static readonly Dictionary<int, TagKey> Tags = new Dictionary<int, TagKey>();

        public static TagKey GetKey(int index)
        {
            return Tags.ContainsKey(index) ? Tags[index] : null;
        }

        static TagKeys()
        {
            foreach (FieldInfo field in typeof(TagKeys).GetFields())
            {
                var key = field.GetValue(null) as TagKey;
                key.Name = field.Name;
                Tags.Add(key.ID, key);
            }
        }
        #endregion

        //524864 == hasinteractionoptions?

        // MarkerSet Tags
        public static TagKeySNO QuestRange = new TagKeySNO() { ID = 524544 };
        public static TagKeySNO ConversationList = new TagKeySNO() { ID = 526080 };
        public static TagKeyFloat Scale = new TagKeyFloat() { ID = 524288 };
        public static TagKeySNO OnActorSpawnedScript = new TagKeySNO() { ID = 524808 };
        public static TagKeyInt GroupHash = new TagKeyInt() { ID = 524814 };

        // Used for portal destination resolution
        public static TagKeySNO DestinationWorld = new TagKeySNO() { ID = 526850 };
        public static TagKeyInt DestinationActorTag = new TagKeyInt() { ID = 526851 };
        public static TagKeyInt ActorTag = new TagKeyInt() { ID = 526852 };
        public static TagKeySNO DestinationLevelArea = new TagKeySNO() { ID = 526853 };

        public static TagKeySNO TriggeredConversation = new TagKeySNO() { ID = 528128 };

        // Actor Tags
        public static TagKeySNO Flippy = new TagKeySNO() { ID = 65688 };
        public static TagKeySNO Projectile = new TagKeySNO() { ID = 66138 };
        public static TagKeySNO Lore = new TagKeySNO() { ID = 67331 };

        public static TagKeySNO MinimapMarker = new TagKeySNO() { ID = 458752 };

        public static TagKeySNO FireEffectGroup = new TagKeySNO() { ID = 74064 };
        public static TagKeySNO ColdEffectGroup = new TagKeySNO() { ID = 74065 };
        public static TagKeySNO LightningEffectGroup = new TagKeySNO() { ID = 74066 };
        public static TagKeySNO PoisonEffectGroup = new TagKeySNO() { ID = 74067 };
        public static TagKeySNO ArcaneEffectGroup = new TagKeySNO() { ID = 74068 };

        public static TagKeySNO LifeStealEffectGroup = new TagKeySNO() { ID = 74070 };
        public static TagKeySNO ManaStealEffectGroup = new TagKeySNO() { ID = 74071 };
        public static TagKeySNO MagicFindEffectGroup = new TagKeySNO() { ID = 74072 };
        public static TagKeySNO GoldFindEffectGroup = new TagKeySNO() { ID = 74073 };
        public static TagKeySNO AttackEffectGroup = new TagKeySNO() { ID = 74074 };
        public static TagKeySNO CastEffectGroup = new TagKeySNO() { ID = 74075 };
        public static TagKeySNO HolyEffectGroup = new TagKeySNO() { ID = 74076 };
        public static TagKeySNO Spell1EffectGroup = new TagKeySNO() { ID = 74077 };
        public static TagKeySNO Spell2EffectGroup = new TagKeySNO() { ID = 74078 };
    }

    public class TagMapEntry
    {
        public int Type { get; private set; }
        public int TagID { get; private set; }
        public ScriptFormula ScriptFormula { get; private set; }
        public int Int { get; private set; }
        public float Float { get; private set; }

        public TagMapEntry(int tag, int value, int type)
        {
            Type = type;
            TagID = tag;
            Int = value;
        }

        public override string ToString()
        {
            TagKeys.TagKey key = TagKeys.GetKey(TagID);

            switch (Type)
            {
                case 1: return String.Format("{0} = {1}", key != null ? key.Name : TagID.ToString(), Float);
                case 4: return String.Format("{0} = {1}", key != null ? key.Name : TagID.ToString(), ScriptFormula);
                default: return String.Format("{0} = {1}", key != null ? key.Name : TagID.ToString(), Int);
            }
        }

        public TagMapEntry(MpqFileStream stream)
        {
            this.Type = stream.ReadValueS32();
            this.TagID = stream.ReadValueS32();

            switch (this.Type)
            {
                case 0:
                    this.Int = stream.ReadValueS32();
                    break;
                case 1:
                    Float = stream.ReadValueF32();
                    break;
                case 2: // SNO
                    this.Int = stream.ReadValueS32();
                    break;
                case 4:
                    this.ScriptFormula = new ScriptFormula(stream);
                    break;
                default:
                    this.Int = stream.ReadValueS32();
                    break;
            }
        }
    }
}