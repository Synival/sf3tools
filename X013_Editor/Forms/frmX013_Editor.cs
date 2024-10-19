using System;
using BrightIdeasSoftware;
using SF3.Types;
using System.Collections.Generic;
using SF3.Editor.Forms;
using SF3.Editor.Extensions;
using static SF3.Editor.Extensions.TabControlExtensions;
using SF3.FileEditors;

namespace SF3.X013_Editor.Forms
{
    public partial class frmX013_Editor : EditorForm
    {
        // Used to display version in the application
        private string Version = "0.18";

        new public IX013_FileEditor FileEditor => base.FileEditor as IX013_FileEditor;

        public frmX013_Editor()
        {
            InitializeComponent();
            BaseTitle = this.Text + " " + Version;

            this.tsmiHelp_Version.Text = "Version " + Version;

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

        protected override string FileDialogFilter => "SF3 data (X013.bin)|X013.bin|Binary File (*.bin)|*.bin|" + "All Files (*.*)|*.*";

        protected override IFileEditor MakeFileEditor() => new X013_FileEditor(Scenario);

        protected override bool OnLoad()
        {
            if (!base.OnLoad())
            {
                return false;
            }

            return tabMain.PopulateAndToggleTabs(new List<PopulateTabConfig>()
            {
                new PopulateTabConfig(tabSpecials, olvSpecials, FileEditor.SpecialsList),
                new PopulateTabConfig(tabFriendshipExp, olvFriendshipExp, FileEditor.FriendshipExpList),
                new PopulateTabConfig(tabSupportType, olvSupportType, FileEditor.SupportTypeList),
                new PopulateTabConfig(tabSupportStats, olvSupportStats, FileEditor.SupportStatsList),
                new PopulateTabConfig(tabSoulmate, olvSoulmate, FileEditor.SoulmateList),
                new PopulateTabConfig(tabSoulmateChanceFail, olvSoulmateChanceFail, FileEditor.SoulfailList),
                new PopulateTabConfig(tabMagicBonus, olvMagicBonus, FileEditor.MagicBonusList),
                new PopulateTabConfig(tabCritVantages, olvCritVantages, FileEditor.CritModList),
                new PopulateTabConfig(tabCritCounterRate, olvCritCounterRate, FileEditor.CritrateList),
                new PopulateTabConfig(tabSpecialChance, olvSpecialChance, FileEditor.SpecialChanceList),
                new PopulateTabConfig(tabExpLimit, olvExpLimit, FileEditor.ExpLimitList),
                new PopulateTabConfig(tabHealExp, olvHealExp, FileEditor.HealExpList),
                new PopulateTabConfig(tabWeaponSpellRank, olvWeaponSpellRank, FileEditor.WeaponSpellRankList),
                new PopulateTabConfig(tabStatusGroups, olvStatusGroups, FileEditor.StatusEffectList)
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
