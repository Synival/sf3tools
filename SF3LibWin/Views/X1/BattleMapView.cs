using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Models.Structs.X1.Battle;

namespace SF3.Win.Views.X1 {
    public class BattleMapView : TabView {
        public BattleMapView(string name, BattleMap model, INameGetterContext nameGetterContext) : base(name) {
            Model = model;
            NameGetterContext = nameGetterContext;
        }

        public override Control Create() {
            base.Create();

            var ngc = NameGetterContext;
            if (Model.Header != null)
                CreateChild(new DataModelView("Header", Model.Header, ngc));
            if (Model.UnitTable != null) {
                CreateChild(new TableView("Units 1",              Model.UnitTable, ngc, displayGroups: ["Metadata", "Page1"]));
                CreateChild(new TableView("Units 2",              Model.UnitTable, ngc, displayGroups: ["Metadata", "Page2"]));
                CreateChild(new TableView("Units 3 (Conditions)", Model.UnitTable, ngc, displayGroups: ["Metadata", "Page3"]));
                CreateChild(new TableView("Units 4 (AI)",         Model.UnitTable, ngc, displayGroups: ["Metadata", "Page4"]));
                CreateChild(new TableView("Units 5 (Flags)",      Model.UnitTable, ngc, displayGroups: ["Metadata", "Page5"]));
            }
            if (Model.ZoneTable != null)
                CreateChild(new TableView("Zones", Model.ZoneTable, ngc));
            if (Model.LocationTable != null)
                CreateChild(new TableView("Locations", Model.LocationTable, ngc));
            if (Model.PathTable != null)
                CreateChild(new TableView("Paths", Model.PathTable, ngc));
            if (Model.MapMoveTargetTable != null)
                CreateChild(new TableView("Map Move Arrival Locations", Model.MapMoveTargetTable, ngc));

            return Control;
        }

        public BattleMap Model { get; }
        public INameGetterContext NameGetterContext { get; }
    }
}
