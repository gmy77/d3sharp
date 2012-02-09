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

using Mooege.Core.GS.Map;
using Mooege.Core.GS.Common.Types.TagMap;

namespace Mooege.Core.GS.Actors.Implementations
{
    [HandledSNO(3739)]
    class CaptainRumford : InteractiveNPC
    {
        public CaptainRumford(World world, int snoID, TagMap tags)
            : base(world, snoID, tags)
        { }

        // One of the rumfords is not tagged with a conversation list, although his conversation list is available.
        // there may be two reasons for this: ConversationLists are not used anymore which i doubt as i works beautifully with them
        // or the information is no longer available in the client which would be possible tagging and stuff is only relevant to the server
        // TODO If the client lacks all information, we need a system to combine mpq data with custom data
        protected override void ReadTags()
        {
            if (!Tags.ContainsKey(MarkerKeys.ConversationList))
                Tags.Add(MarkerKeys.ConversationList, new TagMapEntry(MarkerKeys.ConversationList.ID, 108832, 2));

            base.ReadTags();
        }
    }
}
