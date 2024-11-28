using System;
using System.Collections.Generic;
using BrightIdeasSoftware;
using SF3.Win.Extensions;
using SF3.Win.Forms;
using SF3.Editors;
using SF3.Editors.X002;
using SF3.ModelLoaders;
using SF3.NamedValues;
using static SF3.Win.Extensions.TabControlExtensions;

namespace SF3.X002_Editor.Forms {
    public partial class frmX002_Editor : EditorForm {
        // Used to display version in the application
        protected override string Version => "0.24";

        public IX002_Editor Editor => base.FileLoader.Editor as IX002_Editor;

        public frmX002_Editor() {
            InitializeComponent();
            InitializeEditor(menuStrip2);
        }

        protected override string FileDialogFilter => "SF3 Data (X002.BIN)|X002.BIN|" + base.FileDialogFilter;

        protected override IBaseEditor MakeEditor(IFileLoader loader)
            => Editors.X002.X002_Editor.Create(loader.RawEditor, new NameGetterContext(Scenario), Scenario);

        protected override bool OnLoad() {
            if (!base.OnLoad())
                return false;

            return tabMain.PopulateAndToggleTabs(new List<IPopulateTabConfig>()             {
                new PopulateOLVTabConfig(tabItems, olvItems, Editor.ItemTable),
                new PopulateOLVTabConfig(tabSpells, olvSpells, Editor.SpellTable),
                new PopulateOLVTabConfig(tabWeaponSpells, olvWeaponSpells, Editor.WeaponSpellTable),
                new PopulateOLVTabConfig(tabLoaded, olvLoaded, Editor.LoadingTable),
                new PopulateOLVTabConfig(tabLoadedOverride, olvLoadedOverride, Editor.LoadedOverrideTable),
                new PopulateOLVTabConfig(tabStatBoost, olvStatBoost, Editor.StatBoostTable),
                new PopulateOLVTabConfig(tabWeaponRankAttack, olvWeaponRankAttack, Editor.WeaponRankTable),
                new PopulateOLVTabConfig(tabAttackResist, olvAttackResist, Editor.AttackResistTable),
                new PopulateOLVTabConfig(tabWarpTable, olvWarpTable, Editor.WarpTable),
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