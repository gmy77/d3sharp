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
using Mooege.Common.Logging;

namespace Mooege.Core.GS.AI
{

    public class Pathfinder
    {
        //TODO Grab Z axis for each move from scene grid.
        //Shortcomings; Grids are built off Walkable, no accounting for flying,Ghost allowed movements etc With current method this would require duplicating WalkGrid for each scene with FlyGrid and GhostGrid
        //Avg 9ms when finding random 0-40 distance paths - DarkLotus
        private static readonly Logger Logger = LogManager.CreateLogger();
        private static System.Collections.Concurrent.ConcurrentDictionary<int, PathFinderFast> patherlist = new System.Collections.Concurrent.ConcurrentDictionary<int, PathFinderFast>();
        private Algorithms.PathFinderFast mPathFinder;
        private List<Vector3D> vectorPathList = new List<Vector3D>();
        private List<PathFinderNode> nodePathList = new List<PathFinderNode>();
        private Point start = new Point();
        private Point dest = new Point();
        private float basex = 0, basey = 0;
        private Scene curScene;
        private Scene destScene;
        public Pathfinder()
        {
        }
        public List<Vector3D> FindPath(Actor actor, Vector3D Start, Vector3D Destination)
        {
            basex = 0; basey = 0;
            if (curScene == null)
            {
                curScene = actor.CurrentScene;
                if(!patherlist.TryGetValue(curScene.SceneSNO.Id,out mPathFinder))
                {
                    mPathFinder = new PathFinderFast(curScene.NavMesh.WalkGrid);
                    patherlist.TryAdd(curScene.SceneSNO.Id, mPathFinder);
                }
                
                initPathFinder();
            }
            

            if (!curScene.Bounds.IntersectsWith(new System.Windows.Rect(Start.X, Start.Y, 1, 1)))
            {
                curScene = actor.CurrentScene;
                if (!patherlist.TryGetValue(curScene.SceneSNO.Id, out mPathFinder))
                {
                    mPathFinder = new PathFinderFast(curScene.NavMesh.WalkGrid);
                    patherlist.TryAdd(curScene.SceneSNO.Id, mPathFinder);
                }

                initPathFinder();
            }
            basex = curScene.Position.X;
            basey = curScene.Position.Y;
            if (curScene.Bounds.IntersectsWith(new System.Windows.Rect(Destination.X, Destination.Y, 1, 1)))
            {
                destScene = curScene;
            }
            else
            {
                destScene = curScene.World.QuadTree.Query<Scene>(new System.Windows.Rect(Destination.X, Destination.Y, 1, 1)).FirstOrDefault();
                mPathFinder = new PathFinderFast(buildOutOfSceneGrid(curScene, destScene, ref basex, ref basey));
                initPathFinder();
            }
            //2.5f is because Scene navmesh's are based on 96x96 for a 240x240 scene - Darklotus
            start.X = (int)((Start.X - basex) / 2.5f);
            start.Y = (int)((Start.Y - basey) / 2.5f);
            dest.X = (int)((Destination.X - basex) / 2.5f);
            dest.Y = (int)((Destination.Y - basey) / 2.5f);

            nodePathList = mPathFinder.FindPath(start, dest);
            if (vectorPathList == null)
                vectorPathList = new List<Vector3D>();
            else
                vectorPathList.Clear();

            if (nodePathList == null) { return vectorPathList; }
            if (nodePathList.Count < 1) { return vectorPathList; }
           
            for (int i = 0; i < nodePathList.Count; i++)
            {
                vectorPathList.Insert(0, new Vector3D(nodePathList[i].X * 2.5f + basex, nodePathList[i].Y * 2.5f + basey, 0));
            }
            return vectorPathList;
        }

        private  void initPathFinder()
        {
            mPathFinder.Formula = HeuristicFormula.Custom1;
            mPathFinder.Diagonals = true; // Because otherwise they can walk over corners of walls etc. - DarkLotus
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

        private byte[,] buildOutOfSceneGrid(Scene s1, Scene s2, ref float basex, ref float basey)
        {
            // This really isnt optimal but its worked in my test cases, only alternative i could think was loading the entire world into a grid, but thats 4k x 4k and would require duplicating as each world is random - DarkLotus
            if (s1.SceneSNO.Id == s2.SceneSNO.Id) { return s1.NavMesh.WalkGrid; }
            //Logger.Debug("Start scene " + s1.SceneSNO.Name + " dest scene " + s2.SceneSNO.Name);
            byte[,] grid = new byte[256, 256]; // should use 512,512 as there is a scene larger than 128x128. -DarkLotus

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
            // Need a way to get a scenes neighbours or something as this is very expensive- DarkLotus
            foreach (var s in s1.World.QuadTree.Query<Scene>(new Circle(basex + 5, basey + 5, 1f)))
            {
                //Logger.Debug("Inserting Scene: " + s.SceneSNO.Name + " at offset: 0/0");
                insertgridintogrid(ref grid, s, 0, 0);
            }

            foreach (var s in s1.World.QuadTree.Query<Scene>(new Circle(basex + (float)s1.Size.Width + 10, basey + 10, 1f)))
            {
                //Logger.Debug("Inserting Scene: " + s.SceneSNO.Name + " at offset: " + s1.Size.Width + "/0");
                insertgridintogrid(ref grid, s, (int)(s1.Size.Width / 2.5), 0);
            }
            foreach (var s in s1.World.QuadTree.Query<Scene>(new Circle(basex + 10, basey + (float)s1.Size.Height + 10, 1f)))
            {
                //Logger.Debug("Inserting Scene: " + s.SceneSNO.Name + " at offset: 0/" + s1.Size.Width);
                insertgridintogrid(ref grid, s, 0, (int)(s1.Size.Height / 2.5));
            }
            foreach (var s in s1.World.QuadTree.Query<Scene>(new Circle(basex + (float)s1.Size.Width + 10, basey + (float)s1.Size.Height + 10, 1f)))
            {
                //Logger.Debug("Inserting Scene: " + s.SceneSNO.Name + " at offset: " + s1.Size.Width + "/" + s1.Size.Height);
                insertgridintogrid(ref grid, s, (int)(s1.Size.Width / 2.5), (int)(s1.Size.Height / 2.5));
            }
            return grid;

        }

        private void insertgridintogrid(ref byte[,] Grid, Scene scene, int offsetx, int offsety)
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

    

    public class Pather
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        private System.Collections.Concurrent.ConcurrentDictionary<uint, List<Vector3D>> CompletedTasks = new System.Collections.Concurrent.ConcurrentDictionary<uint, List<Vector3D>>();
        private System.Collections.Concurrent.ConcurrentDictionary<uint, PathingTask> queuedtasks = new System.Collections.Concurrent.ConcurrentDictionary<uint, PathingTask>();

        private AI.Pathfinder aipather;
        private Games.Game game;

        public Pather(Games.Game game)
        {

            this.game = game;
        }

        //This submits a request for a path to the pathfinder thread. - DarkLotus
        internal void GetPath(Actor owner, Vector3D vector3D, Vector3D heading, ref List<Vector3D> Path)
        {
            if (aipather == null)
                aipather = new Pathfinder();

            queuedtasks.TryAdd(owner.DynamicID, new PathingTask(aipather, owner, owner.Position, heading, ref Path));
            return;
        }

        
        //Runs a copy for each game currently running- Processes all pathrequests queued.
        public void UpdateLoop()
        {
            PathingTask temporaryPathTask;
            KeyValuePair<uint, PathingTask> workPathTask;
            while (true)
            {
                if (!queuedtasks.IsEmpty)
                {
                    workPathTask = queuedtasks.First();
                    workPathTask.Value.getit();
                    queuedtasks.TryRemove(workPathTask.Key, out temporaryPathTask);
                }
                else { System.Threading.Thread.Sleep(50); }
            }

        }
        public class PathingTask
        {
            private Pathfinder pathing;
            public Actor Actor;
            private Vector3D start;
            private Vector3D destination;
            List<Vector3D> Path;
            public PathingTask(Pathfinder pathing, Actor actor, Vector3D Start, Vector3D Destination, ref List<Vector3D> Path)
            {
                this.pathing = pathing;
                this.Actor = actor;
                this.start = Start;
                this.destination = Destination;
                this.Path = Path;

            }
            public List<Vector3D> getit()
            {
                Path.AddRange(pathing.FindPath(Actor, start, destination));

                if (Path.Count == 0)
                {
                    //Path = null; 
                    Path.Add(new Vector3D(0,0,0));
                    // This is a horrible hack because Path = null doesnt seem to propergate back to PathfindToPoint.cs - Darklotus
                }
                return Path;
            }
        }




       
    }
}
