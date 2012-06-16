﻿/*
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

using System;
using System.Diagnostics;
using System.Windows;
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Players;

namespace Mooege.Core.GS.Objects
{
    /// <summary>
    /// An object that can be placed in world.
    /// </summary>
    public abstract class WorldObject : DynamicObject, IRevealable
    {
        /// <summary>
        /// The world object belongs to.
        /// </summary>
        public World World { get; protected set; }

        protected Vector3D _position;

        /// <summary>
        /// The position of the object.
        /// </summary>
        public Vector3D Position
        {
            get { return _position; }
            set
            {
                _position = value;
                this.Bounds = new Rect(this.Position.X, this.Position.Y, this.Size.Width, this.Size.Height);
                var handler = PositionChanged;
                if (handler != null) handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Event handler for position-change.
        /// </summary>
        public event EventHandler PositionChanged;

        /// <summary>
        /// Size of the object.
        /// </summary>
        public Size Size { get; protected set; }

        /// <summary>
        /// Automatically calculated bounds for object used by QuadTree.
        /// </summary>
        public Rect Bounds { get; private set; }

        /// <summary>
        /// Scale of the object.
        /// </summary>
        public float Scale { get; set; }

        public Vector3D RotationAxis { get; set; }

        public float RotationW { get; set; }

        /// <summary>
        /// Creates a new world object.
        /// </summary>
        /// <param name="world">The world object belongs to.</param>
        /// <param name="dynamicID">The dynamicId of the object.</param>
        protected WorldObject(World world, uint dynamicID)
            : base(dynamicID)
        {
            Debug.Assert(world != null);
            this.World = world;
            this.World.Game.StartTracking(this); // track the object.
            this.RotationAxis = new Vector3D();
            this._position = new Vector3D();
        }

        /// <summary>
        /// Reveals the object to given player.
        /// </summary>
        /// <param name="player">The player to reveal the object.</param>
        /// <returns>true if the object was revealed or false if the object was already revealed.</returns>
        public abstract bool Reveal(Player player);

        /// <summary>
        /// Unreveals the object to given plaer.
        /// </summary>
        /// <param name="player">The player to unreveal the object.</param>
        /// <returns>true if the object was unrevealed or false if the object wasn't already revealed.</returns>
        public abstract bool Unreveal(Player player);

        /// <summary>
        /// Makes the object leave the world and then destroys it.
        /// </summary>
        public override void Destroy()
        {
            if (this is Actor)
                this.World.Leave(this as Actor);

            this.World.Game.EndTracking(this);
            this.World = null;
        }
    }
}
