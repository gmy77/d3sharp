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

using System;
using System.Collections.Generic;
using System.Linq;
using Mooege.Core.GS.Common.Types.Math;
using System.Drawing;
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Common.Types.Misc;
using Algorithms;
using Mooege.Core.GS.Actors;
using Mooege.Common.Logging;

namespace Mooege.Core.GS.AI
{
    public class Pather
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        private System.Collections.Concurrent.ConcurrentDictionary<uint, List<Vector3D>> _completedPathTasks = new System.Collections.Concurrent.ConcurrentDictionary<uint, List<Vector3D>>();
        private System.Collections.Concurrent.ConcurrentDictionary<uint, PathRequestTask> _queuedPathTasks = new System.Collections.Concurrent.ConcurrentDictionary<uint, PathRequestTask>();

        private Pathfinder aipather;
        private Games.Game game;

        public Pather(Games.Game game)
        {

            this.game = game;
        }

        //This submits a request for a path to the pathfinder thread. This is the main point of entry for usage. - DarkLotus
        public PathRequestTask GetPath(Actor owner, Vector3D vector3D, Vector3D heading)
        {
            if (aipather == null)
                aipather = new Pathfinder();
            var pathRequestTask = new PathRequestTask(aipather, owner, owner.Position, heading);
            _queuedPathTasks.TryAdd(owner.DynamicID, pathRequestTask);
            return pathRequestTask;
        }


        //Runs a copy for each game currently running- Processes all pathrequests queued.
        public void UpdateLoop()
        {
            PathRequestTask temporaryPathTask;
            KeyValuePair<uint, PathRequestTask> workPathTask;
            while (true)
            {
                if (!_queuedPathTasks.IsEmpty)
                {
                    workPathTask = _queuedPathTasks.First();
                    workPathTask.Value.GetPath();
                    _queuedPathTasks.TryRemove(workPathTask.Key, out temporaryPathTask);
                }
                else { System.Threading.Thread.Sleep(50); }
            }

        }
        public class Pathfinder
        {
            //TODO Grab Z axis for each move from scene grid, if Z is desired. Walking works fine sending 0 Z axis.
            //Shortcomings; Grids are built off Walkable, no accounting for flying,Ghost allowed movements etc With current method this would require duplicating WalkGrid for each scene with FlyGrid and GhostGrid
            //Avg 9ms when finding random 0-40 distance paths - DarkLotus
            private static readonly Logger Logger = LogManager.CreateLogger();

            //This holds a single copy of PathFinderFast for each Scene SNO so they can be reused for later pathing requests. This saves reiniting the pather and allocating 230kb. Multiscene grids are not stored.
            private static System.Collections.Concurrent.ConcurrentDictionary<int, PathFinderFast> listOfPathFinderInstances = new System.Collections.Concurrent.ConcurrentDictionary<int, PathFinderFast>();
            private Algorithms.PathFinderFast mPathFinder;
            private List<Vector3D> vectorPathList = new List<Vector3D>(); // the list of world vectors that gets returned to the action
            private List<PathFinderNode> nodePathList = new List<PathFinderNode>(); //Nodelist returned from the pathfinder algo, coordinates are scene local.
            private Point _startSceneLocal = new Point(); // local scene position of the start location
            private Point _destinationSceneLocal = new Point(); // 
            private float _baseX = 0, _baseY = 0; // Used to convert local scene coordinates back to world coordinates. These are the scenes x/y normally.
            private Scene _curScene; // scene containing the start location
            private Scene _destScene; // scene containing destination
            public Pathfinder()
            {
            }
            public List<Vector3D> FindPath(Actor actor, Vector3D Start, Vector3D Destination)
            {
                _baseX = 0; _baseY = 0; // reset to 0

                // Should only be null first time a path is requested.
                if (_curScene == null)
                {
                    _curScene = actor.CurrentScene;
                    if (!listOfPathFinderInstances.TryGetValue(_curScene.SceneSNO.Id, out mPathFinder)) // Attempts to pull the pathfinder which matches the scenes SNO from the patherlist
                    {
                        mPathFinder = new PathFinderFast(_curScene.NavMesh.WalkGrid); // Create a new pather, using the current scenes grid.
                        listOfPathFinderInstances.TryAdd(_curScene.SceneSNO.Id, mPathFinder); // add it to our patherlist, with the SNO as key.
                    }

                    InitPathFinder();
                }

                // Checks if our path start location is inside current scene, if it isnt, we reset curScene and set mPathfinder to the corrent grid.
                if (!_curScene.Bounds.IntersectsWith(new System.Windows.Rect(Start.X, Start.Y, 1, 1)))
                {
                    _curScene = actor.CurrentScene;
                    if (!listOfPathFinderInstances.TryGetValue(_curScene.SceneSNO.Id, out mPathFinder))
                    {
                        mPathFinder = new PathFinderFast(_curScene.NavMesh.WalkGrid);
                        listOfPathFinderInstances.TryAdd(_curScene.SceneSNO.Id, mPathFinder);
                    }

                    InitPathFinder();
                }

                _baseX = _curScene.Position.X; //Our base location for working out world > SceneLocal coordinates.
                _baseY = _curScene.Position.Y;

                // Path's start and destination are both in same scene.
                if (_curScene.Bounds.IntersectsWith(new System.Windows.Rect(Destination.X, Destination.Y, 1, 1)))
                {
                    _destScene = _curScene;
                }
                else
                {
                    //Builds a new grid on the fly containing both the start and destination scenes. This is not really optimal, but its a trade off.
                    // Keeping grids Scene based means they can be used cross game even when laid out different in a seperate world. This keeps memory usage down substantially.
                    // Also limited to a max distance of scene > scene. Again this keeps memory usage low.
                    _destScene = _curScene.World.QuadTree.Query<Scene>(new System.Windows.Rect(Destination.X, Destination.Y, 1, 1)).FirstOrDefault();
                    mPathFinder = new PathFinderFast(BuildOutOfSceneGrid(_curScene, _destScene, ref _baseX, ref _baseY));
                    InitPathFinder();
                }
                //2.5f is because Scene navmesh's are based on 96x96 for a 240x240 scene - Darklotus
                _startSceneLocal.X = (int)((Start.X - _baseX) / 2.5f);
                _startSceneLocal.Y = (int)((Start.Y - _baseY) / 2.5f);
                _destinationSceneLocal.X = (int)((Destination.X - _baseX) / 2.5f);
                _destinationSceneLocal.Y = (int)((Destination.Y - _baseY) / 2.5f);
                //Possibily add a check to ensure start/dest local coords are valid. Unneeded so far.

                nodePathList = mPathFinder.FindPath(_startSceneLocal, _destinationSceneLocal); // The actual pathfind request, the path is found here.

                vectorPathList.Clear(); // Clear the previous path.

                if (nodePathList == null) { return vectorPathList; }// No Path Found.
                if (nodePathList.Count < 1) { return vectorPathList; } // Safety net Incase start/dest are the same.

                for (int i = 0; i < nodePathList.Count; i++)
                {
                    // Convert the path into world coordinates for use in Movement.
                    // TODO Objectpool maybe?
                    vectorPathList.Insert(0, new Vector3D(nodePathList[i].X * 2.5f + _baseX, nodePathList[i].Y * 2.5f + _baseY, 0));
                }
                return vectorPathList;
            }

            private void InitPathFinder()
            {

                mPathFinder.Formula = HeuristicFormula.Custom1;
                mPathFinder.Diagonals = true; // Looks better, they can cut corners with this set to true - DarkLotus
                mPathFinder.ReopenCloseNodes = true;
                mPathFinder.HeavyDiagonals = false;
                mPathFinder.HeuristicEstimate = 1;
                mPathFinder.PunishChangeDirection = true;
                mPathFinder.TieBreaker = true; ;
                mPathFinder.SearchLimit = 75000;
                mPathFinder.DebugProgress = false;
                mPathFinder.ReopenCloseNodes = true;
                mPathFinder.DebugFoundPath = false;
            }

            // Builds a 2x2 grid containing the start/dest location hopefully. If start/dest are further apart the grid will not contain both.
            private byte[,] BuildOutOfSceneGrid(Scene s1, Scene s2, ref float basex, ref float basey)
            {
                // This really isnt optimal but its worked in my test cases, only alternative i could think was loading the entire world into a grid, but thats 4k x 4k and would require duplicating as each world is random - DarkLotus
                if (s1.SceneSNO.Id == s2.SceneSNO.Id) { return s1.NavMesh.WalkGrid; }
                //Logger.Debug("Start scene " + s1.SceneSNO.Name + " dest scene " + s2.SceneSNO.Name);
                byte[,] grid = new byte[256, 256]; // should use 512,512 as there is a scene larger than 128x128. -DarkLotus

                //Work out if start or dest scene has lowest X/Y to use as our 0,0 for our new 2x2 scene grid.
                if (s1.Position.X < s2.Position.X)
                {
                    basex = s1.Position.X;
                }
                else if (s1.Position.X >= s2.Position.X)
                {
                    basex = s2.Position.X;
                }
                if (s1.Position.Y < s2.Position.Y)
                {
                    basey = s1.Position.Y;
                }
                else if (s1.Position.Y >= s2.Position.Y)
                {
                    basey = s2.Position.Y;
                }

                foreach (var s in s1.World.QuadTree.Query<Scene>(new Circle(basex + 5, basey + 5, 1f)))
                {
                    //Logger.Debug("Inserting Scene: " + s.SceneSNO.Name + " at offset: 0/0");
                    InsertSceneGridIntoMultiSceneGrid(ref grid, s, 0, 0);
                }

                foreach (var s in s1.World.QuadTree.Query<Scene>(new Circle(basex + (float)s1.Size.Width + 10, basey + 10, 1f)))
                {
                    //Logger.Debug("Inserting Scene: " + s.SceneSNO.Name + " at offset: " + s1.Size.Width + "/0");
                    InsertSceneGridIntoMultiSceneGrid(ref grid, s, (int)(s1.Size.Width / 2.5), 0);
                }
                foreach (var s in s1.World.QuadTree.Query<Scene>(new Circle(basex + 10, basey + (float)s1.Size.Height + 10, 1f)))
                {
                    //Logger.Debug("Inserting Scene: " + s.SceneSNO.Name + " at offset: 0/" + s1.Size.Width);
                    InsertSceneGridIntoMultiSceneGrid(ref grid, s, 0, (int)(s1.Size.Height / 2.5));
                }
                foreach (var s in s1.World.QuadTree.Query<Scene>(new Circle(basex + (float)s1.Size.Width + 10, basey + (float)s1.Size.Height + 10, 1f)))
                {
                    //Logger.Debug("Inserting Scene: " + s.SceneSNO.Name + " at offset: " + s1.Size.Width + "/" + s1.Size.Height);
                    InsertSceneGridIntoMultiSceneGrid(ref grid, s, (int)(s1.Size.Width / 2.5), (int)(s1.Size.Height / 2.5));
                }
                return grid;

            }

            //Inserts a smaller scenes nav grid into a larger grid.
            private void InsertSceneGridIntoMultiSceneGrid(ref byte[,] Grid, Scene scene, int offsetx, int offsety)
            {
                byte[,] smallgrid = scene.NavMesh.WalkGrid;
                for (int X = 0; X < scene.NavMesh.SquaresCountX; X++)
                {
                    for (int Y = 0; Y < scene.NavMesh.SquaresCountY; Y++)
                    {
                        Grid[X + offsetx, Y + offsety] = smallgrid[X, Y];
                    }
                }
            }
        }




        public class PathRequestTask
        {
            private Pathfinder _pathfinder;
            public Actor _actor;
            private Vector3D _start;
            private Vector3D _destination;
            public List<Vector3D> Path;
            public Boolean PathFound = false;

            public PathRequestTask(Pathfinder pathing, Actor actor, Vector3D Start, Vector3D Destination)
            {
                this._pathfinder = pathing;
                this._actor = actor;
                this._start = Start;
                this._destination = Destination;
                this.Path = new List<Vector3D>();
            }
            public List<Vector3D> GetPath()
            {
                Path.AddRange(_pathfinder.FindPath(_actor, _start, _destination));
                PathFound = true;

                return Path;
            }
        }
    }
}
