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

using Google.ProtocolBuffers;
using Mooege.Core.MooNet.Helpers;

namespace Mooege.Core.MooNet.Toons
{
    public class ToonHandleHelper
    {
        public ulong ID { get; private set; }

        // Commented out all Program, Region, Realm stuff because **as of right now** we don't section Toon High's with that fields.
        // All we have in hand is a default program, region, realm there. So after we get all stuff implemented, we should also
        // sectionize HighId's so we'll have technical correctiness :) 

        // And remember that this data is the sectioning for the EntityId High value. For type-checking we can't just do
        // eid.High == EntityHelper.ToonId, instead we need to mask off the first 7 bytes to get _only_ the object type.

        //public uint Program { get; private set; }
        //public uint Region { get; private set; }
        //public uint Realm { get; private set; }

        public ToonHandleHelper(ulong id)
        {
            this.ID = id;
            //this.Program = 0x00004433;
            //this.Region = 0x62;
            //this.Realm = 0x01;
        }

        public ToonHandleHelper(D3.OnlineService.EntityId entityID)
        {
            var stream = CodedInputStream.CreateInstance(entityID.ToByteArray());
            ulong tmp = 0;
            // I believe this actually calls ReadRawVarint64(), but just to be sure..
            stream.ReadUInt64(ref tmp);
            this.ID = tmp;
            //this.Program = stream.ReadRawVarint32();
            //this.Region = stream.ReadRawVarint32();
            //this.Realm = stream.ReadRawVarint32();
        }

        public D3.OnlineService.EntityId ToD3EntityID()
        {
            return D3.OnlineService.EntityId.CreateBuilder().SetIdHigh((ulong)EntityIdHelper.HighIdType.ToonId).SetIdLow(this.ID).Build();
        }

        public bnet.protocol.EntityId ToBnetEntityID()
        {
            return bnet.protocol.EntityId.CreateBuilder().SetHigh((ulong)EntityIdHelper.HighIdType.ToonId).SetLow(this.ID).Build();
        }
    }
}
