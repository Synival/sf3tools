using System;

namespace SF3.Editor.Forms {
    public partial class SF3EditorForm {
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
