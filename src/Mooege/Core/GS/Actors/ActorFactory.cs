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
using System.Reflection;
using Mooege.Common.MPQ;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Common.Types.SNO;
using Mooege.Core.GS.Map;

namespace Mooege.Core.GS.Actors
{
    public static class ActorFactory
    {
        private static readonly Dictionary<int, Type> SNOHandlers = new Dictionary<int, Type>();

        static ActorFactory()
        {
            LoadHandlers();
        }

        public static Actor Create(int snoId, World world, Vector3D position)
        {
            if (!MPQStorage.Data.Assets[SNOGroup.Actor].ContainsKey(snoId))
                return null;

            var actorAsset = MPQStorage.Data.Assets[SNOGroup.Actor][snoId];
            if(actorAsset==null) return null;
            var actorData = actorAsset.Data as Mooege.Common.MPQ.FileFormats.Actor;
            if (actorData == null) return null;

            if (actorData.Type == Mooege.Common.MPQ.FileFormats.Actor.ActorType.Invalid) 
                return null;
            else if (SNOHandlers.ContainsKey(snoId))
                return (Actor) Activator.CreateInstance(SNOHandlers[snoId], new object[] {world, snoId, position}); // check for handled-actor implementations.
            else switch (actorData.Type)
            {
                case Mooege.Common.MPQ.FileFormats.Actor.ActorType.Monster:
                    return new Monster(world, snoId, position);
                case Mooege.Common.MPQ.FileFormats.Actor.ActorType.Gizmo:
                    return new Gizmo(world, snoId, position);
            }

            return null;
        }

        public static void LoadHandlers()
        {
            SNOHandlers.Clear();
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (!type.IsSubclassOf(typeof (Actor))) continue;

                var attributes = (HandledSNOAttribute[]) type.GetCustomAttributes(typeof (HandledSNOAttribute), true);
                if (attributes.Length == 0) continue;

                foreach (var sno in attributes.First().SNOIds)
                {
                    SNOHandlers.Add(sno, type);
                }
            }
        }

        public static bool HasHandler(int sno)
        {
            return SNOHandlers.ContainsKey(sno);
        }
    }
}
