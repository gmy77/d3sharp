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
using Mooege.Common;
using Mooege.Common.MPQ;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Common.Types.SNO;
using Mooege.Core.GS.Map;

namespace Mooege.Core.GS.Actors
{
    public static class ActorFactory
    {
        static readonly Logger Logger = LogManager.CreateLogger();

        public static Actor Create(int snoId, World world, Vector3D position)
        {
            if (!MPQStorage.Data.Assets[SNOGroup.Actor].ContainsKey(snoId))
                return null;

            var actorAsset = MPQStorage.Data.Assets[SNOGroup.Actor][snoId];
            if(actorAsset==null) return null;
            var actorData = actorAsset.Data as Mooege.Common.MPQ.FileFormats.Actor;
            if (actorData == null) return null;

            Logger.Trace("Actor: {0} Type: {1}", actorAsset.Name, actorData.Type);
            if (actorData.Type == Mooege.Common.MPQ.FileFormats.Actor.ActorType.Invalid) return null;

            //if (actorData.Type == Mooege.Common.MPQ.FileFormats.Actor.ActorType.Monster)
            //    return new Monster(world, snoId, position);

            //if (actorData.Type == Mooege.Common.MPQ.FileFormats.Actor.ActorType.Enviroment)
            //    return new Environment(world, snoId, position);

            if (actorData.Type == Mooege.Common.MPQ.FileFormats.Actor.ActorType.Gizmo)
                return new Gizmo(world, snoId, position);

            return null;
        }
    }
}
