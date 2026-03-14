using System;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.Shared;

namespace SF3.Editor.Forms {
    public partial class SF3EditorForm {
        private void InitSettingsMenu() {
            // Link some dropdowns/values to the app state.
            tsmiSettings_UseDropdowns.Checked                         = _appSettings.UseDropdownsForNamedValues;
            tsmiSettings_EnableDebugSettings.Checked                  = _appSettings.EnableDebugSettings;
            tsmiSettings_ShowErrorsOnFileLoad.Checked                 = _appSettings.ShowErrorsOnFileLoad;
            tsmiSettings_MinimalChanges.Checked                       = _appSettings.MinimalChangesWhenImportingSpritesheets;

            tsmiSettings_MPD_ImprovedNormalCalculations.Checked       = _appSettings.UseImprovedNormalCalculations;
            tsmiSettings_MPD_UseFullHeightForNormals.Checked          = !_appSettings.UseVanillaHalfHeightForSurfaceNormalCalculations;
            tsmiSettings_MPD_IgnoreBlankTilesForSurfaceNormals.Checked = _appSettings.IgnoreBlankTilesForSurfaceNormalCalculations;
            tsmiSettings_MPD_FixNormalOverflowUnderflowErrors.Checked = _appSettings.FixSurfaceMapTileNormalOverflowUnderflowErrors;
            tsmiSettings_MPD_UpdateChunkTableOnChunkResize.Checked    = _appSettings.AutoUpdateMPDChunkTableOnChunkResize;
            tsmiSettings_MPD_AutoRebuildMPDChunkTable.Checked         = _appSettings.AutoRebuildMPDChunkTableOnSave;

            StatGrowthStatistics.DebugGrowthValues = _appSettings.EnableDebugSettings;
            MPD_File.UpdateChunkTableOnChunkResize = _appSettings.AutoUpdateMPDChunkTableOnChunkResize;
            MPD_File.RebuildChunkTableOnFinish     = _appSettings.AutoRebuildMPDChunkTableOnSave;

            _appSettings.UseDropdownsForNamedValuesChanged += (s, e)
                => { tsmiSettings_UseDropdowns.Checked = _appSettings.UseDropdownsForNamedValues; _appSettings.Serialize(); };
            _appSettings.EnableDebugSettingsChanged += (s, e) => {
                tsmiSettings_EnableDebugSettings.Checked = _appSettings.EnableDebugSettings;
                StatGrowthStatistics.DebugGrowthValues = _appSettings.EnableDebugSettings;
                _appSettings.Serialize();
            };
            _appSettings.ShowErrorsOnFileLoadChanged += (s, e)
                => { tsmiSettings_ShowErrorsOnFileLoad.Checked = _appSettings.ShowErrorsOnFileLoad; _appSettings.Serialize(); };
            _appSettings.MinimalChangesWhenImportingSpritesheetsChanged += (s, e)
                => { tsmiSettings_MinimalChanges.Checked = _appSettings.MinimalChangesWhenImportingSpritesheets; _appSettings.Serialize(); };

            _appSettings.UseImprovedNormalCalculationsChanged += (s, e)
                => { tsmiSettings_MPD_ImprovedNormalCalculations.Checked = _appSettings.UseImprovedNormalCalculations; _appSettings.Serialize(); };
            _appSettings.UseVanillaHalfHeightForSurfaceNormalCalculationsChanged += (s, e)
                => { tsmiSettings_MPD_UseFullHeightForNormals.Checked = !_appSettings.UseVanillaHalfHeightForSurfaceNormalCalculations; _appSettings.Serialize(); };
            _appSettings.IgnoreBlankTilesForSurfaceNormalCalculationsChanged += (s, e)
                => { tsmiSettings_MPD_IgnoreBlankTilesForSurfaceNormals.Checked = _appSettings.IgnoreBlankTilesForSurfaceNormalCalculations; _appSettings.Serialize(); };
            _appSettings.FixSurfaceMapTileNormalOverflowUnderflowErrorsChanged += (s, e)
                => { tsmiSettings_MPD_FixNormalOverflowUnderflowErrors.Checked = _appSettings.FixSurfaceMapTileNormalOverflowUnderflowErrors; _appSettings.Serialize(); };

            _appSettings.AutoUpdateMPDChunkTableOnChunkResizeChanged += (s, e) => {
                tsmiSettings_MPD_UpdateChunkTableOnChunkResize.Checked = _appSettings.AutoUpdateMPDChunkTableOnChunkResize;
                MPD_File.UpdateChunkTableOnChunkResize = _appSettings.AutoUpdateMPDChunkTableOnChunkResize;
                _appSettings.Serialize();
            };

            _appSettings.AutoRebuildMPDChunkTableOnSaveChanged += (s, e) => {
                tsmiSettings_MPD_AutoRebuildMPDChunkTable.Checked = _appSettings.AutoRebuildMPDChunkTableOnSave;
                MPD_File.RebuildChunkTableOnFinish = _appSettings.AutoRebuildMPDChunkTableOnSave;
                _appSettings.Serialize();
            };
        }

        private void tsmiSettings_UseDropdowns_Click(object sender, EventArgs e)
            => _appSettings.UseDropdownsForNamedValues = !_appSettings.UseDropdownsForNamedValues;
        private void tsmiSettings_EnableDebugSettings_Click(object sender, EventArgs e)
            => _appSettings.EnableDebugSettings                              = !_appSettings.EnableDebugSettings;
        private void tsmiSettings_ShowErrorsOnFileLoad_Click(object sender, EventArgs e)
            => _appSettings.ShowErrorsOnFileLoad = ! _appSettings.ShowErrorsOnFileLoad;
        private void tsmiSettings_MinimalChanges_Click(object sender, EventArgs e)
            => _appSettings.MinimalChangesWhenImportingSpritesheets = ! _appSettings.MinimalChangesWhenImportingSpritesheets;

        private void tsmiSettings_MPD_ImprovedNormalCalculations_Click(object sender, EventArgs e)
            => _appSettings.UseImprovedNormalCalculations = !_appSettings.UseImprovedNormalCalculations;
        private void tsmiSettings_MPD_UseFullHeightForNormals_Click(object sender, EventArgs e)
            => _appSettings.UseVanillaHalfHeightForSurfaceNormalCalculations = !_appSettings.UseVanillaHalfHeightForSurfaceNormalCalculations;
        private void tsmiSettings_MPD_IgnoreBlankTilesForSurfaceNormals_Click(object sender, EventArgs e)
            => _appSettings.IgnoreBlankTilesForSurfaceNormalCalculations = !_appSettings.IgnoreBlankTilesForSurfaceNormalCalculations;
        private void tsmiSettings_MPD_FixNormalOverflowUnderflowErrors_Click(object sender, EventArgs e)
            => _appSettings.FixSurfaceMapTileNormalOverflowUnderflowErrors = !_appSettings.FixSurfaceMapTileNormalOverflowUnderflowErrors;
        private void tsmiSettings_MPD_UpdateChunkTableOnChunkResize_Click(object sender, EventArgs e)
            => _appSettings.AutoUpdateMPDChunkTableOnChunkResize = !_appSettings.AutoUpdateMPDChunkTableOnChunkResize;
        private void tsmiSettings_MPD_AutoRebuildMPDChunkTable_Click(object sender, EventArgs e)
            => _appSettings.AutoRebuildMPDChunkTableOnSave = !_appSettings.AutoRebuildMPDChunkTableOnSave;
    }
}
