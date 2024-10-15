﻿using System;
using System.Windows.Forms;
using BrightIdeasSoftware;
using SF3.Types;
using System.Collections.Generic;
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

        new public IX002_FileEditor FileEditor => base.FileEditor as IX002_FileEditor;

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
            if (FileEditor?.SpellList != null)
            {
                olvSpells.AddObjects(FileEditor.SpellList.Models);
            }

            olvStatBoost.ClearObjects();
            if (FileEditor?.StatList != null)
            {
                olvStatBoost.AddObjects(FileEditor.StatList.Models);
            }
        }

        protected override string FileDialogFilter => "SF3 scn3 data (X002.bin)|X002.bin|Binary File (*.bin)|*.bin|" + "All Files (*.*)|*.*";

        protected override IFileEditor MakeFileEditor() => new X002_FileEditor(Scenario);

        protected override bool LoadOpenedFile()
        {
            return tabMain.PopulateAndShowTabs(new List<PopulateAndShowTabConfig>()
            {
                new PopulateAndShowTabConfig(true, tabItems, olvItems, FileEditor.ItemList),
                new PopulateAndShowTabConfig(true, tabSpells, olvSpells, FileEditor.SpellList),
                new PopulateAndShowTabConfig(true, tabPreset, olvPreset, FileEditor.PresetList),
                new PopulateAndShowTabConfig(true, tabLoaded, olvLoaded, FileEditor.LoadList),
                new PopulateAndShowTabConfig(true, tabLoadedOverride, olvLoadedOverride, FileEditor.MusicOverrideList),
                new PopulateAndShowTabConfig(true, tabStatBoost, olvStatBoost, FileEditor.StatList),
                new PopulateAndShowTabConfig(true, tabWeaponRankAttack, olvWeaponRankAttack, FileEditor.WeaponRankList),
                new PopulateAndShowTabConfig(true, tabAttackResist, olvAttackResist, FileEditor.AttackResistList),
                new PopulateAndShowTabConfig(Scenario == ScenarioType.Scenario1, tabWarpTable, olvWarpTable, FileEditor.WarpList),
            });
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
