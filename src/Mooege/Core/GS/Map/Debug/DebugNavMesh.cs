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
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Players;

namespace Mooege.Core.GS.Map.Debug
{
    public class DebugNavMesh
    {
        public World World { get; private set; }
        public Rect Bounds { get { return World.QuadTree.RootNode.Bounds; } }

        public object Lock = new object();

        public List<Scene> MasterScenes { get; private set; }
        public List<Scene> SubScenes { get; private set; }
        public List<Rect> UnWalkableCells { get; private set; }
        public List<Rect> WalkableCells { get; private set; }
        public List<Player> Players { get; private set; }
        public List<Monster> Monsters { get; private set; }
        public List<NPC> NPCs { get; private set; }

        public bool DrawMasterScenes;
        public bool DrawSubScenes;
        public bool DrawWalkableCells;
        public bool DrawUnwalkableCells;
        public bool DrawMonsters;
        public bool DrawNPCs;
        public bool DrawPlayers;
        public bool PrintLabels;
        public bool FillCells;

        private readonly Pen _masterScenePen = new Pen(Color.Black, 1.0f);
        private readonly Pen _subScenePen = new Pen(Color.DarkGray, 1.0f);
        private readonly Brush _unwalkableBrush = Brushes.Red;
        private readonly Pen _unwalkablePen = new Pen(Color.Red, 1.0f);
        private readonly Brush _walkableBrush = Brushes.Blue;
        private readonly Pen _walkablePen = new Pen(Color.Blue, 1.0f);
        private readonly Font _sceneFont = new Font("Verdana", 7);

        public DebugNavMesh(World world)
        {
            this.World = world;

            this.MasterScenes = new List<Scene>();
            this.SubScenes = new List<Scene>();
            this.UnWalkableCells = new List<Rect>();
            this.WalkableCells = new List<Rect>();
            this.Players = new List<Player>();
            this.Monsters = new List<Monster>();
            this.NPCs = new List<NPC>();

            this._subScenePen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;

            this.Update();
        }

        #region update

        public void Update()
        {
            lock (this.Lock)
            {
                this.MasterScenes.Clear();
                this.SubScenes.Clear();
                this.WalkableCells.Clear();
                this.UnWalkableCells.Clear();
                this.Players.Clear();
                this.Monsters.Clear();
                this.NPCs.Clear();

                foreach (var scene in World.QuadTree.Query<Scene>(World.QuadTree.RootNode.Bounds))
                {
                    if (scene.Parent == null)
                        this.MasterScenes.Add(scene);
                    else
                        this.SubScenes.Add(scene);

                    this.AnalyzeScene(scene);
                }

                foreach (var actor in World.QuadTree.Query<Actor>(World.QuadTree.RootNode.Bounds))
                {
                    if (actor is Player)
                        this.Players.Add(actor as Player);
                    else if (actor is NPC)
                        this.NPCs.Add(actor as NPC);
                    else if (actor is Monster)
                        this.Monsters.Add(actor as Monster);
                }
            }
        }

        private void AnalyzeScene(Scene scene)
        {
            foreach (var cell in scene.NavZone.NavCells)
            {
                float x = scene.Position.X + cell.Min.X;
                float y = scene.Position.Y + cell.Min.Y;

                float sizex = cell.Max.X - cell.Min.X;
                float sizey = cell.Max.Y - cell.Min.Y;

                var rect = new Rect(x, y, sizex, sizey);

                if ((cell.Flags & 0x1) != 0x1)
                    UnWalkableCells.Add(rect);
                else
                    WalkableCells.Add(rect);
            }
        }

        #endregion

        #region drawing 

        public Bitmap Draw()
        {
            lock (this.Lock)
            {
                var bitmap = new Bitmap((int)this.World.QuadTree.RootNode.Bounds.Width, (int)this.World.QuadTree.RootNode.Bounds.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                var graphics = Graphics.FromImage(bitmap);

                graphics.FillRectangle(Brushes.Wheat, 0, 0, bitmap.Width, bitmap.Height);

                this.DrawShapes(graphics);

                if (this.PrintLabels)
                    this.DrawLabels(graphics);

                graphics.Save();
                graphics.Dispose();

                return bitmap;
            }
        }

        private void DrawShapes(Graphics graphics)
        {
            if (this.DrawMasterScenes)
            {
                foreach (var scene in this.MasterScenes)
                {
                    this.DrawScene(graphics, scene);
                }
            }

            if (this.DrawSubScenes)
            {
                foreach (var scene in this.SubScenes)
                {
                    this.DrawScene(graphics, scene);
                }
            }

            if (this.DrawWalkableCells)
                this.DrawWalkables(graphics);

            if (this.DrawUnwalkableCells)
                this.DrawUnwalkables(graphics);

            if (this.DrawMonsters)
            {
                foreach (var monster in this.Monsters)
                {
                    this.DrawActor(monster, graphics, Brushes.Green, 7);
                }
            }

            if (this.DrawNPCs)
            {
                foreach (var npc in this.NPCs)
                {
                    this.DrawActor(npc, graphics, Brushes.Orange, 7);
                }
            }

            if (this.DrawPlayers)
            {
                foreach (var player in this.Players)
                {
                    this.DrawActor(player, graphics, Brushes.DarkViolet, 7);
                }
            }
        }

        private void DrawScene(Graphics graphics, Scene scene)
        {
            var rect = new Rectangle((int)scene.Bounds.Left, (int)scene.Bounds.Top, (int)scene.Bounds.Width, (int)scene.Bounds.Height);
            graphics.DrawRectangle(scene.Parent == null ? _masterScenePen : _subScenePen, rect);
        }

        private void DrawWalkables(Graphics graphics)
        {
            foreach (var cell in this.WalkableCells)
            {
                var rect = new Rectangle(new System.Drawing.Point((int)cell.Left, (int)cell.Top), new System.Drawing.Size((int)cell.Width, (int)cell.Height));
                
                if (this.FillCells)
                    graphics.FillRectangle(_walkableBrush, rect);
                else
                    graphics.DrawRectangle(_walkablePen, rect);
            }
        }

        private void DrawUnwalkables(Graphics graphics)
        {
            foreach (var cell in this.UnWalkableCells)
            {
                var rect = new Rectangle(new System.Drawing.Point((int)cell.Left, (int)cell.Top), new System.Drawing.Size((int)cell.Width, (int)cell.Height));
                
                if (this.FillCells)
                    graphics.FillRectangle(_unwalkableBrush, rect);
                else
                    graphics.DrawRectangle(_unwalkablePen, rect);
            }
        }

        private void DrawActor(Actor actor, Graphics graphics, Brush brush, int radius)
        {
            var rect = new Rectangle((int)actor.Bounds.X, (int)actor.Bounds.Y, (int)actor.Bounds.Width + radius, (int)actor.Bounds.Height + radius);
            graphics.FillEllipse(brush, rect);
        }

        private void DrawLabels(Graphics graphics)
        {
            if (this.DrawMasterScenes)
            {
                foreach (var scene in this.MasterScenes)
                {
                    this.DrawSceneLabel(graphics, scene);
                }
            }

            if (this.DrawSubScenes)
            {
                foreach (var scene in this.SubScenes)
                {
                    this.DrawSceneLabel(graphics, scene);
                }
            }
        }

        private void DrawSceneLabel(Graphics graphics, Scene scene)
        {
            var stringRectangle = new RectangleF((float)scene.Bounds.Left, (float)scene.Bounds.Top, (float)scene.Bounds.Width, (float)scene.Bounds.Height);
            var drawFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

            if (!string.IsNullOrEmpty(scene.SceneSNO.Name))
                graphics.DrawString(scene.SceneSNO.Name, _sceneFont, scene.Parent == null ? Brushes.Black : Brushes.Gray, stringRectangle, drawFormat);
        }

        #endregion
    }
}
