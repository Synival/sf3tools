using System.Windows.Forms;
using SF3.Models.Files.X033_X031;

namespace SF3.Win.Views.X033_X031 {
    public class X033_X031_View : TabView {
        public X033_X031_View(string name, IX033_X031_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;

            if (Model.StatsTable != null) {
                CreateChild(new TableView("Characters and Classes", Model.StatsTable, ngc, displayGroups: ["Metadata", "CharAndClass"]));
                CreateChild(new TableView("Stats (1/2) (Growth)",   Model.StatsTable, ngc, displayGroups: ["Metadata", "Stats"]));
                CreateChild(new TableView("Stats (2/2)",            Model.StatsTable, ngc, displayGroups: ["Metadata", "Stats2"]));
                CreateChild(new TableView("Magic Res",              Model.StatsTable, ngc, displayGroups: ["Metadata", "MagicRes"]));
                CreateChild(new TableView("Spells",                 Model.StatsTable, ngc, displayGroups: ["Metadata", "Spells"]));
                CreateChild(new TableView("Weapons / Accessories",  Model.StatsTable, ngc, displayGroups: ["Metadata", "Equipment"]));
                CreateChild(new TableView("Specials",               Model.StatsTable, ngc, displayGroups: ["Metadata", "Specials"]));
                CreateChild(new TableView("Growth Curve Calc",      Model.StatsTable, ngc, displayGroups: ["Metadata", "CurveCalc"]));
                CreateChild(new StatGrowthChartView("Growth Graph",     Model.StatsTable));
            }

            if (Model.InitialInfoTable != null)
                CreateChild(new TableView("Initial Info", Model.InitialInfoTable, ngc));

            if (Model.WeaponLevelExp != null)
                CreateChild(new DataModelView("Weapon Level Exp", Model.WeaponLevelExp, ngc));

            // TODO: Curve Graph

            return Control;
        }

        public IX033_X031_File Model { get; }
    }
}
