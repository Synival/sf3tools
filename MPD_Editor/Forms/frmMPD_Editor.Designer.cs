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
            tsmiTextures = new System.Windows.Forms.ToolStripMenuItem();
            tsmiTextures_ImportFolder = new System.Windows.Forms.ToolStripMenuItem();
            tsmiTextures_ExportToFolder = new System.Windows.Forms.ToolStripMenuItem();
            tsmiHelp = new System.Windows.Forms.ToolStripMenuItem();
            tsmiHelp_About = new System.Windows.Forms.ToolStripMenuItem();
            menuStrip2.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip2
            // 
            menuStrip2.Dock = System.Windows.Forms.DockStyle.None;
            menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiTextures, tsmiHelp });
            menuStrip2.Location = new System.Drawing.Point(0, 0);
            menuStrip2.Name = "menuStrip2";
            menuStrip2.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            menuStrip2.Size = new System.Drawing.Size(115, 24);
            menuStrip2.TabIndex = 1;
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
            tsmiTextures_ImportFolder.Size = new System.Drawing.Size(166, 22);
            tsmiTextures_ImportFolder.Text = "&Import Folder...";
            tsmiTextures_ImportFolder.Click += tsmiTextures_ImportFolder_Click;
            // 
            // tsmiTextures_ExportToFolder
            // 
            tsmiTextures_ExportToFolder.Enabled = false;
            tsmiTextures_ExportToFolder.Name = "tsmiTextures_ExportToFolder";
            tsmiTextures_ExportToFolder.Size = new System.Drawing.Size(166, 22);
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
            // frmMPDEditor
            // 
            AllowDrop = true;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(891, 616);
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
    }
}

