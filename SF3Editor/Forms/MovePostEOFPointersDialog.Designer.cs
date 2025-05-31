namespace SF3.Editor.Forms {
    partial class MovePostEOFPointersDialog {
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
            labelMoveBy = new System.Windows.Forms.Label();
            btnCancel = new System.Windows.Forms.Button();
            btnMove = new System.Windows.Forms.Button();
            tbMoveByHex = new System.Windows.Forms.TextBox();
            labelFirstAddrRam = new System.Windows.Forms.Label();
            labelLastAddrRam = new System.Windows.Forms.Label();
            tbFirstAddrRam = new System.Windows.Forms.TextBox();
            tbLastAddrRam = new System.Windows.Forms.TextBox();
            tbFirstAddrFile = new System.Windows.Forms.TextBox();
            tbLastAddrFile = new System.Windows.Forms.TextBox();
            labelLastAddrFile = new System.Windows.Forms.Label();
            labelFirstAddrFile = new System.Windows.Forms.Label();
            labelFileEndRam = new System.Windows.Forms.Label();
            tbFileEndRam = new System.Windows.Forms.TextBox();
            tbFreeSpace = new System.Windows.Forms.TextBox();
            labelFreeSpace = new System.Windows.Forms.Label();
            labelValuesInHex = new System.Windows.Forms.Label();
            tbFileEndFile = new System.Windows.Forms.TextBox();
            labelFileEndFile = new System.Windows.Forms.Label();
            tbFileStartRam = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            labelChangeAnyValue = new System.Windows.Forms.Label();
            tbX1Limit = new System.Windows.Forms.TextBox();
            labelX1Limit = new System.Windows.Forms.Label();
            tbFreeSpaceBeforeLimit = new System.Windows.Forms.TextBox();
            labelFreeSpaceBeforeLimit = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // labelMoveBy
            // 
            labelMoveBy.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelMoveBy.AutoSize = true;
            labelMoveBy.Location = new System.Drawing.Point(108, 39);
            labelMoveBy.Name = "labelMoveBy";
            labelMoveBy.Size = new System.Drawing.Size(102, 15);
            labelMoveBy.TabIndex = 9;
            labelMoveBy.Text = "Move pointers by:";
            labelMoveBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCancel
            // 
            btnCancel.Anchor =  System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.Location = new System.Drawing.Point(260, 453);
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
            btnMove.Location = new System.Drawing.Point(179, 453);
            btnMove.Name = "btnMove";
            btnMove.Size = new System.Drawing.Size(75, 23);
            btnMove.TabIndex = 11;
            btnMove.Text = "Move";
            btnMove.UseVisualStyleBackColor = true;
            btnMove.Click += btnMove_Click;
            // 
            // tbMoveByHex
            // 
            tbMoveByHex.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tbMoveByHex.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            tbMoveByHex.Location = new System.Drawing.Point(216, 36);
            tbMoveByHex.MaxLength = 9;
            tbMoveByHex.Name = "tbMoveByHex";
            tbMoveByHex.Size = new System.Drawing.Size(119, 21);
            tbMoveByHex.TabIndex = 0;
            tbMoveByHex.Text = "0";
            tbMoveByHex.TextChanged += tbMoveByHex_TextChanged;
            // 
            // labelFirstAddrRam
            // 
            labelFirstAddrRam.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelFirstAddrRam.AutoSize = true;
            labelFirstAddrRam.Location = new System.Drawing.Point(20, 81);
            labelFirstAddrRam.Name = "labelFirstAddrRam";
            labelFirstAddrRam.Size = new System.Drawing.Size(190, 15);
            labelFirstAddrRam.TabIndex = 10;
            labelFirstAddrRam.Text = "First post-EOF data address (RAM):";
            labelFirstAddrRam.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelLastAddrRam
            // 
            labelLastAddrRam.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelLastAddrRam.AutoSize = true;
            labelLastAddrRam.Location = new System.Drawing.Point(18, 151);
            labelLastAddrRam.Name = "labelLastAddrRam";
            labelLastAddrRam.Size = new System.Drawing.Size(189, 15);
            labelLastAddrRam.TabIndex = 12;
            labelLastAddrRam.Text = "Last post-EOF data address (RAM):";
            labelLastAddrRam.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbFirstAddrRam
            // 
            tbFirstAddrRam.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tbFirstAddrRam.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            tbFirstAddrRam.Location = new System.Drawing.Point(216, 78);
            tbFirstAddrRam.MaxLength = 9;
            tbFirstAddrRam.Name = "tbFirstAddrRam";
            tbFirstAddrRam.Size = new System.Drawing.Size(119, 21);
            tbFirstAddrRam.TabIndex = 1;
            tbFirstAddrRam.Text = "0";
            tbFirstAddrRam.TextChanged += tbFirstAddrRam_TextChanged;
            // 
            // tbLastAddrRam
            // 
            tbLastAddrRam.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tbLastAddrRam.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            tbLastAddrRam.Location = new System.Drawing.Point(216, 148);
            tbLastAddrRam.MaxLength = 9;
            tbLastAddrRam.Name = "tbLastAddrRam";
            tbLastAddrRam.Size = new System.Drawing.Size(119, 21);
            tbLastAddrRam.TabIndex = 3;
            tbLastAddrRam.Text = "0";
            tbLastAddrRam.TextChanged += tbLastAddrRam_TextChanged;
            // 
            // tbFirstAddrFile
            // 
            tbFirstAddrFile.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tbFirstAddrFile.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            tbFirstAddrFile.Location = new System.Drawing.Point(216, 105);
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
            tbLastAddrFile.Location = new System.Drawing.Point(216, 175);
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
            labelLastAddrFile.Location = new System.Drawing.Point(174, 178);
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
            labelFirstAddrFile.Location = new System.Drawing.Point(174, 108);
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
            labelFileEndRam.Location = new System.Drawing.Point(122, 247);
            labelFileEndRam.Name = "labelFileEndRam";
            labelFileEndRam.Size = new System.Drawing.Size(88, 15);
            labelFileEndRam.TabIndex = 14;
            labelFileEndRam.Text = "File end (RAM):";
            labelFileEndRam.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbFileEndRam
            // 
            tbFileEndRam.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tbFileEndRam.BackColor = System.Drawing.SystemColors.Window;
            tbFileEndRam.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            tbFileEndRam.Location = new System.Drawing.Point(216, 244);
            tbFileEndRam.MaxLength = 9;
            tbFileEndRam.Name = "tbFileEndRam";
            tbFileEndRam.Size = new System.Drawing.Size(119, 21);
            tbFileEndRam.TabIndex = 6;
            tbFileEndRam.Text = "0";
            tbFileEndRam.TextChanged += tbFileEndRam_TextChanged;
            // 
            // tbFreeSpace
            // 
            tbFreeSpace.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tbFreeSpace.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            tbFreeSpace.Location = new System.Drawing.Point(216, 310);
            tbFreeSpace.MaxLength = 9;
            tbFreeSpace.Name = "tbFreeSpace";
            tbFreeSpace.Size = new System.Drawing.Size(119, 21);
            tbFreeSpace.TabIndex = 8;
            tbFreeSpace.Text = "0";
            tbFreeSpace.TextChanged += tbFreeSpace_TextChanged;
            // 
            // labelFreeSpace
            // 
            labelFreeSpace.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelFreeSpace.AutoSize = true;
            labelFreeSpace.Location = new System.Drawing.Point(85, 305);
            labelFreeSpace.Name = "labelFreeSpace";
            labelFreeSpace.Size = new System.Drawing.Size(122, 30);
            labelFreeSpace.TabIndex = 15;
            labelFreeSpace.Text = "Free space after EOF\r\nbefore post-EOF data:";
            labelFreeSpace.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelValuesInHex
            // 
            labelValuesInHex.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelValuesInHex.AutoSize = true;
            labelValuesInHex.Location = new System.Drawing.Point(216, 9);
            labelValuesInHex.Name = "labelValuesInHex";
            labelValuesInHex.Size = new System.Drawing.Size(82, 15);
            labelValuesInHex.TabIndex = 16;
            labelValuesInHex.Text = "(Values in hex)";
            // 
            // tbFileEndFile
            // 
            tbFileEndFile.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tbFileEndFile.BackColor = System.Drawing.SystemColors.Window;
            tbFileEndFile.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            tbFileEndFile.Location = new System.Drawing.Point(216, 271);
            tbFileEndFile.MaxLength = 9;
            tbFileEndFile.Name = "tbFileEndFile";
            tbFileEndFile.Size = new System.Drawing.Size(119, 21);
            tbFileEndFile.TabIndex = 7;
            tbFileEndFile.Text = "0";
            tbFileEndFile.TextChanged += tbFileEndFile_TextChanged;
            // 
            // labelFileEndFile
            // 
            labelFileEndFile.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelFileEndFile.AutoSize = true;
            labelFileEndFile.Location = new System.Drawing.Point(174, 274);
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
            tbFileStartRam.Enabled = false;
            tbFileStartRam.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            tbFileStartRam.Location = new System.Drawing.Point(216, 217);
            tbFileStartRam.MaxLength = 9;
            tbFileStartRam.Name = "tbFileStartRam";
            tbFileStartRam.Size = new System.Drawing.Size(119, 21);
            tbFileStartRam.TabIndex = 5;
            tbFileStartRam.Text = "0";
            // 
            // label1
            // 
            label1.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(119, 220);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(91, 15);
            label1.TabIndex = 20;
            label1.Text = "File start (RAM):";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelChangeAnyValue
            // 
            labelChangeAnyValue.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelChangeAnyValue.AutoSize = true;
            labelChangeAnyValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point,  0);
            labelChangeAnyValue.Location = new System.Drawing.Point(12, 9);
            labelChangeAnyValue.Name = "labelChangeAnyValue";
            labelChangeAnyValue.Size = new System.Drawing.Size(180, 15);
            labelChangeAnyValue.TabIndex = 21;
            labelChangeAnyValue.Text = "(Changing one field updates all)";
            // 
            // tbX1Limit
            // 
            tbX1Limit.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tbX1Limit.BackColor = System.Drawing.SystemColors.Control;
            tbX1Limit.Enabled = false;
            tbX1Limit.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            tbX1Limit.Location = new System.Drawing.Point(216, 421);
            tbX1Limit.MaxLength = 9;
            tbX1Limit.Name = "tbX1Limit";
            tbX1Limit.Size = new System.Drawing.Size(119, 21);
            tbX1Limit.TabIndex = 10;
            tbX1Limit.Text = "0";
            // 
            // labelX1Limit
            // 
            labelX1Limit.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelX1Limit.AutoSize = true;
            labelX1Limit.Location = new System.Drawing.Point(28, 424);
            labelX1Limit.Name = "labelX1Limit";
            labelX1Limit.Size = new System.Drawing.Size(179, 15);
            labelX1Limit.TabIndex = 23;
            labelX1Limit.Text = "X1 limit / start of next file (RAM):";
            labelX1Limit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbFreeSpaceBeforeLimit
            // 
            tbFreeSpaceBeforeLimit.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tbFreeSpaceBeforeLimit.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            tbFreeSpaceBeforeLimit.Location = new System.Drawing.Point(216, 349);
            tbFreeSpaceBeforeLimit.MaxLength = 9;
            tbFreeSpaceBeforeLimit.Name = "tbFreeSpaceBeforeLimit";
            tbFreeSpaceBeforeLimit.Size = new System.Drawing.Size(119, 21);
            tbFreeSpaceBeforeLimit.TabIndex = 9;
            tbFreeSpaceBeforeLimit.Text = "0";
            tbFreeSpaceBeforeLimit.TextChanged += tbFreeSpaceBeforeLimit_TextChanged;
            // 
            // labelFreeSpaceBeforeLimit
            // 
            labelFreeSpaceBeforeLimit.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelFreeSpaceBeforeLimit.AutoSize = true;
            labelFreeSpaceBeforeLimit.Location = new System.Drawing.Point(22, 344);
            labelFreeSpaceBeforeLimit.Name = "labelFreeSpaceBeforeLimit";
            labelFreeSpaceBeforeLimit.Size = new System.Drawing.Size(188, 30);
            labelFreeSpaceBeforeLimit.TabIndex = 25;
            labelFreeSpaceBeforeLimit.Text = "Free space after last post-EOF data\r\nbefore X1 limit / start of next file:*";
            labelFreeSpaceBeforeLimit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            label2.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label2.AutoSize = true;
            label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            label2.Location = new System.Drawing.Point(12, 382);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(275, 30);
            label2.TabIndex = 26;
            label2.Text = "(* doesn't account for length of last post-EOF data,\r\n  which is unknown)";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MovePostEOFPointersDialog
            // 
            AcceptButton = btnMove;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(347, 488);
            Controls.Add(label2);
            Controls.Add(labelFreeSpaceBeforeLimit);
            Controls.Add(tbFreeSpaceBeforeLimit);
            Controls.Add(labelX1Limit);
            Controls.Add(tbX1Limit);
            Controls.Add(labelChangeAnyValue);
            Controls.Add(tbFileStartRam);
            Controls.Add(label1);
            Controls.Add(tbFileEndFile);
            Controls.Add(labelFileEndFile);
            Controls.Add(labelValuesInHex);
            Controls.Add(labelFreeSpace);
            Controls.Add(tbFreeSpace);
            Controls.Add(tbFileEndRam);
            Controls.Add(labelFileEndRam);
            Controls.Add(labelFirstAddrFile);
            Controls.Add(labelLastAddrFile);
            Controls.Add(tbLastAddrFile);
            Controls.Add(tbFirstAddrFile);
            Controls.Add(tbLastAddrRam);
            Controls.Add(tbFirstAddrRam);
            Controls.Add(labelLastAddrRam);
            Controls.Add(labelFirstAddrRam);
            Controls.Add(tbMoveByHex);
            Controls.Add(btnMove);
            Controls.Add(btnCancel);
            Controls.Add(labelMoveBy);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MovePostEOFPointersDialog";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Move Post-EOF X1 Pointers";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label labelMoveBy;
        private System.Windows.Forms.Label labelRamAddress;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnMove;
        private System.Windows.Forms.TextBox tbMoveByHex;
        private System.Windows.Forms.Label labelFirstAddrRam;
        private System.Windows.Forms.Label labelLastAddrRam;
        private System.Windows.Forms.TextBox tbFirstAddrRam;
        private System.Windows.Forms.TextBox tbLastAddrRam;
        private System.Windows.Forms.TextBox tbFirstAddrFile;
        private System.Windows.Forms.TextBox tbLastAddrFile;
        private System.Windows.Forms.Label labelLastAddrFile;
        private System.Windows.Forms.Label labelFirstAddrFile;
        private System.Windows.Forms.Label labelFileEndRam;
        private System.Windows.Forms.TextBox tbFileEndRam;
        private System.Windows.Forms.TextBox tbFreeSpace;
        private System.Windows.Forms.Label labelFreeSpace;
        private System.Windows.Forms.Label labelValuesInHex;
        private System.Windows.Forms.TextBox tbFileEndFile;
        private System.Windows.Forms.Label labelFileEndFile;
        private System.Windows.Forms.TextBox tbFileStartRam;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelChangeAnyValue;
        private System.Windows.Forms.TextBox tbX1Limit;
        private System.Windows.Forms.Label labelX1Limit;
        private System.Windows.Forms.TextBox tbFreeSpaceBeforeLimit;
        private System.Windows.Forms.Label labelFreeSpaceBeforeLimit;
        private System.Windows.Forms.Label label2;
    }
}