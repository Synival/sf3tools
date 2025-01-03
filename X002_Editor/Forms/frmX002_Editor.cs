using System;
using System.Collections.Generic;
using SF3.ModelLoaders;
using SF3.Models.Files;
using SF3.Models.Files.X002;
using SF3.NamedValues;
using SF3.Win.Extensions;
using SF3.Win.Forms;
using static SF3.Win.Extensions.TabControlExtensions;

namespace SF3.X002_Editor.Forms {
    public partial class frmX002_Editor : EditorForm {
        // Used to display version in the application
        protected override string Version => "0.24";

        public IX002_File File => base.FileLoader.Model as IX002_File;

        public frmX002_Editor() {
            InitializeComponent();
            InitializeEditor(menuStrip2);
        }

        protected override string FileDialogFilter => "SF3 Data (X002.BIN)|X002.BIN|" + base.FileDialogFilter;

        protected override IBaseFile CreateModel(IModelFileLoader loader)
            => X002_File.Create(loader.ByteData, new NameGetterContext(Scenario), Scenario);

        protected override bool OnLoad() {
            if (!base.OnLoad())
                return false;

            return tabMain.PopulateAndToggleTabs(new List<IPopulateTabConfig>() {
                new PopulateOLVTabConfig(tabItems, olvItems, File.ItemTable),
                new PopulateOLVTabConfig(tabSpells, olvSpells, File.SpellTable),
                new PopulateOLVTabConfig(tabWeaponSpells, olvWeaponSpells, File.WeaponSpellTable),
                new PopulateOLVTabConfig(tabLoaded, olvLoaded, File.LoadingTable),
                new PopulateOLVTabConfig(tabLoadedOverride, olvLoadedOverride, File.LoadedOverrideTable),
                new PopulateOLVTabConfig(tabStatBoost, olvStatBoost, File.StatBoostTable),
                new PopulateOLVTabConfig(tabWeaponRankAttack, olvWeaponRankAttack, File.WeaponRankTable),
                new PopulateOLVTabConfig(tabAttackResist, olvAttackResist, File.AttackResistTable),
                new PopulateOLVTabConfig(tabWarpTable, olvWarpTable, File.WarpTable),
            });
        }

        private void tsmiDebug_ShowDebugColumns_Click(object sender, EventArgs e) {
            bool isChecked = !tsmiDebug_ShowDebugColumns.Checked;
            tsmiDebug_ShowDebugColumns.Checked = isChecked;
            lvcItemsEffectsEquip.IsVisible = isChecked;
            lvcItemsRequirements.IsVisible = isChecked;

            olvItems.FinishCellEdit();
            olvItems.Hide();
            olvItems.RebuildColumns();
            olvItems.Show();
        }
    }
}