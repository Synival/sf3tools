namespace SF3.Editor.Forms {
    partial class ManipulateChunkDialog {
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
            btnCancel = new System.Windows.Forms.Button();
            btnAction = new System.Windows.Forms.Button();
            labelSelectAChunk = new System.Windows.Forms.Label();
            cbChunk = new System.Windows.Forms.ComboBox();
            labelWhichData = new System.Windows.Forms.Label();
            rbCompressed = new System.Windows.Forms.RadioButton();
            rbUncompressed = new System.Windows.Forms.RadioButton();
            SuspendLayout();
            // 
            // btnCancel
            // 
            btnCancel.Anchor =  System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.Location = new System.Drawing.Point(339, 77);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnAction
            // 
            btnAction.Anchor =  System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnAction.Location = new System.Drawing.Point(258, 77);
            btnAction.Name = "btnAction";
            btnAction.Size = new System.Drawing.Size(75, 23);
            btnAction.TabIndex = 4;
            btnAction.Text = "(Action)";
            btnAction.UseVisualStyleBackColor = true;
            btnAction.Click += btnAction_Click;
            // 
            // labelSelectAChunk
            // 
            labelSelectAChunk.AutoSize = true;
            labelSelectAChunk.Location = new System.Drawing.Point(12, 9);
            labelSelectAChunk.Name = "labelSelectAChunk";
            labelSelectAChunk.Size = new System.Drawing.Size(86, 15);
            labelSelectAChunk.TabIndex = 2;
            labelSelectAChunk.Text = "Select a chunk:";
            // 
            // cbChunk
            // 
            cbChunk.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cbChunk.FormattingEnabled = true;
            cbChunk.Location = new System.Drawing.Point(12, 27);
            cbChunk.Name = "cbChunk";
            cbChunk.Size = new System.Drawing.Size(402, 23);
            cbChunk.TabIndex = 1;
            // 
            // labelWhichData
            // 
            labelWhichData.Anchor =  System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            labelWhichData.AutoSize = true;
            labelWhichData.Location = new System.Drawing.Point(12, 63);
            labelWhichData.Name = "labelWhichData";
            labelWhichData.Size = new System.Drawing.Size(72, 15);
            labelWhichData.TabIndex = 4;
            labelWhichData.Text = "Which data?";
            // 
            // rbCompressed
            // 
            rbCompressed.Anchor =  System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            rbCompressed.AutoSize = true;
            rbCompressed.Location = new System.Drawing.Point(12, 81);
            rbCompressed.Name = "rbCompressed";
            rbCompressed.Size = new System.Drawing.Size(91, 19);
            rbCompressed.TabIndex = 2;
            rbCompressed.Text = "Compressed";
            rbCompressed.UseVisualStyleBackColor = true;
            // 
            // rbUncompressed
            // 
            rbUncompressed.Anchor =  System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            rbUncompressed.AutoSize = true;
            rbUncompressed.Checked = true;
            rbUncompressed.Location = new System.Drawing.Point(109, 81);
            rbUncompressed.Name = "rbUncompressed";
            rbUncompressed.Size = new System.Drawing.Size(104, 19);
            rbUncompressed.TabIndex = 3;
            rbUncompressed.TabStop = true;
            rbUncompressed.Text = "Uncompressed";
            rbUncompressed.UseVisualStyleBackColor = true;
            // 
            // ManipulateChunkDialog
            // 
            AcceptButton = btnAction;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(426, 112);
            Controls.Add(rbUncompressed);
            Controls.Add(rbCompressed);
            Controls.Add(labelWhichData);
            Controls.Add(cbChunk);
            Controls.Add(labelSelectAChunk);
            Controls.Add(btnAction);
            Controls.Add(btnCancel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ManipulateChunkDialog";
            ShowIcon = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Manipulate Chunk Dialog";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnAction;
        private System.Windows.Forms.Label labelSelectAChunk;
        private System.Windows.Forms.ComboBox cbChunk;
        private System.Windows.Forms.Label labelWhichData;
        private System.Windows.Forms.RadioButton rbCompressed;
        private System.Windows.Forms.RadioButton rbUncompressed;
    }
}