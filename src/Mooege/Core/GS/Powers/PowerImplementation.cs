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

using System.Collections.Generic;
using Mooege.Common;
using Mooege.Core.GS.Ticker;

namespace Mooege.Core.GS.Powers
{
    public abstract class PowerImplementation : PowerContext
    {
        public static readonly Logger Logger = LogManager.CreateLogger();

        // Called to start executing a power
        // Yields timers that signify when to continue execution.
        public abstract IEnumerable<TickTimer> Run();

        // token instance that can be yielded by Run() to indicate the power manager should stop
        // running a power implementation.
        public static readonly TickTimer StopExecution = null;
    }
}
