using System.Windows.Forms;
using SF3.Models.Tables.Shared;
using SF3.Win.Controls;

namespace SF3.Win.Views {
    public class StatGrowthChartView : ControlView<StatGrowthChartControl> {
        public StatGrowthChartView(string name, StatGrowthStatisticsTable statStatistics) : base(name) {
            StatStatistics = statStatistics;
        }

        public override Control Create() {
            var rval = base.Create();
            Control.StatGrowthStatistics = StatStatistics;
            return rval;
        }

        public override void Destroy() {
            base.Destroy();
        }

        public override void RefreshContent() {
            if (!IsCreated)
                return;
            Control.RefreshData();
        }

        public StatGrowthStatisticsTable StatStatistics { get; }
    }
}
