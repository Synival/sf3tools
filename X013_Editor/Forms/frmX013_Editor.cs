using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using SF3.X013_Editor.Models.SupportTypes;
using SF3.X013_Editor.Models.Presets;
using SF3.X013_Editor.Models.Specials;
using SF3.X013_Editor.Models.SupportStats;
using SF3.X013_Editor.Models.Soulmate;
using SF3.X013_Editor.Models.Soulfail;
using SF3.X013_Editor.Models.MagicBonus;
using SF3.X013_Editor.Models.CritMod;
using SF3.X013_Editor.Models.Critrate;
using SF3.X013_Editor.Models.SpecialChance;
using SF3.X013_Editor.Models.ExpLimit;
using SF3.X013_Editor.Models.HealExp;
using SF3.X013_Editor.Models.WeaponSpellRank;
using SF3.X013_Editor.Models.StatusEffects;
using BrightIdeasSoftware;
using SF3.Types;
using System.Collections.Generic;
using System.Linq;
using SF3.Editor.Forms;
using SF3.Models;
using SF3.Editor.Extensions;
using static SF3.Editor.Extensions.TabControlExtensions;

namespace SF3.X013_Editor.Forms
{
    public partial class frmX013_Editor : EditorForm
    {
        // Used to display version in the application
        private string Version = "0.17";

        private SpecialList _specialsList;
        private SupportTypeList _supportTypeList;
        private FriendshipExpList _friendshipExpList;
        private SupportStatsList _supportStatsList;
        private SoulmateList _soulmateList;
        private SoulfailList _soulfailList;
        private MagicBonusList _magicBonusList;
        private CritModList _critModList;
        private CritrateList _critrateList;
        private SpecialChanceList _specialChanceList;
        private ExpLimitList _expLimitList;
        private HealExpList _healExpList;
        private WeaponSpellRankList _weaponSpellRankList;
        private StatusEffectList _statusEffectList;

        public frmX013_Editor()
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

        protected override string FileDialogFilter => "SF3 scn3 data (X013.bin)|X013.bin|Binary File (*.bin)|*.bin|" + "All Files (*.*)|*.*";

        protected override IFileEditor MakeFileEditor() => new X013_FileEditor(Scenario);

        protected override bool LoadOpenedFile()
        {
            var fileEditor = FileEditor as IX013_FileEditor;

            _specialsList = new SpecialList(fileEditor);
            _supportTypeList = new SupportTypeList(fileEditor);
            _friendshipExpList = new FriendshipExpList(fileEditor);
            _supportStatsList = new SupportStatsList(fileEditor);
            _soulmateList = new SoulmateList(fileEditor);
            _soulfailList = new SoulfailList(fileEditor);
            _magicBonusList = new MagicBonusList(fileEditor);
            _critModList = new CritModList(fileEditor);
            _critrateList = new CritrateList(fileEditor);
            _specialChanceList = new SpecialChanceList(fileEditor);
            _expLimitList = new ExpLimitList(fileEditor);
            _healExpList = new HealExpList(fileEditor);
            _weaponSpellRankList = new WeaponSpellRankList(fileEditor);
            _statusEffectList = new StatusEffectList(fileEditor);

            return tabMain.PopulateTabs(new List<PopulateTabConfig>()
            {
                new PopulateTabConfig(tabSpecials, olvSpecials, _specialsList),
                new PopulateTabConfig(tabFriendshipExp, olvFriendshipExp, _friendshipExpList),
                new PopulateTabConfig(tabSupportType, olvSupportType, _supportTypeList),
                new PopulateTabConfig(tabSupportStats, olvSupportStats, _supportStatsList),
                new PopulateTabConfig(tabSoulmate, olvSoulmate, _soulmateList),
                new PopulateTabConfig(tabSoulmateChanceFail, olvSoulmateChanceFail, _soulfailList),
                new PopulateTabConfig(tabMagicBonus, olvMagicBonus, _magicBonusList),
                new PopulateTabConfig(tabCritVantages, olvCritVantages, _critModList),
                new PopulateTabConfig(tabCritCounterRate, olvCritCounterRate, _critrateList),
                new PopulateTabConfig(tabSpecialChance, olvSpecialChance, _specialChanceList),
                new PopulateTabConfig(tabExpLimit, olvExpLimit, _expLimitList),
                new PopulateTabConfig(tabHealExp, olvHealExp, _healExpList),
                new PopulateTabConfig(tabWeaponSpellRank, olvWeaponSpellRank, _weaponSpellRankList),
                new PopulateTabConfig(tabStatusGroups, olvStatusGroups, _statusEffectList)
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
