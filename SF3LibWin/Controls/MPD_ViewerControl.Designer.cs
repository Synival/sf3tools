namespace SF3.Win.Controls {
    partial class MPD_ViewerControl {
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(MPD_ViewerControl));
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            tsbToggleWireframe = new System.Windows.Forms.ToolStripButton();
            tsbToggleHelp = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            tsbToggleNormals = new System.Windows.Forms.ToolStripButton();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsbToggleWireframe, tsbToggleHelp, toolStripSeparator1, tsbToggleNormals });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(500, 31);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // tsbToggleWireframe
            // 
            tsbToggleWireframe.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbToggleWireframe.Image = (System.Drawing.Image) resources.GetObject("tsbToggleWireframe.Image");
            tsbToggleWireframe.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbToggleWireframe.Name = "tsbToggleWireframe";
            tsbToggleWireframe.Size = new System.Drawing.Size(28, 28);
            tsbToggleWireframe.Text = "Draw Wireframe";
            tsbToggleWireframe.ToolTipText = "Show Wireframe";
            tsbToggleWireframe.Click += tsbToggleWireframe_Click;
            // 
            // tsbToggleHelp
            // 
            tsbToggleHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbToggleHelp.Image = (System.Drawing.Image) resources.GetObject("tsbToggleHelp.Image");
            tsbToggleHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbToggleHelp.Name = "tsbToggleHelp";
            tsbToggleHelp.Size = new System.Drawing.Size(28, 28);
            tsbToggleHelp.Text = "Show Help";
            tsbToggleHelp.Click += tsbToggleHelp_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // tsbToggleNormals
            // 
            tsbToggleNormals.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbToggleNormals.Image = (System.Drawing.Image) resources.GetObject("tsbToggleNormals.Image");
            tsbToggleNormals.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbToggleNormals.Name = "tsbToggleNormals";
            tsbToggleNormals.Size = new System.Drawing.Size(28, 28);
            tsbToggleNormals.Text = "Draw Normal Map";
            tsbToggleNormals.ToolTipText = "Show Normal Map";
            tsbToggleNormals.Click += tsbToggleNormals_Click;
            // 
            // MPD_ViewerControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(toolStrip1);
            Name = "MPD_ViewerControl";
            Size = new System.Drawing.Size(500, 500);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbToggleWireframe;
        private System.Windows.Forms.ToolStripButton tsbToggleHelp;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbToggleNormals;
    }
}
