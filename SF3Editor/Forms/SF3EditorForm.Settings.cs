using System;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.Shared;

namespace SF3.Editor.Forms {
    public partial class SF3EditorForm {
        private void InitSettingsMenu() {
            // Link some dropdowns/values to the app state.
            tsmiSettings_UseDropdowns.Checked                         = _appState.UseDropdownsForNamedValues;
            tsmiSettings_EnableDebugSettings.Checked                  = _appState.EnableDebugSettings;
            tsmiSettings_ShowErrorsOnFileLoad.Checked                 = _appState.ShowErrorsOnFileLoad;

            tsmiSettings_MPD_ImprovedNormalCalculations.Checked       = _appState.UseImprovedNormalCalculations;
            tsmiSettings_MPD_UseFullHeightForNormals.Checked          = !_appState.UseVanillaHalfHeightForSurfaceNormalCalculations;
            tsmiSettings_MPD_FixNormalOverflowUnderflowErrors.Checked = _appState.FixSurfaceMapTileNormalOverflowUnderflowErrors;
            tsmiSettings_MPD_UpdateChunkTableOnChunkResize.Checked    = _appState.AutoUpdateMPDChunkTableOnChunkResize;
            tsmiSettings_MPD_AutoRebuildMPDChunkTable.Checked         = _appState.AutoRebuildMPDChunkTableOnSave;

            StatGrowthStatistics.DebugGrowthValues = _appState.EnableDebugSettings;
            MPD_File.RebuildChunkTableOnFinish     = _appState.AutoRebuildMPDChunkTableOnSave;

            _appState.UseDropdownsForNamedValuesChanged += (s, e)
                => { tsmiSettings_UseDropdowns.Checked = _appState.UseDropdownsForNamedValues; _appState.Serialize(); };
            _appState.EnableDebugSettingsChanged += (s, e) => {
                tsmiSettings_EnableDebugSettings.Checked = _appState.EnableDebugSettings;
                StatGrowthStatistics.DebugGrowthValues = _appState.EnableDebugSettings;
                _appState.Serialize();
            };
            _appState.ShowErrorsOnFileLoadChanged += (s, e)
                => { tsmiSettings_ShowErrorsOnFileLoad.Checked = _appState.ShowErrorsOnFileLoad; _appState.Serialize(); };

            _appState.UseImprovedNormalCalculationsChanged += (s, e)
                => { tsmiSettings_MPD_ImprovedNormalCalculations.Checked = _appState.UseImprovedNormalCalculations; _appState.Serialize(); };
            _appState.UseVanillaHalfHeightForSurfaceNormalCalculationsChanged += (s, e)
                => { tsmiSettings_MPD_UseFullHeightForNormals.Checked = !_appState.UseVanillaHalfHeightForSurfaceNormalCalculations; _appState.Serialize(); };
            _appState.FixSurfaceMapTileNormalOverflowUnderflowErrorsChanged += (s, e)
                => { tsmiSettings_MPD_FixNormalOverflowUnderflowErrors.Checked = _appState.FixSurfaceMapTileNormalOverflowUnderflowErrors; _appState.Serialize(); };

            _appState.AutoUpdateMPDChunkTableOnChunkResizeChanged += (s, e) => {
                tsmiSettings_MPD_UpdateChunkTableOnChunkResize.Checked = _appState.AutoUpdateMPDChunkTableOnChunkResize;
                MPD_File.UpdateChunkTableOnChunkResize = _appState.AutoUpdateMPDChunkTableOnChunkResize;
                _appState.Serialize();
            };

            _appState.AutoRebuildMPDChunkTableOnSaveChanged += (s, e) => {
                tsmiSettings_MPD_AutoRebuildMPDChunkTable.Checked = _appState.AutoRebuildMPDChunkTableOnSave;
                MPD_File.RebuildChunkTableOnFinish = _appState.AutoRebuildMPDChunkTableOnSave;
                _appState.Serialize();
            };
        }

        private void tsmiSettings_UseDropdowns_Click(object sender, EventArgs e)
            => _appState.UseDropdownsForNamedValues = !_appState.UseDropdownsForNamedValues;
        private void tsmiSettings_EnableDebugSettings_Click(object sender, EventArgs e)
            => _appState.EnableDebugSettings                              = !_appState.EnableDebugSettings;
        private void tsmiSettings_ShowErrorsOnFileLoad_Click(object sender, EventArgs e)
            => _appState.ShowErrorsOnFileLoad = ! _appState.ShowErrorsOnFileLoad;

        private void tsmiSettings_MPD_ImprovedNormalCalculations_Click(object sender, EventArgs e)
            => _appState.UseImprovedNormalCalculations = !_appState.UseImprovedNormalCalculations;
        private void tsmiSettings_MPD_UseFullHeightForNormals_Click(object sender, EventArgs e)
            => _appState.UseVanillaHalfHeightForSurfaceNormalCalculations = !_appState.UseVanillaHalfHeightForSurfaceNormalCalculations;
        private void tsmiSettings_MPD_FixNormalOverflowUnderflowErrors_Click(object sender, EventArgs e)
            => _appState.FixSurfaceMapTileNormalOverflowUnderflowErrors = !_appState.FixSurfaceMapTileNormalOverflowUnderflowErrors;
        private void tsmiSettings_MPD_UpdateChunkTableOnChunkResize_Click(object sender, EventArgs e)
            => _appState.AutoUpdateMPDChunkTableOnChunkResize = !_appState.AutoUpdateMPDChunkTableOnChunkResize;
        private void tsmiSettings_MPD_AutoRebuildMPDChunkTable_Click(object sender, EventArgs e)
            => _appState.AutoRebuildMPDChunkTableOnSave = !_appState.AutoRebuildMPDChunkTableOnSave;
    }
}
