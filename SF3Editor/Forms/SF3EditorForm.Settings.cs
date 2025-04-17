﻿using System;
using SF3.Models.Structs.X033_X031;

namespace SF3.Editor.Forms {
    public partial class SF3EditorForm {
        private void InitSettingsMenu() {
            // Link some dropdowns/values to the app state.
            tsmiSettings_UseDropdowns.Checked                   = _appState.UseDropdownsForNamedValues;
            tsmiSettings_EnableDebugSettings.Checked            = _appState.EnableDebugSettings;
            tsmiSettings_MPD_ImprovedNormalCalculations.Checked = _appState.UseImprovedNormalCalculations;
            tsmiSettings_MPD_UseFullHeightForNormals.Checked    = !_appState.UseVanillaHalfHeightForSurfaceNormalCalculations;
            tsmiSettings_MPD_FixNormalOverflowUnderflowErrors.Checked = _appState.FixSurfaceMapTileNormalOverflowUnderflowErrors;
            Stats.DebugGrowthValues = _appState.EnableDebugSettings;

            _appState.UseDropdownsForNamedValuesChanged                       += (s, e) => { tsmiSettings_UseDropdowns.Checked                   = _appState.UseDropdownsForNamedValues; _appState.Serialize(); };
            _appState.EnableDebugSettingsChanged                              += (s, e) => { tsmiSettings_EnableDebugSettings.Checked            = _appState.EnableDebugSettings; Stats.DebugGrowthValues = _appState.EnableDebugSettings; _appState.Serialize(); };
            _appState.UseImprovedNormalCalculationsChanged                    += (s, e) => { tsmiSettings_MPD_ImprovedNormalCalculations.Checked = _appState.UseImprovedNormalCalculations; _appState.Serialize(); };
            _appState.UseVanillaHalfHeightForSurfaceNormalCalculationsChanged += (s, e) => { tsmiSettings_MPD_UseFullHeightForNormals.Checked    = !_appState.UseVanillaHalfHeightForSurfaceNormalCalculations; _appState.Serialize(); };
            _appState.FixSurfaceMapTileNormalOverflowUnderflowErrorsChanged   += (s, e) => { tsmiSettings_MPD_FixNormalOverflowUnderflowErrors.Checked = _appState.FixSurfaceMapTileNormalOverflowUnderflowErrors; _appState.Serialize(); };
        }

        private void tsmiSettings_UseDropdowns_Click(object sender, EventArgs e)                   => _appState.UseDropdownsForNamedValues                       = !_appState.UseDropdownsForNamedValues;
        private void tsmiSettings_EnableDebugSettings_Click(object sender, EventArgs e)            => _appState.EnableDebugSettings                              = !_appState.EnableDebugSettings;
        private void tsmiSettings_MPD_ImprovedNormalCalculations_Click(object sender, EventArgs e) => _appState.UseImprovedNormalCalculations                    = !_appState.UseImprovedNormalCalculations;
        private void tsmiSettings_MPD_UseFullHeightForNormals_Click(object sender, EventArgs e)    => _appState.UseVanillaHalfHeightForSurfaceNormalCalculations = !_appState.UseVanillaHalfHeightForSurfaceNormalCalculations;
        private void tsmiSettings_MPD_FixNormalOverflowUnderflowErrors_Click(object sender, EventArgs e) => _appState.FixSurfaceMapTileNormalOverflowUnderflowErrors = !_appState.FixSurfaceMapTileNormalOverflowUnderflowErrors;
    }
}
