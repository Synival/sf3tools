using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CommonLib.Imaging;
using CommonLib.NamedValues;
using Microsoft.WindowsAPICodePack.Dialogs;
using SF3.ModelLoaders;
using SF3.Models.Files;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.TextureAnimation;
using SF3.Models.Structs.MPD.TextureChunk;
using SF3.NamedValues;
using SF3.Types;
using SF3.Win.Forms;
using SF3.Win.Types;
using SF3.Win.Views;
using SF3.Win.Views.MPD;
using static CommonLib.Imaging.PixelConversion;
using static CommonLib.Utils.Compression;
using static CommonLib.Win.Utils.MessageUtils;

namespace SF3.MPD_Editor.Forms {
    public partial class frmMPDEditor : EditorFormNew {
        // Used to display version in the application
        protected override string Version => "0.2 (2025-02-26 dev)";

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

                var textures1 = (File.TextureCollections == null) ? [] : File.TextureCollections
                    .Where(x => x != null && x.TextureTable != null)
                    .SelectMany(x => x.TextureTable)
                    .Where(x => x.TextureIsLoaded && x.Collection == TextureCollectionType.PrimaryTextures)
                    .ToDictionary(x => x.Name, x => new { Model = (object) x, x.Texture });

                var textures2 = (File.TextureAnimations == null) ? [] : File.TextureAnimations
                    .SelectMany(x => x.FrameTable)
                    .GroupBy(x => x.CompressedImageDataOffset)
                    .Select(x => x.First())
                    .Where(x => x.TextureIsLoaded)
                    .ToDictionary(x => x.Name, x => new { Model = (object) x, x.Texture });

                var textures = textures1.Concat(textures2).ToDictionary(x => x.Key, x => x.Value);

                int succeeded = 0;
                int failed = 0;
                int missing = 0;
                int skipped = 0;

                foreach (var textureKv in textures) {
                    var name = textureKv.Key;
                    var model = textureKv.Value.Model;
                    var texture = textureKv.Value.Texture;

                    if (texture.PixelFormat != TexturePixelFormat.ABGR1555) {
                        skipped++;
                        continue;
                    }

                    var filename = files.FirstOrDefault(x => Path.GetFileNameWithoutExtension(x).ToLower() == name.ToLower());
                    if (filename == null) {
                        missing++;
                        continue;
                    }

                    // Try to actually load the texture!
                    try {
                        var image = System.Drawing.Image.FromFile(filename);
                        if (image.Width != texture.Width || image.Height != texture.Height) {
                            failed++;
                            continue;
                        }

                        if (texture.PixelFormat == TexturePixelFormat.ABGR1555) {
                            using (var bitmap = new Bitmap(texture.Width, texture.Height, PixelFormat.Format32bppArgb)) {
                                using (var graphics = Graphics.FromImage(bitmap)) {
                                    graphics.DrawImage(image, new Point(0, 0));
                                }
                                var readBytes = new byte[texture.Width * texture.Height * 4];

                                var bitmapData = bitmap.LockBits(new Rectangle(0, 0, texture.Width, texture.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
                                Marshal.Copy(bitmapData.Scan0, readBytes, 0, readBytes.Length);
                                bitmap.UnlockBits(bitmapData);

                                var newImageData = new ushort[texture.Width, texture.Height];
                                int pos = 0;
                                for (int y = 0; y < texture.Height; y++) {
                                    for (int x = 0; x < texture.Width; x++) {
                                        var byteB = readBytes[pos++];
                                        var byteG = readBytes[pos++];
                                        var byteR = readBytes[pos++];
                                        var byteA = readBytes[pos++];

                                        var channels = new PixelChannels() { a = byteA, r = byteR, g = byteG, b = byteB };
                                        var abgr1555 = channels.ToABGR1555();

                                        // If the alpha channel is clear, preserve whatever color was originally used.
                                        // This should prevent marking data as 'modified' too often.
                                        if ((abgr1555 & 0x8000) == 0) {
                                            var cached = ABGR1555toChannels(texture.ImageData16Bit[x, y]);
                                            cached.a = 0;
                                            abgr1555 = cached.ToABGR1555();
                                        }

                                        newImageData[x, y] = abgr1555;
                                    }
                                }

                                if (model is TextureModel tm)
                                    tm.RawImageData16Bit = newImageData;
                                else if (model is FrameModel fm) {
                                    var referenceTex = File.TextureCollections.Where(x => x != null).Select(x => x.TextureTable).SelectMany(x => x).FirstOrDefault(x => x.ID == fm.TextureID)?.Texture;
                                    _ = fm.UpdateTextureABGR1555(File.Chunk3Frames.First(x => x.Offset == fm.CompressedImageDataOffset).Data.DecompressedData, newImageData, referenceTex);
                                }
                                else
                                    throw new NotSupportedException("Not sure what this is, but it's not supported here");
                            }
                            succeeded++;
                        }
                        else
                            skipped++;
                    }
                    catch {
                        failed++;
                    }
                }

                var message =
                    "Import complete.\n" +
                    "   Imported successfully: " + succeeded + "\n" +
                    "   Textures missing: " + missing + "\n" +
                    "   Failed: " + failed + "\n" +
                    "   Ignored (256-color not yet supported): " + skipped;
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

                var textures1 = (File.TextureCollections == null) ? [] : File.TextureCollections
                    .Where(x => x != null && x.TextureTable != null)
                    .SelectMany(x => x.TextureTable)
                    .Where(x => x.TextureIsLoaded && x.Collection == TextureCollectionType.PrimaryTextures)
                    .ToDictionary(x => x.Name, x => x.Texture);

                var textures2 = (File.TextureAnimations == null) ? [] : File.TextureAnimations
                    .SelectMany(x => x.FrameTable)
                    .GroupBy(x => x.CompressedImageDataOffset)
                    .Select(x => x.First())
                    .Where(x => x.TextureIsLoaded)
                    .ToDictionary(x => x.Name, x => x.Texture);

                var textures = textures1.Concat(textures2).ToDictionary(x => x.Key, x => x.Value);

                int succeeded = 0;
                int failed = 0;
                int skipped = 0;

                foreach (var textureKv in textures) {
                    var name = textureKv.Key;
                    var texture = textureKv.Value;

                    var filename = Path.Combine(path, name + ".png");
                    try {
                        if (texture.PixelFormat == TexturePixelFormat.ABGR1555) {
                            using (var bitmap = new Bitmap(texture.Width, texture.Height, PixelFormat.Format16bppArgb1555)) {
                                var bitmapData = bitmap.LockBits(new Rectangle(0, 0, texture.Width, texture.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
                                Marshal.Copy(texture.BitmapDataARGB1555, 0, bitmapData.Scan0, texture.BitmapDataARGB1555.Length);
                                bitmap.UnlockBits(bitmapData);
                                bitmap.Save(filename, ImageFormat.Png);
                            }
                            succeeded++;
                        }
                        else
                            skipped++;
                    }
                    catch {
                        failed++;
                    }
                }

                var message =
                    "Export complete.\n" +
                    "   Exported successfully: " + succeeded + "\n" +
                    "   Failed: " + failed + "\n" +
                    "   Ignored (256-color not yet supported): " + skipped;
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
    }
}
