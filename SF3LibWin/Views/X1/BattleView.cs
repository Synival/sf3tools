using System.Linq;
using System.Windows.Forms;
using SF3.Models.Files.X1;

namespace SF3.Win.Views.MPD {
    public class BattleView : TabView {
        public BattleView(string name, Battle model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            base.Create();

            var ngc = Model.NameGetterContext;
            if (Model.BattleHeader != null)
                CreateChild(new ModelView("Header", Model.BattleHeader, ngc));
            if (Model.SlotTable != null) {
                CreateChild(new TableView("Slots1", Model.SlotTable, ngc, displayGroups: ["Metadata", "Page1"]));
                CreateChild(new TableView("Slots2", Model.SlotTable, ngc, displayGroups: ["Metadata", "Page2"]));
                CreateChild(new TableView("Slots3", Model.SlotTable, ngc, displayGroups: ["Metadata", "Page3"]));
                CreateChild(new TableView("Slots4", Model.SlotTable, ngc, displayGroups: ["Metadata", "Page4"]));
            }
            if (Model.SpawnZoneTable != null)
                CreateChild(new TableView("SpawnZones", Model.SpawnZoneTable, ngc));
            if (Model.AITable != null)
                CreateChild(new TableView("AI TargetPosition", Model.AITable, ngc));
            if (Model.CustomMovementTable != null)
                CreateChild(new TableView("ScriptedMovement", Model.CustomMovementTable, ngc));

            return Control;
        }

        public Battle Model { get; }
    }
}
