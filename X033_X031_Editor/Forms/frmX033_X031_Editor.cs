using System;
using System.Collections.Generic;
using BrightIdeasSoftware;
using SF3.Win.Extensions;
using SF3.Win.Forms;
using SF3.FileModels;
using SF3.ModelLoaders;
using SF3.Models.X033_X031;
using static SF3.Win.Extensions.TabControlExtensions;
using SF3.NamedValues;
using SF3.FileModels.X033_X031;

namespace SF3.X033_X031_Editor.Forms {
    public partial class frmX033_X031_Editor : EditorForm {
        // Used to display version in the application
        protected override string Version => "0.23";

        public IX033_X031_Editor Editor => base.FileLoader.Model as IX033_X031_Editor;
        private StatGrowthChart _statGrowthChart;

        public frmX033_X031_Editor() {
            InitializeComponent();
            InitializeEditor(menuStrip2);
            _statGrowthChart = new StatGrowthChart(CurveGraph);
        }

        protected override string FileDialogFilter => "SF3 Data (X033.BIN;X031.BIN)|X033.BIN;X031.BIN|" + base.FileDialogFilter;

        protected override IBaseEditor MakeEditor(IModelFileLoader loader)
            => Editors.X033_X031.X033_X031_Editor.Create(loader.RawEditor, new NameGetterContext(Scenario), Scenario);

        protected override bool OnLoad() {
            if (!base.OnLoad())
                return false;

            if (!tabMain.PopulateAndToggleTabs(new List<IPopulateTabConfig>() {
                new PopulateOLVTabConfig(tabStats, olvStats, Editor.StatsTable),
                new PopulateOLVTabConfig(tabSpells, olvSpells, Editor.StatsTable),
                new PopulateOLVTabConfig(tabEquipStatistics, olvEquipStatistics, Editor.StatsTable),
                new PopulateOLVTabConfig(tabMiscellaneous, olvMiscellaneous, Editor.StatsTable),
                new PopulateOLVTabConfig(tabInitialInfo, olvInitialInfo, Editor.InitialInfoTable),
                new PopulateOLVTabConfig(tabWeaponLevelReq, olvWeaponLevelReq, Editor.WeaponLevelTable),
                new PopulateOLVTabConfig(tabCurveCalc, olvCurveCalc, Editor.StatsTable)
            })) {
                return false;
            }

            // Update curve graph controls.
            cbCurveGraphCharacter.DataSource = Editor.StatsTable.Rows;
            cbCurveGraphCharacter.DisplayMember = "Name";

            return true;
        }

        private void tsmiDebug_DebugCurve_Click(object sender, EventArgs e) {
            Stats.DebugGrowthValues = !Stats.DebugGrowthValues;
            tsmiDebug_DebugCurve.Checked = Stats.DebugGrowthValues;
        }

        private void tabMain_Click(object sender, EventArgs e) {
            olvCurveCalc.ClearObjects();
            if (Editor?.StatsTable != null)
                olvCurveCalc.AddObjects(Editor?.StatsTable.Rows);
        }

        private void CurveGraphCharacterComboBox_SelectedIndexChanged(object sender, EventArgs e)
            => _statGrowthChart.RefreshCurveGraph(Editor.StatsTable, cbCurveGraphCharacter);
    }
}
