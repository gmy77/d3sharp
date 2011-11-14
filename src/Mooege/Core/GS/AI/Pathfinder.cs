using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Core.GS.Common.Types.Math;
using System.Drawing;
using Mooege.Common;
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Common.Types.Misc;
using Algorithms;
using Mooege.Core.GS.Actors;

namespace Mooege.Core.GS.AI
{
    public class Pathfinder
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        World world;
        private byte[,] buildOutOfSceneGrid(Scene s1, Scene s2)
        {
            //TODO fix for diff scene sizes
            byte[,] grid = new byte[256, 256];
            float basex = 0, basey = 0;

            if (s1.Position.X < s2.Position.X)
            { basex = s1.Position.X; }
            else if (s1.Position.X >= s2.Position.X)
            {
                basex = s2.Position.X;
            }
            if (s1.Position.Y < s2.Position.Y)
            { basey = s1.Position.Y; }
            else if (s1.Position.Y >= s2.Position.Y)
            {
                basey = s2.Position.Y;
            }
            foreach (var s in world.QuadTree.Query<Scene>(new Circle(basex + 5, basey + 5, 1f)))
            {
                insertgridintogrid(ref grid, s.NavMesh.WalkGrid, 0, 0);
            }

            foreach (var s in world.QuadTree.Query<Scene>(new Circle(basex + (float)s1.Size.Width + 5, basey + 5, 1f)))
            {
                insertgridintogrid(ref grid, s.NavMesh.WalkGrid, (int)(s1.Size.Width / 2.5), 0);
            }
            foreach (var s in world.QuadTree.Query<Scene>(new Circle(basex + 5, basey + (float)s1.Size.Height + 5, 1f)))
            {
                insertgridintogrid(ref grid, s.NavMesh.WalkGrid, 0, (int)(s1.Size.Height / 2.5));
            }
            foreach (var s in world.QuadTree.Query<Scene>(new Circle(basex + (float)s1.Size.Width + 5, basey + (float)s1.Size.Height + 5, 1f)))
            {
                insertgridintogrid(ref grid, s.NavMesh.WalkGrid, (int)(s1.Size.Width / 2.5), (int)(s1.Size.Height / 2.5));
            }

            return grid;

        }
        private void insertgridintogrid(ref byte[,] Grid, byte[,] smallgrid, int offsetx, int offsety)
        {
            for (int X = 0; X < smallgrid.GetLength(0); X++)
            {
                for (int Y = 0; Y < smallgrid.GetLength(1); Y++)
                {
                    Grid[X + offsetx, Y + offsety] = smallgrid[X, Y];
                }
            }
        }
        private Algorithms.IPathFinder mPathFinder;
        List<Vector3D> path = new List<Vector3D>();
        List<PathFinderNode> Path = new List<PathFinderNode>();
        Point start, dest;
        int lastSceneSNO = 0;
        public List<Vector3D> FindPath(Actor actor, Vector3D Start, Vector3D Destination)
        {
            //InitWorld(actor.World);
            
            var curscene = new Vector3D(actor.CurrentScene.Position);
            Path = new List<PathFinderNode>();
            path = new List<Vector3D>();
            start = new Point((int)((Start.X - curscene.X) / 2.5f), (int)((Start.Y - curscene.Y) / 2.5f));
            dest = new Point((int)((Destination.X - curscene.X) / 2.5f), (int)((Destination.Y - curscene.Y) / 2.5f));
            if (mPathFinder == null)
            {
                lastSceneSNO = actor.CurrentScene.SceneSNO.Id;
                if (!actor.CurrentScene.Bounds.IntersectsWith(new System.Windows.Rect(Destination.X, Destination.Y, 0f, 0f)))
                {
                    this.world = actor.World;
                    mPathFinder = new PathFinderFast(buildOutOfSceneGrid(actor.CurrentScene, world.QuadTree.Query<Scene>(new System.Windows.Rect(Destination.X, Destination.Y, 1, 1)).First()));
                }
                else
                {
                    mPathFinder = new PathFinderFast(actor.CurrentScene.NavMesh.WalkGrid);
                }

                mPathFinder.Formula = HeuristicFormula.Custom1;
                mPathFinder.Diagonals = true;
                mPathFinder.ReopenCloseNodes = true;

                mPathFinder.HeavyDiagonals = false;
                mPathFinder.HeuristicEstimate = 1;
                mPathFinder.PunishChangeDirection = false;
                mPathFinder.TieBreaker = true; ;
                mPathFinder.SearchLimit = 7500;
                mPathFinder.DebugProgress = false;
                mPathFinder.ReopenCloseNodes = true;
                mPathFinder.DebugFoundPath = false;
            }
            else if(actor.CurrentScene.SceneSNO.Id != lastSceneSNO)
            { // TODO - only load new grid if scenes changed from last path req.
                if (!actor.CurrentScene.Bounds.IntersectsWith(new System.Windows.Rect(Destination.X, Destination.Y, 0f, 0f)))
                {
                    this.world = actor.World;
                    mPathFinder = new PathFinderFast(buildOutOfSceneGrid(actor.CurrentScene, world.QuadTree.Query<Scene>(new System.Windows.Rect(Destination.X, Destination.Y, 1, 1)).First()));
                }
                else
                {
                    mPathFinder = new PathFinderFast(actor.CurrentScene.NavMesh.WalkGrid);
                }
            }
            Path = mPathFinder.FindPath(start, dest);
            //if (path == null) { path = new List<Vector3D>(); }
            if (Path == null) { path.Add(Destination); return path; }
            for (int i = 0; i < Path.Count; i++)
            {
                path.Add(new Vector3D(Path[i].X * 2.5f + curscene.X, Path[i].Y * 2.5f + curscene.Y, 0));
            }
            return path;
        }
    }



    public static class Pather
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        public static Dictionary<uint, List<Vector3D>> cache = new Dictionary<uint, List<Vector3D>>();
        public static Dictionary<uint, PathingTask> Tasks = new Dictionary<uint, PathingTask>();
        public class PathingTask
        {
            public Pathfinder pathing;
            public Actor actor;
            public Vector3D Start;
            public Vector3D Destination;
            public PathingTask(Pathfinder pathing, Actor actor, Vector3D Start, Vector3D Destination)
            {
                this.pathing = pathing;
                this.actor = actor;
                this.Start = Start;
                this.Destination = Destination;
            }
            public List<Vector3D> getit()
            {
                return pathing.FindPath(actor, Start, Destination);
            }
        }
        public static void AddRequest(Pathfinder pathing, Actor actor, Vector3D Start, Vector3D Destination)
        {
            lock (Tasks)
            {
                if (!Tasks.ContainsKey(actor.DynamicID))
                    Tasks.Add(actor.DynamicID, new PathingTask(pathing, actor, Start, Destination));
            }

        }
        public static List<Vector3D> GetPath(Pathfinder pathing, Actor actor, Vector3D Start, Vector3D Destination)
        {
            if (cache.ContainsKey(actor.DynamicID))
            {
                var x = cache[actor.DynamicID];
                cache.Remove(actor.DynamicID);
                return x;
            }
            AddRequest(pathing, actor, Start, Destination);
            return null;
        }

        public static void UpdateLoop()
        {
            while (true)
            {
                if (Tasks.Count > 0)
                {
                    lock (Tasks)
                    {
                        var x = Tasks.Values.First();
                        lock (cache)
                        {
                            if (!cache.ContainsKey(x.actor.DynamicID))
                                cache.Add(x.actor.DynamicID, x.getit());
                        }
                        Tasks.Remove(x.actor.DynamicID);
                    }

                }
                else { System.Threading.Thread.Sleep(5); }
            }

        }

    }
}
