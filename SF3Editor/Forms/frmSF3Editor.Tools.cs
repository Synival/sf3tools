using System;

namespace SF3.Editor.Forms {
    public partial class frmSF3Editor {
        private void tsmiTools_ApplyDFR_Click(object sender, EventArgs e) {
            if (_selectedFile != null)
                ApplyDFRDialog(_selectedFile);
        }

        private void tsmiTools_CreateDFR_Click(object sender, EventArgs e) {
            if (_selectedFile != null)
                CreateDFRDialog(_selectedFile);
        }

        private void tsmiTools_ImportTable_Click(object sender, EventArgs e) {
            if (_selectedFile != null)
                CopyTablesFrom(_selectedFile);
        }

        private void tsmiTools_ExportTable_Click(object sender, EventArgs e) {
            if (_selectedFile != null)
                CopyTablesTo(_selectedFile);
        }
    }
}
