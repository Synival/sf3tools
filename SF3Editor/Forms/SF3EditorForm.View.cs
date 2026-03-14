using System;

namespace SF3.Editor.Forms {
    public partial class SF3EditorForm {
        private void InitViewMenu() {
            tsmiView_DarkMode.Checked                    = _appSettings.DarkMode;
            tsmiView_HighlightEndcodesInTextureViews.Checked = _appSettings.HighlightEndCodesInTextureView;

            tsmiView_MPD_DrawSurfaceModel.Checked        = _appSettings.ViewerDrawSurfaceModel;
            tsmiView_MPD_DrawModels.Checked              = _appSettings.ViewerDrawModels;
            tsmiView_MPD_DrawExtraModels.Checked         = _appSettings.ViewerDrawExtraModels;
            tsmiView_MPD_DrawGround.Checked              = _appSettings.ViewerDrawGround;
            tsmiView_MPD_DrawSky.Checked                 = _appSettings.ViewerDrawSky;
            tsmiView_MPD_RunAnimations.Checked           = _appSettings.ViewerRunAnimations;
            tsmiView_MPD_ApplyLighting.Checked           = _appSettings.ViewerApplyLighting;
            tsmiView_MPD_DrawGradients.Checked           = _appSettings.ViewerDrawGradients;
            tsmiView_MPD_DrawActors.Checked              = _appSettings.ViewerDrawActors;

            tsmiView_MPD_DrawWireframes.Checked          = _appSettings.ViewerDrawWireframe;
            tsmiView_MPD_DrawBoundaries.Checked          = _appSettings.ViewerDrawBoundaries;
            tsmiView_MPD_DrawTerrainTypes.Checked        = _appSettings.ViewerDrawTerrainTypes;
            tsmiView_MPD_DrawEventIDs.Checked            = _appSettings.ViewerDrawEventIDs;
            tsmiView_MPD_DrawCollisionLines.Checked      = _appSettings.ViewerDrawCollisionLines;
            tsmiView_MPD_HideModelsNotFacingCamera.Checked = _appSettings.HideModelsNotFacingCamera;
            tsmiView_MPD_ApplyShadowTags.Checked         = _appSettings.ViewerApplyShadowTags;
            tsmiView_MPD_ApplyHideTags.Checked           = _appSettings.ViewerApplyHideTags;

            tsmiView_MPD_DrawNormalMap.Checked           = _appSettings.ViewerDrawNormals;
            tsmiView_MPD_RotateSpritesUpToCamera.Checked = _appSettings.ViewerRotateSpritesUp;
            tsmiView_MPD_RenderOnBlackBackground.Checked = _appSettings.RenderOnBlackBackground;

            tsmiView_MPD_EnableBlankFieldV2Controls.Checked = _appSettings.EnableExperimentalBlankFieldV2Brushes;

            _appSettings.DarkModeChanged += (s, e)
                => { tsmiView_DarkMode.Checked = _appSettings.DarkMode; _appSettings.Serialize(); };
            _appSettings.HighlightEndCodesInTextureViewChanged += (s, e)
                => { tsmiView_HighlightEndcodesInTextureViews.Checked = _appSettings.HighlightEndCodesInTextureView; _appSettings.Serialize(); };

            _appSettings.ViewerDrawSurfaceModelChanged += (s, e)
                => { tsmiView_MPD_DrawSurfaceModel.Checked = _appSettings.ViewerDrawSurfaceModel; _appSettings.Serialize(); };
            _appSettings.ViewerDrawModelsChanged += (s, e)
                => { tsmiView_MPD_DrawModels.Checked = _appSettings.ViewerDrawModels; _appSettings.Serialize(); };
            _appSettings.ViewerDrawExtraModelsChanged += (s, e)
                => { tsmiView_MPD_DrawExtraModels.Checked = _appSettings.ViewerDrawExtraModels; _appSettings.Serialize(); };
            _appSettings.ViewerDrawGroundChanged += (s, e)
                => { tsmiView_MPD_DrawGround.Checked = _appSettings.ViewerDrawGround; _appSettings.Serialize(); };
            _appSettings.ViewerDrawSkyChanged += (s, e)
                => { tsmiView_MPD_DrawSky.Checked = _appSettings.ViewerDrawSky; _appSettings.Serialize(); };
            _appSettings.ViewerRunAnimationsChanged += (s, e)
                => { tsmiView_MPD_RunAnimations.Checked = _appSettings.ViewerRunAnimations; _appSettings.Serialize(); };
            _appSettings.ViewerApplyLightingChanged += (s, e)
                => { tsmiView_MPD_ApplyLighting.Checked = _appSettings.ViewerApplyLighting; _appSettings.Serialize(); };
            _appSettings.ViewerDrawGradientsChanged += (s, e)
                => { tsmiView_MPD_DrawGradients.Checked = _appSettings.ViewerDrawGradients; _appSettings.Serialize(); };
            _appSettings.ViewerDrawActorsChanged += (s, e)
                => { tsmiView_MPD_DrawActors.Checked = _appSettings.ViewerDrawActors; _appSettings.Serialize(); };

            _appSettings.ViewerDrawWireframeChanged += (s, e)
                => { tsmiView_MPD_DrawWireframes.Checked = _appSettings.ViewerDrawWireframe; _appSettings.Serialize(); };
            _appSettings.ViewerDrawBoundariesChanged += (s, e)
                => { tsmiView_MPD_DrawBoundaries.Checked = _appSettings.ViewerDrawBoundaries; _appSettings.Serialize(); };
            _appSettings.ViewerDrawTerrainTypesChanged   += (s, e)
                => { tsmiView_MPD_DrawTerrainTypes.Checked = _appSettings.ViewerDrawTerrainTypes; _appSettings.Serialize(); };
            _appSettings.ViewerDrawEventIDsChanged += (s, e)
                => { tsmiView_MPD_DrawEventIDs.Checked = _appSettings.ViewerDrawEventIDs; _appSettings.Serialize(); };
            _appSettings.ViewerDrawCollisionLinesChanged += (s, e)
                => { tsmiView_MPD_DrawCollisionLines.Checked = _appSettings.ViewerDrawCollisionLines; _appSettings.Serialize(); };
            _appSettings.HideModelsNotFacingCameraChanged += (s, e)
                => { tsmiView_MPD_HideModelsNotFacingCamera.Checked = _appSettings.HideModelsNotFacingCamera; _appSettings.Serialize(); };
            _appSettings.ViewerApplyShadowTagsChanged += (s, e)
                => { tsmiView_MPD_ApplyShadowTags.Checked = _appSettings.ViewerApplyShadowTags; _appSettings.Serialize(); };
            _appSettings.ViewerApplyHideTagsChanged += (s, e)
                => { tsmiView_MPD_ApplyHideTags.Checked = _appSettings.ViewerApplyHideTags; _appSettings.Serialize(); };

            _appSettings.ViewerDrawNormalsChanged += (s, e)
                => { tsmiView_MPD_DrawNormalMap.Checked = _appSettings.ViewerDrawNormals; _appSettings.Serialize(); };
            _appSettings.ViewerRotateSpritesUpChanged += (s, e)
                => { tsmiView_MPD_RotateSpritesUpToCamera.Checked = _appSettings.ViewerRotateSpritesUp; _appSettings.Serialize(); };
            _appSettings.RenderOnBlackBackgroundChanged  += (s, e)
                => { tsmiView_MPD_RenderOnBlackBackground.Checked = _appSettings.RenderOnBlackBackground; _appSettings.Serialize(); };

            _appSettings.EnableExperimentalBlankFieldV2BrushesChanged += (s, e)
                => { tsmiView_MPD_EnableBlankFieldV2Controls.Checked = _appSettings.EnableExperimentalBlankFieldV2Brushes; _appSettings.Serialize(); };
        }

        private void tsmiView_HighlightEndcodesInTextureViews_Click(object sender, EventArgs e)
            => _appSettings.HighlightEndCodesInTextureView = !_appSettings.HighlightEndCodesInTextureView;
        private void tsmiView_DarkMode_Click(object sender, EventArgs e)
            => _appSettings.DarkMode = !_appSettings.DarkMode;

        private void tsmiView_MPD_DrawSurfaceModel_Click(object sender, EventArgs e)
            => _appSettings.ViewerDrawSurfaceModel = !_appSettings.ViewerDrawSurfaceModel;
        private void tsmiView_MPD_DrawModels_Click(object sender, EventArgs e)
            => _appSettings.ViewerDrawModels = !_appSettings.ViewerDrawModels;
        private void tsmiView_MPD_DrawExtraModels_Click(object sender, EventArgs e)
            => _appSettings.ViewerDrawExtraModels = !_appSettings.ViewerDrawExtraModels;
        private void tsmiView_MPD_DrawGround_Click(object sender, EventArgs e)
            => _appSettings.ViewerDrawGround = !_appSettings.ViewerDrawGround;
        private void tsmiView_MPD_DrawSky_Click(object sender, EventArgs e)
            => _appSettings.ViewerDrawSky = !_appSettings.ViewerDrawSky;
        private void tsmiView_MPD_RunAnimations_Click(object sender, EventArgs e)
            => _appSettings.ViewerRunAnimations = !_appSettings.ViewerRunAnimations;
        private void tsmiView_MPD_ApplyLighting_Click(object sender, EventArgs e)
            => _appSettings.ViewerApplyLighting = !_appSettings.ViewerApplyLighting;
        private void tsmiView_MPD_DrawGradients_Click(object sender, EventArgs e)
            => _appSettings.ViewerDrawGradients = !_appSettings.ViewerDrawGradients;
        private void tsmiView_MPD_DrawActors_Click(object sender, EventArgs e)
            => _appSettings.ViewerDrawActors = !_appSettings.ViewerDrawActors;
        private void tsmiView_MPD_HideModelsNotFacingCamera_Click(object sender, EventArgs e)
            => _appSettings.HideModelsNotFacingCamera = !_appSettings.HideModelsNotFacingCamera;
        private void tsmiView_MPD_ApplyShadowTags_Click(object sender, EventArgs e)
            => _appSettings.ViewerApplyShadowTags = !_appSettings.ViewerApplyShadowTags;
        private void tsmiView_MPD_ApplyHideTags_Click(object sender, EventArgs e)
            => _appSettings.ViewerApplyHideTags = !_appSettings.ViewerApplyHideTags;

        private void tsmiView_MPD_DrawWireframes_Click(object sender, EventArgs e)
            => _appSettings.ViewerDrawWireframe = !_appSettings.ViewerDrawWireframe;
        private void tsmiView_MPD_DrawBoundaries_Click(object sender, EventArgs e)
            => _appSettings.ViewerDrawBoundaries = !_appSettings.ViewerDrawBoundaries;
        private void tsmiView_MPD_DrawTerrainTypes_Click(object sender, EventArgs e)
            => _appSettings.ViewerDrawTerrainTypes = !_appSettings.ViewerDrawTerrainTypes;
        private void tsmiView_MPD_DrawEventIDs_Click(object sender, EventArgs e)
            => _appSettings.ViewerDrawEventIDs = !_appSettings.ViewerDrawEventIDs;
        private void tsmiView_MPD_DrawCollisionLines_Click(object sender, EventArgs e)
            => _appSettings.ViewerDrawCollisionLines = !_appSettings.ViewerDrawCollisionLines;
        private void tsmiView_MPD_DrawNormalMap_Click(object sender, EventArgs e)
            => _appSettings.ViewerDrawNormals = !_appSettings.ViewerDrawNormals;
        private void tsmiView_MPD_RotateSpritesUpToCamera_Click(object sender, EventArgs e)
            => _appSettings.ViewerRotateSpritesUp = !_appSettings.ViewerRotateSpritesUp;

        private void tsmiView_MPD_RenderOnBlackBackground_Click(object sender, EventArgs e)
            => _appSettings.RenderOnBlackBackground = ! _appSettings.RenderOnBlackBackground;

        private void tsmiView_MPD_EnableBlankFieldV2Controls_Click(object sender, EventArgs e)
            => _appSettings.EnableExperimentalBlankFieldV2Brushes = !_appSettings.EnableExperimentalBlankFieldV2Brushes;
    }
}
