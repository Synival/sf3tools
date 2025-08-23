using System.Windows.Forms;
using SF3.Models.Files.X033;
using SF3.Win.Views.X1;

namespace SF3.Win.Views.X033 {
    public class X033_View : TabView {
        public X033_View(string name, IX033_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;

            if (Model.StatsTable != null) {
                CreateChild(new StatTableView("Character Stats", Model.StatsTable, ngc));
                CreateChild(new StatGrowthStatisticsView("Stat Statistics", Model.StatGrowthStatistics, ngc));
            }
            if (Model.InitialInfoTable != null)
                CreateChild(new TableView("Initial Info", Model.InitialInfoTable, ngc));
            if (Model.WeaponLevelExp != null)
                CreateChild(new DataModelView("Weapon Level Exp", Model.WeaponLevelExp, ngc));

            CreateChild(new TechnicalView("Technical Info", Model));

            return Control;
        }

        public IX033_File Model { get; }
    }
}
