using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Map;

namespace Mooege.Core.GS.AI
{
    public partial class NavDebug : Form
    {
        // add System.Windows.Forms.Application.Run(new Mooege.Core.GS.AI.NavDebug(player.World)); to a command to init.
        Graphics surface;
        List<Vector3D> path;
        Pen pen5 = new Pen(Color.DarkViolet, 2.0f);
        Pen pen4 = new Pen(Color.DarkMagenta, 2.0f);
        Pen pen3 = new Pen(Color.Green, 2.0f);
        Pen pen1 = new Pen(Color.Red, 1.0f);
        Pen pen2 = new Pen(Color.Blue, 1.0f);

        public NavDebug(World world)
        {
            InitializeComponent();
            surface = panel1.CreateGraphics();
            //this.Show();           
            Pathfinding.InitWorld(world);
            WorldNavMesh.Init(world);
            path = Pathfinding.FindPath(world.Players.Values.First(),new Common.Types.Math.Vector3D(2962, 2850, 24), new Common.Types.Math.Vector3D(3003, 2817, 24));
            panel1.Paint += new PaintEventHandler(panel1_Paint);
        }

 
        void panel1_Paint(object sender, PaintEventArgs e)
        {
            
            // this part draws the byte grid, comment out if you just want to see the navcells.
           /* int fakex = 0;
            List<Rectangle> walk = new List<Rectangle>(); List<Rectangle> nowalk = new List<Rectangle>();
            int fakey = 0;
            for (int x = 2000; x < 3100; x++)
            {
                fakey = 0;
                for (int y = 1500; y < 3100; y++)
                {

                    if (Pathfinding.WorldMesh[x, y] == 0)
                    {
                        nowalk.Add(new Rectangle(fakex, fakey, 1, 1));
                        //e.Graphics.DrawRectangle(pen1, new Rectangle(fakex, fakey, 1, 1));
                    }
                    else
                    {
                        walk.Add(new Rectangle(fakex, fakey, 1, 1));
                        //e.Graphics.DrawRectangle(pen2, new Rectangle(fakex, fakey, 1, 1));
                    }
                    fakey++;
                }
                fakex++;
            }
            var xxx = walk.ToArray();
            e.Graphics.DrawRectangles(pen2, xxx);
            return;*/
            // navcells below here

            foreach (var x in WorldNavMesh.Walkable)
            {
                Rectangle rec = new Rectangle(new Point((int)x.Left, (int)x.Top), new Size((int)x.Width, (int)x.Height));
                e.Graphics.DrawRectangle(pen2, rec);

            }
            WorldNavMesh.UpdateActors();

            foreach (var x in WorldNavMesh.UnWalkable)
            {
                Rectangle rec = new Rectangle(new Point((int)x.Left, (int)x.Top), new Size((int)x.Width, (int)x.Height));
                e.Graphics.DrawRectangle(pen1, rec);

            }
            foreach (var x in WorldNavMesh.Actors)
            {
                Rectangle rec = new Rectangle(new Point((int)x.Left, (int)x.Top), new Size((int)x.Width, (int)x.Height));
                e.Graphics.DrawRectangle(pen3, rec);

            } foreach (var x in WorldNavMesh.Player)
            {
                Rectangle rec = new Rectangle(new Point((int)x.Left, (int)x.Top), new Size((int)x.Width * 4, (int)x.Height * 4));
                e.Graphics.DrawRectangle(pen4, rec);

            }

            for (int i = 0; i < path.Count - 1; i++)
            {
                e.Graphics.DrawLine(pen5, new Point((int)path[i].X, (int)path[i].Y), new Point((int)path[i + 1].X, (int)path[i + 1].Y));
            }
            surface.Save();
        }

        public static class WorldNavMesh
        {

            public static World world;
            public static List<System.Windows.Rect> UnWalkable;
            public static List<System.Windows.Rect> Walkable;
            public static List<System.Windows.Rect> Actors;
            public static List<System.Windows.Rect> Player;
            public static bool IsBlocked(Vector3D worldpos)
            {
                System.Windows.Rect actor = new System.Windows.Rect(worldpos.X, worldpos.Y, 2.5f, 2.5f);
                foreach (var r in UnWalkable)
                {
                    if (actor.IntersectsWith(r))
                    { return true; }
                }

                return false;
            }
            public static void UpdateActors()
            {
                Actors = new List<System.Windows.Rect>();
                Player = new List<System.Windows.Rect>();
                /*foreach (var x in world.actors)
                {
                    Actors.Add(x.Bounds);
                }*/
                foreach (var x in world.Players)
                {
                    Player.Add(x.Value.Bounds);
                }
            }
            public static void Init(World World)
            {
                if (world == null)
                {
                    world = World;
                }
                if (world.SNOId == World.SNOId && Actors != null) { return; };

                world = World;
                Actors = new List<System.Windows.Rect>();
                UnWalkable = new List<System.Windows.Rect>();
                Walkable = new List<System.Windows.Rect>();

                float Xl = 0f, Xh = 0f, Yl = 0f, Yh = 0f;
                foreach (var scene in world.QuadTree.Query<Scene>(new Common.Types.Misc.Circle(new Vector2F(2000, 3000), 5000)))
                {
                    int scenesize = (int)(scene.NavMesh.SquaresCountX * 2.5);

                    //Console.WriteLine(scene.Transform.Quaternion.Vector3D.ToString() + scene.Transform.Vector3D.ToString());
                    foreach (var cell in scene.NavZone.NavCells)
                    {
                        float x = scene.Position.X + cell.Min.X;//(scenesize - cell.Max.X);//;// 
                        float y = scene.Position.Y + cell.Min.Y;
                        if (x < Xl) { Xl = x; }
                        if (x > Xh) { Xh = x; }
                        if (y < Yl) { Yl = y; }
                        if (y > Yh) { Yh = y; }
                        float sizex = cell.Max.X - cell.Min.X;
                        float sizey = cell.Max.Y - cell.Min.Y;
                        System.Windows.Rect rect = new System.Windows.Rect(x, y, sizex, sizey);
                        if ((cell.Flags & 0x1) != 0x1)
                        {
                            UnWalkable.Add(rect);
                        }
                        else { Walkable.Add(rect); }

                    }
                }


            }


            public static Players.Player Player1;
            internal static void Init(World World, Players.Player player)
            {
                Init(world);
                Player1 = player;
            }
        }
    }


}
