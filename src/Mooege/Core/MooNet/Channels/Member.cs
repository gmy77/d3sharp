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

namespace Mooege.Core.MooNet.Channels
{
    public class Member
    {
        // TODO: Need moar!
        public enum Role : uint
        {
            ChannelMember = 1,
            ChannelCreator = 2,
            PartyMember = 100,
            PartyLeader = 101 // There's a cap where no member has Role.ChannelCreator (which is plausible since games are actually channels)
        }

        // TODO: These are flags..
        [Flags]
        public enum Privilege : ulong
        {
            None = 0,
            Chat = 0x20000,

            // Combinations with unknowns..
            UnkCreator = 0x0000FBFF,
            UnkMember = 0x00030BAA,
            UnkMember2 = 0x00030B80,
            UnkJoinedMember = 0x0000DBC5
        }

        public bnet.protocol.Identity Identity { get; set; }
        public Privilege Privileges { get; set; }
        public List<Role> Roles { get; private set; }
        public bnet.protocol.AccountInfo Info { get; private set; }

        public bnet.protocol.channel.MemberState BnetMemberState
        {
            get
            {
                var builder = bnet.protocol.channel.MemberState.CreateBuilder();
                if (this.Privileges != Privilege.None)
                    builder.SetPrivileges((ulong)this.Privileges); // We don't have to set this if it is the default (0)
                foreach (var role in this.Roles)
                {
                    builder.AddRole((uint)role);
                }
                builder.SetInfo(this.Info);
                return builder.Build();
            }
        }

        public bnet.protocol.channel.Member BnetMember
        {
            get
            {
                return bnet.protocol.channel.Member.CreateBuilder()
                    .SetIdentity(this.Identity)
                    .SetState(this.BnetMemberState)
                    .Build();
            }
        }

        public Member(bnet.protocol.Identity identity, Privilege privs, params Role[] roles)
        {
            this.Identity = identity;
            this.Privileges = privs;
            this.Roles = new List<Role>();
            AddRoles(roles);
            this.Info = bnet.protocol.AccountInfo.CreateBuilder()
                .SetAccountPaid(true)
                .SetCountryId(21843)
                .Build();
        }

        public void AddRoles(params Role[] roles)
        {
            foreach (var role in roles)
                AddRole(role);
        }

        public void AddRole(Role role)
        {
            if (!this.Roles.Contains(role))
                this.Roles.Add(role);
        }
    }
}
