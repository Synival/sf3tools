﻿using System.Windows.Forms;
using SF3.Models.Tables.X033_X031;
using SF3.Win.Controls;

namespace SF3.Win.Views {
    public class GrowthChartView : ControlView<StatGrowthChartControl> {
        public GrowthChartView(string name, StatsTable statsTable) : base(name) {
            StatsTable = statsTable;
        }

        public override Control Create() {
            var rval = base.Create();
            Control.StatsTable = StatsTable;
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

        public StatsTable StatsTable { get; }
    }
}
