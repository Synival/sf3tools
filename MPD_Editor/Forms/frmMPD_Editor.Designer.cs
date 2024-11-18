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
            tsmiHelp = new System.Windows.Forms.ToolStripMenuItem();
            tsmiHelp_Separator1 = new System.Windows.Forms.ToolStripSeparator();
            tsmiHelp_Credits1 = new System.Windows.Forms.ToolStripMenuItem();
            tsmiHelp_Credits2 = new System.Windows.Forms.ToolStripMenuItem();
            menuStrip2.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip2
            // 
            menuStrip2.Dock = System.Windows.Forms.DockStyle.None;
            menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiHelp });
            menuStrip2.Location = new System.Drawing.Point(0, 0);
            menuStrip2.Name = "menuStrip2";
            menuStrip2.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            menuStrip2.Size = new System.Drawing.Size(173, 24);
            menuStrip2.TabIndex = 1;
            // 
            // tsmiHelp
            // 
            tsmiHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiHelp_Separator1, tsmiHelp_Credits1, tsmiHelp_Credits2 });
            tsmiHelp.Name = "tsmiHelp";
            tsmiHelp.Size = new System.Drawing.Size(44, 20);
            tsmiHelp.Text = "&Help";
            // 
            // tsmiHelp_Separator1
            // 
            tsmiHelp_Separator1.Name = "tsmiHelp_Separator1";
            tsmiHelp_Separator1.Size = new System.Drawing.Size(254, 6);
            // 
            // tsmiHelp_Credits1
            // 
            tsmiHelp_Credits1.Name = "tsmiHelp_Credits1";
            tsmiHelp_Credits1.Size = new System.Drawing.Size(257, 22);
            tsmiHelp_Credits1.Text = "All credit to AggroCrag for the";
            // 
            // tsmiHelp_Credits2
            // 
            tsmiHelp_Credits2.Name = "tsmiHelp_Credits2";
            tsmiHelp_Credits2.Size = new System.Drawing.Size(257, 22);
            tsmiHelp_Credits2.Text = "compression/decompression code";
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
        private System.Windows.Forms.ToolStripSeparator tsmiHelp_Separator1;
        private System.Windows.Forms.ToolStripMenuItem tsmiHelp_Credits1;
        private System.Windows.Forms.ToolStripMenuItem tsmiHelp_Credits2;
    }
}

