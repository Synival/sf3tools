namespace SF3.X1_Editor.Forms {
    partial class frmX1_Editor {
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(frmX1_Editor));
            menuStrip2 = new System.Windows.Forms.MenuStrip();
            tsmiScenario = new System.Windows.Forms.ToolStripMenuItem();
            tsmiScenario_BTL99 = new System.Windows.Forms.ToolStripMenuItem();
            tsmiHelp = new System.Windows.Forms.ToolStripMenuItem();
            tsmiHelp_About = new System.Windows.Forms.ToolStripMenuItem();
            menuStrip2.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip2
            // 
            menuStrip2.Dock = System.Windows.Forms.DockStyle.None;
            menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiScenario, tsmiHelp });
            menuStrip2.Location = new System.Drawing.Point(0, 0);
            menuStrip2.Name = "menuStrip2";
            menuStrip2.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            menuStrip2.Size = new System.Drawing.Size(237, 24);
            menuStrip2.TabIndex = 1;
            // 
            // tsmiScenario
            // 
            tsmiScenario.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiScenario_BTL99 });
            tsmiScenario.Name = "tsmiScenario";
            tsmiScenario.Size = new System.Drawing.Size(64, 20);
            tsmiScenario.Text = "&Scenario";
            // 
            // tsmiScenario_BTL99
            // 
            tsmiScenario_BTL99.Name = "tsmiScenario_BTL99";
            tsmiScenario_BTL99.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B;
            tsmiScenario_BTL99.Size = new System.Drawing.Size(185, 22);
            tsmiScenario_BTL99.Text = "&BTL99 Toggle";
            tsmiScenario_BTL99.Click += tsmiScenario_BTL99_Click;
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
            tsmiHelp_About.Size = new System.Drawing.Size(116, 22);
            tsmiHelp_About.Text = "About...";
            tsmiHelp_About.Click += tsmiHelp_About_Click;
            // 
            // frmX1_Editor
            // 
            AllowDrop = true;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(891, 631);
            Controls.Add(menuStrip2);
            Icon = (System.Drawing.Icon) resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "frmX1_Editor";
            Text = "SF3 X1 Editor";
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
        private System.Windows.Forms.ToolStripMenuItem tsmiScenario;
        private System.Windows.Forms.ToolStripMenuItem tsmiScenario_BTL99;
    }
}

