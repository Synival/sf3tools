using System.Linq;
using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Models.Structs.X1.Battle;

namespace SF3.Win.Views.X1 {
    public class BattleView : TabView {
        public BattleView(string name, BattleHeader model, INameGetterContext nameGetterContext) : base(name) {
            Model = model;
            NameGetterContext = nameGetterContext;
        }

        public override Control Create() {
            var control = base.Create();
            if (control == null)
                return null;

            var ngc = NameGetterContext;

            if (Model.MapPointerTable != null)
                foreach (var battle in Model.MapPointerTable.Select(x => x.BattleMap).Where(x => x != null).ToArray())
                    CreateChild(new BattleMapView($"Map ({battle.MapLeader})", battle, ngc));
            if (Model.MapPointerTable != null)
                CreateChild(new TableView("Map Pointers", Model.MapPointerTable, ngc));

            if (Model.MapMoveCoordFlagsPointerTable != null)
                CreateChild(new TableView("Map Move Coord Flag Pointers", Model.MapMoveCoordFlagsPointerTable, ngc));
            if (Model.MapMoveCoordFlagsPointerTable != null)
                foreach (var warps in Model.MapMoveCoordFlagsPointerTable.Select(x => x.MapMoveCoordFlagsTable).Where(x => x != null).ToArray())
                    CreateChild(new TableView($"Map Move Coord Flags ({warps.MapLeader})", warps, ngc));

            if (Model.TeamsCantAttackTable != null)
                CreateChild(new TableView("Teams (Can't Attack)", Model.TeamsCantAttackTable, ngc));
            if (Model.TeamsCanSupportTable != null)
                CreateChild(new TableView("Teams (Can Support)", Model.TeamsCanSupportTable, ngc));

            return control;
        }

        public BattleHeader Model { get; }
        public INameGetterContext NameGetterContext { get; }
    }
}
