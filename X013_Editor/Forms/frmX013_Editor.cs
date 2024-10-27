using System.Collections.Generic;
using BrightIdeasSoftware;
using SF3.Editor.Extensions;
using SF3.Editor.Forms;
using SF3.FileEditors;
using static SF3.Editor.Extensions.TabControlExtensions;

namespace SF3.X013_Editor.Forms {
    public partial class frmX013_Editor : EditorForm {
        // Used to display version in the application
        protected override string Version => "0.19";

        public new IX013_FileEditor FileEditor => base.FileEditor as IX013_FileEditor;

        public frmX013_Editor() {
            InitializeComponent();
            InitializeEditor(null);
        }

        protected override string FileDialogFilter => "SF3 Data (X013.BIN)|X013.BIN|" + base.FileDialogFilter;

        protected override IFileEditor MakeFileEditor() => new X013_FileEditor(Scenario);

        protected override bool OnLoad() {
            if (!base.OnLoad())
                return false;

            return tabMain.PopulateAndToggleTabs(new List<PopulateTabConfig>() {
                new PopulateTabConfig(tabSpecials, olvSpecials, FileEditor.SpecialsTable),
                new PopulateTabConfig(tabFriendshipExp, olvFriendshipExp, FileEditor.FriendshipExpTable),
                new PopulateTabConfig(tabSupportType, olvSupportType, FileEditor.SupportTypeTable),
                new PopulateTabConfig(tabSupportStats, olvSupportStats, FileEditor.SupportStatsTable),
                new PopulateTabConfig(tabSoulmate, olvSoulmate, FileEditor.SoulmateTable),
                new PopulateTabConfig(tabSoulmateChanceFail, olvSoulmateChanceFail, FileEditor.SoulfailTable),
                new PopulateTabConfig(tabMagicBonus, olvMagicBonus, FileEditor.MagicBonusTable),
                new PopulateTabConfig(tabCritVantages, olvCritVantages, FileEditor.CritModTable),
                new PopulateTabConfig(tabCritCounterRate, olvCritCounterRate, FileEditor.CritrateTable),
                new PopulateTabConfig(tabSpecialChance, olvSpecialChance, FileEditor.SpecialChanceTable),
                new PopulateTabConfig(tabExpLimit, olvExpLimit, FileEditor.ExpLimitTable),
                new PopulateTabConfig(tabHealExp, olvHealExp, FileEditor.HealExpTable),
                new PopulateTabConfig(tabWeaponSpellRank, olvWeaponSpellRank, FileEditor.WeaponSpellRankTable),
                new PopulateTabConfig(tabStatusGroups, olvStatusGroups, FileEditor.StatusEffectTable)
            });
        }

        private void olvCellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e) => (sender as ObjectListView).EnhanceOlvCellEditControl(e);
    }
}
