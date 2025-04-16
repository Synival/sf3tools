using System;

namespace SF3.Editor.Forms {
    public partial class SF3EditorForm {
        private void InitViewMenu() {
            tsmiView_MPD_DrawSurfaceModel.Checked        = _appState.ViewerDrawSurfaceModel;
            tsmiView_MPD_DrawModels.Checked              = _appState.ViewerDrawModels;
            tsmiView_MPD_DrawGround.Checked              = _appState.ViewerDrawGround;
            tsmiView_MPD_DrawSkyBox.Checked              = _appState.ViewerDrawSkyBox;
            tsmiView_MPD_RunAnimations.Checked           = _appState.ViewerRunAnimations;
            tsmiView_MPD_ApplyLighting.Checked           = _appState.ViewerApplyLighting;
            tsmiView_MPD_DrawGradients.Checked           = _appState.ViewerDrawGradients;
            tsmiView_MPD_DrawWireframes.Checked          = _appState.ViewerDrawWireframe;
            tsmiView_MPD_DrawBoundaries.Checked          = _appState.ViewerDrawBoundaries;
            tsmiView_MPD_DrawTerrainTypes.Checked        = _appState.ViewerDrawTerrainTypes;
            tsmiView_MPD_DrawEventIDs.Checked            = _appState.ViewerDrawEventIDs;
            tsmiView_MPD_DrawCollisionLines.Checked      = _appState.ViewerDrawCollisionLines;
            tsmiView_MPD_DrawNormalMap.Checked           = _appState.ViewerDrawNormals;
            tsmiView_MPD_RotateSpritesUpToCamera.Checked = _appState.ViewerRotateSpritesUp;
            tsmiView_MPD_ShowHelp.Checked                = _appState.ViewerDrawHelp;
            tsmiView_MPD_EnableBlankFieldV2Controls.Checked = _appState.EnableExperimentalBlankFieldV2Brushes;

            _appState.ViewerDrawSurfaceModelChanged   += (s, e) => { tsmiView_MPD_DrawSurfaceModel.Checked        = _appState.ViewerDrawSurfaceModel;   _appState.Serialize(); };
            _appState.ViewerDrawModelsChanged         += (s, e) => { tsmiView_MPD_DrawModels.Checked              = _appState.ViewerDrawModels;         _appState.Serialize(); };
            _appState.ViewerDrawGroundChanged         += (s, e) => { tsmiView_MPD_DrawGround.Checked              = _appState.ViewerDrawGround;         _appState.Serialize(); };
            _appState.ViewerDrawSkyBoxChanged         += (s, e) => { tsmiView_MPD_DrawSkyBox.Checked              = _appState.ViewerDrawSkyBox;         _appState.Serialize(); };
            _appState.ViewerRunAnimationsChanged      += (s, e) => { tsmiView_MPD_RunAnimations.Checked           = _appState.ViewerRunAnimations;      _appState.Serialize(); };
            _appState.ViewerApplyLightingChanged      += (s, e) => { tsmiView_MPD_ApplyLighting.Checked           = _appState.ViewerApplyLighting;      _appState.Serialize(); };
            _appState.ViewerDrawGradientsChanged      += (s, e) => { tsmiView_MPD_DrawGradients.Checked           = _appState.ViewerDrawGradients;      _appState.Serialize(); };
            _appState.ViewerDrawWireframeChanged      += (s, e) => { tsmiView_MPD_DrawWireframes.Checked          = _appState.ViewerDrawWireframe;      _appState.Serialize(); };
            _appState.ViewerDrawBoundariesChanged     += (s, e) => { tsmiView_MPD_DrawBoundaries.Checked          = _appState.ViewerDrawBoundaries;     _appState.Serialize(); };
            _appState.ViewerDrawTerrainTypesChanged   += (s, e) => { tsmiView_MPD_DrawTerrainTypes.Checked        = _appState.ViewerDrawTerrainTypes;   _appState.Serialize(); };
            _appState.ViewerDrawEventIDsChanged       += (s, e) => { tsmiView_MPD_DrawEventIDs.Checked            = _appState.ViewerDrawEventIDs;       _appState.Serialize(); };
            _appState.ViewerDrawCollisionLinesChanged += (s, e) => { tsmiView_MPD_DrawCollisionLines.Checked      = _appState.ViewerDrawCollisionLines; _appState.Serialize(); };
            _appState.ViewerDrawNormalsChanged        += (s, e) => { tsmiView_MPD_DrawNormalMap.Checked           = _appState.ViewerDrawNormals;        _appState.Serialize(); };
            _appState.ViewerRotateSpritesUpChanged    += (s, e) => { tsmiView_MPD_RotateSpritesUpToCamera.Checked = _appState.ViewerRotateSpritesUp;    _appState.Serialize(); };
            _appState.ViewerDrawHelpChanged           += (s, e) => { tsmiView_MPD_ShowHelp.Checked                = _appState.ViewerDrawHelp;           _appState.Serialize(); };
            _appState.EnableExperimentalBlankFieldV2BrushesChanged += (s, e) => { tsmiView_MPD_EnableBlankFieldV2Controls.Checked = _appState.EnableExperimentalBlankFieldV2Brushes; _appState.Serialize(); };
        }

        private void tsmiView_MPD_DrawSurfaceModel_Click(object sender, EventArgs e)        => _appState.ViewerDrawSurfaceModel = !_appState.ViewerDrawSurfaceModel;
        private void tsmiView_MPD_DrawModels_Click(object sender, EventArgs e)              => _appState.ViewerDrawModels = !_appState.ViewerDrawModels;
        private void tsmiView_MPD_DrawGround_Click(object sender, EventArgs e)              => _appState.ViewerDrawGround = !_appState.ViewerDrawGround;
        private void tsmiView_MPD_DrawSkyBox_Click(object sender, EventArgs e)              => _appState.ViewerDrawSkyBox = !_appState.ViewerDrawSkyBox;
        private void tsmiView_MPD_RunAnimations_Click(object sender, EventArgs e)           => _appState.ViewerRunAnimations = !_appState.ViewerRunAnimations;
        private void tsmiView_MPD_ApplyLighting_Click(object sender, EventArgs e)           => _appState.ViewerApplyLighting = !_appState.ViewerApplyLighting;
        private void tsmiView_MPD_DrawGradients_Click(object sender, EventArgs e)           => _appState.ViewerDrawGradients = !_appState.ViewerDrawGradients;
        private void tsmiView_MPD_DrawWireframes_Click(object sender, EventArgs e)          => _appState.ViewerDrawWireframe = !_appState.ViewerDrawWireframe;
        private void tsmiView_MPD_DrawBoundaries_Click(object sender, EventArgs e)          => _appState.ViewerDrawBoundaries = !_appState.ViewerDrawBoundaries;
        private void tsmiView_MPD_DrawTerrainTypes_Click(object sender, EventArgs e)        => _appState.ViewerDrawTerrainTypes = !_appState.ViewerDrawTerrainTypes;
        private void tsmiView_MPD_DrawEventIDs_Click(object sender, EventArgs e)            => _appState.ViewerDrawEventIDs = !_appState.ViewerDrawEventIDs;
        private void tsmiView_MPD_DrawCollisionLines_Click(object sender, EventArgs e)      => _appState.ViewerDrawCollisionLines = !_appState.ViewerDrawCollisionLines;
        private void tsmiView_MPD_DrawNormalMap_Click(object sender, EventArgs e)           => _appState.ViewerDrawNormals = !_appState.ViewerDrawNormals;
        private void tsmiView_MPD_RotateSpritesUpToCamera_Click(object sender, EventArgs e) => _appState.ViewerRotateSpritesUp = !_appState.ViewerRotateSpritesUp;
        private void tsmiView_MPD_ShowHelp_Click(object sender, EventArgs e)                => _appState.ViewerDrawHelp = !_appState.ViewerDrawHelp;
        private void tsmiView_MPD_EnableBlankFieldV2Controls_Click(object sender, EventArgs e) => _appState.EnableExperimentalBlankFieldV2Brushes = !_appState.EnableExperimentalBlankFieldV2Brushes;
    }
}
