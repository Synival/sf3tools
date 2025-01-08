namespace SF3.IconPointerEditor.Forms
{
    partial class frmIconPointerEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(frmIconPointerEditor));
            menuStrip2 = new System.Windows.Forms.MenuStrip();
            tsmiHelp = new System.Windows.Forms.ToolStripMenuItem();
            tsmiHelp_OpenInfo = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            tsmiHelp_About = new System.Windows.Forms.ToolStripMenuItem();
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
            menuStrip2.Text = "menuStrip2";
            // 
            // tsmiHelp
            // 
            tsmiHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiHelp_OpenInfo, toolStripSeparator1, tsmiHelp_About });
            tsmiHelp.Name = "tsmiHelp";
            tsmiHelp.Size = new System.Drawing.Size(44, 20);
            tsmiHelp.Text = "&Help";
            // 
            // tsmiHelp_OpenInfo
            // 
            tsmiHelp_OpenInfo.Name = "tsmiHelp_OpenInfo";
            tsmiHelp_OpenInfo.Size = new System.Drawing.Size(224, 22);
            tsmiHelp_OpenInfo.Text = "Opens X011, X021, X026 Files";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(221, 6);
            // 
            // tsmiHelp_About
            // 
            tsmiHelp_About.Name = "tsmiHelp_About";
            tsmiHelp_About.Size = new System.Drawing.Size(224, 22);
            tsmiHelp_About.Text = "About...";
            tsmiHelp_About.Click += tsmiHelp_About_Click;
            // 
            // frmIconPointerEditor
            // 
            AllowDrop = true;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(891, 616);
            Controls.Add(menuStrip2);
            Icon = (System.Drawing.Icon) resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "frmIconPointerEditor";
            Text = "SF3 Icon Pointer Editor";
            Controls.SetChildIndex(menuStrip2, 0);
            menuStrip2.ResumeLayout(false);
            menuStrip2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem tsmiHelp;
        private System.Windows.Forms.ToolStripMenuItem tsmiHelp_OpenInfo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmiHelp_About;
    }
}
