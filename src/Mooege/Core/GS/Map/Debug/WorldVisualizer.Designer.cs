namespace Mooege.Core.GS.Map.Debug
{
    partial class WorldVisualizer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorldVisualizer));
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.Stage = new System.Windows.Forms.PictureBox();
            this.groupActorVisibility = new System.Windows.Forms.GroupBox();
            this.checkMonsters = new System.Windows.Forms.CheckBox();
            this.checkPlayers = new System.Windows.Forms.CheckBox();
            this.checkNPCs = new System.Windows.Forms.CheckBox();
            this.groupMapVisibility = new System.Windows.Forms.GroupBox();
            this.checkUnwalkableCells = new System.Windows.Forms.CheckBox();
            this.checkWalkableCells = new System.Windows.Forms.CheckBox();
            this.checkSubScenes = new System.Windows.Forms.CheckBox();
            this.checkMasterScenes = new System.Windows.Forms.CheckBox();
            this.groupOptions = new System.Windows.Forms.GroupBox();
            this.checkPrintLabels = new System.Windows.Forms.CheckBox();
            this.checkFillCells = new System.Windows.Forms.CheckBox();
			#if NET_4_0
			// Does not work for mono yet. https://bugzilla.novell.com/show_bug.cgi?id=648403			
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
			#endif
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Stage)).BeginInit();
            this.groupActorVisibility.SuspendLayout();
            this.groupMapVisibility.SuspendLayout();
            this.groupOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.AutoScroll = true;
            this.splitContainer.Panel1.Controls.Add(this.Stage);
            this.splitContainer.Panel1.Resize += new System.EventHandler(this.splitContainer_Panel1_Resize);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.groupOptions);
            this.splitContainer.Panel2.Controls.Add(this.groupActorVisibility);
            this.splitContainer.Panel2.Controls.Add(this.groupMapVisibility);
            this.splitContainer.Size = new System.Drawing.Size(1008, 730);
            this.splitContainer.SplitterDistance = 597;
            this.splitContainer.TabIndex = 2;
            // 
            // Stage
            // 
            this.Stage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Stage.Location = new System.Drawing.Point(0, 0);
            this.Stage.Name = "Stage";
            this.Stage.Size = new System.Drawing.Size(1008, 597);
            this.Stage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Stage.TabIndex = 0;
            this.Stage.TabStop = false;
            this.Stage.Paint += new System.Windows.Forms.PaintEventHandler(this.Stage_Paint);
            // 
            // groupActorVisibility
            // 
            this.groupActorVisibility.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupActorVisibility.Controls.Add(this.checkMonsters);
            this.groupActorVisibility.Controls.Add(this.checkPlayers);
            this.groupActorVisibility.Controls.Add(this.checkNPCs);
            this.groupActorVisibility.Location = new System.Drawing.Point(137, 9);
            this.groupActorVisibility.Name = "groupActorVisibility";
            this.groupActorVisibility.Size = new System.Drawing.Size(119, 108);
            this.groupActorVisibility.TabIndex = 1;
            this.groupActorVisibility.TabStop = false;
            this.groupActorVisibility.Text = "Actor Visibility";
            // 
            // checkMonsters
            // 
            this.checkMonsters.AutoSize = true;
            this.checkMonsters.Checked = true;
            this.checkMonsters.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkMonsters.ForeColor = System.Drawing.Color.Green;
            this.checkMonsters.Location = new System.Drawing.Point(6, 65);
            this.checkMonsters.Name = "checkMonsters";
            this.checkMonsters.Size = new System.Drawing.Size(69, 17);
            this.checkMonsters.TabIndex = 4;
            this.checkMonsters.Text = "Monsters";
            this.checkMonsters.UseVisualStyleBackColor = true;
            this.checkMonsters.CheckedChanged += new System.EventHandler(this.checkMonsters_CheckedChanged);
            // 
            // checkPlayers
            // 
            this.checkPlayers.AutoSize = true;
            this.checkPlayers.Checked = true;
            this.checkPlayers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkPlayers.ForeColor = System.Drawing.Color.DarkViolet;
            this.checkPlayers.Location = new System.Drawing.Point(6, 19);
            this.checkPlayers.Name = "checkPlayers";
            this.checkPlayers.Size = new System.Drawing.Size(60, 17);
            this.checkPlayers.TabIndex = 2;
            this.checkPlayers.Text = "Players";
            this.checkPlayers.UseVisualStyleBackColor = true;
            this.checkPlayers.CheckedChanged += new System.EventHandler(this.checkPlayers_CheckedChanged);
            // 
            // checkNPCs
            // 
            this.checkNPCs.AutoSize = true;
            this.checkNPCs.Checked = true;
            this.checkNPCs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkNPCs.ForeColor = System.Drawing.Color.Orange;
            this.checkNPCs.Location = new System.Drawing.Point(6, 42);
            this.checkNPCs.Name = "checkNPCs";
            this.checkNPCs.Size = new System.Drawing.Size(53, 17);
            this.checkNPCs.TabIndex = 3;
            this.checkNPCs.Text = "NPCs";
            this.checkNPCs.UseVisualStyleBackColor = true;
            this.checkNPCs.CheckedChanged += new System.EventHandler(this.checkNPCs_CheckedChanged);
            // 
            // groupMapVisibility
            // 
            this.groupMapVisibility.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupMapVisibility.Controls.Add(this.checkUnwalkableCells);
            this.groupMapVisibility.Controls.Add(this.checkWalkableCells);
            this.groupMapVisibility.Controls.Add(this.checkSubScenes);
            this.groupMapVisibility.Controls.Add(this.checkMasterScenes);
            this.groupMapVisibility.Location = new System.Drawing.Point(12, 9);
            this.groupMapVisibility.Name = "groupMapVisibility";
            this.groupMapVisibility.Size = new System.Drawing.Size(119, 108);
            this.groupMapVisibility.TabIndex = 0;
            this.groupMapVisibility.TabStop = false;
            this.groupMapVisibility.Text = "Map Visibility";
            // 
            // checkUnwalkableCells
            // 
            this.checkUnwalkableCells.AutoSize = true;
            this.checkUnwalkableCells.Checked = true;
            this.checkUnwalkableCells.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkUnwalkableCells.ForeColor = System.Drawing.Color.Red;
            this.checkUnwalkableCells.Location = new System.Drawing.Point(6, 88);
            this.checkUnwalkableCells.Name = "checkUnwalkableCells";
            this.checkUnwalkableCells.Size = new System.Drawing.Size(107, 17);
            this.checkUnwalkableCells.TabIndex = 4;
            this.checkUnwalkableCells.Text = "Unwalkable Cells";
            this.checkUnwalkableCells.UseVisualStyleBackColor = true;
            this.checkUnwalkableCells.CheckedChanged += new System.EventHandler(this.checkUnwalkableCells_CheckedChanged);
            // 
            // checkWalkableCells
            // 
            this.checkWalkableCells.AutoSize = true;
            this.checkWalkableCells.Checked = true;
            this.checkWalkableCells.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkWalkableCells.ForeColor = System.Drawing.Color.Blue;
            this.checkWalkableCells.Location = new System.Drawing.Point(6, 65);
            this.checkWalkableCells.Name = "checkWalkableCells";
            this.checkWalkableCells.Size = new System.Drawing.Size(96, 17);
            this.checkWalkableCells.TabIndex = 3;
            this.checkWalkableCells.Text = "Walkable Cells";
            this.checkWalkableCells.UseVisualStyleBackColor = true;
            this.checkWalkableCells.CheckedChanged += new System.EventHandler(this.checkWalkableCells_CheckedChanged);
            // 
            // checkSubScenes
            // 
            this.checkSubScenes.AutoSize = true;
            this.checkSubScenes.Checked = true;
            this.checkSubScenes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkSubScenes.ForeColor = System.Drawing.Color.DarkGray;
            this.checkSubScenes.Location = new System.Drawing.Point(6, 42);
            this.checkSubScenes.Name = "checkSubScenes";
            this.checkSubScenes.Size = new System.Drawing.Size(84, 17);
            this.checkSubScenes.TabIndex = 2;
            this.checkSubScenes.Text = "Sub Scenes";
            this.checkSubScenes.UseVisualStyleBackColor = true;
            this.checkSubScenes.CheckedChanged += new System.EventHandler(this.checkSubScenes_CheckedChanged);
            // 
            // checkMasterScenes
            // 
            this.checkMasterScenes.AutoSize = true;
            this.checkMasterScenes.Checked = true;
            this.checkMasterScenes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkMasterScenes.Location = new System.Drawing.Point(6, 19);
            this.checkMasterScenes.Name = "checkMasterScenes";
            this.checkMasterScenes.Size = new System.Drawing.Size(97, 17);
            this.checkMasterScenes.TabIndex = 1;
            this.checkMasterScenes.Text = "Master Scenes";
            this.checkMasterScenes.UseVisualStyleBackColor = true;
            this.checkMasterScenes.CheckedChanged += new System.EventHandler(this.checkMasterScenes_CheckedChanged);
            // 
            // groupOptions
            // 
            this.groupOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupOptions.Controls.Add(this.checkFillCells);
            this.groupOptions.Controls.Add(this.checkPrintLabels);
            this.groupOptions.Location = new System.Drawing.Point(262, 9);
            this.groupOptions.Name = "groupOptions";
            this.groupOptions.Size = new System.Drawing.Size(119, 108);
            this.groupOptions.TabIndex = 2;
            this.groupOptions.TabStop = false;
            this.groupOptions.Text = "Options";
            // 
            // checkPrintLabels
            // 
            this.checkPrintLabels.AutoSize = true;
            this.checkPrintLabels.Checked = true;
            this.checkPrintLabels.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkPrintLabels.Location = new System.Drawing.Point(6, 19);
            this.checkPrintLabels.Name = "checkPrintLabels";
            this.checkPrintLabels.Size = new System.Drawing.Size(81, 17);
            this.checkPrintLabels.TabIndex = 0;
            this.checkPrintLabels.Text = "Print Labels";
            this.checkPrintLabels.UseVisualStyleBackColor = true;
            this.checkPrintLabels.CheckedChanged += new System.EventHandler(this.checkPrintLabels_CheckedChanged);
            // 
            // checkFillCells
            // 
            this.checkFillCells.AutoSize = true;
            this.checkFillCells.Location = new System.Drawing.Point(6, 42);
            this.checkFillCells.Name = "checkFillCells";
            this.checkFillCells.Size = new System.Drawing.Size(63, 17);
            this.checkFillCells.TabIndex = 1;
            this.checkFillCells.Text = "Fill Cells";
            this.checkFillCells.UseVisualStyleBackColor = true;
            this.checkFillCells.CheckedChanged += new System.EventHandler(this.checkFillCells_CheckedChanged);
            // 
            // WorldVisualizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 730);
            this.Controls.Add(this.splitContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WorldVisualizer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "World Visualizer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WorldVisualizer_FormClosing);
            this.Load += new System.EventHandler(this.WorldVisualizer_Load);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
			#if NET_4_0
			// Does not work for mono yet. https://bugzilla.novell.com/show_bug.cgi?id=648403	
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
			#endif
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Stage)).EndInit();
            this.groupActorVisibility.ResumeLayout(false);
            this.groupActorVisibility.PerformLayout();
            this.groupMapVisibility.ResumeLayout(false);
            this.groupMapVisibility.PerformLayout();
            this.groupOptions.ResumeLayout(false);
            this.groupOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.PictureBox Stage;
        private System.Windows.Forms.GroupBox groupMapVisibility;
        private System.Windows.Forms.CheckBox checkUnwalkableCells;
        private System.Windows.Forms.CheckBox checkWalkableCells;
        private System.Windows.Forms.CheckBox checkSubScenes;
        private System.Windows.Forms.CheckBox checkMasterScenes;
        private System.Windows.Forms.CheckBox checkMonsters;
        private System.Windows.Forms.CheckBox checkNPCs;
        private System.Windows.Forms.CheckBox checkPlayers;
        private System.Windows.Forms.GroupBox groupActorVisibility;
        private System.Windows.Forms.GroupBox groupOptions;
        private System.Windows.Forms.CheckBox checkFillCells;
        private System.Windows.Forms.CheckBox checkPrintLabels;

    }
}