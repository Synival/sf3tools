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
            tsbCursorSelect = new System.Windows.Forms.ToolStripButton();
            tsbCursorNavigate = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            tsbDrawSurfaceModel = new System.Windows.Forms.ToolStripButton();
            tsbDrawModels = new System.Windows.Forms.ToolStripButton();
            tsbDrawGround = new System.Windows.Forms.ToolStripButton();
            tsbDrawSkyBox = new System.Windows.Forms.ToolStripButton();
            tsbRunAnimations = new System.Windows.Forms.ToolStripButton();
            tsbDrawGradients = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            tsbToggleWireframe = new System.Windows.Forms.ToolStripButton();
            tsbToggleBoundaries = new System.Windows.Forms.ToolStripButton();
            tsbToggleTerrainType = new System.Windows.Forms.ToolStripButton();
            tsbToggleEventID = new System.Windows.Forms.ToolStripButton();
            tsbToggleNormals = new System.Windows.Forms.ToolStripButton();
            tsbRotateSpritesUp = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            tsbCameraReset = new System.Windows.Forms.ToolStripButton();
            tsbCameraTopView = new System.Windows.Forms.ToolStripButton();
            tsbCameraLookAtCenter = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            tsbRecalculateLightmapOriginalMath = new System.Windows.Forms.ToolStripButton();
            tsbUpdateLightmapUpdatedMath = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            tsbToggleHelp = new System.Windows.Forms.ToolStripButton();
            tilePropertyControl1 = new TilePropertiesControl();
            mpdViewerGLControl1 = new MPD_ViewerGLControl();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsbCursorSelect, tsbCursorNavigate, toolStripSeparator5, tsbDrawSurfaceModel, tsbDrawModels, tsbDrawGround, tsbDrawSkyBox, tsbRunAnimations, tsbDrawGradients, toolStripSeparator2, tsbToggleWireframe, tsbToggleBoundaries, tsbToggleTerrainType, tsbToggleEventID, tsbToggleNormals, tsbRotateSpritesUp, toolStripSeparator1, tsbCameraReset, tsbCameraTopView, tsbCameraLookAtCenter, toolStripSeparator3, tsbRecalculateLightmapOriginalMath, tsbUpdateLightmapUpdatedMath, toolStripSeparator4, tsbToggleHelp });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(789, 31);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // tsbCursorSelect
            // 
            tsbCursorSelect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbCursorSelect.Image = Properties.Resources.CursorSelectBmp;
            tsbCursorSelect.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbCursorSelect.Name = "tsbCursorSelect";
            tsbCursorSelect.Size = new System.Drawing.Size(28, 28);
            tsbCursorSelect.Text = "Select";
            tsbCursorSelect.Click += tsbCursorSelect_Click;
            // 
            // tsbCursorNavigate
            // 
            tsbCursorNavigate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbCursorNavigate.Image = Properties.Resources.CursorNavigateBmp;
            tsbCursorNavigate.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbCursorNavigate.Name = "tsbCursorNavigate";
            tsbCursorNavigate.Size = new System.Drawing.Size(28, 28);
            tsbCursorNavigate.Text = "Navigate";
            tsbCursorNavigate.Click += tsbCursorNavigate_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new System.Drawing.Size(6, 31);
            // 
            // tsbDrawSurfaceModel
            // 
            tsbDrawSurfaceModel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbDrawSurfaceModel.Image = Properties.Resources.ShowSurfaceModelBmp;
            tsbDrawSurfaceModel.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbDrawSurfaceModel.Name = "tsbDrawSurfaceModel";
            tsbDrawSurfaceModel.Size = new System.Drawing.Size(28, 28);
            tsbDrawSurfaceModel.Text = "Draw Surface Model";
            tsbDrawSurfaceModel.ToolTipText = "Draw Surface Model";
            tsbDrawSurfaceModel.Click += tsbDrawSurfaceModel_Click;
            // 
            // tsbDrawModels
            // 
            tsbDrawModels.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbDrawModels.Image = Properties.Resources.ShowModelsBmp;
            tsbDrawModels.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbDrawModels.Name = "tsbDrawModels";
            tsbDrawModels.Size = new System.Drawing.Size(28, 28);
            tsbDrawModels.Text = "Draw Models";
            tsbDrawModels.ToolTipText = "Draw Models";
            tsbDrawModels.Click += tsbDrawModels_Click;
            // 
            // tsbDrawGround
            // 
            tsbDrawGround.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbDrawGround.Image = Properties.Resources.ShowGroundWaterBmp;
            tsbDrawGround.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbDrawGround.Name = "tsbDrawGround";
            tsbDrawGround.Size = new System.Drawing.Size(28, 28);
            tsbDrawGround.Text = "Draw Ground";
            tsbDrawGround.ToolTipText = "Draw Ground";
            tsbDrawGround.Click += tsbDrawGround_Click;
            // 
            // tsbDrawSkyBox
            // 
            tsbDrawSkyBox.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbDrawSkyBox.Image = Properties.Resources.ShowSkyBoxBmp;
            tsbDrawSkyBox.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbDrawSkyBox.Name = "tsbDrawSkyBox";
            tsbDrawSkyBox.Size = new System.Drawing.Size(28, 28);
            tsbDrawSkyBox.Text = "Draw Sky Box";
            tsbDrawSkyBox.ToolTipText = "Draw Sky Box";
            tsbDrawSkyBox.Click += tsbDrawSkyBox_Click;
            // 
            // tsbRunAnimations
            // 
            tsbRunAnimations.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbRunAnimations.Image = Properties.Resources.RunAnimationsBmp;
            tsbRunAnimations.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbRunAnimations.Name = "tsbRunAnimations";
            tsbRunAnimations.Size = new System.Drawing.Size(28, 28);
            tsbRunAnimations.Text = "Run Animations";
            tsbRunAnimations.Click += tsbRunAnimations_Click;
            // 
            // tsbDrawGradients
            // 
            tsbDrawGradients.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbDrawGradients.Image = Properties.Resources.ShowGradientsBmp;
            tsbDrawGradients.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbDrawGradients.Name = "tsbDrawGradients";
            tsbDrawGradients.Size = new System.Drawing.Size(28, 28);
            tsbDrawGradients.Text = "Draw Gradients";
            tsbDrawGradients.ToolTipText = "Draw Gradients";
            tsbDrawGradients.Click += tsbDrawGradients_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(6, 31);
            // 
            // tsbToggleWireframe
            // 
            tsbToggleWireframe.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbToggleWireframe.Image = Properties.Resources.IconWireframeBmp;
            tsbToggleWireframe.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbToggleWireframe.Name = "tsbToggleWireframe";
            tsbToggleWireframe.Size = new System.Drawing.Size(28, 28);
            tsbToggleWireframe.Text = "Draw Wireframe";
            tsbToggleWireframe.ToolTipText = "Draw Wireframe";
            tsbToggleWireframe.Click += tsbToggleWireframe_Click;
            // 
            // tsbToggleBoundaries
            // 
            tsbToggleBoundaries.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbToggleBoundaries.Image = Properties.Resources.ShowCameraBoundariesBmp;
            tsbToggleBoundaries.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbToggleBoundaries.Name = "tsbToggleBoundaries";
            tsbToggleBoundaries.Size = new System.Drawing.Size(28, 28);
            tsbToggleBoundaries.Text = "Draw Boundaries";
            tsbToggleBoundaries.Click += tsbToggleBoundaries_Click;
            // 
            // tsbToggleTerrainType
            // 
            tsbToggleTerrainType.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbToggleTerrainType.Image = Properties.Resources.ShowTerrainTypesBmp;
            tsbToggleTerrainType.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbToggleTerrainType.Name = "tsbToggleTerrainType";
            tsbToggleTerrainType.Size = new System.Drawing.Size(28, 28);
            tsbToggleTerrainType.Text = "Draw Terrain Type";
            tsbToggleTerrainType.Click += tsbToggleTerrainType_Click;
            // 
            // tsbToggleEventID
            // 
            tsbToggleEventID.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbToggleEventID.Image = Properties.Resources.ShowEventIDsBmp;
            tsbToggleEventID.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbToggleEventID.Name = "tsbToggleEventID";
            tsbToggleEventID.Size = new System.Drawing.Size(28, 28);
            tsbToggleEventID.Text = "Draw Event ID";
            tsbToggleEventID.Click += tsbToggleEventID_Click;
            // 
            // tsbToggleNormals
            // 
            tsbToggleNormals.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbToggleNormals.Image = Properties.Resources.NormalsBmp;
            tsbToggleNormals.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbToggleNormals.Name = "tsbToggleNormals";
            tsbToggleNormals.Size = new System.Drawing.Size(28, 28);
            tsbToggleNormals.Text = "Show Normal Map";
            tsbToggleNormals.ToolTipText = "Show Normal Map";
            tsbToggleNormals.Click += tsbToggleNormals_Click;
            // 
            // tsbRotateSpritesUp
            // 
            tsbRotateSpritesUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbRotateSpritesUp.Image = Properties.Resources.SpritesPointingUpBmp;
            tsbRotateSpritesUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbRotateSpritesUp.Name = "tsbRotateSpritesUp";
            tsbRotateSpritesUp.Size = new System.Drawing.Size(28, 28);
            tsbRotateSpritesUp.Text = "Rotate Sprites Up to Camera";
            tsbRotateSpritesUp.Click += tsbRotateSpritesUp_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // tsbCameraReset
            // 
            tsbCameraReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbCameraReset.Image = Properties.Resources.CameraFromFrontBmp;
            tsbCameraReset.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbCameraReset.Name = "tsbCameraReset";
            tsbCameraReset.Size = new System.Drawing.Size(28, 28);
            tsbCameraReset.Text = "Reset Camera";
            tsbCameraReset.Click += tsbCameraReset_Click;
            // 
            // tsbCameraTopView
            // 
            tsbCameraTopView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbCameraTopView.Image = Properties.Resources.CameraFromTopBmp;
            tsbCameraTopView.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbCameraTopView.Name = "tsbCameraTopView";
            tsbCameraTopView.Size = new System.Drawing.Size(28, 28);
            tsbCameraTopView.Text = "Reset Camera to Top View";
            tsbCameraTopView.Click += tsbCameraTopView_Click;
            // 
            // tsbCameraLookAtCenter
            // 
            tsbCameraLookAtCenter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbCameraLookAtCenter.Image = Properties.Resources.CameraPointToCenterBmp;
            tsbCameraLookAtCenter.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbCameraLookAtCenter.Name = "tsbCameraLookAtCenter";
            tsbCameraLookAtCenter.Size = new System.Drawing.Size(28, 28);
            tsbCameraLookAtCenter.Text = "Look at Map Center";
            tsbCameraLookAtCenter.Click += tsbCameraLookAtCenter_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(6, 31);
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
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(6, 31);
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
        private System.Windows.Forms.ToolStripButton tsbToggleNormals;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbRecalculateLightmapOriginalMath;
        private System.Windows.Forms.ToolStripButton tsbUpdateLightmapUpdatedMath;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private TilePropertiesControl tilePropertyControl1;
        private MPD_ViewerGLControl mpdViewerGLControl1;
        private System.Windows.Forms.ToolStripButton tsbToggleTerrainType;
        private System.Windows.Forms.ToolStripButton tsbToggleEventID;
        private System.Windows.Forms.ToolStripButton tsbToggleBoundaries;
        private System.Windows.Forms.ToolStripButton tsbDrawSurfaceModel;
        private System.Windows.Forms.ToolStripButton tsbDrawModels;
        private System.Windows.Forms.ToolStripButton tsbDrawGround;
        private System.Windows.Forms.ToolStripButton tsbDrawSkyBox;
        private System.Windows.Forms.ToolStripButton tsbDrawGradients;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton tsbRunAnimations;
        private System.Windows.Forms.ToolStripButton tsbRotateSpritesUp;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbCameraReset;
        private System.Windows.Forms.ToolStripButton tsbCameraTopView;
        private System.Windows.Forms.ToolStripButton tsbCameraLookAtCenter;
        private System.Windows.Forms.ToolStripButton tsbCursorSelect;
        private System.Windows.Forms.ToolStripButton tsbCursorNavigate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
    }
}
