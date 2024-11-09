namespace DFRTool.GUI.Controls {
    partial class ApplyDFRControl {
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
            this.cbApplyToOriginalFile = new System.Windows.Forms.CheckBox();
            this.btnApplyDFR = new System.Windows.Forms.Button();
            this.labelSeparator2 = new System.Windows.Forms.Label();
            this.tbOutputFile = new System.Windows.Forms.TextBox();
            this.tbDFRFile = new System.Windows.Forms.TextBox();
            this.labelSelectOutputDestination = new System.Windows.Forms.Label();
            this.labelSeparator1 = new System.Windows.Forms.Label();
            this.btnOutputFile = new System.Windows.Forms.Button();
            this.btnDFRFile = new System.Windows.Forms.Button();
            this.tbOriginalFile = new System.Windows.Forms.TextBox();
            this.btnOriginalFile = new System.Windows.Forms.Button();
            this.labelSelectInputFiles = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbApplyToOriginalFile
            // 
            this.cbApplyToOriginalFile.AutoSize = true;
            this.cbApplyToOriginalFile.Checked = true;
            this.cbApplyToOriginalFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbApplyToOriginalFile.Location = new System.Drawing.Point(3, 101);
            this.cbApplyToOriginalFile.Name = "cbApplyToOriginalFile";
            this.cbApplyToOriginalFile.Size = new System.Drawing.Size(146, 17);
            this.cbApplyToOriginalFile.TabIndex = 19;
            this.cbApplyToOriginalFile.Text = "Apply DFR to Original File";
            this.cbApplyToOriginalFile.UseVisualStyleBackColor = true;
            this.cbApplyToOriginalFile.CheckedChanged += new System.EventHandler(this.cbApplyToOriginalFile_CheckedChanged);
            // 
            // btnApplyDFR
            // 
            this.btnApplyDFR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApplyDFR.Location = new System.Drawing.Point(576, 167);
            this.btnApplyDFR.Name = "btnApplyDFR";
            this.btnApplyDFR.Size = new System.Drawing.Size(87, 25);
            this.btnApplyDFR.TabIndex = 23;
            this.btnApplyDFR.Text = "Apply DFR";
            this.btnApplyDFR.UseVisualStyleBackColor = true;
            this.btnApplyDFR.Click += new System.EventHandler(this.btnApplyDFR_Click);
            // 
            // labelSeparator2
            // 
            this.labelSeparator2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSeparator2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelSeparator2.Location = new System.Drawing.Point(0, 153);
            this.labelSeparator2.Name = "labelSeparator2";
            this.labelSeparator2.Size = new System.Drawing.Size(663, 2);
            this.labelSeparator2.TabIndex = 22;
            // 
            // tbOutputFile
            // 
            this.tbOutputFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOutputFile.BackColor = System.Drawing.SystemColors.Window;
            this.tbOutputFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbOutputFile.Enabled = false;
            this.tbOutputFile.Location = new System.Drawing.Point(93, 124);
            this.tbOutputFile.Name = "tbOutputFile";
            this.tbOutputFile.Size = new System.Drawing.Size(570, 20);
            this.tbOutputFile.TabIndex = 21;
            this.tbOutputFile.Text = "(Same as Original File)";
            // 
            // tbDFRFile
            // 
            this.tbDFRFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDFRFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbDFRFile.Location = new System.Drawing.Point(93, 44);
            this.tbDFRFile.Name = "tbDFRFile";
            this.tbDFRFile.Size = new System.Drawing.Size(570, 20);
            this.tbDFRFile.TabIndex = 16;
            // 
            // labelSelectOutputDestination
            // 
            this.labelSelectOutputDestination.AutoSize = true;
            this.labelSelectOutputDestination.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSelectOutputDestination.Location = new System.Drawing.Point(0, 81);
            this.labelSelectOutputDestination.Name = "labelSelectOutputDestination";
            this.labelSelectOutputDestination.Size = new System.Drawing.Size(153, 13);
            this.labelSelectOutputDestination.TabIndex = 18;
            this.labelSelectOutputDestination.Text = "Select Output Destination";
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
            this.btnOutputFile.Enabled = false;
            this.btnOutputFile.Location = new System.Drawing.Point(0, 123);
            this.btnOutputFile.Name = "btnOutputFile";
            this.btnOutputFile.Size = new System.Drawing.Size(87, 22);
            this.btnOutputFile.TabIndex = 20;
            this.btnOutputFile.Text = "Output File...";
            this.btnOutputFile.UseVisualStyleBackColor = true;
            this.btnOutputFile.Click += new System.EventHandler(this.btnOutputFile_Click);
            // 
            // btnDFRFile
            // 
            this.btnDFRFile.Location = new System.Drawing.Point(0, 43);
            this.btnDFRFile.Name = "btnDFRFile";
            this.btnDFRFile.Size = new System.Drawing.Size(87, 22);
            this.btnDFRFile.TabIndex = 15;
            this.btnDFRFile.Text = "DFR File...";
            this.btnDFRFile.UseVisualStyleBackColor = true;
            this.btnDFRFile.Click += new System.EventHandler(this.btnDFRFile_Click);
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
            // labelSelectInputFiles
            // 
            this.labelSelectInputFiles.AutoSize = true;
            this.labelSelectInputFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSelectInputFiles.Location = new System.Drawing.Point(0, 0);
            this.labelSelectInputFiles.Name = "labelSelectInputFiles";
            this.labelSelectInputFiles.Size = new System.Drawing.Size(106, 13);
            this.labelSelectInputFiles.TabIndex = 12;
            this.labelSelectInputFiles.Text = "Select Input Files";
            // 
            // ApplyDFRControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.cbApplyToOriginalFile);
            this.Controls.Add(this.btnApplyDFR);
            this.Controls.Add(this.labelSeparator2);
            this.Controls.Add(this.tbOutputFile);
            this.Controls.Add(this.tbDFRFile);
            this.Controls.Add(this.labelSelectOutputDestination);
            this.Controls.Add(this.labelSeparator1);
            this.Controls.Add(this.btnOutputFile);
            this.Controls.Add(this.btnDFRFile);
            this.Controls.Add(this.tbOriginalFile);
            this.Controls.Add(this.btnOriginalFile);
            this.Controls.Add(this.labelSelectInputFiles);
            this.Name = "ApplyDFRControl";
            this.Size = new System.Drawing.Size(663, 192);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbApplyToOriginalFile;
        private System.Windows.Forms.Button btnApplyDFR;
        private System.Windows.Forms.Label labelSeparator2;
        private System.Windows.Forms.TextBox tbOutputFile;
        private System.Windows.Forms.TextBox tbDFRFile;
        private System.Windows.Forms.Label labelSelectOutputDestination;
        private System.Windows.Forms.Label labelSeparator1;
        private System.Windows.Forms.Button btnOutputFile;
        private System.Windows.Forms.Button btnDFRFile;
        private System.Windows.Forms.TextBox tbOriginalFile;
        private System.Windows.Forms.Button btnOriginalFile;
        private System.Windows.Forms.Label labelSelectInputFiles;
    }
}
