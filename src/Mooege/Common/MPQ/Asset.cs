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
using Mooege.Core.GS.Common.Types.SNO;
using Mooege.Common.Logging;

namespace Mooege.Common.MPQ
{
    public abstract class Asset
    {
        public SNOGroup Group { get; private set; }
        public Int32 SNOId { get; private set; }
        public string Name { get; private set; }
        public string FileName { get; protected set; }
        public Type Parser { get; set; }

        protected FileFormat _data = null;
        protected static readonly Logger Logger = LogManager.CreateLogger();

        public FileFormat Data
        {
            get
            {
                if (_data == null && SourceAvailable && Parser != null)
                {
                    try
                    {
                        RunParser();
                    }
                    catch (Exception e)
                    {
                        Logger.FatalException(e, "Bad MPQ detected, failed parsing asset: {0}", this.FileName);
                    }
                }
                return _data;
            }
        }

        protected abstract bool SourceAvailable { get; }


        public Asset(SNOGroup group, Int32 snoId, string name)
        {
            this.FileName = group + "\\" + name + FileExtensions.Extensions[(int)group];
            this.Group = group;
            this.SNOId = snoId;
            this.Name = name;
        }

        public abstract void RunParser();
    }
}
