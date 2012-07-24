namespace GameMessageViewer
{
    partial class MessageFilter
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
            this.cmdOk = new System.Windows.Forms.Button();
            this.Presets = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // cmdOk
            // 
            this.cmdOk.Location = new System.Drawing.Point(863, 343);
            this.cmdOk.Name = "cmdOk";
            this.cmdOk.Size = new System.Drawing.Size(144, 34);
            this.cmdOk.TabIndex = 0;
            this.cmdOk.Text = "OK";
            this.cmdOk.UseVisualStyleBackColor = true;
            this.cmdOk.Click += new System.EventHandler(this.cmdOk_Click);
            // 
            // Presets
            // 
            this.Presets.BackColor = System.Drawing.SystemColors.Window;
            this.Presets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Presets.FormattingEnabled = true;
            this.Presets.Location = new System.Drawing.Point(12, 351);
            this.Presets.Name = "Presets";
            this.Presets.Size = new System.Drawing.Size(162, 21);
            this.Presets.TabIndex = 3;
            this.Presets.SelectedIndexChanged += new System.EventHandler(this.Presets_SelectedIndexChanged);
            // 
            // MessageFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1019, 389);
            this.ControlBox = false;
            this.Controls.Add(this.Presets);
            this.Controls.Add(this.cmdOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MessageFilter";
            this.Text = "Message filter";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdOk;
        private System.Windows.Forms.ComboBox Presets;
    }
}