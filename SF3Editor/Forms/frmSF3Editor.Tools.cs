using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using CommonLib.Extensions;
using CommonLib.NamedValues;
using DFRLib.Types;
using DFRLib.Win.Forms;
using SF3.ModelLoaders;
using static CommonLib.Win.Utils.MessageUtils;
using static SF3.Utils.FileUtils;

namespace SF3.Editor.Forms {
    public partial class frmSF3Editor {

        /// <summary>
        /// Creates an "Open" dialog and, if a DFR file was chosen, does the following:
        /// 1. Create a ByteDiff() from the DFR file
        /// 2. Close the current document
        /// 3. Reload the current document, but with the new data instead of its own
        /// </summary>
        /// <param name="file">The loaded file to apply a DFR file to.</param>
        /// <returns>'true' if the DFR was applied and the file was reloaded successfully and the user did not cancel. Otherwise, 'false'.</returns>
        public bool ApplyDFRDialog(LoadedFile file) {
            if (file.Loader.IsModified)
                if (PromptForSave(file) == DialogResult.Cancel)
                    return false;

            var form = new frmDFRTool(CommandType.Apply, dialogMode: true);
            form.ApplyDFRInputData = file.Loader.ByteData.GetDataCopy();
            form.ApplyDFRInMemory = true;
            var dialogResult = form.ShowDialog();
            if (dialogResult != DialogResult.OK)
                return false;

            try {
                var filename = file.Loader.Filename;
                var scenario = file.Scenario;
                var fileType = file.FileType;
                var newBytes = form.ApplyDFRInMemoryOutput;

                if (newBytes == null)
                    throw new NullReferenceException("Internal error: No result from 'Apply DFR' command!");

                if (!CloseFile(file, true))
                    return false;

                LoadedFile? newFile = null;
                using (var newBytesStream = new MemoryStream(newBytes))
                    if ((newFile = LoadFile(filename, scenario, fileType, newBytesStream)) == null)
                        return false;

                newFile.Loader.IsModified = true;
            }
            catch (Exception e) {
                ErrorMessage("Error loading modified data:\n\n" + e.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Opens the DFRToolGUI as a modal dialog with the current editor data as its "Altered File".
        /// </summary>
        /// <param name="file">The loaded file to create a DFR file from.</param>
        /// <returns>'true' if the DFR file was created successfully and the user did not cancel. Otherwise, 'false'.</returns>
        public bool CreateDFRDialog(LoadedFile file) {
            var form = new frmDFRTool(CommandType.Create, dialogMode: true);
            form.CreateDFRAlteredData = file.Loader.ByteData.GetDataCopy();
            var dialogResult = form.ShowDialog();
            return (dialogResult == DialogResult.OK);
        }

        /// <summary>
        /// Opens a dialog to perform a bulk copy of tables to another .BIN file.
        /// </summary>
        public void CopyTablesTo(LoadedFile file) {
            if (file?.Loader?.IsLoaded != true)
                return;

            var saveFileDialog = new SaveFileDialog {
                Title = "Copy Tables To",
                Filter = file.Loader.FileDialogFilter
            };
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;
            var copyToFilename = saveFileDialog.FileName;

            // If we don't know the scenario, we can't load it.
            var scenario = OpenScenario ?? DetermineScenario(copyToFilename, file.FileType);
            if (!scenario.HasValue) {
                ErrorMessage("Can't determine scenario for '" + copyToFilename + "'.");
                return;
            }

            ObjectExtensions.BulkCopyPropertiesResult result = null;
            try {
                var copyModelLoader = new ModelFileLoader();
                if (!copyModelLoader.LoadFile(copyToFilename, null, loader => CreateFile(loader.ByteData, file.FileType, c_nameGetterContexts, scenario.Value))) {
                    ErrorMessage("Error trying to load file. It is probably in use by another process.");
                    return;
                }

                result = file.Loader.Model.BulkCopyProperties(copyModelLoader.Model);
                if (!copyModelLoader.SaveFile(copyToFilename)) {
                    ErrorMessage("Error trying to update file.");
                    return;
                }
            }
            catch (Exception e) {
                //wrong file was selected
                ErrorMessage("Data in '" + copyToFilename + "' appears corrupt or invalid:\n\n" +
                             e.Message + "\n\n" +
                             "Is this the correct type of file?");
                return;
            }

            ProduceAndPresentBulkCopyReport(result, file.Loader.Model.NameGetterContext);
        }

        /// <summary>
        /// Opens a dialog to perform a bulk copy of tables from another .BIN file.
        /// </summary>
        public void CopyTablesFrom(LoadedFile file) {
            if (file?.Loader?.IsLoaded != true)
                return;

            var openFileDialog = new OpenFileDialog {
                Title = "Copy Tables From",
                Filter = file.Loader.FileDialogFilter
            };
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;
            var copyFromFilename = openFileDialog.FileName;

            // If we don't know the scenario, we can't load it.
            var scenario = OpenScenario ?? DetermineScenario(copyFromFilename, file.FileType);
            if (!scenario.HasValue) {
                ErrorMessage("Can't determine scenario for '" + copyFromFilename + "'.");
                return;
            }

            ObjectExtensions.BulkCopyPropertiesResult result = null;
            try {
                var copyModelLoader = new ModelFileLoader();
                if (!copyModelLoader.LoadFile(copyFromFilename, null, loader => CreateFile(loader.ByteData, file.FileType, c_nameGetterContexts, scenario.Value))) {
                    ErrorMessage("Error trying to load file. It is probably in use by another process.");
                    return;
                }
                result = copyModelLoader.Model.BulkCopyProperties(file.Loader.Model);
            }
            catch (Exception e) {
                //wrong file was selected
                ErrorMessage("Data in '" + copyFromFilename + "' appears corrupt or invalid:\n\n" +
                             e.Message + "\n\n" +
                             "Is this the correct type of file?");
                return;
            }

            file.View.RefreshContent();
            ProduceAndPresentBulkCopyReport(result, file.Loader.Model.NameGetterContext);
        }

        private void ProduceAndPresentBulkCopyReport(ObjectExtensions.BulkCopyPropertiesResult result, INameGetterContext nameContext) {
            var copyReport = result.MakeSummaryReport(nameContext);

            // Output summary files.
            var fullReport = result.MakeFullReport(nameContext);
            var wroteBulkCopyReport = false;
            if (fullReport != "") {
                try {
                    File.WriteAllText("BulkCopyReport.txt", fullReport);
                    wroteBulkCopyReport = true;
                    copyReport += "\n\nDetailed reports dumped to 'BulkCopyReport.txt'.";
                }
                catch {
                    copyReport += "\n\nError: Couldn't dump detailed report to 'BulkCopyReport.txt'.";
                }
            }

            // Show the user a nice report.
            InfoMessage("Copy successful.\n\nResults:\n\n" + copyReport);
            if (wroteBulkCopyReport) {
                _ = new Process {
                    StartInfo = new ProcessStartInfo("BulkCopyReport.txt") {
                        UseShellExecute = true
                    }
                }.Start();
            }
        }

        private void tsmiTools_ApplyDFR_Click(object sender, EventArgs e) {
            if (SelectedFile != null)
                ApplyDFRDialog(SelectedFile);
        }

        private void tsmiTools_CreateDFR_Click(object sender, EventArgs e) {
            if (SelectedFile != null)
                CreateDFRDialog(SelectedFile);
        }

        private void tsmiTools_ImportTable_Click(object sender, EventArgs e) {
            if (SelectedFile != null)
                CopyTablesFrom(SelectedFile);
        }

        private void tsmiTools_ExportTable_Click(object sender, EventArgs e) {
            if (SelectedFile != null)
                CopyTablesTo(SelectedFile);
        }
    }
}
