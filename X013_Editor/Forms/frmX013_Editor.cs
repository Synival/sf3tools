using System.Collections.Generic;
using BrightIdeasSoftware;
using SF3.Win.Extensions;
using SF3.Win.Forms;
using SF3.ModelLoaders;
using static SF3.Win.Extensions.TabControlExtensions;
using SF3.NamedValues;
using SF3.Models.Files;
using SF3.Models.Files.X013;

namespace SF3.X013_Editor.Forms {
    public partial class frmX013_Editor : EditorForm {
        // Used to display version in the application
        protected override string Version => "0.21";

        public IX013_File File => base.FileLoader.Model as IX013_File;

        public frmX013_Editor() {
            InitializeComponent();
            InitializeEditor();
        }

        protected override string FileDialogFilter => "SF3 Data (X013.BIN)|X013.BIN|" + base.FileDialogFilter;

        protected override IBaseFile MakeEditor(IModelFileLoader loader)
            => X013_File.Create(loader.RawData, new NameGetterContext(Scenario), Scenario);

        protected override bool OnLoad() {
            if (!base.OnLoad())
                return false;

            return tabMain.PopulateAndToggleTabs(new List<IPopulateTabConfig>() {
                new PopulateOLVTabConfig(tabSpecials, olvSpecials, File.SpecialsTable),
                new PopulateOLVTabConfig(tabSpecialEffects, olvSpecialEffects, File.SpecialEffectTable),
                new PopulateOLVTabConfig(tabFriendshipExp, olvFriendshipExp, File.FriendshipExpTable),
                new PopulateOLVTabConfig(tabSupportType, olvSupportType, File.SupportTypeTable),
                new PopulateOLVTabConfig(tabSupportStats, olvSupportStats, File.SupportStatsTable),
                new PopulateOLVTabConfig(tabSoulmate, olvSoulmate, File.SoulmateTable),
                new PopulateOLVTabConfig(tabSoulmateChanceFail, olvSoulmateChanceFail, File.SoulfailTable),
                new PopulateOLVTabConfig(tabMagicBonus, olvMagicBonus, File.MagicBonusTable),
                new PopulateOLVTabConfig(tabCritVantages, olvCritVantages, File.CritModTable),
                new PopulateOLVTabConfig(tabCritCounterRate, olvCritCounterRate, File.CritrateTable),
                new PopulateOLVTabConfig(tabSpecialChance, olvSpecialChance, File.SpecialChanceTable),
                new PopulateOLVTabConfig(tabExpLimit, olvExpLimit, File.ExpLimitTable),
                new PopulateOLVTabConfig(tabHealExp, olvHealExp, File.HealExpTable),
                new PopulateOLVTabConfig(tabWeaponSpellRank, olvWeaponSpellRank, File.WeaponSpellRankTable),
                new PopulateOLVTabConfig(tabStatusGroups, olvStatusGroups, File.StatusEffectTable)
            });
        }
    }
}
