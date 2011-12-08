using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Common.Types.TagMap;

namespace Mooege.Core.GS.Actors.Implementations
{
    [HandledSNO(3739)]
    class CaptainRumford : InteractiveNPC
    {
        public CaptainRumford(World world, int snoID, TagMap tags)
            : base(world, snoID, tags)
        {
        }

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
