using System;
using System.Collections.Generic;
using BrightIdeasSoftware;
using SF3.Editor.Extensions;
using SF3.Editor.Forms;
using SF3.FileEditors;
using static SF3.Editor.Extensions.TabControlExtensions;

namespace SF3.X002_Editor.Forms {
    public partial class frmX002_Editor : EditorForm {
        // Used to display version in the application
        protected override string Version => "0.22";

        public new IX002_FileEditor FileEditor => base.FileEditor as IX002_FileEditor;

        public frmX002_Editor() {
            InitializeComponent();
            InitializeEditor(menuStrip2);
        }

        protected override string FileDialogFilter => "SF3 Data (X002.BIN)|X002.BIN|" + base.FileDialogFilter;

        protected override IFileEditor MakeFileEditor() => new X002_FileEditor(Scenario);

        protected override bool OnLoad() {
            if (!base.OnLoad())
                return false;

            return tabMain.PopulateAndToggleTabs(new List<IPopulateTabConfig>()             {
                new PopulateOLVTabConfig(tabItems, olvItems, FileEditor.ItemTable),
                new PopulateOLVTabConfig(tabSpells, olvSpells, FileEditor.SpellTable),
                new PopulateOLVTabConfig(tabWeaponSpells, olvWeaponSpells, FileEditor.WeaponSpellTable),
                new PopulateOLVTabConfig(tabLoaded, olvLoaded, FileEditor.LoadingTable),
                new PopulateOLVTabConfig(tabLoadedOverride, olvLoadedOverride, FileEditor.LoadedOverrideTable),
                new PopulateOLVTabConfig(tabStatBoost, olvStatBoost, FileEditor.StatBoostTable),
                new PopulateOLVTabConfig(tabWeaponRankAttack, olvWeaponRankAttack, FileEditor.WeaponRankTable),
                new PopulateOLVTabConfig(tabAttackResist, olvAttackResist, FileEditor.AttackResistTable),
                new PopulateOLVTabConfig(tabWarpTable, olvWarpTable, FileEditor.WarpTable),
            });
        }

        private void olvCellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e) => (sender as ObjectListView).EnhanceOlvCellEditControl(e);

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