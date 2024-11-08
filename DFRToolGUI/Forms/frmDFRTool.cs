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

            btnAlteredFile.Enabled = false;
            tbAlteredFile.Enabled = false;
            tbAlteredFile.Text = "(Data Read from Editor)";
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

        private void btnOriginalFile_Click(object sender, EventArgs e) {
            var dialog = new OpenFileDialog {
                Filter = "BIN Files (*.BIN)|*.BIN|All Files (*.*)|*.*"
            };
            if (dialog.ShowDialog() != DialogResult.OK)
                return;
            tbOriginalFile.Text = dialog.FileName;
        }

        private void btnAlteredFile_Click(object sender, EventArgs e) {
            var dialog = new OpenFileDialog {
                Filter = "BIN Files (*.BIN)|*.BIN|All Files (*.*)|*.*"
            };
            if (dialog.ShowDialog() != DialogResult.OK)
                return;
            tbAlteredFile.Text = dialog.FileName;
        }

        private void btnOutputFile_Click(object sender, EventArgs e) {
            var dialog = new SaveFileDialog();

            var originalFileSplit = tbOriginalFile.Text.Split('\\');
            var suggestedName = (originalFileSplit.Length >= 1 && tbOriginalFile.Text != "")
                ? originalFileSplit[originalFileSplit.Length - 1] + ".DFR"
                : "Patch.BIN.DFR";

            dialog.Filter = "DFR Files (*.DFR)|*.DFR|All Files (*.*)|*.*";
            dialog.FileName = suggestedName;

            if (dialog.ShowDialog() != DialogResult.OK)
                return;
            tbOutputFile.Text = dialog.FileName;
        }

        private void btnGenerateDFR_Click(object sender, EventArgs e) {
            if (tbOriginalFile.Text.Length == 0) {
                InfoMessage("Please select an original file.");
                return;
            }

            if (Data == null && tbAlteredFile.Text.Length == 0) {
                InfoMessage("Please select an altered file.");
                return;
            }

            if (tbOutputFile.Text.Length == 0) {
                InfoMessage("Please select a destination for the DFR file.");
                return;
            }

            try {
                string dfrText = null;
                using (Stream origStream = new FileStream(tbOriginalFile.Text, FileMode.Open, FileAccess.Read),
                              alteredStream = (Data != null)
                                ? (Stream) new MemoryStream(Data)
                                : new FileStream(tbAlteredFile.Text, FileMode.Open, FileAccess.Read)) {
                    var diffChunk = new ByteDiff(origStream, alteredStream, new ByteDiffChunkBuilderOptions {
                        CombineAppendedChunks = cbCombineAllAppendedData.Checked
                    });
                    dfrText = diffChunk.ToDFR();
                }
                File.WriteAllText(tbOutputFile.Text, dfrText);
            }
            catch (Exception ex) {
                ErrorMessage("DFR generation failed:\n\n" + ex.Message);
                return;
            }

            InfoMessage("DFR file generated successfully.");

            if (cbOpenWhenGenerated.Checked) {
                _ = new Process {
                    StartInfo = new ProcessStartInfo(tbOutputFile.Text) {
                        UseShellExecute = true
                    }
                }.Start();
            }
        }

        private byte[] Data { get; } = null;
    }
}
