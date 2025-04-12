using System;
using System.IO;
using SF3.Models.Files.MPD;
using SF3.Types;

namespace SF3.Editor.Forms {
    public partial class frmSF3Editor {
        private void tsmiMPD_Textures_ImportAll_Click(object sender, EventArgs e) {
            if (_selectedFile?.FileType == SF3FileType.MPD)
                ImportAllMPDTexturesDialog((IMPD_File) _selectedFile.Loader.Model);
        }

        private void tsmiMPD_Textures_ExportAll_Click(object sender, EventArgs e) {
            if (_selectedFile?.FileType == SF3FileType.MPD)
                ExportAllMPDTexturesDialog((IMPD_File) _selectedFile.Loader.Model);
        }

        private void tsmiMPD_Chunks_ExportChunk_Click(object sender, EventArgs e) {
            if (_selectedFile?.FileType == SF3FileType.MPD)
                ExportMPDChunkDialog((IMPD_File) _selectedFile.Loader.Model, Path.GetFileNameWithoutExtension(_selectedFile.Loader.ShortFilename));
        }

        private void tsmiMPD_Chunks_ImportChunk_Click(object sender, EventArgs e) {
            if (_selectedFile?.FileType == SF3FileType.MPD)
                ImportMPDChunkDialog((IMPD_File) _selectedFile.Loader.Model, Path.GetFileNameWithoutExtension(_selectedFile.Loader.ShortFilename));
        }

        private void tsmiMPD_Chunks_DeleteChunk_Click(object sender, EventArgs e) {
            if (_selectedFile?.FileType == SF3FileType.MPD)
                DeleteMPDChunkDialog((IMPD_File) _selectedFile.Loader.Model, Path.GetFileNameWithoutExtension(_selectedFile.Loader.ShortFilename));
        }

        private void tsmiMPD_EnableBlankFieldV2Controls_Click(object sender, EventArgs e)
            => _appState.EnableExperimentalBlankFieldV2Brushes = !_appState.EnableExperimentalBlankFieldV2Brushes;
    }
}
