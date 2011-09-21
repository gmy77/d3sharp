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
using D3Sharp.Core.Helpers;
using D3Sharp.Core.Objects;
using D3Sharp.Core.Storage;
using D3Sharp.Utils;
using D3Sharp.Utils.Helpers;

namespace D3Sharp.Core.Toons
{
    public class Toon : RPCObject
    {        
        private static readonly Logger Logger = LogManager.CreateLogger();

        /// <summary>
        /// The actual id.
        /// </summary>
        public ulong Id { get; private set; }

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

        //TODO: toons should be linked to accounts actually here /raist.

        public int ClassID
        {
            get
            {
                switch (this.Class)
                {
                    case ToonClass.Barbarian:
                        return 0x4FB91EE2;
                    case ToonClass.DemonHunter:
                        return -930376119;
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
            this.Id = id;
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

        public Toon(string name, int classId, ToonGender gender, byte level, long accountId)
            : this(StringHashHelper.HashString(name), name, GetClassByID(classId), gender , level, accountId)
        {
        }

        private static ToonClass GetClassByID(int classId)
        {
            switch (classId)
            {
                case 0x4FB91EE2:
                    return ToonClass.Barbarian;
                case -930376119:
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

        public override void NotifySubscriber(Net.BNet.BNetClient client)
        {
            // check d3sharp / docs / rpc / notification-data-layout.txt  for fields keys.

            // banner config
            var fieldKey1 = FieldKeyHelper.Create(FieldKeyHelper.Program.D3, 2, 1, 0);
            var field1 = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey1).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(client.Account.BannerConfiguration.ToByteString()).Build()).Build();
            var fieldOperation1 = bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field1).Build();

            // class
            var fieldKey2 = FieldKeyHelper.Create(FieldKeyHelper.Program.D3, 3, 1, 0);
            var field2 = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey2).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue(this.ClassID).Build()).Build();
            var fieldOperation2 = bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field2).Build();

            // level
            var fieldKey3 = FieldKeyHelper.Create(FieldKeyHelper.Program.D3, 3, 2, 0);
            var field3 = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey3).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue(this.Level).Build()).Build();
            var fieldOperation3 = bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field3).Build();

            // equipment
            var fieldKey4 = FieldKeyHelper.Create(FieldKeyHelper.Program.D3, 3, 3, 0);
            var field4 = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey4).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(this.Equipment.ToByteString()).Build()).Build();
            var fieldOperation4 = bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field4).Build();

            // gender
            var fieldKey5 = FieldKeyHelper.Create(FieldKeyHelper.Program.D3, 3, 4, 0);
            var field5 = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey5).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue(this.GenderID).Build()).Build();
            var fieldOperation5 = bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field5).Build();

            // name
            var fieldKey6 = FieldKeyHelper.Create(FieldKeyHelper.Program.BNet, 3, 2, 0);
            var field6 = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey6).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetStringValue(this.Name).Build()).Build();
            var fieldOperation6 = bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field6).Build();

            // unknown boolean - probably harcore mode enabled?? /raist
            var fieldKey7 = FieldKeyHelper.Create(FieldKeyHelper.Program.BNet, 3, 3, 0);
            var field7 = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey7).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetBoolValue(false).Build()).Build();
            var fieldOperation7 = bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field7).Build();

            // program - FourCC "D3"
            var fieldKey8 = FieldKeyHelper.Create(FieldKeyHelper.Program.BNet, 3, 9, 0);
            var field8 = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey8).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetFourccValue("D3").Build()).Build();
            var fieldOperation8 = bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field8).Build();

            // unknown int  - maybe highest completed act? /raist.
            var fieldKey9 = FieldKeyHelper.Create(FieldKeyHelper.Program.BNet, 3, 9, 10);
            var field9 = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey9).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue(0).Build()).Build();
            var fieldOperation9 = bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field9).Build();

            // cretea presence.ChannelState
            var state = bnet.protocol.presence.ChannelState.CreateBuilder().SetEntityId(this.BnetEntityID)
                .AddFieldOperation(fieldOperation1).AddFieldOperation(fieldOperation2).AddFieldOperation(fieldOperation3).AddFieldOperation(fieldOperation4)
                .AddFieldOperation(fieldOperation5).AddFieldOperation(fieldOperation6).AddFieldOperation(fieldOperation7).AddFieldOperation(fieldOperation8)
                .AddFieldOperation(fieldOperation9).Build();

            // embed in  channel.ChannelState
            var channelState = bnet.protocol.channel.ChannelState.CreateBuilder().SetExtension(bnet.protocol.presence.ChannelState.Presence, state);

            // put in addnotification message
            var builder = bnet.protocol.channel.AddNotification.CreateBuilder().SetChannelState(channelState);

            // make the rpc call.
            client.CallMethod(bnet.protocol.channel.ChannelSubscriber.Descriptor.FindMethodByName("NotifyAdd"), builder.Build(),this.LocalObjectId);
        }

        public override void NotifyAllSubscriber()
        {
            foreach (var subscriber in this.Subscribers)
            {
                this.NotifySubscriber(subscriber);
            }
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
                            Name, (byte)Class, (byte)Gender, Level, this.AccountID, Id);

                    var cmd = new SQLiteCommand(query, DBManager.Connection);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    var query =
                        string.Format(
                            "INSERT INTO toons (id, name, class, gender, level, accountId) VALUES({0},'{1}',{2},{3},{4},{5})",
                            Id, Name, (byte) Class, (byte) Gender, Level, this.AccountID);

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

                var query = string.Format("DELETE FROM toons WHERE id={0}", this.Id);
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
                    this.Id);

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
