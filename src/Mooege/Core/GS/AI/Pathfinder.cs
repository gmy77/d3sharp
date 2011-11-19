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
        //TODO Grab Z axis for each move from scene grid. - DarkLotus
        private static readonly Logger Logger = LogManager.CreateLogger();
        private static System.Collections.Concurrent.ConcurrentDictionary<int, PathFinderFast> patherlist = new System.Collections.Concurrent.ConcurrentDictionary<int, PathFinderFast>();
        private World world;
        private Algorithms.PathFinderFast mPathFinder;
        private List<Vector3D> vectorPathList = new List<Vector3D>();
        private List<PathFinderNode> nodePathList = new List<PathFinderNode>();
        private Point start = new Point();
        private Point dest = new Point();
        private float basex = 0, basey = 0;
        private Scene curScene;
        private Scene destScene;
        public Pathfinder(World world)
        {
            this.world = world;
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
                destScene = world.QuadTree.Query<Scene>(new System.Windows.Rect(Destination.X, Destination.Y, 1, 1)).FirstOrDefault();
                mPathFinder = new PathFinderFast(buildOutOfSceneGrid(curScene, destScene, ref basex, ref basey));
                initPathFinder();
            }
                        
            //nodePathList = new List<PathFinderNode>();
            start.X = (int)((Start.X - basex) / 2.5f);
            start.Y = (int)((Start.Y - basey) / 2.5f);
            dest.X = (int)((Destination.X - basex) / 2.5f);
            dest.Y = (int)((Destination.Y - basey) / 2.5f);
            //start = new Point((int)((Start.X - basex) / 2.5f), (int)((Start.Y - basey) / 2.5f));
            //dest = new Point((int)((Destination.X - basex) / 2.5f), (int)((Destination.Y - basey) / 2.5f));

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
                //vectorPathList.Add(new Vector3D(nodePathList[i].X * 2.5f + basex, nodePathList[i].Y * 2.5f + basey, 0));
            }
            return vectorPathList;
        }

        private  void initPathFinder()
        {
            mPathFinder.Formula = HeuristicFormula.Custom1;
            mPathFinder.Diagonals = false;
            mPathFinder.ReopenCloseNodes = true;
            mPathFinder.HeavyDiagonals = false;
            mPathFinder.HeuristicEstimate = 1;
            mPathFinder.PunishChangeDirection = false;
            mPathFinder.TieBreaker = true; ;
            mPathFinder.SearchLimit = 75000;
            mPathFinder.DebugProgress = false;
            mPathFinder.ReopenCloseNodes = true;
            mPathFinder.DebugFoundPath = false;
        }

        private byte[,] buildOutOfSceneGrid(Scene s1, Scene s2, ref float basex, ref float basey)
        {
            if (s1.SceneSNO.Id == s2.SceneSNO.Id) { return s1.NavMesh.WalkGrid; }
            //Logger.Debug("Start scene " + s1.SceneSNO.Name + " dest scene " + s2.SceneSNO.Name);
            byte[,] grid = new byte[256, 256]; // probably should be 512,512 as theres one scene thats like 152x140

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
            foreach (var s in world.QuadTree.Query<Scene>(new Circle(basex + 5, basey + 5, 1f)))
            {
                //Logger.Debug("Inserting Scene: " + s.SceneSNO.Name + " at offset: 0/0");
                insertgridintogrid(ref grid, s, 0, 0);
            }

            foreach (var s in world.QuadTree.Query<Scene>(new Circle(basex + (float)s1.Size.Width + 10, basey + 10, 1f)))
            {
                //Logger.Debug("Inserting Scene: " + s.SceneSNO.Name + " at offset: " + s1.Size.Width + "/0");
                insertgridintogrid(ref grid, s, (int)(s1.Size.Width / 2.5), 0);
            }
            foreach (var s in world.QuadTree.Query<Scene>(new Circle(basex + 10, basey + (float)s1.Size.Height + 10, 1f)))
            {
                //Logger.Debug("Inserting Scene: " + s.SceneSNO.Name + " at offset: 0/" + s1.Size.Width);
                insertgridintogrid(ref grid, s, 0, (int)(s1.Size.Height / 2.5));
            }
            foreach (var s in world.QuadTree.Query<Scene>(new Circle(basex + (float)s1.Size.Width + 10, basey + (float)s1.Size.Height + 10, 1f)))
            {
                //Logger.Debug("Inserting Scene: " + s.SceneSNO.Name + " at offset: " + s1.Size.Width + "/" + s1.Size.Height);
                insertgridintogrid(ref grid, s, (int)(s1.Size.Width / 2.5), (int)(s1.Size.Height / 2.5));
            }
            /*var stream = System.IO.File.CreateText("testtt.txt");
            for (int y = 0; y < 256; y++) 
            {
                for (int x = 0; x < 256; x++)
                {
                    if (grid[x, y] == 1)
                    {
                        stream.Write("X");
                    }
                    else
                    stream.Write(grid[x, y]);
                }
                stream.Write(stream.NewLine);
            }
            stream.Close();*/
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
        //private static Dictionary<uint, PathingTask> queuedTasks = new Dictionary<uint, PathingTask>();
        private System.Collections.Concurrent.ConcurrentDictionary<uint, PathingTask> queuedtasks = new System.Collections.Concurrent.ConcurrentDictionary<uint, PathingTask>();

        private AI.Pathfinder aipather;
        private Games.Game game;

        public Pather(Games.Game game)
        {
            // TODO: Complete member initialization
            this.game = game;
        }

        internal void GetPath(Actor owner, Vector3D vector3D, Vector3D heading, ref List<Vector3D> Path, ref int finished)
        {
            if (aipather == null) { aipather = new Pathfinder(owner.World); } // TODO make this a list one for each world in the game.
            //if (!CompletedTasks.TryRemove(owner.DynamicID, out Path))
            //{
                AddRequest(aipather, owner, vector3D, heading,ref Path,ref finished);
                return;
            //}
            //return;
        }

        private void AddRequest(AI.Pathfinder pathing, Actor actor, Vector3D Start, Vector3D Destination, ref List<Vector3D> Path,ref int finished)
        {
            queuedtasks.TryAdd(actor.DynamicID, new PathingTask(pathing, actor, Start, Destination,ref Path,ref finished));
        }

        public List<Vector3D> GetPath(Actor actor, Vector3D Start, Vector3D Destination)
        {
            /*List<Vector3D> path;
            PathFinderFast pather;
            CompletedTasks.TryRemove(actor.DynamicID, out path);
            if(path == null)

                if (actor.CurrentScene.Bounds.IntersectsWith(new System.Windows.Rect(Start.X, Start.Y, 1, 1)) && actor.CurrentScene.Bounds.IntersectsWith(new System.Windows.Rect(Destination.X, Destination.Y, 1, 1)))
                {
                    if (patherlist.TryGetValue(actor.CurrentScene.SceneSNO.Id, out pather))
                    {
                        AddRequest(pather, actor, Start, Destination);
                    }
                    else
                    {
                        pather = new PathFinderFast(actor.CurrentScene.NavMesh.WalkGrid);
                        patherlist.TryAdd(actor.CurrentScene.SceneSNO.Id, pather);
                    }
                }
            */
            return null;
        }
        //static System.Diagnostics.Stopwatch st;
        //static int cnt;
        public void UpdateLoop()
        {
            //st = new System.Diagnostics.Stopwatch();
            while (true)
            {
                if (!queuedtasks.IsEmpty)
                {
                    PathingTask k; 
                    var x = queuedtasks.First(); 
                    
                    //if (!CompletedTasks.ContainsKey(x.Value.Actor.DynamicID))
                                //CompletedTasks.TryAdd(x.Value.Actor.DynamicID, x.Value.getit());
                    x.Value.getit();
                    queuedtasks.TryRemove(x.Key, out k);
                }
                /*if (queuedTasks.Count > 0)
                {
                    lock (queuedTasks)
                    {
                        st.Restart();
                        foreach (var x in queuedTasks.Values)
                        {
                            if (!CompletedTasks.ContainsKey(x.Actor.DynamicID))
                                CompletedTasks.Add(x.Actor.DynamicID, x.getit());
                        }
                        cnt = queuedTasks.Count;
                        queuedTasks.Clear();
                        st.Stop();
                        //Logger.Debug("Found: " + cnt + " Paths in " + st.Elapsed);
                        var x = queuedTasks.Values.First();
                        lock (CompletedTasks)
                        {
                            if (!CompletedTasks.ContainsKey(x.Actor.DynamicID))
                                CompletedTasks.Add(x.Actor.DynamicID, x.getit());
                        }
                        queuedTasks.Remove(x.Actor.DynamicID);
                    }

                }*/
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
            int finished;
            public PathingTask(Pathfinder pathing, Actor actor, Vector3D Start, Vector3D Destination, ref List<Vector3D> Path, ref int finished)
            {
                this.pathing = pathing;
                this.Actor = actor;
                this.start = Start;
                this.destination = Destination;
                this.Path = Path;
                this.finished = finished;
            }
            public List<Vector3D> getit()
            {
                Path.AddRange(pathing.FindPath(Actor, start, destination));
                this.finished = 1;
                if (Path.Count == 0)
                {
                    Path.Add(new Vector3D(0,0,0));
                }
                return Path;
            }
        }




       
    }
}
