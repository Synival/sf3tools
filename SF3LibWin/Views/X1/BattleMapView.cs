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
            if (Model.UnitTable != null)
                CreateChild(new BattleMapUnitsView("Units", Model.UnitTable, ngc));
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
