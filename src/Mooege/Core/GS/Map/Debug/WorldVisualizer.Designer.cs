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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorldVisualizer));
            this.pictureBoxStage = new System.Windows.Forms.PictureBox();
            this.checkBoxFillCells = new System.Windows.Forms.CheckBox();
            this.checkBoxPrintSceneLabels = new System.Windows.Forms.CheckBox();
            this.groupActorVisibility = new System.Windows.Forms.GroupBox();
            this.checkBoxMonsters = new System.Windows.Forms.CheckBox();
            this.checkBoxPlayers = new System.Windows.Forms.CheckBox();
            this.checkBoxNPCs = new System.Windows.Forms.CheckBox();
            this.groupMapVisibility = new System.Windows.Forms.GroupBox();
            this.checkBoxUnwalkableCells = new System.Windows.Forms.CheckBox();
            this.checkBoxWalkableCells = new System.Windows.Forms.CheckBox();
            this.checkBoxSubScenes = new System.Windows.Forms.CheckBox();
            this.checkBoxMasterScenes = new System.Windows.Forms.CheckBox();
            this.groupSettings = new System.Windows.Forms.GroupBox();
            this.groupOptions = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.trackBarUpdateFrequency = new System.Windows.Forms.TrackBar();
            this.groupBoxRenderMode = new System.Windows.Forms.GroupBox();
            this.checkBoxDrawPlayerProximityRect = new System.Windows.Forms.CheckBox();
            this.checkBoxDrawPlayerProximityCircle = new System.Windows.Forms.CheckBox();
            this.radioButtonPlayerProximity = new System.Windows.Forms.RadioButton();
            this.radioButtonAllWorld = new System.Windows.Forms.RadioButton();
            this.groupPreview = new System.Windows.Forms.GroupBox();
            this.pictureBoxPreview = new System.Windows.Forms.PictureBox();
            this.panelStage = new System.Windows.Forms.Panel();
            this.timerUpdate = new System.Windows.Forms.Timer(this.components);
            this.btnDumpMeshToObj = new System.Windows.Forms.Button();
            this.btnNavToToolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStage)).BeginInit();
            this.groupActorVisibility.SuspendLayout();
            this.groupMapVisibility.SuspendLayout();
            this.groupSettings.SuspendLayout();
            this.groupOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarUpdateFrequency)).BeginInit();
            this.groupBoxRenderMode.SuspendLayout();
            this.groupPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).BeginInit();
            this.panelStage.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBoxStage
            // 
            this.pictureBoxStage.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxStage.Name = "pictureBoxStage";
            this.pictureBoxStage.Size = new System.Drawing.Size(1008, 520);
            this.pictureBoxStage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxStage.TabIndex = 0;
            this.pictureBoxStage.TabStop = false;
            // 
            // checkBoxFillCells
            // 
            this.checkBoxFillCells.AutoSize = true;
            this.checkBoxFillCells.Location = new System.Drawing.Point(6, 43);
            this.checkBoxFillCells.Name = "checkBoxFillCells";
            this.checkBoxFillCells.Size = new System.Drawing.Size(96, 17);
            this.checkBoxFillCells.TabIndex = 1;
            this.checkBoxFillCells.Text = "Fill Visible Cells";
            this.checkBoxFillCells.UseVisualStyleBackColor = true;
            this.checkBoxFillCells.CheckedChanged += new System.EventHandler(this.checkFillCells_CheckedChanged);
            // 
            // checkBoxPrintSceneLabels
            // 
            this.checkBoxPrintSceneLabels.AutoSize = true;
            this.checkBoxPrintSceneLabels.Checked = true;
            this.checkBoxPrintSceneLabels.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxPrintSceneLabels.Location = new System.Drawing.Point(6, 19);
            this.checkBoxPrintSceneLabels.Name = "checkBoxPrintSceneLabels";
            this.checkBoxPrintSceneLabels.Size = new System.Drawing.Size(115, 17);
            this.checkBoxPrintSceneLabels.TabIndex = 0;
            this.checkBoxPrintSceneLabels.Text = "Print Scene Labels";
            this.checkBoxPrintSceneLabels.UseVisualStyleBackColor = true;
            this.checkBoxPrintSceneLabels.CheckedChanged += new System.EventHandler(this.checkPrintLabels_CheckedChanged);
            // 
            // groupActorVisibility
            // 
            this.groupActorVisibility.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupActorVisibility.Controls.Add(this.checkBoxMonsters);
            this.groupActorVisibility.Controls.Add(this.checkBoxPlayers);
            this.groupActorVisibility.Controls.Add(this.checkBoxNPCs);
            this.groupActorVisibility.Location = new System.Drawing.Point(399, 10);
            this.groupActorVisibility.Name = "groupActorVisibility";
            this.groupActorVisibility.Size = new System.Drawing.Size(197, 191);
            this.groupActorVisibility.TabIndex = 4;
            this.groupActorVisibility.TabStop = false;
            this.groupActorVisibility.Text = "Actor Visibility";
            // 
            // checkBoxMonsters
            // 
            this.checkBoxMonsters.AutoSize = true;
            this.checkBoxMonsters.Checked = true;
            this.checkBoxMonsters.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxMonsters.ForeColor = System.Drawing.Color.Green;
            this.checkBoxMonsters.Location = new System.Drawing.Point(6, 65);
            this.checkBoxMonsters.Name = "checkBoxMonsters";
            this.checkBoxMonsters.Size = new System.Drawing.Size(102, 17);
            this.checkBoxMonsters.TabIndex = 4;
            this.checkBoxMonsters.Text = "Monsters Visible";
            this.checkBoxMonsters.UseVisualStyleBackColor = true;
            this.checkBoxMonsters.CheckedChanged += new System.EventHandler(this.checkMonsters_CheckedChanged);
            // 
            // checkBoxPlayers
            // 
            this.checkBoxPlayers.AutoSize = true;
            this.checkBoxPlayers.Checked = true;
            this.checkBoxPlayers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxPlayers.ForeColor = System.Drawing.Color.DarkViolet;
            this.checkBoxPlayers.Location = new System.Drawing.Point(6, 19);
            this.checkBoxPlayers.Name = "checkBoxPlayers";
            this.checkBoxPlayers.Size = new System.Drawing.Size(93, 17);
            this.checkBoxPlayers.TabIndex = 2;
            this.checkBoxPlayers.Text = "Players Visible";
            this.checkBoxPlayers.UseVisualStyleBackColor = true;
            this.checkBoxPlayers.CheckedChanged += new System.EventHandler(this.checkPlayers_CheckedChanged);
            // 
            // checkBoxNPCs
            // 
            this.checkBoxNPCs.AutoSize = true;
            this.checkBoxNPCs.Checked = true;
            this.checkBoxNPCs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxNPCs.ForeColor = System.Drawing.Color.Orange;
            this.checkBoxNPCs.Location = new System.Drawing.Point(6, 42);
            this.checkBoxNPCs.Name = "checkBoxNPCs";
            this.checkBoxNPCs.Size = new System.Drawing.Size(86, 17);
            this.checkBoxNPCs.TabIndex = 3;
            this.checkBoxNPCs.Text = "NPCs Visible";
            this.checkBoxNPCs.UseVisualStyleBackColor = true;
            this.checkBoxNPCs.CheckedChanged += new System.EventHandler(this.checkNPCs_CheckedChanged);
            // 
            // groupMapVisibility
            // 
            this.groupMapVisibility.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupMapVisibility.Controls.Add(this.checkBoxUnwalkableCells);
            this.groupMapVisibility.Controls.Add(this.checkBoxWalkableCells);
            this.groupMapVisibility.Controls.Add(this.checkBoxSubScenes);
            this.groupMapVisibility.Controls.Add(this.checkBoxMasterScenes);
            this.groupMapVisibility.Location = new System.Drawing.Point(196, 10);
            this.groupMapVisibility.Name = "groupMapVisibility";
            this.groupMapVisibility.Size = new System.Drawing.Size(197, 191);
            this.groupMapVisibility.TabIndex = 3;
            this.groupMapVisibility.TabStop = false;
            this.groupMapVisibility.Text = "Map Visibility";
            // 
            // checkBoxUnwalkableCells
            // 
            this.checkBoxUnwalkableCells.AutoSize = true;
            this.checkBoxUnwalkableCells.ForeColor = System.Drawing.Color.Red;
            this.checkBoxUnwalkableCells.Location = new System.Drawing.Point(6, 88);
            this.checkBoxUnwalkableCells.Name = "checkBoxUnwalkableCells";
            this.checkBoxUnwalkableCells.Size = new System.Drawing.Size(140, 17);
            this.checkBoxUnwalkableCells.TabIndex = 4;
            this.checkBoxUnwalkableCells.Text = "Unwalkable Cells Visible";
            this.checkBoxUnwalkableCells.UseVisualStyleBackColor = true;
            this.checkBoxUnwalkableCells.CheckedChanged += new System.EventHandler(this.checkUnwalkableCells_CheckedChanged);
            // 
            // checkBoxWalkableCells
            // 
            this.checkBoxWalkableCells.AutoSize = true;
            this.checkBoxWalkableCells.Checked = true;
            this.checkBoxWalkableCells.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxWalkableCells.ForeColor = System.Drawing.Color.Blue;
            this.checkBoxWalkableCells.Location = new System.Drawing.Point(6, 65);
            this.checkBoxWalkableCells.Name = "checkBoxWalkableCells";
            this.checkBoxWalkableCells.Size = new System.Drawing.Size(129, 17);
            this.checkBoxWalkableCells.TabIndex = 3;
            this.checkBoxWalkableCells.Text = "Walkable Cells Visible";
            this.checkBoxWalkableCells.UseVisualStyleBackColor = true;
            this.checkBoxWalkableCells.CheckedChanged += new System.EventHandler(this.checkWalkableCells_CheckedChanged);
            // 
            // checkBoxSubScenes
            // 
            this.checkBoxSubScenes.AutoSize = true;
            this.checkBoxSubScenes.Checked = true;
            this.checkBoxSubScenes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSubScenes.ForeColor = System.Drawing.Color.DarkGray;
            this.checkBoxSubScenes.Location = new System.Drawing.Point(6, 42);
            this.checkBoxSubScenes.Name = "checkBoxSubScenes";
            this.checkBoxSubScenes.Size = new System.Drawing.Size(117, 17);
            this.checkBoxSubScenes.TabIndex = 2;
            this.checkBoxSubScenes.Text = "Sub Scenes Visible";
            this.checkBoxSubScenes.UseVisualStyleBackColor = true;
            this.checkBoxSubScenes.CheckedChanged += new System.EventHandler(this.checkSubScenes_CheckedChanged);
            // 
            // checkBoxMasterScenes
            // 
            this.checkBoxMasterScenes.AutoSize = true;
            this.checkBoxMasterScenes.Checked = true;
            this.checkBoxMasterScenes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxMasterScenes.Location = new System.Drawing.Point(6, 19);
            this.checkBoxMasterScenes.Name = "checkBoxMasterScenes";
            this.checkBoxMasterScenes.Size = new System.Drawing.Size(130, 17);
            this.checkBoxMasterScenes.TabIndex = 1;
            this.checkBoxMasterScenes.Text = "Master Scenes Visible";
            this.checkBoxMasterScenes.UseVisualStyleBackColor = true;
            this.checkBoxMasterScenes.CheckedChanged += new System.EventHandler(this.checkMasterScenes_CheckedChanged);
            // 
            // groupSettings
            // 
            this.groupSettings.Controls.Add(this.groupOptions);
            this.groupSettings.Controls.Add(this.groupBoxRenderMode);
            this.groupSettings.Controls.Add(this.groupPreview);
            this.groupSettings.Controls.Add(this.groupMapVisibility);
            this.groupSettings.Controls.Add(this.groupActorVisibility);
            this.groupSettings.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupSettings.Location = new System.Drawing.Point(0, 520);
            this.groupSettings.Name = "groupSettings";
            this.groupSettings.Size = new System.Drawing.Size(1008, 210);
            this.groupSettings.TabIndex = 1;
            this.groupSettings.TabStop = false;
            // 
            // groupOptions
            // 
            this.groupOptions.Controls.Add(this.btnDumpMeshToObj);
            this.groupOptions.Controls.Add(this.label1);
            this.groupOptions.Controls.Add(this.trackBarUpdateFrequency);
            this.groupOptions.Controls.Add(this.checkBoxFillCells);
            this.groupOptions.Controls.Add(this.checkBoxPrintSceneLabels);
            this.groupOptions.Location = new System.Drawing.Point(805, 10);
            this.groupOptions.Name = "groupOptions";
            this.groupOptions.Size = new System.Drawing.Size(197, 191);
            this.groupOptions.TabIndex = 7;
            this.groupOptions.TabStop = false;
            this.groupOptions.Text = "Options";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Update Frequency:";
            // 
            // trackBarUpdateFrequency
            // 
            this.trackBarUpdateFrequency.Location = new System.Drawing.Point(6, 87);
            this.trackBarUpdateFrequency.Name = "trackBarUpdateFrequency";
            this.trackBarUpdateFrequency.Size = new System.Drawing.Size(185, 45);
            this.trackBarUpdateFrequency.TabIndex = 2;
            this.trackBarUpdateFrequency.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarUpdateFrequency.ValueChanged += new System.EventHandler(this.trackBarUpdateFrequency_ValueChanged);
            // 
            // groupBoxRenderMode
            // 
            this.groupBoxRenderMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxRenderMode.Controls.Add(this.checkBoxDrawPlayerProximityRect);
            this.groupBoxRenderMode.Controls.Add(this.checkBoxDrawPlayerProximityCircle);
            this.groupBoxRenderMode.Controls.Add(this.radioButtonPlayerProximity);
            this.groupBoxRenderMode.Controls.Add(this.radioButtonAllWorld);
            this.groupBoxRenderMode.Location = new System.Drawing.Point(602, 10);
            this.groupBoxRenderMode.Name = "groupBoxRenderMode";
            this.groupBoxRenderMode.Size = new System.Drawing.Size(197, 191);
            this.groupBoxRenderMode.TabIndex = 6;
            this.groupBoxRenderMode.TabStop = false;
            this.groupBoxRenderMode.Text = "Render Mode";
            // 
            // checkBoxDrawPlayerProximityRect
            // 
            this.checkBoxDrawPlayerProximityRect.AutoSize = true;
            this.checkBoxDrawPlayerProximityRect.Enabled = false;
            this.checkBoxDrawPlayerProximityRect.Location = new System.Drawing.Point(5, 87);
            this.checkBoxDrawPlayerProximityRect.Name = "checkBoxDrawPlayerProximityRect";
            this.checkBoxDrawPlayerProximityRect.Size = new System.Drawing.Size(153, 17);
            this.checkBoxDrawPlayerProximityRect.TabIndex = 2;
            this.checkBoxDrawPlayerProximityRect.Text = "Draw Player Proximity Rect";
            this.checkBoxDrawPlayerProximityRect.UseVisualStyleBackColor = true;
            this.checkBoxDrawPlayerProximityRect.CheckedChanged += new System.EventHandler(this.checkBoxDrawPlayerProximityRect_CheckedChanged);
            // 
            // checkBoxDrawPlayerProximityCircle
            // 
            this.checkBoxDrawPlayerProximityCircle.AutoSize = true;
            this.checkBoxDrawPlayerProximityCircle.Enabled = false;
            this.checkBoxDrawPlayerProximityCircle.Location = new System.Drawing.Point(5, 65);
            this.checkBoxDrawPlayerProximityCircle.Name = "checkBoxDrawPlayerProximityCircle";
            this.checkBoxDrawPlayerProximityCircle.Size = new System.Drawing.Size(156, 17);
            this.checkBoxDrawPlayerProximityCircle.TabIndex = 1;
            this.checkBoxDrawPlayerProximityCircle.Text = "Draw Player Proximity Circle";
            this.checkBoxDrawPlayerProximityCircle.UseVisualStyleBackColor = true;
            this.checkBoxDrawPlayerProximityCircle.CheckedChanged += new System.EventHandler(this.checkBoxDrawPlayerProximityCircle_CheckedChanged);
            // 
            // radioButtonPlayerProximity
            // 
            this.radioButtonPlayerProximity.AutoSize = true;
            this.radioButtonPlayerProximity.Enabled = false;
            this.radioButtonPlayerProximity.Location = new System.Drawing.Point(6, 42);
            this.radioButtonPlayerProximity.Name = "radioButtonPlayerProximity";
            this.radioButtonPlayerProximity.Size = new System.Drawing.Size(149, 17);
            this.radioButtonPlayerProximity.TabIndex = 0;
            this.radioButtonPlayerProximity.Text = "Objects In Player Proximity";
            this.radioButtonPlayerProximity.UseVisualStyleBackColor = true;
            // 
            // radioButtonAllWorld
            // 
            this.radioButtonAllWorld.AutoSize = true;
            this.radioButtonAllWorld.Checked = true;
            this.radioButtonAllWorld.Location = new System.Drawing.Point(6, 19);
            this.radioButtonAllWorld.Name = "radioButtonAllWorld";
            this.radioButtonAllWorld.Size = new System.Drawing.Size(82, 17);
            this.radioButtonAllWorld.TabIndex = 0;
            this.radioButtonAllWorld.TabStop = true;
            this.radioButtonAllWorld.Text = "All the world";
            this.radioButtonAllWorld.UseVisualStyleBackColor = true;
            this.radioButtonAllWorld.CheckedChanged += new System.EventHandler(this.radioButtonAllWorld_CheckedChanged);
            // 
            // groupPreview
            // 
            this.groupPreview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupPreview.Controls.Add(this.pictureBoxPreview);
            this.groupPreview.Location = new System.Drawing.Point(6, 10);
            this.groupPreview.Name = "groupPreview";
            this.groupPreview.Size = new System.Drawing.Size(186, 194);
            this.groupPreview.TabIndex = 2;
            this.groupPreview.TabStop = false;
            // 
            // pictureBoxPreview
            // 
            this.pictureBoxPreview.Location = new System.Drawing.Point(4, 9);
            this.pictureBoxPreview.Name = "pictureBoxPreview";
            this.pictureBoxPreview.Size = new System.Drawing.Size(180, 180);
            this.pictureBoxPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxPreview.TabIndex = 3;
            this.pictureBoxPreview.TabStop = false;
            this.pictureBoxPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxPreview_Paint);
            this.pictureBoxPreview.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxPreview_MouseMove);
            // 
            // panelStage
            // 
            this.panelStage.AutoScroll = true;
            this.panelStage.AutoScrollMinSize = new System.Drawing.Size(1008, 520);
            this.panelStage.Controls.Add(this.pictureBoxStage);
            this.panelStage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStage.Location = new System.Drawing.Point(0, 0);
            this.panelStage.Name = "panelStage";
            this.panelStage.Size = new System.Drawing.Size(1008, 520);
            this.panelStage.TabIndex = 0;
            this.panelStage.Scroll += new System.Windows.Forms.ScrollEventHandler(this.panelStage_Scroll);
            // 
            // timerUpdate
            // 
            this.timerUpdate.Tick += new System.EventHandler(this.timerUpdate_Tick);
            // 
            // btnDumpMeshToObj
            // 
            this.btnDumpMeshToObj.Location = new System.Drawing.Point(6, 162);
            this.btnDumpMeshToObj.Name = "btnDumpMeshToObj";
            this.btnDumpMeshToObj.Size = new System.Drawing.Size(75, 23);
            this.btnDumpMeshToObj.TabIndex = 4;
            this.btnDumpMeshToObj.Text = "NavTo .Obj";
            this.btnNavToToolTip.SetToolTip(this.btnDumpMeshToObj, "Dumps all walkable cells into a 2d WaveFront model, placed in your Mooege root fo" +
                    "lder");
            this.btnDumpMeshToObj.UseVisualStyleBackColor = true;
            this.btnDumpMeshToObj.Click += new System.EventHandler(this.btnDumpMeshToObj_Click);
            // 
            // WorldVisualizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 730);
            this.Controls.Add(this.panelStage);
            this.Controls.Add(this.groupSettings);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "WorldVisualizer";
            this.Text = "World Visualizer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WorldVisualizer_FormClosing);
            this.Load += new System.EventHandler(this.WorldVisualizer_Load);
            this.SizeChanged += new System.EventHandler(this.WorldVisualizer_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStage)).EndInit();
            this.groupActorVisibility.ResumeLayout(false);
            this.groupActorVisibility.PerformLayout();
            this.groupMapVisibility.ResumeLayout(false);
            this.groupMapVisibility.PerformLayout();
            this.groupSettings.ResumeLayout(false);
            this.groupOptions.ResumeLayout(false);
            this.groupOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarUpdateFrequency)).EndInit();
            this.groupBoxRenderMode.ResumeLayout(false);
            this.groupBoxRenderMode.PerformLayout();
            this.groupPreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).EndInit();
            this.panelStage.ResumeLayout(false);
            this.panelStage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxStage;
        private System.Windows.Forms.GroupBox groupMapVisibility;
        private System.Windows.Forms.CheckBox checkBoxUnwalkableCells;
        private System.Windows.Forms.CheckBox checkBoxWalkableCells;
        private System.Windows.Forms.CheckBox checkBoxSubScenes;
        private System.Windows.Forms.CheckBox checkBoxMasterScenes;
        private System.Windows.Forms.CheckBox checkBoxMonsters;
        private System.Windows.Forms.CheckBox checkBoxNPCs;
        private System.Windows.Forms.CheckBox checkBoxPlayers;
        private System.Windows.Forms.GroupBox groupActorVisibility;
        private System.Windows.Forms.CheckBox checkBoxFillCells;
        private System.Windows.Forms.CheckBox checkBoxPrintSceneLabels;
        private System.Windows.Forms.GroupBox groupSettings;
        private System.Windows.Forms.Panel panelStage;
        private System.Windows.Forms.GroupBox groupPreview;
        private System.Windows.Forms.PictureBox pictureBoxPreview;
        private System.Windows.Forms.GroupBox groupBoxRenderMode;
        private System.Windows.Forms.RadioButton radioButtonPlayerProximity;
        private System.Windows.Forms.RadioButton radioButtonAllWorld;
        private System.Windows.Forms.GroupBox groupOptions;
        private System.Windows.Forms.CheckBox checkBoxDrawPlayerProximityRect;
        private System.Windows.Forms.CheckBox checkBoxDrawPlayerProximityCircle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar trackBarUpdateFrequency;
        private System.Windows.Forms.Timer timerUpdate;
        private System.Windows.Forms.Button btnDumpMeshToObj;
        private System.Windows.Forms.ToolTip btnNavToToolTip;

    }
}