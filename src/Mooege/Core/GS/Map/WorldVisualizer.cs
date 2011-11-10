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
using Mooege.Core.GS.Common.Types.Math;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace Mooege.Core.GS.Map
{
    public partial class WorldVisualizer : Form
    {
        public World World { get; private set; }
        public WorldNavMesh Mesh { get; private set; }
        public Bitmap Bitmap { get; private set; }

        Pen playerPen = new Pen(Color.DarkMagenta, 2.0f);
        Pen actorPen = new Pen(Color.Green, 2.0f);
        Pen unWalkablePen = new Pen(Color.Red, 1.0f);
        Pen walkablePen = new Pen(Color.Blue, 1.0f);

        public WorldVisualizer(World world)
        {
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

            foreach (var x in this.Mesh.Walkable)
            {
                var rec = new Rectangle(new Point((int)x.Left, (int)x.Top), new Size((int)x.Width, (int)x.Height));
                graphicsObject.DrawRectangle(walkablePen, rec);

            }

            this.Mesh.UpdateActors();

            foreach (var x in this.Mesh.UnWalkable)
            {
                var rec = new Rectangle(new Point((int)x.Left, (int)x.Top), new Size((int)x.Width, (int)x.Height));
                graphicsObject.DrawRectangle(unWalkablePen, rec);

            }

            foreach (var x in this.Mesh.Actors)
            {
                var rec = new Rectangle(new Point((int)x.Left, (int)x.Top), new Size((int)x.Width, (int)x.Height));
                graphicsObject.DrawRectangle(actorPen, rec);

            }

            foreach (var x in this.Mesh.Players)
            {
                var rec = new Rectangle(new Point((int)x.Left, (int)x.Top), new Size((int)x.Width * 4, (int)x.Height * 4));
                graphicsObject.DrawRectangle(playerPen, rec);

            }

            graphicsObject.Save();
            graphicsObject.Dispose();
        }

        public class WorldNavMesh
        {
            public World World { get; private set; }
            public Rect Bounds { get { return World.QuadTree.RootNode.Bounds; } }

            public List<Rect> UnWalkable { get; private set; }
            public List<Rect> Walkable { get; private set; }
            public List<Rect> Actors { get; private set; }
            public List<Rect> Players { get; private set; }

            public WorldNavMesh(World world)
            {
                this.World = world;
                this.Update();
            }

            public void Update()
            {
                Actors = new List<Rect>();
                UnWalkable = new List<Rect>();
                Walkable = new List<Rect>();

                float Xl = 0f;
                float Xh = 0f;
                float Yl = 0f;
                float Yh = 0f;

                foreach (var scene in World.QuadTree.Query<Scene>(World.QuadTree.RootNode.Bounds))
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
