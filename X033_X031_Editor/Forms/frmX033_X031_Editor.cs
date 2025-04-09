using System;
using System.Collections.Generic;
using SF3.ModelLoaders;
using SF3.Models.Files;
using SF3.Models.Files.X033_X031;
using SF3.Models.Structs.X033_X031;
using SF3.NamedValues;
using SF3.Win.Extensions;
using SF3.Win.Forms;
using static SF3.Win.Extensions.TabControlExtensions;

namespace SF3.X033_X031_Editor.Forms {
    public partial class frmX033_X031_Editor : EditorForm {
        // Used to display version in the application
        protected override string Version => "0.23 (2025-02-26 dev)";

        public IX033_X031_File File => base.FileLoader.Model as IX033_X031_File;

        public frmX033_X031_Editor() {
            InitializeComponent();
            InitializeEditor(menuStrip2);
        }

        protected override string FileDialogFilter => "SF3 Data (X033.BIN;X031.BIN)|X033.BIN;X031.BIN|" + base.FileDialogFilter;

        protected override IBaseFile CreateModel(IModelFileLoader loader)
            => X033_X031_File.Create(loader.ByteData, new NameGetterContext(Scenario), Scenario);

        protected override bool OnLoad() {
            if (!base.OnLoad())
                return false;

            if (!tabMain.PopulateAndToggleTabs(new List<IPopulateTabConfig>() {
                new PopulateOLVTabConfig(tabEquipStatistics, olvEquipStatistics, File.StatsTable),
                new PopulateOLVTabConfig(tabMiscellaneous, olvMiscellaneous, File.StatsTable),
                new PopulateOLVTabConfig(tabInitialInfo, olvInitialInfo, File.InitialInfoTable),
            })) {
                return false;
            }

            // Update curve graph controls.
            cbCurveGraphCharacter.DataSource = File.StatsTable.Rows;
            cbCurveGraphCharacter.DisplayMember = "Name";

            return true;
        }

        private void tsmiDebug_DebugCurve_Click(object sender, EventArgs e) {
            Stats.DebugGrowthValues = !Stats.DebugGrowthValues;
            tsmiDebug_DebugCurve.Checked = Stats.DebugGrowthValues;
        }

        private void tabMain_Click(object sender, EventArgs e) {
            olvCurveCalc.ClearObjects();
            if (File?.StatsTable != null)
                olvCurveCalc.AddObjects(File?.StatsTable.Rows);
        }

        private void CurveGraphCharacterComboBox_SelectedIndexChanged(object sender, EventArgs e) {}
    }
}
