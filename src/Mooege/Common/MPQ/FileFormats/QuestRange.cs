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

using CrystalMpq;
using Gibbed.IO;
using Mooege.Core.GS.Common.Types.SNO;
using Mooege.Common.MPQ.FileFormats.Types;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.QuestRange)]
    public class QuestRange : FileFormat
    {
        public Header Header { get; private set; }
        public QuestTime Start { get; private set; }
        public QuestTime End { get; private set; }

        public QuestRange(MpqFile file)
        {
            var stream = file.Open();
            this.Header = new Header(stream);
            this.Start = new QuestTime(stream);
            this.End = new QuestTime(stream);
            stream.Close();
        }
    }

    public class QuestTime
    {
        public int SNOQuest { get; private set; }
        public int StepID { get; private set; }

        public QuestTime(MpqFileStream stream)
        {
            this.SNOQuest = stream.ReadValueS32();
            this.StepID = stream.ReadValueS32();
        }
    }
}