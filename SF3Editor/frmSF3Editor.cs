using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CommonLib.NamedValues;
using DFRLib.Types;
using DFRLib.Win.Forms;
using SF3.ModelLoaders;
using SF3.NamedValues;
using SF3.Types;
using SF3.Win;
using SF3.Win.Views;
using static CommonLib.Win.Utils.MessageUtils;
using static SF3.Utils.FileUtils;

namespace SF3Editor {
    public partial class frmSF3Editor : Form {
        public static readonly string Version = "0.1 (2025-04-08 dev)";

        private readonly Dictionary<ScenarioType, INameGetterContext> c_nameGetterContexts = Enum.GetValues<ScenarioType>()
            .ToDictionary(x => x, x => (INameGetterContext) new NameGetterContext(x));

        private readonly string FileDialogFilter =
            "All Supported Files|X1*.BIN;X002.BIN;X005.BIN;X011.BIN;X012.BIN;X013.BIN;X014.BIN;X019.BIN;X021.BIN;X026.BIN;X031.BIN;X033.BIN;X044.BIN;*.MPD"
            + "|IconPointer Files (X011.BIN;X021.BIN;X026.BIN)|X011.BIN;X021.BIN;X026.BIN"
            + "|X1 Files (X1*.BIN)|X1*.BIN"
            + "|X1BTL99 File (X1BTL99.BIN)|X1BTL99.BIN"
            + "|X002 File (X002.BIN)|X002.BIN"
            + "|X005 File (X005.BIN)|X005.BIN"
            + "|X012 File (X013.BIN)|X012.BIN"
            + "|X013 File (X013.BIN)|X013.BIN"
            + "|X014 File (X014.BIN)|X014.BIN"
            + "|Monster Files (X019.BIN;X044.BIN)|X019.BIN;X044.BIN"
            + "|X031 File (X031.BIN)|X031.BIN"
            + "|X033 File (X033.BIN)|X033.BIN"
            + "|MPD Files (*.MPD)|*.MPD"
            + "|All Files (*.*)|*.*"
            ;

        public frmSF3Editor() {
            SuspendLayout();
            InitializeComponent();

            // The application state should never change for the app's lifetime.
            _appState = AppState.RetrieveAppState();

            // Create a container for all files.
            _fileContainerView = new TabView("File Container", lazyLoad: false);
            _ = _fileContainerView.Create();
            var fileContainerControl = _fileContainerView.TabControl;
            Controls.Add(fileContainerControl);
            fileContainerControl.Dock = DockStyle.Fill;
            fileContainerControl.BringToFront(); // If this isn't in the front, the menu is placed behind it (eep)
            fileContainerControl.Selected += (s, e) => FocusFileTab(e.TabPage);

            // Store the original title in a few different forms. It's going to be changing around.
            _baseTitle = Text;
            _versionTitle = _baseTitle + " v" + Version;
            Text = _versionTitle;

            ResumeLayout();

            // Remember the last Scenario type to open.
            UpdateLocalOpenScenario();
            _appState.OpenScenarioChanged += (s, e) => {
                UpdateLocalOpenScenario();
                _appState.Serialize();
            };

            // Link some dropdowns/values to the app state.
            tsmiEdit_UseDropdowns.Checked = _appState.UseDropdownsForNamedValues;
            _appState.UseDropdownsForNamedValuesChanged += (s, e) => {
                tsmiEdit_UseDropdowns.Checked = _appState.UseDropdownsForNamedValues;
                _appState.Serialize();
            };

            tsmiEdit_EnableBlankFieldV2Controls.Checked = _appState.EnableExperimentalBlankFieldV2Brushes;
            _appState.EnableExperimentalBlankFieldV2BrushesChanged += (s, e) => {
                tsmiEdit_EnableBlankFieldV2Controls.Checked = _appState.EnableExperimentalBlankFieldV2Brushes;
                _appState.Serialize();
            };

            tsmiEdit_EnableDebugSettings.Checked = _appState.EnableDebugSettings;
            _appState.EnableDebugSettingsChanged += (s, e) => {
                tsmiEdit_EnableDebugSettings.Checked = _appState.EnableDebugSettings;
                _appState.Serialize();
            };
        }

        protected override void OnFormClosing(FormClosingEventArgs e) {
            if (!CloseAllFiles())
                e.Cancel = true;
            base.OnFormClosing(e);
        }

        /// <summary>
        /// Creates an "Open" dialog and, if a file was chosen, opens it, processes its data, and loads it.
        /// </summary>
        /// <returns>A record for the file loaded, or 'null' on failure/cancel.</returns>
        public LoadedFile? OpenFileDialog() {
            var openfile = new OpenFileDialog {
                Filter = FileDialogFilter
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
            return LoadFile(openfile.FileName, scenario.Value, fileType.Value);
        }

        /// <summary>
        /// Opens any file, provided a correct scenario type and file type.
        /// </summary>
        /// <param name="filename">Path/filename of the file to open.</param>
        /// <param name="scenario">Scenario for the file to open.</param>
        /// <param name="fileType">Type of the file to open.</param>
        /// <returns>A record for the file loaded, or 'null' on failure/cancel.</returns>
        public LoadedFile? LoadFile(string filename, ScenarioType scenario, SF3FileType fileType) {
            try {
                using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
                    return LoadFile(filename, scenario, fileType, stream);
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
        public LoadedFile? LoadFile(string filename, ScenarioType scenario, SF3FileType fileType, Stream stream) {
            // Attempt to the load the file.
            var fileLoader = new ModelFileLoader();
            bool success = fileLoader.LoadFile(filename, stream,
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
                if (_selectedFile?.Loader == fileLoader)
                    Text = fileLoader.ModelTitle(_versionTitle);
            };

            return loadedFile;
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
                if (!SaveFile(file))
                    return DialogResult.Cancel;

            return result;
        }

        /// <summary>
        /// Raises a "Save As" dialog for saving a file with a specific path/filename.
        /// </summary>
        /// <param name="file">The loaded file to save.</param>
        /// <returns>'true' if a file was saved successfully (and not cancelled). Otherwise, 'false'.</returns>
        public bool SaveFileAsDialog(LoadedFile file) {
            var savefile = new SaveFileDialog {
                Filter = FileDialogFilter,
                FileName = Path.GetFileName(file.Loader.Filename)
            };
            if (savefile.ShowDialog() != DialogResult.OK)
                return false;

            return SaveFile(file, savefile.FileName);
        }

        /// <summary>
        /// Saves a file to the path/filename in which it was opened.
        /// </summary>
        /// <param name="file">The loaded file to save.</param>
        /// <returns>'true' if a file was saved successfully. Otherwise, 'false'.</returns>
        public bool SaveFile(LoadedFile file)
            => SaveFile(file, file.Loader.Filename);

        /// <summary>
        /// Saves a file to a given path/filename.
        /// </summary>
        /// <param name="file">The loaded file to save.</param>
        /// <param name="filename">The filename to save the file as.</param>
        /// <returns>'true' if a file was saved successfully. Otherwise, 'false'.</returns>
        private bool SaveFile(LoadedFile file, string filename) {
            string? error = null;
            try {
                if (!file.Loader.SaveFile(filename)) {
                    // TODO: Actually get an error from SaveFile()!
                    error = "Save failed";
                }
            }
            catch (Exception e) {
                error = $"{e.GetType().Name} exception with message:\r\n{e.Message}";
            }

            if (error != null)
                ErrorMessage(error);

            return error == null;
        }

        private void FocusFileTab(TabPage? tabPage) {
            var file = (tabPage == null) ? null : _loadedFiles.FirstOrDefault(x => x.TabPage == tabPage);
            if (_selectedFile == file)
                return;

            var hasFile = file != null;
            tsmiFile_Save.Enabled         = hasFile;
            tsmiFile_SaveAs.Enabled       = hasFile;
            tsmiFile_Close.Enabled        = hasFile;
            tsmiTools_ApplyDFR.Enabled    = hasFile;
            tsmiTools_CreateDFR.Enabled   = hasFile;
            tsmiTools_ImportTable.Enabled = hasFile;
            tsmiTools_ExportTable.Enabled = hasFile;

            _selectedFile = file;
            Text = file == null ? _versionTitle : file.Loader.ModelTitle(_versionTitle);
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

        private void UpdateLocalOpenScenario() {
            var os = _appState.OpenScenario;
            _openScenario = (os < 0 || !Enum.IsDefined(typeof(ScenarioType), (ScenarioType) os)) ? null : (ScenarioType) os;

            tsmiScenario_Detect.Checked      = _openScenario == null;
            tsmiScenario_Scenario1.Checked   = _openScenario == ScenarioType.Scenario1;
            tsmiScenario_Scenario2.Checked   = _openScenario == ScenarioType.Scenario2;
            tsmiScenario_Scenario3.Checked   = _openScenario == ScenarioType.Scenario3;
            tsmiScenario_PremiumDisk.Checked = _openScenario == ScenarioType.PremiumDisk;
        }

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

        private void tsmiFile_Open_Click(object sender, EventArgs e) => OpenFileDialog();

        private void tsmiFile_Save_Click(object sender, EventArgs e) {
            if (_selectedFile != null)
                _ = SaveFile(_selectedFile);
        }

        private void tsmiFile_SaveAs_Click(object sender, EventArgs e) {
            if (_selectedFile != null)
                _ = SaveFileAsDialog(_selectedFile);
        }

        private void tsmiFile_Close_Click(object sender, EventArgs e) {
            if (_selectedFile != null)
                _ = CloseFile(_selectedFile);
        }

        private void tsmiTools_ApplyDFR_Click(object sender, EventArgs e) {
            if (_selectedFile != null)
                ApplyDFRDialog(_selectedFile);
        }

        private void tsmiTools_CreateDFR_Click(object sender, EventArgs e) {
            if (_selectedFile != null)
                CreateDFRDialog(_selectedFile);
        }

        private void tsmiTools_ImportTable_Click(object sender, EventArgs e)
            => InfoMessage("Not yet implemented - sorry!");
        private void tsmiTools_ExportTable_Click(object sender, EventArgs e)
            => InfoMessage("Not yet implemented - sorry!");

        private void tsmiFile_Exit_Click(object sender, EventArgs e) => Close();

        private void tsmiScenario_Detect_Click(object sender, EventArgs e) => OpenScenario = null;
        private void tsmiScenario_Scenario1_Click(object sender, EventArgs e) => OpenScenario = ScenarioType.Scenario1;

        private void tsmiScenario_Scenario2_Click(object sender, EventArgs e) => OpenScenario = ScenarioType.Scenario2;

        private void tsmiScenario_Scenario3_Click(object sender, EventArgs e) => OpenScenario = ScenarioType.Scenario3;

        private void tsmiScenario_PremiumDisk_Click(object sender, EventArgs e) => OpenScenario = ScenarioType.PremiumDisk;

        private void tsmiEdit_UseDropdowns_Click(object sender, EventArgs e) => _appState.UseDropdownsForNamedValues = !_appState.UseDropdownsForNamedValues;

        private void tsmiHelp_About_Click(object sender, EventArgs e) {
            // Fetch copyright info from the assembly itself.
            string? legalCopyright = null;
            try {
                var versionInfo = FileVersionInfo.GetVersionInfo(Environment.ProcessPath ?? "");
                legalCopyright = versionInfo.LegalCopyright;
            }
            catch (Exception ex) {
                legalCopyright = $"(Couldn't fetch copyright: {ex.GetType().Name} exception thrown with message: {ex.Message}";
            }

            // Show info, credits, and special thanks.
            MessageBox.Show(
                _versionTitle + "\n\n" +
                legalCopyright + "\n\n" +
                "All credit to Agrathejagged for the MPD compression/decompression code: https://github.com/Agrathejagged",
                "About " + _baseTitle
            );
        }

        public class LoadedFile {
            public LoadedFile(ModelFileLoader loader, ScenarioType scenario, SF3FileType fileType, TabPage tabPage, FileView view) {
                Loader   = loader;
                Scenario = scenario;
                FileType = fileType;
                TabPage  = tabPage;
                View     = view;
            }

            public readonly ModelFileLoader Loader;
            public readonly ScenarioType Scenario;
            public readonly SF3FileType FileType;
            public readonly TabPage TabPage;
            public readonly FileView View;

        };

        private List<LoadedFile> _loadedFiles = [];
        private LoadedFile? _selectedFile = null;

        private readonly string _baseTitle;
        private readonly string _versionTitle;
        private readonly TabView _fileContainerView;
        private readonly AppState _appState;

        private void tsmiEdit_EnableBlankFieldV2Controls_Click(object sender, EventArgs e)
            => _appState.EnableExperimentalBlankFieldV2Brushes = !_appState.EnableExperimentalBlankFieldV2Brushes;

        private void tsmiEdit_EnableDebugSettings_Click(object sender, EventArgs e)
            => _appState.EnableDebugSettings = !_appState.EnableDebugSettings;
    }
}
