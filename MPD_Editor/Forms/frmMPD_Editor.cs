using System.Windows.Forms;
using SF3.Win.Forms;
using SF3.Editors;
using SF3.Editors.MPD;
using SF3.Loaders;
using SF3.NamedValues;
using SF3.Win.EditorControls.MPD;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;
using SF3.Types;
using static CommonLib.Win.Utils.MessageUtils;
using static CommonLib.Utils.PixelConversion;

namespace SF3.MPD_Editor.Forms {

    public partial class frmMPDEditor : EditorForm {
        // Used to display version in the application
        protected override string Version => "0.3";

        public IMPD_Editor Editor => base.FileLoader.Editor as IMPD_Editor;
        public MPD_EditorControl EditorControl { get; private set; } = null;
        public Control Control { get; private set; } = null;

        public frmMPDEditor() {
            InitializeComponent();
            InitializeEditor(menuStrip2);

            FileIsLoadedChanged += (s, e) => {
                tsmiTextures_ImportFolder.Enabled = IsLoaded;
                tsmiTextures_ExportToFolder.Enabled = IsLoaded;
            };
        }

        protected override string FileDialogFilter
            => "SF3 Data (*.MPD)|*.MPD|" + base.FileDialogFilter;

        protected override IBaseEditor MakeEditor(IFileLoader loader)
            => Editors.MPD.MPD_Editor.Create(loader.RawEditor, new NameGetterContext(Scenario), Scenario);

        protected override bool OnLoad() {
            if (!base.OnLoad())
                return false;

            SuspendLayout();
    
            try {
                EditorControl = new MPD_EditorControl(FileLoader.Filename, Editor);
                Control = EditorControl.Create();
            }
            catch {
                if (EditorControl != null) {
                    EditorControl.Dispose();
                    EditorControl = null;
                }
                if (Control != null) {
                    Control.Dispose();
                    Control = null;
                }
                ResumeLayout();
                return false;
            }

            Control.Dock = DockStyle.Fill;
            Controls.Add(Control);
            Control.BringToFront(); // If this isn't in the front, the menu is placed behind it (eep)
            ResumeLayout();

            return true;
        }

        protected override bool OnClose() {
            bool wasFocused = ContainsFocus;

            SuspendLayout();
            if (Control != null) {
                Controls.Remove(Control);
                Control.Dispose();
                Control = null;
            }
            if (EditorControl != null) {
                EditorControl.Dispose();
                EditorControl = null;
            }
            ResumeLayout();

            if (!base.OnClose())
                return false;

            if (wasFocused && !ContainsFocus)
                Focus();

            return true;
        }

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

                var textures = Editor.TextureChunks
                    .Where(x => x != null && x.TextureTable != null)
                    .SelectMany(x => x.TextureTable.Rows)
                    .ToList();

                int succeeded = 0;
                int failed = 0;
                int missing = 0;
                int skipped = 0;

                foreach (var texture in textures) {
                    if (texture.AssumedPixelFormat != TexturePixelFormat.ABGR1555) {
                        skipped++;
                        continue;
                    }

                    var filename = files.FirstOrDefault(x => Path.GetFileNameWithoutExtension(x).ToLower() == texture.Name.ToLower());
                    if (filename == null) {
                        missing++;
                        continue;
                    }

                    // Try to actually load the texture!
                    try {
                        var image = Image.FromFile(filename);
                        if (image.Width != texture.Width || image.Height != texture.Height) {
                            failed++;
                            continue;
                        }

                        if (texture.AssumedPixelFormat == TexturePixelFormat.ABGR1555) {
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
                                            var cached = ABGR1555toChannels(texture.CachedImageData16Bit[x, y]);
                                            cached.a = 0;
                                            abgr1555 = cached.ToABGR1555();
                                        }

                                        newImageData[x, y] = abgr1555;
                                    }
                                }

                                texture.ImageData16Bit = newImageData;
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

                var textures = Editor.TextureChunks
                    .Where(x => x != null && x.TextureTable != null)
                    .SelectMany(x => x.TextureTable.Rows)
                    .ToList();

                int succeeded = 0;
                int failed = 0;
                int skipped = 0;

                foreach (var texture in textures) {
                    var filename = Path.Combine(path, texture.Name + ".png");
                    try {
                        if (texture.AssumedPixelFormat == Types.TexturePixelFormat.ABGR1555) {
                            using (var bitmap = new Bitmap(texture.Width, texture.Height, PixelFormat.Format16bppArgb1555)) {
                                var bitmapData = bitmap.LockBits(new Rectangle(0, 0, texture.Width, texture.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
                                Marshal.Copy(texture.CachedBitmapDataARGB1555, 0, bitmapData.Scan0, texture.CachedBitmapDataARGB1555.Length);
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
    }
}
