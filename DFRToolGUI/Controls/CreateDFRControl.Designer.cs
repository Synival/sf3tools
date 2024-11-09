namespace DFRTool.GUI.Controls {
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
            this.cbCombineAllAppendedData = new System.Windows.Forms.CheckBox();
            this.cbOpenWhenGenerated = new System.Windows.Forms.CheckBox();
            this.btnGenerateDFR = new System.Windows.Forms.Button();
            this.labelSeparator2 = new System.Windows.Forms.Label();
            this.tbOutputFile = new System.Windows.Forms.TextBox();
            this.tbAlteredFile = new System.Windows.Forms.TextBox();
            this.tbOriginalFile = new System.Windows.Forms.TextBox();
            this.labelOutputFile = new System.Windows.Forms.Label();
            this.labelSelectInputs = new System.Windows.Forms.Label();
            this.labelSeparator1 = new System.Windows.Forms.Label();
            this.btnOutputFile = new System.Windows.Forms.Button();
            this.btnAlteredFile = new System.Windows.Forms.Button();
            this.btnOriginalFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbCombineAllAppendedData
            // 
            this.cbCombineAllAppendedData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbCombineAllAppendedData.AutoSize = true;
            this.cbCombineAllAppendedData.Checked = true;
            this.cbCombineAllAppendedData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCombineAllAppendedData.Location = new System.Drawing.Point(3, 138);
            this.cbCombineAllAppendedData.Name = "cbCombineAllAppendedData";
            this.cbCombineAllAppendedData.Size = new System.Drawing.Size(155, 17);
            this.cbCombineAllAppendedData.TabIndex = 22;
            this.cbCombineAllAppendedData.Text = "Combine all appended data";
            this.cbCombineAllAppendedData.UseVisualStyleBackColor = true;
            // 
            // cbOpenWhenGenerated
            // 
            this.cbOpenWhenGenerated.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbOpenWhenGenerated.AutoSize = true;
            this.cbOpenWhenGenerated.Checked = true;
            this.cbOpenWhenGenerated.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbOpenWhenGenerated.Location = new System.Drawing.Point(3, 158);
            this.cbOpenWhenGenerated.Name = "cbOpenWhenGenerated";
            this.cbOpenWhenGenerated.Size = new System.Drawing.Size(233, 30);
            this.cbOpenWhenGenerated.TabIndex = 23;
            this.cbOpenWhenGenerated.Text = "Open automatically when created\r\n(.DFR files can be opened in any text editor)";
            this.cbOpenWhenGenerated.UseVisualStyleBackColor = true;
            // 
            // btnGenerateDFR
            // 
            this.btnGenerateDFR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerateDFR.Location = new System.Drawing.Point(576, 167);
            this.btnGenerateDFR.Name = "btnGenerateDFR";
            this.btnGenerateDFR.Size = new System.Drawing.Size(87, 25);
            this.btnGenerateDFR.TabIndex = 24;
            this.btnGenerateDFR.Text = "Create DFR";
            this.btnGenerateDFR.UseVisualStyleBackColor = true;
            this.btnGenerateDFR.Click += new System.EventHandler(this.btnGenerateDFR_Click);
            // 
            // labelSeparator2
            // 
            this.labelSeparator2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSeparator2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelSeparator2.Location = new System.Drawing.Point(0, 127);
            this.labelSeparator2.Name = "labelSeparator2";
            this.labelSeparator2.Size = new System.Drawing.Size(663, 2);
            this.labelSeparator2.TabIndex = 20;
            // 
            // tbOutputFile
            // 
            this.tbOutputFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOutputFile.BackColor = System.Drawing.SystemColors.Window;
            this.tbOutputFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbOutputFile.Location = new System.Drawing.Point(93, 99);
            this.tbOutputFile.Name = "tbOutputFile";
            this.tbOutputFile.Size = new System.Drawing.Size(570, 20);
            this.tbOutputFile.TabIndex = 21;
            // 
            // tbAlteredFile
            // 
            this.tbAlteredFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbAlteredFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbAlteredFile.Location = new System.Drawing.Point(93, 44);
            this.tbAlteredFile.Name = "tbAlteredFile";
            this.tbAlteredFile.Size = new System.Drawing.Size(570, 20);
            this.tbAlteredFile.TabIndex = 16;
            // 
            // tbOriginalFile
            // 
            this.tbOriginalFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOriginalFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbOriginalFile.Location = new System.Drawing.Point(93, 19);
            this.tbOriginalFile.Name = "tbOriginalFile";
            this.tbOriginalFile.Size = new System.Drawing.Size(570, 20);
            this.tbOriginalFile.TabIndex = 14;
            // 
            // labelOutputFile
            // 
            this.labelOutputFile.AutoSize = true;
            this.labelOutputFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOutputFile.Location = new System.Drawing.Point(0, 81);
            this.labelOutputFile.Name = "labelOutputFile";
            this.labelOutputFile.Size = new System.Drawing.Size(153, 13);
            this.labelOutputFile.TabIndex = 18;
            this.labelOutputFile.Text = "Select Output Destination";
            // 
            // labelSelectInputs
            // 
            this.labelSelectInputs.AutoSize = true;
            this.labelSelectInputs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSelectInputs.Location = new System.Drawing.Point(0, 0);
            this.labelSelectInputs.Name = "labelSelectInputs";
            this.labelSelectInputs.Size = new System.Drawing.Size(106, 13);
            this.labelSelectInputs.TabIndex = 12;
            this.labelSelectInputs.Text = "Select Input Files";
            // 
            // labelSeparator1
            // 
            this.labelSeparator1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSeparator1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelSeparator1.Location = new System.Drawing.Point(0, 72);
            this.labelSeparator1.Name = "labelSeparator1";
            this.labelSeparator1.Size = new System.Drawing.Size(663, 2);
            this.labelSeparator1.TabIndex = 17;
            // 
            // btnOutputFile
            // 
            this.btnOutputFile.Location = new System.Drawing.Point(0, 98);
            this.btnOutputFile.Name = "btnOutputFile";
            this.btnOutputFile.Size = new System.Drawing.Size(87, 22);
            this.btnOutputFile.TabIndex = 19;
            this.btnOutputFile.Text = "Output File...";
            this.btnOutputFile.UseVisualStyleBackColor = true;
            this.btnOutputFile.Click += new System.EventHandler(this.btnOutputFile_Click);
            // 
            // btnAlteredFile
            // 
            this.btnAlteredFile.Location = new System.Drawing.Point(0, 43);
            this.btnAlteredFile.Name = "btnAlteredFile";
            this.btnAlteredFile.Size = new System.Drawing.Size(87, 22);
            this.btnAlteredFile.TabIndex = 15;
            this.btnAlteredFile.Text = "Altered File...";
            this.btnAlteredFile.UseVisualStyleBackColor = true;
            this.btnAlteredFile.Click += new System.EventHandler(this.btnAlteredFile_Click);
            // 
            // btnOriginalFile
            // 
            this.btnOriginalFile.Location = new System.Drawing.Point(0, 18);
            this.btnOriginalFile.Name = "btnOriginalFile";
            this.btnOriginalFile.Size = new System.Drawing.Size(87, 22);
            this.btnOriginalFile.TabIndex = 13;
            this.btnOriginalFile.Text = "Original File...";
            this.btnOriginalFile.UseVisualStyleBackColor = true;
            this.btnOriginalFile.Click += new System.EventHandler(this.btnOriginalFile_Click);
            // 
            // CreateDFRControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.cbCombineAllAppendedData);
            this.Controls.Add(this.cbOpenWhenGenerated);
            this.Controls.Add(this.btnGenerateDFR);
            this.Controls.Add(this.labelSeparator2);
            this.Controls.Add(this.tbOutputFile);
            this.Controls.Add(this.tbAlteredFile);
            this.Controls.Add(this.tbOriginalFile);
            this.Controls.Add(this.labelOutputFile);
            this.Controls.Add(this.labelSelectInputs);
            this.Controls.Add(this.labelSeparator1);
            this.Controls.Add(this.btnOutputFile);
            this.Controls.Add(this.btnAlteredFile);
            this.Controls.Add(this.btnOriginalFile);
            this.Name = "CreateDFRControl";
            this.Size = new System.Drawing.Size(663, 192);
            this.ResumeLayout(false);
            this.PerformLayout();

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
