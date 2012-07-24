namespace Mooege.Tools.StringViewer
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxInput = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBoxOutputType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.hexBox = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cboProtos = new System.Windows.Forms.ComboBox();
            this.richTextBoxProto = new System.Windows.Forms.RichTextBox();
            this.textBoxProtoType = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdDetect = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.textBoxInput);
            this.groupBox1.Location = new System.Drawing.Point(12, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(429, 123);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input Proto String";
            // 
            // textBoxInput
            // 
            this.textBoxInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxInput.Location = new System.Drawing.Point(3, 16);
            this.textBoxInput.Multiline = true;
            this.textBoxInput.Name = "textBoxInput";
            this.textBoxInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxInput.Size = new System.Drawing.Size(423, 104);
            this.textBoxInput.TabIndex = 0;
            this.textBoxInput.TextChanged += new System.EventHandler(this.textBoxInput_TextChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.comboBoxOutputType);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.hexBox);
            this.groupBox2.Location = new System.Drawing.Point(12, 138);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(429, 177);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Hex Output";
            // 
            // comboBoxOutputType
            // 
            this.comboBoxOutputType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxOutputType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOutputType.FormattingEnabled = true;
            this.comboBoxOutputType.Items.AddRange(new object[] {
            "Raw",
            "C# Array"});
            this.comboBoxOutputType.Location = new System.Drawing.Point(49, 23);
            this.comboBoxOutputType.Name = "comboBoxOutputType";
            this.comboBoxOutputType.Size = new System.Drawing.Size(374, 21);
            this.comboBoxOutputType.TabIndex = 2;
            this.comboBoxOutputType.SelectedIndexChanged += new System.EventHandler(this.comboBoxOutputType_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Type:";
            // 
            // hexBox
            // 
            this.hexBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hexBox.Location = new System.Drawing.Point(12, 51);
            this.hexBox.Multiline = true;
            this.hexBox.Name = "hexBox";
            this.hexBox.ReadOnly = true;
            this.hexBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.hexBox.Size = new System.Drawing.Size(411, 120);
            this.hexBox.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.cmdDetect);
            this.groupBox3.Controls.Add(this.cboProtos);
            this.groupBox3.Controls.Add(this.richTextBoxProto);
            this.groupBox3.Controls.Add(this.textBoxProtoType);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(15, 321);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(429, 177);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Proto Output";
            // 
            // cboProtos
            // 
            this.cboProtos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProtos.FormattingEnabled = true;
            this.cboProtos.Location = new System.Drawing.Point(126, 42);
            this.cboProtos.Name = "cboProtos";
            this.cboProtos.Size = new System.Drawing.Size(225, 21);
            this.cboProtos.TabIndex = 7;
            this.cboProtos.SelectedIndexChanged += new System.EventHandler(this.cboProtos_SelectedIndexChanged);
            // 
            // richTextBoxProto
            // 
            this.richTextBoxProto.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxProto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBoxProto.Location = new System.Drawing.Point(9, 69);
            this.richTextBoxProto.Name = "richTextBoxProto";
            this.richTextBoxProto.ReadOnly = true;
            this.richTextBoxProto.Size = new System.Drawing.Size(411, 102);
            this.richTextBoxProto.TabIndex = 5;
            this.richTextBoxProto.Text = "";
            // 
            // textBoxProtoType
            // 
            this.textBoxProtoType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxProtoType.Location = new System.Drawing.Point(126, 16);
            this.textBoxProtoType.Name = "textBoxProtoType";
            this.textBoxProtoType.Size = new System.Drawing.Size(225, 20);
            this.textBoxProtoType.TabIndex = 4;
            this.textBoxProtoType.TextChanged += new System.EventHandler(this.textBoxProtoType_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Convert as proto type:";
            // 
            // cmdDetect
            // 
            this.cmdDetect.Location = new System.Drawing.Point(363, 42);
            this.cmdDetect.Name = "cmdDetect";
            this.cmdDetect.Size = new System.Drawing.Size(57, 21);
            this.cmdDetect.TabIndex = 8;
            this.cmdDetect.Text = "Detect";
            this.cmdDetect.UseVisualStyleBackColor = true;
            this.cmdDetect.Click += new System.EventHandler(this.cmdDetect_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(453, 510);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.Text = "String Viewer";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxInput;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBoxProtoType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox richTextBoxProto;
        private System.Windows.Forms.TextBox hexBox;
        private System.Windows.Forms.ComboBox comboBoxOutputType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboProtos;
        private System.Windows.Forms.Button cmdDetect;
    }
}

