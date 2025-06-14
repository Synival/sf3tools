using System;
using System.Linq;
using System.Windows.Forms;
using SF3.Types;
using static SF3.Utils.FileUtils;
using static CommonLib.Win.Utils.MessageUtils;
using System.IO;
using SF3.ModelLoaders;
using SF3.Win.Views;
using SF3.Win;
using SF3.Models.Files.MPD;
using SF3.Models.Files;

namespace SF3.Editor.Forms {
    public partial class SF3EditorForm {
        private void InitFileMenu() {
            // Remember the last Scenario type to open.
            UpdateLocalOpenScenario();
            _appState.OpenScenarioChanged += (s, e) => {
                UpdateLocalOpenScenario();
                _appState.Serialize();
            };

            tsmiFile_SwapToPrev.ShortcutKeyDisplayString = "Ctrl+Alt+,";
            tsmiFile_SwapToPrev.ShowShortcutKeys = true;
            tsmiFile_SwapToNext.ShortcutKeyDisplayString = "Ctrl+Alt+.";
            tsmiFile_SwapToNext.ShowShortcutKeys = true;

            UpdateRecentFilesMenu();
            _appState.RecentFilesChanged += (s, e) => UpdateRecentFilesMenu();
        }

        /// <summary>
        /// Creates an "Open" dialog and, if a file was chosen, opens it, processes its data, and loads it.
        /// </summary>
        /// <returns>A record for the file loaded, or 'null' on failure/cancel.</returns>
        public LoadedFile? OpenFileDialog() {
            var openfile = new OpenFileDialog {
                Filter = OpenDialogFilter
            };
            if (openfile.ShowDialog() != DialogResult.OK)
                return null;

            // Split each filter into a an n-element array of 2 strings
            var filters = openfile.Filter
                .Split('|')
                .Select((x, i) => new { Index = i / 2, Value = x })
                .GroupBy(x => x.Index)
                .ToDictionary(x => x.Key, x => x.Select(y => y.Value).ToArray());

            // If we don't know the file type, we can't load it.
            var fileType = DetermineFileType(openfile.FileName, filters[openfile.FilterIndex - 1][1]);
            if (!fileType.HasValue) {
                ErrorMessage("Can't determine file type for '" + openfile.FileName + "'.");
                return null;
            }

            // If we don't know the scenario, we can't load it.
            var scenario = OpenScenario ?? DetermineScenario(openfile.FileName, fileType);
            if (!scenario.HasValue) {
                ErrorMessage("Can't determine scenario for '" + openfile.FileName + "'.");
                return null;
            }

            // Attempt to load the file. Use an explicitly specificed scenario and file type if provided.
            return LoadFile(openfile.FileName, scenario.Value, fileType.Value, true);
        }

        /// <summary>
        /// Opens any file, provided a correct scenario type and file type.
        /// </summary>
        /// <param name="filename">Path/filename of the file to open.</param>
        /// <param name="scenario">Scenario for the file to open.</param>
        /// <param name="fileType">Type of the file to open.</param>
        /// <returns>A record for the file loaded, or 'null' on failure/cancel.</returns>
        public LoadedFile? LoadFile(string filename, ScenarioType scenario, SF3FileType fileType, bool addToRecentFiles) {
            try {
                using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
                    return LoadFile(filename, scenario, fileType, stream, addToRecentFiles);
            }
            catch (Exception) {
                return null;
            }
        }

        /// <summary>
        /// Opens any file, provided a correct scenario type and file type. The 'filename' provided is just for
        /// reference; the actual data will come from 'stream'.
        /// </summary>
        /// <param name="filename">Path/filename of the file belonging to 'stream'.</param>
        /// <param name="scenario">Scenario for the file to open.</param>
        /// <param name="fileType">Type of the file to open.</param>
        /// <param name="stream">Stream from which the input data comes.</param>
        /// <returns>A record for the file loaded, or 'null' on failure/cancel.</returns>
        public LoadedFile? LoadFile(string filename, ScenarioType scenario, SF3FileType fileType, Stream stream, bool addToRecentFiles) {
            // Attempt to the load the file.
            var fileLoader = new ModelFileLoader();
            bool success = fileLoader.LoadFile(filename, GetFileDialogFilterForFileType(fileType), stream,
                loader => CreateFile(loader.ByteData, fileType, c_nameGetterContexts, scenario));

            if (!success) {
                // TODO: maybe an actual error???
                // Wrong file was selected.
                ErrorMessage($"Data in '{filename}' appears corrupt or invalid.\r\n\r\n" +
                             $"Attempted to open as type '{fileType}' for '{scenario}'.\r\n\r\n" +
                              "Is this the correct type of file and the correct scenario?");
                fileLoader.Close();
                return null;
            }

            // Create a view for the file.
            var view = new FileView(fileLoader.Title, fileLoader.Model);
            Control? newControl = null;
            _fileContainerView.CreateChild(view, control => {
                // Focus the first control. Drill downward through control containers
                // to find the bottom-most control.
                var focusView = (IView) view;
                var focusControl = control;
                while (focusControl != null && focusView is IContainerView cc) {
                    var firstChild = cc.ChildViews.FirstOrDefault();
                    var firstChildControl = firstChild?.Control;
                    if (firstChildControl == null)
                        break;

                    focusView = firstChild;
                    focusControl = firstChildControl;
                }
                if (focusControl == null)
                    focusControl = this;

                focusControl.Focus();

                // The control should be created immediately; grab it!
                newControl = control;
            });

            // If there's no control here, something went wrong. Likely an unsupported file.
            // TODO: what's the actual error?
            if (newControl == null) {
                _fileContainerView.RemoveChild(view);
                _ = fileLoader.Close();
                ErrorMessage("Failed to create view. Maybe the file isn't supported yet?");
                return null;
            }

            // Focus the tab itself.
            var tabPage = (TabPage) newControl.Parent!;
            var loadedFile = new LoadedFile(fileLoader, scenario, fileType, tabPage, view);
            _loadedFiles.Add(loadedFile);

            if (_fileContainerView.TabControl.SelectedTab != tabPage)
                _fileContainerView.TabControl.SelectedTab = tabPage;
            else
                FocusFileTab(tabPage);

            // Wire the view up to the file loader so it reacts to closing, OnModified changes, etc.
            fileLoader.Closed += (s, e) => {
                _ = _fileContainerView.RemoveChild(view);
                fileLoader.Dispose();

                var lf = _loadedFiles.FirstOrDefault(x => x.Loader == fileLoader);
                if (lf != null)
                    _loadedFiles.Remove(lf);
            };

            fileLoader.TitleChanged += (s, e) => {
                tabPage.Text = fileLoader.Title;
                if (SelectedFile?.Loader == fileLoader)
                    Text = fileLoader.ModelTitle(_versionTitle);
            };

            // Add this file to the 'Recent Files' menu.
            if (addToRecentFiles) {
                _appState.PushRecentFile(filename, scenario, fileType);
                _appState.Serialize();
            }

            return loadedFile;
        }

        /// <summary>
        /// Saves a file to the path/filename in which it was opened.
        /// </summary>
        /// <param name="file">The loaded file to save.</param>
        /// <returns>'true' if a file was saved successfully. Otherwise, 'false'.</returns>
        public bool SaveFile(LoadedFile file, bool addToRecentFiles)
            => SaveFile(file, file.Loader.Filename, addToRecentFiles);

        /// <summary>
        /// Saves a file to a given path/filename.
        /// </summary>
        /// <param name="file">The loaded file to save.</param>
        /// <param name="filename">The filename to save the file as.</param>
        /// <returns>'true' if a file was saved successfully. Otherwise, 'false'.</returns>
        public bool SaveFile(LoadedFile file, string filename, bool addToRecentFiles) {
            string? error = null;
            try {
                if (!file.Loader.SaveFile(filename)) {
                    // TODO: Actually get an error from SaveFile()!
                    error = $"Error while saving '{filename}'";
                }
            }
            catch (Exception e) {
                error = $"{e.GetType().Name} exception while saving '{filename}' with message:\r\n{e.Message}";
            }

            if (error != null) {
                ErrorMessage(error);
                return false;
            }

            if (addToRecentFiles) {
                _appState.PushRecentFile(filename, file.Scenario, file.FileType);
                _appState.Serialize();
            }

            return true;
        }

        /// <summary>
        /// Raises a "Save As" dialog for saving a file with a specific path/filename.
        /// </summary>
        /// <param name="file">The loaded file to save.</param>
        /// <returns>'true' if a file was saved successfully (and not cancelled). Otherwise, 'false'.</returns>
        public bool SaveFileAsDialog(LoadedFile file) {
            var savefile = new SaveFileDialog {
                Filter = file.Loader.FileDialogFilter + "|All Files (*.*)|*.*",
                FileName = Path.GetFileName(file.Loader.Filename)
            };
            if (savefile.ShowDialog() != DialogResult.OK)
                return false;

            return SaveFile(file, savefile.FileName, true);
        }

        /// <summary>
        /// Attempts to save all files, bringing up a 'Save As...' dialog when necessary.
        /// TODO: this is never necessary, so it never does it!
        /// Cancelling the dialog will not cancel saving, only saving for the particular cancelled document.
        /// Files not marked as modified are not saved.
        /// </summary>
        /// <returns>Returns 'true' if saving was performed, otherwise 'false'.</returns>
        public bool SaveAllFiles() {
            bool saveHappened = false;
            foreach (var loadedFile in _loadedFiles)
                if (loadedFile.Loader?.Model?.IsModified == true)
                    saveHappened |= SaveFile(loadedFile, true);
            return saveHappened;
        }

        /// <summary>
        /// Closes a file in a tab.
        /// It may prompt the user to save if the file has been modified.
        /// </summary>
        /// <param name="file">The loaded file to close.</param>
        /// <param name="force">When true, a "Save Changes" dialog is never offered.</param>
        /// <returns>'true' if the file was closed or never existed in the first place. Returns 'false' if the user clicked 'cancel' when prompted to save changes.</returns>
        public bool CloseFile(LoadedFile file, bool force = false) {
            if (file?.Loader == null || !file.Loader.IsLoaded)
                return true;

            if (!force && file.Loader.IsModified)
                if (PromptForSave(file) == DialogResult.Cancel)
                    return false;

            bool wasFocused = ContainsFocus;
            _ = file.Loader.Close();
            if (wasFocused && !ContainsFocus)
                _ = Focus();

            return true;
        }

        /// <summary>
        /// Closes all files in all open tabs, prompting for save when necessary.
        /// The process is aborted on failure to close or file *or* by the user clicking 'Cancel'.
        /// </summary>
        /// <returns>'true' if all tabs were closed, otherwise 'false'.</returns>
        public bool CloseAllFiles(bool force = false) {
            // Close all tags, prompting the user to save for each tab if necessary.
            while (_loadedFiles.Count > 0) {
                var loadedFile = _loadedFiles[0];
                var closed = CloseFile(loadedFile, force);

                // Abort on the first 'Cancel' result or failed closure.
                if (!closed)
                    break;
            }

            // Only return 'true' if no tabs remain.
            return (_fileContainerView.TabControl.TabPages.Count == 0);
        }

        /// <summary>
        /// Swaps out the file loaded specified for the previous file in the same folder with the same type.
        /// </summary>
        /// <param name="file">The file to swap out.</param>
        /// <returns>The new file loaded, if any.</returns>
        public LoadedFile? SwapToPrevOfSameTypeInFolder(LoadedFile file) {
            var filesInDir = GetOtherFilesAtDirectoryForOpenFilter(file);
            if (filesInDir.Length <= 1)
                return null;

            var index = filesInDir.Select((x, i) => new {Entry = x, Index = i}).FirstOrDefault(x => x.Entry.Filename == file.Loader.Filename)?.Index;
            var indexToLoad = (index == null) ? 0 : (index == 0) ? (filesInDir.Length - 1) : ((int) index - 1);
            var fileToLoad = filesInDir[indexToLoad];

            return SwapToFile(file, fileToLoad.Filename, fileToLoad.Scenario, fileToLoad.FileType);
        }

        /// <summary>
        /// Swaps out the file loaded specified for the next file in the same folder with the same type.
        /// </summary>
        /// <param name="file">The file to swap out.</param>
        /// <returns>The new file loaded, if any.</returns>
        public LoadedFile? SwapToNextOfSameTypeInFolder(LoadedFile file) {
            var filesInDir = GetOtherFilesAtDirectoryForOpenFilter(file);
            if (filesInDir.Length <= 1)
                return null;

            var index = filesInDir.Select((x, i) => new {Entry = x, Index = i}).FirstOrDefault(x => x.Entry.Filename == file.Loader.Filename)?.Index;
            var indexToLoad = (index == null) ? 0 : (index == filesInDir.Length - 1) ? 0 : ((int) index + 1);
            var fileToLoad = filesInDir[indexToLoad];

            return SwapToFile(file, fileToLoad.Filename, fileToLoad.Scenario, fileToLoad.FileType);
        }

        /// <summary>
        /// Opens a recent file in the saved list of recent files, with '0' being the most recent.
        /// </summary>
        /// <param name="index">The index of the recent file to open, with '0' being the most recent.</param>
        /// <returns>The new LoadedFile or 'null' if an error occured.</returns>
        public LoadedFile? OpenRecentFile(int index) {
            var recentItems = _appState.RecentFiles ?? [];
            if (index >= recentItems.Length)
                return null;

            var recentItem = recentItems[index];
            return LoadFile(recentItem.Filename, recentItem.Scenario, recentItem.FileType, true);
        }

        /// <summary>
        /// The ScenarioType to use when opening a file. Set to 'null' to auto-detect.
        /// </summary>
        public ScenarioType? OpenScenario {
            get => _openScenario;
            private set {
                _appState.OpenScenario = ((int?) value) ?? -1;
            }
        }
        private ScenarioType? _openScenario = null;

        private void FocusFileTab(TabPage? tabPage) {
            var file = (tabPage == null) ? null : _loadedFiles.FirstOrDefault(x => x.TabPage == tabPage);
            if (SelectedFile == file)
                return;

            var hasFile = file != null;
            var scenarioTableFile = file?.Loader?.Model as ScenarioTableFile;

            tsmiFile_Save.Enabled         = hasFile;
            tsmiFile_SaveAs.Enabled       = hasFile;
            tsmiFile_SaveAll.Enabled      = hasFile;
            tsmiFile_Close.Enabled        = hasFile;
            tsmiFile_CloseAll.Enabled     = hasFile;
            tsmiFile_SwapToPrev.Enabled   = hasFile;
            tsmiFile_SwapToNext.Enabled   = hasFile;

            tsmiTools_ApplyDFR.Enabled    = hasFile;
            tsmiTools_CreateDFR.Enabled   = hasFile;
            tsmiTools_ImportTable.Enabled = hasFile;
            tsmiTools_ExportTable.Enabled = hasFile;
            tsmiTools_MovePostEOFData.Enabled = hasFile && scenarioTableFile?.Discoveries != null;

            SelectedFile = file;
            Text = file == null ? _versionTitle : file.Loader.ModelTitle(_versionTitle);

            var fileType = file?.FileType;
            tsmiX1.Visible   = tsmiX1.Enabled   = hasFile && (fileType == SF3FileType.X1);
            tsmiX019.Visible = tsmiX019.Enabled = hasFile && (fileType == SF3FileType.X019 || fileType == SF3FileType.X044);
            tsmiMPD.Visible  = tsmiMPD.Enabled  = hasFile && (fileType == SF3FileType.MPD);

            UpdateModelSwitchGroupsMenu((fileType == SF3FileType.MPD && file?.Loader?.Model != null) ? (IMPD_File) file.Loader.Model : null);
        }

        private void UpdateLocalOpenScenario() {
            var os = _appState.OpenScenario;
            _openScenario = (os < 0 || !Enum.IsDefined(typeof(ScenarioType), (ScenarioType) os)) ? null : (ScenarioType) os;

            tsmiFile_OpenScenario_Detect.Checked      = _openScenario == null;
            tsmiFile_OpenScenario_Scenario1.Checked   = _openScenario == ScenarioType.Scenario1;
            tsmiFile_OpenScenario_Scenario2.Checked   = _openScenario == ScenarioType.Scenario2;
            tsmiFile_OpenScenario_Scenario3.Checked   = _openScenario == ScenarioType.Scenario3;
            tsmiFile_OpenScenario_PremiumDisk.Checked = _openScenario == ScenarioType.PremiumDisk;
        }

        private DialogResult PromptForSave(LoadedFile file) {
            var result = MessageBox.Show(
                $"{file.Loader.Filename} has unsaved changes.\r\n\r\n" +
                "Would you like to save?", "Save Changes",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question
            );

            if (result == DialogResult.Cancel) {
                return result;
            }
            else if (result == DialogResult.Yes)
                if (!SaveFile(file, true))
                    return DialogResult.Cancel;

            return result;
        }

        private LoadedFile? SwapToFile(LoadedFile file, string filename, ScenarioType scenario, SF3FileType fileType) {
            // TODO: The tab should be at the same index.

            var newLoadedFile = LoadFile(filename, scenario, fileType, true);
            if (newLoadedFile == null) {
                ErrorMessage($"Couldn't open {fileType} file '{filename}'");
                return null;
            }

            if (!CloseFile(file)) {
                newLoadedFile.Loader.Close();
                return null;
            }

            return newLoadedFile;
        }

        private struct FileInDirectory {
            public string Filename;
            public SF3FileType FileType;
            public ScenarioType Scenario;
        };

        private FileInDirectory[] GetOtherFilesAtDirectoryForOpenFilter(LoadedFile file) {
            if (file?.Loader?.IsLoaded != true)
                return [];

            var path = Path.GetDirectoryName(file.Loader.Filename);
            if (path == null)
                return [];

            var filterSplit = file.Loader.FileDialogFilter?.Split('|');
            if (filterSplit?.Length != 2)
                return [];

            var filter = filterSplit[1];

            // If we don't know the scenario, we can't load it.
            return Directory.GetFiles(path, filter)
                .OrderBy(x => x)
                .Select(x => {
                    var fileType = DetermineFileType(x, null);
                    return new {
                        Filename = x,
                        FileType = fileType,
                        Scenario = DetermineScenario(x, fileType)
                    };
                })
                .Where(x => x.FileType.HasValue && x.Scenario.HasValue && x.FileType == file.FileType)
                .Select(x => new FileInDirectory { Filename = x.Filename, FileType = x.FileType!.Value, Scenario = x.Scenario!.Value })
                .ToArray();
        }

        private ToolStripMenuItem[] _recentFileMenuItems = null;

        private void UpdateRecentFilesMenu() {
            if (_recentFileMenuItems == null) {
                _recentFileMenuItems = [
                    tsmiFile_RecentFiles_1,
                    tsmiFile_RecentFiles_2,
                    tsmiFile_RecentFiles_3,
                    tsmiFile_RecentFiles_4,
                    tsmiFile_RecentFiles_5,
                    tsmiFile_RecentFiles_6,
                    tsmiFile_RecentFiles_7,
                    tsmiFile_RecentFiles_8,
                    tsmiFile_RecentFiles_9,
                    tsmiFile_RecentFiles_10
                ];
            }

            var recentFiles = _appState.RecentFiles ?? [];
            for (int i = 0; i < _recentFileMenuItems.Length; i++) {
                var menuItem = _recentFileMenuItems[i];
                var recentFile = (i < recentFiles.Length) ? recentFiles[i] : (AppState.RecentFile?) null;

                menuItem.Enabled = (recentFile != null);
                menuItem.Text = ((i == 9) ? "1&0" : $"&{i + 1}") + $" - " + (recentFile.HasValue ? recentFile.Value.Filename : "");
            }
        }

        private void tsmiFile_Open_Click(object sender, EventArgs e)
            => OpenFileDialog();

        private void tsmiFile_OpenScenario_Detect_Click(object sender, EventArgs e)
            => OpenScenario = null;

        private void tsmiFile_OpenScenario_Scenario1_Click(object sender, EventArgs e)
            => OpenScenario = ScenarioType.Scenario1;

        private void tsmiFile_OpenScenario_Scenario2_Click(object sender, EventArgs e)
            => OpenScenario = ScenarioType.Scenario2;

        private void tsmiFile_OpenScenario_Scenario3_Click(object sender, EventArgs e)
            => OpenScenario = ScenarioType.Scenario3;

        private void tsmiFile_OpenScenario_PremiumDisk_Click(object sender, EventArgs e)
            => OpenScenario = ScenarioType.PremiumDisk;

        private void tsmiFile_Save_Click(object sender, EventArgs e) {
            if (SelectedFile != null)
                _ = SaveFile(SelectedFile, true);
        }

        private void tsmiFile_SaveAs_Click(object sender, EventArgs e) {
            if (SelectedFile != null)
                _ = SaveFileAsDialog(SelectedFile);
        }

        private void tsmiFile_SaveAll_Click(object sender, EventArgs e)
            => SaveAllFiles();

        private void tsmiFile_Close_Click(object sender, EventArgs e) {
            if (SelectedFile != null)
                _ = CloseFile(SelectedFile);
        }

        private void tsmiFile_CloseAll_Click(object sender, EventArgs e)
            => CloseAllFiles();

        private void tsmiFile_SwapToPrev_Click(object sender, EventArgs e) {
            if (SelectedFile != null)
                _ = SwapToPrevOfSameTypeInFolder(SelectedFile);
        }

        private void tsmiFile_SwapToNext_Click(object sender, EventArgs e) {
            if (SelectedFile != null)
                _ = SwapToNextOfSameTypeInFolder(SelectedFile);
        }

        private void tsmiFile_RecentFiles_1_Click(object sender, EventArgs e)
            => OpenRecentFile(0);

        private void tsmiFile_RecentFiles_2_Click(object sender, EventArgs e)
            => OpenRecentFile(1);

        private void tsmiFile_RecentFiles_3_Click(object sender, EventArgs e)
            => OpenRecentFile(2);

        private void tsmiFile_RecentFiles_4_Click(object sender, EventArgs e)
            => OpenRecentFile(3);

        private void tsmiFile_RecentFiles_5_Click(object sender, EventArgs e)
            => OpenRecentFile(4);

        private void tsmiFile_RecentFiles_6_Click(object sender, EventArgs e)
            => OpenRecentFile(5);

        private void tsmiFile_RecentFiles_7_Click(object sender, EventArgs e)
            => OpenRecentFile(6);

        private void tsmiFile_RecentFiles_8_Click(object sender, EventArgs e)
            => OpenRecentFile(7);

        private void tsmiFile_RecentFiles_9_Click(object sender, EventArgs e)
            => OpenRecentFile(8);

        private void tsmiFile_RecentFiles_10_Click(object sender, EventArgs e)
            => OpenRecentFile(9);

        private void tsmiFile_Exit_Click(object sender, EventArgs e) => Close();
    }
}
