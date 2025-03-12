using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.ModelLoaders;
using SF3.Models.Files.X1;
using SF3.NamedValues;
using SF3.Types;
using SF3.Win.Views;
using static CommonLib.Win.Utils.MessageUtils;
using static SF3.Win.Extensions.ObjectListViewExtensions;

namespace SF3Editor {
    public partial class frmSF3Editor : Form {
        public static readonly string Version = "0.1";

        private readonly Dictionary<ScenarioType, INameGetterContext> c_nameGetterContexts = Enum.GetValues<ScenarioType>()
            .ToDictionary(x => x, x => (INameGetterContext) new NameGetterContext(x));

        private readonly string FileDialogFilter =
            "All Supported Files|X1*.BIN;X002.BIN;X005.BIN;X012.BIN;X013.BIN;X014.BIN;X019.BIN;X031.BIN;X033.BIN;*.MPD"
            + "|X1 Files (X1*.BIN)|X1*.BIN"
            + "|X002 File (X002.BIN)|X002.BIN"
            + "|X005 File (X005.BIN)|X005.BIN"
            + "|X013 File (X013.BIN)|X013.BIN"
            + "|X014 File (X014.BIN)|X014.BIN"
            + "|X019 File (X019.BIN)|X019.BIN"
            + "|X031 File (X031.BIN)|X031.BIN"
            + "|X033 File (X033.BIN)|X033.BIN"
            + "|MPD File (*.MPD)|*.MPD"
            + "|All Files (*.*)|*.*"
            ;

        public frmSF3Editor() {
            RegisterNamedValues();

            SuspendLayout();
            InitializeComponent();

            _fileContainerView = new TabView("File Container", lazyLoad: false);
            _ = _fileContainerView.Create();
            var fileContainerControl = _fileContainerView.TabControl;
            Controls.Add(fileContainerControl);
            fileContainerControl.Dock = DockStyle.Fill;
            fileContainerControl.BringToFront(); // If this isn't in the front, the menu is placed behind it (eep)

            _baseTitle = Text;
            _versionTitle = _baseTitle + " v" + Version;
            Text = _versionTitle;

            ResumeLayout();

            fileContainerControl.Selected += (s, e) => FocusFileTab(e.TabPage);
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

            return LoadFile(openfile.FileName);
        }

        private bool LoadFile(string filename) {
            try {
                using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
                    return LoadFile(filename, stream);
            }
            catch (Exception e) {
                ErrorMessage("Error trying to load file:\n\n" + e.Message);
                return false;
            }
        }

        private bool LoadFile(string filename, Stream stream) {
            var fileLoader = new ModelFileLoader();
            bool success = fileLoader.LoadFile(filename, loader => {
                // TODO: more than just X1 files.
                // TODO: get scenario.
                return X1_File.Create(loader.ByteData, c_nameGetterContexts[ScenarioType.Scenario1], ScenarioType.Scenario1, false);
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

            if (newControl == null)
                throw new InvalidOperationException(nameof(newControl) + " should never be null at this point!");

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

        private struct TabInfo {
            public ModelFileLoader FileLoader;
            public FileView View;
        };

        private Dictionary<TabPage, TabInfo> _tabInfos = [];

        private readonly string _baseTitle;
        private readonly string _versionTitle;
        private readonly TabView _fileContainerView;
        private ModelFileLoader? _selectedFileLoader = null;
    }
}
