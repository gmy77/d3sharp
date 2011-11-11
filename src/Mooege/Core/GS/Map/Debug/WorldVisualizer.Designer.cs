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
            this.stage = new System.Windows.Forms.PictureBox();
            this.groupOptions = new System.Windows.Forms.GroupBox();
            this.checkBoxFillCells = new System.Windows.Forms.CheckBox();
            this.checkBoxPrintLabels = new System.Windows.Forms.CheckBox();
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
            this.panelStage = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.stage)).BeginInit();
            this.groupOptions.SuspendLayout();
            this.groupActorVisibility.SuspendLayout();
            this.groupMapVisibility.SuspendLayout();
            this.groupSettings.SuspendLayout();
            this.panelStage.SuspendLayout();
            this.SuspendLayout();
            // 
            // stage
            // 
            this.stage.Location = new System.Drawing.Point(6, 3);
            this.stage.Name = "stage";
            this.stage.Size = new System.Drawing.Size(1002, 595);
            this.stage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.stage.TabIndex = 0;
            this.stage.TabStop = false;
            this.stage.Paint += new System.Windows.Forms.PaintEventHandler(this.Stage_Paint);
            // 
            // groupOptions
            // 
            this.groupOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupOptions.Controls.Add(this.checkBoxFillCells);
            this.groupOptions.Controls.Add(this.checkBoxPrintLabels);
            this.groupOptions.Location = new System.Drawing.Point(256, 10);
            this.groupOptions.Name = "groupOptions";
            this.groupOptions.Size = new System.Drawing.Size(119, 110);
            this.groupOptions.TabIndex = 2;
            this.groupOptions.TabStop = false;
            this.groupOptions.Text = "Options";
            // 
            // checkBoxFillCells
            // 
            this.checkBoxFillCells.AutoSize = true;
            this.checkBoxFillCells.Location = new System.Drawing.Point(6, 42);
            this.checkBoxFillCells.Name = "checkBoxFillCells";
            this.checkBoxFillCells.Size = new System.Drawing.Size(63, 17);
            this.checkBoxFillCells.TabIndex = 1;
            this.checkBoxFillCells.Text = "Fill Cells";
            this.checkBoxFillCells.UseVisualStyleBackColor = true;
            this.checkBoxFillCells.CheckedChanged += new System.EventHandler(this.checkFillCells_CheckedChanged);
            // 
            // checkBoxPrintLabels
            // 
            this.checkBoxPrintLabels.AutoSize = true;
            this.checkBoxPrintLabels.Checked = true;
            this.checkBoxPrintLabels.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxPrintLabels.Location = new System.Drawing.Point(6, 19);
            this.checkBoxPrintLabels.Name = "checkBoxPrintLabels";
            this.checkBoxPrintLabels.Size = new System.Drawing.Size(81, 17);
            this.checkBoxPrintLabels.TabIndex = 0;
            this.checkBoxPrintLabels.Text = "Print Labels";
            this.checkBoxPrintLabels.UseVisualStyleBackColor = true;
            this.checkBoxPrintLabels.CheckedChanged += new System.EventHandler(this.checkPrintLabels_CheckedChanged);
            // 
            // groupActorVisibility
            // 
            this.groupActorVisibility.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupActorVisibility.Controls.Add(this.checkBoxMonsters);
            this.groupActorVisibility.Controls.Add(this.checkBoxPlayers);
            this.groupActorVisibility.Controls.Add(this.checkBoxNPCs);
            this.groupActorVisibility.Location = new System.Drawing.Point(131, 10);
            this.groupActorVisibility.Name = "groupActorVisibility";
            this.groupActorVisibility.Size = new System.Drawing.Size(119, 110);
            this.groupActorVisibility.TabIndex = 1;
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
            this.checkBoxMonsters.Size = new System.Drawing.Size(69, 17);
            this.checkBoxMonsters.TabIndex = 4;
            this.checkBoxMonsters.Text = "Monsters";
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
            this.checkBoxPlayers.Size = new System.Drawing.Size(60, 17);
            this.checkBoxPlayers.TabIndex = 2;
            this.checkBoxPlayers.Text = "Players";
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
            this.checkBoxNPCs.Size = new System.Drawing.Size(53, 17);
            this.checkBoxNPCs.TabIndex = 3;
            this.checkBoxNPCs.Text = "NPCs";
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
            this.groupMapVisibility.Location = new System.Drawing.Point(6, 10);
            this.groupMapVisibility.Name = "groupMapVisibility";
            this.groupMapVisibility.Size = new System.Drawing.Size(119, 110);
            this.groupMapVisibility.TabIndex = 0;
            this.groupMapVisibility.TabStop = false;
            this.groupMapVisibility.Text = "Map Visibility";
            // 
            // checkBoxUnwalkableCells
            // 
            this.checkBoxUnwalkableCells.AutoSize = true;
            this.checkBoxUnwalkableCells.Checked = true;
            this.checkBoxUnwalkableCells.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxUnwalkableCells.ForeColor = System.Drawing.Color.Red;
            this.checkBoxUnwalkableCells.Location = new System.Drawing.Point(6, 88);
            this.checkBoxUnwalkableCells.Name = "checkBoxUnwalkableCells";
            this.checkBoxUnwalkableCells.Size = new System.Drawing.Size(107, 17);
            this.checkBoxUnwalkableCells.TabIndex = 4;
            this.checkBoxUnwalkableCells.Text = "Unwalkable Cells";
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
            this.checkBoxWalkableCells.Size = new System.Drawing.Size(96, 17);
            this.checkBoxWalkableCells.TabIndex = 3;
            this.checkBoxWalkableCells.Text = "Walkable Cells";
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
            this.checkBoxSubScenes.Size = new System.Drawing.Size(84, 17);
            this.checkBoxSubScenes.TabIndex = 2;
            this.checkBoxSubScenes.Text = "Sub Scenes";
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
            this.checkBoxMasterScenes.Size = new System.Drawing.Size(97, 17);
            this.checkBoxMasterScenes.TabIndex = 1;
            this.checkBoxMasterScenes.Text = "Master Scenes";
            this.checkBoxMasterScenes.UseVisualStyleBackColor = true;
            this.checkBoxMasterScenes.CheckedChanged += new System.EventHandler(this.checkMasterScenes_CheckedChanged);
            // 
            // groupSettings
            // 
            this.groupSettings.Controls.Add(this.groupOptions);
            this.groupSettings.Controls.Add(this.groupMapVisibility);
            this.groupSettings.Controls.Add(this.groupActorVisibility);
            this.groupSettings.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupSettings.Location = new System.Drawing.Point(0, 604);
            this.groupSettings.Name = "groupSettings";
            this.groupSettings.Size = new System.Drawing.Size(1008, 126);
            this.groupSettings.TabIndex = 3;
            this.groupSettings.TabStop = false;
            // 
            // panelStage
            // 
            this.panelStage.AutoScroll = true;
            this.panelStage.AutoScrollMinSize = new System.Drawing.Size(500, 500);
            this.panelStage.Controls.Add(this.stage);
            this.panelStage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStage.Location = new System.Drawing.Point(0, 0);
            this.panelStage.Name = "panelStage";
            this.panelStage.Size = new System.Drawing.Size(1008, 604);
            this.panelStage.TabIndex = 4;
            // 
            // WorldVisualizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 730);
            this.Controls.Add(this.panelStage);
            this.Controls.Add(this.groupSettings);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WorldVisualizer";
            this.Text = "World Visualizer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WorldVisualizer_FormClosing);
            this.Load += new System.EventHandler(this.WorldVisualizer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.stage)).EndInit();
            this.groupOptions.ResumeLayout(false);
            this.groupOptions.PerformLayout();
            this.groupActorVisibility.ResumeLayout(false);
            this.groupActorVisibility.PerformLayout();
            this.groupMapVisibility.ResumeLayout(false);
            this.groupMapVisibility.PerformLayout();
            this.groupSettings.ResumeLayout(false);
            this.panelStage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox stage;
        private System.Windows.Forms.GroupBox groupMapVisibility;
        private System.Windows.Forms.CheckBox checkBoxUnwalkableCells;
        private System.Windows.Forms.CheckBox checkBoxWalkableCells;
        private System.Windows.Forms.CheckBox checkBoxSubScenes;
        private System.Windows.Forms.CheckBox checkBoxMasterScenes;
        private System.Windows.Forms.CheckBox checkBoxMonsters;
        private System.Windows.Forms.CheckBox checkBoxNPCs;
        private System.Windows.Forms.CheckBox checkBoxPlayers;
        private System.Windows.Forms.GroupBox groupActorVisibility;
        private System.Windows.Forms.GroupBox groupOptions;
        private System.Windows.Forms.CheckBox checkBoxFillCells;
        private System.Windows.Forms.CheckBox checkBoxPrintLabels;
        private System.Windows.Forms.GroupBox groupSettings;
        private System.Windows.Forms.Panel panelStage;

    }
}