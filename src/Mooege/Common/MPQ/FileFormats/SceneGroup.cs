﻿/*
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

using System.Collections.Generic;
using CrystalMpq;
using Gibbed.IO;
using Mooege.Common.MPQ.FileFormats.Types;
using Mooege.Core.GS.Common.Types.SNO;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.SceneGroup)]
    public class SceneGroup : FileFormat
    {
        public Header Header { get; private set; }
        public int I0 { get; private set; }
        public List<SceneGroupItem> Items { get; private set; }
        public int I1 { get; private set; }

        public SceneGroup(MpqFile file)
        {
            var stream = file.Open();
            this.Header = new Header(stream);
            this.I0 = stream.ReadValueS32();
            this.Items = stream.ReadSerializedData<SceneGroupItem>();
            stream.Position += 8;
            this.I1 = stream.ReadValueS32();
            stream.Close();
        }
    }

    public class SceneGroupItem : ISerializableData
    {
        public int SNOScene { get; private set; }
        public int I0 { get; private set; }
        public int LabelGBId { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.SNOScene = stream.ReadValueS32();
            this.I0 = stream.ReadValueS32();
            this.LabelGBId = stream.ReadValueS32();
        }
    }
}