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
using System.Data.SQLite;
using Mooege.Common.Helpers;
using Mooege.Common.Helpers.Hash;
using Mooege.Common.Storage;
using Mooege.Core.MooNet.Accounts;
using Mooege.Core.MooNet.Helpers;
using Mooege.Core.MooNet.Objects;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Toons
{
    public class Toon : PersistentRPCObject
    {
        /// <summary>
        /// D3 EntityID encoded id.
        /// </summary>
        public D3.OnlineService.EntityId D3EntityID { get; private set; }

        /// <summary>
        /// Bnet EntityID encoded id.
        /// </summary>
        //public bnet.protocol.EntityId BnetEntityID { get; private set; }

        /// <summary>
        /// Toon handle struct.
        /// </summary>
        public ToonHandleHelper ToonHandle { get; private set; }

        /// <summary>
        /// Toon's name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Toon's hash-code.
        /// </summary>
        public int HashCode { get; set; }

        /// <summary>
        /// Toon's hash-code as string.
        /// </summary>
        public string HashCodeString { get; private set; }

        /// <summary>
        /// NameText as name#hashcode.
        /// </summary>
        public D3.Hero.NameText NameText { get; private set; }

        /// <summary>
        /// Toon's owner account.
        /// </summary>
        public GameAccount GameAccount { get; set; }

        /// <summary>
        /// Toon's class.
        /// </summary>
        public ToonClass Class { get; private set; }

        /// <summary>
        /// Toon's flags.
        /// </summary>
        public ToonFlags Flags { get; private set; }

        /// <summary>
        /// Toon's level.
        /// </summary>
        public byte Level { get; private set; }

        /// <summary>
        /// Total time played for toon.
        /// </summary>
        public uint TimePlayed { get; set; }

        /// <summary>
        /// Last login time for toon.
        /// </summary>
        public uint LoginTime { get; set; }

        /// <summary>
        /// The visual equipment for toon.
        /// </summary>
        public D3.Hero.VisualEquipment Equipment { get; protected set; }

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
                return D3.Hero.Digest.CreateBuilder().SetVersion(893)
                                .SetHeroId(this.D3EntityID)
                    //.SetHeroName(this.Name) //no longer used in 7728, uses D3.Hero.NameText query -Egris
                                .SetGbidClass((int)this.ClassID)
                                .SetPlayerFlags((uint)this.Flags)
                                .SetLevel(this.Level)
                                .SetVisualEquipment(this.Equipment)
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
                    if (this.GameAccount.LoggedInClient.CurrentToon != null)
                        return this.GameAccount.LoggedInClient.CurrentToon.D3EntityID == this.D3EntityID;
                    else
                        return false;
                }
            }
        }

        public Toon(ulong persistentId, string name, int hashCode, byte @class, byte gender, byte level, long accountId, uint timePlayed) // Toon with given persistent ID
            : base(persistentId)
        {
            this.SetFields(name, hashCode, (ToonClass)@class, (ToonFlags)gender, level, GameAccountManager.GetAccountByPersistentID((ulong)accountId), timePlayed);
        }

        public Toon(string name, int hashCode, int classId, ToonFlags flags, byte level, GameAccount account) // Toon with **newly generated** persistent ID
            : base(StringHashHelper.HashIdentity(name + "#" + hashCode.ToString("D3")))
        {
            this.SetFields(name, hashCode, GetClassByID(classId), flags, level, account, 0);
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

        private void SetFields(string name, int hashCode, ToonClass @class, ToonFlags flags, byte level, GameAccount owner, uint timePlayed)
        {
            //this.ToonHandle = new ToonHandleHelper(this.PersistentID);
            //this.D3EntityID = this.ToonHandle.ToD3EntityID();
            //this.BnetEntityID = this.ToonHandle.ToBnetEntityID();
            this.D3EntityID = D3.OnlineService.EntityId.CreateBuilder().SetIdHigh((ulong)EntityIdHelper.HighIdType.ToonId + this.PersistentID).SetIdLow(this.PersistentID).Build();
            //this.BnetEntityID = bnet.protocol.EntityId.CreateBuilder().SetHigh((ulong)EntityIdHelper.HighIdType.ToonId + this.PersistentID).SetLow(this.PersistentID).Build();
            this.Name = name;
            this.HashCode = hashCode;
            this.HashCodeString = HashCode.ToString("D3");
            this.NameText = D3.Hero.NameText.CreateBuilder().SetName(string.Format("{0}#{1}", this.Name, this.HashCodeString)).Build();
            this.Class = @class;
            this.Flags = flags;
            this.Level = level;
            this.GameAccount = owner;
            this.TimePlayed = timePlayed;

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

            this.Equipment = D3.Hero.VisualEquipment.CreateBuilder().AddRangeVisualItem(visualItems).Build();

        }


        public void LevelUp()
        {
            this.Level++;
        }

        private static ToonClass GetClassByID(int classId)
        {
            switch (classId)
            {
                case 0x4FB91EE2:
                    return ToonClass.Barbarian;
                case unchecked((int)0xC88B9649):
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

        public override string ToString()
        {
            return String.Format("{{ Toon: {0} [lowId: {1}] }}", this.Name, this.D3EntityID.IdLow);
        }

        public void SaveToDB()
        {
            try
            {
                if (ExistsInDB())
                {
                    var query =
                        string.Format(
                            "UPDATE toons SET name='{0}', hashCode={1}, class={2}, gender={3}, level={4}, accountId={5}, timePlayed={6} WHERE id={7}",
                            Name, this.HashCode, (byte)this.Class, (byte)this.Gender, this.Level, this.GameAccount.PersistentID, this.TimePlayed, this.PersistentID);

                    var cmd = new SQLiteCommand(query, DBManager.Connection);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    var query =
                        string.Format(
                            "INSERT INTO toons (id, name, hashCode, class, gender, level, timePlayed, accountId) VALUES({0},'{1}',{2},{3},{4},{5},{6},{7})",
                            this.PersistentID, this.Name, this.HashCode, (byte)this.Class, (byte)this.Gender, this.Level, this.TimePlayed, this.GameAccount.PersistentID);

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

                var query = string.Format("DELETE FROM toons WHERE id={0}", this.PersistentID);
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
                    "SELECT id from toons where id={0}",
                    this.PersistentID);

            var cmd = new SQLiteCommand(query, DBManager.Connection);
            var reader = cmd.ExecuteReader();
            return reader.HasRows;
        }
    }

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
        Unknown1 = 0x20,
        Unknown2 = 0x40,
        Unknown3 = 0x80000,
        Unknown4 = 0x2000000,
        AllUnknowns = Unknown1 | Unknown2 | Unknown3 | Unknown4
    }

}
