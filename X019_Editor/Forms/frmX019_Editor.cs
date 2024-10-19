using System;
using BrightIdeasSoftware;
using SF3.Types;
using System.Collections.Generic;
using SF3.Editor.Forms;
using SF3.Editor.Extensions;
using static SF3.Editor.Extensions.TabControlExtensions;
using SF3.FileEditors;

namespace SF3.X019_Editor.Forms
{
    public partial class frmX019_Editor : EditorForm
    {
        // Used to display version in the application
        private string Version = "0.13";

        private bool _isX044 = false;

        public bool IsX044
        {
            get => _isX044;
            set
            {
                if (_isX044 != value)
                {
                    _isX044 = value;
                    UpdateCheckboxes();
                }
            }
        }

        new public IX019_FileEditor FileEditor => base.FileEditor as IX019_FileEditor;

        public frmX019_Editor()
        {
            InitializeComponent();
            BaseTitle = this.Text + " " + Version;

            this.tsmiHelp_Version.Text = "Version " + Version;

            ScenarioChanged += (obj, eargs) => UpdateCheckboxes();
            UpdateCheckboxes();

            FileIsLoadedChanged += (obj, eargs) =>
            {
                tsmiFile_SaveAs.Enabled = IsLoaded == true;
                tsmiFile_Close.Enabled = IsLoaded == true;
            };

            FinalizeForm();
        }


        protected override string FileDialogFilter => IsX044
            ? "SF3 data (X044.bin)|X044.bin|Binary File (*.bin)|*.bin|" + "All Files (*.*)|*.*"
            : "SF3 data (X019.bin)|X019.bin|Binary File (*.bin)|*.bin|" + "All Files (*.*)|*.*";

        protected override IFileEditor MakeFileEditor() => new X019_FileEditor(Scenario, IsX044);

        protected override bool OnLoad()
        {
            if (!base.OnLoad())
            {
                return false;
            }

            return tabMain.PopulateAndToggleTabs(new List<PopulateTabConfig>()
            {
                new PopulateTabConfig(tabMonsterTab1, olvMonsterTab1, FileEditor.MonsterList),
                new PopulateTabConfig(tabMonsterTab2, olvMonsterTab2, FileEditor.MonsterList),
                new PopulateTabConfig(tabMonsterTab3, olvMonsterTab3, FileEditor.MonsterList),
                new PopulateTabConfig(tabMonsterTab4, olvMonsterTab4, FileEditor.MonsterList),
                new PopulateTabConfig(tabMonsterTab5, olvMonsterTab5, FileEditor.MonsterList),
            });
        }

        private void olvCellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e) => (sender as ObjectListView).EnhanceOlvCellEditControl(e);

        private void tsmiFile_Open_Click(object sender, EventArgs e) => OpenFileDialog();
        private void tsmiFile_SaveAs_Click(object sender, EventArgs e) => SaveFileDialog();
        private void tsmiFile_Close_Click(object sender, EventArgs e) => CloseFile();
        private void tsmiFile_Exit_Click(object sender, EventArgs e) => Close();

        private void tsmiScenario_Scenario1_Click(object sender, EventArgs e)
        {
            Scenario = ScenarioType.Scenario1;
            IsX044 = false;
        }

        private void tsmiScenario_Scenario2_Click(object sender, EventArgs e)
        {
            Scenario = ScenarioType.Scenario2;
            IsX044 = false;
        }

        private void tsmiScenario_Scenario3_Click(object sender, EventArgs e)
        {
            Scenario = ScenarioType.Scenario3;
            IsX044 = false;
        }

        private void tsmiScenario_PremiumDisk_Click(object sender, EventArgs e)
        {
            Scenario = ScenarioType.PremiumDisk;
            IsX044 = false;
        }

        private void tsmiScenario_PremiumDiskX044_Click(object sender, EventArgs e)
        {
            Scenario = ScenarioType.PremiumDisk;
            IsX044 = true;
        }

        private void UpdateCheckboxes()
        {
            tsmiScenario_Scenario1.Checked = (Scenario == ScenarioType.Scenario1);
            tsmiScenario_Scenario2.Checked = (Scenario == ScenarioType.Scenario2);
            tsmiScenario_Scenario3.Checked = (Scenario == ScenarioType.Scenario3);
            tsmiScenario_PremiumDisk.Checked = (Scenario == ScenarioType.PremiumDisk && !IsX044);
            tsmiScenario_PremiumDiskX044.Checked = (Scenario == ScenarioType.PremiumDisk && IsX044);
        }
    }
}
