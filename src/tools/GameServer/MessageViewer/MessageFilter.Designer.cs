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
            this.cmdAll = new System.Windows.Forms.Button();
            this.cmdNone = new System.Windows.Forms.Button();
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
            // cmdAll
            // 
            this.cmdAll.Location = new System.Drawing.Point(12, 343);
            this.cmdAll.Name = "cmdAll";
            this.cmdAll.Size = new System.Drawing.Size(54, 34);
            this.cmdAll.TabIndex = 1;
            this.cmdAll.Text = "All";
            this.cmdAll.UseVisualStyleBackColor = true;
            this.cmdAll.Click += new System.EventHandler(this.cmdAll_Click);
            // 
            // cmdNone
            // 
            this.cmdNone.Location = new System.Drawing.Point(72, 343);
            this.cmdNone.Name = "cmdNone";
            this.cmdNone.Size = new System.Drawing.Size(54, 34);
            this.cmdNone.TabIndex = 2;
            this.cmdNone.Text = "None";
            this.cmdNone.UseVisualStyleBackColor = true;
            this.cmdNone.Click += new System.EventHandler(this.cmdNone_Click);
            // 
            // MessageFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1019, 389);
            this.ControlBox = false;
            this.Controls.Add(this.cmdNone);
            this.Controls.Add(this.cmdAll);
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
        private System.Windows.Forms.Button cmdAll;
        private System.Windows.Forms.Button cmdNone;
    }
}