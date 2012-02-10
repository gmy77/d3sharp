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
using CrystalMpq;
using Gibbed.IO;
using Mooege.Common.MPQ.FileFormats.Types;
using Mooege.Core.GS.Common.Types.SNO;
using Mooege.Common.Storage;

namespace Mooege.Common.MPQ.FileFormats
{
    /// <summary>
    /// List of all possible conversations for an actor and when they are available
    /// </summary>
    [FileFormat(SNOGroup.ConversationList)]
    public class ConversationList : FileFormat
    {
        [PersistentProperty("ConversationList")]
        public List<ConversationListEntry> ConversationListEntries { get; private set; }

        public ConversationList() { }
    }

    public class ConversationListEntry
    {
        [PersistentProperty("SNOConversation")]
        public int SNOConv { get; private set; }

        [PersistentProperty("I0")]
        public int I0 { get; private set; }

        [PersistentProperty("I1")]
        public int I1 { get; private set; }

        [PersistentProperty("I2")]
        public int I2 { get; private set; }

        [PersistentProperty("GbidItem")]
        public int GbidItem { get; private set; }

        [PersistentProperty("Noname1")]
        public string Noname1 { get; private set; }

        [PersistentProperty("Noname2")]
        public string Noname2 { get; private set; }

        [PersistentProperty("SNOQuestCurrent")]
        public int SNOQuestCurrent { get; private set; }

        [PersistentProperty("I3")]
        public int I3 { get; private set; }

        [PersistentProperty("SNOQuestAssigned")]
        public int SNOQuestAssigned { get; private set; }

        [PersistentProperty("SNOQuestActive")]
        public int SNOQuestActive { get; private set; }

        [PersistentProperty("SNOQuestComplete")]
        public int SNOQuestComplete { get; private set; }

        [PersistentProperty("SNOQuestRange")]
        public int SNOQuestRange { get; private set; }

        [PersistentProperty("SNOLevelArea")]
        public int SNOLevelArea { get; private set; }

        public ConversationListEntry() { }
    }
}