using System;
using BrightIdeasSoftware;
using SF3.Types;
using System.Collections.Generic;
using SF3.Editor.Forms;
using SF3.Editor.Extensions;
using static SF3.Editor.Extensions.TabControlExtensions;
using SF3.FileEditors;

namespace SF3.IconPointerEditor.Forms
{
    public partial class frmIconPointerEditor : EditorForm
    {
        // Used to display version in the application
        private string Version = "0.10";

        new public IIconPointerFileEditor FileEditor => base.FileEditor as IIconPointerFileEditor;

        private bool _isX026 = false;

        public bool IsX026
        {
            get => _isX026;
            set
            {
                _isX026 = value;
                tsmiHelp_X026Toggle.Checked = _isX026;
                UpdateTitle();
            }
        }

        public frmIconPointerEditor()
        {
            InitializeComponent();
            BaseTitle = this.Text + " " + Version;

            tsmiHelp_Version.Text = "Version " + Version;
            Scenario = ScenarioType.Scenario1;
            IsX026 = false;

            EventHandler onScenarioChanged = (obj, eargs) =>
            {
                tsmiScenario_Scenario1.Checked = (Scenario == ScenarioType.Scenario1);
                tsmiScenario_Scenario2.Checked = (Scenario == ScenarioType.Scenario2);
                tsmiScenario_Scenario3.Checked = (Scenario == ScenarioType.Scenario3);
                tsmiScenario_PremiumDisk.Checked = (Scenario == ScenarioType.PremiumDisk);
            };

            ScenarioChanged += onScenarioChanged;
            onScenarioChanged(null, EventArgs.Empty);

            FileIsLoadedChanged += (obj, eargs) =>
            {
                tsmiFile_SaveAs.Enabled = IsLoaded == true;
                tsmiFile_CopyTablesFrom.Enabled = IsLoaded == true;
                tsmiFile_Close.Enabled = IsLoaded == true;
            };

            FinalizeForm();
        }

        protected override string FileDialogFilter => IsX026
            ? "SF3 data (X026.bin)|X026.bin|Binary File (*.bin)|*.bin|" + "All Files (*.*)|*.*"
            : "SF3 data (X011.bin;X021.bin)|X011.bin;X021.bin|Binary File (*.bin)|*.bin|" + "All Files (*.*)|*.*";

        protected override IFileEditor MakeFileEditor() => new IconPointerFileEditor(Scenario, IsX026);

        protected override bool OnLoad()
        {
            if (!base.OnLoad())
            {
                return false;
            }

            return tabMain.PopulateAndToggleTabs(new List<PopulateTabConfig>()
            {
                new PopulateTabConfig(tabSpellIcons, olvSpellIcons, FileEditor.SpellIconList),
                new PopulateTabConfig(tabItemIcons, olvItemIcons, FileEditor.ItemIconList)
            });
        }

        private void olvCellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e) => (sender as ObjectListView).EnhanceOlvCellEditControl(e);

        private void tsmiFile_Open_Click(object sender, EventArgs e) => OpenFileDialog();
        private void tsmiFile_SaveAs_Click(object sender, EventArgs e) => SaveFileDialog();
        private void tsmiFile_Close_Click(object sender, EventArgs e) => CloseFile();
        private void tsmiFile_CopyTablesFrom_Click(object sender, EventArgs e) => CopyTablesFrom();
        private void tsmiFile_Exit_Click(object sender, EventArgs e) => Close();

        private void tsmiHelp_X026Toggle_Click(object sender, EventArgs e) => IsX026 = !IsX026;

        private void tsmiScenario_Scenario1_Click(object sender, EventArgs e) => Scenario = ScenarioType.Scenario1;
        private void tsmiScenario_Scenario2_Click(object sender, EventArgs e) => Scenario = ScenarioType.Scenario2;
        private void tsmiScenario_Scenario3_Click(object sender, EventArgs e) => Scenario = ScenarioType.Scenario3;
        private void tsmiScenario_PremiumDisk_Click(object sender, EventArgs e) => Scenario = ScenarioType.PremiumDisk;
    }
}
