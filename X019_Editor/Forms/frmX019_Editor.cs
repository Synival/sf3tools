using System;
using System.Collections.Generic;
using BrightIdeasSoftware;
using SF3.Editor.Extensions;
using SF3.Editor.Forms;
using SF3.FileEditors;
using SF3.Types;
using static SF3.Editor.Extensions.TabControlExtensions;

namespace SF3.X019_Editor.Forms {
    public partial class frmX019_Editor : EditorForm {
        // Used to display version in the application
        protected override string Version => "0.14";

        private bool _isX044 = false;

        public bool IsX044 {
            get => _isX044;
            set {
                if (_isX044 != value) {
                    _isX044 = value;
                    tsmiScenario_PremiumDiskX044.Checked = Scenario == ScenarioType.PremiumDisk && IsX044;
                }
            }
        }

        public new IX019_FileEditor FileEditor => base.FileEditor as IX019_FileEditor;

        public frmX019_Editor() {
            InitializeComponent();
            InitializeEditor(menuStrip1);
            ScenarioChanged += (obj, e) => tsmiScenario_PremiumDiskX044.Checked = Scenario == ScenarioType.PremiumDisk && IsX044;
        }

        protected override string FileDialogFilter
            => (IsX044 ? "SF3 Data (X044.BIN)|X044.BIN|" : "SF3 Data (X019.BIN)|X019.BIN|") + base.FileDialogFilter;

        protected override IFileEditor MakeFileEditor() => new X019_FileEditor(Scenario, IsX044);

        protected override bool OnLoad() {
            if (!base.OnLoad())
                return false;

            return tabMain.PopulateAndToggleTabs(new List<PopulateTabConfig>() {
                new PopulateTabConfig(tabMonsterTab1, olvMonsterTab1, FileEditor.MonsterList),
                new PopulateTabConfig(tabMonsterTab2, olvMonsterTab2, FileEditor.MonsterList),
                new PopulateTabConfig(tabMonsterTab3, olvMonsterTab3, FileEditor.MonsterList),
                new PopulateTabConfig(tabMonsterTab4, olvMonsterTab4, FileEditor.MonsterList),
                new PopulateTabConfig(tabMonsterTab5, olvMonsterTab5, FileEditor.MonsterList),
            });
        }

        private void olvCellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e) => (sender as ObjectListView).EnhanceOlvCellEditControl(e);

        protected override void tsmiScenario_Scenario1_Click(object sender, EventArgs e) {
            base.tsmiScenario_Scenario1_Click(sender, e);
            IsX044 = false;
        }

        protected override void tsmiScenario_Scenario2_Click(object sender, EventArgs e) {
            base.tsmiScenario_Scenario2_Click(sender, e);
            IsX044 = false;
        }

        protected override void tsmiScenario_Scenario3_Click(object sender, EventArgs e) {
            base.tsmiScenario_Scenario3_Click(sender, e);
            IsX044 = false;
        }

        private void tsmiScenario_PremiumDiskX044_Click(object sender, EventArgs e) {
            Scenario = ScenarioType.PremiumDisk;
            IsX044 = true;
        }
    }
}
