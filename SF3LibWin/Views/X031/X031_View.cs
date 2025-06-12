using System.Windows.Forms;
using SF3.Models.Files.X031;

namespace SF3.Win.Views.X031 {
    public class X031_View : TabView {
        public X031_View(string name, IX031_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;

            if (Model.StatsTable != null) {
                CreateChild(new StatTableView("Character Stats", Model.StatsTable, ngc));
                CreateChild(new StatGrowthChartView("Growth Graph", Model.StatsTable));
            }
            if (Model.InitialInfoTable != null)
                CreateChild(new TableView("Initial Info", Model.InitialInfoTable, ngc));
            if (Model.WeaponLevelExp != null)
                CreateChild(new DataModelView("Weapon Level Exp", Model.WeaponLevelExp, ngc));

            return Control;
        }

        public IX031_File Model { get; }
    }
}
