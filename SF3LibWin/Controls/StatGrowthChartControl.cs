using System;
using System.Windows.Forms;
using SF3.Models.Tables.X033_X031;

namespace SF3.Win.Controls {
    public partial class StatGrowthChartControl : UserControl {
        public StatGrowthChartControl() {
            InitializeComponent();
            _statGrowthChart = new StatGrowthChart(CurveGraph);
        }

        public StatGrowthChartControl(StatsTable statsTable) {
            InitializeComponent();
            _statGrowthChart = new StatGrowthChart(CurveGraph);
            StatsTable = statsTable;
        }

        public void RefreshData() {
            cbCurveGraphCharacter.DataSource = StatsTable.Rows;
            cbCurveGraphCharacter.DisplayMember = "Name";
            _statGrowthChart?.RefreshCurveGraph(StatsTable, cbCurveGraphCharacter);
        }

        private void CurveGraphCharacterComboBox_SelectedIndexChanged(object sender, EventArgs e)
            => _statGrowthChart.RefreshCurveGraph(StatsTable, cbCurveGraphCharacter);

        private StatsTable _statsTable = null;
        public StatsTable StatsTable {
            get => _statsTable;
            set {
                if (_statsTable != value) {
                    _statsTable = value;
                    RefreshData();
                }
            }
        }

        private readonly StatGrowthChart _statGrowthChart;
    }
}
