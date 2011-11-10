using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrystalMpq;
using Gibbed.IO;
using System.Reflection;
using Mooege.Core.GS.Common.Types.SNO;

namespace Mooege.Common.MPQ.FileFormats.Types
{
    public class TagMap : ISerializableData, IEnumerable<TagMapEntry>
    {
        public int TagMapSize { get; private set; }

        private Dictionary<int, TagMapEntry> _TagMapEntries { get; set; }

        [Obsolete("Use TagKeys instead")]
        public List<TagMapEntry> TagMapEntries
        {
            get
            {
                return _TagMapEntries.Values.ToList();
            }
        }

        [Obsolete("Use strong keys")]
        public bool ContainsKey(int key) { return _TagMapEntries.ContainsKey(key); }
        public bool ContainsKey(TagKey key) { return _TagMapEntries.ContainsKey(key.ID); }

        public void Add(TagKey key, TagMapEntry entry) { _TagMapEntries.Add(key.ID, entry) ; }


        public void Read(MpqFileStream stream)
        {
            object h = TagKeys.ArcaneEffectGroup;

            TagMapSize = stream.ReadValueS32();
            _TagMapEntries = new Dictionary<int, TagMapEntry>();

            for (int i = 0; i < TagMapSize; i++)
            {
                TagMapEntry entry = new TagMapEntry(stream);
                _TagMapEntries.Add(entry.TagID, entry);
            }
        }

        #region accessors

        public int this[TagKeyInt key]
        {
            get
            {
                return _TagMapEntries[key.ID].Int2;
            }
        }

        public float this[TagKeyFloat key]
        {
            get
            {
                return _TagMapEntries[key.ID].Float0;
            }
        }

        public ScriptFormula this[TagKeyScript key]
        {
            get
            {
                return _TagMapEntries[key.ID].ScriptFormula;
            }
        }

        public SNOName this[TagKeySNO key]
        {
            get
            {
                return new SNOName() { Group = 0, SNOId = _TagMapEntries[key.ID].Int2 };
            }
        }

        #endregion

        public IEnumerator<TagMapEntry> GetEnumerator()
        {
            return _TagMapEntries.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _TagMapEntries.Values.GetEnumerator();
        }

        [Obsolete("Use strong keys instead")]
        public TagMapEntry this[int key]
        {
            get
            {
                return _TagMapEntries[key];
            }
        }

    }

    public class TagKey { 
        public int ID; 
        public string Name;
    }

    public class TagKeyInt : TagKey { }
    public class TagKeyFloat : TagKey { }
    public class TagKeyScript : TagKey { }
    public class TagKeySNO : TagKey { }


    public class TagKeys
    {
        private static Dictionary<int, TagKey> tags = new Dictionary<int, TagKey>();

        public static TagKey GetKey(int index)
        {
            return tags.ContainsKey(index) ? tags[index] : null;
        }

        static TagKeys()
        {
            foreach (FieldInfo field in typeof(TagKeys).GetFields())
            {
                TagKey key = field.GetValue(null) as TagKey;
                key.Name = field.Name;
                tags.Add(key.ID, key);
            }
        }

        //524864 == hasinteractionoptions?

        // MarkerSet Tags
        public static TagKeySNO QuestRange = new TagKeySNO() { ID = 524544 };
        public static TagKeySNO ConversationList = new TagKeySNO() { ID = 526080 };
        public static TagKeyFloat Scale = new TagKeyFloat() { ID = 524288 };
        public static TagKeySNO OnActorSpawnedScript = new TagKeySNO() { ID = 524808 };
        public static TagKeyInt SpawnerGroupHash = new TagKeyInt() { ID = 524814 };

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
        public int Int2 { get; private set; }
        public float Float0 { get; private set; }

        public TagMapEntry(int tag, int value, int type)
        {
            Type = type;
            TagID = tag;
            Int2 = value;
        }

        public override string ToString()
        {
            TagKey key = TagKeys.GetKey(TagID);

            switch (Type)
            {
                case 0: return String.Format("{0} = {1}", key != null ? key.Name : TagID.ToString(), Int2);
                case 1: return String.Format("{0} = {1}", key != null ? key.Name : TagID.ToString(), Float0);
                case 2: return String.Format("{0} = {1}", key != null ? key.Name : TagID.ToString(), Int2);
                case 4: return String.Format("{0} = {1}", key != null ? key.Name : TagID.ToString(), ScriptFormula);
                default: return String.Format("{0} = {1}", key != null ? key.Name : TagID.ToString(), Int2);
            }
        }



        public TagMapEntry(MpqFileStream stream)
        {
            this.Type = stream.ReadValueS32();
            this.TagID = stream.ReadValueS32();

            switch (this.Type)
            {
                case 0:
                    this.Int2 = stream.ReadValueS32();
                    break;
                case 1:
                    Float0 = stream.ReadValueF32();
                    break;
                case 2: // SNO
                    this.Int2 = stream.ReadValueS32();
                    break;
                case 4:
                    this.ScriptFormula = new ScriptFormula(stream);
                    break;
                default:
                    this.Int2 = stream.ReadValueS32();
                    break;
            }
        }
    }
}
