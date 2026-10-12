using System;
using System.IO;
using System.Linq;
using CommonLib.Win.Utils;
using SF3.Editor.Utils;
using SF3.Models.Files.X8PC;

namespace SF3.Editor.Forms {
    public partial class SF3EditorForm {
        private void tsmiPolyChar_LoadAnimations_Click(object sender, EventArgs e) {
            if (SelectedFile?.Loader?.Model is IX8PC_File x8pc) {
                var report = X8AnimLoader.LoadAllAnimationsForX8PC(_loadedFiles, Path.GetDirectoryName(SelectedFile.Loader.Filename) ?? "", x8pc);
                var hasError = report.Any(x => x.StartsWith("Error:"));
                var joinedReport = string.Join("\r\n", report);

                if (hasError)
                    MessageUtils.ErrorMessage(joinedReport);
                else
                    MessageUtils.InfoMessage(joinedReport);
            }
        }
    }
}
