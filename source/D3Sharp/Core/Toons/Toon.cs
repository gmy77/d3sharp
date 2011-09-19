using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using D3Sharp.Core.Storage;
using D3Sharp.Utils;
using D3Sharp.Utils.Helpers;
using Google.ProtocolBuffers;

namespace D3Sharp.Core.Toons
{
    public class Toon
    {        
        private static readonly Logger Logger = LogManager.CreateLogger();

        /// <summary>
        /// The actual id.
        /// </summary>
        public ulong ID { get; private set; }

        /// <summary>
        /// D3 EntityID encoded id.
        /// </summary>
        public D3.OnlineService.EntityId D3EntityID { get; private set; }

        /// <summary>
        /// Bnet EntityID encoded id.
        /// </summary>
        public bnet.protocol.EntityId BnetEntityID { get; private set; }

        /// <summary>
        /// Toon handle struct.
        /// </summary>
        public ToonHandleHelper ToonHandle { get; private set; }
        public long AccountID { get; private set; }
        
        public string Name { get; private set; }
        public ToonClass Class { get; private set; }
        public ToonGender Gender { get; private set; }
        public byte Level { get; private set; }
        public D3.Hero.Digest Digest { get; private set; }
        public D3.Hero.VisualEquipment Equipment { get; private set; }

        public uint ClassID
        {
            get
            {
                switch (this.Class)
                {
                    case ToonClass.Barbarian:
                        return 0x4FB91EE2;
                    case ToonClass.DemonHunter:
                        return 0xc88b9649;
                    case ToonClass.Monk:
                        return 0x3DAC15;
                    case ToonClass.WitchDoctor:
                        return 0x343C22A;
                    case ToonClass.Wizard:
                        return 0x1D4681B1;
                }
                return 0x0;
            }
        }

        public uint GenderID
        {
            get {
                return this.Gender == ToonGender.Male ? (uint)0x0 : 0x2000002;
            }
        }

        public Toon(ulong id, string name, ToonClass @class, ToonGender gender, byte level, long accountId)
        {
            this.ID = id;
            this.ToonHandle = new ToonHandleHelper(id);
            this.D3EntityID = this.ToonHandle.ToD3EntityID();
            this.BnetEntityID = this.ToonHandle.ToBnetEntityID();
            this.Name = name;
            this.Class = @class;
            this.Gender = gender;
            this.Level = level;
            this.AccountID = accountId;


            var visualItems = new[]
                            {
                                // Some hack. We should either load strings and then hash it from DB or load hash directly from DB..
                                // Showing a head and a Wep to show how it works

                                // Head
                                D3.Hero.VisualItem.CreateBuilder()
                                    .SetGbid( (int)StringHashHelper.HashString2("Helm_002") )
                                    .SetDyeType(0)
                                    .SetItemEffectType(0)
                                    .SetEffectLevel(0)
                                    .Build(),

                                // Chest
                                D3.Hero.VisualItem.CreateBuilder()
                                    .SetGbid(0)
                                    .SetDyeType(0)
                                    .SetItemEffectType(0)
                                    .SetEffectLevel(0)
                                    .Build(),

                                // Feet
                                D3.Hero.VisualItem.CreateBuilder()
                                    .SetGbid(0)
                                    .SetDyeType(0)
                                    .SetItemEffectType(0)
                                    .SetEffectLevel(0)
                                    .Build(),

                                // Hands
                                D3.Hero.VisualItem.CreateBuilder()
                                    .SetGbid(0)
                                    .SetDyeType(0)
                                    .SetItemEffectType(0)
                                    .SetEffectLevel(0)
                                    .Build(),

                                // Weapon (1)
                                D3.Hero.VisualItem.CreateBuilder()
                                    .SetGbid( (int)StringHashHelper.HashString2("Unique_Mace_1H_012") )
                                    .SetDyeType(0)
                                    .SetItemEffectType(0)
                                    .SetEffectLevel(0)
                                    .Build(),

                                // Weapon (2)
                                D3.Hero.VisualItem.CreateBuilder()
                                    .SetGbid(0)
                                    .SetDyeType(0)
                                    .SetItemEffectType(0)
                                    .SetEffectLevel(0)
                                    .Build(),

                                // Shoulders
                                D3.Hero.VisualItem.CreateBuilder()
                                    .SetGbid(0)
                                    .SetDyeType(0)
                                    .SetItemEffectType(0)
                                    .SetEffectLevel(0)
                                    .Build(),

                                // Legs
                                D3.Hero.VisualItem.CreateBuilder()
                                    .SetGbid(0)
                                    .SetDyeType(0)
                                    .SetItemEffectType(0)
                                    .SetEffectLevel(0)
                                    .Build(),
                            };


            this.Equipment = D3.Hero.VisualEquipment.CreateBuilder().AddRangeVisualItem(visualItems).Build();

            this.Digest = D3.Hero.Digest.CreateBuilder().SetVersion(891)
                .SetHeroId(this.D3EntityID)
                .SetHeroName(this.Name)
                .SetGbidClass((int)this.ClassID)
                .SetPlayerFlags(this.GenderID)
                .SetLevel(this.Level)
                .SetVisualEquipment(this.Equipment)
                .SetLastPlayedAct(0)
                .SetHighestUnlockedAct(0)
                .SetLastPlayedDifficulty(0)
                .SetHighestUnlockedDifficulty(0)
                .SetLastPlayedQuest(-1)
                .SetLastPlayedQuestStep(-1)
                .SetTimePlayed(0)
                .Build();
        }

        public Toon(ulong id, string name, byte @class, byte gender, byte level, long accountId)
            : this(id, name, (ToonClass)@class, (ToonGender)gender, level, accountId)
        {
        }

        public Toon(string name, uint classId, ToonGender gender, byte level, long accountId)
            : this(StringHashHelper.HashString(name), name, GetClassByID(classId), gender , level, accountId)
        {
        }

        private static ToonClass GetClassByID(uint classId)
        {
            switch (classId)
            {
                case 0x4FB91EE2:
                    return ToonClass.Barbarian;
                case 0xc88b9649:
                    return ToonClass.DemonHunter;
                case 0x3DAC15:
                    return ToonClass.Monk;
                case 0x343C22A:
                    return ToonClass.WitchDoctor;
                case 0x1D4681B1:
                    return ToonClass.Wizard;
            }

            return ToonClass.Barbarian;
        }

        private static ToonGender GetGenderByID(uint genderId)
        {
            return genderId== 0x2000002 ? ToonGender.Female : ToonGender.Male;
        }

        public void SaveToDB()
        {
            try
            {                
                if (ExistsInDB())
                {
                    var query =
                        string.Format(
                            "UPDATE toons SET name='{0}', class={1}, gender={2}, level={3}, accountId={4} WHERE id={5}",
                            Name, (byte)Class, (byte)Gender, Level, this.AccountID, ID);

                    var cmd = new SQLiteCommand(query, DBManager.Connection);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    var query =
                        string.Format(
                            "INSERT INTO toons (id, name, class, gender, level, accountId) VALUES({0},'{1}',{2},{3},{4},{5})",
                            ID, Name, (byte) Class, (byte) Gender, Level, this.AccountID);

                    var cmd = new SQLiteCommand(query, DBManager.Connection);
                    cmd.ExecuteNonQuery();                    
                }
            }
            catch (Exception e)
            {
                Logger.ErrorException(e, "Toon.SaveToDB()");
            }
        }

        public bool DeleteFromDB()
        {
            try
            {
                // Remove from DB
                if (!ExistsInDB()) return false;

                var query = string.Format("DELETE FROM toons WHERE id={0}", this.ID);
                var cmd = new SQLiteCommand(query, DBManager.Connection);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Logger.ErrorException(e, "Toon.DeleteFromDB()");
                return false;
            }
        }

        private bool ExistsInDB()
        {
            var query =
                string.Format(
                    "SELECT id  from toons where id={0}",
                    this.ID);

            var cmd = new SQLiteCommand(query, DBManager.Connection);
            var reader = cmd.ExecuteReader();
            return reader.HasRows;
        }
    }

    public enum ToonClass
    {
        Barbarian, // 0x4FB91EE2
        Monk, // 0x3DAC15
        DemonHunter, // 0xc88b9649
        WitchDoctor, // 0x343C22A
        Wizard // 0x1D4681B1
    }

    public enum ToonGender
    {
        Male, // 0x0
        Female // 0x2000002
    }
}
