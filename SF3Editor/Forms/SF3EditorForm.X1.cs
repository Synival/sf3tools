using System;
using System.Linq;
using CommonLib.Discovery;
using CommonLib.Win.Utils;
using SF3.Models.Files.X1;
using SF3.Types;

namespace SF3.Editor.Forms {
    public partial class SF3EditorForm {
        private void tsmiX1_MovePostEOFPointers_Click(object sender, EventArgs e) {
            if (SelectedFile?.FileType == SF3FileType.X1 || SelectedFile?.FileType == SF3FileType.X1BTL99) {
                var x1File = (X1_File) SelectedFile.Loader.Model;
                var discoveriesAfterEOF = x1File.Discoveries.GetAllOrdered()
                    .Where(x => x.Address >= x1File.RamAddress + x1File.Data.Length && x.Address < X1_File.X1RamUpperLimit)
                    .ToArray();

                if (discoveriesAfterEOF.Length > 0)
                    RunMovePostEOFPointersDialog(x1File, discoveriesAfterEOF);
                else
                    MessageUtils.InfoMessage("No pointers to data after EOF; there is nothing to move.");
            }
        }

        private void tsmiX1_AddZeroes_Click(object sender, EventArgs e) {
            // TODO: write this!
        }
 
        private void RunMovePostEOFPointersDialog(X1_File x1File, DiscoveredData[] postEOFData) {
            var dialog = new MovePostEOFX1DataDialog(x1File, postEOFData);
            var result = dialog.ShowDialog();
            // TODO: do something with dialog!
        }
    }
}
