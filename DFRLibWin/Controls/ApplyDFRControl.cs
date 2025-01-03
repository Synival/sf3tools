using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static CommonLib.Win.Utils.MessageUtils;

namespace DFRLib.Win.Controls {
    public partial class ApplyDFRControl : UserControl {
        private const string c_dataReadFromEditor           = "(Data Read from Editor)";
        private const string c_changesAppliedToEditorData   = "(Changes Applied to Editor Data)";
        private const string c_changesAppliedToInputFile = "(Changes Applied to Input File)";

        private string _lastValidOutputFile = "";

        public ApplyDFRControl() {
            SuspendLayout();
            InitializeComponent();
            labelSelectInputFiles.Font = new Font(labelSelectInputFiles.Font, FontStyle.Bold);
            labelSelectOutputDestination.Font = new Font(labelSelectOutputDestination.Font, FontStyle.Bold);
            UpdateOutputFileControls();
            ResumeLayout();
        }

        bool _updatingOutputFileControls = false;

        private void UpdateOutputFileControls() {
            if (_updatingOutputFileControls)
                return;
            _updatingOutputFileControls = true;

            if (ApplyInMemory)
                this.cbApplyToInputFile.Enabled = false;

            // Do nothing if the control was enabled and should stay enabled.
            bool shouldBeEnabled = !cbApplyToInputFile.Checked && !ApplyInMemory;
            if (shouldBeEnabled && tbOutputFile.Enabled)
                return;

            this.cbApplyToInputFile.Enabled = !ApplyInMemory;

            // Remember the last enabled value so we can put it back later if necessary.
            if (tbOutputFile.Enabled)
                _lastValidOutputFile = tbOutputFile.Text;

            btnOutputFile.Enabled = shouldBeEnabled;
            tbOutputFile.Enabled  = shouldBeEnabled;

            tbOutputFile.Text = shouldBeEnabled
                ? _lastValidOutputFile
                : ApplyInMemory
                    ? c_changesAppliedToEditorData
                    : c_changesAppliedToInputFile;

            _updatingOutputFileControls = false;
        }

        private void btnInputFile_Click(object sender, EventArgs e) {
            var dialog = new OpenFileDialog {
                Filter = "BIN Files (*.BIN)|*.BIN|All Files (*.*)|*.*"
            };
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            tbInputFile.Text = dialog.FileName;
        }

        private void btnDFRFile_Click(object sender, EventArgs e) {
            var dialog = new OpenFileDialog();

            var inputFileSplit = tbInputFile.Text.Split('\\');
            var suggestedName = (inputFileSplit.Length >= 1 && tbInputFile.Text != "")
                ? inputFileSplit[inputFileSplit.Length - 1] + ".DFR"
                : "Patch.BIN.DFR";

            dialog.Filter = "DFR Files (*.DFR)|*.DFR|All Files (*.*)|*.*";
            dialog.FileName = suggestedName;

            if (dialog.ShowDialog() != DialogResult.OK)
                return;
            tbDFRFile.Text = dialog.FileName;
        }

        private void cbApplyToInputFile_CheckedChanged(object sender, EventArgs e) => UpdateOutputFileControls();

        private void btnOutputFile_Click(object sender, EventArgs e) {
            if (cbApplyToInputFile.Checked)
                return;

            var dialog = new SaveFileDialog {
                Filter = "BIN Files (*.BIN)|*.BIN|All Files (*.*)|*.*"
            };
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            tbOutputFile.Text = dialog.FileName;
        }

        private void btnApplyDFR_Click(object sender, EventArgs e) {
            if (InputData == null && tbInputFile.Text.Length == 0) {
                InfoMessage("Please select an input file.");
                return;
            }

            if (tbDFRFile.Text.Length == 0) {
                InfoMessage("Please select a DFR file.");
                return;
            }

            string? outputFilename = null;
            if (!ApplyInMemory) {
                outputFilename = cbApplyToInputFile.Checked ? tbInputFile.Text : tbOutputFile.Text;
                if (outputFilename.Length == 0) {
                    InfoMessage("Please select a destination for the patched file.");
                    return;
                }
            }

            try {
                var input = InputData ?? File.ReadAllBytes(tbInputFile.Text);
                var diff = new ByteDiff(tbDFRFile.Text);
                var output = diff.ApplyTo(input);

                if (ApplyInMemory)
                    InMemoryOutput = output;
                else
                    File.WriteAllBytes(outputFilename!, output);
            }
            catch (Exception ex) {
                ErrorMessage("DFR application failed:\n\n" + ex.Message);
                return;
            }

            InfoMessage("DFR file applied successfully.");
            ApplyDFR?.Invoke(this, EventArgs.Empty);
        }

        private byte[]? _inputData = null;

        /// <summary>
        /// When set, the control will use explicit "input file" data instead of an actual file.
        /// </summary>
        public byte[]? InputData {
            get => _inputData;
            set {
                if (_inputData != value) {
                    _inputData = value;
                    btnInputFile.Enabled = (value == null);
                    tbInputFile.Enabled = (value == null);
                    tbInputFile.Text = (value == null) ? "" : c_dataReadFromEditor;
                }
            }
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
        public byte[]? InMemoryOutput { get; private set; } = null;

        /// <summary>
        /// Event triggered after a DFR file is successfully applied.
        /// </summary>
        public event EventHandler? ApplyDFR;
    }
}
