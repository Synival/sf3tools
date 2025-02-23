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
            if (Model.SlotTable != null)
                CreateChild(new TableView("Slots", Model.SlotTable, ngc));
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
