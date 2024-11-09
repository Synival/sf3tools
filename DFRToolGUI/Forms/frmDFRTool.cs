using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using DFRLib;
using static CommonLib.Win.Utils.MessageUtils;

namespace DFRTool.GUI.Forms {
    public partial class frmDFRTool : Form {
        bool _isDialogMode = true;

        /// <summary>
        /// Initializes the DFRToolGUI as a standalone application.
        /// </summary>
        public frmDFRTool() {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes the DFRToolGUI for use inside an editor, with specific data for the "altered" file.
        /// </summary>
        /// <param name="data"></param>
        public frmDFRTool(byte[] data) {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimizeBox = false;
            InitializeComponent();

            btnCreate_AlteredFile.Enabled = false;
            tbCreate_AlteredFile.Enabled = false;
            tbCreate_AlteredFile.Text = "(Data Read from Editor)";
            Data = data;
            _isDialogMode = true;
        }

        protected override bool ProcessDialogKey(Keys keyData) {
            if (_isDialogMode && ModifierKeys == Keys.None && keyData == Keys.Escape) {
                Close();
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }

        private void btnCreate_OriginalFile_Click(object sender, EventArgs e) {
            var dialog = new OpenFileDialog {
                Filter = "BIN Files (*.BIN)|*.BIN|All Files (*.*)|*.*"
            };
            if (dialog.ShowDialog() != DialogResult.OK)
                return;
            tbCreate_OriginalFile.Text = dialog.FileName;
        }

        private void btnCreate_AlteredFile_Click(object sender, EventArgs e) {
            var dialog = new OpenFileDialog {
                Filter = "BIN Files (*.BIN)|*.BIN|All Files (*.*)|*.*"
            };
            if (dialog.ShowDialog() != DialogResult.OK)
                return;
            tbCreate_AlteredFile.Text = dialog.FileName;
        }

        private void btnCreate_OutputFile_Click(object sender, EventArgs e) {
            var dialog = new SaveFileDialog();

            var originalFileSplit = tbCreate_OriginalFile.Text.Split('\\');
            var suggestedName = (originalFileSplit.Length >= 1 && tbCreate_OriginalFile.Text != "")
                ? originalFileSplit[originalFileSplit.Length - 1] + ".DFR"
                : "Patch.BIN.DFR";

            dialog.Filter = "DFR Files (*.DFR)|*.DFR|All Files (*.*)|*.*";
            dialog.FileName = suggestedName;

            if (dialog.ShowDialog() != DialogResult.OK)
                return;
            tbCreate_OutputFile.Text = dialog.FileName;
        }

        private void btnCreate_GenerateDFR_Click(object sender, EventArgs e) {
            if (tbCreate_OriginalFile.Text.Length == 0) {
                InfoMessage("Please select an original file.");
                return;
            }

            if (Data == null && tbCreate_AlteredFile.Text.Length == 0) {
                InfoMessage("Please select an altered file.");
                return;
            }

            if (tbCreate_OutputFile.Text.Length == 0) {
                InfoMessage("Please select a destination for the DFR file.");
                return;
            }

            try {
                string dfrText = null;
                using (Stream origStream = new FileStream(tbCreate_OriginalFile.Text, FileMode.Open, FileAccess.Read),
                              alteredStream = (Data != null)
                                ? (Stream) new MemoryStream(Data)
                                : new FileStream(tbCreate_AlteredFile.Text, FileMode.Open, FileAccess.Read)) {
                    var diffChunk = new ByteDiff(origStream, alteredStream, new ByteDiffChunkBuilderOptions {
                        CombineAppendedChunks = cbCreate_CombineAllAppendedData.Checked
                    });
                    dfrText = diffChunk.ToDFR();
                }
                File.WriteAllText(tbCreate_OutputFile.Text, dfrText);
            }
            catch (Exception ex) {
                ErrorMessage("DFR generation failed:\n\n" + ex.Message);
                return;
            }

            InfoMessage("DFR file generated successfully.");

            if (cbCreate_OpenWhenGenerated.Checked) {
                _ = new Process {
                    StartInfo = new ProcessStartInfo(tbCreate_OutputFile.Text) {
                        UseShellExecute = true
                    }
                }.Start();
            }
        }

        private byte[] Data { get; } = null;
    }
}
