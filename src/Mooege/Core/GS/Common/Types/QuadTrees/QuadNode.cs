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

// based on: http://csharpquadtree.codeplex.com/SourceControl/changeset/view/27798#506270

using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Windows;
using Mooege.Core.GS.Objects;

namespace Mooege.Core.GS.Common.Types.QuadTrees
{
    public class QuadNode
    {
        /// <summary>
        /// Parent node.
        /// </summary>
        public QuadNode Parent { get; internal set; }

        /// <summary>
        /// Childs nodes.
        /// </summary>
        private readonly QuadNode[] _nodes = new QuadNode[4];

        /// <summary>
        /// Read only collection of nodes.
        /// </summary>
        public ReadOnlyCollection<QuadNode> Nodes;

        /// <summary>
        /// Child node in given direction.
        /// </summary>
        /// <param name="direction"><see cref="Direction"/></param>
        /// <returns>The child node.</returns>
        public QuadNode this[Direction direction]
        {
            get
            {
                switch (direction)
                {
                    case Direction.NorthWest:
                        return _nodes[0];
                    case Direction.NorthEast:
                        return _nodes[1];
                    case Direction.SouthWest:
                        return _nodes[2];
                    case Direction.SouthEast:
                        return _nodes[3];
                    default:
                        return null;
                }
            }
            set
            {
                switch (direction)
                {
                    case Direction.NorthWest:
                        _nodes[0] = value;
                        break;
                    case Direction.NorthEast:
                        _nodes[1] = value;
                        break;
                    case Direction.SouthWest:
                        _nodes[2] = value;
                        break;
                    case Direction.SouthEast:
                        _nodes[3] = value;
                        break;
                }
                if (value != null)
                    value.Parent = this;
            }
        }

        /// <summary>
        /// List of contained objects.
        /// </summary>
        public ConcurrentDictionary<uint, WorldObject> ContainedObjects = new ConcurrentDictionary<uint, WorldObject>();

        /// <summary>
        /// The bounds for node.
        /// </summary>
        public Rect Bounds { get; internal set; }

        /// <summary>
        /// Creates a new QuadNode with given bounds.
        /// </summary>
        /// <param name="bounds">The bounds for node.</param>
        public QuadNode(Rect bounds)
        {
            Bounds = bounds;
            Nodes = new ReadOnlyCollection<QuadNode>(_nodes);
        }

        /// <summary>
        /// Creates a new QuadNode with given bounds parameters.
        /// </summary>
        /// <param name="x">The x-coordinate of top-left corner of the region.</param>
        /// <param name="y">The y-coordinate of top-left corner of the region</param>
        /// <param name="width">The width of the region.</param>
        /// <param name="height">The height of the region</param>
        public QuadNode(double x, double y, double width, double height)
            : this(new Rect(x, y, width, height))
        { }

        /// <summary>
        /// Returns true if node has child-nodes.
        /// </summary>
        /// <returns><see cref="bool"/></returns>
        public bool HasChildNodes()
        {
            return _nodes[0] != null;
        }
    }
}
