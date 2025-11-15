using CommonLib.Win.Controls;

namespace SF3.Editor.Forms {
    partial class MovePostEOFDataDialog {
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            labelMoveBy = new DarkModeLabel();
            btnCancel = new DarkModeButton();
            btnMove = new DarkModeButton();
            tbMoveBy = new DarkModeTextBox();
            labelFirstAddrRAM = new DarkModeLabel();
            labelLastAddrRAM = new DarkModeLabel();
            tbFirstAddrRAM = new DarkModeTextBox();
            tbLastAddrRAM = new DarkModeTextBox();
            tbFirstAddrFile = new DarkModeTextBox();
            tbLastAddrFile = new DarkModeTextBox();
            labelLastAddrFile = new DarkModeLabel();
            labelFirstAddrFile = new DarkModeLabel();
            labelFileEndRam = new DarkModeLabel();
            tbFileEndRAM = new DarkModeTextBox();
            tbFreeSpaceBeforePostEOFData = new DarkModeTextBox();
            labelFreeSpace = new DarkModeLabel();
            labelValuesInHex = new DarkModeLabel();
            tbFileEndFile = new DarkModeTextBox();
            labelFileEndFile = new DarkModeLabel();
            tbFileStartRam = new DarkModeTextBox();
            label1 = new DarkModeLabel();
            labelChangeAnyValue = new DarkModeLabel();
            tbLimitRAM = new DarkModeTextBox();
            labelLimit = new DarkModeLabel();
            tbFreeSpaceBeforeLimit = new DarkModeTextBox();
            labelFreeSpaceBeforeLimit = new DarkModeLabel();
            label2 = new DarkModeLabel();
            SuspendLayout();
            // 
            // labelMoveBy
            // 
            labelMoveBy.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelMoveBy.AutoSize = true;
            labelMoveBy.Location = new System.Drawing.Point(169, 35);
            labelMoveBy.Name = "labelMoveBy";
            labelMoveBy.Size = new System.Drawing.Size(207, 15);
            labelMoveBy.TabIndex = 9;
            labelMoveBy.Text = "Move data by (must be divisible by 4):";
            labelMoveBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCancel
            // 
            btnCancel.Anchor =  System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.Location = new System.Drawing.Point(426, 313);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 12;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnMove
            // 
            btnMove.Anchor =  System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnMove.Location = new System.Drawing.Point(345, 313);
            btnMove.Name = "btnMove";
            btnMove.Size = new System.Drawing.Size(75, 23);
            btnMove.TabIndex = 11;
            btnMove.Text = "Move";
            btnMove.UseVisualStyleBackColor = true;
            btnMove.Click += btnMove_Click;
            // 
            // tbMoveBy
            // 
            tbMoveBy.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tbMoveBy.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            tbMoveBy.Location = new System.Drawing.Point(382, 32);
            tbMoveBy.MaxLength = 9;
            tbMoveBy.Name = "tbMoveBy";
            tbMoveBy.Size = new System.Drawing.Size(119, 21);
            tbMoveBy.TabIndex = 0;
            tbMoveBy.Text = "0";
            tbMoveBy.TextChanged += tbMoveBy_TextChanged;
            // 
            // labelFirstAddrRAM
            // 
            labelFirstAddrRAM.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelFirstAddrRAM.AutoSize = true;
            labelFirstAddrRAM.Location = new System.Drawing.Point(12, 77);
            labelFirstAddrRAM.Name = "labelFirstAddrRAM";
            labelFirstAddrRAM.Size = new System.Drawing.Size(190, 15);
            labelFirstAddrRAM.TabIndex = 10;
            labelFirstAddrRAM.Text = "First post-EOF data address (RAM):";
            labelFirstAddrRAM.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelLastAddrRAM
            // 
            labelLastAddrRAM.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelLastAddrRAM.AutoSize = true;
            labelLastAddrRAM.Location = new System.Drawing.Point(13, 104);
            labelLastAddrRAM.Name = "labelLastAddrRAM";
            labelLastAddrRAM.Size = new System.Drawing.Size(189, 15);
            labelLastAddrRAM.TabIndex = 12;
            labelLastAddrRAM.Text = "Last post-EOF data address (RAM):";
            labelLastAddrRAM.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbFirstAddrRAM
            // 
            tbFirstAddrRAM.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tbFirstAddrRAM.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            tbFirstAddrRAM.Location = new System.Drawing.Point(208, 74);
            tbFirstAddrRAM.MaxLength = 9;
            tbFirstAddrRAM.Name = "tbFirstAddrRAM";
            tbFirstAddrRAM.Size = new System.Drawing.Size(119, 21);
            tbFirstAddrRAM.TabIndex = 1;
            tbFirstAddrRAM.Text = "0";
            tbFirstAddrRAM.TextChanged += tbFirstAddrRAM_TextChanged;
            // 
            // tbLastAddrRAM
            // 
            tbLastAddrRAM.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tbLastAddrRAM.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            tbLastAddrRAM.Location = new System.Drawing.Point(208, 101);
            tbLastAddrRAM.MaxLength = 9;
            tbLastAddrRAM.Name = "tbLastAddrRAM";
            tbLastAddrRAM.Size = new System.Drawing.Size(119, 21);
            tbLastAddrRAM.TabIndex = 3;
            tbLastAddrRAM.Text = "0";
            tbLastAddrRAM.TextChanged += tbLastAddrRAM_TextChanged;
            // 
            // tbFirstAddrFile
            // 
            tbFirstAddrFile.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tbFirstAddrFile.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            tbFirstAddrFile.Location = new System.Drawing.Point(382, 74);
            tbFirstAddrFile.MaxLength = 9;
            tbFirstAddrFile.Name = "tbFirstAddrFile";
            tbFirstAddrFile.Size = new System.Drawing.Size(119, 21);
            tbFirstAddrFile.TabIndex = 2;
            tbFirstAddrFile.Text = "0";
            tbFirstAddrFile.TextChanged += tbFirstAddrFile_TextChanged;
            // 
            // tbLastAddrFile
            // 
            tbLastAddrFile.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tbLastAddrFile.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            tbLastAddrFile.Location = new System.Drawing.Point(382, 101);
            tbLastAddrFile.MaxLength = 9;
            tbLastAddrFile.Name = "tbLastAddrFile";
            tbLastAddrFile.Size = new System.Drawing.Size(119, 21);
            tbLastAddrFile.TabIndex = 4;
            tbLastAddrFile.Text = "0";
            tbLastAddrFile.TextChanged += tbLastAddrFile_TextChanged;
            // 
            // labelLastAddrFile
            // 
            labelLastAddrFile.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelLastAddrFile.AutoSize = true;
            labelLastAddrFile.Location = new System.Drawing.Point(340, 104);
            labelLastAddrFile.Name = "labelLastAddrFile";
            labelLastAddrFile.Size = new System.Drawing.Size(36, 15);
            labelLastAddrFile.TabIndex = 13;
            labelLastAddrFile.Text = "(File):";
            labelLastAddrFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelFirstAddrFile
            // 
            labelFirstAddrFile.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelFirstAddrFile.AutoSize = true;
            labelFirstAddrFile.Location = new System.Drawing.Point(340, 77);
            labelFirstAddrFile.Name = "labelFirstAddrFile";
            labelFirstAddrFile.Size = new System.Drawing.Size(36, 15);
            labelFirstAddrFile.TabIndex = 11;
            labelFirstAddrFile.Text = "(File):";
            labelFirstAddrFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelFileEndRam
            // 
            labelFileEndRam.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelFileEndRam.AutoSize = true;
            labelFileEndRam.Location = new System.Drawing.Point(114, 255);
            labelFileEndRam.Name = "labelFileEndRam";
            labelFileEndRam.Size = new System.Drawing.Size(88, 15);
            labelFileEndRam.TabIndex = 14;
            labelFileEndRam.Text = "File end (RAM):";
            labelFileEndRam.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbFileEndRAM
            // 
            tbFileEndRAM.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tbFileEndRAM.BackColor = System.Drawing.SystemColors.Control;
            tbFileEndRAM.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            tbFileEndRAM.Location = new System.Drawing.Point(208, 252);
            tbFileEndRAM.MaxLength = 9;
            tbFileEndRAM.Name = "tbFileEndRAM";
            tbFileEndRAM.ReadOnly = true;
            tbFileEndRAM.Size = new System.Drawing.Size(119, 21);
            tbFileEndRAM.TabIndex = 8;
            tbFileEndRAM.Text = "0";
            // 
            // tbFreeSpaceBeforePostEOFData
            // 
            tbFreeSpaceBeforePostEOFData.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tbFreeSpaceBeforePostEOFData.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            tbFreeSpaceBeforePostEOFData.Location = new System.Drawing.Point(382, 144);
            tbFreeSpaceBeforePostEOFData.MaxLength = 9;
            tbFreeSpaceBeforePostEOFData.Name = "tbFreeSpaceBeforePostEOFData";
            tbFreeSpaceBeforePostEOFData.Size = new System.Drawing.Size(119, 21);
            tbFreeSpaceBeforePostEOFData.TabIndex = 5;
            tbFreeSpaceBeforePostEOFData.Text = "0";
            tbFreeSpaceBeforePostEOFData.TextChanged += tbFreeSpaceBeforePostEOFData_TextChanged;
            // 
            // labelFreeSpace
            // 
            labelFreeSpace.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelFreeSpace.AutoSize = true;
            labelFreeSpace.Location = new System.Drawing.Point(145, 147);
            labelFreeSpace.Name = "labelFreeSpace";
            labelFreeSpace.Size = new System.Drawing.Size(231, 15);
            labelFreeSpace.TabIndex = 15;
            labelFreeSpace.Text = "Free space after EOF before post-EOF data:";
            labelFreeSpace.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelValuesInHex
            // 
            labelValuesInHex.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelValuesInHex.AutoSize = true;
            labelValuesInHex.Location = new System.Drawing.Point(382, 9);
            labelValuesInHex.Name = "labelValuesInHex";
            labelValuesInHex.Size = new System.Drawing.Size(82, 15);
            labelValuesInHex.TabIndex = 16;
            labelValuesInHex.Text = "(Values in hex)";
            // 
            // tbFileEndFile
            // 
            tbFileEndFile.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tbFileEndFile.BackColor = System.Drawing.SystemColors.Control;
            tbFileEndFile.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            tbFileEndFile.Location = new System.Drawing.Point(382, 252);
            tbFileEndFile.MaxLength = 9;
            tbFileEndFile.Name = "tbFileEndFile";
            tbFileEndFile.ReadOnly = true;
            tbFileEndFile.Size = new System.Drawing.Size(119, 21);
            tbFileEndFile.TabIndex = 9;
            tbFileEndFile.Text = "0";
            // 
            // labelFileEndFile
            // 
            labelFileEndFile.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelFileEndFile.AutoSize = true;
            labelFileEndFile.Location = new System.Drawing.Point(340, 255);
            labelFileEndFile.Name = "labelFileEndFile";
            labelFileEndFile.Size = new System.Drawing.Size(36, 15);
            labelFileEndFile.TabIndex = 18;
            labelFileEndFile.Text = "(File):";
            labelFileEndFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbFileStartRam
            // 
            tbFileStartRam.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tbFileStartRam.BackColor = System.Drawing.SystemColors.Control;
            tbFileStartRam.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            tbFileStartRam.Location = new System.Drawing.Point(208, 225);
            tbFileStartRam.MaxLength = 9;
            tbFileStartRam.Name = "tbFileStartRam";
            tbFileStartRam.ReadOnly = true;
            tbFileStartRam.Size = new System.Drawing.Size(119, 21);
            tbFileStartRam.TabIndex = 7;
            tbFileStartRam.Text = "0";
            // 
            // label1
            // 
            label1.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(111, 228);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(91, 15);
            label1.TabIndex = 20;
            label1.Text = "File start (RAM):";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelChangeAnyValue
            // 
            labelChangeAnyValue.AutoSize = true;
            labelChangeAnyValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point,  0);
            labelChangeAnyValue.Location = new System.Drawing.Point(12, 9);
            labelChangeAnyValue.Name = "labelChangeAnyValue";
            labelChangeAnyValue.Size = new System.Drawing.Size(180, 15);
            labelChangeAnyValue.TabIndex = 21;
            labelChangeAnyValue.Text = "(Changing one field updates all)";
            // 
            // tbLimitRAM
            // 
            tbLimitRAM.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tbLimitRAM.BackColor = System.Drawing.SystemColors.Control;
            tbLimitRAM.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            tbLimitRAM.Location = new System.Drawing.Point(208, 279);
            tbLimitRAM.MaxLength = 9;
            tbLimitRAM.Name = "tbLimitRAM";
            tbLimitRAM.ReadOnly = true;
            tbLimitRAM.Size = new System.Drawing.Size(119, 21);
            tbLimitRAM.TabIndex = 10;
            tbLimitRAM.Text = "0";
            // 
            // labelLimit
            // 
            labelLimit.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelLimit.AutoSize = true;
            labelLimit.Location = new System.Drawing.Point(36, 282);
            labelLimit.Name = "labelLimit";
            labelLimit.Size = new System.Drawing.Size(166, 15);
            labelLimit.TabIndex = 23;
            labelLimit.Text = "Limit / start of next file (RAM):";
            labelLimit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbFreeSpaceBeforeLimit
            // 
            tbFreeSpaceBeforeLimit.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tbFreeSpaceBeforeLimit.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            tbFreeSpaceBeforeLimit.Location = new System.Drawing.Point(382, 171);
            tbFreeSpaceBeforeLimit.MaxLength = 9;
            tbFreeSpaceBeforeLimit.Name = "tbFreeSpaceBeforeLimit";
            tbFreeSpaceBeforeLimit.Size = new System.Drawing.Size(119, 21);
            tbFreeSpaceBeforeLimit.TabIndex = 6;
            tbFreeSpaceBeforeLimit.Text = "0";
            tbFreeSpaceBeforeLimit.TextChanged += tbFreeSpaceBeforeLimit_TextChanged;
            // 
            // labelFreeSpaceBeforeLimit
            // 
            labelFreeSpaceBeforeLimit.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelFreeSpaceBeforeLimit.AutoSize = true;
            labelFreeSpaceBeforeLimit.Location = new System.Drawing.Point(24, 174);
            labelFreeSpaceBeforeLimit.Name = "labelFreeSpaceBeforeLimit";
            labelFreeSpaceBeforeLimit.Size = new System.Drawing.Size(352, 15);
            labelFreeSpaceBeforeLimit.TabIndex = 25;
            labelFreeSpaceBeforeLimit.Text = "Free space after last post-EOF data before limit / start of next file*:";
            labelFreeSpaceBeforeLimit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            label2.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label2.AutoSize = true;
            label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            label2.Location = new System.Drawing.Point(86, 195);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(378, 15);
            label2.TabIndex = 26;
            label2.Text = "(* doesn't account for length of last post-EOF data, which is unknown)";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MovePostEOFDataDialog
            // 
            AcceptButton = btnMove;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(513, 348);
            Controls.Add(label2);
            Controls.Add(labelFreeSpaceBeforeLimit);
            Controls.Add(tbFreeSpaceBeforeLimit);
            Controls.Add(labelLimit);
            Controls.Add(tbLimitRAM);
            Controls.Add(labelChangeAnyValue);
            Controls.Add(tbFileStartRam);
            Controls.Add(label1);
            Controls.Add(tbFileEndFile);
            Controls.Add(labelFileEndFile);
            Controls.Add(labelValuesInHex);
            Controls.Add(labelFreeSpace);
            Controls.Add(tbFreeSpaceBeforePostEOFData);
            Controls.Add(tbFileEndRAM);
            Controls.Add(labelFileEndRam);
            Controls.Add(labelFirstAddrFile);
            Controls.Add(labelLastAddrFile);
            Controls.Add(tbLastAddrFile);
            Controls.Add(tbFirstAddrFile);
            Controls.Add(tbLastAddrRAM);
            Controls.Add(tbFirstAddrRAM);
            Controls.Add(labelLastAddrRAM);
            Controls.Add(labelFirstAddrRAM);
            Controls.Add(tbMoveBy);
            Controls.Add(btnMove);
            Controls.Add(btnCancel);
            Controls.Add(labelMoveBy);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MovePostEOFDataDialog";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Move Post-EOF Data";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DarkModeLabel labelMoveBy;
        private DarkModeButton btnCancel;
        private DarkModeButton btnMove;
        private DarkModeTextBox tbMoveBy;
        private DarkModeLabel labelFirstAddrRAM;
        private DarkModeLabel labelLastAddrRAM;
        private DarkModeTextBox tbFirstAddrRAM;
        private DarkModeTextBox tbLastAddrRAM;
        private DarkModeTextBox tbFirstAddrFile;
        private DarkModeTextBox tbLastAddrFile;
        private DarkModeLabel labelLastAddrFile;
        private DarkModeLabel labelFirstAddrFile;
        private DarkModeLabel labelFileEndRam;
        private DarkModeTextBox tbFileEndRAM;
        private DarkModeTextBox tbFreeSpaceBeforePostEOFData;
        private DarkModeLabel labelFreeSpace;
        private DarkModeLabel labelValuesInHex;
        private DarkModeTextBox tbFileEndFile;
        private DarkModeLabel labelFileEndFile;
        private DarkModeTextBox tbFileStartRam;
        private DarkModeLabel label1;
        private DarkModeLabel labelChangeAnyValue;
        private DarkModeTextBox tbLimitRAM;
        private DarkModeLabel labelLimit;
        private DarkModeTextBox tbFreeSpaceBeforeLimit;
        private DarkModeLabel labelFreeSpaceBeforeLimit;
        private DarkModeLabel label2;
    }
}