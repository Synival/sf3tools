using System.Linq;
using System.Windows.Forms;
using SF3.Models.Files.X1;

namespace SF3.Win.Views.MPD {
    public class X1_View : TabView {
        public X1_View(string name, IX1_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            base.Create();

            var ngc = Model.NameGetterContext;
            if (Model.InteractableTable != null)
                CreateChild(new TableView("Interactables", Model.InteractableTable, ngc));
            if (Model.BattlePointersTable != null)
                CreateChild(new TableView("Battle Pointers", Model.BattlePointersTable, ngc));
            if (Model.NpcTable != null)
                CreateChild(new TableView("TownNpcs", Model.NpcTable, ngc));
            if (Model.EnterTable != null)
                CreateChild(new TableView("Non-Battle Enter", Model.EnterTable, ngc));
            if (Model.WarpTable != null)
                CreateChild(new TableView("WarpTable (Scn2+)", Model.WarpTable, ngc));
            if (Model.ArrowTable != null)
                CreateChild(new TableView("Arrows (Scn2+)", Model.ArrowTable, ngc));
            if (Model.TileMovementTable != null)
                CreateChild(new TableView("TileData (Scn2+)", Model.TileMovementTable, ngc));

            if (Model.Battles != null) {
                foreach (var battleKv in Model.Battles.Where(x => x.Value != null))
                    CreateChild(new BattleView($"Battle ({battleKv.Key})", battleKv.Value));
            }

            return Control;
        }

        public IX1_File Model { get; }
    }
}
