namespace DFRLib.Win.Controls {
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
            cbApplyToInputFile = new System.Windows.Forms.CheckBox();
            btnApplyDFR = new System.Windows.Forms.Button();
            labelSeparator2 = new System.Windows.Forms.Label();
            tbOutputFile = new System.Windows.Forms.TextBox();
            tbDFRFile = new System.Windows.Forms.TextBox();
            labelSelectOutputDestination = new System.Windows.Forms.Label();
            labelSeparator1 = new System.Windows.Forms.Label();
            btnOutputFile = new System.Windows.Forms.Button();
            btnDFRFile = new System.Windows.Forms.Button();
            tbInputFile = new System.Windows.Forms.TextBox();
            btnInputFile = new System.Windows.Forms.Button();
            labelSelectInputFiles = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // cbApplyToInputFile
            // 
            cbApplyToInputFile.AutoSize = true;
            cbApplyToInputFile.Location = new System.Drawing.Point(4, 117);
            cbApplyToInputFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cbApplyToInputFile.Name = "cbApplyToInputFile";
            cbApplyToInputFile.Size = new System.Drawing.Size(147, 19);
            cbApplyToInputFile.TabIndex = 19;
            cbApplyToInputFile.Text = "Apply DFR to Input File";
            cbApplyToInputFile.UseVisualStyleBackColor = true;
            cbApplyToInputFile.CheckedChanged += cbApplyToInputFile_CheckedChanged;
            // 
            // btnApplyDFR
            // 
            btnApplyDFR.Anchor =  System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnApplyDFR.Location = new System.Drawing.Point(672, 193);
            btnApplyDFR.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnApplyDFR.Name = "btnApplyDFR";
            btnApplyDFR.Size = new System.Drawing.Size(102, 29);
            btnApplyDFR.TabIndex = 23;
            btnApplyDFR.Text = "Apply DFR";
            btnApplyDFR.UseVisualStyleBackColor = true;
            btnApplyDFR.Click += btnApplyDFR_Click;
            // 
            // labelSeparator2
            // 
            labelSeparator2.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            labelSeparator2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            labelSeparator2.Location = new System.Drawing.Point(0, 177);
            labelSeparator2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelSeparator2.Name = "labelSeparator2";
            labelSeparator2.Size = new System.Drawing.Size(774, 2);
            labelSeparator2.TabIndex = 22;
            // 
            // tbOutputFile
            // 
            tbOutputFile.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tbOutputFile.BackColor = System.Drawing.SystemColors.Window;
            tbOutputFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tbOutputFile.Enabled = false;
            tbOutputFile.Location = new System.Drawing.Point(108, 143);
            tbOutputFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tbOutputFile.Name = "tbOutputFile";
            tbOutputFile.Size = new System.Drawing.Size(665, 23);
            tbOutputFile.TabIndex = 21;
            // 
            // tbDFRFile
            // 
            tbDFRFile.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tbDFRFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tbDFRFile.Location = new System.Drawing.Point(108, 51);
            tbDFRFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tbDFRFile.Name = "tbDFRFile";
            tbDFRFile.Size = new System.Drawing.Size(665, 23);
            tbDFRFile.TabIndex = 16;
            // 
            // labelSelectOutputDestination
            // 
            labelSelectOutputDestination.AutoSize = true;
            labelSelectOutputDestination.Location = new System.Drawing.Point(0, 93);
            labelSelectOutputDestination.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelSelectOutputDestination.Name = "labelSelectOutputDestination";
            labelSelectOutputDestination.Size = new System.Drawing.Size(142, 15);
            labelSelectOutputDestination.TabIndex = 18;
            labelSelectOutputDestination.Text = "Select Output Destination";
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
            btnOutputFile.Enabled = false;
            btnOutputFile.Location = new System.Drawing.Point(0, 142);
            btnOutputFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnOutputFile.Name = "btnOutputFile";
            btnOutputFile.Size = new System.Drawing.Size(102, 25);
            btnOutputFile.TabIndex = 20;
            btnOutputFile.Text = "Output File...";
            btnOutputFile.UseVisualStyleBackColor = true;
            btnOutputFile.Click += btnOutputFile_Click;
            // 
            // btnDFRFile
            // 
            btnDFRFile.Location = new System.Drawing.Point(0, 50);
            btnDFRFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnDFRFile.Name = "btnDFRFile";
            btnDFRFile.Size = new System.Drawing.Size(102, 25);
            btnDFRFile.TabIndex = 15;
            btnDFRFile.Text = "DFR File...";
            btnDFRFile.UseVisualStyleBackColor = true;
            btnDFRFile.Click += btnDFRFile_Click;
            // 
            // tbInputFile
            // 
            tbInputFile.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tbInputFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tbInputFile.Location = new System.Drawing.Point(108, 22);
            tbInputFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tbInputFile.Name = "tbInputFile";
            tbInputFile.Size = new System.Drawing.Size(665, 23);
            tbInputFile.TabIndex = 14;
            // 
            // btnInputFile
            // 
            btnInputFile.Location = new System.Drawing.Point(0, 21);
            btnInputFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnInputFile.Name = "btnInputFile";
            btnInputFile.Size = new System.Drawing.Size(102, 25);
            btnInputFile.TabIndex = 13;
            btnInputFile.Text = "Input File...";
            btnInputFile.UseVisualStyleBackColor = true;
            btnInputFile.Click += btnInputFile_Click;
            // 
            // labelSelectInputFiles
            // 
            labelSelectInputFiles.AutoSize = true;
            labelSelectInputFiles.Location = new System.Drawing.Point(0, 0);
            labelSelectInputFiles.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelSelectInputFiles.Name = "labelSelectInputFiles";
            labelSelectInputFiles.Size = new System.Drawing.Size(95, 15);
            labelSelectInputFiles.TabIndex = 12;
            labelSelectInputFiles.Text = "Select Input Files";
            // 
            // ApplyDFRControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.Transparent;
            Controls.Add(cbApplyToInputFile);
            Controls.Add(btnApplyDFR);
            Controls.Add(labelSeparator2);
            Controls.Add(tbOutputFile);
            Controls.Add(tbDFRFile);
            Controls.Add(labelSelectOutputDestination);
            Controls.Add(labelSeparator1);
            Controls.Add(btnOutputFile);
            Controls.Add(btnDFRFile);
            Controls.Add(tbInputFile);
            Controls.Add(btnInputFile);
            Controls.Add(labelSelectInputFiles);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "ApplyDFRControl";
            Size = new System.Drawing.Size(774, 222);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.CheckBox cbApplyToInputFile;
        private System.Windows.Forms.Button btnApplyDFR;
        private System.Windows.Forms.Label labelSeparator2;
        private System.Windows.Forms.TextBox tbOutputFile;
        private System.Windows.Forms.TextBox tbDFRFile;
        private System.Windows.Forms.Label labelSelectOutputDestination;
        private System.Windows.Forms.Label labelSeparator1;
        private System.Windows.Forms.Button btnOutputFile;
        private System.Windows.Forms.Button btnDFRFile;
        private System.Windows.Forms.TextBox tbInputFile;
        private System.Windows.Forms.Button btnInputFile;
        private System.Windows.Forms.Label labelSelectInputFiles;
    }
}
