using System.Collections.Generic;
using SF3.ModelLoaders;
using SF3.Models.Files;
using SF3.Models.Files.X013;
using SF3.NamedValues;
using SF3.Win.Extensions;
using SF3.Win.Forms;
using static SF3.Win.Extensions.TabControlExtensions;

namespace SF3.X013_Editor.Forms {
    public partial class frmX013_Editor : EditorForm {
        // Used to display version in the application
        protected override string Version => "0.21 (2025-02-26 dev)";

        public IX013_File File => base.FileLoader.Model as IX013_File;

        public frmX013_Editor() {
            InitializeComponent();
            InitializeEditor();
        }

        protected override string FileDialogFilter => "SF3 Data (X013.BIN)|X013.BIN|" + base.FileDialogFilter;

        protected override IBaseFile CreateModel(IModelFileLoader loader)
            => X013_File.Create(loader.ByteData, new NameGetterContext(Scenario), Scenario);

        protected override bool OnLoad() {
            if (!base.OnLoad())
                return false;

            return tabMain.PopulateAndToggleTabs(new List<IPopulateTabConfig>() {
                new PopulateOLVTabConfig(tabSpecialEffects, olvSpecialEffects, File.SpecialEffectTable),
                new PopulateOLVTabConfig(tabSupportType, olvSupportType, File.SupportTypeTable),
                new PopulateOLVTabConfig(tabSupportStats, olvSupportStats, File.SupportStatsTable),
                new PopulateOLVTabConfig(tabSoulmate, olvSoulmate, File.SoulmateTable),
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
