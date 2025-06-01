using System;
using System.Linq;
using CommonLib.Discovery;
using CommonLib.Win.Utils;
using SF3.Models.Files.X1;
using SF3.Types;
using System.Windows.Forms;
using CommonLib.Types;

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

            if (result != DialogResult.OK || dialog.MoveBy == 0)
                return;

            var moveBy   = dialog.MoveBy;
            var fileFrom = x1File.RamAddress;
            var fileTo   = fileFrom + x1File.Data.Length;

            var eofDataFrom = fileTo;
            var eofDataTo   = X1_File.X1RamUpperLimit;

            var pointersToUpdate = x1File.Discoveries.GetAllOrdered()
                .Where(x => x.Type == DiscoveredDataType.Pointer && x.Address >= fileFrom && x.Address < fileTo)
                .Select(x => new { Pointer = x, Value = x1File.Data.GetDouble((int) (x.Address - fileFrom)) })
                .Where(x => x.Value >= eofDataFrom && x.Value < eofDataTo)
                .ToArray();

            foreach (var pointerValue in pointersToUpdate) {
                var addr = (int) (pointerValue.Pointer.Address - fileFrom);
                x1File.Data.SetDouble(addr, pointerValue.Value + moveBy);
            }

            // TODO: a whole bunch of tables now require an update! how do we do that??
        }
    }
}
