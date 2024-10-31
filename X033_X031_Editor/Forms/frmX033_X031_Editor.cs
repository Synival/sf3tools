using System;
using System.Collections.Generic;
using BrightIdeasSoftware;
using SF3.Editor.Extensions;
using SF3.Editor.Forms;
using SF3.FileEditors;
using SF3.Models;
using static SF3.Editor.Extensions.TabControlExtensions;

namespace SF3.X033_X031_Editor.Forms {
    public partial class frmX033_X031_Editor : EditorForm {
        // Used to display version in the application
        protected override string Version => "0.21";

        public new IX033_X031_FileEditor FileEditor => base.FileEditor as IX033_X031_FileEditor;
        private StatGrowthChart _statGrowthChart;

        public frmX033_X031_Editor() {
            InitializeComponent();
            InitializeEditor(menuStrip2);
            _statGrowthChart = new StatGrowthChart(CurveGraph);
        }

        protected override string FileDialogFilter => "SF3 Data (X033.BIN;X031.BIN)|X033.BIN;X031.BIN|" + base.FileDialogFilter;

        protected override IFileEditor MakeFileEditor() => new X033_X031_FileEditor(Scenario);

        protected override bool OnLoad() {
            if (!base.OnLoad())
                return false;

            if (!tabMain.PopulateAndToggleTabs(new List<PopulateTabConfig>() {
                new PopulateTabConfig(tabStats, olvStats, FileEditor.StatsTable),
                new PopulateTabConfig(tabSpells, olvSpells, FileEditor.StatsTable),
                new PopulateTabConfig(tabEquipStatistics, olvEquipStatistics, FileEditor.StatsTable),
                new PopulateTabConfig(tabMiscellaneous, olvMiscellaneous, FileEditor.StatsTable),
                new PopulateTabConfig(tabInitialInfo, olvInitialInfo, FileEditor.InitialInfoTable),
                new PopulateTabConfig(tabWeaponLevelReq, olvWeaponLevelReq, FileEditor.WeaponLevelTable),
                new PopulateTabConfig(tabCurveCalc, olvCurveCalc, FileEditor.StatsTable)
            })) {
                return false;
            }

            // Update curve graph controls.
            cbCurveGraphCharacter.DataSource = FileEditor.StatsTable.Rows;
            cbCurveGraphCharacter.DisplayMember = "Name";

            return true;
        }

        private void olvCellEditStarting(object sender, CellEditEventArgs e) => (sender as ObjectListView).EnhanceOlvCellEditControl(e);

        private void tsmiDebug_DebugCurve_Click(object sender, EventArgs e) {
            Stats.DebugGrowthValues = !Stats.DebugGrowthValues;
            tsmiDebug_DebugCurve.Checked = Stats.DebugGrowthValues;
        }

        private void tabMain_Click(object sender, EventArgs e) {
            olvCurveCalc.ClearObjects();
            if (FileEditor?.StatsTable != null)
                olvCurveCalc.AddObjects(FileEditor?.StatsTable.Rows);
        }

        private void CurveGraphCharacterComboBox_SelectedIndexChanged(object sender, EventArgs e)
            => _statGrowthChart.RefreshCurveGraph(FileEditor.StatsTable, cbCurveGraphCharacter);
    }
}
