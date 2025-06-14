using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CommonLib.Discovery;
using CommonLib.Extensions;
using CommonLib.NamedValues;
using CommonLib.Types;
using DFRLib.Types;
using DFRLib.Win.Forms;
using SF3.ModelLoaders;
using SF3.Models.Files;
using static CommonLib.Win.Utils.MessageUtils;
using static SF3.Utils.FileUtils;

namespace SF3.Editor.Forms {
    public partial class SF3EditorForm {

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
                    if ((newFile = LoadFile(filename, scenario, fileType, newBytesStream, false)) == null)
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

        private void RunMovePostEOFPointersDialog(LoadedFile loadedFile, ScenarioTableFile file, DiscoveredData[] postEOFData) {
            var dialog = new MovePostEOFDataDialog(file, postEOFData);
            var result = dialog.ShowDialog();
            if (result != DialogResult.OK || dialog.MoveBy == 0)
                return;

            var count = MovePointersToAddressRange(file, file.RamAddress + file.Data.Length, file.RamAddressLimit, dialog.MoveBy);
            InfoMessage($"{count} pointer(s) updated.");

            ReloadFile(loadedFile, file.Data.GetDataCopy());
        }

        private void RunInsertDataDialog(LoadedFile loadedFile, ScenarioTableFile file) {
            var dialog = new InsertDataDialog(file);
            var result = dialog.ShowDialog();

            if (result != DialogResult.OK || dialog.InsertAddrFile == 0)
                return;

            var insertAddrFile = dialog.InsertAddrFile;
            var dataToInsert = dialog.DataToInsert;

            var count = MovePointersToAddressRange(file, file.RamAddress + dialog.InsertAddrFile, file.RamAddressLimit, dataToInsert.Length);
            InfoMessage($"{count} pointer(s) updated.");

            var newLen = file.Data.Length + dataToInsert.Length;
            var newData = new byte[newLen];
            var data = file.Data.GetDataCopy();

            // Rebuild our data, with the new data inserted at the desired location.
            // TODO: We don't need to copy this in C# like sniveling cowards, let's use memcpy() like champions
            int pos = 0;
            for (int i = 0; i < insertAddrFile; ++i)
                newData[pos++] = data[i];
            for (int i = 0; i < dataToInsert.Length; ++i)
                newData[pos++] = dataToInsert[i];
            for (int i = insertAddrFile; i < data.Length; ++i)
                newData[pos++] = data[i];

            ReloadFile(loadedFile, newData);
        }

        private int MovePointersToAddressRange(ScenarioTableFile file, int ptrValueFrom, int ptrValueTo, int moveBy) {
            var fileFrom = file.RamAddress;
            var fileTo   = fileFrom + file.Data.Length;

            var pointersToUpdate = file.Discoveries.GetAllOrdered()
                .Where(x => x.Type == DiscoveredDataType.Pointer && x.Address >= fileFrom && x.Address < fileTo)
                .Select(x => new { Pointer = x, Value = file.Data.GetDouble((int) (x.Address - fileFrom)) })
                .GroupBy(x => x.Pointer.Address)
                .Select(x => x.First())
                .Where(x => x.Value >= ptrValueFrom && x.Value < ptrValueTo)
                .ToArray();

            int count = 0;
            foreach (var pointerValue in pointersToUpdate) {
                var addr = (int) (pointerValue.Pointer.Address - fileFrom);
                file.Data.SetDouble(addr, pointerValue.Value + moveBy);
                count++;
            }

            return count;
        }

        private void ReloadFile(LoadedFile loadedFile, byte[] newBytes) {
            var scenario = loadedFile.Scenario;
            var filename = loadedFile.Loader.Filename;
            var fileType = loadedFile.FileType;

            try {
                if (!CloseFile(loadedFile, true)) {
                    ErrorMessage("Error: Couldn't close file");
                    return;
                }

                LoadedFile? newFile = null;
                using (var newBytesStream = new MemoryStream(newBytes)) {
                    if ((newFile = LoadFile(filename, scenario, fileType, newBytesStream, false)) == null) {
                        ErrorMessage("Error: Couldn't open updated file.");
                        return;
                    }
                }

                newFile.Loader.IsModified = true;
            }
            catch (Exception e) {
                ErrorMessage("Error: Exception thrown", e);
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

        private void tsmiTools_MovePostEOFData_Click(object sender, EventArgs e) {
            var file = SelectedFile?.Loader?.Model as ScenarioTableFile;
            if (SelectedFile == null || file == null)
                return;

            var discoveriesAfterEOF = file.Discoveries.GetAllOrdered()
                .Where(x => x.Address >= file.RamAddress + file.Data.Length && x.Address < file.RamAddressLimit)
                .ToArray();

            if (discoveriesAfterEOF.Length > 0)
                RunMovePostEOFPointersDialog(SelectedFile, file, discoveriesAfterEOF);
            else
                InfoMessage("No pointers to data after EOF; there is nothing to move.");
        }

        private void tsmiTools_InsertData_Click(object sender, EventArgs e) {
            var file = SelectedFile?.Loader?.Model as ScenarioTableFile;
            if (SelectedFile != null && file != null)
                RunInsertDataDialog(SelectedFile, file);
        }
    }
}
