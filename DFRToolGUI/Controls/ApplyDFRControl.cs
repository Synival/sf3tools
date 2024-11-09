using System;
using System.IO;
using System.Windows.Forms;
using DFRLib;
using static CommonLib.Win.Utils.MessageUtils;

namespace DFRTool.GUI.Controls {
    public partial class ApplyDFRControl : UserControl {
        private const string c_changesAppliedToOriginalFile = "(Changes Applied to Original File)";

        public ApplyDFRControl() {
            InitializeComponent();
            tbOutputFile.Text = cbApplyToOriginalFile.Checked ? c_changesAppliedToOriginalFile : "";
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

        private void cbApplyToOriginalFile_CheckedChanged(object sender, EventArgs e) {
            bool isChecked = cbApplyToOriginalFile.Checked;
            btnOutputFile.Enabled = !isChecked;
            tbOutputFile.Enabled = !isChecked;
            tbOutputFile.Text = isChecked ? c_changesAppliedToOriginalFile : "";
        }

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

            var outputFilename = cbApplyToOriginalFile.Checked ? tbOriginalFile.Text : tbOutputFile.Text;
            if (outputFilename.Length == 0) {
                InfoMessage("Please select a destination for the altered file.");
                return;
            }

            try {
                var input = File.ReadAllBytes(tbOriginalFile.Text);
                var diff = new ByteDiff(tbDFRFile.Text);
                var output = diff.ApplyTo(input);
                File.WriteAllBytes(outputFilename, output);
            }
            catch (Exception ex) {
                ErrorMessage("DFR application failed:\n\n" + ex.Message);
                return;
            }

            InfoMessage("DFR file applied successfully.");
            ApplyDFR(this, EventArgs.Empty);
        }

        /// <summary>
        /// Event triggered after a DFR file is successfully applied.
        /// </summary>
        public event EventHandler ApplyDFR;
    }
}
