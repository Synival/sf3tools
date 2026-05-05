using System.Linq;
using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Models.Structs.X1.Battle;

namespace SF3.Win.Views.X1 {
    public class BattleTechnicalView : TabView {
        public BattleTechnicalView(string name, BattleHeader model, INameGetterContext nameGetterContext) : base(name) {
            Model = model;
            NameGetterContext = nameGetterContext;
        }

        public override Control Create() {
            var control = base.Create();
            if (control == null)
                return control;

            var ngc = NameGetterContext;
            if (Model.MapPointerTable != null)
                CreateChild(new TableView("Map Pointers", Model.MapPointerTable, ngc));

            if (Model.MapMoveCoordFlagsPointerTable != null)
                CreateChild(new TableView("Map Move Coord Flag Pointers", Model.MapMoveCoordFlagsPointerTable, ngc));
            if (Model.MapMoveCoordFlagsPointerTable != null)
                foreach (var warps in Model.MapMoveCoordFlagsPointerTable.Select(x => x.MapMoveCoordFlagsTable).Where(x => x != null).ToArray())
                    CreateChild(new TableView($"Map Move Coord Flags ({warps.MapLeader})", warps, ngc));

            return control;
        }

        public BattleHeader Model { get; }
        public INameGetterContext NameGetterContext { get; }
    }
}
