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
using System.Drawing;
using System.Windows.Forms;
using Mooege.Core.GS.Players;

namespace Mooege.Core.GS.Map.Debug
{
    public partial class WorldVisualizer : Form
    {
        public World World { get; private set; }
        public Player Player { get; private set; }
        public DebugNavMesh Mesh { get; private set; }
        public Bitmap StageBitmap { get; private set; }
        public Bitmap PreviewBitmap { get; private set; }

        private readonly Pen _selectionPen = new Pen(Brushes.Blue, 2);

        public WorldVisualizer(World world, Player player = null)
        {
            InitializeComponent();

            this.pictureBoxStage.DoubleBuffer();
            this.pictureBoxPreview.DoubleBuffer();

            this.World = world;
            this.Player = player;
            
            radioButtonPlayerProximity.Enabled = radioButtonPlayerProximity.Checked = this.Player != null;
            checkBoxDrawPlayerProximityCircle.Enabled = checkBoxDrawPlayerProximityCircle.Checked = this.Player != null;
            checkBoxDrawPlayerProximityRect.Enabled = checkBoxDrawPlayerProximityRect.Checked = this.Player != null;
        }

        private void WorldVisualizer_Load(object sender, EventArgs e)
        {
            this.Text = string.Format("World Visualizer - {0} [{1}]", this.World.WorldSNO.Name, this.World.WorldSNO.Id);
            this.Mesh = new DebugNavMesh(this.World, this.Player);
            this.Mesh.Update(this.radioButtonAllWorld.Checked);

            this.RequestStageRedraw();
        }

        #region mesh redrawing

        private void RequestStageRedraw()
        {
            this.groupSettings.Enabled = false;

            this.Mesh.DrawMasterScenes = checkBoxMasterScenes.Checked;
            this.Mesh.DrawSubScenes = checkBoxSubScenes.Checked;
            this.Mesh.DrawWalkableCells = checkBoxWalkableCells.Checked;
            this.Mesh.DrawUnwalkableCells = checkBoxUnwalkableCells.Checked;
            this.Mesh.DrawMonsters = checkBoxMonsters.Checked;
            this.Mesh.DrawNPCs = checkBoxNPCs.Checked;
            this.Mesh.DrawPlayers = checkBoxPlayers.Checked;
            this.Mesh.DrawPlayerProximityCircle = checkBoxDrawPlayerProximityCircle.Checked;
            this.Mesh.DrawPlayerProximityRectangle = checkBoxDrawPlayerProximityRect.Checked;
            this.Mesh.PrintSceneLabels = checkBoxPrintSceneLabels.Checked;
            this.Mesh.FillCells = checkBoxFillCells.Checked;

            if (this.StageBitmap != null)
            {
                this.StageBitmap.Dispose();
                this.StageBitmap = null;
            }

            if(this.PreviewBitmap!=null)
            {
                this.PreviewBitmap.Dispose();
                this.PreviewBitmap = null;
            }

            GC.Collect(); // force a garbage collection.
            GC.WaitForPendingFinalizers();

            this.StageBitmap = this.Mesh.Draw();
            this.PreviewBitmap = this.ResizeImage(this.StageBitmap, this.pictureBoxPreview.Width, this.pictureBoxPreview.Height);
            this.pictureBoxStage.Image = this.StageBitmap;

            this.groupSettings.Enabled = true;
        }

        #endregion

        #region preview handling

        private bool _donePaintingPreview = true;

        private void pictureBoxPreview_Paint(object sender, PaintEventArgs e)
        {
            if (!_donePaintingPreview) return;

            _donePaintingPreview = false;
            e.Graphics.DrawImage(this.PreviewBitmap, 0, 0, this.PreviewBitmap.Width, this.PreviewBitmap.Height);

            var rectLeft = (this.pictureBoxPreview.Width * this.panelStage.HorizontalScroll.Value) / this.panelStage.HorizontalScroll.Maximum;
            var rectTop = (this.pictureBoxPreview.Height * this.panelStage.VerticalScroll.Value) / this.panelStage.VerticalScroll.Maximum;
            var rectWidth = (this.pictureBoxPreview.Width * this.panelStage.Size.Width) / this.pictureBoxStage.Width;
            var rectHeight = (this.pictureBoxPreview.Height * this.panelStage.Size.Height) / this.pictureBoxStage.Height;

            e.Graphics.DrawRectangle(this._selectionPen, rectLeft, rectTop, rectWidth, rectHeight);
            _donePaintingPreview = true;
        }

        private void panelStage_Scroll(object sender, ScrollEventArgs e)
        {
            this.pictureBoxPreview.Refresh();
            this.pictureBoxStage.Refresh();
        }

        private void pictureBoxPreview_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            var x = (e.X*this.panelStage.HorizontalScroll.Maximum)/this.pictureBoxPreview.Width;
            var y = (e.Y*this.panelStage.VerticalScroll.Maximum)/this.pictureBoxPreview.Height;

            if (this.panelStage.HorizontalScroll.Minimum <= x && x <= this.panelStage.HorizontalScroll.Maximum)
                this.panelStage.HorizontalScroll.Value = x;

            if (this.panelStage.VerticalScroll.Minimum <= y && y <= this.panelStage.VerticalScroll.Maximum)
                this.panelStage.VerticalScroll.Value = y;

            this.pictureBoxPreview.Refresh();
        }

        private void WorldVisualizer_SizeChanged(object sender, EventArgs e)
        {
            this.pictureBoxPreview.Refresh();
        }

        #endregion

        #region updates

        private void trackBarUpdateFrequency_ValueChanged(object sender, EventArgs e)
        {
            if (trackBarUpdateFrequency.Value == 0)
                timerUpdate.Enabled = false;
            else
            {
                timerUpdate.Interval = trackBarUpdateFrequency.Value*1000;
                timerUpdate.Enabled = true;
            }
        }

        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            this.RequestStageRedraw();
        }

        #endregion

        #region options

        private void checkMasterScenes_CheckedChanged(object sender, System.EventArgs e)
        {
            this.RequestStageRedraw();
        }

        private void checkSubScenes_CheckedChanged(object sender, System.EventArgs e)
        {
            this.RequestStageRedraw();
        }

        private void checkWalkableCells_CheckedChanged(object sender, System.EventArgs e)
        {
            this.RequestStageRedraw();
        }

        private void checkUnwalkableCells_CheckedChanged(object sender, System.EventArgs e)
        {
            this.RequestStageRedraw();
        }

        private void checkPlayers_CheckedChanged(object sender, EventArgs e)
        {
            this.RequestStageRedraw();
        }

        private void checkNPCs_CheckedChanged(object sender, EventArgs e)
        {
            this.RequestStageRedraw();
        }

        private void checkMonsters_CheckedChanged(object sender, EventArgs e)
        {
            this.RequestStageRedraw();
        }

        private void checkPrintLabels_CheckedChanged(object sender, EventArgs e)
        {
            this.RequestStageRedraw();
        }

        private void checkFillCells_CheckedChanged(object sender, EventArgs e)
        {
            this.RequestStageRedraw();
        }

        private void radioButtonAllWorld_CheckedChanged(object sender, EventArgs e)
        {
            if (this.Mesh == null)
                return;

            this.Mesh.Update(this.radioButtonAllWorld.Checked);
            this.RequestStageRedraw();
        }

        private void checkBoxDrawPlayerProximityCircle_CheckedChanged(object sender, EventArgs e)
        {
            if (this.Mesh == null)
                return;

            this.RequestStageRedraw();
        }

        private void checkBoxDrawPlayerProximityRect_CheckedChanged(object sender, EventArgs e)
        {
            if (this.Mesh == null)
                return;

            this.RequestStageRedraw();
        }

        #endregion

        #region bitmap helpers

        public Bitmap ResizeImage(Image image, int width, int height)
        {
            //a holder for the result
            var result = new Bitmap(width, height);

            //use a graphics object to draw the resized image into the bitmap
            using (Graphics graphics = Graphics.FromImage(result))
            {
                //set the resize quality modes to high quality
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                //draw the image into the target bitmap
                graphics.DrawImage(image, 0, 0, result.Width, result.Height);
            }

            //return the resulting bitmap
            return result;
        }

        #endregion

        #region form-close

        private void WorldVisualizer_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.World = null;
            this.Mesh = null;

            if (this.StageBitmap != null)
            {
                this.StageBitmap.Dispose();
                this.StageBitmap = null;
            }

			if(this.PreviewBitmap!=null)
            {
                this.PreviewBitmap.Dispose();
                this.PreviewBitmap = null;
            }

            GC.Collect(); // force a garbage collection.
            GC.WaitForPendingFinalizers();
        }

        #endregion            

        private void btnDumpMeshToObj_Click(object sender, EventArgs e)
        {
            this.Mesh.DumpNavMeshToObj();
        }

    }    
}
