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
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            tsbToggleWireframe = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            tsbToggleNormals = new System.Windows.Forms.ToolStripButton();
            tsbToggleTerrainType = new System.Windows.Forms.ToolStripButton();
            tsbToggleEventID = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            tsbRecalculateLightmapOriginalMath = new System.Windows.Forms.ToolStripButton();
            tsbUpdateLightmapUpdatedMath = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            tsbToggleHelp = new System.Windows.Forms.ToolStripButton();
            tilePropertyControl1 = new TilePropertiesControl();
            mpdViewerGLControl1 = new MPD_ViewerGLControl();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsbToggleWireframe, toolStripSeparator1, tsbToggleNormals, tsbToggleTerrainType, tsbToggleEventID, toolStripSeparator2, tsbRecalculateLightmapOriginalMath, tsbUpdateLightmapUpdatedMath, toolStripSeparator3, tsbToggleHelp });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(789, 31);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // tsbToggleWireframe
            // 
            tsbToggleWireframe.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbToggleWireframe.Image = Properties.Resources.IconWireframeBmp;
            tsbToggleWireframe.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbToggleWireframe.Name = "tsbToggleWireframe";
            tsbToggleWireframe.Size = new System.Drawing.Size(28, 28);
            tsbToggleWireframe.Text = "Draw Wireframe";
            tsbToggleWireframe.ToolTipText = "Show Wireframe";
            tsbToggleWireframe.Click += tsbToggleWireframe_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // tsbToggleNormals
            // 
            tsbToggleNormals.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbToggleNormals.Image = Properties.Resources.NormalsBmp;
            tsbToggleNormals.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbToggleNormals.Name = "tsbToggleNormals";
            tsbToggleNormals.Size = new System.Drawing.Size(28, 28);
            tsbToggleNormals.Text = "Draw Normal Map";
            tsbToggleNormals.ToolTipText = "Show Normal Map";
            tsbToggleNormals.Click += tsbToggleNormals_Click;
            // 
            // tsbToggleTerrainType
            // 
            tsbToggleTerrainType.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbToggleTerrainType.Image = Properties.Resources.TerrainTypesBmp;
            tsbToggleTerrainType.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbToggleTerrainType.Name = "tsbToggleTerrainType";
            tsbToggleTerrainType.Size = new System.Drawing.Size(28, 28);
            tsbToggleTerrainType.Text = "Toggle Terrain Type";
            // 
            // tsbToggleEventID
            // 
            tsbToggleEventID.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbToggleEventID.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbToggleEventID.Name = "tsbToggleEventID";
            tsbToggleEventID.Size = new System.Drawing.Size(23, 28);
            tsbToggleEventID.Text = "Toggle Event ID";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(6, 31);
            // 
            // tsbRecalculateLightmapOriginalMath
            // 
            tsbRecalculateLightmapOriginalMath.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbRecalculateLightmapOriginalMath.Image = Properties.Resources.LightingOldBmp;
            tsbRecalculateLightmapOriginalMath.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbRecalculateLightmapOriginalMath.Name = "tsbRecalculateLightmapOriginalMath";
            tsbRecalculateLightmapOriginalMath.Size = new System.Drawing.Size(28, 28);
            tsbRecalculateLightmapOriginalMath.Text = "Recalculate Lightmap (Original Math)";
            tsbRecalculateLightmapOriginalMath.Click += tsbRecalculateLightmapOriginalMath_Click;
            // 
            // tsbUpdateLightmapUpdatedMath
            // 
            tsbUpdateLightmapUpdatedMath.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbUpdateLightmapUpdatedMath.Image = Properties.Resources.LightingNewBmp;
            tsbUpdateLightmapUpdatedMath.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbUpdateLightmapUpdatedMath.Name = "tsbUpdateLightmapUpdatedMath";
            tsbUpdateLightmapUpdatedMath.Size = new System.Drawing.Size(28, 28);
            tsbUpdateLightmapUpdatedMath.Text = "Update Lightmap (Updated Math)";
            tsbUpdateLightmapUpdatedMath.Click += tsbUpdateLightmapUpdatedMath_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(6, 31);
            // 
            // tsbToggleHelp
            // 
            tsbToggleHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbToggleHelp.Image = Properties.Resources.IconHelpBmp;
            tsbToggleHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbToggleHelp.Name = "tsbToggleHelp";
            tsbToggleHelp.Size = new System.Drawing.Size(28, 28);
            tsbToggleHelp.Text = "Show Help";
            tsbToggleHelp.Click += tsbToggleHelp_Click;
            // 
            // tilePropertyControl1
            // 
            tilePropertyControl1.Dock = System.Windows.Forms.DockStyle.Right;
            tilePropertyControl1.Location = new System.Drawing.Point(582, 31);
            tilePropertyControl1.MaximumSize = new System.Drawing.Size(207, 10000);
            tilePropertyControl1.MinimumSize = new System.Drawing.Size(207, 446);
            tilePropertyControl1.Name = "tilePropertyControl1";
            tilePropertyControl1.Size = new System.Drawing.Size(207, 545);
            tilePropertyControl1.TabIndex = 2;
            tilePropertyControl1.Tile = null;
            // 
            // mpdViewerGLControl1
            // 
            mpdViewerGLControl1.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            mpdViewerGLControl1.APIVersion = new System.Version(3, 3, 0, 0);
            mpdViewerGLControl1.BackColor = System.Drawing.Color.FromArgb(  64,   64,   64);
            mpdViewerGLControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            mpdViewerGLControl1.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            mpdViewerGLControl1.IsEventDriven = true;
            mpdViewerGLControl1.Location = new System.Drawing.Point(0, 31);
            mpdViewerGLControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            mpdViewerGLControl1.MinimumSize = new System.Drawing.Size(320, 240);
            mpdViewerGLControl1.Name = "mpdViewerGLControl1";
            mpdViewerGLControl1.Profile = OpenTK.Windowing.Common.ContextProfile.Core;
            mpdViewerGLControl1.SharedContext = null;
            mpdViewerGLControl1.Size = new System.Drawing.Size(582, 545);
            mpdViewerGLControl1.TabIndex = 1;
            // 
            // MPD_ViewerControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(mpdViewerGLControl1);
            Controls.Add(tilePropertyControl1);
            Controls.Add(toolStrip1);
            Name = "MPD_ViewerControl";
            Size = new System.Drawing.Size(789, 576);
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
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbRecalculateLightmapOriginalMath;
        private System.Windows.Forms.ToolStripButton tsbUpdateLightmapUpdatedMath;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private TilePropertiesControl tilePropertyControl1;
        private MPD_ViewerGLControl mpdViewerGLControl1;
        private System.Windows.Forms.ToolStripButton tsbToggleTerrainType;
        private System.Windows.Forms.ToolStripButton tsbToggleEventID;
    }
}
