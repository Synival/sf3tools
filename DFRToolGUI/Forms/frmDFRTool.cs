using DFRLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DFRToolGUI.Forms
{
    public partial class frmDFRTool : Form
    {
        public frmDFRTool()
        {
            InitializeComponent();
        }

        private void btnOriginalFile_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = ".BIN files (*.BIN)|*.BIN|All files (*.*)|*.*";
            if (dialog.ShowDialog() != DialogResult.OK)
                return;
            tbOriginalFile.Text = dialog.FileName;
        }

        private void btnAlteredFile_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = ".BIN files (*.BIN)|*.BIN|All files (*.*)|*.*";
            if (dialog.ShowDialog() != DialogResult.OK)
                return;
            tbAlteredFile.Text = dialog.FileName;
        }

        private void btnOutputFile_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog();

            var originalFileSplit = tbOriginalFile.Text.Split('\\');
            var suggestedName = (originalFileSplit.Length >= 1 && tbOriginalFile.Text != "")
                ? originalFileSplit[originalFileSplit.Length - 1] + ".DFR"
                : "Patch.BIN.DFR";

            dialog.Filter = ".DFR files (*.DFR)|*.DFR|All files (*.*)|*.*";
            dialog.FileName = suggestedName;

            if (dialog.ShowDialog() != DialogResult.OK)
                return;
            tbOutputFile.Text = dialog.FileName;
        }

        private void btnGenerateDFR_Click(object sender, EventArgs e)
        {
            const string messageBoxTitle = "DFRTool";

            if (tbOriginalFile.Text.Length == 0)
            {
                MessageBox.Show("Please select an original file.", messageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (tbAlteredFile.Text.Length == 0)
            {
                MessageBox.Show("Please select an altered file.", messageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (tbOutputFile.Text.Length == 0)
            {
                MessageBox.Show("Please select a destination for the DFR file.", messageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var diffChunk = new ByteDiff(tbOriginalFile.Text, tbAlteredFile.Text, new ByteDiffChunkBuilderOptions
                {
                    CombineAppendedChunks = cbCombineAllAppendedData.Checked
                });
                var dfrText = diffChunk.ToDFR();
                File.WriteAllText(tbOutputFile.Text, dfrText);
            }
            catch (Exception ex)
            {
                MessageBox.Show("DFR generation failed:\n\n" + ex.Message, messageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            MessageBox.Show("DFR file generated successfully.", messageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (cbOpenWhenGenerated.Checked)
            {
                new Process
                {
                    StartInfo = new ProcessStartInfo(tbOutputFile.Text)
                    {
                        UseShellExecute = true
                    }
                }.Start();
            }
        }
    }
}
