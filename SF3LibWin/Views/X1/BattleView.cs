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
                CreateChild(new TableView("Map Pointers", Model.MapPointerTable, ngc));
            foreach (var battle in Model.MapPointerTable?.Select(x => x.BattleMap)?.Where(x => x != null)?.ToArray())
                CreateChild(new BattleMapView($"Map ({battle.MapLeader})", battle, ngc));
            if (Model.Unknown0x08Table != null)
                CreateChild(new TableView("Unknown 0x08", Model.Unknown0x08Table, ngc));
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
