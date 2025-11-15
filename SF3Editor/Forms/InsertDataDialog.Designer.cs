using CommonLib.Win.Controls;

namespace SF3.Editor.Forms {
    partial class InsertDataDialog {
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
            labelInsertAddressRAM = new DarkModeLabel();
            btnCancel = new DarkModeButton();
            btnInsert = new DarkModeButton();
            tbInsertAddressRAM = new DarkModeTextBox();
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
            tbLimitRAM = new DarkModeTextBox();
            labelLimit = new DarkModeLabel();
            tbFreeSpaceBeforeLimit = new DarkModeTextBox();
            labelFreeSpaceBeforeLimit = new DarkModeLabel();
            label2 = new DarkModeLabel();
            tbInsertAddressFile = new DarkModeTextBox();
            labelInsertAddressFile = new DarkModeLabel();
            labelData = new DarkModeLabel();
            tbData = new DarkModeTextBox();
            tbDataLength = new DarkModeTextBox();
            labelDataLength = new DarkModeLabel();
            SuspendLayout();
            // 
            // labelInsertAddressRAM
            // 
            labelInsertAddressRAM.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelInsertAddressRAM.AutoSize = true;
            labelInsertAddressRAM.Location = new System.Drawing.Point(83, 35);
            labelInsertAddressRAM.Name = "labelInsertAddressRAM";
            labelInsertAddressRAM.Size = new System.Drawing.Size(119, 15);
            labelInsertAddressRAM.TabIndex = 9;
            labelInsertAddressRAM.Text = "Insert address (RAM):";
            labelInsertAddressRAM.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCancel
            // 
            btnCancel.Anchor =  System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.Location = new System.Drawing.Point(426, 419);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnInsert
            // 
            btnInsert.Anchor =  System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnInsert.Location = new System.Drawing.Point(345, 419);
            btnInsert.Name = "btnInsert";
            btnInsert.Size = new System.Drawing.Size(75, 23);
            btnInsert.TabIndex = 4;
            btnInsert.Text = "Insert";
            btnInsert.UseVisualStyleBackColor = true;
            btnInsert.Click += btnInsert_Click;
            // 
            // tbInsertAddressRAM
            // 
            tbInsertAddressRAM.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tbInsertAddressRAM.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            tbInsertAddressRAM.Location = new System.Drawing.Point(208, 32);
            tbInsertAddressRAM.MaxLength = 9;
            tbInsertAddressRAM.Name = "tbInsertAddressRAM";
            tbInsertAddressRAM.Size = new System.Drawing.Size(119, 21);
            tbInsertAddressRAM.TabIndex = 0;
            tbInsertAddressRAM.Text = "0";
            tbInsertAddressRAM.TextChanged += tbInsertAddressRAM_TextChanged;
            // 
            // labelFirstAddrRAM
            // 
            labelFirstAddrRAM.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelFirstAddrRAM.AutoSize = true;
            labelFirstAddrRAM.Location = new System.Drawing.Point(12, 184);
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
            labelLastAddrRAM.Location = new System.Drawing.Point(13, 211);
            labelLastAddrRAM.Name = "labelLastAddrRAM";
            labelLastAddrRAM.Size = new System.Drawing.Size(189, 15);
            labelLastAddrRAM.TabIndex = 12;
            labelLastAddrRAM.Text = "Last post-EOF data address (RAM):";
            labelLastAddrRAM.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbFirstAddrRAM
            // 
            tbFirstAddrRAM.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tbFirstAddrRAM.BackColor = System.Drawing.SystemColors.Control;
            tbFirstAddrRAM.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            tbFirstAddrRAM.Location = new System.Drawing.Point(208, 181);
            tbFirstAddrRAM.MaxLength = 9;
            tbFirstAddrRAM.Name = "tbFirstAddrRAM";
            tbFirstAddrRAM.ReadOnly = true;
            tbFirstAddrRAM.Size = new System.Drawing.Size(119, 21);
            tbFirstAddrRAM.TabIndex = 4;
            tbFirstAddrRAM.Text = "0";
            // 
            // tbLastAddrRAM
            // 
            tbLastAddrRAM.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tbLastAddrRAM.BackColor = System.Drawing.SystemColors.Control;
            tbLastAddrRAM.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            tbLastAddrRAM.Location = new System.Drawing.Point(208, 208);
            tbLastAddrRAM.MaxLength = 9;
            tbLastAddrRAM.Name = "tbLastAddrRAM";
            tbLastAddrRAM.ReadOnly = true;
            tbLastAddrRAM.Size = new System.Drawing.Size(119, 21);
            tbLastAddrRAM.TabIndex = 6;
            tbLastAddrRAM.Text = "0";
            // 
            // tbFirstAddrFile
            // 
            tbFirstAddrFile.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tbFirstAddrFile.BackColor = System.Drawing.SystemColors.Control;
            tbFirstAddrFile.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            tbFirstAddrFile.Location = new System.Drawing.Point(382, 181);
            tbFirstAddrFile.MaxLength = 9;
            tbFirstAddrFile.Name = "tbFirstAddrFile";
            tbFirstAddrFile.ReadOnly = true;
            tbFirstAddrFile.Size = new System.Drawing.Size(119, 21);
            tbFirstAddrFile.TabIndex = 5;
            tbFirstAddrFile.Text = "0";
            // 
            // tbLastAddrFile
            // 
            tbLastAddrFile.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tbLastAddrFile.BackColor = System.Drawing.SystemColors.Control;
            tbLastAddrFile.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            tbLastAddrFile.Location = new System.Drawing.Point(382, 208);
            tbLastAddrFile.MaxLength = 9;
            tbLastAddrFile.Name = "tbLastAddrFile";
            tbLastAddrFile.ReadOnly = true;
            tbLastAddrFile.Size = new System.Drawing.Size(119, 21);
            tbLastAddrFile.TabIndex = 7;
            tbLastAddrFile.Text = "0";
            // 
            // labelLastAddrFile
            // 
            labelLastAddrFile.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelLastAddrFile.AutoSize = true;
            labelLastAddrFile.Location = new System.Drawing.Point(340, 211);
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
            labelFirstAddrFile.Location = new System.Drawing.Point(340, 184);
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
            labelFileEndRam.Location = new System.Drawing.Point(114, 361);
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
            tbFileEndRAM.Location = new System.Drawing.Point(208, 358);
            tbFileEndRAM.MaxLength = 9;
            tbFileEndRAM.Name = "tbFileEndRAM";
            tbFileEndRAM.ReadOnly = true;
            tbFileEndRAM.Size = new System.Drawing.Size(119, 21);
            tbFileEndRAM.TabIndex = 11;
            tbFileEndRAM.Text = "0";
            // 
            // tbFreeSpaceBeforePostEOFData
            // 
            tbFreeSpaceBeforePostEOFData.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tbFreeSpaceBeforePostEOFData.BackColor = System.Drawing.SystemColors.Control;
            tbFreeSpaceBeforePostEOFData.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            tbFreeSpaceBeforePostEOFData.Location = new System.Drawing.Point(382, 252);
            tbFreeSpaceBeforePostEOFData.MaxLength = 9;
            tbFreeSpaceBeforePostEOFData.Name = "tbFreeSpaceBeforePostEOFData";
            tbFreeSpaceBeforePostEOFData.ReadOnly = true;
            tbFreeSpaceBeforePostEOFData.Size = new System.Drawing.Size(119, 21);
            tbFreeSpaceBeforePostEOFData.TabIndex = 8;
            tbFreeSpaceBeforePostEOFData.Text = "0";
            // 
            // labelFreeSpace
            // 
            labelFreeSpace.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelFreeSpace.AutoSize = true;
            labelFreeSpace.Location = new System.Drawing.Point(145, 255);
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
            tbFileEndFile.Location = new System.Drawing.Point(382, 358);
            tbFileEndFile.MaxLength = 9;
            tbFileEndFile.Name = "tbFileEndFile";
            tbFileEndFile.ReadOnly = true;
            tbFileEndFile.Size = new System.Drawing.Size(119, 21);
            tbFileEndFile.TabIndex = 12;
            tbFileEndFile.Text = "0";
            // 
            // labelFileEndFile
            // 
            labelFileEndFile.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelFileEndFile.AutoSize = true;
            labelFileEndFile.Location = new System.Drawing.Point(340, 361);
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
            tbFileStartRam.Location = new System.Drawing.Point(208, 331);
            tbFileStartRam.MaxLength = 9;
            tbFileStartRam.Name = "tbFileStartRam";
            tbFileStartRam.ReadOnly = true;
            tbFileStartRam.Size = new System.Drawing.Size(119, 21);
            tbFileStartRam.TabIndex = 10;
            tbFileStartRam.Text = "0";
            // 
            // label1
            // 
            label1.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(111, 334);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(91, 15);
            label1.TabIndex = 20;
            label1.Text = "File start (RAM):";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbLimitRAM
            // 
            tbLimitRAM.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tbLimitRAM.BackColor = System.Drawing.SystemColors.Control;
            tbLimitRAM.Enabled = false;
            tbLimitRAM.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            tbLimitRAM.Location = new System.Drawing.Point(208, 385);
            tbLimitRAM.MaxLength = 9;
            tbLimitRAM.Name = "tbLimitRAM";
            tbLimitRAM.Size = new System.Drawing.Size(119, 21);
            tbLimitRAM.TabIndex = 13;
            tbLimitRAM.Text = "0";
            // 
            // labelLimit
            // 
            labelLimit.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelLimit.AutoSize = true;
            labelLimit.Location = new System.Drawing.Point(36, 388);
            labelLimit.Name = "labelLimit";
            labelLimit.Size = new System.Drawing.Size(166, 15);
            labelLimit.TabIndex = 23;
            labelLimit.Text = "Limit / start of next file (RAM):";
            labelLimit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbFreeSpaceBeforeLimit
            // 
            tbFreeSpaceBeforeLimit.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tbFreeSpaceBeforeLimit.BackColor = System.Drawing.SystemColors.Control;
            tbFreeSpaceBeforeLimit.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            tbFreeSpaceBeforeLimit.Location = new System.Drawing.Point(382, 279);
            tbFreeSpaceBeforeLimit.MaxLength = 9;
            tbFreeSpaceBeforeLimit.Name = "tbFreeSpaceBeforeLimit";
            tbFreeSpaceBeforeLimit.ReadOnly = true;
            tbFreeSpaceBeforeLimit.Size = new System.Drawing.Size(119, 21);
            tbFreeSpaceBeforeLimit.TabIndex = 9;
            tbFreeSpaceBeforeLimit.Text = "0";
            // 
            // labelFreeSpaceBeforeLimit
            // 
            labelFreeSpaceBeforeLimit.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelFreeSpaceBeforeLimit.AutoSize = true;
            labelFreeSpaceBeforeLimit.Location = new System.Drawing.Point(24, 282);
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
            label2.Location = new System.Drawing.Point(67, 303);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(378, 15);
            label2.TabIndex = 26;
            label2.Text = "(* doesn't account for length of last post-EOF data, which is unknown)";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbInsertAddressFile
            // 
            tbInsertAddressFile.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tbInsertAddressFile.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            tbInsertAddressFile.Location = new System.Drawing.Point(382, 32);
            tbInsertAddressFile.MaxLength = 9;
            tbInsertAddressFile.Name = "tbInsertAddressFile";
            tbInsertAddressFile.Size = new System.Drawing.Size(119, 21);
            tbInsertAddressFile.TabIndex = 1;
            tbInsertAddressFile.Text = "0";
            tbInsertAddressFile.TextChanged += tbInsertAddressFile_TextChanged;
            // 
            // labelInsertAddressFile
            // 
            labelInsertAddressFile.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelInsertAddressFile.AutoSize = true;
            labelInsertAddressFile.Location = new System.Drawing.Point(337, 35);
            labelInsertAddressFile.Name = "labelInsertAddressFile";
            labelInsertAddressFile.Size = new System.Drawing.Size(36, 15);
            labelInsertAddressFile.TabIndex = 28;
            labelInsertAddressFile.Text = "(File):";
            labelInsertAddressFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelData
            // 
            labelData.AutoSize = true;
            labelData.Location = new System.Drawing.Point(12, 63);
            labelData.Name = "labelData";
            labelData.Size = new System.Drawing.Size(249, 15);
            labelData.TabIndex = 29;
            labelData.Text = "Data (number of bytes must be divisible by 4):";
            labelData.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbData
            // 
            tbData.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tbData.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            tbData.Location = new System.Drawing.Point(12, 81);
            tbData.Multiline = true;
            tbData.Name = "tbData";
            tbData.Size = new System.Drawing.Size(489, 51);
            tbData.TabIndex = 2;
            tbData.Text = "00000000";
            tbData.TextChanged += tbData_TextChanged;
            // 
            // tbDataLength
            // 
            tbDataLength.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tbDataLength.BackColor = System.Drawing.SystemColors.Control;
            tbDataLength.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            tbDataLength.Location = new System.Drawing.Point(382, 138);
            tbDataLength.MaxLength = 5;
            tbDataLength.Name = "tbDataLength";
            tbDataLength.ReadOnly = true;
            tbDataLength.Size = new System.Drawing.Size(119, 21);
            tbDataLength.TabIndex = 3;
            tbDataLength.Text = "4";
            // 
            // labelDataLength
            // 
            labelDataLength.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelDataLength.AutoSize = true;
            labelDataLength.Location = new System.Drawing.Point(180, 141);
            labelDataLength.Name = "labelDataLength";
            labelDataLength.Size = new System.Drawing.Size(196, 15);
            labelDataLength.TabIndex = 32;
            labelDataLength.Text = "Data length (must be divisible by 4):";
            labelDataLength.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // InsertDataDialog
            // 
            AcceptButton = btnInsert;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(513, 454);
            Controls.Add(labelDataLength);
            Controls.Add(tbDataLength);
            Controls.Add(tbData);
            Controls.Add(labelData);
            Controls.Add(labelInsertAddressFile);
            Controls.Add(tbInsertAddressFile);
            Controls.Add(label2);
            Controls.Add(labelFreeSpaceBeforeLimit);
            Controls.Add(tbFreeSpaceBeforeLimit);
            Controls.Add(labelLimit);
            Controls.Add(tbLimitRAM);
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
            Controls.Add(tbInsertAddressRAM);
            Controls.Add(btnInsert);
            Controls.Add(btnCancel);
            Controls.Add(labelInsertAddressRAM);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "InsertDataDialog";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Insert Data";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DarkModeLabel labelInsertAddressRAM;
        private DarkModeButton btnCancel;
        private DarkModeButton btnInsert;
        private DarkModeTextBox tbInsertAddressRAM;
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
        private DarkModeTextBox tbLimitRAM;
        private DarkModeLabel labelLimit;
        private DarkModeTextBox tbFreeSpaceBeforeLimit;
        private DarkModeLabel labelFreeSpaceBeforeLimit;
        private DarkModeLabel label2;
        private DarkModeTextBox tbInsertAddressFile;
        private DarkModeLabel labelInsertAddressFile;
        private DarkModeLabel labelData;
        private DarkModeTextBox tbData;
        private DarkModeTextBox tbDataLength;
        private DarkModeLabel labelDataLength;
    }
}