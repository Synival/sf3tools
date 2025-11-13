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
            toolStrip1 = new DarkModeToolStrip();
            tsbCursorSelect = new System.Windows.Forms.ToolStripButton();
            tsbCursorNavigate = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            tsbDrawSurfaceModel = new System.Windows.Forms.ToolStripButton();
            tsbDrawModels = new System.Windows.Forms.ToolStripButton();
            tsbDrawGround = new System.Windows.Forms.ToolStripButton();
            tsbDrawSkyBox = new System.Windows.Forms.ToolStripButton();
            tsbRunAnimations = new System.Windows.Forms.ToolStripButton();
            tsbApplyLighting = new System.Windows.Forms.ToolStripButton();
            tsbDrawGradients = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            tsbToggleWireframe = new System.Windows.Forms.ToolStripButton();
            tsbToggleBoundaries = new System.Windows.Forms.ToolStripButton();
            tsbToggleTerrainType = new System.Windows.Forms.ToolStripButton();
            tsbToggleEventID = new System.Windows.Forms.ToolStripButton();
            tsbToggleCollisions = new System.Windows.Forms.ToolStripButton();
            tsbHideModelsNotFacingCamera = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            tsbRenderOnBlackBackground = new System.Windows.Forms.ToolStripButton();
            tsbToggleNormals = new System.Windows.Forms.ToolStripButton();
            tsbRotateSpritesUp = new System.Windows.Forms.ToolStripButton();
            tsbApplyShadowTags = new System.Windows.Forms.ToolStripButton();
            tsbApplyHideTags = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            tsbCameraReset = new System.Windows.Forms.ToolStripButton();
            tsbCameraTopView = new System.Windows.Forms.ToolStripButton();
            tsbCameraLookAtCenter = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            tsbToggleHelp = new System.Windows.Forms.ToolStripButton();
            tilePropertyControl1 = new TilePropertiesControl();
            mpdViewerGLControl1 = new MPD_ViewerGLControl();
            toolStrip2 = new DarkModeToolStrip();
            tsbDrawGrassland = new System.Windows.Forms.ToolStripButton();
            tsbDrawDirt = new System.Windows.Forms.ToolStripButton();
            tsbDrawDarkGrass = new System.Windows.Forms.ToolStripButton();
            tsbDrawForest = new System.Windows.Forms.ToolStripButton();
            tsbDrawBrownMountain = new System.Windows.Forms.ToolStripButton();
            tsbDrawGreyMountain = new System.Windows.Forms.ToolStripButton();
            tsbDrawMountainPeak = new System.Windows.Forms.ToolStripButton();
            tsbDrawDesert = new System.Windows.Forms.ToolStripButton();
            tsbDrawRiver = new System.Windows.Forms.ToolStripButton();
            tsbDrawBridge = new System.Windows.Forms.ToolStripButton();
            tsbDrawWater = new System.Windows.Forms.ToolStripButton();
            tsbDrawNoEntry = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            tsbFixTiles = new System.Windows.Forms.ToolStripButton();
            toolStrip1.SuspendLayout();
            toolStrip2.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsbCursorSelect, tsbCursorNavigate, toolStripSeparator5, tsbDrawSurfaceModel, tsbDrawModels, tsbDrawGround, tsbDrawSkyBox, tsbRunAnimations, tsbApplyLighting, tsbDrawGradients, toolStripSeparator2, tsbToggleWireframe, tsbToggleBoundaries, tsbToggleTerrainType, tsbToggleEventID, tsbToggleCollisions, tsbHideModelsNotFacingCamera, tsbApplyShadowTags, tsbApplyHideTags, toolStripSeparator4, tsbRenderOnBlackBackground, tsbToggleNormals, tsbRotateSpritesUp, toolStripSeparator1, tsbCameraReset, tsbCameraTopView, tsbCameraLookAtCenter, toolStripSeparator3, tsbToggleHelp });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(789, 31);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            // 
            // tsbCursorSelect
            // 
            tsbCursorSelect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbCursorSelect.Image = Properties.Resources.CursorSelectBmp;
            tsbCursorSelect.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbCursorSelect.Name = "tsbCursorSelect";
            tsbCursorSelect.Size = new System.Drawing.Size(28, 28);
            tsbCursorSelect.Text = "Select";
            tsbCursorSelect.ToolTipText = "Select (E)";
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
            tsbCursorNavigate.ToolTipText = "Navigate (N)";
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
            // tsbApplyLighting
            // 
            tsbApplyLighting.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbApplyLighting.Image = Properties.Resources.ShowLightingBmp;
            tsbApplyLighting.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbApplyLighting.Name = "tsbApplyLighting";
            tsbApplyLighting.Size = new System.Drawing.Size(28, 28);
            tsbApplyLighting.Text = "Apply Lighting";
            tsbApplyLighting.Click += tsbApplyLighting_Click;
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
            // tsbToggleCollisions
            // 
            tsbToggleCollisions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbToggleCollisions.Image = Properties.Resources.ShowCollisionsBmp;
            tsbToggleCollisions.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbToggleCollisions.Name = "tsbToggleCollisions";
            tsbToggleCollisions.Size = new System.Drawing.Size(28, 28);
            tsbToggleCollisions.Text = "Draw Collision Lines";
            tsbToggleCollisions.Click += tsbToggleCollisions_Click;
            // 
            // tsbHideModelsNotFacingCamera
            // 
            tsbHideModelsNotFacingCamera.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbHideModelsNotFacingCamera.Image = Properties.Resources.HideModelsFacingAwayBmp;
            tsbHideModelsNotFacingCamera.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbHideModelsNotFacingCamera.Name = "tsbHideModelsNotFacingCamera";
            tsbHideModelsNotFacingCamera.Size = new System.Drawing.Size(28, 28);
            tsbHideModelsNotFacingCamera.Text = "Hide Models Not Facing Camera";
            tsbHideModelsNotFacingCamera.Click += tsbHideModelsNotFacingCamera_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(6, 31);
            // 
            // tsbRenderOnBlackBackground
            // 
            tsbRenderOnBlackBackground.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbRenderOnBlackBackground.Image = Properties.Resources.RenderOnBlackBackgroundBmp;
            tsbRenderOnBlackBackground.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbRenderOnBlackBackground.Name = "tsbRenderOnBlackBackground";
            tsbRenderOnBlackBackground.Size = new System.Drawing.Size(28, 28);
            tsbRenderOnBlackBackground.Text = "Render on Black Background";
            tsbRenderOnBlackBackground.Click += tsbRenderOnBlackBackground_Click;
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
            // tsbApplyShadowTags
            // 
            tsbApplyShadowTags.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbApplyShadowTags.Image = Properties.Resources.ApplyShadowTagsBmp;
            tsbApplyShadowTags.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbApplyShadowTags.Name = "tsbApplyShadowTags";
            tsbApplyShadowTags.Size = new System.Drawing.Size(28, 28);
            tsbApplyShadowTags.Text = "Apply Shadow Tags";
            tsbApplyShadowTags.Click += tsbApplyShadowTags_Click;
            // 
            // tsbApplyHideTags
            // 
            tsbApplyHideTags.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbApplyHideTags.Image = Properties.Resources.ApplyHideTagsBmp;
            tsbApplyHideTags.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbApplyHideTags.Name = "tsbApplyHideTags";
            tsbApplyHideTags.Size = new System.Drawing.Size(28, 28);
            tsbApplyHideTags.Text = "Apply Hide Tags";
            tsbApplyHideTags.Click += tsbApplyHideTags_Click;
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
            tilePropertyControl1.Location = new System.Drawing.Point(582, 62);
            tilePropertyControl1.MaximumSize = new System.Drawing.Size(207, 10000);
            tilePropertyControl1.MinimumSize = new System.Drawing.Size(207, 446);
            tilePropertyControl1.Name = "tilePropertyControl1";
            tilePropertyControl1.Size = new System.Drawing.Size(207, 514);
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
            mpdViewerGLControl1.Location = new System.Drawing.Point(0, 62);
            mpdViewerGLControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            mpdViewerGLControl1.MinimumSize = new System.Drawing.Size(320, 240);
            mpdViewerGLControl1.Name = "mpdViewerGLControl1";
            mpdViewerGLControl1.Profile = OpenTK.Windowing.Common.ContextProfile.Core;
            mpdViewerGLControl1.SharedContext = null;
            mpdViewerGLControl1.Size = new System.Drawing.Size(582, 514);
            mpdViewerGLControl1.TabIndex = 1;
            // 
            // toolStrip2
            // 
            toolStrip2.ImageScalingSize = new System.Drawing.Size(24, 24);
            toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsbDrawGrassland, tsbDrawDirt, tsbDrawDarkGrass, tsbDrawForest, tsbDrawBrownMountain, tsbDrawGreyMountain, tsbDrawMountainPeak, tsbDrawDesert, tsbDrawRiver, tsbDrawBridge, tsbDrawWater, tsbDrawNoEntry, toolStripSeparator6, tsbFixTiles });
            toolStrip2.Location = new System.Drawing.Point(0, 31);
            toolStrip2.Name = "toolStrip2";
            toolStrip2.Size = new System.Drawing.Size(789, 31);
            toolStrip2.TabIndex = 3;
            toolStrip2.Text = "toolStrip2";
            toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            // 
            // tsbDrawGrassland
            // 
            tsbDrawGrassland.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbDrawGrassland.Image = (System.Drawing.Image) resources.GetObject("tsbDrawGrassland.Image");
            tsbDrawGrassland.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbDrawGrassland.Name = "tsbDrawGrassland";
            tsbDrawGrassland.Size = new System.Drawing.Size(28, 28);
            tsbDrawGrassland.Text = "Draw Grassland";
            tsbDrawGrassland.ToolTipText = "Draw Grassland (1)";
            tsbDrawGrassland.Click += tsbDrawGrassland_Click;
            // 
            // tsbDrawDirt
            // 
            tsbDrawDirt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbDrawDirt.Image = (System.Drawing.Image) resources.GetObject("tsbDrawDirt.Image");
            tsbDrawDirt.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbDrawDirt.Name = "tsbDrawDirt";
            tsbDrawDirt.Size = new System.Drawing.Size(28, 28);
            tsbDrawDirt.Text = "Draw Dirt";
            tsbDrawDirt.ToolTipText = "Draw Dirt (2)";
            tsbDrawDirt.Click += tsbDrawDirt_Click;
            // 
            // tsbDrawDarkGrass
            // 
            tsbDrawDarkGrass.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbDrawDarkGrass.Image = (System.Drawing.Image) resources.GetObject("tsbDrawDarkGrass.Image");
            tsbDrawDarkGrass.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbDrawDarkGrass.Name = "tsbDrawDarkGrass";
            tsbDrawDarkGrass.Size = new System.Drawing.Size(28, 28);
            tsbDrawDarkGrass.Text = "Draw Dark Grass";
            tsbDrawDarkGrass.ToolTipText = "Draw Dark Grass (3)";
            tsbDrawDarkGrass.Click += tsbDrawDarkGrass_Click;
            // 
            // tsbDrawForest
            // 
            tsbDrawForest.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbDrawForest.Image = (System.Drawing.Image) resources.GetObject("tsbDrawForest.Image");
            tsbDrawForest.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbDrawForest.Name = "tsbDrawForest";
            tsbDrawForest.Size = new System.Drawing.Size(28, 28);
            tsbDrawForest.Text = "Draw Forest";
            tsbDrawForest.ToolTipText = "Draw Forest (4)";
            tsbDrawForest.Click += tsbDrawForest_Click;
            // 
            // tsbDrawBrownMountain
            // 
            tsbDrawBrownMountain.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbDrawBrownMountain.Image = (System.Drawing.Image) resources.GetObject("tsbDrawBrownMountain.Image");
            tsbDrawBrownMountain.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbDrawBrownMountain.Name = "tsbDrawBrownMountain";
            tsbDrawBrownMountain.Size = new System.Drawing.Size(28, 28);
            tsbDrawBrownMountain.Text = "Draw Brown Mountain";
            tsbDrawBrownMountain.ToolTipText = "Draw Brown Mountain (5)";
            tsbDrawBrownMountain.Click += tsbDrawBrownMountain_Click;
            // 
            // tsbDrawGreyMountain
            // 
            tsbDrawGreyMountain.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbDrawGreyMountain.Image = (System.Drawing.Image) resources.GetObject("tsbDrawGreyMountain.Image");
            tsbDrawGreyMountain.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbDrawGreyMountain.Name = "tsbDrawGreyMountain";
            tsbDrawGreyMountain.Size = new System.Drawing.Size(28, 28);
            tsbDrawGreyMountain.Text = "Draw Grey Mountain";
            tsbDrawGreyMountain.ToolTipText = "Draw Grey Mountain (6)";
            tsbDrawGreyMountain.Click += tsbDrawGreyMountain_Click;
            // 
            // tsbDrawMountainPeak
            // 
            tsbDrawMountainPeak.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbDrawMountainPeak.Image = (System.Drawing.Image) resources.GetObject("tsbDrawMountainPeak.Image");
            tsbDrawMountainPeak.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbDrawMountainPeak.Name = "tsbDrawMountainPeak";
            tsbDrawMountainPeak.Size = new System.Drawing.Size(28, 28);
            tsbDrawMountainPeak.Text = "Draw Mountain Peak";
            tsbDrawMountainPeak.ToolTipText = "Draw Mountain Peak (7)";
            tsbDrawMountainPeak.Click += tsbDrawMountainPeak_Click;
            // 
            // tsbDrawDesert
            // 
            tsbDrawDesert.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbDrawDesert.Image = (System.Drawing.Image) resources.GetObject("tsbDrawDesert.Image");
            tsbDrawDesert.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbDrawDesert.Name = "tsbDrawDesert";
            tsbDrawDesert.Size = new System.Drawing.Size(28, 28);
            tsbDrawDesert.Text = "Draw Desert";
            tsbDrawDesert.ToolTipText = "Draw Desert (8)";
            tsbDrawDesert.Click += tsbDrawDesert_Click;
            // 
            // tsbDrawRiver
            // 
            tsbDrawRiver.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbDrawRiver.Image = (System.Drawing.Image) resources.GetObject("tsbDrawRiver.Image");
            tsbDrawRiver.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbDrawRiver.Name = "tsbDrawRiver";
            tsbDrawRiver.Size = new System.Drawing.Size(28, 28);
            tsbDrawRiver.Text = "Draw River";
            tsbDrawRiver.ToolTipText = "Draw River (9)";
            tsbDrawRiver.Click += tsbDrawRiver_Click;
            // 
            // tsbDrawBridge
            // 
            tsbDrawBridge.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbDrawBridge.Image = (System.Drawing.Image) resources.GetObject("tsbDrawBridge.Image");
            tsbDrawBridge.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbDrawBridge.Name = "tsbDrawBridge";
            tsbDrawBridge.Size = new System.Drawing.Size(28, 28);
            tsbDrawBridge.Text = "Draw Bridge";
            tsbDrawBridge.ToolTipText = "Draw Bridge (0)";
            tsbDrawBridge.Click += tsbDrawBridge_Click;
            // 
            // tsbDrawWater
            // 
            tsbDrawWater.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbDrawWater.Image = (System.Drawing.Image) resources.GetObject("tsbDrawWater.Image");
            tsbDrawWater.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbDrawWater.Name = "tsbDrawWater";
            tsbDrawWater.Size = new System.Drawing.Size(28, 28);
            tsbDrawWater.Text = "Draw Water";
            tsbDrawWater.ToolTipText = "Draw Water (-)";
            tsbDrawWater.Click += tsbDrawWater_Click;
            // 
            // tsbDrawNoEntry
            // 
            tsbDrawNoEntry.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbDrawNoEntry.Image = (System.Drawing.Image) resources.GetObject("tsbDrawNoEntry.Image");
            tsbDrawNoEntry.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbDrawNoEntry.Name = "tsbDrawNoEntry";
            tsbDrawNoEntry.Size = new System.Drawing.Size(28, 28);
            tsbDrawNoEntry.Text = "Draw NoEntry";
            tsbDrawNoEntry.ToolTipText = "Draw NoEntry (=)";
            tsbDrawNoEntry.Click += tsbDrawNoEntry_Click;
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new System.Drawing.Size(6, 31);
            // 
            // tsbFixTiles
            // 
            tsbFixTiles.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsbFixTiles.Image = (System.Drawing.Image) resources.GetObject("tsbFixTiles.Image");
            tsbFixTiles.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbFixTiles.Name = "tsbFixTiles";
            tsbFixTiles.Size = new System.Drawing.Size(28, 28);
            tsbFixTiles.Text = "Fix All Tile Textures Based on Neighbors\n(You'll probably want to save first!!)";
            tsbFixTiles.Click += tsbFixTiles_Click;
            // 
            // MPD_ViewerControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(mpdViewerGLControl1);
            Controls.Add(tilePropertyControl1);
            Controls.Add(toolStrip2);
            Controls.Add(toolStrip1);
            Name = "MPD_ViewerControl";
            Size = new System.Drawing.Size(789, 576);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            toolStrip2.ResumeLayout(false);
            toolStrip2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DarkModeToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbToggleWireframe;
        private System.Windows.Forms.ToolStripButton tsbToggleHelp;
        private System.Windows.Forms.ToolStripButton tsbToggleNormals;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
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
        private System.Windows.Forms.ToolStripButton tsbRunAnimations;
        private System.Windows.Forms.ToolStripButton tsbRotateSpritesUp;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbCameraReset;
        private System.Windows.Forms.ToolStripButton tsbCameraTopView;
        private System.Windows.Forms.ToolStripButton tsbCameraLookAtCenter;
        private System.Windows.Forms.ToolStripButton tsbCursorSelect;
        private System.Windows.Forms.ToolStripButton tsbCursorNavigate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton tsbApplyLighting;
        private System.Windows.Forms.ToolStripButton tsbToggleCollisions;
        private DarkModeToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton tsbDrawNoEntry;
        private System.Windows.Forms.ToolStripButton tsbDrawWater;
        private System.Windows.Forms.ToolStripButton tsbDrawRiver;
        private System.Windows.Forms.ToolStripButton tsbDrawGrassland;
        private System.Windows.Forms.ToolStripButton tsbDrawDirt;
        private System.Windows.Forms.ToolStripButton tsbDrawDarkGrass;
        private System.Windows.Forms.ToolStripButton tsbDrawForest;
        private System.Windows.Forms.ToolStripButton tsbDrawBrownMountain;
        private System.Windows.Forms.ToolStripButton tsbDrawGreyMountain;
        private System.Windows.Forms.ToolStripButton tsbDrawMountainPeak;
        private System.Windows.Forms.ToolStripButton tsbDrawDesert;
        private System.Windows.Forms.ToolStripButton tsbDrawBridge;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton tsbFixTiles;
        private System.Windows.Forms.ToolStripButton tsbHideModelsNotFacingCamera;
        private System.Windows.Forms.ToolStripButton tsbRenderOnBlackBackground;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton tsbApplyShadowTags;
        private System.Windows.Forms.ToolStripButton tsbApplyHideTags;
    }
}
