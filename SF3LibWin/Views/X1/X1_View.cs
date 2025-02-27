using System.Linq;
using System.Windows.Forms;
using SF3.Models.Files.X1;
using SF3.Models.Tables.X1;

namespace SF3.Win.Views.X1 {
    public class X1_View : TabView {
        public X1_View(string name, IX1_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            if (Model.InteractableTable != null)
                CreateChild(new TableView("Interactables", Model.InteractableTable, ngc));
            if (Model.BattlePointersTable != null)
                CreateChild(new TableView("Battle Pointers", Model.BattlePointersTable, ngc));
            if (Model.NpcTable != null)
                CreateChild(new TableView("Town NPCs", Model.NpcTable, ngc));
            if (Model.EnterTable != null)
                CreateChild(new TableView("Non-Battle Enter", Model.EnterTable, ngc));
            if (Model.WarpTable != null)
                CreateChild(new TableView("Warp Table (Scn2+)", Model.WarpTable, ngc));
            if (Model.ArrowTable != null)
                CreateChild(new TableView("Arrows (Scn2+)", Model.ArrowTable, ngc));
            if (Model.TileMovementTable != null)
                CreateChild(new TableView("Tile Data (Scn2+)", Model.TileMovementTable, ngc));
            if (Model.CharacterTargetPriorityTables != null)
                CreateChild(new TableArrayView<CharacterTargetPriorityTable>("Character Target Priorities", Model.CharacterTargetPriorityTables, ngc));
            if (Model.CharacterTargetUnknownTables != null)
                CreateChild(new TableArrayView<CharacterTargetUnknownTable>("Unknown 16 Tables", Model.CharacterTargetUnknownTables, ngc));

            if (Model.Battles != null) {
                foreach (var battleKv in Model.Battles.Where(x => x.Value != null))
                    CreateChild(new BattleView($"Battle ({battleKv.Key})", battleKv.Value));
            }

            return Control;
        }

        public IX1_File Model { get; }
    }
}
