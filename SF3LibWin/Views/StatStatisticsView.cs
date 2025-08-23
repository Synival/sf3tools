using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Models.Tables.Shared;

namespace SF3.Win.Views {
    public class StatStatisticsView : TabView {
        public StatStatisticsView(string name, StatStatisticsTable model, INameGetterContext ngc) : base(name) {
            Model = model;
            NameGetterContext = ngc;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = NameGetterContext;
            CreateChild(new StatGrowthChartView("Growth Graph", Model));
            CreateChild(new TableView("Growth Curve Calc", Model, ngc, displayGroups: ["Metadata", "CurveCalc"]));

            return Control;
        }

        public StatStatisticsTable Model { get; }
        public INameGetterContext NameGetterContext { get; }
    }
}
