using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Common.Helpers;
using System.Diagnostics;
using Mooege.Net.GS.Message;
using Mooege.Common;
using Mooege.Core.GS.Players;
using Mooege.Net.GS.Message.Fields;
using Mooege.Core.GS.Map;
using Mooege.Common.MPQ;
using Mooege.Net.GS.Message.Definitions.World;
using Mooege.Core.GS.Common.Types.SNO;
using Mooege.Core.GS.Markers;

namespace Mooege.Core.Common.Items.Implementations
{
    [HandledType("Book")]
    public class Book : Item
    {
        public static readonly Logger Logger = LogManager.CreateLogger();

        public int LoreSNOId { get; private set; }

        public Book(World world, Mooege.Common.MPQ.FileFormats.ItemTable definition)
            : base(world, definition)
        {
            // Items are NOT constructed with tags
            var actorData = (Mooege.Common.MPQ.FileFormats.Actor)Mooege.Common.MPQ.MPQStorage.Data.Assets[SNOGroup.Actor][this.SNOId].Data;
            var loreTagEntry = actorData.TagMap.TagMapEntries.FirstOrDefault(x => x.TagID == (int)MarkerTagTypes.LoreSNOId);
            if (loreTagEntry != null)
            {
                LoreSNOId = loreTagEntry.Int2;
            }
        }

/*
  // Items are NOT constructed with tags!
        protected override void ReadTags()
        {
            base.ReadTags();
            if (this.Tags.ContainsKey((int)MarkerTagTypes.LoreSNOId))
            {
                LoreSNOId = Tags[(int)MarkerTagTypes.LoreSNOId].Int2;
            }
            else
            {
                LoreSNOId = -1;
            }
        }
*/
        public override void OnTargeted(Player player, TargetMessage message)
        {
            //Logger.Trace("OnTargeted");
            if (LoreSNOId != -1)
            {
                player.PlayLore(LoreSNOId, true);
            }
            if (player.GroundItems.ContainsKey(this.DynamicID))
                player.GroundItems.Remove(this.DynamicID);
            this.Destroy();
        }
    }
}
