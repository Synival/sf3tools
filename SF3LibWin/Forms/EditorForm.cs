using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.Extensions;
using CommonLib.NamedValues;
using DFRLib.Types;
using DFRLib.Win.Forms;
using SF3.ModelLoaders;
using SF3.Models.Files;
using SF3.Types;
using SF3.Win.Extensions;
using static CommonLib.Win.Utils.MessageUtils;
using static SF3.Win.Extensions.ObjectListViewExtensions;

namespace SF3.Win.Forms {
    /// <summary>
    /// Base editor form from which all other editors are derived.
    /// </summary>
    public partial class EditorForm : Form {
        private const string SavePromptString = "You have unsaved changes. Would you like to save?";

        public EditorForm() {
            InitializeComponent();
            RegisterNamedValues();

            tsmiEdit_UseDropdowns.Checked = Globals.UseDropdowns;
            Globals.UseDropdownsChanged += (s, e) => tsmiEdit_UseDropdowns.Checked = Globals.UseDropdowns;

            AttachFileLoader(FileLoader);
        }

        public EditorForm(IContainer container) {
            container.Add(this);
            InitializeComponent();
        }

        /// <summary>
        /// Function to be called after derived class's InitializeComponent() is called.
        /// </summary>
        public void InitializeEditor(ToolStrip toolStrip = null, IEnumerable<ObjectListView> extraOLVs = null) {
            // Merge menu bars!
            if (toolStrip != null) {
                // Gather EditorForm's menu items in the form of a dictionary.
                var menuItems = new Dictionary<string, ToolStripMenuItem>();
                foreach (var ti in menuStrip1.Items) {
                    if (ti is ToolStripMenuItem tsmi)
                        menuItems.Add(tsmi.Text, tsmi);
                }

                // It's dangerous to modify the list as we iterate over it, so store the actions for later.
                var actions = new List<Action>();

                // Add menus one by one.
                foreach (var ti in toolStrip.Items) {
                    // If this is a top-level dropdown menu, see if they can be merged.
                    if (ti is ToolStripMenuItem tsmi) {
                        // If the menu already exists, append each item.
                        var tsmiCopy = tsmi;
                        if (menuItems.ContainsKey(tsmi.Text)) {
                            actions.Add(() => {
                                while (tsmiCopy.DropDownItems.Count > 0)
                                    _=menuItems[tsmiCopy.Text].DropDownItems.Add(tsmiCopy.DropDownItems[0]);
                                tsmiCopy.DropDownItems.Clear();
                            });
                        }
                        // The enu doesn't exist - just add it.
                        else
                            actions.Add(() => menuStrip1.Items.Insert(menuStrip1.Items.IndexOf(tsmiHelp), tsmi));
                    }
                    // (Untested) This is just a single button. Just add it.
                    else {
                        var tsi = ti as ToolStripItem;
                        actions.Add(() => menuStrip1.Items.Add(tsi));
                    }
                }

                // Perform queued actions.
                foreach (var action in actions)
                    action();

                // Destroy everything left of the now-merged menu.
                toolStrip.Items.Clear();
                Controls.Remove(toolStrip);
            }

            _baseTitle = Text + " v" + Version;
            tsmiHelp_Version.Text = "Version " + Version;
            Scenario = ScenarioType.Scenario1;

            void onScenarioChanged(object obj, EventArgs eargs) {
                tsmiScenario_Scenario1.Checked = Scenario == ScenarioType.Scenario1;
                tsmiScenario_Scenario2.Checked = Scenario == ScenarioType.Scenario2;
                tsmiScenario_Scenario3.Checked = Scenario == ScenarioType.Scenario3;
                tsmiScenario_PremiumDisk.Checked = Scenario == ScenarioType.PremiumDisk;
            }

            ScenarioChanged += onScenarioChanged;
            onScenarioChanged(null, EventArgs.Empty);

            FileIsLoadedChanged += (obj, eargs) => {
                tsmiFile_OpenPrevious.Enabled    = IsLoaded;
                tsmiFile_OpenNext.Enabled        = IsLoaded;
                tsmiFile_Save.Enabled            = IsLoaded && FileLoader.IsModified;
                tsmiFile_SaveAs.Enabled          = IsLoaded;
                tsmiFile_ApplyDFRFile.Enabled    = IsLoaded;
                tsmiFile_GenerateDFRFile.Enabled = IsLoaded;
                tsmiFile_CopyTablesTo.Enabled    = IsLoaded;
                tsmiFile_CopyTablesFrom.Enabled  = IsLoaded;
                tsmiFile_Close.Enabled           = IsLoaded;
            };

            FileModifiedChanged += (obj, eargs) => tsmiFile_Save.Enabled = IsLoaded && FileLoader.IsModified;

            ObjectListViews = this.GetAllObjectsOfTypeInFields<ObjectListView>(false);
            if (extraOLVs != null)
                ObjectListViews.AddRange(extraOLVs);

            foreach (var olv in ObjectListViews)
                olv.Enhance(() => FileLoader.Model?.NameGetterContext);

            UpdateTitle();
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
                ErrorMessage("Error trying to load file:\n\n" + e.Message + "\n\nIt is probably in use by another process.");
                return false;
            }
        }

        private bool LoadFile(string filename, Stream stream) {
            // Close the file first, and don't proceed if the user aborted it.
            if (!CloseFile())
                return false;

            var success = new Func<bool>(() => {
                try {
                    return FileLoader.LoadFile(filename, stream, CreateModel) && OnLoad();
                }
                catch {
                    return false;
                }
            })();

            if (!success) {
                //wrong file was selected
                ErrorMessage("Data in '" + filename + "' appears corrupt or invalid.\n" +
                             "Is this the correct type of file?");
                CloseFile();
                return false;
            }

            return true;
        }

        private void AttachFileLoader(IModelFileLoader loader) {
            loader.TitleChanged      += (obj, args) => UpdateTitle();
            loader.PreLoaded         += (obj, args) => PreFileLoaded?.Invoke(this, args);
            loader.Loaded            += (obj, args) => FileLoaded?.Invoke(this, args);
            loader.PreClosed         += (obj, args) => PreFileClosed?.Invoke(this, args);
            loader.Closed            += (obj, args) => FileClosed?.Invoke(this, args);
            loader.PreSaved          += (obj, args) => PreFileSaved?.Invoke(this, args);
            loader.Saved             += (obj, args) => FileSaved?.Invoke(this, args);
            loader.IsModifiedChanged += (obj, args) => FileModifiedChanged?.Invoke(this, args);
            loader.IsLoadedChanged   += (obj, args) => FileIsLoadedChanged?.Invoke(this, args);
        }

        /// <summary>
        /// If a file isn't open, it does nothing and returns 'false'.
        /// But, if a file IS open, it shows a "Save As" dialog and, if a file was chosen, saves open data
        /// and reports an error if unsuccessful.
        /// </summary>
        /// <returns>'true' if a file was saved successfully. Otherwise, 'false'.</returns>
        public bool SaveFileDialog() {
            if (FileLoader == null)
                return false;

            ObjectListViews.ForEach(x => x.FinishCellEdit());

            var savefile = new SaveFileDialog {
                Filter = FileDialogFilter,
                FileName = Path.GetFileName(FileLoader.Filename)
            };
            if (savefile.ShowDialog() != DialogResult.OK)
                return false;

            return SaveFile(savefile.FileName);
        }

        /// <summary>
        /// If a file isn't open and modified, it does nothing and returns 'false'.
        /// But if a file IS open and modified, it saves the current file and reports an error if unsuccessful.
        /// </summary>
        /// <returns>'true' if a file was saved successfully. Otherwise, 'false'.</returns>
        public bool Save() {
            if (FileLoader == null || !FileLoader.IsModified)
                return false;
            return SaveFile(FileLoader.Filename);
        }

        private bool SaveFile(string filename) {
            var success = true;
            try {
                if (!FileLoader.SaveFile(filename))
                    success = false;
            }
            catch {
                success = false;
            }
            if (!success)
                ErrorMessage("Error trying to save file. It is probably in use by another process.");

            return success;
        }

        private DialogResult PromptForSave() {
            var result = MessageBox.Show(SavePromptString, "Save Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Cancel) {
                return result;
            }
            else if (result == DialogResult.Yes)
                if (!Save())
                    return DialogResult.Cancel;
            return result;
        }

        /// <summary>
        /// Function called after closing is performed.
        /// </summary>
        protected virtual bool OnClose() => true;

        /// <summary>
        /// Closes a file if open.
        /// If may prompt the user to save if the file has been modified.
        /// </summary>
        /// <param name="force">When true, a "Save Changes" dialog is never offered.</param>
        /// <returns>'true' if no file was open or the file was closed. Returns 'false' if the user clicked 'cancel' when
        /// prompted to save changes.</returns>
        public bool CloseFile(bool force = false) {
            if (FileLoader == null || !FileLoader.IsLoaded)
                return true;

            if (!force && FileLoader.IsModified)
                if (PromptForSave() == DialogResult.Cancel)
                    return false;

            ObjectListViews.ForEach(x => x.ClearObjects());
            _ = FileLoader.Close();
            FileLoader.Dispose();

            return OnClose();
        }

        /// <summary>
        /// Creates an "Open" dialog and, if a DFR file was chosen, does the following:
        /// 1. Create a ByteDiff() from the DFR file
        /// 2. Close the current document
        /// 3. Reload the current document, but with the new data instead of its own
        /// </summary>
        public bool ApplyDFRDialog() {
            if (!IsLoaded)
                return false;

            if (FileLoader.IsModified)
                if (PromptForSave() == DialogResult.Cancel)
                    return false;

            var form = new frmDFRTool(CommandType.Apply, dialogMode: true);
            form.ApplyDFRInputData = FileLoader.ByteData.GetDataCopy();
            form.ApplyDFRInMemory = true;
            var dialogResult = form.ShowDialog();
            if (dialogResult != DialogResult.OK)
                return false;

            try {
                var filename = FileLoader.Filename;
                var newBytes = form.ApplyDFRInMemoryOutput;
                if (newBytes == null)
                    throw new NullReferenceException("Internal error: No result from 'Apply DFR' command!");

                if (!CloseFile(true))
                    return false;
                using (var newBytesStream = new MemoryStream(newBytes)) {
                    if (!LoadFile(filename, newBytesStream)) {
                        if (FileLoader != null)
                            FileLoader.IsModified = true;
                        return false;
                    }
                }
                FileLoader.IsModified = true;
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
        public bool GenerateDFRFile() {
            if (!IsLoaded)
                return false;

            var form = new frmDFRTool(CommandType.Create, dialogMode: true);
            form.CreateDFRAlteredData = FileLoader.ByteData.GetDataCopy();
            var dialogResult = form.ShowDialog();
            return (dialogResult == DialogResult.OK);
        }

        /// <summary>
        /// Opens a dialog to perform a bulk copy of tables to another .BIN file.
        /// </summary>
        public void CopyTablesTo() {
            if (FileLoader == null)
                return;

            ObjectListViews.ForEach(x => x.FinishCellEdit());

            var saveFileDialog = new SaveFileDialog {
                Title = "Copy Tables To",
                Filter = FileDialogFilter
            };
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;
            var copyToFilename = saveFileDialog.FileName;

            ObjectExtensions.BulkCopyPropertiesResult result = null;
            try {
                var copyFileLoader = new ModelFileLoader();
                if (!copyFileLoader.LoadFile(copyToFilename, CreateModel)) {
                    ErrorMessage("Error trying to load file. It is probably in use by another process.");
                    return;
                }

                result = FileLoader.Model.BulkCopyProperties(copyFileLoader.Model);
                if (!copyFileLoader.SaveFile(copyToFilename)) {
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

            ProduceAndPresentBulkCopyReport(result, FileLoader.Model.NameGetterContext);
        }

        /// <summary>
        /// Opens a dialog to perform a bulk copy of tables from another .BIN file.
        /// </summary>
        public void CopyTablesFrom() {
            if (FileLoader == null)
                return;

            ObjectListViews.ForEach(x => x.FinishCellEdit());

            var openFileDialog = new OpenFileDialog {
                Title = "Copy Tables From",
                Filter = FileDialogFilter
            };
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;
            var copyFromFilename = openFileDialog.FileName;

            ObjectExtensions.BulkCopyPropertiesResult result = null;
            try {
                var copyFileLoader = new ModelFileLoader();
                if (!copyFileLoader.LoadFile(copyFromFilename, CreateModel)) {
                    ErrorMessage("Error trying to load file. It is probably in use by another process.");
                    return;
                }
                result = copyFileLoader.Model.BulkCopyProperties(FileLoader.Model);
            }
            catch (Exception e) {
                //wrong file was selected
                ErrorMessage("Data in '" + copyFromFilename + "' appears corrupt or invalid:\n\n" +
                             e.Message + "\n\n" +
                             "Is this the correct type of file?");
                return;
            }

            ObjectListViews.ForEach(x => x.RefreshAllItems());
            ProduceAndPresentBulkCopyReport(result, FileLoader.Model.NameGetterContext);
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
                _=new Process {
                    StartInfo = new ProcessStartInfo("BulkCopyReport.txt") {
                        UseShellExecute = true
                    }
                }.Start();
            }
        }

        /// <summary>
        /// Updates the title of the form.
        /// </summary>
        protected void UpdateTitle() {
            var newTitle = MakeTitle();
            if (Text != newTitle) {
                Text = newTitle;
                TitleChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Function to load data from a file opened with OpenFileDialog(). Must be overridden.
        /// </summary>
        protected virtual bool OnLoad() => true;

        private string _baseTitle;

        protected virtual string Version => "(unset)";

        private ScenarioType _scenario = (ScenarioType) (-1); // Uninitialized value

        /// <summary>
        /// The Scenario set for editing.
        /// </summary>
        public ScenarioType Scenario {
            get => _scenario;
            set {
                if (_scenario != value) {
                    _scenario = value;
                    ScenarioChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Is 'true' when a file has been loaded.
        /// </summary>
        public bool IsLoaded => FileLoader?.IsLoaded == true;

        /// <summary>
        /// FileLoader open for the current file.
        /// </summary>
        protected IModelFileLoader FileLoader { get; } = new ModelFileLoader();

        /// <summary>
        /// All ObjectListView's present in the form. Populated automatically.
        /// </summary>
        protected List<ObjectListView> ObjectListViews { get; private set; }

        /// <summary>
        /// The title to set when using UpdateTitle().
        /// </summary>
        /// <returns></returns>
        protected virtual string MakeTitle() => FileLoader?.ModelTitle(_baseTitle) ?? _baseTitle;

        /// <summary>
        /// File filter for OpenFileDialog() and SaveFileDialog(). Must be overridden.
        /// </summary>
        protected virtual string FileDialogFilter => "BIN Files (*.BIN)|*.BIN|All Files (*.*)|*.*";

        /// <summary>
        /// Factory method for creating an IBaseFile in OpenFileDialog(). Must be overridden.
        /// (Cannot be abstract because then the VS component editor wouldn't work)
        /// </summary>
        protected virtual IBaseFile CreateModel(IModelFileLoader loader) => throw new NotImplementedException();

        /// <summary>
        /// The main menu strip.
        /// </summary>
        protected MenuStrip MenuStrip => menuStrip1;

        /// <summary>
        /// Triggered when Scenario has a new value.
        /// </summary>
        public event EventHandler ScenarioChanged;

        /// <summary>
        /// Triggered whenever UpdateTitle() makes a change.
        /// </summary>
        public event EventHandler TitleChanged;

        /// <summary>
        /// Triggered before FileLoader loads a file.
        /// </summary>
        public event EventHandler PreFileLoaded;

        /// <summary>
        /// Triggered after FileLoader loads a file.
        /// </summary>
        public event EventHandler FileLoaded;

        /// <summary>
        /// Triggered before FileLoader saves a file.
        /// </summary>
        public event EventHandler PreFileSaved;

        /// <summary>
        /// Triggered after FileLoader saves a file.
        /// </summary>
        public event EventHandler FileSaved;

        /// <summary>
        /// Triggered before FileLoader closes a file.
        /// </summary>
        public event EventHandler PreFileClosed;

        /// <summary>
        /// Triggered after FileLoader closes a file.
        /// </summary>
        public event EventHandler FileClosed;

        /// <summary>
        /// Triggered after FileLoader's modified state has been changed.
        /// </summary>
        public event EventHandler FileModifiedChanged;

        /// <summary>
        /// Triggered after FileLoader's loaded state has been changed.
        /// </summary>
        public event EventHandler FileIsLoadedChanged;

        protected virtual void EditorForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (FileLoader?.IsModified == true) {
                if (!CloseFile())
                    e.Cancel = true;
            }
        }

        protected virtual void tsmiFile_Open_Click(object sender, EventArgs e) => OpenFileDialog();
        protected virtual void tsmiFile_Save_Click(object sender, EventArgs e) => Save();
        protected virtual void tsmiFile_SaveAs_Click(object sender, EventArgs e) => SaveFileDialog();
        protected virtual void tsmiFile_Close_Click(object sender, EventArgs e) => CloseFile();
        protected virtual void tsmiFile_Exit_Click(object sender, EventArgs e) => Close();

        protected virtual void tsmiFile_applyDFRFile_Click(object sender, EventArgs e) => ApplyDFRDialog();
        protected virtual void tsmiFile_generateDFRFile_Click(object sender, EventArgs e) => GenerateDFRFile();

        protected virtual void tsmiFile_CopyTablesTo_Click(object sender, EventArgs e) => CopyTablesTo();
        protected virtual void tsmiFile_CopyTablesFrom_Click(object sender, EventArgs e) => CopyTablesFrom();

        protected virtual void tsmiScenario_Scenario1_Click(object sender, EventArgs e) => Scenario = ScenarioType.Scenario1;
        protected virtual void tsmiScenario_Scenario2_Click(object sender, EventArgs e) => Scenario = ScenarioType.Scenario2;
        protected virtual void tsmiScenario_Scenario3_Click(object sender, EventArgs e) => Scenario = ScenarioType.Scenario3;
        protected virtual void tsmiScenario_PremiumDisk_Click(object sender, EventArgs e) => Scenario = ScenarioType.PremiumDisk;

        private void tsmiEdit_UseDropdowns_Click(object sender, EventArgs e) => Globals.UseDropdowns = !Globals.UseDropdowns;

        private string[] GetOtherFilesAtDirectoryForOpenFilter() {
            var path = Path.GetDirectoryName(FileLoader.Filename);
            var filters = FileDialogFilter.Split('|')[1].Split(';');
            return filters.SelectMany(x => Directory.GetFiles(path, x)).OrderBy(x => x).ToArray();
        }

        private void tsmiFile_OpenPrevious_Click(object sender, EventArgs e) {
            var filesInDir = GetOtherFilesAtDirectoryForOpenFilter();
            if (filesInDir.Length <= 1)
                return;

            var index = filesInDir.Select((x, i) => new {x, i}).FirstOrDefault(x => x.x == FileLoader.Filename)?.i;
            var indexToLoad = (index == null) ? 0 : (index == 0) ? (filesInDir.Length - 1) : ((int) index - 1);
            _ = LoadFile(filesInDir[indexToLoad]);
        }

        private void tsmiFile_OpenNext_Click(object sender, EventArgs e) {
            var filesInDir = GetOtherFilesAtDirectoryForOpenFilter();
            if (filesInDir.Length <= 1)
                return;

            var index = filesInDir.Select((x, i) => new {x, i}).FirstOrDefault(x => x.x == FileLoader.Filename)?.i;
            var indexToLoad = (index == null) ? 0 : (index == filesInDir.Length - 1) ? 0 : ((int) index + 1);
            _ = LoadFile(filesInDir[indexToLoad]);
        }
    }
}
