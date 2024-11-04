using System.Collections.Generic;
using BrightIdeasSoftware;
using SF3.Editor.Extensions;
using SF3.Editor.Forms;
using SF3.FileEditors;
using static SF3.Editor.Extensions.TabControlExtensions;

namespace SF3.X013_Editor.Forms {
    public partial class frmX013_Editor : EditorForm {
        // Used to display version in the application
        protected override string Version => "0.20";

        public new IX013_FileEditor FileEditor => base.FileEditor as IX013_FileEditor;

        public frmX013_Editor() {
            InitializeComponent();
            InitializeEditor();
        }

        protected override string FileDialogFilter => "SF3 Data (X013.BIN)|X013.BIN|" + base.FileDialogFilter;

        protected override IFileEditor MakeFileEditor() => new X013_FileEditor(Scenario);

        protected override bool OnLoad() {
            if (!base.OnLoad())
                return false;

            return tabMain.PopulateAndToggleTabs(new List<IPopulateTabConfig>() {
                new PopulateOLVTabConfig(tabSpecials, olvSpecials, FileEditor.SpecialsTable),
                new PopulateOLVTabConfig(tabFriendshipExp, olvFriendshipExp, FileEditor.FriendshipExpTable),
                new PopulateOLVTabConfig(tabSupportType, olvSupportType, FileEditor.SupportTypeTable),
                new PopulateOLVTabConfig(tabSupportStats, olvSupportStats, FileEditor.SupportStatsTable),
                new PopulateOLVTabConfig(tabSoulmate, olvSoulmate, FileEditor.SoulmateTable),
                new PopulateOLVTabConfig(tabSoulmateChanceFail, olvSoulmateChanceFail, FileEditor.SoulfailTable),
                new PopulateOLVTabConfig(tabMagicBonus, olvMagicBonus, FileEditor.MagicBonusTable),
                new PopulateOLVTabConfig(tabCritVantages, olvCritVantages, FileEditor.CritModTable),
                new PopulateOLVTabConfig(tabCritCounterRate, olvCritCounterRate, FileEditor.CritrateTable),
                new PopulateOLVTabConfig(tabSpecialChance, olvSpecialChance, FileEditor.SpecialChanceTable),
                new PopulateOLVTabConfig(tabExpLimit, olvExpLimit, FileEditor.ExpLimitTable),
                new PopulateOLVTabConfig(tabHealExp, olvHealExp, FileEditor.HealExpTable),
                new PopulateOLVTabConfig(tabWeaponSpellRank, olvWeaponSpellRank, FileEditor.WeaponSpellRankTable),
                new PopulateOLVTabConfig(tabStatusGroups, olvStatusGroups, FileEditor.StatusEffectTable)
            });
        }

        private void olvCellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e) => (sender as ObjectListView).EnhanceOlvCellEditControl(e);
    }
}
