using System.Collections.Generic;
using BrightIdeasSoftware;
using SF3.Win.Extensions;
using SF3.Win.Forms;
using SF3.FileModels;
using SF3.ModelLoaders;
using static SF3.Win.Extensions.TabControlExtensions;
using SF3.NamedValues;
using SF3.FileModels.X013;

namespace SF3.X013_Editor.Forms {
    public partial class frmX013_Editor : EditorForm {
        // Used to display version in the application
        protected override string Version => "0.21";

        public IX013_Editor Editor => base.FileLoader.Model as IX013_Editor;

        public frmX013_Editor() {
            InitializeComponent();
            InitializeEditor();
        }

        protected override string FileDialogFilter => "SF3 Data (X013.BIN)|X013.BIN|" + base.FileDialogFilter;

        protected override IBaseEditor MakeEditor(IModelFileLoader loader)
            => FileModels.X013.X013_Editor.Create(loader.RawEditor, new NameGetterContext(Scenario), Scenario);

        protected override bool OnLoad() {
            if (!base.OnLoad())
                return false;

            return tabMain.PopulateAndToggleTabs(new List<IPopulateTabConfig>() {
                new PopulateOLVTabConfig(tabSpecials, olvSpecials, Editor.SpecialsTable),
                new PopulateOLVTabConfig(tabSpecialEffects, olvSpecialEffects, Editor.SpecialEffectTable),
                new PopulateOLVTabConfig(tabFriendshipExp, olvFriendshipExp, Editor.FriendshipExpTable),
                new PopulateOLVTabConfig(tabSupportType, olvSupportType, Editor.SupportTypeTable),
                new PopulateOLVTabConfig(tabSupportStats, olvSupportStats, Editor.SupportStatsTable),
                new PopulateOLVTabConfig(tabSoulmate, olvSoulmate, Editor.SoulmateTable),
                new PopulateOLVTabConfig(tabSoulmateChanceFail, olvSoulmateChanceFail, Editor.SoulfailTable),
                new PopulateOLVTabConfig(tabMagicBonus, olvMagicBonus, Editor.MagicBonusTable),
                new PopulateOLVTabConfig(tabCritVantages, olvCritVantages, Editor.CritModTable),
                new PopulateOLVTabConfig(tabCritCounterRate, olvCritCounterRate, Editor.CritrateTable),
                new PopulateOLVTabConfig(tabSpecialChance, olvSpecialChance, Editor.SpecialChanceTable),
                new PopulateOLVTabConfig(tabExpLimit, olvExpLimit, Editor.ExpLimitTable),
                new PopulateOLVTabConfig(tabHealExp, olvHealExp, Editor.HealExpTable),
                new PopulateOLVTabConfig(tabWeaponSpellRank, olvWeaponSpellRank, Editor.WeaponSpellRankTable),
                new PopulateOLVTabConfig(tabStatusGroups, olvStatusGroups, Editor.StatusEffectTable)
            });
        }
    }
}
