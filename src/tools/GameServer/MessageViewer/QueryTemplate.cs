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
using Mooege.Net.GS.Message;
using System.Collections.Generic;
using GameMessageViewer;
using System.Linq;
using System.Linq.Dynamic;

public class QueryTemplate<T> where T : GameMessage
{
    public IEnumerable<MessageNode> Query(List<MessageNode> nodes, string whereClause) 
    {
        List<T> messages = new List<T>();
        foreach (MessageNode n in nodes)
            if (n.gameMessage is T)
                messages.Add((T)n.gameMessage);

        IEnumerable<T> result = messages.AsQueryable<T>().Where(whereClause);

        foreach (T message in result)
            yield return new MessageNode(message);
    }
}