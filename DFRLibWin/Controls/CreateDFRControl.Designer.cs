namespace DFRLib.Win.Controls {
    partial class CreateDFRControl {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            cbCombineAllAppendedData = new System.Windows.Forms.CheckBox();
            cbOpenWhenGenerated = new System.Windows.Forms.CheckBox();
            btnGenerateDFR = new System.Windows.Forms.Button();
            labelSeparator2 = new System.Windows.Forms.Label();
            tbOutputFile = new System.Windows.Forms.TextBox();
            tbAlteredFile = new System.Windows.Forms.TextBox();
            tbOriginalFile = new System.Windows.Forms.TextBox();
            labelOutputFile = new System.Windows.Forms.Label();
            labelSelectInputs = new System.Windows.Forms.Label();
            labelSeparator1 = new System.Windows.Forms.Label();
            btnOutputFile = new System.Windows.Forms.Button();
            btnAlteredFile = new System.Windows.Forms.Button();
            btnOriginalFile = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // cbCombineAllAppendedData
            // 
            cbCombineAllAppendedData.Anchor =  System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            cbCombineAllAppendedData.AutoSize = true;
            cbCombineAllAppendedData.Checked = true;
            cbCombineAllAppendedData.CheckState = System.Windows.Forms.CheckState.Checked;
            cbCombineAllAppendedData.Location = new System.Drawing.Point(4, 160);
            cbCombineAllAppendedData.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cbCombineAllAppendedData.Name = "cbCombineAllAppendedData";
            cbCombineAllAppendedData.Size = new System.Drawing.Size(172, 19);
            cbCombineAllAppendedData.TabIndex = 22;
            cbCombineAllAppendedData.Text = "Combine all appended data";
            cbCombineAllAppendedData.UseVisualStyleBackColor = true;
            // 
            // cbOpenWhenGenerated
            // 
            cbOpenWhenGenerated.Anchor =  System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            cbOpenWhenGenerated.AutoSize = true;
            cbOpenWhenGenerated.Checked = true;
            cbOpenWhenGenerated.CheckState = System.Windows.Forms.CheckState.Checked;
            cbOpenWhenGenerated.Location = new System.Drawing.Point(4, 183);
            cbOpenWhenGenerated.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cbOpenWhenGenerated.Name = "cbOpenWhenGenerated";
            cbOpenWhenGenerated.Size = new System.Drawing.Size(255, 34);
            cbOpenWhenGenerated.TabIndex = 23;
            cbOpenWhenGenerated.Text = "Open automatically when created\r\n(.DFR files can be opened in any text editor)";
            cbOpenWhenGenerated.UseVisualStyleBackColor = true;
            // 
            // btnGenerateDFR
            // 
            btnGenerateDFR.Anchor =  System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnGenerateDFR.Location = new System.Drawing.Point(672, 193);
            btnGenerateDFR.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnGenerateDFR.Name = "btnGenerateDFR";
            btnGenerateDFR.Size = new System.Drawing.Size(102, 29);
            btnGenerateDFR.TabIndex = 24;
            btnGenerateDFR.Text = "Create DFR";
            btnGenerateDFR.UseVisualStyleBackColor = true;
            btnGenerateDFR.Click += btnGenerateDFR_Click;
            // 
            // labelSeparator2
            // 
            labelSeparator2.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            labelSeparator2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            labelSeparator2.Location = new System.Drawing.Point(0, 147);
            labelSeparator2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelSeparator2.Name = "labelSeparator2";
            labelSeparator2.Size = new System.Drawing.Size(774, 2);
            labelSeparator2.TabIndex = 20;
            // 
            // tbOutputFile
            // 
            tbOutputFile.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tbOutputFile.BackColor = System.Drawing.SystemColors.Window;
            tbOutputFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tbOutputFile.Location = new System.Drawing.Point(108, 114);
            tbOutputFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tbOutputFile.Name = "tbOutputFile";
            tbOutputFile.Size = new System.Drawing.Size(665, 23);
            tbOutputFile.TabIndex = 21;
            // 
            // tbAlteredFile
            // 
            tbAlteredFile.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tbAlteredFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tbAlteredFile.Location = new System.Drawing.Point(108, 51);
            tbAlteredFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tbAlteredFile.Name = "tbAlteredFile";
            tbAlteredFile.Size = new System.Drawing.Size(665, 23);
            tbAlteredFile.TabIndex = 16;
            // 
            // tbOriginalFile
            // 
            tbOriginalFile.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tbOriginalFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tbOriginalFile.Location = new System.Drawing.Point(108, 22);
            tbOriginalFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tbOriginalFile.Name = "tbOriginalFile";
            tbOriginalFile.Size = new System.Drawing.Size(665, 23);
            tbOriginalFile.TabIndex = 14;
            // 
            // labelOutputFile
            // 
            labelOutputFile.AutoSize = true;
            labelOutputFile.Location = new System.Drawing.Point(0, 93);
            labelOutputFile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelOutputFile.Name = "labelOutputFile";
            labelOutputFile.Size = new System.Drawing.Size(142, 15);
            labelOutputFile.TabIndex = 18;
            labelOutputFile.Text = "Select Output Destination";
            // 
            // labelSelectInputs
            // 
            labelSelectInputs.AutoSize = true;
            labelSelectInputs.Location = new System.Drawing.Point(0, 0);
            labelSelectInputs.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelSelectInputs.Name = "labelSelectInputs";
            labelSelectInputs.Size = new System.Drawing.Size(95, 15);
            labelSelectInputs.TabIndex = 12;
            labelSelectInputs.Text = "Select Input Files";
            // 
            // labelSeparator1
            // 
            labelSeparator1.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            labelSeparator1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            labelSeparator1.Location = new System.Drawing.Point(0, 83);
            labelSeparator1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelSeparator1.Name = "labelSeparator1";
            labelSeparator1.Size = new System.Drawing.Size(774, 2);
            labelSeparator1.TabIndex = 17;
            // 
            // btnOutputFile
            // 
            btnOutputFile.Location = new System.Drawing.Point(0, 113);
            btnOutputFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnOutputFile.Name = "btnOutputFile";
            btnOutputFile.Size = new System.Drawing.Size(102, 25);
            btnOutputFile.TabIndex = 19;
            btnOutputFile.Text = "Output File...";
            btnOutputFile.UseVisualStyleBackColor = true;
            btnOutputFile.Click += btnOutputFile_Click;
            // 
            // btnAlteredFile
            // 
            btnAlteredFile.Location = new System.Drawing.Point(0, 50);
            btnAlteredFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnAlteredFile.Name = "btnAlteredFile";
            btnAlteredFile.Size = new System.Drawing.Size(102, 25);
            btnAlteredFile.TabIndex = 15;
            btnAlteredFile.Text = "Altered File...";
            btnAlteredFile.UseVisualStyleBackColor = true;
            btnAlteredFile.Click += btnAlteredFile_Click;
            // 
            // btnOriginalFile
            // 
            btnOriginalFile.Location = new System.Drawing.Point(0, 21);
            btnOriginalFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnOriginalFile.Name = "btnOriginalFile";
            btnOriginalFile.Size = new System.Drawing.Size(102, 25);
            btnOriginalFile.TabIndex = 13;
            btnOriginalFile.Text = "Original File...";
            btnOriginalFile.UseVisualStyleBackColor = true;
            btnOriginalFile.Click += btnOriginalFile_Click;
            // 
            // CreateDFRControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.Transparent;
            Controls.Add(cbCombineAllAppendedData);
            Controls.Add(cbOpenWhenGenerated);
            Controls.Add(btnGenerateDFR);
            Controls.Add(labelSeparator2);
            Controls.Add(tbOutputFile);
            Controls.Add(tbAlteredFile);
            Controls.Add(tbOriginalFile);
            Controls.Add(labelOutputFile);
            Controls.Add(labelSelectInputs);
            Controls.Add(labelSeparator1);
            Controls.Add(btnOutputFile);
            Controls.Add(btnAlteredFile);
            Controls.Add(btnOriginalFile);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "CreateDFRControl";
            Size = new System.Drawing.Size(774, 222);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.CheckBox cbCombineAllAppendedData;
        private System.Windows.Forms.CheckBox cbOpenWhenGenerated;
        private System.Windows.Forms.Button btnGenerateDFR;
        private System.Windows.Forms.Label labelSeparator2;
        private System.Windows.Forms.TextBox tbOutputFile;
        private System.Windows.Forms.TextBox tbAlteredFile;
        private System.Windows.Forms.TextBox tbOriginalFile;
        private System.Windows.Forms.Label labelOutputFile;
        private System.Windows.Forms.Label labelSelectInputs;
        private System.Windows.Forms.Label labelSeparator1;
        private System.Windows.Forms.Button btnOutputFile;
        private System.Windows.Forms.Button btnAlteredFile;
        private System.Windows.Forms.Button btnOriginalFile;
    }
}
