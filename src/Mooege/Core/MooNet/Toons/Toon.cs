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
using System.Collections.Generic;
using System.Data.SQLite;
using Mooege.Common.Helpers.Hash;
using Mooege.Common.Storage;
using Mooege.Core.MooNet.Accounts;
using Mooege.Core.MooNet.Helpers;
using Mooege.Core.MooNet.Objects;
using Mooege.Core.GS.Skills;
using Mooege.Core.GS.Players;

namespace Mooege.Core.MooNet.Toons
{
    public class Toon : PersistentRPCObject
    {

        public IntPresenceField HeroClassField
            = new IntPresenceField(FieldKeyHelper.Program.D3, FieldKeyHelper.OriginatingClass.Hero, 1, 0);

        public IntPresenceField HeroLevelField
            = new IntPresenceField(FieldKeyHelper.Program.D3, FieldKeyHelper.OriginatingClass.Hero, 2, 0);

        public ByteStringPresenceField<D3.Hero.VisualEquipment> HeroVisualEquipmentField
            = new ByteStringPresenceField<D3.Hero.VisualEquipment>(FieldKeyHelper.Program.D3, FieldKeyHelper.OriginatingClass.Hero, 3, 0);

        public IntPresenceField HeroFlagsField
            = new IntPresenceField(FieldKeyHelper.Program.D3, FieldKeyHelper.OriginatingClass.Hero, 4, 0);

        public StringPresenceField HeroNameField
            = new StringPresenceField(FieldKeyHelper.Program.D3, FieldKeyHelper.OriginatingClass.Hero, 5, 0);

        public IntPresenceField HighestUnlockedAct
            = new IntPresenceField(FieldKeyHelper.Program.D3, FieldKeyHelper.OriginatingClass.Hero, 6, 0, 0);

        public IntPresenceField HighestUnlockedDifficulty
            = new IntPresenceField(FieldKeyHelper.Program.D3, FieldKeyHelper.OriginatingClass.Hero, 7, 0, 0);

        /// <summary>
        /// D3 EntityID encoded id.
        /// </summary>
        public D3.OnlineService.EntityId D3EntityID { get; private set; }

        /// <summary>
        /// True if toon has been recently deleted;
        /// </summary>
        private bool _deleted = false;
        public bool Deleted {
            get
            {
                return _deleted;
            }
            set
            {
                _deleted = value;
            }
        }

        /// <summary>
        /// Toon handle struct.
        /// </summary>
        public ToonHandleHelper ToonHandle { get; private set; }

        /// <summary>
        /// Toon's name.
        /// </summary>
        private string _name;
        public string Name {
            get
            {
                return _name;
            }
            private set
            {
                this._name = value;
                this.HeroNameField.Value = value;
            }
        }

        /// <summary>
        /// Toon's hash-code.
        /// </summary>
        public int HashCode { get; set; }

        /// <summary>
        /// Toon's owner account.
        /// </summary>
        public GameAccount GameAccount { get; set; }

        /// <summary>
        /// Toon's class.
        /// </summary>
        private ToonClass _class;
        public ToonClass Class
        {
            get
            {
                return _class;
            }
            private set
            {
                _class = value;
                switch (_class)
                {
                    case ToonClass.Barbarian:
                        this.HeroClassField.Value = 0x4FB91EE2;
                        break;
                    case ToonClass.DemonHunter:
                        this.HeroClassField.Value = unchecked((int)0xC88B9649);
                        break;
                    case ToonClass.Monk:
                        this.HeroClassField.Value = 0x3DAC15;
                        break;
                    case ToonClass.WitchDoctor:
                        this.HeroClassField.Value = 0x343C22A;
                        break;
                    case ToonClass.Wizard:
                        this.HeroClassField.Value = 0x1D4681B1;
                        break;
                    default:
                        this.HeroClassField.Value = 0x0;
                        break;
                }
            }
        }

        /// <summary>
        /// Toon's flags.
        /// </summary>
        private ToonFlags _flags;
        public ToonFlags Flags
        {
            get
            {
                return _flags;
            }
            private set
            {
                _flags = value | ToonFlags.AllUnknowns;
                this.HeroFlagsField.Value = (int)(value | ToonFlags.AllUnknowns);
            }
        }

        /// <summary>
        /// Toon's level.
        /// </summary>
        //TODO: Remove this as soon as everywhere the field is used
        private byte _level;
        public byte Level
        {
            get
            {
                return _level;
            }
            private set
            {
                this._level = value;
                this.HeroLevelField.Value = value;
            }
        }

        /// <summary>
        /// Experience to next level
        /// </summary>
        public int ExperienceNext { get; set; }

        /// <summary>
        /// Total time played for toon.
        /// </summary>
        public uint TimePlayed { get; set; }

        /// <summary>
        /// Last login time for toon.
        /// </summary>
        public uint LoginTime { get; set; }

        /// <summary>
        /// Settings for toon.
        /// </summary>
        private D3.Client.ToonSettings _settings = D3.Client.ToonSettings.CreateBuilder().Build();
        public D3.Client.ToonSettings Settings
        {
            get
            {
                return this._settings;
            }
            set
            {
                this._settings = value;
            }
        }

        /// <summary>
        /// Toon digest.
        /// </summary>
        public D3.Hero.Digest Digest
        {
            get
            {
                return D3.Hero.Digest.CreateBuilder().SetVersion(902)
                                .SetHeroId(this.D3EntityID)
                                .SetHeroName(this.Name)
                                .SetGbidClass((int)this.ClassID)
                                .SetPlayerFlags((uint)this.Flags)
                                .SetLevel(this.Level)
                                .SetVisualEquipment(this.HeroVisualEquipmentField.Value)
                                .SetLastPlayedAct(0)
                                .SetHighestUnlockedAct(0)
                                .SetLastPlayedDifficulty(0)
                                .SetHighestUnlockedDifficulty(0)
                                .SetLastPlayedQuest(-1)
                                .SetLastPlayedQuestStep(-1)
                                .SetTimePlayed(this.TimePlayed)
                                .Build();
            }
        }

        /// <summary>
        /// Hero Profile.
        /// </summary>
        public D3.Profile.HeroProfile Profile
        {
            get
            {
                return D3.Profile.HeroProfile.CreateBuilder()
                    .SetHardcore(false)
                    .SetHeroId(this.D3EntityID)
                    .SetHighestDifficulty(0)
                    .SetHighestLevel(this.Level)
                    .SetMonstersKilled(923)
                    .Build();
            }
        }

        public bool IsSelected
        {
            get
            {
                if (!this.GameAccount.IsOnline) return false;
                else
                {
                    if (this.GameAccount.CurrentToon != null)
                        return this.GameAccount.CurrentToon.D3EntityID == this.D3EntityID;
                    else
                        return false;
                }
            }
        }

        public int ClassID
        {
            get
            {
                switch (this.Class)
                {
                    case ToonClass.Barbarian:
                        return 0x4FB91EE2;
                    case ToonClass.DemonHunter:
                        return unchecked((int)0xC88B9649);
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

        public int VoiceClassID // Used for Conversations
        {
            get
            {
                switch (this.Class)
                {
                    case ToonClass.DemonHunter:
                        return 0;
                    case ToonClass.Barbarian:
                        return 1;
                    case ToonClass.Wizard:
                        return 2;
                    case ToonClass.WitchDoctor:
                        return 3;
                    case ToonClass.Monk:
                        return 4;
                }
                return 0;
            }
        }

        public int Gender
        {
            get
            {
                return (int)(this.Flags & ToonFlags.Female); // 0x00 for male, so we can just return the AND operation
            }
        }

        #region c-tor and setfields

        public Toon(string name, int hashCode, int classId, ToonFlags flags, byte level, GameAccount account) // Toon with **newly generated** persistent ID
            : base(StringHashHelper.HashIdentity(name + "#" + hashCode.ToString("D3")))
        {
            this.D3EntityID = D3.OnlineService.EntityId.CreateBuilder().SetIdHigh((ulong)EntityIdHelper.HighIdType.ToonId).SetIdLow(this.PersistentID).Build();

            this.Name = name;
            this.HashCode = hashCode;
            this.Class = @GetClassByID(classId);
            this.Flags = flags;
            this.Level = level;
            this.ExperienceNext = Player.LevelBorders[level];
            this.GameAccount = account;
            this.TimePlayed = 0;

            var visualItems = new[]
            {                                
                D3.Hero.VisualItem.CreateBuilder().SetEffectLevel(0).Build(), // Head
                D3.Hero.VisualItem.CreateBuilder().SetEffectLevel(0).Build(), // Chest
                D3.Hero.VisualItem.CreateBuilder().SetEffectLevel(0).Build(), // Feet
                D3.Hero.VisualItem.CreateBuilder().SetEffectLevel(0).Build(), // Hands
                D3.Hero.VisualItem.CreateBuilder().SetEffectLevel(0).Build(), // Weapon (1)
                D3.Hero.VisualItem.CreateBuilder().SetEffectLevel(0).Build(), // Weapon (2)
                D3.Hero.VisualItem.CreateBuilder().SetEffectLevel(0).Build(), // Shoulders
                D3.Hero.VisualItem.CreateBuilder().SetEffectLevel(0).Build(), // Legs
            };

            this.HeroVisualEquipmentField.Value = D3.Hero.VisualEquipment.CreateBuilder().AddRangeVisualItem(visualItems).Build();
        }

        public Toon(ulong persistentId)     // Load a toon from database with a given persistentId
            : base(persistentId)
        {
            this.D3EntityID = D3.OnlineService.EntityId.CreateBuilder().SetIdHigh((ulong)EntityIdHelper.HighIdType.ToonId).SetIdLow(this.PersistentID).Build();

            var sqlQuery  = string.Format("SELECT * FROM toons WHERE id = {0}", persistentId);
            var sqlCmd    = new SQLiteCommand(sqlQuery, DBManager.Connection);
            var sqlReader = sqlCmd.ExecuteReader();

            // Use name of column to prevent errors if column moved
            while (sqlReader.Read())
            {
                this.Name = Convert.ToString(sqlReader["name"]);
                this.HashCode = Convert.ToInt32(sqlReader["hashCode"]);
                this.Class = (ToonClass)Convert.ToInt32(sqlReader["class"]);
                this.Flags = (ToonFlags)Convert.ToInt32(sqlReader["gender"]);
                this.Level = Convert.ToByte(sqlReader["level"]);
                this.ExperienceNext = Convert.ToInt32(sqlReader["experience"]);
                this.GameAccount = GameAccountManager.GetAccountByPersistentID(Convert.ToUInt64(sqlReader["accountId"]));
                this.TimePlayed = Convert.ToUInt32(sqlReader["timePlayed"]);
                this.Deleted = Convert.ToBoolean(sqlReader["deleted"]);
            }

            var visualItems = new[]
            {                                
                D3.Hero.VisualItem.CreateBuilder().SetEffectLevel(0).Build(), // Head
                D3.Hero.VisualItem.CreateBuilder().SetEffectLevel(0).Build(), // Chest
                D3.Hero.VisualItem.CreateBuilder().SetEffectLevel(0).Build(), // Feet
                D3.Hero.VisualItem.CreateBuilder().SetEffectLevel(0).Build(), // Hands
                D3.Hero.VisualItem.CreateBuilder().SetEffectLevel(0).Build(), // Weapon (1)
                D3.Hero.VisualItem.CreateBuilder().SetEffectLevel(0).Build(), // Weapon (2)
                D3.Hero.VisualItem.CreateBuilder().SetEffectLevel(0).Build(), // Shoulders
                D3.Hero.VisualItem.CreateBuilder().SetEffectLevel(0).Build(), // Legs
            };
            
            // Load Visual Equipment
            Dictionary<int, int> visualToSlotMapping = new Dictionary<int, int>();
            visualToSlotMapping.Add(1, 0);
            visualToSlotMapping.Add(2, 1);
            visualToSlotMapping.Add(7, 2);
            visualToSlotMapping.Add(5, 3);
            visualToSlotMapping.Add(4, 4);
            visualToSlotMapping.Add(3, 5);
            visualToSlotMapping.Add(8, 6);
            visualToSlotMapping.Add(9, 7);
            
            //add visual equipment form DB, only the visualizable equipment, not everything
            var itemQuery = string.Format("SELECT * FROM inventory WHERE toon_id = {0} AND equipment_slot <> -1 AND item_id <> -1", persistentId);
            var itemCmd = new SQLiteCommand(itemQuery, DBManager.Connection);
            var itemReader = itemCmd.ExecuteReader();
            if (itemReader.HasRows)
            {
                while (itemReader.Read())
                {
                    var slot = Convert.ToInt32(itemReader["equipment_slot"]);
                    if (!visualToSlotMapping.ContainsKey(slot))
                        continue;
                    // decode vislual slot from equipment slot
                    slot = visualToSlotMapping[slot];
                    var gbid = Convert.ToInt32(itemReader["item_id"]);
                    visualItems[slot] = D3.Hero.VisualItem.CreateBuilder()
                        .SetGbid(gbid)
                        .SetEffectLevel(0)
                        .Build();
                }
            }
            this.HeroVisualEquipmentField.Value = D3.Hero.VisualEquipment.CreateBuilder().AddRangeVisualItem(visualItems).Build();
        }

        #endregion

        public void LevelUp()
        {
            this.Level++;
            this.GameAccount.ChangedFields.SetIntPresenceFieldValue(this.HeroLevelField);
        }

        #region Notifications

        //hero class generated
        //D3,Hero,1,0 -> D3.Hero.GbidClass: Hero Class
        //D3,Hero,2,0 -> D3.Hero.Level: Hero's current level
        //D3,Hero,3,0 -> D3.Hero.VisualEquipment: VisualEquipment
        //D3,Hero,4,0 -> D3.Hero.PlayerFlags: Hero's flags
        //D3,Hero,5,0 -> ?D3.Hero.NameText: Hero's Name
        //D3,Hero,6,0 -> Unk Int64 (0)
        //D3,Hero,7,0 -> Unk Int64 (0)

        public override List<bnet.protocol.presence.FieldOperation> GetSubscriptionNotifications()
        {
            var operationList = new List<bnet.protocol.presence.FieldOperation>();
            operationList.Add(this.HeroClassField.GetFieldOperation());
            operationList.Add(this.HeroLevelField.GetFieldOperation());
            operationList.Add(this.HeroVisualEquipmentField.GetFieldOperation());
            operationList.Add(this.HeroFlagsField.GetFieldOperation());
            operationList.Add(this.HeroNameField.GetFieldOperation());
            operationList.Add(this.HighestUnlockedAct.GetFieldOperation());
            operationList.Add(this.HighestUnlockedDifficulty.GetFieldOperation());

            return operationList;
        }

        #endregion

        private static ToonClass GetClassByID(int classId)
        {
            switch (classId)
            {
                case 0x4FB91EE2:
                    return ToonClass.Barbarian;
                case unchecked((int)0xC88B9649):
                    return ToonClass.DemonHunter;
                case 0x003DAC15:
                    return ToonClass.Monk;
                case 0x0343C22A:
                    return ToonClass.WitchDoctor;
                case 0x1D4681B1:
                    return ToonClass.Wizard;
            }

            return ToonClass.Barbarian;
        }

        public override string ToString()
        {
            return String.Format("{{ Toon: {0} [lowId: {1}] }}", this.Name, this.D3EntityID.IdLow);
        }

        #region DB

        public void SaveToDB()
        {
            try
            {
                // save character base data
                if (ExistsInDB())
                {
                    var query =
                        string.Format(
                            "UPDATE toons SET name='{0}', hashCode={1}, class={2}, gender={3}, level={4}, experience={5}, accountId={6}, timePlayed={7}, deleted={8} WHERE id={9}",
                            this.Name, this.HashCode, (byte)this.Class, (byte)this.Gender, this.Level, this.ExperienceNext, this.GameAccount.PersistentID, this.TimePlayed, this.Deleted ? 1 : 0, this.PersistentID);

                    var cmd = new SQLiteCommand(query, DBManager.Connection);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    var query =
                        string.Format(
                            "INSERT INTO toons (id, name, hashCode, class, gender, level, experience, timePlayed, accountId) VALUES({0},'{1}',{2},{3},{4},{5},{6},{7},{8})",
                            this.PersistentID, this.Name, this.HashCode, (byte)this.Class, (byte)this.Gender, this.Level, this.ExperienceNext, this.TimePlayed, this.GameAccount.PersistentID);

                    var cmd = new SQLiteCommand(query, DBManager.Connection);
                    cmd.ExecuteNonQuery();
                    Logger.Debug("Create Toon for the first time in DB {0}", this.PersistentID);
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

                //delete items from DB
                var itemQuery = string.Format("DELETE FROM inventory WHERE toon_id={0}", this.PersistentID);
                var itemCmd = new SQLiteCommand(itemQuery, DBManager.Connection);
                itemCmd.ExecuteNonQuery();

                //delete entry from active_skills table
                var asSkillquery = string.Format("DELETE FROM active_skills WHERE id_toon={0}", this.PersistentID);
                var asCmd = new SQLiteCommand(asSkillquery, DBManager.Connection);
                asCmd.ExecuteNonQuery();

                //delete the actual toon from toons table
                var query = string.Format("DELETE FROM toons WHERE id={0}", this.PersistentID);
                var cmd = new SQLiteCommand(query, DBManager.Connection);
                cmd.ExecuteNonQuery();
				
				Logger.Debug("Deleting toon {0}",this.PersistentID);
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
            var query = string.Format("SELECT id FROM toons WHERE id={0}", this.PersistentID);

            var cmd = new SQLiteCommand(query, DBManager.Connection);
            var reader = cmd.ExecuteReader();
            return reader.HasRows;
        }

        private bool VisualItemExistsInDb(int slot)
        {
            var query = string.Format("SELECT toon_id FROM inventory WHERE toon_id = {0} AND equipment_slot = {1}", this.PersistentID, slot);
            var cmd = new SQLiteCommand(query, DBManager.Connection);
            var reader = cmd.ExecuteReader();
            return reader.HasRows;
        }
    }
    #endregion

    #region Definitions and Enums
    //Order is important as actor voices and saved data is based on enum index
    public enum ToonClass
    {
        Barbarian, // 0x4FB91EE2
        Monk, // 0x3DAC15
        DemonHunter, // 0xC88B9649
        WitchDoctor, // 0x343C22A
        Wizard // 0x1D4681B1
    }

    [Flags]
    public enum ToonFlags : uint
    {
        Male = 0x00,
        Female = 0x02,
        // TODO: These two need to be figured out still.. /plash
        //Unknown1 = 0x20,
        Unknown2 = 0x40,
        Unknown3 = 0x80000,
        Unknown4 = 0x2000000,
        AllUnknowns = Unknown2 | Unknown3 | Unknown4
    }
    #endregion
}
