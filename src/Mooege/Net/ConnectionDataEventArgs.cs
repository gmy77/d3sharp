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

using System.Collections.Generic;
using System.Linq;

namespace Mooege.Net
{
    public sealed class ConnectionDataEventArgs : ConnectionEventArgs
    {
        public IEnumerable<byte> Data { get; private set; }

        public ConnectionDataEventArgs(IConnection connection, IEnumerable<byte> data)
            : base(connection)
        {
            this.Data = data ?? new byte[0];
        }

        public override string ToString()
        {
            return Connection.RemoteEndPoint != null
                ? string.Format("{0}: {1} bytes", Connection.RemoteEndPoint, Data.Count())
                : string.Format("Not Connected: {0} bytes", Data.Count());
        }
    }
}

