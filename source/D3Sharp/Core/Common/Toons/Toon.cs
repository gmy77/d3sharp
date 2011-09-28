/*
 * Copyright (C) 2011 D3Sharp Project
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
using System.Data.SQLite;
//<<<<<<< HEAD
//using System.Linq;
//using System.Collections.Generic;
//using D3.Account;
//using D3Sharp.Core.Accounts;
//using D3Sharp.Core.Channels;
//=======
using D3Sharp.Core.BNet.Accounts;
using D3Sharp.Core.BNet.Objects;
using D3Sharp.Core.Common.Storage;
//>>>>>>> 8574c238179fe15ed89f064963d15f70293617bc
using D3Sharp.Core.Helpers;
using D3Sharp.Utils.Helpers;
using Account = D3Sharp.Core.BNet.Accounts.Account;

namespace D3Sharp.Core.Common.Toons
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
        public bnet.protocol.EntityId BnetEntityID { get; private set; }

        /// <summary>
        /// Toon handle struct.
        /// </summary>
        public ToonHandleHelper ToonHandle { get; private set; }

        public string Name { get; private set; }
        public ToonClass Class { get; private set; }
        public ToonFlags Flags { get; private set; }
        public byte Level { get; private set; }
        public D3.Hero.Digest Digest { get; private set; }
        public D3.Hero.VisualEquipment Equipment { get; private set; }

        public Account Owner { get; set; }       
        
        //TODO: Toons should be linked to accounts here. /raist

        public Toon(ulong persistantId, string name, byte @class, byte gender, byte level, long accountId) // Toon with given persistent ID
            :base(persistantId)
        {
            this.SetFields(name, (ToonClass)@class, (ToonFlags)gender, level, AccountManager.GetAccountByPersistantID((ulong)accountId));
        }

        public Toon(string name, int classId, ToonFlags flags, byte level, Account account) // Toon with **newly generated** persistent ID
            : base(StringHashHelper.HashIdentity(name))
        {
            this.SetFields(name, GetClassByID(classId), flags, level, account);
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

        public int Gender
        {
            get
            {
                return (int)(this.Flags & ToonFlags.Female); // 0x00 for male, so we can just return the AND operation
            }
        }

//<<<<<<< HEAD
//        public Toon(ulong persistantId, string name, byte @class, byte gender, byte level, long accountId) // Toon with given persistent ID
//            :base(persistantId)
//        {
//            this.SetFields(name, (ToonClass)@class, (ToonFlags)gender, level, AccountManager.GetAccountByPersistantID((ulong)accountId));
//        }

//        public Toon(string name, int classId, ToonFlags flags, byte level, Account account) // Toon with **newly generated** persistent ID
//            : base(StringHashHelper.HashIdentity(name))
//        {
//            this.SetFields(name, GetClassByID(classId), flags, level, account);
//        }

//=======
//>>>>>>> 8574c238179fe15ed89f064963d15f70293617bc
        private void SetFields(string name, ToonClass @class, ToonFlags flags, byte level, Account owner)
        {
            this.ToonHandle = new ToonHandleHelper(this.PersistentID);
            this.D3EntityID = this.ToonHandle.ToD3EntityID();
            this.BnetEntityID = this.ToonHandle.ToBnetEntityID();
            this.Name = name;
            this.Class = @class;
            this.Flags = flags;
            this.Level = level;
            this.Owner = owner;

            var itemsGenerator = new Items.ItemTypeGenerator();

            var visualItems = new[]
                            {
                                // Head
                                D3.Hero.VisualItem.CreateBuilder()
                                    .SetGbid( itemsGenerator.generateRandomElement(Items.ItemType.Helm).Gbid)
                                    .SetDyeType(0)
                                    .SetItemEffectType(0)
                                    .SetEffectLevel(0)
                                    .Build(),

                                // Chest
                                D3.Hero.VisualItem.CreateBuilder()
                                    .SetGbid(itemsGenerator.generateRandomElement(Items.ItemType.ChestArmor).Gbid)
                                    .SetDyeType(0)
                                    .SetItemEffectType(0)
                                    .SetEffectLevel(0)
                                    .Build(),

                                // Feet
                                D3.Hero.VisualItem.CreateBuilder()
                                    .SetGbid(itemsGenerator.generateRandomElement(Items.ItemType.Boots).Gbid)
                                    .SetDyeType(0)
                                    .SetItemEffectType(0)
                                    .SetEffectLevel(0)
                                    .Build(),

                                // Hands
                                D3.Hero.VisualItem.CreateBuilder()
                                    .SetGbid(itemsGenerator.generateRandomElement(Items.ItemType.Gloves).Gbid)
                                    .SetDyeType(0)
                                    .SetItemEffectType(0)
                                    .SetEffectLevel(0)
                                    .Build(),

                                // Weapon (1)
                                D3.Hero.VisualItem.CreateBuilder()
                                    .SetGbid(itemsGenerator.generateRandomElement(Items.ItemType.Sword_1H).Gbid)
                                    .SetDyeType(0)
                                    .SetItemEffectType(0)
                                    .SetEffectLevel(0)
                                    .Build(),

                                // Weapon (2)
                                D3.Hero.VisualItem.CreateBuilder()
                                    .SetGbid(itemsGenerator.generateRandomElement(Items.ItemType.Shield).Gbid)
                                    .SetDyeType(0)
                                    .SetItemEffectType(0)
                                    .SetEffectLevel(0)
                                    .Build(),

                                // Shoulders
                                D3.Hero.VisualItem.CreateBuilder()
                                    .SetGbid(itemsGenerator.generateRandomElement(Items.ItemType.Shoulders).Gbid)
                                    .SetDyeType(0)
                                    .SetItemEffectType(0)
                                    .SetEffectLevel(0)
                                    .Build(),

                                // Legs
                                D3.Hero.VisualItem.CreateBuilder()
                                    .SetGbid(itemsGenerator.generateRandomElement(Items.ItemType.Pants).Gbid)
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
                .SetPlayerFlags((uint)this.Flags)
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

        public bnet.protocol.presence.Field QueryField(bnet.protocol.presence.FieldKey queryKey)
        {
            var field = bnet.protocol.presence.Field.CreateBuilder().SetKey(queryKey);

            switch ((FieldKeyHelper.Program)queryKey.Program)
            {
                case FieldKeyHelper.Program.D3:
                    if (queryKey.Group == 2 && queryKey.Field == 1) // Banner configuration
                    {
                        field.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(this.Owner.BannerConfiguration.ToByteString()).Build());
                    }
                    else if (queryKey.Group == 3 && queryKey.Field == 1) // Hero's class (GbidClass)
                    {
                        field.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue(this.ClassID).Build());
                    }
                    else if (queryKey.Group == 3 && queryKey.Field == 2) // Hero's current level
                    {
                        field.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue(this.Level).Build());
                    }
                    else if (queryKey.Group == 3 && queryKey.Field == 3) // Hero's visible equipment
                    {
                        field.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(this.Equipment.ToByteString()).Build());
                    }
                    else if (queryKey.Group == 3 && queryKey.Field == 4) // Hero's flags (gender and such)
                    {
                        field.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue((uint)(this.Flags | ToonFlags.BothUnknowns)).Build());
                    }
                    else if (queryKey.Group == 4 && queryKey.Field == 1) // Channel ID if the client is online
                    {
                        if(this.Owner.LoggedInBNetClient!=null) field.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(this.Owner.LoggedInBNetClient.CurrentChannel.D3EntityId.ToByteString()).Build());
                    }
                    else if (queryKey.Group == 4 && queryKey.Field == 2) // Current screen (all known values are just "in-menu"; also see ScreenStatuses sent in ChannelService.UpdateChannelState)
                    {
                        field.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue(0).Build());
                    }
                    break;
                case FieldKeyHelper.Program.BNet:
                    if (queryKey.Group == 3 && queryKey.Field == 2) // Toon name
                    {
                        field.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetStringValue(this.Name).Build());
                    }
                    else if (queryKey.Group == 3 && queryKey.Field == 3) // Whether the toon is online
                    {
                        field.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetBoolValue(true).Build());
                    }
                    else if (queryKey.Group == 3 && queryKey.Field == 5) // Away status - 0 for online
                    {
                        field.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue(0).Build());
                    }
                    else if (queryKey.Group == 3 && queryKey.Field == 9) // Program - always D3
                    {
                        field.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetFourccValue("D3").Build());
                    }
                    break;
            }

            return field.HasValue ? field.Build() : null;
        }

        protected override void NotifySubscriptionAdded(Net.BNet.BNetClient client)
        {
            // Check docs/rpc/fields.txt for fields keys

            // Banner configuration
            var fieldKey1 = FieldKeyHelper.Create(FieldKeyHelper.Program.D3, 2, 1, 0);
            var field1 = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey1).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(client.Account.BannerConfiguration.ToByteString()).Build()).Build();
            var fieldOperation1 = bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field1).Build();

            // Class
            var fieldKey2 = FieldKeyHelper.Create(FieldKeyHelper.Program.D3, 3, 1, 0);
            var field2 = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey2).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue(this.ClassID).Build()).Build();
            var fieldOperation2 = bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field2).Build();

            // Level
            var fieldKey3 = FieldKeyHelper.Create(FieldKeyHelper.Program.D3, 3, 2, 0);
            var field3 = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey3).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue(this.Level).Build()).Build();
            var fieldOperation3 = bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field3).Build();

            // Equipment
            var fieldKey4 = FieldKeyHelper.Create(FieldKeyHelper.Program.D3, 3, 3, 0);
            var field4 = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey4).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(this.Equipment.ToByteString()).Build()).Build();
            var fieldOperation4 = bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field4).Build();

            // Flags
            var fieldKey5 = FieldKeyHelper.Create(FieldKeyHelper.Program.D3, 3, 4, 0);
            var field5 = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey5).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue((uint)(this.Flags | ToonFlags.BothUnknowns)).Build()).Build();
            var fieldOperation5 = bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field5).Build();

            // Name
            var fieldKey6 = FieldKeyHelper.Create(FieldKeyHelper.Program.BNet, 3, 2, 0);
            var field6 = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey6).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetStringValue(this.Name).Build()).Build();
            var fieldOperation6 = bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field6).Build();

            // Whether the toon is online
            var fieldKey7 = FieldKeyHelper.Create(FieldKeyHelper.Program.BNet, 3, 3, 0);
            var field7 = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey7).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetBoolValue(true).Build()).Build();
            var fieldOperation7 = bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field7).Build();

            // Program - FourCC "D3"
            var fieldKey8 = FieldKeyHelper.Create(FieldKeyHelper.Program.BNet, 3, 9, 0);
            var field8 = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey8).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetFourccValue("D3").Build()).Build();
            var fieldOperation8 = bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field8).Build();

            // Unknown int - maybe highest completed act? /raist
            var fieldKey9 = FieldKeyHelper.Create(FieldKeyHelper.Program.BNet, 3, 9, 10);
            var field9 = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey9).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue(0).Build()).Build();
            var fieldOperation9 = bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field9).Build();

            // Create a presence.ChannelState
            var state = bnet.protocol.presence.ChannelState.CreateBuilder().SetEntityId(this.BnetEntityID)
                .AddFieldOperation(fieldOperation1).AddFieldOperation(fieldOperation2).AddFieldOperation(fieldOperation3).AddFieldOperation(fieldOperation4)
                .AddFieldOperation(fieldOperation5).AddFieldOperation(fieldOperation6).AddFieldOperation(fieldOperation7).AddFieldOperation(fieldOperation8)
                .AddFieldOperation(fieldOperation9).Build();

            // Embed in channel.ChannelState
            var channelState = bnet.protocol.channel.ChannelState.CreateBuilder().SetExtension(bnet.protocol.presence.ChannelState.Presence, state);

            // Put in AddNotification message
            var builder = bnet.protocol.channel.AddNotification.CreateBuilder().SetChannelState(channelState);

            // Make the RPC call
            client.CallMethod(bnet.protocol.channel.ChannelSubscriber.Descriptor.FindMethodByName("NotifyAdd"), builder.Build(),this.DynamicId);
        }

        public override string ToString()
        {
            return String.Format("Name: {0} High: {1} Low: {2}", this.Name, this.BnetEntityID.High,
                                 this.BnetEntityID.Low);
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
                            Name, (byte)this.Class, (byte)this.Gender, this.Level, this.Owner.PersistentID, this.PersistentID);

                    var cmd = new SQLiteCommand(query, DBManager.Connection);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    var query =
                        string.Format(
                            "INSERT INTO toons (id, name, class, gender, level, accountId) VALUES({0},'{1}',{2},{3},{4},{5})",
                            this.PersistentID, this.Name, (byte)this.Class, (byte)this.Gender, this.Level, this.Owner.PersistentID);

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
//<<<<<<< HEAD

//        //////////////////////////////////////////////////////////////////////////
//        // ingame data required by the universe follows

//        public int CurrentWorldID;
//        public int CurrentWorldSNO;
//        public float PosX, PosY, PosZ;
//        public List<int> RevealedWorlds;
//=======
//>>>>>>> 8574c238179fe15ed89f064963d15f70293617bc
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
        Male=0x00,
        Female=0x02,
        // TODO: These two need to be figured out still.. /plash
        Unknown1=0x20,
        Unknown2=0x2000000,
        BothUnknowns=Unknown1 | Unknown2
    }
}
