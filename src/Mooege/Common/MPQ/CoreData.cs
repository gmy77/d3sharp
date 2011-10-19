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
using Mooege.Common.MPQ.FileFormats;

namespace Mooege.Common.MPQ
{
    public class CoreData : MPQPatchChain
    {
        public readonly Dictionary<int, ActorDefinition> Actors = new Dictionary<int, ActorDefinition>();

        public CoreData()
            : base("CoreData.mpq", "/base/d3-update-base-(?<version>.*?).MPQ")
        {
            this.LoadActors();
        }

        private void LoadActors()
        {
            foreach(var file in this.FindMatchingFiles(".acr"))
            {
                var mpqFile = this.FileSystem.FindFile(file);
                if (mpqFile == null || mpqFile.Size < 10) continue;

                var actorDefinition = new ActorDefinition(mpqFile);
                this.Actors.Add(actorDefinition.ActorSNO, actorDefinition);
            }
        }
    }
}
