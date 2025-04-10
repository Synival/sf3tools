using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CommonLib.NamedValues;
using CommonLib.Utils;
using Microsoft.WindowsAPICodePack.Dialogs;
using SF3.ModelLoaders;
using SF3.Models.Files;
using SF3.Models.Files.MPD;
using SF3.NamedValues;
using SF3.Types;
using SF3.Win;
using SF3.Win.Extensions;
using SF3.Win.Forms;
using SF3.Win.Types;
using SF3.Win.Utils;
using SF3.Win.Views;
using SF3.Win.Views.MPD;
using static CommonLib.Utils.Compression;
using static CommonLib.Win.Utils.MessageUtils;

namespace SF3.MPD_Editor.Forms {
    public partial class frmMPDEditor : EditorFormNew {
        // Used to display version in the application
        protected override string Version => "0.3";

        public IMPD_File File => ModelLoader.Model as IMPD_File;

        public frmMPDEditor() {
            InitializeComponent();
            InitializeEditor(menuStrip2);

            // Scenario is auto-detected.
            foreach (var i in MenuStrip.Items) {
                var tsi = (ToolStripItem) i;
                if (tsi.Name == "tsmiScenario") {
                    MenuStrip.Items.Remove(tsi);
                    break;
                }
            }

            // Remove "Copy Tables To/From"; that's not a good idea for the MPD Editor!
            foreach (var i in MenuStrip.Items) {
                if (i is ToolStripDropDownItem tsdd) {
                    var itemsToRemove = new List<ToolStripItem>();
                    foreach (var j in tsdd.DropDownItems) {
                        var tsi = (ToolStripItem) j;
                        if (tsi.Name == "tsmiFile_CopyTablesTo" ||
                            tsi.Name == "tsmiFile_CopyTablesFrom" ||
                            tsi.Name == "tsmiFile_CopyTablesSeparator"
                        ) {
                            itemsToRemove.Add(tsi);
                        }
                    }
                    foreach (var tsi in itemsToRemove)
                        tsdd.DropDownItems.Remove(tsi);
                }
            }

            // TODO: re-enable these features once they're working properly...
            tsmiChunks.Visible = false;

            // Link some dropdowns/values to the app state.
            _appState = AppState.RetrieveAppState();

            tsmiEdit_EnableBlankFieldV2Controls.Checked = _appState.EnableExperimentalBlankFieldV2Brushes;
            _appState.EnableExperimentalBlankFieldV2BrushesChanged += (s, e) => {
                tsmiEdit_EnableBlankFieldV2Controls.Checked = _appState.EnableExperimentalBlankFieldV2Brushes;
                _appState.Serialize();
            };

            FileIsLoadedChanged += (s, e) => {
                tsmiChunks_ExportChunk.Enabled = IsLoaded;
                tsmiChunks_ImportChunk.Enabled = IsLoaded;
                tsmiChunks_DeleteChunk.Enabled = IsLoaded;

                tsmiTextures_ImportFolder.Enabled = IsLoaded;
                tsmiTextures_ExportToFolder.Enabled = IsLoaded;
            };
        }

        protected override string FileDialogFilter
            => "SF3 Data (*.MPD)|*.MPD|" + base.FileDialogFilter;

        protected override IBaseFile CreateModel(IModelFileLoader loader) {
            var nameGetters = Enum
                .GetValues<ScenarioType>()
                .ToDictionary(x => x, x => (INameGetterContext) new NameGetterContext(x));
            return MPD_File.Create(loader.ByteData, nameGetters);
        }

        protected override IView CreateView(IModelFileLoader loader, IBaseFile model)
            => new MPD_View(loader.Filename, (MPD_File) model);

        private void tsmiTextures_ImportFolder_Click(object sender, System.EventArgs e) {
            using (var dialog = new CommonOpenFileDialog() {
                Title = "Import Textures",
                IsFolderPicker = true,
                Multiselect = false,
                EnsureValidNames = true,
                EnsureFileExists = true,
                EnsurePathExists = true,
            }) {
                if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
                    return;

                var path = dialog.FileName;
                var files = Directory.GetFiles(path);

                var results = File.ReplaceTexturesFromFiles(files, (f) => Image.FromFile(f).Get2DDataABGR1555());
                var message =
                    "Import complete.\n" +
                    "   Imported successfully: " + results.Replaced + "\n" +
                    "   Textures missing: " + results.Missing + "\n" +
                    "   Failed: " + results.Failed + "\n" +
                    "   Ignored (256-color not yet supported): " + results.Skipped;

                InfoMessage(message);
            }
        }

        private void tsmiTextures_ExportToFolder_Click(object sender, System.EventArgs e) {
            using (var dialog = new CommonOpenFileDialog() {
                Title = "Export Textures to Folder",
                IsFolderPicker = true,
                Multiselect = false,
                EnsureValidNames = false,
                EnsureFileExists = false,
                EnsurePathExists = false, // Not respected!! User still has to select a folder that exists :( :(
            }) {
                if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
                    return;

                var path = dialog.FileName;

                var results = File.ExportTexturesToPath(path, (f, data) => ImageUtils.SaveBitmapToFile(f, data, ImageFormat.Png));
                var message =
                    "Export complete.\n" +
                    "   Exported successfully: " + results.Exported + "\n" +
                    "   Failed: " + results.Failed + "\n" +
                    "   Ignored (256-color not yet supported): " + results.Skipped;

                InfoMessage(message);

                // Automatically open the folder, just for fun.
                try {
                    using (Process.Start(new ProcessStartInfo(path) { UseShellExecute = true }))
#pragma warning disable CS0642 // Possible mistaken empty statement
                        ;
#pragma warning restore CS0642 // Possible mistaken empty statement
                }
                catch { }
            }
        }

        private void tsmiHelp_About_Click(object sender, EventArgs e) {
            var versionInfo = FileVersionInfo.GetVersionInfo(Environment.ProcessPath);
            var legalCopyright = versionInfo.LegalCopyright;

            MessageBox.Show(
                VersionTitle + "\n\n" +
                legalCopyright + "\n\n" +
                "All credit to Agrathejagged for the compression/decompression code:\n" +
                "https://github.com/Agrathejagged",
                "About " + BaseTitle
            );
        }

        private string FileNameWithoutExtension => Path.GetFileNameWithoutExtension(ModelLoader.Filename);

        private void tsmiChunks_ExportChunk_Click(object sender, EventArgs e) {
            var dialog = new ManipulateChunkDialog(ManipulateChunkDialogType.ExportChunk, File.ChunkHeader.Rows, FileNameWithoutExtension);
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            try {
                var chunkData = File.ChunkData[dialog.SelectedChunk.ID];
                var chunkDataBytes = dialog.Uncompressed ? chunkData.DecompressedData.GetDataCopy() : chunkData.GetDataCopy();
                System.IO.File.WriteAllBytes(dialog.FileName, chunkDataBytes);
                InfoMessage("Chunk exported successfully.");
            }
            catch (Exception ex) {
                ErrorMessage("Error while exporting:\r\n\r\n" + ex.Message);
            }
        }

        private void tsmiChunks_ImportChunk_Click(object sender, EventArgs e) {
            var dialog = new ManipulateChunkDialog(ManipulateChunkDialogType.ImportChunk, File.ChunkHeader.Rows, FileNameWithoutExtension);
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            try {
                var chunk = dialog.SelectedChunk;
                var chunkData = File.ChunkData[chunk.ID];
                if (chunkData == null)
                    chunkData = ((MPD_File) File).MakeChunkData(chunk.ID, ChunkType.Unknown, dialog.Uncompressed ? CompressionType.Uncompressed : CompressionType.Compressed);

                var chunkDataBytes = System.IO.File.ReadAllBytes(dialog.FileName);
                if (dialog.Uncompressed)
                    chunkData.DecompressedData.SetDataTo(chunkDataBytes);
                else {
                    // Make sure this can actually be decompressed.
                    try {
                        var decompressedData = Decompress(chunkDataBytes, null, out var _);
                    }
                    catch (Exception ex) {
                        ErrorMessage("Data is corrupt - failure to decompress:\r\n\r\n" + ex.Message);
                        return;
                    }
                    chunkData.SetDataTo(chunkDataBytes);
                }
                InfoMessage("Chunk imported successfully.\r\n" +
                            "(You should probably save and reload the file!!!)");
            }
            catch (Exception ex) {
                ErrorMessage("Error while importing:\r\n\r\n" + ex.Message);
            }
        }

        private void tsmiChunks_DeleteChunk_Click(object sender, EventArgs e) {
            var dialog = new ManipulateChunkDialog(ManipulateChunkDialogType.DeleteChunk, File.ChunkHeader.Rows, FileNameWithoutExtension);
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            try {
                var chunkData = File.ChunkData[dialog.SelectedChunk.ID];
                chunkData.SetDataTo([]);
                InfoMessage("Chunk deleted successfully." +
                            "(You should probably save and reload the file!!!)");
            }
            catch (Exception ex) {
                ErrorMessage("Error while deleting:\r\n\r\n" + ex.Message);
            }
        }

        private void tsmiEdit_EnableBlankFieldV2Controls_Click(object sender, EventArgs e)
            => _appState.EnableExperimentalBlankFieldV2Brushes = !_appState.EnableExperimentalBlankFieldV2Brushes;

        private readonly AppState _appState;
    }
}
