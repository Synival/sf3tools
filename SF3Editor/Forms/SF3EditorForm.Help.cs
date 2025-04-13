using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace SF3.Editor.Forms {
    public partial class SF3EditorForm {
        private void tsmiHelp_About_Click(object sender, EventArgs e) {
            // Fetch copyright info from the assembly itself.
            string? legalCopyright = null;
            try {
                var versionInfo = FileVersionInfo.GetVersionInfo(Environment.ProcessPath ?? "");
                legalCopyright = versionInfo.LegalCopyright;
            }
            catch (Exception ex) {
                legalCopyright = $"(Couldn't fetch copyright: {ex.GetType().Name} exception thrown with message: {ex.Message}";
            }

            // Show info, credits, and special thanks.
            MessageBox.Show(
                _versionTitle + "\n\n" +
                legalCopyright + "\n\n" +
                "All credit to Agrathejagged for the MPD compression/decompression code: https://github.com/Agrathejagged",
                "About " + _baseTitle
            );
        }
    }
}
