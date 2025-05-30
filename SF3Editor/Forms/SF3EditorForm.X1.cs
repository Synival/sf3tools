using System;
using SF3.Models.Files.X1;
using SF3.Types;

namespace SF3.Editor.Forms {
    public partial class SF3EditorForm {
        private void tsmiX1_MovePostEOFPointers_Click(object sender, EventArgs e) {
            if (SelectedFile?.FileType == SF3FileType.X1 || SelectedFile?.FileType == SF3FileType.X1BTL99) {
                var x1File = (X1_File) SelectedFile.Loader.Model;
                RunMovePostEOFPointersDialog(x1File);
            }
        }

        private void tsmiX1_AddZeroes_Click(object sender, EventArgs e) {
            // TODO: write this!
        }
 
        private void RunMovePostEOFPointersDialog(X1_File x1File) {
            var dialog = new MovePostEOFPointersDialog(x1File);
            var result = dialog.ShowDialog();
            // TODO: do something with dialog!
        }
    }
}
