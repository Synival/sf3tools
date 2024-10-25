using System;
using System.Collections.Generic;
using BrightIdeasSoftware;
using SF3.Editor.Extensions;
using SF3.Editor.Forms;
using SF3.FileEditors;
using SF3.Types;
using static SF3.Editor.Extensions.TabControlExtensions;

namespace SF3.X013_Editor.Forms {
    public partial class frmX013_Editor : EditorForm {
        // Used to display version in the application
        protected override string Version => "0.19";

        new public IX013_FileEditor FileEditor => base.FileEditor as IX013_FileEditor;

        public frmX013_Editor() {
            InitializeComponent();
            InitializeEditor(null);
        }

        protected override string FileDialogFilter => "SF3 data (X013.bin)|X013.bin|Binary File (*.bin)|*.bin|" + "All Files (*.*)|*.*";

        protected override IFileEditor MakeFileEditor() => new X013_FileEditor(Scenario);

        protected override bool OnLoad() {
            if (!base.OnLoad())
                return false;

            return tabMain.PopulateAndToggleTabs(new List<PopulateTabConfig>() {
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
    }
}
