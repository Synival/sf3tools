using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using SF3.X019_Editor.Models.Monsters;
using BrightIdeasSoftware;
using SF3.Types;
using System.Collections.Generic;
using System.Linq;
using SF3.Editor.Forms;
using SF3.Editor.Extensions;

namespace SF3.X019_Editor.Forms
{
    public partial class frmX019_Editor : EditorForm
    {
        // Used to display version in the application
        private string Version = "0.12";

        private MonsterList _monsterList;

        public frmX019_Editor()
        {
            InitializeComponent();
            BaseTitle = this.Text;

            this.tsmiHelp_Version.Text = "Version " + Version;

            EventHandler onScenarioChanged = (obj, eargs) =>
            {
                tsmiScenario_Scenario1.Checked = (Scenario == ScenarioType.Scenario1);
                tsmiScenario_Scenario2.Checked = (Scenario == ScenarioType.Scenario2);
                tsmiScenario_Scenario3.Checked = (Scenario == ScenarioType.Scenario3);
                tsmiScenario_PremiumDisk.Checked = (Scenario == ScenarioType.PremiumDisk);
                tsmiScenario_PremiumDiskX044.Checked = (Scenario == ScenarioType.Other);
            };

            ScenarioChanged += onScenarioChanged;
            onScenarioChanged(null, EventArgs.Empty);

            FileIsLoadedChanged += (obj, eargs) =>
            {
                tsmiFile_SaveAs.Enabled = IsLoaded == true;
                tsmiFile_Close.Enabled = IsLoaded == true;
            };

            FinalizeForm();
        }

        protected override string FileDialogFilter => "SF3 data (X019.bin)|X019.bin|Binary File (*.bin)|*.bin|" + "All Files (*.*)|*.*";

        protected override IFileEditor MakeFileEditor() => new X019_FileEditor(Scenario);

        protected override bool LoadOpenedFile()
        {
            var fileEditor = FileEditor as IX019_FileEditor;

            _monsterList = new MonsterList(fileEditor);
            if (!_monsterList.Load())
            {
                MessageBox.Show("Could not load " + _monsterList.ResourceFile);
                return false;
            }

            ObjectListViews.ForEach(x => x.ClearObjects());

            olvMonsterTab1.AddObjects(_monsterList.Models);
            olvMonsterTab2.AddObjects(_monsterList.Models);
            olvMonsterTab3.AddObjects(_monsterList.Models);
            olvMonsterTab4.AddObjects(_monsterList.Models);
            olvMonsterTab5.AddObjects(_monsterList.Models);

            return true;
        }

        private void olvCellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e) => (sender as ObjectListView).EnhanceOlvCellEditControl(e);

        private void tsmiFile_Open_Click(object sender, EventArgs e) => OpenFileDialog();
        private void tsmiFile_SaveAs_Click(object sender, EventArgs e) => SaveFileDialog();
        private void tsmiFile_Close_Click(object sender, EventArgs e) => CloseFile();
        private void tsmiFile_Exit_Click(object sender, EventArgs e) => Close();

        private void tsmiScenario_Scenario1_Click(object sender, EventArgs e) => Scenario = ScenarioType.Scenario1;
        private void tsmiScenario_Scenario2_Click(object sender, EventArgs e) => Scenario = ScenarioType.Scenario2;
        private void tsmiScenario_Scenario3_Click(object sender, EventArgs e) => Scenario = ScenarioType.Scenario3;
        private void tsmiScenario_PremiumDisk_Click(object sender, EventArgs e) => Scenario = ScenarioType.PremiumDisk;

        private void tsmiScenario_PremiumDiskX044_Click(object sender, EventArgs e) => Scenario = ScenarioType.Other;
    }
}
