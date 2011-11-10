namespace Mooege.Core.GS.Map
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
            this.Stage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Stage)).BeginInit();
            this.SuspendLayout();
            // 
            // Stage
            // 
            this.Stage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Stage.Location = new System.Drawing.Point(0, 0);
            this.Stage.Name = "Stage";
            this.Stage.Size = new System.Drawing.Size(678, 494);
            this.Stage.TabIndex = 0;
            this.Stage.TabStop = false;
            this.Stage.Paint += new System.Windows.Forms.PaintEventHandler(this.Stage_Paint);
            // 
            // WorldVisualizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(678, 494);
            this.Controls.Add(this.Stage);
            this.Name = "WorldVisualizer";
            this.Text = "WorldVisualizer";
            ((System.ComponentModel.ISupportInitialize)(this.Stage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Stage;
    }
}