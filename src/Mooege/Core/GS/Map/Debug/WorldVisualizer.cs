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
using System.Drawing;
using System.Windows.Forms;
using Size = System.Drawing.Size;

namespace Mooege.Core.GS.Map.Debug
{
    public partial class WorldVisualizer : Form
    {
        public World World { get; private set; }
        public DebugNavMesh Mesh { get; private set; }
        public Bitmap Bitmap { get; private set; }

        public WorldVisualizer(World world)
        {
            InitializeComponent();
            this.Stage.DoubleBuffer();
            this.World = world;
        }

        private void WorldVisualizer_Load(object sender, EventArgs e)
        {
            this.Text = string.Format("World Visualizer - {0} [{1}]", this.World.WorldSNO.Name, this.World.WorldSNO.SNOId);
            this.Mesh = new DebugNavMesh(this.World);

            splitContainer_Panel1_Resize(null, EventArgs.Empty);
            this.RequestRedraw();
        }

        #region stage-paint & redraw

        private void Stage_Paint(object sender, PaintEventArgs e)
        {
            lock (this.Mesh.Lock)
            {
                e.Graphics.Clear(Color.White);
                e.Graphics.DrawImage(this.Bitmap, 0, 0, this.Bitmap.Width, this.Bitmap.Height);
            }
        }

        private void splitContainer_Panel1_Resize(object sender, EventArgs e)
        {
            if (this.Mesh != null)
                splitContainer.Panel1.AutoScrollMinSize = new Size((int)this.Mesh.Bounds.Width, (int)this.Mesh.Bounds.Height);
        }

        private void RequestRedraw()
        {
            this.groupMapVisibility.Enabled = false;
            this.groupActorVisibility.Enabled = false;
            this.groupOptions.Enabled = false;

            this.Mesh.DrawMasterScenes = checkMasterScenes.Checked;
            this.Mesh.DrawSubScenes = checkSubScenes.Checked;
            this.Mesh.DrawWalkableCells = checkWalkableCells.Checked;
            this.Mesh.DrawUnwalkableCells = checkUnwalkableCells.Checked;
            this.Mesh.DrawMonsters = checkMonsters.Checked;
            this.Mesh.DrawNPCs = checkNPCs.Checked;
            this.Mesh.DrawPlayers = checkPlayers.Checked;
            this.Mesh.PrintLabels = checkPrintLabels.Checked;
            this.Mesh.FillCells = checkFillCells.Checked;

            if (this.Bitmap != null)
            {
                this.Bitmap.Dispose();
                this.Bitmap = null;
            }

            this.Bitmap = this.Mesh.Draw();
            this.Stage.Refresh();

            this.groupMapVisibility.Enabled = true;
            this.groupActorVisibility.Enabled = true;
            this.groupOptions.Enabled = true;
        }

        #endregion

        #region options

        private void checkMasterScenes_CheckedChanged(object sender, System.EventArgs e)
        {
            this.RequestRedraw();
        }

        private void checkSubScenes_CheckedChanged(object sender, System.EventArgs e)
        {
            this.RequestRedraw();
        }

        private void checkWalkableCells_CheckedChanged(object sender, System.EventArgs e)
        {
            this.RequestRedraw();
        }

        private void checkUnwalkableCells_CheckedChanged(object sender, System.EventArgs e)
        {
            this.RequestRedraw();
        }

        private void checkPlayers_CheckedChanged(object sender, EventArgs e)
        {
            this.RequestRedraw();
        }

        private void checkNPCs_CheckedChanged(object sender, EventArgs e)
        {
            this.RequestRedraw();
        }

        private void checkMonsters_CheckedChanged(object sender, EventArgs e)
        {
            this.RequestRedraw();
        }

        private void checkPrintLabels_CheckedChanged(object sender, EventArgs e)
        {
            this.RequestRedraw();
        }

        private void checkFillCells_CheckedChanged(object sender, EventArgs e)
        {
            this.RequestRedraw();
        }

        #endregion

        #region form-close

        private void WorldVisualizer_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.World = null;
            this.Mesh = null;

            if (this.Bitmap != null)
            {
                this.Bitmap.Dispose();
                this.Bitmap = null;
            }

            GC.Collect(); // force a garbage collection.
            GC.WaitForPendingFinalizers();
        }

        #endregion
    }    
}
