namespace SF3.Win.Controls {
    partial class MPD_FlagEditor {
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
            cbModifyPalette1ForGradient = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbChunk1IsLoadedFromLowMemory = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbChunk1IsLoadedFromHighMemory = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbHasAnySkyBox = new CommonLib.Win.Controls.DarkModeCheckBox();
            labelGroundImageType = new CommonLib.Win.Controls.DarkModeLabel();
            cbGroundImageType = new CommonLib.Win.Controls.DarkModeComboBox();
            labelBackgroundImageType = new CommonLib.Win.Controls.DarkModeLabel();
            cbBackgroundImageType = new CommonLib.Win.Controls.DarkModeComboBox();
            cbUnknownMapFlag0x0001 = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbUnknownMapFlag0x0002 = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbUnknownMapFlag0x0020 = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbHasChunk19Model = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbHasModels = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbHasCutsceneSkyBox = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbHasBattleSkyBox = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbHasExtraChunk1ModelWithChunk21Textures = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbChunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbChunk1IsModels = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbChunk20IsModels = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbHighMemoryHasModels = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbHasSurfaceTextureRotation = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbAddDotProductBasedNoiseToStandardLightmap = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbKeepTexturelessFlatTiles = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbHasSurfaceModel = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbNarrowAngleBasedLightmap = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbChunk20IsSurfaceModelIfExists = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbChunk2IsSurfaceModel = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbChunk20IsSurfaceModel = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbHighMemoryHasSurfaceModel = new CommonLib.Win.Controls.DarkModeCheckBox();
            gbSurface = new CommonLib.Win.Controls.DarkModeGroupBox();
            labelSeparator2 = new CommonLib.Win.Controls.DarkModeLabel();
            labelSeparator1 = new CommonLib.Win.Controls.DarkModeLabel();
            gbModels = new CommonLib.Win.Controls.DarkModeGroupBox();
            labelSeparator3 = new CommonLib.Win.Controls.DarkModeLabel();
            gbExtraModels = new CommonLib.Win.Controls.DarkModeGroupBox();
            gbOtherFlags = new CommonLib.Win.Controls.DarkModeGroupBox();
            gbPlanesBackgrounds = new CommonLib.Win.Controls.DarkModeGroupBox();
            labelSeparator4 = new CommonLib.Win.Controls.DarkModeLabel();
            darkModeGroupBox1 = new CommonLib.Win.Controls.DarkModeGroupBox();
            labelSeparator5 = new CommonLib.Win.Controls.DarkModeLabel();
            gbSurface.SuspendLayout();
            gbModels.SuspendLayout();
            gbExtraModels.SuspendLayout();
            gbOtherFlags.SuspendLayout();
            gbPlanesBackgrounds.SuspendLayout();
            darkModeGroupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // cbModifyPalette1ForGradient
            // 
            cbModifyPalette1ForGradient.AutoSize = true;
            cbModifyPalette1ForGradient.Location = new System.Drawing.Point(6, 22);
            cbModifyPalette1ForGradient.Name = "cbModifyPalette1ForGradient";
            cbModifyPalette1ForGradient.Size = new System.Drawing.Size(219, 19);
            cbModifyPalette1ForGradient.TabIndex = 1;
            cbModifyPalette1ForGradient.Text = "Modify Palette1 for Gradient (Scn2+)";
            cbModifyPalette1ForGradient.UseVisualStyleBackColor = true;
            // 
            // cbChunk1IsLoadedFromLowMemory
            // 
            cbChunk1IsLoadedFromLowMemory.AutoSize = true;
            cbChunk1IsLoadedFromLowMemory.Enabled = false;
            cbChunk1IsLoadedFromLowMemory.Location = new System.Drawing.Point(6, 86);
            cbChunk1IsLoadedFromLowMemory.Name = "cbChunk1IsLoadedFromLowMemory";
            cbChunk1IsLoadedFromLowMemory.Size = new System.Drawing.Size(267, 19);
            cbChunk1IsLoadedFromLowMemory.TabIndex = 1;
            cbChunk1IsLoadedFromLowMemory.Text = "(Auto) Chunk[1] Is Loaded from Low Memory";
            cbChunk1IsLoadedFromLowMemory.UseVisualStyleBackColor = true;
            // 
            // cbChunk1IsLoadedFromHighMemory
            // 
            cbChunk1IsLoadedFromHighMemory.AutoSize = true;
            cbChunk1IsLoadedFromHighMemory.Enabled = false;
            cbChunk1IsLoadedFromHighMemory.Location = new System.Drawing.Point(6, 108);
            cbChunk1IsLoadedFromHighMemory.Name = "cbChunk1IsLoadedFromHighMemory";
            cbChunk1IsLoadedFromHighMemory.Size = new System.Drawing.Size(271, 19);
            cbChunk1IsLoadedFromHighMemory.TabIndex = 1;
            cbChunk1IsLoadedFromHighMemory.Text = "(Auto) Chunk[1] Is Loaded from High Memory";
            cbChunk1IsLoadedFromHighMemory.UseVisualStyleBackColor = true;
            // 
            // cbHasAnySkyBox
            // 
            cbHasAnySkyBox.AutoSize = true;
            cbHasAnySkyBox.Enabled = false;
            cbHasAnySkyBox.Location = new System.Drawing.Point(6, 128);
            cbHasAnySkyBox.Name = "cbHasAnySkyBox";
            cbHasAnySkyBox.Size = new System.Drawing.Size(186, 19);
            cbHasAnySkyBox.TabIndex = 1;
            cbHasAnySkyBox.Text = "(Auto) Has any Sky Box Chunk";
            cbHasAnySkyBox.UseVisualStyleBackColor = true;
            // 
            // labelGroundImageType
            // 
            labelGroundImageType.AutoSize = true;
            labelGroundImageType.DisabledColor = System.Drawing.Color.Empty;
            labelGroundImageType.IsSeparator = false;
            labelGroundImageType.Location = new System.Drawing.Point(6, 49);
            labelGroundImageType.Name = "labelGroundImageType";
            labelGroundImageType.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            labelGroundImageType.Size = new System.Drawing.Size(114, 15);
            labelGroundImageType.TabIndex = 5;
            labelGroundImageType.Text = "Ground Image Type:";
            // 
            // cbGroundImageType
            // 
            cbGroundImageType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            cbGroundImageType.FormattingEnabled = true;
            cbGroundImageType.Location = new System.Drawing.Point(150, 46);
            cbGroundImageType.Name = "cbGroundImageType";
            cbGroundImageType.Size = new System.Drawing.Size(157, 24);
            cbGroundImageType.TabIndex = 4;
            // 
            // labelBackgroundImageType
            // 
            labelBackgroundImageType.AutoSize = true;
            labelBackgroundImageType.DisabledColor = System.Drawing.Color.Empty;
            labelBackgroundImageType.IsSeparator = false;
            labelBackgroundImageType.Location = new System.Drawing.Point(6, 19);
            labelBackgroundImageType.Name = "labelBackgroundImageType";
            labelBackgroundImageType.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            labelBackgroundImageType.Size = new System.Drawing.Size(138, 15);
            labelBackgroundImageType.TabIndex = 3;
            labelBackgroundImageType.Text = "Background Image Type:";
            // 
            // cbBackgroundImageType
            // 
            cbBackgroundImageType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            cbBackgroundImageType.FormattingEnabled = true;
            cbBackgroundImageType.Location = new System.Drawing.Point(150, 16);
            cbBackgroundImageType.Name = "cbBackgroundImageType";
            cbBackgroundImageType.Size = new System.Drawing.Size(157, 24);
            cbBackgroundImageType.TabIndex = 2;
            // 
            // cbUnknownMapFlag0x0001
            // 
            cbUnknownMapFlag0x0001.AutoSize = true;
            cbUnknownMapFlag0x0001.Location = new System.Drawing.Point(6, 47);
            cbUnknownMapFlag0x0001.Name = "cbUnknownMapFlag0x0001";
            cbUnknownMapFlag0x0001.Size = new System.Drawing.Size(157, 19);
            cbUnknownMapFlag0x0001.TabIndex = 0;
            cbUnknownMapFlag0x0001.Text = "Unknown Flag 1 (0x0001)";
            cbUnknownMapFlag0x0001.UseVisualStyleBackColor = true;
            // 
            // cbUnknownMapFlag0x0002
            // 
            cbUnknownMapFlag0x0002.AutoSize = true;
            cbUnknownMapFlag0x0002.Location = new System.Drawing.Point(6, 72);
            cbUnknownMapFlag0x0002.Name = "cbUnknownMapFlag0x0002";
            cbUnknownMapFlag0x0002.Size = new System.Drawing.Size(202, 19);
            cbUnknownMapFlag0x0002.TabIndex = 1;
            cbUnknownMapFlag0x0002.Text = "Unknown Flag 2 (0x0002) (Scn1,2)";
            cbUnknownMapFlag0x0002.UseVisualStyleBackColor = true;
            // 
            // cbUnknownMapFlag0x0020
            // 
            cbUnknownMapFlag0x0020.AutoSize = true;
            cbUnknownMapFlag0x0020.Location = new System.Drawing.Point(6, 97);
            cbUnknownMapFlag0x0020.Name = "cbUnknownMapFlag0x0020";
            cbUnknownMapFlag0x0020.Size = new System.Drawing.Size(157, 19);
            cbUnknownMapFlag0x0020.TabIndex = 1;
            cbUnknownMapFlag0x0020.Text = "Unknown Flag 3 (0x0020)";
            cbUnknownMapFlag0x0020.UseVisualStyleBackColor = true;
            // 
            // cbHasChunk19Model
            // 
            cbHasChunk19Model.AutoSize = true;
            cbHasChunk19Model.Location = new System.Drawing.Point(6, 22);
            cbHasChunk19Model.Name = "cbHasChunk19Model";
            cbHasChunk19Model.Size = new System.Drawing.Size(205, 19);
            cbHasChunk19Model.TabIndex = 1;
            cbHasChunk19Model.Text = "Has Extra Chunk[19] Model (Scn1)";
            cbHasChunk19Model.UseVisualStyleBackColor = true;
            // 
            // cbHasModels
            // 
            cbHasModels.AutoSize = true;
            cbHasModels.Location = new System.Drawing.Point(6, 22);
            cbHasModels.Name = "cbHasModels";
            cbHasModels.Size = new System.Drawing.Size(88, 19);
            cbHasModels.TabIndex = 1;
            cbHasModels.Text = "Has Models";
            cbHasModels.UseVisualStyleBackColor = true;
            // 
            // cbHasCutsceneSkyBox
            // 
            cbHasCutsceneSkyBox.AutoSize = true;
            cbHasCutsceneSkyBox.Location = new System.Drawing.Point(6, 101);
            cbHasCutsceneSkyBox.Name = "cbHasCutsceneSkyBox";
            cbHasCutsceneSkyBox.Size = new System.Drawing.Size(185, 19);
            cbHasCutsceneSkyBox.TabIndex = 1;
            cbHasCutsceneSkyBox.Text = "Has Cutscene Sky Box (Scn2+)";
            cbHasCutsceneSkyBox.UseVisualStyleBackColor = true;
            // 
            // cbHasBattleSkyBox
            // 
            cbHasBattleSkyBox.AutoSize = true;
            cbHasBattleSkyBox.Location = new System.Drawing.Point(6, 76);
            cbHasBattleSkyBox.Name = "cbHasBattleSkyBox";
            cbHasBattleSkyBox.Size = new System.Drawing.Size(158, 19);
            cbHasBattleSkyBox.TabIndex = 1;
            cbHasBattleSkyBox.Text = "Has Battle Sky Box (Scn1)";
            cbHasBattleSkyBox.UseVisualStyleBackColor = true;
            // 
            // cbHasExtraChunk1ModelWithChunk21Textures
            // 
            cbHasExtraChunk1ModelWithChunk21Textures.AutoSize = true;
            cbHasExtraChunk1ModelWithChunk21Textures.Location = new System.Drawing.Point(6, 47);
            cbHasExtraChunk1ModelWithChunk21Textures.Name = "cbHasExtraChunk1ModelWithChunk21Textures";
            cbHasExtraChunk1ModelWithChunk21Textures.Size = new System.Drawing.Size(247, 34);
            cbHasExtraChunk1ModelWithChunk21Textures.TabIndex = 1;
            cbHasExtraChunk1ModelWithChunk21Textures.Text = "Has Extra Chunk[1] Model with Chunk[21]\r\n    Textures (Scn2+)";
            cbHasExtraChunk1ModelWithChunk21Textures.UseVisualStyleBackColor = true;
            // 
            // cbChunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists
            // 
            cbChunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists.AutoSize = true;
            cbChunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists.Location = new System.Drawing.Point(6, 22);
            cbChunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists.Name = "cbChunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists";
            cbChunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists.Size = new System.Drawing.Size(262, 34);
            cbChunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists.TabIndex = 1;
            cbChunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists.Text = "Chunk[1] Is Still Loaded from Low Memory if\r\n    Surface Model Exists (Scn1)";
            cbChunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists.UseVisualStyleBackColor = true;
            // 
            // cbChunk1IsModels
            // 
            cbChunk1IsModels.AutoSize = true;
            cbChunk1IsModels.Enabled = false;
            cbChunk1IsModels.Location = new System.Drawing.Point(6, 49);
            cbChunk1IsModels.Name = "cbChunk1IsModels";
            cbChunk1IsModels.Size = new System.Drawing.Size(165, 19);
            cbChunk1IsModels.TabIndex = 1;
            cbChunk1IsModels.Text = "(Auto) Chunk[1] Is Models";
            cbChunk1IsModels.UseVisualStyleBackColor = true;
            // 
            // cbChunk20IsModels
            // 
            cbChunk20IsModels.AutoSize = true;
            cbChunk20IsModels.Enabled = false;
            cbChunk20IsModels.Location = new System.Drawing.Point(6, 74);
            cbChunk20IsModels.Name = "cbChunk20IsModels";
            cbChunk20IsModels.Size = new System.Drawing.Size(215, 19);
            cbChunk20IsModels.TabIndex = 1;
            cbChunk20IsModels.Text = "(Auto) Chunk[20] Is Models (Scn2+)";
            cbChunk20IsModels.UseVisualStyleBackColor = true;
            // 
            // cbHighMemoryHasModels
            // 
            cbHighMemoryHasModels.AutoSize = true;
            cbHighMemoryHasModels.Enabled = false;
            cbHighMemoryHasModels.Location = new System.Drawing.Point(6, 99);
            cbHighMemoryHasModels.Name = "cbHighMemoryHasModels";
            cbHighMemoryHasModels.Size = new System.Drawing.Size(202, 19);
            cbHighMemoryHasModels.TabIndex = 1;
            cbHighMemoryHasModels.Text = "(Auto) High Memory Has Models";
            cbHighMemoryHasModels.UseVisualStyleBackColor = true;
            // 
            // cbHasSurfaceTextureRotation
            // 
            cbHasSurfaceTextureRotation.AutoSize = true;
            cbHasSurfaceTextureRotation.Location = new System.Drawing.Point(6, 99);
            cbHasSurfaceTextureRotation.Name = "cbHasSurfaceTextureRotation";
            cbHasSurfaceTextureRotation.Size = new System.Drawing.Size(221, 19);
            cbHasSurfaceTextureRotation.TabIndex = 1;
            cbHasSurfaceTextureRotation.Text = "Has Surface Texture Rotation (Scn3+)";
            cbHasSurfaceTextureRotation.UseVisualStyleBackColor = true;
            // 
            // cbAddDotProductBasedNoiseToStandardLightmap
            // 
            cbAddDotProductBasedNoiseToStandardLightmap.AutoSize = true;
            cbAddDotProductBasedNoiseToStandardLightmap.Location = new System.Drawing.Point(6, 49);
            cbAddDotProductBasedNoiseToStandardLightmap.Name = "cbAddDotProductBasedNoiseToStandardLightmap";
            cbAddDotProductBasedNoiseToStandardLightmap.Size = new System.Drawing.Size(294, 19);
            cbAddDotProductBasedNoiseToStandardLightmap.TabIndex = 1;
            cbAddDotProductBasedNoiseToStandardLightmap.Text = "Add Dot Product-Based Noise to Surface Lightmap";
            cbAddDotProductBasedNoiseToStandardLightmap.UseVisualStyleBackColor = true;
            // 
            // cbKeepTexturelessFlatTiles
            // 
            cbKeepTexturelessFlatTiles.AutoSize = true;
            cbKeepTexturelessFlatTiles.Location = new System.Drawing.Point(6, 124);
            cbKeepTexturelessFlatTiles.Name = "cbKeepTexturelessFlatTiles";
            cbKeepTexturelessFlatTiles.Size = new System.Drawing.Size(161, 19);
            cbKeepTexturelessFlatTiles.TabIndex = 1;
            cbKeepTexturelessFlatTiles.Text = "Keep Textureless Flat Tiles";
            cbKeepTexturelessFlatTiles.UseVisualStyleBackColor = true;
            // 
            // cbHasSurfaceModel
            // 
            cbHasSurfaceModel.AutoSize = true;
            cbHasSurfaceModel.Location = new System.Drawing.Point(6, 22);
            cbHasSurfaceModel.Name = "cbHasSurfaceModel";
            cbHasSurfaceModel.Size = new System.Drawing.Size(125, 19);
            cbHasSurfaceModel.TabIndex = 1;
            cbHasSurfaceModel.Text = "Has Surface Model";
            cbHasSurfaceModel.UseVisualStyleBackColor = true;
            // 
            // cbNarrowAngleBasedLightmap
            // 
            cbNarrowAngleBasedLightmap.AutoSize = true;
            cbNarrowAngleBasedLightmap.Location = new System.Drawing.Point(6, 74);
            cbNarrowAngleBasedLightmap.Name = "cbNarrowAngleBasedLightmap";
            cbNarrowAngleBasedLightmap.Size = new System.Drawing.Size(275, 19);
            cbNarrowAngleBasedLightmap.TabIndex = 1;
            cbNarrowAngleBasedLightmap.Text = "Narrow Angle-Based Surface Lightmap (Scn2+)";
            cbNarrowAngleBasedLightmap.UseVisualStyleBackColor = true;
            // 
            // cbChunk20IsSurfaceModelIfExists
            // 
            cbChunk20IsSurfaceModelIfExists.AutoSize = true;
            cbChunk20IsSurfaceModelIfExists.Enabled = false;
            cbChunk20IsSurfaceModelIfExists.Location = new System.Drawing.Point(6, 58);
            cbChunk20IsSurfaceModelIfExists.Name = "cbChunk20IsSurfaceModelIfExists";
            cbChunk20IsSurfaceModelIfExists.Size = new System.Drawing.Size(256, 19);
            cbChunk20IsSurfaceModelIfExists.TabIndex = 1;
            cbChunk20IsSurfaceModelIfExists.Text = "Chunk[20] Is Surface Model if Exists (Scn2+)";
            cbChunk20IsSurfaceModelIfExists.UseVisualStyleBackColor = true;
            // 
            // cbChunk2IsSurfaceModel
            // 
            cbChunk2IsSurfaceModel.AutoSize = true;
            cbChunk2IsSurfaceModel.Enabled = false;
            cbChunk2IsSurfaceModel.Location = new System.Drawing.Point(6, 151);
            cbChunk2IsSurfaceModel.Name = "cbChunk2IsSurfaceModel";
            cbChunk2IsSurfaceModel.Size = new System.Drawing.Size(202, 19);
            cbChunk2IsSurfaceModel.TabIndex = 1;
            cbChunk2IsSurfaceModel.Text = "(Auto) Chunk[2] Is Surface Model";
            cbChunk2IsSurfaceModel.UseVisualStyleBackColor = true;
            // 
            // cbChunk20IsSurfaceModel
            // 
            cbChunk20IsSurfaceModel.AutoSize = true;
            cbChunk20IsSurfaceModel.Enabled = false;
            cbChunk20IsSurfaceModel.Location = new System.Drawing.Point(6, 176);
            cbChunk20IsSurfaceModel.Name = "cbChunk20IsSurfaceModel";
            cbChunk20IsSurfaceModel.Size = new System.Drawing.Size(252, 19);
            cbChunk20IsSurfaceModel.TabIndex = 1;
            cbChunk20IsSurfaceModel.Text = "(Auto) Chunk[20] Is Surface Model (Scn2+)";
            cbChunk20IsSurfaceModel.UseVisualStyleBackColor = true;
            // 
            // cbHighMemoryHasSurfaceModel
            // 
            cbHighMemoryHasSurfaceModel.AutoSize = true;
            cbHighMemoryHasSurfaceModel.Enabled = false;
            cbHighMemoryHasSurfaceModel.Location = new System.Drawing.Point(6, 201);
            cbHighMemoryHasSurfaceModel.Name = "cbHighMemoryHasSurfaceModel";
            cbHighMemoryHasSurfaceModel.Size = new System.Drawing.Size(239, 19);
            cbHighMemoryHasSurfaceModel.TabIndex = 1;
            cbHighMemoryHasSurfaceModel.Text = "(Auto) High Memory Has Surface Model";
            cbHighMemoryHasSurfaceModel.UseVisualStyleBackColor = true;
            // 
            // gbSurface
            // 
            gbSurface.Controls.Add(labelSeparator2);
            gbSurface.Controls.Add(labelSeparator1);
            gbSurface.Controls.Add(cbHasSurfaceTextureRotation);
            gbSurface.Controls.Add(cbAddDotProductBasedNoiseToStandardLightmap);
            gbSurface.Controls.Add(cbKeepTexturelessFlatTiles);
            gbSurface.Controls.Add(cbHasSurfaceModel);
            gbSurface.Controls.Add(cbNarrowAngleBasedLightmap);
            gbSurface.Controls.Add(cbChunk2IsSurfaceModel);
            gbSurface.Controls.Add(cbChunk20IsSurfaceModel);
            gbSurface.Controls.Add(cbHighMemoryHasSurfaceModel);
            gbSurface.Location = new System.Drawing.Point(328, 3);
            gbSurface.Name = "gbSurface";
            gbSurface.Size = new System.Drawing.Size(313, 224);
            gbSurface.TabIndex = 1;
            gbSurface.TabStop = false;
            gbSurface.Text = "Surface";
            // 
            // labelSeparator2
            // 
            labelSeparator2.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            labelSeparator2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            labelSeparator2.DisabledColor = System.Drawing.Color.Empty;
            labelSeparator2.IsSeparator = true;
            labelSeparator2.Location = new System.Drawing.Point(6, 146);
            labelSeparator2.Name = "labelSeparator2";
            labelSeparator2.OriginalBorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            labelSeparator2.Size = new System.Drawing.Size(300, 2);
            labelSeparator2.TabIndex = 3;
            // 
            // labelSeparator1
            // 
            labelSeparator1.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            labelSeparator1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            labelSeparator1.DisabledColor = System.Drawing.Color.Empty;
            labelSeparator1.IsSeparator = true;
            labelSeparator1.Location = new System.Drawing.Point(6, 44);
            labelSeparator1.Name = "labelSeparator1";
            labelSeparator1.OriginalBorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            labelSeparator1.Size = new System.Drawing.Size(300, 2);
            labelSeparator1.TabIndex = 2;
            // 
            // gbModels
            // 
            gbModels.Controls.Add(labelSeparator3);
            gbModels.Controls.Add(cbHasModels);
            gbModels.Controls.Add(cbChunk1IsModels);
            gbModels.Controls.Add(cbChunk20IsModels);
            gbModels.Controls.Add(cbHighMemoryHasModels);
            gbModels.Location = new System.Drawing.Point(3, 3);
            gbModels.Name = "gbModels";
            gbModels.Size = new System.Drawing.Size(313, 125);
            gbModels.TabIndex = 2;
            gbModels.TabStop = false;
            gbModels.Text = "Models";
            // 
            // labelSeparator3
            // 
            labelSeparator3.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            labelSeparator3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            labelSeparator3.DisabledColor = System.Drawing.Color.Empty;
            labelSeparator3.IsSeparator = true;
            labelSeparator3.Location = new System.Drawing.Point(6, 44);
            labelSeparator3.Name = "labelSeparator3";
            labelSeparator3.OriginalBorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            labelSeparator3.Size = new System.Drawing.Size(301, 2);
            labelSeparator3.TabIndex = 3;
            // 
            // gbExtraModels
            // 
            gbExtraModels.Controls.Add(cbHasChunk19Model);
            gbExtraModels.Controls.Add(cbHasExtraChunk1ModelWithChunk21Textures);
            gbExtraModels.Location = new System.Drawing.Point(328, 243);
            gbExtraModels.Name = "gbExtraModels";
            gbExtraModels.Size = new System.Drawing.Size(313, 93);
            gbExtraModels.TabIndex = 3;
            gbExtraModels.TabStop = false;
            gbExtraModels.Text = "Extra Models";
            // 
            // gbOtherFlags
            // 
            gbOtherFlags.Controls.Add(cbModifyPalette1ForGradient);
            gbOtherFlags.Controls.Add(cbUnknownMapFlag0x0001);
            gbOtherFlags.Controls.Add(cbUnknownMapFlag0x0002);
            gbOtherFlags.Controls.Add(cbUnknownMapFlag0x0020);
            gbOtherFlags.Location = new System.Drawing.Point(328, 345);
            gbOtherFlags.Name = "gbOtherFlags";
            gbOtherFlags.Size = new System.Drawing.Size(313, 120);
            gbOtherFlags.TabIndex = 4;
            gbOtherFlags.TabStop = false;
            gbOtherFlags.Text = "Other Flags";
            // 
            // gbPlanesBackgrounds
            // 
            gbPlanesBackgrounds.Controls.Add(labelSeparator4);
            gbPlanesBackgrounds.Controls.Add(labelGroundImageType);
            gbPlanesBackgrounds.Controls.Add(cbHasCutsceneSkyBox);
            gbPlanesBackgrounds.Controls.Add(cbHasAnySkyBox);
            gbPlanesBackgrounds.Controls.Add(cbGroundImageType);
            gbPlanesBackgrounds.Controls.Add(cbHasBattleSkyBox);
            gbPlanesBackgrounds.Controls.Add(labelBackgroundImageType);
            gbPlanesBackgrounds.Controls.Add(cbBackgroundImageType);
            gbPlanesBackgrounds.Location = new System.Drawing.Point(3, 311);
            gbPlanesBackgrounds.Name = "gbPlanesBackgrounds";
            gbPlanesBackgrounds.Size = new System.Drawing.Size(313, 154);
            gbPlanesBackgrounds.TabIndex = 5;
            gbPlanesBackgrounds.TabStop = false;
            gbPlanesBackgrounds.Text = "Planes / Backgrounds";
            // 
            // labelSeparator4
            // 
            labelSeparator4.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            labelSeparator4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            labelSeparator4.DisabledColor = System.Drawing.Color.Empty;
            labelSeparator4.IsSeparator = true;
            labelSeparator4.Location = new System.Drawing.Point(4, 123);
            labelSeparator4.Name = "labelSeparator4";
            labelSeparator4.OriginalBorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            labelSeparator4.Size = new System.Drawing.Size(305, 2);
            labelSeparator4.TabIndex = 6;
            // 
            // darkModeGroupBox1
            // 
            darkModeGroupBox1.Controls.Add(labelSeparator5);
            darkModeGroupBox1.Controls.Add(cbChunk1IsLoadedFromHighMemory);
            darkModeGroupBox1.Controls.Add(cbChunk1IsLoadedFromLowMemory);
            darkModeGroupBox1.Controls.Add(cbChunk20IsSurfaceModelIfExists);
            darkModeGroupBox1.Controls.Add(cbChunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists);
            darkModeGroupBox1.Location = new System.Drawing.Point(3, 152);
            darkModeGroupBox1.Name = "darkModeGroupBox1";
            darkModeGroupBox1.Size = new System.Drawing.Size(313, 135);
            darkModeGroupBox1.TabIndex = 6;
            darkModeGroupBox1.TabStop = false;
            darkModeGroupBox1.Text = "Model / Surface Memory";
            // 
            // labelSeparator5
            // 
            labelSeparator5.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            labelSeparator5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            labelSeparator5.DisabledColor = System.Drawing.Color.Empty;
            labelSeparator5.IsSeparator = true;
            labelSeparator5.Location = new System.Drawing.Point(6, 80);
            labelSeparator5.Name = "labelSeparator5";
            labelSeparator5.OriginalBorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            labelSeparator5.Size = new System.Drawing.Size(301, 2);
            labelSeparator5.TabIndex = 7;
            // 
            // MPD_FlagEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(darkModeGroupBox1);
            Controls.Add(gbPlanesBackgrounds);
            Controls.Add(gbOtherFlags);
            Controls.Add(gbExtraModels);
            Controls.Add(gbModels);
            Controls.Add(gbSurface);
            Name = "MPD_FlagEditor";
            Size = new System.Drawing.Size(645, 468);
            gbSurface.ResumeLayout(false);
            gbSurface.PerformLayout();
            gbModels.ResumeLayout(false);
            gbModels.PerformLayout();
            gbExtraModels.ResumeLayout(false);
            gbExtraModels.PerformLayout();
            gbOtherFlags.ResumeLayout(false);
            gbOtherFlags.PerformLayout();
            gbPlanesBackgrounds.ResumeLayout(false);
            gbPlanesBackgrounds.PerformLayout();
            darkModeGroupBox1.ResumeLayout(false);
            darkModeGroupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private CommonLib.Win.Controls.DarkModeCheckBox cbUnknownMapFlag0x0001;
        private CommonLib.Win.Controls.DarkModeCheckBox cbUnknownMapFlag0x0002;
        private CommonLib.Win.Controls.DarkModeCheckBox cbHasSurfaceTextureRotation;
        private CommonLib.Win.Controls.DarkModeCheckBox cbAddDotProductBasedNoiseToStandardLightmap;
        private CommonLib.Win.Controls.DarkModeCheckBox cbKeepTexturelessFlatTiles;
        private CommonLib.Win.Controls.DarkModeCheckBox cbUnknownMapFlag0x0020;
        private CommonLib.Win.Controls.DarkModeCheckBox cbHasChunk19Model;
        private CommonLib.Win.Controls.DarkModeCheckBox cbModifyPalette1ForGradient;
        private CommonLib.Win.Controls.DarkModeCheckBox cbHasModels;
        private CommonLib.Win.Controls.DarkModeCheckBox cbHasSurfaceModel;
        private CommonLib.Win.Controls.DarkModeCheckBox cbHasCutsceneSkyBox;
        private CommonLib.Win.Controls.DarkModeCheckBox cbHasBattleSkyBox;
        private CommonLib.Win.Controls.DarkModeCheckBox cbNarrowAngleBasedLightmap;
        private CommonLib.Win.Controls.DarkModeCheckBox cbHasExtraChunk1ModelWithChunk21Textures;
        private CommonLib.Win.Controls.DarkModeCheckBox cbChunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists;
        private CommonLib.Win.Controls.DarkModeCheckBox cbChunk1IsLoadedFromLowMemory;
        private CommonLib.Win.Controls.DarkModeCheckBox cbChunk1IsLoadedFromHighMemory;
        private CommonLib.Win.Controls.DarkModeCheckBox cbChunk20IsSurfaceModelIfExists;
        private CommonLib.Win.Controls.DarkModeCheckBox cbChunk1IsModels;
        private CommonLib.Win.Controls.DarkModeCheckBox cbChunk2IsSurfaceModel;
        private CommonLib.Win.Controls.DarkModeCheckBox cbChunk20IsSurfaceModel;
        private CommonLib.Win.Controls.DarkModeCheckBox cbChunk20IsModels;
        private CommonLib.Win.Controls.DarkModeCheckBox cbHighMemoryHasModels;
        private CommonLib.Win.Controls.DarkModeCheckBox cbHighMemoryHasSurfaceModel;
        private CommonLib.Win.Controls.DarkModeCheckBox cbHasAnySkyBox;
        private CommonLib.Win.Controls.DarkModeLabel labelBackgroundImageType;
        private CommonLib.Win.Controls.DarkModeComboBox cbBackgroundImageType;
        private CommonLib.Win.Controls.DarkModeLabel labelGroundImageType;
        private CommonLib.Win.Controls.DarkModeComboBox cbGroundImageType;
        private CommonLib.Win.Controls.DarkModeGroupBox gbSurface;
        private CommonLib.Win.Controls.DarkModeLabel labelSeparator1;
        private CommonLib.Win.Controls.DarkModeLabel labelSeparator2;
        private CommonLib.Win.Controls.DarkModeGroupBox gbModels;
        private CommonLib.Win.Controls.DarkModeLabel labelSeparator3;
        private CommonLib.Win.Controls.DarkModeGroupBox gbExtraModels;
        private CommonLib.Win.Controls.DarkModeGroupBox gbPlanesBackgrounds;
        private CommonLib.Win.Controls.DarkModeLabel labelSeparator4;
        private CommonLib.Win.Controls.DarkModeGroupBox darkModeGroupBox1;
        private CommonLib.Win.Controls.DarkModeLabel labelSeparator5;
        private CommonLib.Win.Controls.DarkModeGroupBox gbOtherFlags;
    }
}
