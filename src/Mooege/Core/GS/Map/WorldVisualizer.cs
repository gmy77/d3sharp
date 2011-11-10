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
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace Mooege.Core.GS.Map
{
    public partial class WorldVisualizer : Form
    {
        public World World { get; private set; }
        public WorldNavMesh Mesh { get; private set; }
        public Bitmap Bitmap { get; private set; }

        private readonly Pen _masterScenePen = new Pen(Color.DarkGray, 5.0f);
        private readonly Pen _subScenePen = new Pen(Color.Gray, 3.0f);
        private readonly Pen _playerPen = new Pen(Color.DarkMagenta, 2.0f);
        private readonly Pen _actorPen = new Pen(Color.Green, 2.0f);
        private readonly Pen _unWalkablePen = new Pen(Color.Red, 1.0f);
        private readonly Pen _walkablePen = new Pen(Color.Blue, 1.0f);

        private readonly Font _sceneFont = new Font("Verdana", 7);

        public WorldVisualizer(World world)
        {
            _subScenePen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;

            InitializeComponent();
            this.World = world;
            this.Mesh = new WorldNavMesh(world);

            this.DrawWorld();
        }

        private void Stage_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(this.Bitmap, 0, 0, this.Bitmap.Width, this.Bitmap.Height);
        }

        private void DrawWorld()
        {
            this.Bitmap = new Bitmap((int)this.World.QuadTree.RootNode.Bounds.Width, (int)this.World.QuadTree.RootNode.Bounds.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            var graphicsObject = Graphics.FromImage(Bitmap);

            graphicsObject.FillRectangle(Brushes.Wheat, 0, 0, this.Bitmap.Width, this.Bitmap.Height);

            foreach(var scene in this.Mesh.Scenes)
            {
                var rect = new Rectangle((int) scene.Bounds.Left, (int) scene.Bounds.Top, (int) scene.Bounds.Width, (int) scene.Bounds.Height);
                graphicsObject.DrawRectangle(scene.Parent!=null ? _masterScenePen:_subScenePen, rect);

                if (!string.IsNullOrEmpty(scene.SceneSNO.Name))
                    graphicsObject.DrawString(scene.SceneSNO.Name, _sceneFont, Brushes.Gray, rect);
            }

            //foreach (var x in this.Mesh.Walkable)
            //{
            //    var rec = new Rectangle(new Point((int)x.Left, (int)x.Top), new Size((int)x.Width, (int)x.Height));
            //    graphicsObject.DrawRectangle(_walkablePen, rec);

            //}

            //this.Mesh.UpdateActors();

            //foreach (var x in this.Mesh.UnWalkable)
            //{
            //    var rec = new Rectangle(new Point((int)x.Left, (int)x.Top), new Size((int)x.Width, (int)x.Height));
            //    graphicsObject.DrawRectangle(_unWalkablePen, rec);

            //}

            //foreach (var x in this.Mesh.Actors)
            //{
            //    var rec = new Rectangle(new Point((int)x.Left, (int)x.Top), new Size((int)x.Width, (int)x.Height));
            //    graphicsObject.DrawRectangle(_actorPen, rec);

            //}

            //foreach (var x in this.Mesh.Players)
            //{
            //    var rec = new Rectangle(new Point((int)x.Left, (int)x.Top), new Size((int)x.Width * 4, (int)x.Height * 4));
            //    graphicsObject.DrawRectangle(_playerPen, rec);

            //}

            graphicsObject.Save();
            graphicsObject.Dispose();
        }

        public class WorldNavMesh
        {
            public World World { get; private set; }
            public Rect Bounds { get { return World.QuadTree.RootNode.Bounds; } }

            public List<Scene> Scenes { get; private set; }
            public List<Rect> UnWalkable { get; private set; }
            public List<Rect> Walkable { get; private set; }
            public List<Rect> Actors { get; private set; }
            public List<Rect> Players { get; private set; }

            public WorldNavMesh(World world)
            {
                this.World = world;

                this.Scenes = new List<Scene>();
                this.Actors = new List<Rect>();
                this.UnWalkable = new List<Rect>();
                this.Walkable = new List<Rect>();

                this.Update();
            }

            public void Update()
            {                
                float Xl = 0f;
                float Xh = 0f;
                float Yl = 0f;
                float Yh = 0f;

                this.Scenes = World.QuadTree.Query<Scene>(World.QuadTree.RootNode.Bounds);

                foreach (var scene in this.Scenes)
                {
                    var scenesize = (int)(scene.NavMesh.SquaresCountX * 2.5);

                    foreach (var cell in scene.NavZone.NavCells)
                    {
                        float x = scene.Position.X + cell.Min.X;
                        float y = scene.Position.Y + cell.Min.Y;

                        if (x < Xl) Xl = x;
                        if (x > Xh) Xh = x;
                        if (y < Yl) Yl = y;
                        if (y > Yh) Yh = y;

                        float sizex = cell.Max.X - cell.Min.X;
                        float sizey = cell.Max.Y - cell.Min.Y;

                        var rect = new Rect(x, y, sizex, sizey);

                        if ((cell.Flags & 0x1) != 0x1)
                            UnWalkable.Add(rect);
                        else 
                            Walkable.Add(rect);
                    }
                }
            }

            public void UpdateActors()
            {
                Actors = new List<Rect>();
                Players = new List<Rect>();

                foreach (var x in World.Players)
                {
                    Players.Add(x.Value.Bounds);
                }
            }
        }
    }
}
