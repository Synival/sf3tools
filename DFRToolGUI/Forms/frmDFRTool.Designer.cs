namespace DFRToolGUI.Forms
{
    partial class frmDFRTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDFRTool));
            btnOutputFile = new Button();
            btnAlteredFile = new Button();
            btnOriginalFile = new Button();
            labelSeparator1 = new Label();
            labelSelectInputs = new Label();
            labelOutputFile = new Label();
            btnGenerateDFR = new Button();
            tbOriginalFile = new TextBox();
            tbAlteredFile = new TextBox();
            tbOutputFile = new TextBox();
            labelSeparator2 = new Label();
            cbOpenWhenGenerated = new CheckBox();
            SuspendLayout();
            // 
            // btnOutputFile
            // 
            btnOutputFile.Location = new Point(12, 117);
            btnOutputFile.Name = "btnOutputFile";
            btnOutputFile.Size = new Size(102, 23);
            btnOutputFile.TabIndex = 2;
            btnOutputFile.Text = "Output File...";
            btnOutputFile.UseVisualStyleBackColor = true;
            btnOutputFile.Click += btnOutputFile_Click;
            // 
            // btnAlteredFile
            // 
            btnAlteredFile.Location = new Point(12, 56);
            btnAlteredFile.Name = "btnAlteredFile";
            btnAlteredFile.Size = new Size(102, 23);
            btnAlteredFile.TabIndex = 1;
            btnAlteredFile.Text = "Altered File...";
            btnAlteredFile.UseVisualStyleBackColor = true;
            btnAlteredFile.Click += btnAlteredFile_Click;
            // 
            // btnOriginalFile
            // 
            btnOriginalFile.Location = new Point(12, 27);
            btnOriginalFile.Name = "btnOriginalFile";
            btnOriginalFile.Size = new Size(102, 23);
            btnOriginalFile.TabIndex = 0;
            btnOriginalFile.Text = "Original File...";
            btnOriginalFile.UseVisualStyleBackColor = true;
            btnOriginalFile.Click += btnOriginalFile_Click;
            // 
            // labelSeparator1
            // 
            labelSeparator1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            labelSeparator1.BorderStyle = BorderStyle.Fixed3D;
            labelSeparator1.Location = new Point(12, 88);
            labelSeparator1.Name = "labelSeparator1";
            labelSeparator1.Size = new Size(555, 2);
            labelSeparator1.TabIndex = 3;
            // 
            // labelSelectInputs
            // 
            labelSelectInputs.AutoSize = true;
            labelSelectInputs.Location = new Point(12, 9);
            labelSelectInputs.Name = "labelSelectInputs";
            labelSelectInputs.Size = new Size(95, 15);
            labelSelectInputs.TabIndex = 4;
            labelSelectInputs.Text = "Select Input Files";
            // 
            // labelOutputFile
            // 
            labelOutputFile.AutoSize = true;
            labelOutputFile.Location = new Point(12, 99);
            labelOutputFile.Name = "labelOutputFile";
            labelOutputFile.Size = new Size(142, 15);
            labelOutputFile.TabIndex = 5;
            labelOutputFile.Text = "Select Output Destination";
            // 
            // btnGenerateDFR
            // 
            btnGenerateDFR.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnGenerateDFR.Location = new Point(465, 166);
            btnGenerateDFR.Name = "btnGenerateDFR";
            btnGenerateDFR.Size = new Size(102, 23);
            btnGenerateDFR.TabIndex = 6;
            btnGenerateDFR.Text = "Generate DFR";
            btnGenerateDFR.UseVisualStyleBackColor = true;
            btnGenerateDFR.Click += btnGenerateDFR_Click;
            // 
            // tbOriginalFile
            // 
            tbOriginalFile.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbOriginalFile.BorderStyle = BorderStyle.FixedSingle;
            tbOriginalFile.Location = new Point(120, 28);
            tbOriginalFile.Name = "tbOriginalFile";
            tbOriginalFile.Size = new Size(447, 23);
            tbOriginalFile.TabIndex = 7;
            // 
            // tbAlteredFile
            // 
            tbAlteredFile.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbAlteredFile.BorderStyle = BorderStyle.FixedSingle;
            tbAlteredFile.Location = new Point(120, 56);
            tbAlteredFile.Name = "tbAlteredFile";
            tbAlteredFile.Size = new Size(447, 23);
            tbAlteredFile.TabIndex = 8;
            // 
            // tbOutputFile
            // 
            tbOutputFile.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbOutputFile.BackColor = SystemColors.Window;
            tbOutputFile.BorderStyle = BorderStyle.FixedSingle;
            tbOutputFile.Location = new Point(120, 117);
            tbOutputFile.Name = "tbOutputFile";
            tbOutputFile.Size = new Size(447, 23);
            tbOutputFile.TabIndex = 9;
            // 
            // labelSeparator2
            // 
            labelSeparator2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            labelSeparator2.BorderStyle = BorderStyle.Fixed3D;
            labelSeparator2.Location = new Point(12, 149);
            labelSeparator2.Name = "labelSeparator2";
            labelSeparator2.Size = new Size(555, 2);
            labelSeparator2.TabIndex = 10;
            // 
            // cbOpenWhenGenerated
            // 
            cbOpenWhenGenerated.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            cbOpenWhenGenerated.AutoSize = true;
            cbOpenWhenGenerated.Location = new Point(204, 161);
            cbOpenWhenGenerated.Name = "cbOpenWhenGenerated";
            cbOpenWhenGenerated.Size = new Size(255, 34);
            cbOpenWhenGenerated.TabIndex = 11;
            cbOpenWhenGenerated.Text = "Open automatically when generated\r\n(.DFR files can be opened in any text editor)";
            cbOpenWhenGenerated.UseVisualStyleBackColor = true;
            // 
            // frmDFRTool
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(579, 207);
            Controls.Add(cbOpenWhenGenerated);
            Controls.Add(labelSeparator2);
            Controls.Add(tbOutputFile);
            Controls.Add(tbAlteredFile);
            Controls.Add(tbOriginalFile);
            Controls.Add(btnGenerateDFR);
            Controls.Add(labelOutputFile);
            Controls.Add(labelSelectInputs);
            Controls.Add(labelSeparator1);
            Controls.Add(btnOutputFile);
            Controls.Add(btnAlteredFile);
            Controls.Add(btnOriginalFile);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximumSize = new Size(1280, 246);
            MinimumSize = new Size(405, 246);
            Name = "frmDFRTool";
            Text = "DFRTool";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button btnOutputFile;
        private Button btnAlteredFile;
        private Button btnOriginalFile;
        private Label labelSeparator1;
        private Label labelSelectInputs;
        private Label labelOutputFile;
        private Button btnGenerateDFR;
        private TextBox tbOriginalFile;
        private TextBox tbAlteredFile;
        private TextBox tbOutputFile;
        private Label labelSeparator2;
        private CheckBox cbOpenWhenGenerated;
    }
}