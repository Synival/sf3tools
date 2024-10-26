using System;
using System.Collections.Generic;
using BrightIdeasSoftware;
using SF3.Editor.Extensions;
using SF3.Editor.Forms;
using SF3.FileEditors;
using SF3.Types;
using static SF3.Editor.Extensions.TabControlExtensions;

namespace SF3.X002_Editor.Forms {
    public partial class frmX002_Editor : EditorForm {
        // Used to display version in the application
        protected override string Version => "0.22";

        new public IX002_FileEditor FileEditor => base.FileEditor as IX002_FileEditor;

        public frmX002_Editor() {
            InitializeComponent();
            InitializeEditor(null);
        }

        private void tabMain_Click(object sender, EventArgs e) {
            olvSpells.ClearObjects();
            if (FileEditor?.SpellList != null)
                olvSpells.AddObjects(FileEditor.SpellList.Models);

            olvStatBoost.ClearObjects();
            if (FileEditor?.StatList != null)
                olvStatBoost.AddObjects(FileEditor.StatList.Models);
        }

        protected override string FileDialogFilter => "SF3 Data (X002.BIN)|X002.BIN|" + base.FileDialogFilter;

        protected override IFileEditor MakeFileEditor() => new X002_FileEditor(Scenario);

        protected override bool OnLoad() {
            if (!base.OnLoad())
                return false;

            return tabMain.PopulateAndToggleTabs(new List<PopulateTabConfig>()             {
                new PopulateTabConfig(tabItems, olvItems, FileEditor.ItemList),
                new PopulateTabConfig(tabSpells, olvSpells, FileEditor.SpellList),
                new PopulateTabConfig(tabPreset, olvPreset, FileEditor.PresetList),
                new PopulateTabConfig(tabLoaded, olvLoaded, FileEditor.LoadList),
                new PopulateTabConfig(tabLoadedOverride, olvLoadedOverride, FileEditor.LoadedOverrideList),
                new PopulateTabConfig(tabStatBoost, olvStatBoost, FileEditor.StatList),
                new PopulateTabConfig(tabWeaponRankAttack, olvWeaponRankAttack, FileEditor.WeaponRankList),
                new PopulateTabConfig(tabAttackResist, olvAttackResist, FileEditor.AttackResistList),
                new PopulateTabConfig(tabWarpTable, olvWarpTable, FileEditor.WarpList),
            });
        }

        private void olvCellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e) => (sender as ObjectListView).EnhanceOlvCellEditControl(e);
    }
}
