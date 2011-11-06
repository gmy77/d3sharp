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
using Mooege.Common.Helpers.Assets;

namespace Mooege.Core.Common.Items.Implementations
{
    [HandledType("Book")]
    public class Book : Item
    {
        public static readonly Logger Logger = LogManager.CreateLogger();

        public Book(World world, Mooege.Common.MPQ.FileFormats.ItemTable definition)
            : base(world, definition)
        {
        }

        public override void OnTargeted(Player player, TargetMessage message)
        {
            //Logger.Trace("OnTargeted");
            int loreSNOId = LoreAssetHelper.GetLoreForItem(this.SNOId);
            if (loreSNOId != -1)
            {
                player.PlayLore(loreSNOId, true);
                if (player.GroundItems.ContainsKey(this.DynamicID))
                    player.GroundItems.Remove(this.DynamicID);
                this.Destroy();
            }
        }
    }
}
