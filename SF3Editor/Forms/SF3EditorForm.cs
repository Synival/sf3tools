using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.ModelLoaders;
using SF3.NamedValues;
using SF3.Types;
using SF3.Win;
using SF3.Win.Forms;
using SF3.Win.Views;
using static SF3.Utils.FileUtils;

namespace SF3.Editor.Forms {
    public partial class SF3EditorForm : DarkModeForm {
        public static readonly string Version = "0.2.1" + " (dev build: " + DateTime.Today.ToString("yyyy/MM/dd") + ")";

        private readonly Dictionary<ScenarioType, INameGetterContext> c_nameGetterContexts = Enum.GetValues<ScenarioType>()
            .ToDictionary(x => x, x => (INameGetterContext) new NameGetterContext(x));

        private readonly string OpenDialogFilter =
            "All Supported Files|" + string.Join(';', Enum.GetValues<SF3FileType>().Select(x => GetFileFilterForFileType(x)).Distinct())
            + "|" + string.Join('|', Enum.GetValues<SF3FileType>().Select(x => GetFileDialogFilterForFileType(x)).Distinct())
            + "|All Files (*.*)|*.*"
            ;

        public SF3EditorForm() {
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

            // Initialize menu states and set up events.
            InitFileMenu();
            InitViewMenu();
            InitSettingsMenu();

            ResumeLayout();
        }

        protected override void OnFormClosing(FormClosingEventArgs e) {
            if (!CloseAllFiles())
                e.Cancel = true;
            base.OnFormClosing(e);
        }

        /// <summary>
        /// Information about a file loaded into the SF3Editor, including UI object references.
        /// </summary>
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

        /// <summary>
        /// The file/tab currently selected.
        /// </summary>
        public LoadedFile? SelectedFile {
            get => _selectedFile;
            private set {
                if (_selectedFile != value) {
                    _selectedFile = value;
                    if (_selectedFile != null)
                        _selectedFile.TabPage.Select();
                }
            }
        }
        private LoadedFile? _selectedFile = null;

        private List<LoadedFile> _loadedFiles = [];

        private readonly string _baseTitle;
        private readonly string _versionTitle;
        private readonly TabView _fileContainerView;
        private readonly AppState _appState;
    }
}
