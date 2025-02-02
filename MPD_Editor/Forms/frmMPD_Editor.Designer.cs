namespace SF3.MPD_Editor.Forms {
    partial class frmMPDEditor {
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMPDEditor));
            menuStrip2 = new System.Windows.Forms.MenuStrip();
            tsmiChunks = new System.Windows.Forms.ToolStripMenuItem();
            tsmiChunks_exportChunk = new System.Windows.Forms.ToolStripMenuItem();
            tsmiChunks_ImportChunk = new System.Windows.Forms.ToolStripMenuItem();
            tsmiChunks_DeleteChunk = new System.Windows.Forms.ToolStripMenuItem();
            tsmiTextures = new System.Windows.Forms.ToolStripMenuItem();
            tsmiTextures_ImportFolder = new System.Windows.Forms.ToolStripMenuItem();
            tsmiTextures_ExportToFolder = new System.Windows.Forms.ToolStripMenuItem();
            tsmiHelp = new System.Windows.Forms.ToolStripMenuItem();
            tsmiHelp_About = new System.Windows.Forms.ToolStripMenuItem();
            tsmiChunks_Separator1 = new System.Windows.Forms.ToolStripSeparator();
            tsmiChunks_RecompressChunks = new System.Windows.Forms.ToolStripMenuItem();
            tsmiChunks_CorrectChunkPlacement = new System.Windows.Forms.ToolStripMenuItem();
            tsmiChunks_Separator2 = new System.Windows.Forms.ToolStripSeparator();
            tsmiChunks_MoveChunksAutomatically = new System.Windows.Forms.ToolStripMenuItem();
            menuStrip2.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip2
            // 
            menuStrip2.Dock = System.Windows.Forms.DockStyle.None;
            menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiChunks, tsmiTextures, tsmiHelp });
            menuStrip2.Location = new System.Drawing.Point(0, 0);
            menuStrip2.Name = "menuStrip2";
            menuStrip2.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            menuStrip2.Size = new System.Drawing.Size(294, 24);
            menuStrip2.TabIndex = 1;
            // 
            // tsmiChunks
            // 
            tsmiChunks.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiChunks_exportChunk, tsmiChunks_ImportChunk, tsmiChunks_DeleteChunk, tsmiChunks_Separator1, tsmiChunks_RecompressChunks, tsmiChunks_CorrectChunkPlacement, tsmiChunks_Separator2, tsmiChunks_MoveChunksAutomatically });
            tsmiChunks.Name = "tsmiChunks";
            tsmiChunks.Size = new System.Drawing.Size(59, 20);
            tsmiChunks.Text = "&Chunks";
            // 
            // tsmiChunks_exportChunk
            // 
            tsmiChunks_exportChunk.Enabled = false;
            tsmiChunks_exportChunk.Name = "tsmiChunks_exportChunk";
            tsmiChunks_exportChunk.Size = new System.Drawing.Size(224, 22);
            tsmiChunks_exportChunk.Text = "&Export Chunk...";
            // 
            // tsmiChunks_ImportChunk
            // 
            tsmiChunks_ImportChunk.Enabled = false;
            tsmiChunks_ImportChunk.Name = "tsmiChunks_ImportChunk";
            tsmiChunks_ImportChunk.Size = new System.Drawing.Size(224, 22);
            tsmiChunks_ImportChunk.Text = "&Import Chunk...";
            // 
            // tsmiChunks_DeleteChunk
            // 
            tsmiChunks_DeleteChunk.Enabled = false;
            tsmiChunks_DeleteChunk.Name = "tsmiChunks_DeleteChunk";
            tsmiChunks_DeleteChunk.Size = new System.Drawing.Size(224, 22);
            tsmiChunks_DeleteChunk.Text = "&Delete Chunk...";
            // 
            // tsmiTextures
            // 
            tsmiTextures.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiTextures_ImportFolder, tsmiTextures_ExportToFolder });
            tsmiTextures.Name = "tsmiTextures";
            tsmiTextures.Size = new System.Drawing.Size(62, 20);
            tsmiTextures.Text = "&Textures";
            // 
            // tsmiTextures_ImportFolder
            // 
            tsmiTextures_ImportFolder.Enabled = false;
            tsmiTextures_ImportFolder.Name = "tsmiTextures_ImportFolder";
            tsmiTextures_ImportFolder.Size = new System.Drawing.Size(180, 22);
            tsmiTextures_ImportFolder.Text = "&Import Folder...";
            tsmiTextures_ImportFolder.Click += tsmiTextures_ImportFolder_Click;
            // 
            // tsmiTextures_ExportToFolder
            // 
            tsmiTextures_ExportToFolder.Enabled = false;
            tsmiTextures_ExportToFolder.Name = "tsmiTextures_ExportToFolder";
            tsmiTextures_ExportToFolder.Size = new System.Drawing.Size(180, 22);
            tsmiTextures_ExportToFolder.Text = "&Export to Folder...";
            tsmiTextures_ExportToFolder.Click += tsmiTextures_ExportToFolder_Click;
            // 
            // tsmiHelp
            // 
            tsmiHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiHelp_About });
            tsmiHelp.Name = "tsmiHelp";
            tsmiHelp.Size = new System.Drawing.Size(44, 20);
            tsmiHelp.Text = "&Help";
            // 
            // tsmiHelp_About
            // 
            tsmiHelp_About.Name = "tsmiHelp_About";
            tsmiHelp_About.Size = new System.Drawing.Size(180, 22);
            tsmiHelp_About.Text = "About...";
            tsmiHelp_About.Click += tsmiHelp_About_Click;
            // 
            // tsmiChunks_Separator1
            // 
            tsmiChunks_Separator1.Name = "tsmiChunks_Separator1";
            tsmiChunks_Separator1.Size = new System.Drawing.Size(221, 6);
            // 
            // tsmiChunks_RecompressChunks
            // 
            tsmiChunks_RecompressChunks.Enabled = false;
            tsmiChunks_RecompressChunks.Name = "tsmiChunks_RecompressChunks";
            tsmiChunks_RecompressChunks.Size = new System.Drawing.Size(224, 22);
            tsmiChunks_RecompressChunks.Text = "Recompress Chunks";
            // 
            // tsmiChunks_CorrectChunkPlacement
            // 
            tsmiChunks_CorrectChunkPlacement.Enabled = false;
            tsmiChunks_CorrectChunkPlacement.Name = "tsmiChunks_CorrectChunkPlacement";
            tsmiChunks_CorrectChunkPlacement.Size = new System.Drawing.Size(224, 22);
            tsmiChunks_CorrectChunkPlacement.Text = "Correct Chunk Placement";
            // 
            // tsmiChunks_Separator2
            // 
            tsmiChunks_Separator2.Name = "tsmiChunks_Separator2";
            tsmiChunks_Separator2.Size = new System.Drawing.Size(221, 6);
            // 
            // tsmiChunks_MoveChunksAutomatically
            // 
            tsmiChunks_MoveChunksAutomatically.Checked = true;
            tsmiChunks_MoveChunksAutomatically.CheckState = System.Windows.Forms.CheckState.Checked;
            tsmiChunks_MoveChunksAutomatically.Enabled = false;
            tsmiChunks_MoveChunksAutomatically.Name = "tsmiChunks_MoveChunksAutomatically";
            tsmiChunks_MoveChunksAutomatically.Size = new System.Drawing.Size(224, 22);
            tsmiChunks_MoveChunksAutomatically.Text = "Move Chunks Automatically";
            // 
            // frmMPDEditor
            // 
            AllowDrop = true;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(891, 631);
            Controls.Add(menuStrip2);
            Icon = (System.Drawing.Icon) resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "frmMPDEditor";
            Text = "SF3 MPD Editor";
            Controls.SetChildIndex(menuStrip2, 0);
            menuStrip2.ResumeLayout(false);
            menuStrip2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem tsmiHelp;
        private System.Windows.Forms.ToolStripMenuItem tsmiHelp_About;
        private System.Windows.Forms.ToolStripMenuItem tsmiTextures;
        private System.Windows.Forms.ToolStripMenuItem tsmiTextures_ImportFolder;
        private System.Windows.Forms.ToolStripMenuItem tsmiTextures_ExportToFolder;
        private System.Windows.Forms.ToolStripMenuItem tsmiChunks;
        private System.Windows.Forms.ToolStripMenuItem tsmiChunks_exportChunk;
        private System.Windows.Forms.ToolStripMenuItem tsmiChunks_ImportChunk;
        private System.Windows.Forms.ToolStripMenuItem tsmiChunks_DeleteChunk;
        private System.Windows.Forms.ToolStripSeparator tsmiChunks_Separator1;
        private System.Windows.Forms.ToolStripMenuItem tsmiChunks_RecompressChunks;
        private System.Windows.Forms.ToolStripMenuItem tsmiChunks_CorrectChunkPlacement;
        private System.Windows.Forms.ToolStripSeparator tsmiChunks_Separator2;
        private System.Windows.Forms.ToolStripMenuItem tsmiChunks_MoveChunksAutomatically;
    }
}

