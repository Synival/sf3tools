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
                CreateChild(new TableView("Stats", Model.StatsTable, ngc, displayGroups: ["Metadata", "Stats"]));
                CreateChild(new TableView("Spells", Model.StatsTable, ngc, displayGroups: ["Metadata", "Spells"]));
                CreateChild(new TableView("Miscellaneous", Model.StatsTable, ngc, displayGroups: ["Metadata", "Miscellaneous"]));
            }
            if (Model.InitialInfoTable != null)
                CreateChild(new TableView("Initial Info", Model.InitialInfoTable, ngc));
            if (Model.WeaponLevelExp != null)
                CreateChild(new DataModelView("Weapon Level Exp", Model.WeaponLevelExp, ngc));
            if (Model.StatsTable != null)
                CreateChild(new TableView("Curve Calc", Model.StatsTable, ngc, displayGroups: ["Metadata", "CurveGraph"]));

            // TODO: Curve Graph

            return Control;
        }

        public IX033_X031_File Model { get; }
    }
}
