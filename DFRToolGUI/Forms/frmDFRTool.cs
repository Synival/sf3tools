using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using DFRLib;

namespace DFRToolGUI.Forms {
    public partial class frmDFRTool : Form {
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
            InitializeComponent();
            this.btnAlteredFile.Enabled = false;
            this.tbAlteredFile.Enabled = false;
            this.tbAlteredFile.Text = "(reading from editor)";

            Data = data;
        }

        private void btnOriginalFile_Click(object sender, EventArgs e) {
            var dialog = new OpenFileDialog();
            dialog.Filter = "BIN Files (*.BIN)|*.BIN|All Files (*.*)|*.*";
            if (dialog.ShowDialog() != DialogResult.OK)
                return;
            tbOriginalFile.Text = dialog.FileName;
        }

        private void btnAlteredFile_Click(object sender, EventArgs e) {
            var dialog = new OpenFileDialog();
            dialog.Filter = "BIN Files (*.BIN)|*.BIN|All Files (*.*)|*.*";
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
            const string messageBoxTitle = "DFRTool";

            if (tbOriginalFile.Text.Length == 0) {
                MessageBox.Show("Please select an original file.", messageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (Data == null && tbAlteredFile.Text.Length == 0) {
                MessageBox.Show("Please select an altered file.", messageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (tbOutputFile.Text.Length == 0) {
                MessageBox.Show("Please select a destination for the DFR file.", messageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try {
                FileStream origStream = null;
                Stream alteredStream = null;
                string dfrText = null;

                try {
                    origStream = new FileStream(tbOriginalFile.Text, FileMode.Open, FileAccess.Read);
                    alteredStream = (Data != null)
                        ? (Stream) new MemoryStream(Data)
                        : new FileStream(tbAlteredFile.Text, FileMode.Open, FileAccess.Read);

                    var diffChunk = new ByteDiff(origStream, alteredStream, new ByteDiffChunkBuilderOptions {
                        CombineAppendedChunks = cbCombineAllAppendedData.Checked
                    });
                    dfrText = diffChunk.ToDFR();
                }
                catch {
                    throw;
                }
                finally {
                    if (origStream != null)
                        origStream.Close();
                    if (alteredStream != null)
                        alteredStream.Close();
                }

                File.WriteAllText(tbOutputFile.Text, dfrText);
            }
            catch (Exception ex) {
                MessageBox.Show("DFR generation failed:\n\n" + ex.Message, messageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            MessageBox.Show("DFR file generated successfully.", messageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (cbOpenWhenGenerated.Checked) {
                new Process {
                    StartInfo = new ProcessStartInfo(tbOutputFile.Text) {
                        UseShellExecute = true
                    }
                }.Start();
            }
        }

        private byte[] Data { get; } = null;
    }
}
