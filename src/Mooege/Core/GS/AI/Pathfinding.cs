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
using Mooege.Core.GS.Common.Types.Math;
using System.Drawing;
using Algorithms;
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Map;

namespace Mooege.Core.GS.AI
{
    public static class Pathfinding
    {
        public static byte[,] WorldMesh = new byte[4096, 8192];

        private static readonly Logger Logger = LogManager.CreateLogger();
        private static World world;
        private static Algorithms.IPathFinder mPathFinder;

        public static List<Vector3D> FindPath(Actor actor, Vector3D Start, Vector3D Destination)
        {
            // TODO check if dest is blocked, if target is standing against some walls it may be blocked, replace dest with closest reachable.
            InitWorld(actor.World);
            Point start = new Point((int)Start.X, (int)Start.Y);
            Point dest = new Point((int)Destination.X, (int)Destination.Y);
            if (mPathFinder == null)
            {
                mPathFinder = new PathFinderFast(WorldMesh);
                mPathFinder.Formula = HeuristicFormula.Custom1;
                mPathFinder.Diagonals = true;
                mPathFinder.ReopenCloseNodes = true;

                mPathFinder.HeavyDiagonals = false;
                mPathFinder.HeuristicEstimate = 1;
                mPathFinder.PunishChangeDirection = false;
                mPathFinder.TieBreaker = true; ;
                mPathFinder.SearchLimit = 250000;
                mPathFinder.DebugProgress = false;
                mPathFinder.ReopenCloseNodes = true;
                mPathFinder.DebugFoundPath = false;
            }
            List<PathFinderNode> Path = mPathFinder.FindPath(start, dest);

            List<Vector3D> path = new List<Vector3D>();
            if (Path == null) 
            {
                //path.Add(Destination); 
                return path;
            }
            for (int i = 0; i < Path.Count; i++)
            {
                path.Add(new Vector3D(Path[i].X, Path[i].Y, 0)); 
                // Cool fact. Client doesnt seem to care if we send 0 instead of real Z, mobile stays at correct Z still.
                // Correct Z can be grabbed from the navsquare. - DarkLotus
            }
            return path;
        }

        public static void InitWorld(World World)
        {
            // Todo make multiple world aware, store mesh's for each world. Generation is fast using the squares for lookups rather than cells.
            if (world != null) { return; }
            world = World;
            foreach (var scene in world.QuadTree.Query<Scene>(new Common.Types.Misc.Circle(new Vector2F(2000, 3000), 5000)))// We just want all the scenes in the world.
            {
                int x = (int)scene.Position.X;
                int y = (int)scene.Position.Y;
                int xx, yy;
                for (int i = 0; i < scene.NavMesh.NavMeshSquareCount; i++)
                {
                    if ((scene.NavMesh.Squares[i].Flags & 0x01) == 0x01)
                    {
                        int cnt = i;
                        xx = 0; yy = 0;
                        while (cnt > scene.NavMesh.SquaresCountY - 1)
                        {
                            yy++;
                            cnt -= scene.NavMesh.SquaresCountY;
                        }
                        xx = cnt;
                        xx = (int)(xx * 2.5);
                        yy = (int)(yy * 2.5);
                        for (int X = 0; X < 3; X++)
                        {
                            for (int Y = 0; Y < 3; Y++)
                            {
                                if (X + xx <= scene.NavMesh.SquaresCountX * 2.5 && Y + yy <= scene.NavMesh.SquaresCountY * 2.5)
                                {
                                    WorldMesh[X + xx + x, Y + yy + y] = 1;
                                }

                            }
                        }
                    }
                }
            }
        }
    }
}
