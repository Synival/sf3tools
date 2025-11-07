using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using CommonLib.Logging;
using CommonLib.Types;
using Microsoft.WindowsAPICodePack.Dialogs;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD;
using SF3.Types;
using SF3.Win.Extensions;
using SF3.Win.Types;
using SF3.Win.Utils;
using SF3.Win.Views.MPD;
using static CommonLib.Utils.Compression;
using static CommonLib.Win.Utils.MessageUtils;

namespace SF3.Editor.Forms {
    public partial class SF3EditorForm {

        /// <summary>
        /// Opens a dialog to export a particular chunk of an MPD file.
        /// </summary>
        /// <param name="mpdFile">The MPD file from which to export a chunk.</param>
        /// <param name="filenameWithoutExtension">The name of the MPD file that it is normally saved to, without the .MPD extension.</param>
        /// <returns>'true' if an export was successful, otherwise 'false'.</returns>
        public bool ExportMPDChunkDialog(IMPD_File mpdFile, string filenameWithoutExtension) {
            var dialog = new ManipulateChunkDialog(ManipulateChunkDialogType.ExportChunk, mpdFile.ChunkLocations.Rows, filenameWithoutExtension);
            if (dialog.ShowDialog() != DialogResult.OK)
                return false;

            try {
                var chunkData = mpdFile.ChunkData[dialog.SelectedChunk.ID];
                var chunkDataBytes = dialog.Uncompressed ? chunkData.DecompressedData.GetDataCopy() : chunkData.GetDataCopy();
                File.WriteAllBytes(dialog.FileName, chunkDataBytes);
                InfoMessage("Chunk exported successfully.");
            }
            catch (Exception ex) {
                ErrorMessage("Error while exporting:\r\n\r\n" + ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Opens a dialog to import a particular chunk into an MPD file.
        /// </summary>
        /// <param name="mpdFile">The MPD file to which a chunk should be imported.</param>
        /// <param name="filenameWithoutExtension">The name of the MPD file that it is normally saved to, without the .MPD extension.</param>
        /// <returns>'true' if an import was successful, otherwise 'false'.</returns>
        public bool ImportMPDChunkDialog(IMPD_File mpdFile, string filenameWithoutExtension) {
            var dialog = new ManipulateChunkDialog(ManipulateChunkDialogType.ImportChunk, mpdFile.ChunkLocations.Rows, filenameWithoutExtension);
            if (dialog.ShowDialog() != DialogResult.OK)
                return false;

            try {
                var chunk = dialog.SelectedChunk;
                var chunkData = mpdFile.ChunkData[chunk.ID];
                if (chunkData == null)
                    chunkData = ((MPD_File) mpdFile).MakeChunkData(chunk.ID, ChunkType.Unknown, dialog.Uncompressed ? CompressionType.Uncompressed : CompressionType.Compressed);

                var chunkDataBytes = System.IO.File.ReadAllBytes(dialog.FileName);
                if (dialog.Uncompressed)
                    chunkData.DecompressedData.SetDataTo(chunkDataBytes);
                else {
                    // Make sure this can actually be decompressed.
                    try {
                        var decompressedData = DecompressLZSS(chunkDataBytes, 0, null, out var _, out var endDataFound);
                        if (!endDataFound)
                            Logger.WriteLine("Chunk data read may be corrupt -- no 0x0000 'end' code found", LogType.Warning);
                    }
                    catch (Exception ex) {
                        ErrorMessage("Data is corrupt - failure to decompress:\r\n\r\n" + ex.Message);
                        return false;
                    }
                    chunkData.SetDataTo(chunkDataBytes);
                }
                InfoMessage("Chunk imported successfully.\r\n" +
                            "(You should probably save and reload the file!!!)");
            }
            catch (Exception ex) {
                ErrorMessage("Error while importing:\r\n\r\n" + ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Opens a dialog to delete a particular chunk of an MPD file.
        /// </summary>
        /// <param name="mpdFile">The MPD file from which a chunk should be deleted.</param>
        /// <param name="filenameWithoutExtension">The name of the MPD file that it is normally saved to, without the .MPD extension.</param>
        /// <returns>'true' if a deletion was successful, otherwise 'false'.</returns>
        public bool DeleteMPDChunkDialog(IMPD_File mpdFile, string fileNameWithoutExtension) {
            var dialog = new ManipulateChunkDialog(ManipulateChunkDialogType.DeleteChunk, mpdFile.ChunkLocations.Rows, fileNameWithoutExtension);
            if (dialog.ShowDialog() != DialogResult.OK)
                return false;

            try {
                var chunkData = mpdFile.ChunkData[dialog.SelectedChunk.ID];
                chunkData.SetDataTo([]);
                InfoMessage("Chunk deleted successfully." +
                            "(You should probably save and reload the file!!!)");
            }
            catch (Exception ex) {
                ErrorMessage("Error while deleting:\r\n\r\n" + ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Opens a dialog to import all textures to an MPD file.
        /// </summary>
        /// <param name="mpdFile">The MPD file to load textures into.</param>
        /// <returns>'true' if an import was successful, otherwise 'false'.</returns>
        public bool ImportAllMPDTexturesDialog(IMPD_File mpdFile) {
            using (var dialog = new CommonOpenFileDialog() {
                Title = "Import Textures",
                IsFolderPicker = true,
                Multiselect = false,
                EnsureValidNames = true,
                EnsureFileExists = true,
                EnsurePathExists = true,
            }) {
                if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
                    return false;

                try {
                    var path = dialog.FileName;
                    var files = Directory.GetFiles(path);

                    var results = mpdFile.ReplaceTexturesFromFiles(files, (f) => Image.FromFile(f).Get2DDataABGR1555());

                    var message =
                        "Import complete.\n" +
                        "   Imported successfully: " + results.Replaced + "\n" +
                        "   Textures missing: " + results.Missing + "\n" +
                        "   Failed: " + results.Failed + "\n" +
                        "   Ignored (256-color not yet supported): " + results.Skipped;

                    InfoMessage(message);
                }
                catch (Exception ex) {
                    ErrorMessage("Error while importing:\r\n\r\n" + ex.Message);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Opens a dialog to export all textures to an MPD file.
        /// </summary>
        /// <param name="mpdFile">The MPD file to export textures from.</param>
        /// <returns>'true' if an export was successful, otherwise 'false'.</returns>
        public bool ExportAllMPDTexturesDialog(IMPD_File mpdFile) {
            using (var dialog = new CommonOpenFileDialog() {
                Title = "Export Textures to Folder",
                IsFolderPicker = true,
                Multiselect = false,
                EnsureValidNames = false,
                EnsureFileExists = false,
                EnsurePathExists = false, // Not respected!! User still has to select a folder that exists :( :(
            }) {
                if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
                    return false;

                var path = dialog.FileName;

                try {
                    var results = mpdFile.ExportTexturesToPath(path, (f, data) => ImageUtils.SaveBitmapToFile(f, data, ImageFormat.Png));
                    var message =
                        "Export complete.\n" +
                        "   Exported successfully: " + results.Exported + "\n" +
                        "   Failed: " + results.Failed + "\n" +
                        "   Ignored (256-color not yet supported): " + results.Skipped;

                    InfoMessage(message);
                }
                catch (Exception ex) {
                    ErrorMessage("Error while exporting:\r\n\r\n" + ex.Message);
                    return false;
                }

                // Automatically open the folder, just for fun.
                try {
                    using (Process.Start(new ProcessStartInfo(path) { UseShellExecute = true }))
#pragma warning disable CS0642 // Possible mistaken empty statement
                        ;
#pragma warning restore CS0642 // Possible mistaken empty statement
                }
                catch { }
            }

            return true;
        }

        private void tsmiMPD_Textures_ImportAll_Click(object sender, EventArgs e) {
            if (SelectedFile?.FileType == SF3FileType.MPD)
                ImportAllMPDTexturesDialog((IMPD_File) SelectedFile.Loader.Model);
        }

        private void tsmiMPD_Textures_ExportAll_Click(object sender, EventArgs e) {
            if (SelectedFile?.FileType == SF3FileType.MPD)
                ExportAllMPDTexturesDialog((IMPD_File) SelectedFile.Loader.Model);
        }

        private void tsmiMPD_Chunks_ExportChunk_Click(object sender, EventArgs e) {
            if (SelectedFile?.FileType == SF3FileType.MPD)
                ExportMPDChunkDialog((IMPD_File) SelectedFile.Loader.Model, Path.GetFileNameWithoutExtension(SelectedFile.Loader.ShortFilename));
        }

        private void tsmiMPD_Chunks_ImportChunk_Click(object sender, EventArgs e) {
            if (SelectedFile?.FileType == SF3FileType.MPD)
                ImportMPDChunkDialog((IMPD_File) SelectedFile.Loader.Model, Path.GetFileNameWithoutExtension(SelectedFile.Loader.ShortFilename));
        }

        private void tsmiMPD_Chunks_DeleteChunk_Click(object sender, EventArgs e) {
            if (SelectedFile?.FileType == SF3FileType.MPD)
                DeleteMPDChunkDialog((IMPD_File) SelectedFile.Loader.Model, Path.GetFileNameWithoutExtension(SelectedFile.Loader.ShortFilename));
        }

        private void tsmiMPD_RecalculateSurfaceModelNormals_Click(object sender, EventArgs e) {
            if (SelectedFile?.FileType == SF3FileType.MPD) {
                var mpdFile           = (IMPD_File) SelectedFile.Loader.Model;
                mpdFile.SurfaceModel?.UpdateVertexNormals(mpdFile.Surface?.HeightmapRowTable, _appState.MakeNormalCalculationSettings());

                var mpdView = (MPD_View) (SelectedFile.View.ActualView);
                mpdView.UpdateViewerMap();
            }
        }

        private void tsmiMPD_Chunks_RecompressModifiedChunks_Click(object sender, EventArgs e) {
            if (SelectedFile?.FileType == SF3FileType.MPD)
                ((IMPD_File) SelectedFile.Loader.Model).RecompressChunks(onlyModified: true);
        }

        private void tsmiMPD_Chunks_RecompressAllChunks_Click(object sender, EventArgs e) {
            if (SelectedFile?.FileType == SF3FileType.MPD)
                ((IMPD_File) SelectedFile.Loader.Model).RecompressChunks(onlyModified: false);
        }

        private void tsmiMPD_Chunks_RebuildChunkTable_Click(object sender, EventArgs e) {
            if (SelectedFile?.FileType == SF3FileType.MPD)
                ((IMPD_File) SelectedFile.Loader.Model).RebuildChunkTable();
        }

        private void UpdateMPD_ModelSwitchGroupsMenu(IMPD_File? mpdFile) {
            var items = tsmiMPD_ModelSwitchGroups.DropDown.Items;

            items.Clear();
            int itemIndex = 1;
            if (mpdFile?.ModelSwitchGroupsTable?.Rows?.Length > 0) {
                var ngc = mpdFile.NameGetterContext;
                foreach (var msg in mpdFile.ModelSwitchGroupsTable) {
                    var flag = msg.Flag;
                    var flagName = ngc.GetName(null, null, msg.Flag, [NamedValueType.GameFlag]) ?? "";

                    var newItem = new ToolStripMenuItem(
                        $"&{itemIndex} - Flag {flag:X3}" + (flagName == "" ? "" : $" ({flagName})"),
                        null,
                        null,
                        $"tsbMPD_ModelSwitchGroups_Item{itemIndex}"
                    );
                    newItem.Checked = msg.StateInEditor;
                    newItem.Click += (s, e) => ToggleModelSwitchGroup(mpdFile, msg, newItem);

                    _ = items.Add(newItem);

                    itemIndex++;
                }
            }

            tsmiMPD_ModelSwitchGroups.Enabled = items.Count > 0;
        }

        private void ToggleModelSwitchGroup(IMPD_File mpdFile, ModelSwitchGroup msg, ToolStripMenuItem item) {
            item.Checked = msg.StateInEditor = !msg.StateInEditor;
            if (SelectedFile?.View?.ActualView is MPD_View mpdView && mpdView.Model == mpdFile)
                mpdView.UpdateViewerMap();
        }
    }
}
