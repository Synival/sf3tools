using System;
using System.IO;
using System.Windows.Forms;
using DFRLib;
using static CommonLib.Win.Utils.MessageUtils;

namespace DFRTool.GUI.Controls {
    public partial class ApplyDFRControl : UserControl {
        private const string c_changesAppliedToEditorData = "(Changes Applied to Editor Data)";
        private const string c_changesAppliedToOriginalFile = "(Changes Applied to Original File)";

        private string _lastValidOutputFile = "";

        public ApplyDFRControl() {
            InitializeComponent();
            UpdateOutputFileControls();
        }

        bool _updatingOutputFileControls = false;

        private void UpdateOutputFileControls() {
            if (_updatingOutputFileControls)
                return;
            _updatingOutputFileControls = true;

            if (ApplyInMemory)
                this.cbApplyToOriginalFile.Enabled = false;

            // Do nothing if the control was enabled and should stay enabled.
            bool shouldBeEnabled = !cbApplyToOriginalFile.Checked && !ApplyInMemory;
            if (shouldBeEnabled && tbOutputFile.Enabled)
                return;

            this.cbApplyToOriginalFile.Enabled = !ApplyInMemory;

            // Remember the last enabled value so we can put it back later if necessary.
            if (tbOutputFile.Enabled)
                _lastValidOutputFile = tbOutputFile.Text;

            btnOutputFile.Enabled = shouldBeEnabled;
            tbOutputFile.Enabled  = shouldBeEnabled;

            tbOutputFile.Text = shouldBeEnabled
                ? _lastValidOutputFile
                : ApplyInMemory
                    ? c_changesAppliedToEditorData
                    : c_changesAppliedToOriginalFile;

            _updatingOutputFileControls = false;
        }

        private void btnOriginalFile_Click(object sender, EventArgs e) {
            var dialog = new OpenFileDialog {
                Filter = "BIN Files (*.BIN)|*.BIN|All Files (*.*)|*.*"
            };
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            tbOriginalFile.Text = dialog.FileName;
        }

        private void btnDFRFile_Click(object sender, EventArgs e) {
            var dialog = new OpenFileDialog();

            var originalFileSplit = tbOriginalFile.Text.Split('\\');
            var suggestedName = (originalFileSplit.Length >= 1 && tbOriginalFile.Text != "")
                ? originalFileSplit[originalFileSplit.Length - 1] + ".DFR"
                : "Patch.BIN.DFR";

            dialog.Filter = "DFR Files (*.DFR)|*.DFR|All Files (*.*)|*.*";
            dialog.FileName = suggestedName;

            if (dialog.ShowDialog() != DialogResult.OK)
                return;
            tbDFRFile.Text = dialog.FileName;
        }

        private void cbApplyToOriginalFile_CheckedChanged(object sender, EventArgs e) => UpdateOutputFileControls();

        private void btnOutputFile_Click(object sender, EventArgs e) {
            if (cbApplyToOriginalFile.Checked)
                return;

            var dialog = new SaveFileDialog {
                Filter = "BIN Files (*.BIN)|*.BIN|All Files (*.*)|*.*"
            };
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            tbOutputFile.Text = dialog.FileName;
        }

        private void btnApplyDFR_Click(object sender, EventArgs e) {
            if (tbOriginalFile.Text.Length == 0) {
                InfoMessage("Please select an original file.");
                return;
            }

            if (tbDFRFile.Text.Length == 0) {
                InfoMessage("Please select a DFR file.");
                return;
            }

            string outputFilename = null;
            if (!ApplyInMemory) {
                outputFilename = cbApplyToOriginalFile.Checked ? tbOriginalFile.Text : tbOutputFile.Text;
                if (outputFilename.Length == 0) {
                    InfoMessage("Please select a destination for the altered file.");
                    return;
                }
            }

            try {
                var input = File.ReadAllBytes(tbOriginalFile.Text);
                var diff = new ByteDiff(tbDFRFile.Text);
                var output = diff.ApplyTo(input);

                if (ApplyInMemory)
                    InMemoryOutput = output;
                else
                    File.WriteAllBytes(outputFilename, output);
            }
            catch (Exception ex) {
                ErrorMessage("DFR application failed:\n\n" + ex.Message);
                return;
            }

            InfoMessage("DFR file applied successfully.");
            ApplyDFR(this, EventArgs.Empty);
        }

        private bool _applyInMemory = false;

        /// <summary>
        /// When set, the editor will set its byte[] output to InMemoryOutput rather than output
        /// to a file. The output file selection options are disabled.
        /// </summary>
        public bool ApplyInMemory {
            get => _applyInMemory;
            set {
                if (_applyInMemory != value) {
                    _applyInMemory = value;
                    UpdateOutputFileControls();
                }
            }
        }

        /// <summary>
        /// In-memory output set upon applying a DFR instead of outputting to a file.
        /// Set if ApplyInMemory is 'true'.
        /// </summary>
        public byte[] InMemoryOutput { get; private set; } = null;

        /// <summary>
        /// Event triggered after a DFR file is successfully applied.
        /// </summary>
        public event EventHandler ApplyDFR;
    }
}
