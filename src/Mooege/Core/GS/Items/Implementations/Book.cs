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

using System.Linq;
using Mooege.Common;
using Mooege.Core.GS.Players;
using Mooege.Core.GS.Map;
using Mooege.Net.GS.Message.Definitions.World;
using Mooege.Core.GS.Common.Types.SNO;
using Mooege.Core.GS.Markers;

namespace Mooege.Core.GS.Items.Implementations
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
