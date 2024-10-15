using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using SF3.X002_Editor.Models.Spells;
using SF3.X002_Editor.Models.Presets;
using SF3.X002_Editor.Models.Items;
using SF3.X002_Editor.Models.Loading;
using SF3.X002_Editor.Models.StatBoost;
using SF3.X002_Editor.Models.WeaponRank;
using SF3.X002_Editor.Models.AttackResist;
using SF3.X002_Editor.Models.Warps;
using SF3.X002_Editor.Models.MusicOverride;
using BrightIdeasSoftware;
using SF3.Types;
using System.Collections.Generic;
using System.Linq;
using SF3.Editor.Forms;
using SF3.Models;
using SF3.Editor.Extensions;
using static SF3.Editor.Extensions.TabControlExtensions;

namespace SF3.X002_Editor.Forms
{
    public partial class frmX002_Editor : EditorForm
    {
        // Used to display version in the application
        private string Version = "0.20";

        public frmX002_Editor()
        {
            InitializeComponent();
            BaseTitle = this.Text;

            tsmiHelp_Version.Text = "Version " + Version;

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
                tsmiFile_Close.Enabled = IsLoaded == true;
            };

            FinalizeForm();
        }

        private void tabMain_Click(object sender, EventArgs e)
        {
            olvSpells.ClearObjects();
            var fileEditor = FileEditor as IX002_FileEditor;
            if (fileEditor?.SpellList != null)
            {
                olvSpells.AddObjects(fileEditor.SpellList.Models);
            }

            olvStatBoost.ClearObjects();
            if (fileEditor?.StatList != null)
            {
                olvStatBoost.AddObjects(fileEditor.StatList.Models);
            }
        }

        protected override string FileDialogFilter => "SF3 scn3 data (X002.bin)|X002.bin|Binary File (*.bin)|*.bin|" + "All Files (*.*)|*.*";

        protected override IFileEditor MakeFileEditor() => new X002_FileEditor(Scenario);

        protected override bool LoadOpenedFile()
        {
            var fileEditor = FileEditor as IX002_FileEditor;

            var loadLists = new List<IModelArray>()
            {
                fileEditor.ItemList,
                fileEditor.SpellList,
                fileEditor.PresetList,
                fileEditor.LoadList,
                fileEditor.StatList,
                fileEditor.WeaponRankList,
                fileEditor.AttackResistList,
                fileEditor.MusicOverrideList,
            };

            foreach (var list in loadLists)
            {
                if (!list.Load())
                {
                    MessageBox.Show("Could not load " + list.ResourceFile);
                    return false;
                }
            }

            ObjectListViews.ForEach(x => x.ClearObjects());

            olvItems.AddObjects(fileEditor.ItemList.Models);
            olvSpells.AddObjects(fileEditor.SpellList.Models);
            olvPreset.AddObjects(fileEditor.PresetList.Models);
            olvLoaded.AddObjects(fileEditor.LoadList.Models);
            olvStatBoost.AddObjects(fileEditor.StatList.Models);
            olvWeaponRankAttack.AddObjects(fileEditor.WeaponRankList.Models);
            olvAttackResist.AddObjects(fileEditor.AttackResistList.Models);
            olvLoadedOverride.AddObjects(fileEditor.MusicOverrideList.Models);

            if (!tabMain.PopulateAndShowTabs(new List<PopulateAndShowTabConfig>() {
                new PopulateAndShowTabConfig(Scenario == ScenarioType.Scenario1, tabWarpTable, olvWarpTable, fileEditor.WarpList)
            }))
            {
                return false;
            }

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
    }
}
