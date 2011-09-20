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
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using D3Sharp.Core.Helpers;
using D3Sharp.Core.Storage;
using D3Sharp.Core.Toons;
using D3Sharp.Utils;
using D3Sharp.Utils.Helpers;

namespace D3Sharp.Core.Accounts
{
    public class Account
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        /// <summary>
        /// The actual id.
        /// </summary>
        public ulong ID { get; private set; }

        public bnet.protocol.EntityId BnetAccountID { get; private set; }
        public bnet.protocol.EntityId BnetGameAccountID { get; private set; }
        public string Email { get; private set; }

        public D3.Account.Digest Digest
        {
            get
            {
                var builder = D3.Account.Digest.CreateBuilder().SetVersion(99)
                    .SetBannerConfiguration(D3.Account.BannerConfiguration.CreateBuilder()
                                                .SetBackgroundColorIndex(0)
                                                .SetBannerIndex(3)
                                                .SetPattern(0)
                                                .SetPatternColorIndex(0)
                                                .SetPlacementIndex(0)
                                                .SetSigilAccent(0)
                                                .SetSigilMain(0)
                                                .SetSigilColorIndex(0)
                                                .SetUseSigilVariant(false)
                                                .Build())
                    .SetFlags(0);

                builder.SetLastPlayedHeroId(Toons.Count > 0
                                                ? Toons.First().Value.D3EntityID
                                                : D3.OnlineService.EntityId.CreateBuilder().SetIdHigh(0).SetIdLow(0).
                                                      Build());
                return builder.Build();
            }
        }

        public Dictionary<ulong, Toon> Toons
        {
            get { return ToonManager.GetToonsForAccount(this); }
        }

        public Account(ulong id, string email)
        {
            this.Email = email;
            this.ID = id;
            this.BnetAccountID = bnet.protocol.EntityId.CreateBuilder().SetHigh((ulong)EntityIdHelper.HighIdType.AccountId).SetLow(this.ID).Build();
            this.BnetGameAccountID = bnet.protocol.EntityId.CreateBuilder().SetHigh((ulong)EntityIdHelper.HighIdType.GameAccountId).SetLow(this.ID).Build();            
        }

        public Account(string email)
            : this(StringHashHelper.HashString(email), email)
        {
        }

        public void SaveToDB()
        {
            try
            {
                var query =
                    string.Format(
                        "INSERT INTO accounts (id, email) VALUES({0},'{1}')",
                        this.ID, this.Email);

                    var cmd = new SQLiteCommand(query, DBManager.Connection);
                    cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Logger.ErrorException(e, "SaveToDB()");
            }
        }
    }
}
