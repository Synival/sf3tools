using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using BrightIdeasSoftware;
using DFRLib;
using DFRToolGUI.Forms;
using SF3.Editor.Extensions;
using SF3.Extensions;
using SF3.FileEditors;
using SF3.Types;
using SF3.Utils;

namespace SF3.Editor.Forms {
    /// <summary>
    /// Base editor form from which all other editors are derived.
    /// </summary>
    public partial class EditorForm : Form {
        private const string SavePromptString = "You have unsaved changes. Would you like to save?";

        public EditorForm() {
            InitializeComponent();
        }

        public EditorForm(IContainer container) {
            container.Add(this);
            InitializeComponent();
        }

        /// <summary>
        /// Function to be called after derived class's InitializeComponent() is called.
        /// </summary>
        public void InitializeEditor(ToolStrip toolStrip) {
            if (toolStrip != null) {
                var menuItems = new Dictionary<string, ToolStripMenuItem>();
                foreach (var ti in this.menuStrip1.Items) {
                    if (ti is ToolStripMenuItem tsmi)
                        menuItems.Add(tsmi.Text, tsmi);
                }

                // It's dangerous to modify the list as we iterate over it, so store the actions for later.
                var actions = new List<Action>();

                foreach (var ti in toolStrip.Items) {
                    if (ti is ToolStripMenuItem tsmi) {
                        var tsmiCopy = tsmi;
                        if (menuItems.ContainsKey(tsmi.Text)) {
                            actions.Add(() => {
                                while (tsmiCopy.DropDownItems.Count > 0)
                                    menuItems[tsmiCopy.Text].DropDownItems.Add(tsmiCopy.DropDownItems[0]);
                                tsmiCopy.DropDownItems.Clear();
                            });
                        }
                        else
                            actions.Add(() => menuStrip1.Items.Insert(menuStrip1.Items.IndexOf(tsmiHelp), tsmi));
                    }
                    else {
                        var tsi = ti as ToolStripItem;
                        actions.Add(() => menuStrip1.Items.Add(tsi));
                    }
                }

                foreach (var action in actions)
                    action();

                toolStrip.Items.Clear();
                this.Controls.Remove(toolStrip);
            }

            _baseTitle = this.Text + " v" + Version;
            this.tsmiHelp_Version.Text = "Version " + Version;
            this.Scenario = ScenarioType.Scenario1;

            EventHandler onScenarioChanged = (obj, eargs) => {
                tsmiScenario_Scenario1.Checked = (Scenario == ScenarioType.Scenario1);
                tsmiScenario_Scenario2.Checked = (Scenario == ScenarioType.Scenario2);
                tsmiScenario_Scenario3.Checked = (Scenario == ScenarioType.Scenario3);
                tsmiScenario_PremiumDisk.Checked = (Scenario == ScenarioType.PremiumDisk);
            };

            ScenarioChanged += onScenarioChanged;
            onScenarioChanged(null, EventArgs.Empty);

            FileIsLoadedChanged += (obj, eargs) => {
                tsmiFile_SaveAs.Enabled          = IsLoaded;
                tsmiFile_ApplyDFRFile.Enabled    = IsLoaded;
                tsmiFile_GenerateDFRFile.Enabled = IsLoaded;
                tsmiFile_CopyTablesTo.Enabled    = IsLoaded;
                tsmiFile_CopyTablesFrom.Enabled  = IsLoaded;
                tsmiFile_Close.Enabled           = IsLoaded;
            };

            ObjectListViews = this.GetAllObjectsOfTypeInFields<ObjectListView>(false);
            UpdateTitle();
        }

        /// <summary>
        /// Creates an "Open" dialog and, if a file was chosen, opens it, processes its data, and loads it.
        /// </summary>
        public bool OpenFileDialog() {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = FileDialogFilter;
            if (openfile.ShowDialog() != DialogResult.OK)
                return false;

            return LoadFile(openfile.FileName);
        }

        private bool LoadFile(string filename) {
            FileStream stream = null;
            try {
                try {
                    stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                }
                catch {
                    MessageBox.Show("Error trying to load file. It is probably in use by another process.");
                    throw;
                }
                return LoadFile(filename, stream);
            }
            finally {
                if (stream != null)
                    stream.Close();
            }
        }

        private bool LoadFile(string filename, Stream stream) {
            // Close the file first, and don't proceed if the user aborted it.
            if (!CloseFile())
                return false;

            FileEditor = MakeFileEditor();
            AttachFileEditor(FileEditor);

            bool success = new Func<bool>(() => {
                try {
                    if (!FileEditor.LoadFile(filename, stream))
                        return false;
                    if (!OnLoad())
                        return false;
                    return true;
                }
                catch {
                    return false;
                }
            })();

            if (!success) {
                //wrong file was selected
                MessageBox.Show("Data in '" + filename + "' appears corrupt or invalid.\n" +
                                "Is this the correct type of file?");
                return false;
            }

            return true;
        }

        private void AttachFileEditor(IFileEditor fileEditor) {
            fileEditor.TitleChanged += (obj, args) => UpdateTitle();
            fileEditor.PreLoaded += (obj, eargs) => this.PreFileLoaded?.Invoke(this, eargs);
            fileEditor.Loaded += (obj, eargs) => this.FileLoaded?.Invoke(this, eargs);
            fileEditor.PreClosed += (obj, eargs) => this.PreFileClosed?.Invoke(this, eargs);
            fileEditor.Closed += (obj, eargs) => this.FileClosed?.Invoke(this, eargs);
            fileEditor.PreSaved += (obj, eargs) => this.PreFileSaved?.Invoke(this, eargs);
            fileEditor.Saved += (obj, eargs) => this.FileSaved?.Invoke(this, eargs);
            fileEditor.ModifiedChanged += (obj, eargs) => this.FileModifiedChanged?.Invoke(this, eargs);
            fileEditor.IsLoadedChanged += (obj, eargs) => this.FileIsLoadedChanged?.Invoke(this, eargs);
        }

        /// <summary>
        /// If a file is open, it does nothing. But, if a file IS open,
        /// it creates an "Save As" dialog and, if a file was chosen, saves open data.
        /// </summary>
        /// <returns>'true' if a file was saved successfully. Otherwise, 'false'.</returns>
        public bool SaveFileDialog() {
            if (FileEditor == null)
                return false;

            ObjectListViews.ForEach(x => x.FinishCellEdit());

            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Filter = FileDialogFilter;
            savefile.FileName = Path.GetFileName(FileEditor.Filename);
            if (savefile.ShowDialog() != DialogResult.OK)
                return false;

            return FileEditor.SaveFile(savefile.FileName);
        }

        /// <summary>
        /// Closes a file if open.
        /// If may prompt the user to save if the file has been modified.
        /// </summary>
        /// <param name="force">When true, a "Save Changes" dialog is never offered.</param>
        /// <returns>'true' if no file was open or the file was closed. Returns 'false' if the user clicked 'cancel' when
        /// prompted to save changes.</returns>
        public bool CloseFile(bool force = false) {
            if (FileEditor == null)
                return true;

            if (!force && FileEditor.IsModified) {
                var result = MessageBox.Show(SavePromptString, "Save Changes", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Cancel)
                    return false;
                else if (result == DialogResult.Yes) {
                    if (!SaveFileDialog())
                        return false;
                }
            }

            ObjectListViews.ForEach(x => x.ClearObjects());
            FileEditor.CloseFile();
            FileEditor = null;
            return true;
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

            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "DFR Files (*.DFR)|*.DFR|All Files (*.*)|*.*";
            if (openfile.ShowDialog() != DialogResult.OK)
                return false;

            FileStream file = null;
            try {
                file = File.Open(openfile.FileName, FileMode.Open, FileAccess.Read);
                var diff = new ByteDiff(file);
                var oldBytes = FileEditor.GetAllData();
                var newBytes = diff.ApplyTo(oldBytes);

                var filename = FileEditor.Filename;
                if (!CloseFile())
                    return false;
                if (!LoadFile(filename, new MemoryStream(newBytes))) {
                    if (FileEditor != null)
                        FileEditor.IsModified = true;
                    return false;
                }

                FileEditor.IsModified = true;
                MessageBox.Show("DFR file successfully applied.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception e) {
                MessageBox.Show("Error loading DFR file:\n\n" + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally {
                file.Close();
            }

            return true;
        }

        /// <summary>
        /// Opens the DFRToolGUI as a modal dialog with the current editor data as its "Altered File".
        /// </summary>
        public bool GenerateDFRFile() {
            var form = new frmDFRTool(FileEditor.GetAllData());
            form.ShowDialog();
            return true;
        }

        /// <summary>
        /// Opens a dialog to perform a bulk copy of tables to another .BIN file.
        /// </summary>
        public void CopyTablesTo() {
            if (FileEditor == null)
                return;

            ObjectListViews.ForEach(x => x.FinishCellEdit());

            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Copy Tables To";
            saveFileDialog.Filter = FileDialogFilter;
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;
            var copyToFilename = saveFileDialog.FileName;

            string copyReport = "";
            try {
                var copyFileEditor = MakeFileEditor();
                if (!copyFileEditor.LoadFile(copyToFilename)) {
                    MessageBox.Show("Error trying to load file. It is probably in use by another process.");
                    return;
                }

                var result = FileEditor.BulkCopyProperties(copyFileEditor);
                if (!copyFileEditor.SaveFile(copyToFilename)) {
                    MessageBox.Show("Error trying to update file.");
                    return;
                }

                copyReport += result.MakeSummaryReport();

                // Output summary files.
                var fullReport = result.MakeFullReport();
                if (fullReport != "") {
                    try {
                        File.WriteAllText("BulkCopyReport.txt", fullReport);
                        copyReport += "\n\nDetailed reports dumped to 'BulkCopyReport.txt'.";
                    }
                    catch {
                        copyReport += "\n\nError: Couldn't dump detailed report to 'BulkCopyReport.txt'.";
                    }
                }
            }
            catch (Exception e) {
                //wrong file was selected
                MessageBox.Show("Data in '" + copyToFilename + "' appears corrupt or invalid:\n\n" +
                                e.Message + "\n\n" +
                                "Is this the correct type of file?");
                return;
            }

            // Show the user a nice report.
            MessageBox.Show("Copy successful.\n\nResults:\n\n" + copyReport);
        }

        /// <summary>
        /// Opens a dialog to perform a bulk copy of tables from another .BIN file.
        /// </summary>
        public void CopyTablesFrom() {
            if (FileEditor == null)
                return;

            ObjectListViews.ForEach(x => x.FinishCellEdit());

            var openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Copy Tables From";
            openFileDialog.Filter = FileDialogFilter;
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;
            var copyFromFilename = openFileDialog.FileName;

            string copyReport = "";
            try {
                var copyFileEditor = MakeFileEditor();
                if (!copyFileEditor.LoadFile(copyFromFilename)) {
                    MessageBox.Show("Error trying to load file. It is probably in use by another process.");
                    return;
                }

                var result = copyFileEditor.BulkCopyProperties(FileEditor);
                ObjectListViews.ForEach(x => x.RefreshAllItems());

                copyReport += result.MakeSummaryReport();

                // Output summary files.
                var fullReport = result.MakeFullReport();
                if (fullReport != "") {
                    try {
                        File.WriteAllText("BulkCopyReport.txt", fullReport);
                        copyReport += "\n\nDetailed reports dumped to 'BulkCopyReport.txt'.";
                    }
                    catch {
                        copyReport += "\n\nError: Couldn't dump detailed report to 'BulkCopyReport.txt'.";
                    }
                }
            }
            catch (Exception e) {
                //wrong file was selected
                MessageBox.Show("Data in '" + copyFromFilename + "' appears corrupt or invalid:\n\n" +
                                e.Message + "\n\n" +
                                "Is this the correct type of file?");
                return;
            }

            // Show the user a nice report.
            MessageBox.Show("Copy successful.\n\nResults:\n\n" + copyReport);
        }

        /// <summary>
        /// Updates the title of the form.
        /// </summary>
        protected void UpdateTitle() {
            string newTitle = MakeTitle();
            if (this.Text != newTitle) {
                this.Text = newTitle;
                TitleChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Function to load data from a file opened with OpenFileDialog(). Must be overridden.
        /// </summary>
        protected virtual bool OnLoad() => true;

        private void EditorForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (FileEditor?.IsModified == true)
                if (!CloseFile())
                    e.Cancel = true;
        }

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
        public bool IsLoaded => FileEditor?.IsLoaded == true;

        /// <summary>
        /// FileEditor open for the current file.
        /// </summary>
        protected IFileEditor FileEditor { get; private set; }

        /// <summary>
        /// All ObjectListView's present in the form. Populated automatically.
        /// </summary>
        protected List<ObjectListView> ObjectListViews { get; private set; }

        /// <summary>
        /// The title to set when using UpdateTitle().
        /// </summary>
        /// <returns></returns>
        protected virtual string MakeTitle() => (FileEditor?.IsLoaded == true)
            ? FileEditor.EditorTitle(_baseTitle)
            : _baseTitle;

        /// <summary>
        /// File filter for OpenFileDialog() and SaveFileDialog(). Must be overridden.
        /// </summary>
        protected virtual string FileDialogFilter => "BIN Files (*.BIN)|*.BIN|All Files (*.*)|*.*";

        /// <summary>
        /// Factory method for creating an IFileEditor in OpenFileDialog(). Must be overridden.
        /// </summary>
        protected virtual IFileEditor MakeFileEditor() => throw new NotImplementedException();

        /// <summary>
        /// Triggered when Scenario has a new value.
        /// </summary>
        public event EventHandler ScenarioChanged;

        /// <summary>
        /// Triggered whenever UpdateTitle() makes a change.
        /// </summary>
        public event EventHandler TitleChanged;

        /// <summary>
        /// Triggered before FileEditor loads a file.
        /// </summary>
        public event EventHandler PreFileLoaded;

        /// <summary>
        /// Triggered after FileEditor loads a file.
        /// </summary>
        public event EventHandler FileLoaded;

        /// <summary>
        /// Triggered before FileEditor saves a file.
        /// </summary>
        public event EventHandler PreFileSaved;

        /// <summary>
        /// Triggered after FileEditor saves a file.
        /// </summary>
        public event EventHandler FileSaved;

        /// <summary>
        /// Triggered before FileEditor closes a file.
        /// </summary>
        public event EventHandler PreFileClosed;

        /// <summary>
        /// Triggered after FileEditor closes a file.
        /// </summary>
        public event EventHandler FileClosed;

        /// <summary>
        /// Triggered after FileEditor's modified state has been changed.
        /// </summary>
        public event EventHandler FileModifiedChanged;

        /// <summary>
        /// Triggered after FileEditor's loaded state has been changed.
        /// </summary>
        public event EventHandler FileIsLoadedChanged;

        protected virtual void tsmiFile_Open_Click(object sender, EventArgs e) => OpenFileDialog();
        protected virtual void tsmiFile_SaveAs_Click(object sender, EventArgs e) => SaveFileDialog();
        protected virtual void tsmiFile_Close_Click(object sender, EventArgs e) => CloseFile();
        protected virtual void tsmiFile_Exit_Click(object sender, EventArgs e) => Close();

        protected virtual void tsmiFile_applyDFRFile_Click(object sender, EventArgs e) => ApplyDFRDialog();
        protected virtual void tsmiFile_generateDFRFile_Click(object sender, EventArgs e) => GenerateDFRFile();

        protected virtual void tsmiScenario_Scenario1_Click(object sender, EventArgs e) => Scenario = ScenarioType.Scenario1;
        protected virtual void tsmiScenario_Scenario2_Click(object sender, EventArgs e) => Scenario = ScenarioType.Scenario2;
        protected virtual void tsmiScenario_Scenario3_Click(object sender, EventArgs e) => Scenario = ScenarioType.Scenario3;
        protected virtual void tsmiScenario_PremiumDisk_Click(object sender, EventArgs e) => Scenario = ScenarioType.PremiumDisk;

        protected virtual void tsmiFile_CopyTablesTo_Click(object sender, EventArgs e) => CopyTablesTo();
        protected virtual void tsmiFile_CopyTablesFrom_Click(object sender, EventArgs e) => CopyTablesFrom();
    }
}
