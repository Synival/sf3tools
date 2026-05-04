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
            if (Model.SlotTable != null) {
                CreateChild(new TableView("Slots 1",              Model.SlotTable, ngc, displayGroups: ["Metadata", "Page1"]));
                CreateChild(new TableView("Slots 2",              Model.SlotTable, ngc, displayGroups: ["Metadata", "Page2"]));
                CreateChild(new TableView("Slots 3 (Conditions)", Model.SlotTable, ngc, displayGroups: ["Metadata", "Page3"]));
                CreateChild(new TableView("Slots 4 (AI)",         Model.SlotTable, ngc, displayGroups: ["Metadata", "Page4"]));
                CreateChild(new TableView("Slots 5 (Flags)",      Model.SlotTable, ngc, displayGroups: ["Metadata", "Page5"]));
            }
            if (Model.ZoneTable != null)
                CreateChild(new TableView("Zones", Model.ZoneTable, ngc));
            if (Model.AITargetLocationsTable != null)
                CreateChild(new TableView("Locations", Model.AITargetLocationsTable, ngc));
            if (Model.AITarrgetPathTable != null)
                CreateChild(new TableView("Paths", Model.AITarrgetPathTable, ngc));
            if (Model.MapMoveCoordTable != null)
                CreateChild(new TableView("Map Move Coords", Model.MapMoveCoordTable, ngc));

            return Control;
        }

        public BattleMap Model { get; }
        public INameGetterContext NameGetterContext { get; }
    }
}
