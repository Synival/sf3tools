using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.ModelLoaders;
using SF3.Models.Files.MPD;
using SF3.Models.Files.X002;
using SF3.Models.Files.X005;
using SF3.Models.Files.X012;
using SF3.Models.Files.X013;
using SF3.Models.Files.X014;
using SF3.Models.Files.X019;
using SF3.Models.Files.X033_X031;
using SF3.Models.Files.X1;
using SF3.NamedValues;
using SF3.Types;
using SF3.Win;
using SF3.Win.Views;
using static CommonLib.Win.Utils.MessageUtils;

namespace SF3Editor {
    public partial class frmSF3Editor : Form {
        public static readonly string Version = "0.1";

        private readonly Dictionary<ScenarioType, INameGetterContext> c_nameGetterContexts = Enum.GetValues<ScenarioType>()
            .ToDictionary(x => x, x => (INameGetterContext) new NameGetterContext(x));

        private readonly string FileDialogFilter =
            "All Supported Files|X1*.BIN;X002.BIN;X005.BIN;X012.BIN;X013.BIN;X014.BIN;X019.BIN;X031.BIN;X033.BIN;*.MPD"
            + "|X1 Files (X1*.BIN)|X1*.BIN"
            + "|X1BTL99 File (X1BTL99.BIN)|X1BTL99.BIN"
            + "|X002 File (X002.BIN)|X002.BIN"
            + "|X005 File (X005.BIN)|X005.BIN"
            + "|X012 File (X013.BIN)|X012.BIN"
            + "|X013 File (X013.BIN)|X013.BIN"
            + "|X014 File (X014.BIN)|X014.BIN"
            + "|X019 File (X019.BIN)|X019.BIN"
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

            // Remember the last Scenario type to open.
            _openScenario = (_appState.OpenScenario < 0 || !Enum.IsDefined(typeof(ScenarioType), (ScenarioType) _appState.OpenScenario))
                ? null : (ScenarioType) _appState.OpenScenario;
            UpdateCheckedOpenScenario();

            ResumeLayout();
        }

        /// <summary>
        /// Creates an "Open" dialog and, if a file was chosen, opens it, processes its data, and loads it.
        /// </summary>
        public bool OpenFileDialog() {
            var openfile = new OpenFileDialog {
                Filter = FileDialogFilter
            };
            if (openfile.ShowDialog() != DialogResult.OK)
                return false;

            var filters = openfile.Filter
                .Split('|')
                .Select((x, i) => new { Index = i / 2, Value = x })
                .GroupBy(x => x.Index)
                .ToDictionary(x => x.Key, x => x.Select(y => y.Value).ToArray());

            return LoadFile(
                openfile.FileName,
                OpenScenario ?? DetermineScenario(openfile.FileName),
                DetermineFileType(openfile.FileName, filters[openfile.FilterIndex - 1][1])
            );
        }

        private static SF3FileType? DetermineFileType(string filename, string filter) {
            // If the filter is explicit, don't guess.
            switch (filter) {
                case "X1*.BIN": {
                    if (filename == "X1BTL99.BIN")
                        return SF3FileType.X1BTL99;
                    else
                        return SF3FileType.X1;
                }

                case "X1BTL99.BIN":
                    return SF3FileType.X1BTL99;
                case "X002.BIN":
                    return SF3FileType.X002;
                case "X005.BIN":
                    return SF3FileType.X005;
                case "X012.BIN":
                    return SF3FileType.X012;
                case "X013.BIN":
                    return SF3FileType.X013;
                case "X014.BIN":
                    return SF3FileType.X014;
                case "X019.BIN":
                    return SF3FileType.X019;
                case "X031.BIN":
                    return SF3FileType.X031;
                case "X033.BIN":
                    return SF3FileType.X033;
                case "*.MPD":
                    return SF3FileType.MPD;
            }

            // No explicit filter. Look at the filename and guess.
            var filenameUpper = filename.ToUpper();
            var preExtension = Path.GetFileNameWithoutExtension(filenameUpper);

            if (filenameUpper.Contains(".MPD"))
                return SF3FileType.MPD;
            else if (filenameUpper.Contains(".BIN")) {
                if (preExtension.Contains("X1BTL99"))
                    return SF3FileType.X1BTL99;
                else if (preExtension.Contains("X1"))
                    return SF3FileType.X1;
                else if (preExtension.Contains("X002"))
                    return SF3FileType.X002;
                else if (preExtension.Contains("X005"))
                    return SF3FileType.X005;
                else if (preExtension.Contains("X012"))
                    return SF3FileType.X012;
                else if (preExtension.Contains("X013"))
                    return SF3FileType.X013;
                else if (preExtension.Contains("X014"))
                    return SF3FileType.X014;
                else if (preExtension.Contains("X019"))
                    return SF3FileType.X019;
                else if (preExtension.Contains("X031"))
                    return SF3FileType.X031;
                else if (preExtension.Contains("X033"))
                    return SF3FileType.X033;
            }

            // Couldn't figure it out; it's unknown.
            return null;
        }

        private static ScenarioType? DetermineScenario(string filename) {
            // Determine the scenario based on the path.
            var directory = Path.GetDirectoryName(filename);
            if (directory.EndsWith(Path.DirectorySeparatorChar))
                directory = directory.Substring(0, directory.Length - 1);
            var paths = directory.Split(Path.DirectorySeparatorChar);

            if (paths.Length == 0)
                return null;

            switch (paths[paths.Length - 1]) {
                case "GS-9175":
                case "MK-81383":
                    return ScenarioType.Scenario1;
                case "GS-9188":
                    return ScenarioType.Scenario2;
                case "GS-9203":
                    return ScenarioType.Scenario3;
                case "6106979":
                    return ScenarioType.PremiumDisk;
            }

            // Check the label of the drive.
            if (paths[0].Length == 2 && paths[0][1] == ':') {
                var drivePath = paths[0][0] + ":\\";
                var drives = DriveInfo.GetDrives();
                var drive = drives.FirstOrDefault(x => x.Name == drivePath);
                if (drive != null) {
                    switch (drive.VolumeLabel) {
                        case "SHINING_FORCE_3_1":
                            return ScenarioType.Scenario1;
                        case "SHINING_FORCE_3_2":
                            return ScenarioType.Scenario2;
                        case "SHINING_FORCE_3_3":
                            return ScenarioType.Scenario3;
                        case "SHINING_FORCE_3_P":
                            return ScenarioType.PremiumDisk;
                    }
                }
            }

            return null;
        }

        private bool LoadFile(string filename, ScenarioType? scenario, SF3FileType? fileType) {
            // If we don't know the scenario, we can't load it.
            if (!scenario.HasValue) {
                ErrorMessage("Can't determine scenario for '" + filename + "'.");
                return false;
            }

            // If we don't know the file type, we can't load it.
            if (!fileType.HasValue) {
                ErrorMessage("Can't determine file type for '" + filename + "'.");
                return false;
            }

            // Attempt to the load the file.
            var fileLoader = new ModelFileLoader();
            bool success = fileLoader.LoadFile(filename, loader => {
                var sc = scenario.Value;
                switch (fileType) {
                    case SF3FileType.X1:
                        return X1_File.Create(loader.ByteData, c_nameGetterContexts[sc], sc, false);
                    case SF3FileType.X1BTL99:
                        return X1_File.Create(loader.ByteData, c_nameGetterContexts[sc], sc, true);
                    case SF3FileType.X002:
                        return X002_File.Create(loader.ByteData, c_nameGetterContexts[sc], sc);
                    case SF3FileType.X005:
                        return X005_File.Create(loader.ByteData, c_nameGetterContexts[sc], sc);
                    case SF3FileType.X012:
                        return X012_File.Create(loader.ByteData, c_nameGetterContexts[sc], sc);
                    case SF3FileType.X013:
                        return X013_File.Create(loader.ByteData, c_nameGetterContexts[sc], sc);
                    case SF3FileType.X014:
                        return X014_File.Create(loader.ByteData, c_nameGetterContexts[sc], sc);
                    case SF3FileType.X019:
                        return X019_File.Create(loader.ByteData, c_nameGetterContexts[sc], sc);
                    case SF3FileType.X031:
                    case SF3FileType.X033:
                        return X033_X031_File.Create(loader.ByteData, c_nameGetterContexts[sc], sc);
                    case SF3FileType.MPD:
                        return MPD_File.Create(loader.ByteData, c_nameGetterContexts);
                    default:
                        throw new InvalidOperationException($"Unhandled file type '{fileType}'");
                }
            });

            if (!success) {
                // Wrong file was selected.
                ErrorMessage("Data in '" + filename + "' appears corrupt or invalid.\n" +
                             "Is this the correct type of file?");
                fileLoader.Close();
                return false;
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
                fileLoader.Close();
                ErrorMessage("Failed to create view. Maybe the file isn't supported yet?");
                return false;
            }

            // Focus the tab itself.
            var tabPage = (TabPage) newControl.Parent!;
            _tabInfos[tabPage] = new TabInfo { FileLoader = fileLoader, View = view };

            if (_fileContainerView.TabControl.SelectedTab != tabPage)
                _fileContainerView.TabControl.SelectedTab = tabPage;
            else
                FocusFileTab(tabPage);

            // Wire the view up to the file loader so it reacts to closing, OnModified changes, etc.
            fileLoader.Closed += (s, e) => {
                _ = _fileContainerView.RemoveChild(view);
                fileLoader.Dispose();

                var ti = _tabInfos.Select(x => new { x.Key, x.Value }).FirstOrDefault(x => x.Value.FileLoader == fileLoader);
                if (ti != null)
                    _tabInfos.Remove(ti.Key);
            };

            fileLoader.TitleChanged += (s, e) => {
                tabPage.Text = fileLoader.Title;
                if (_selectedFileLoader == fileLoader)
                    Text = fileLoader.ModelTitle(_versionTitle);
            };

            return true;
        }

        /// <summary>
        /// Closes a file in a tab.
        /// If may prompt the user to save if the file has been modified.
        /// </summary>
        /// <param name="force">When true, a "Save Changes" dialog is never offered.</param>
        /// <returns>'true' if the file was closed. Returns 'false' if the user clicked 'cancel' when prompted to save changes.</returns>
        public bool CloseFile(ModelFileLoader loader, bool force = false) {
            if (loader == null || !loader.IsLoaded)
                return true;

            if (!force && loader.IsModified)
                ;
            // TODO: prompt for save!
/*
                if (PromptForSave() == DialogResult.Cancel)
                    return false;
*/

            bool wasFocused = ContainsFocus;
            _ = loader.Close();
            if (wasFocused && !ContainsFocus)
                _ = Focus();

            return true;
        }

        private void FocusFileTab(TabPage? tabPage) {
            var ti = (tabPage == null) ? null : _tabInfos.TryGetValue(tabPage, out var tiValue) ? (TabInfo?) tiValue : null;

            var hasFile = ti.HasValue;
            if (_selectedFileLoader == ti?.FileLoader)
                return;

            tsmiFile_Save.Enabled   = hasFile;
            tsmiFile_SaveAs.Enabled = hasFile;
            tsmiFile_Close.Enabled  = hasFile;
            _selectedFileLoader     = ti?.FileLoader;
            Text = _selectedFileLoader == null ? _versionTitle : _selectedFileLoader.ModelTitle(_versionTitle);
        }

        private void tsmiFile_Open_Click(object sender, EventArgs e) => OpenFileDialog();

        private void tsmiFile_Close_Click(object sender, EventArgs e) {
            if (_selectedFileLoader != null)
                _ = CloseFile(_selectedFileLoader);
        }

        private void tsmiFile_Exit_Click(object sender, EventArgs e) => Close();

        private void tsmiScenario_Detect_Click(object sender, EventArgs e) => OpenScenario = null;
        private void tsmiScenario_Scenario1_Click(object sender, EventArgs e) => OpenScenario = ScenarioType.Scenario1;

        private void tsmiScenario_Scenario2_Click(object sender, EventArgs e) => OpenScenario = ScenarioType.Scenario2;

        private void tsmiScenario_Scenario3_Click(object sender, EventArgs e) => OpenScenario = ScenarioType.Scenario3;

        private void tsmiScenario_PremiumDisk_Click(object sender, EventArgs e) => OpenScenario = ScenarioType.PremiumDisk;

        private struct TabInfo {
            public ModelFileLoader FileLoader;
            public FileView View;
        };

        private Dictionary<TabPage, TabInfo> _tabInfos = [];

        private readonly string _baseTitle;
        private readonly string _versionTitle;
        private readonly TabView _fileContainerView;
        private readonly AppState _appState;

        private ModelFileLoader? _selectedFileLoader = null;

        private ScenarioType? _openScenario = null;
        private ScenarioType? OpenScenario {
            get => _openScenario;
            set {
                if (_openScenario != value) {
                    _openScenario = value;
                    _appState.OpenScenario = (_openScenario == null) ? -1 : (int) _openScenario;
                    _appState.Serialize();
                    UpdateCheckedOpenScenario();
                }
            }
        }

        private void UpdateCheckedOpenScenario() {
            tsmiScenario_Detect.Checked      = _openScenario == null;
            tsmiScenario_Scenario1.Checked   = _openScenario == ScenarioType.Scenario1;
            tsmiScenario_Scenario2.Checked   = _openScenario == ScenarioType.Scenario2;
            tsmiScenario_Scenario3.Checked   = _openScenario == ScenarioType.Scenario3;
            tsmiScenario_PremiumDisk.Checked = _openScenario == ScenarioType.PremiumDisk;
        }
    }
}
