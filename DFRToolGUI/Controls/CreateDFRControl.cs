using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using DFRLib;
using static CommonLib.Win.Utils.MessageUtils;

namespace DFRTool.GUI.Controls {
    public partial class CreateDFRControl : UserControl {
        public CreateDFRControl() {
            InitializeComponent();

        }

        private byte[] _alteredData = null;

        /// <summary>
        /// When set, the control will use explicit data instead of data from a file.
        /// </summary>
        public byte[] AlteredData {
            get => _alteredData;
            set {
                if (_alteredData != value) {
                    _alteredData = value;
                    btnAlteredFile.Enabled = (value == null);
                    tbAlteredFile.Enabled = (value == null);
                    tbAlteredFile.Text = (value == null) ? "" : "(Data Read from Editor)";
                }
            }
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

            if (AlteredData == null && tbAlteredFile.Text.Length == 0) {
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
                              alteredStream = (AlteredData != null)
                                ? (Stream) new MemoryStream(AlteredData)
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
    }
}
