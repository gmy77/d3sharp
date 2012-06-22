/*
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Common.Logging;
using Mooege.Core.GS.Players;


namespace Mooege.Core.GS.QuestEvents
{
    //TODO: Add attributes
    public abstract class QuestEvent
    {
        Logger logger = new Logger("Conversation");

        public uint ConversationSNOId { get; set; }


        public QuestEvent(uint conversationSNOId)
        {
            this.ConversationSNOId = conversationSNOId;
        }

        public abstract void Execute(Map.World world);
    }
}
