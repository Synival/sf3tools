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
            gbContent = new CommonLib.Win.Controls.DarkModeGroupBox();
            labelGroundImageType = new CommonLib.Win.Controls.DarkModeLabel();
            cbGroundImageType = new CommonLib.Win.Controls.DarkModeComboBox();
            labelBackgroundImageType = new CommonLib.Win.Controls.DarkModeLabel();
            cbBackgroundImageType = new CommonLib.Win.Controls.DarkModeComboBox();
            cbUnknownMapFlag0x0001 = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbUnknownMapFlag0x0002 = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbHasSurfaceTextureRotation = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbAddDotProductBasedNoiseToStandardLightmap = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbUnknownFlatTileFlag0x0008 = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbUnknownMapFlag0x0020 = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbHasChunk19Model = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbModifyPalette1ForGradient = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbHasModels = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbHasSurfaceModel = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbHasCutsceneSkyBox = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbHasBattleSkyBox = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbNarrowAngleBasedLightmap = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbHasExtraChunk1ModelWithChunk21Textures = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbChunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbChunk1IsLoadedFromLowMemory = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbChunk1IsLoadedFromHighMemory = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbChunk20IsSurfaceModelIfExists = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbChunk1IsModels = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbChunk2IsSurfaceModel = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbChunk20IsSurfaceModel = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbChunk20IsModels = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbHighMemoryHasModels = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbHighMemoryHasSurfaceModel = new CommonLib.Win.Controls.DarkModeCheckBox();
            cbHasAnySkyBox = new CommonLib.Win.Controls.DarkModeCheckBox();
            gbContent.SuspendLayout();
            SuspendLayout();
            // 
            // gbContent
            // 
            gbContent.Controls.Add(labelGroundImageType);
            gbContent.Controls.Add(cbGroundImageType);
            gbContent.Controls.Add(labelBackgroundImageType);
            gbContent.Controls.Add(cbBackgroundImageType);
            gbContent.Controls.Add(cbUnknownMapFlag0x0001);
            gbContent.Controls.Add(cbUnknownMapFlag0x0002);
            gbContent.Controls.Add(cbHasSurfaceTextureRotation);
            gbContent.Controls.Add(cbAddDotProductBasedNoiseToStandardLightmap);
            gbContent.Controls.Add(cbUnknownFlatTileFlag0x0008);
            gbContent.Controls.Add(cbUnknownMapFlag0x0020);
            gbContent.Controls.Add(cbHasChunk19Model);
            gbContent.Controls.Add(cbModifyPalette1ForGradient);
            gbContent.Controls.Add(cbHasModels);
            gbContent.Controls.Add(cbHasSurfaceModel);
            gbContent.Controls.Add(cbHasCutsceneSkyBox);
            gbContent.Controls.Add(cbHasBattleSkyBox);
            gbContent.Controls.Add(cbNarrowAngleBasedLightmap);
            gbContent.Controls.Add(cbHasExtraChunk1ModelWithChunk21Textures);
            gbContent.Controls.Add(cbChunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists);
            gbContent.Controls.Add(cbChunk1IsLoadedFromLowMemory);
            gbContent.Controls.Add(cbChunk1IsLoadedFromHighMemory);
            gbContent.Controls.Add(cbChunk20IsSurfaceModelIfExists);
            gbContent.Controls.Add(cbChunk1IsModels);
            gbContent.Controls.Add(cbChunk2IsSurfaceModel);
            gbContent.Controls.Add(cbChunk20IsSurfaceModel);
            gbContent.Controls.Add(cbChunk20IsModels);
            gbContent.Controls.Add(cbHighMemoryHasModels);
            gbContent.Controls.Add(cbHighMemoryHasSurfaceModel);
            gbContent.Controls.Add(cbHasAnySkyBox);
            gbContent.Location = new System.Drawing.Point(3, 3);
            gbContent.Name = "gbContent";
            gbContent.Size = new System.Drawing.Size(568, 471);
            gbContent.TabIndex = 0;
            gbContent.TabStop = false;
            gbContent.Text = "Content";
            // 
            // labelGroundImageType
            // 
            labelGroundImageType.AutoSize = true;
            labelGroundImageType.DisabledColor = System.Drawing.Color.Empty;
            labelGroundImageType.IsSeparator = false;
            labelGroundImageType.Location = new System.Drawing.Point(6, 297);
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
            cbGroundImageType.Location = new System.Drawing.Point(126, 294);
            cbGroundImageType.Name = "cbGroundImageType";
            cbGroundImageType.Size = new System.Drawing.Size(121, 24);
            cbGroundImageType.TabIndex = 4;
            // 
            // labelBackgroundImageType
            // 
            labelBackgroundImageType.AutoSize = true;
            labelBackgroundImageType.DisabledColor = System.Drawing.Color.Empty;
            labelBackgroundImageType.IsSeparator = false;
            labelBackgroundImageType.Location = new System.Drawing.Point(6, 147);
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
            cbBackgroundImageType.Location = new System.Drawing.Point(150, 144);
            cbBackgroundImageType.Name = "cbBackgroundImageType";
            cbBackgroundImageType.Size = new System.Drawing.Size(121, 24);
            cbBackgroundImageType.TabIndex = 2;
            // 
            // cbUnknownMapFlag0x0001
            // 
            cbUnknownMapFlag0x0001.AutoSize = true;
            cbUnknownMapFlag0x0001.Location = new System.Drawing.Point(6, 22);
            cbUnknownMapFlag0x0001.Name = "cbUnknownMapFlag0x0001";
            cbUnknownMapFlag0x0001.Size = new System.Drawing.Size(157, 19);
            cbUnknownMapFlag0x0001.TabIndex = 0;
            cbUnknownMapFlag0x0001.Text = "Unknown Flag 1 (0x0001)";
            cbUnknownMapFlag0x0001.UseVisualStyleBackColor = true;
            // 
            // cbUnknownMapFlag0x0002
            // 
            cbUnknownMapFlag0x0002.AutoSize = true;
            cbUnknownMapFlag0x0002.Location = new System.Drawing.Point(6, 47);
            cbUnknownMapFlag0x0002.Name = "cbUnknownMapFlag0x0002";
            cbUnknownMapFlag0x0002.Size = new System.Drawing.Size(157, 19);
            cbUnknownMapFlag0x0002.TabIndex = 1;
            cbUnknownMapFlag0x0002.Text = "Unknown Flag 2 (0x0002)";
            cbUnknownMapFlag0x0002.UseVisualStyleBackColor = true;
            // 
            // cbHasSurfaceTextureRotation
            // 
            cbHasSurfaceTextureRotation.AutoSize = true;
            cbHasSurfaceTextureRotation.Location = new System.Drawing.Point(6, 72);
            cbHasSurfaceTextureRotation.Name = "cbHasSurfaceTextureRotation";
            cbHasSurfaceTextureRotation.Size = new System.Drawing.Size(177, 19);
            cbHasSurfaceTextureRotation.TabIndex = 1;
            cbHasSurfaceTextureRotation.Text = "Has Surface Texture Rotation";
            cbHasSurfaceTextureRotation.UseVisualStyleBackColor = true;
            // 
            // cbAddDotProductBasedNoiseToStandardLightmap
            // 
            cbAddDotProductBasedNoiseToStandardLightmap.AutoSize = true;
            cbAddDotProductBasedNoiseToStandardLightmap.Location = new System.Drawing.Point(6, 97);
            cbAddDotProductBasedNoiseToStandardLightmap.Name = "cbAddDotProductBasedNoiseToStandardLightmap";
            cbAddDotProductBasedNoiseToStandardLightmap.Size = new System.Drawing.Size(294, 19);
            cbAddDotProductBasedNoiseToStandardLightmap.TabIndex = 1;
            cbAddDotProductBasedNoiseToStandardLightmap.Text = "Add Dot Product-Based Noise to Surface Lightmap";
            cbAddDotProductBasedNoiseToStandardLightmap.UseVisualStyleBackColor = true;
            // 
            // cbUnknownFlatTileFlag0x0008
            // 
            cbUnknownFlatTileFlag0x0008.AutoSize = true;
            cbUnknownFlatTileFlag0x0008.Location = new System.Drawing.Point(6, 122);
            cbUnknownFlatTileFlag0x0008.Name = "cbUnknownFlatTileFlag0x0008";
            cbUnknownFlatTileFlag0x0008.Size = new System.Drawing.Size(192, 19);
            cbUnknownFlatTileFlag0x0008.TabIndex = 1;
            cbUnknownFlatTileFlag0x0008.Text = "Unknown Flat Tile Flag (0x0008)";
            cbUnknownFlatTileFlag0x0008.UseVisualStyleBackColor = true;
            // 
            // cbUnknownMapFlag0x0020
            // 
            cbUnknownMapFlag0x0020.AutoSize = true;
            cbUnknownMapFlag0x0020.Location = new System.Drawing.Point(6, 172);
            cbUnknownMapFlag0x0020.Name = "cbUnknownMapFlag0x0020";
            cbUnknownMapFlag0x0020.Size = new System.Drawing.Size(157, 19);
            cbUnknownMapFlag0x0020.TabIndex = 1;
            cbUnknownMapFlag0x0020.Text = "Unknown Flag 3 (0x0020)";
            cbUnknownMapFlag0x0020.UseVisualStyleBackColor = true;
            // 
            // cbHasChunk19Model
            // 
            cbHasChunk19Model.AutoSize = true;
            cbHasChunk19Model.Location = new System.Drawing.Point(6, 197);
            cbHasChunk19Model.Name = "cbHasChunk19Model";
            cbHasChunk19Model.Size = new System.Drawing.Size(141, 19);
            cbHasChunk19Model.TabIndex = 1;
            cbHasChunk19Model.Text = "Has Chunk[19] Model";
            cbHasChunk19Model.UseVisualStyleBackColor = true;
            // 
            // cbModifyPalette1ForGradient
            // 
            cbModifyPalette1ForGradient.AutoSize = true;
            cbModifyPalette1ForGradient.Location = new System.Drawing.Point(6, 222);
            cbModifyPalette1ForGradient.Name = "cbModifyPalette1ForGradient";
            cbModifyPalette1ForGradient.Size = new System.Drawing.Size(175, 19);
            cbModifyPalette1ForGradient.TabIndex = 1;
            cbModifyPalette1ForGradient.Text = "Modify Palette1 for Gradient";
            cbModifyPalette1ForGradient.UseVisualStyleBackColor = true;
            // 
            // cbHasModels
            // 
            cbHasModels.AutoSize = true;
            cbHasModels.Location = new System.Drawing.Point(6, 247);
            cbHasModels.Name = "cbHasModels";
            cbHasModels.Size = new System.Drawing.Size(88, 19);
            cbHasModels.TabIndex = 1;
            cbHasModels.Text = "Has Models";
            cbHasModels.UseVisualStyleBackColor = true;
            // 
            // cbHasSurfaceModel
            // 
            cbHasSurfaceModel.AutoSize = true;
            cbHasSurfaceModel.Location = new System.Drawing.Point(6, 272);
            cbHasSurfaceModel.Name = "cbHasSurfaceModel";
            cbHasSurfaceModel.Size = new System.Drawing.Size(125, 19);
            cbHasSurfaceModel.TabIndex = 1;
            cbHasSurfaceModel.Text = "Has Surface Model";
            cbHasSurfaceModel.UseVisualStyleBackColor = true;
            // 
            // cbHasCutsceneSkyBox
            // 
            cbHasCutsceneSkyBox.AutoSize = true;
            cbHasCutsceneSkyBox.Location = new System.Drawing.Point(6, 322);
            cbHasCutsceneSkyBox.Name = "cbHasCutsceneSkyBox";
            cbHasCutsceneSkyBox.Size = new System.Drawing.Size(141, 19);
            cbHasCutsceneSkyBox.TabIndex = 1;
            cbHasCutsceneSkyBox.Text = "Has Cutscene Sky Box";
            cbHasCutsceneSkyBox.UseVisualStyleBackColor = true;
            // 
            // cbHasBattleSkyBox
            // 
            cbHasBattleSkyBox.AutoSize = true;
            cbHasBattleSkyBox.Location = new System.Drawing.Point(6, 347);
            cbHasBattleSkyBox.Name = "cbHasBattleSkyBox";
            cbHasBattleSkyBox.Size = new System.Drawing.Size(122, 19);
            cbHasBattleSkyBox.TabIndex = 1;
            cbHasBattleSkyBox.Text = "Has Battle Sky Box";
            cbHasBattleSkyBox.UseVisualStyleBackColor = true;
            // 
            // cbNarrowAngleBasedLightmap
            // 
            cbNarrowAngleBasedLightmap.AutoSize = true;
            cbNarrowAngleBasedLightmap.Location = new System.Drawing.Point(6, 372);
            cbNarrowAngleBasedLightmap.Name = "cbNarrowAngleBasedLightmap";
            cbNarrowAngleBasedLightmap.Size = new System.Drawing.Size(231, 19);
            cbNarrowAngleBasedLightmap.TabIndex = 1;
            cbNarrowAngleBasedLightmap.Text = "Narrow Angle-Based Surface Lightmap";
            cbNarrowAngleBasedLightmap.UseVisualStyleBackColor = true;
            // 
            // cbHasExtraChunk1ModelWithChunk21Textures
            // 
            cbHasExtraChunk1ModelWithChunk21Textures.AutoSize = true;
            cbHasExtraChunk1ModelWithChunk21Textures.Location = new System.Drawing.Point(6, 397);
            cbHasExtraChunk1ModelWithChunk21Textures.Name = "cbHasExtraChunk1ModelWithChunk21Textures";
            cbHasExtraChunk1ModelWithChunk21Textures.Size = new System.Drawing.Size(293, 19);
            cbHasExtraChunk1ModelWithChunk21Textures.TabIndex = 1;
            cbHasExtraChunk1ModelWithChunk21Textures.Text = "Has Extra Chunk[1] Model with Chunk[21] Textures";
            cbHasExtraChunk1ModelWithChunk21Textures.UseVisualStyleBackColor = true;
            // 
            // cbChunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists
            // 
            cbChunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists.AutoSize = true;
            cbChunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists.Location = new System.Drawing.Point(6, 422);
            cbChunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists.Name = "cbChunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists";
            cbChunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists.Size = new System.Drawing.Size(372, 19);
            cbChunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists.TabIndex = 1;
            cbChunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists.Text = "Chunk[1] Is Still Loaded from Low Memory if Surface Model Exists";
            cbChunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists.UseVisualStyleBackColor = true;
            // 
            // cbChunk1IsLoadedFromLowMemory
            // 
            cbChunk1IsLoadedFromLowMemory.AutoSize = true;
            cbChunk1IsLoadedFromLowMemory.Enabled = false;
            cbChunk1IsLoadedFromLowMemory.Location = new System.Drawing.Point(329, 22);
            cbChunk1IsLoadedFromLowMemory.Name = "cbChunk1IsLoadedFromLowMemory";
            cbChunk1IsLoadedFromLowMemory.Size = new System.Drawing.Size(230, 19);
            cbChunk1IsLoadedFromLowMemory.TabIndex = 1;
            cbChunk1IsLoadedFromLowMemory.Text = "Chunk[1] Is Loaded from Low Memory";
            cbChunk1IsLoadedFromLowMemory.UseVisualStyleBackColor = true;
            // 
            // cbChunk1IsLoadedFromHighMemory
            // 
            cbChunk1IsLoadedFromHighMemory.AutoSize = true;
            cbChunk1IsLoadedFromHighMemory.Enabled = false;
            cbChunk1IsLoadedFromHighMemory.Location = new System.Drawing.Point(329, 47);
            cbChunk1IsLoadedFromHighMemory.Name = "cbChunk1IsLoadedFromHighMemory";
            cbChunk1IsLoadedFromHighMemory.Size = new System.Drawing.Size(234, 19);
            cbChunk1IsLoadedFromHighMemory.TabIndex = 1;
            cbChunk1IsLoadedFromHighMemory.Text = "Chunk[1] Is Loaded from High Memory";
            cbChunk1IsLoadedFromHighMemory.UseVisualStyleBackColor = true;
            // 
            // cbChunk20IsSurfaceModelIfExists
            // 
            cbChunk20IsSurfaceModelIfExists.AutoSize = true;
            cbChunk20IsSurfaceModelIfExists.Enabled = false;
            cbChunk20IsSurfaceModelIfExists.Location = new System.Drawing.Point(6, 447);
            cbChunk20IsSurfaceModelIfExists.Name = "cbChunk20IsSurfaceModelIfExists";
            cbChunk20IsSurfaceModelIfExists.Size = new System.Drawing.Size(212, 19);
            cbChunk20IsSurfaceModelIfExists.TabIndex = 1;
            cbChunk20IsSurfaceModelIfExists.Text = "Chunk[20] Is Surface Model if Exists";
            cbChunk20IsSurfaceModelIfExists.UseVisualStyleBackColor = true;
            // 
            // cbChunk1IsModels
            // 
            cbChunk1IsModels.AutoSize = true;
            cbChunk1IsModels.Enabled = false;
            cbChunk1IsModels.Location = new System.Drawing.Point(329, 72);
            cbChunk1IsModels.Name = "cbChunk1IsModels";
            cbChunk1IsModels.Size = new System.Drawing.Size(128, 19);
            cbChunk1IsModels.TabIndex = 1;
            cbChunk1IsModels.Text = "Chunk[1] Is Models";
            cbChunk1IsModels.UseVisualStyleBackColor = true;
            // 
            // cbChunk2IsSurfaceModel
            // 
            cbChunk2IsSurfaceModel.AutoSize = true;
            cbChunk2IsSurfaceModel.Enabled = false;
            cbChunk2IsSurfaceModel.Location = new System.Drawing.Point(329, 97);
            cbChunk2IsSurfaceModel.Name = "cbChunk2IsSurfaceModel";
            cbChunk2IsSurfaceModel.Size = new System.Drawing.Size(165, 19);
            cbChunk2IsSurfaceModel.TabIndex = 1;
            cbChunk2IsSurfaceModel.Text = "Chunk[2] Is Surface Model";
            cbChunk2IsSurfaceModel.UseVisualStyleBackColor = true;
            // 
            // cbChunk20IsSurfaceModel
            // 
            cbChunk20IsSurfaceModel.AutoSize = true;
            cbChunk20IsSurfaceModel.Enabled = false;
            cbChunk20IsSurfaceModel.Location = new System.Drawing.Point(329, 122);
            cbChunk20IsSurfaceModel.Name = "cbChunk20IsSurfaceModel";
            cbChunk20IsSurfaceModel.Size = new System.Drawing.Size(171, 19);
            cbChunk20IsSurfaceModel.TabIndex = 1;
            cbChunk20IsSurfaceModel.Text = "Chunk[20] Is Surface Model";
            cbChunk20IsSurfaceModel.UseVisualStyleBackColor = true;
            // 
            // cbChunk20IsModels
            // 
            cbChunk20IsModels.AutoSize = true;
            cbChunk20IsModels.Enabled = false;
            cbChunk20IsModels.Location = new System.Drawing.Point(329, 147);
            cbChunk20IsModels.Name = "cbChunk20IsModels";
            cbChunk20IsModels.Size = new System.Drawing.Size(134, 19);
            cbChunk20IsModels.TabIndex = 1;
            cbChunk20IsModels.Text = "Chunk[20] Is Models";
            cbChunk20IsModels.UseVisualStyleBackColor = true;
            // 
            // cbHighMemoryHasModels
            // 
            cbHighMemoryHasModels.AutoSize = true;
            cbHighMemoryHasModels.Enabled = false;
            cbHighMemoryHasModels.Location = new System.Drawing.Point(329, 172);
            cbHighMemoryHasModels.Name = "cbHighMemoryHasModels";
            cbHighMemoryHasModels.Size = new System.Drawing.Size(165, 19);
            cbHighMemoryHasModels.TabIndex = 1;
            cbHighMemoryHasModels.Text = "High Memory Has Models";
            cbHighMemoryHasModels.UseVisualStyleBackColor = true;
            // 
            // cbHighMemoryHasSurfaceModel
            // 
            cbHighMemoryHasSurfaceModel.AutoSize = true;
            cbHighMemoryHasSurfaceModel.Enabled = false;
            cbHighMemoryHasSurfaceModel.Location = new System.Drawing.Point(329, 197);
            cbHighMemoryHasSurfaceModel.Name = "cbHighMemoryHasSurfaceModel";
            cbHighMemoryHasSurfaceModel.Size = new System.Drawing.Size(202, 19);
            cbHighMemoryHasSurfaceModel.TabIndex = 1;
            cbHighMemoryHasSurfaceModel.Text = "High Memory Has Surface Model";
            cbHighMemoryHasSurfaceModel.UseVisualStyleBackColor = true;
            // 
            // cbHasAnySkyBox
            // 
            cbHasAnySkyBox.AutoSize = true;
            cbHasAnySkyBox.Enabled = false;
            cbHasAnySkyBox.Location = new System.Drawing.Point(329, 222);
            cbHasAnySkyBox.Name = "cbHasAnySkyBox";
            cbHasAnySkyBox.Size = new System.Drawing.Size(111, 19);
            cbHasAnySkyBox.TabIndex = 1;
            cbHasAnySkyBox.Text = "Has any Sky Box";
            cbHasAnySkyBox.UseVisualStyleBackColor = true;
            // 
            // MPD_FlagEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(gbContent);
            Name = "MPD_FlagEditor";
            Size = new System.Drawing.Size(575, 477);
            gbContent.ResumeLayout(false);
            gbContent.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private CommonLib.Win.Controls.DarkModeGroupBox gbContent;
        private CommonLib.Win.Controls.DarkModeCheckBox cbUnknownMapFlag0x0001;
        private CommonLib.Win.Controls.DarkModeCheckBox cbUnknownMapFlag0x0002;
        private CommonLib.Win.Controls.DarkModeCheckBox cbHasSurfaceTextureRotation;
        private CommonLib.Win.Controls.DarkModeCheckBox cbAddDotProductBasedNoiseToStandardLightmap;
        private CommonLib.Win.Controls.DarkModeCheckBox cbUnknownFlatTileFlag0x0008;
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
    }
}
