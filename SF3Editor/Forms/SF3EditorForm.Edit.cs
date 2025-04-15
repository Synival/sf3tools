using System;

namespace SF3.Editor.Forms {
    public partial class SF3EditorForm {

        private void tsmiSettings_UseDropdowns_Click(object sender, EventArgs e)
            => _appState.UseDropdownsForNamedValues = !_appState.UseDropdownsForNamedValues;

        private void tsmiSettings_EnableDebugSettings_Click(object sender, EventArgs e)
            => _appState.EnableDebugSettings = !_appState.EnableDebugSettings;
    }
}
