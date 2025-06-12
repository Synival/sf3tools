using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Models.Tables.Shared;

namespace SF3.Win.Views {
    public class StatTableView : TabView {
        public StatTableView(string name, StatsTable model, INameGetterContext ngc) : base(name) {
            Model = model;
            NameGetterContext = ngc;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = NameGetterContext;
            CreateChild(new TableView("Characters and Classes", Model, ngc, displayGroups: ["Metadata", "CharAndClass"]));
            CreateChild(new TableView("Stats (1/2) (Growth)",   Model, ngc, displayGroups: ["Metadata", "Stats"]));
            CreateChild(new TableView("Stats (2/2)",            Model, ngc, displayGroups: ["Metadata", "Stats2"]));
            CreateChild(new TableView("Magic Res",              Model, ngc, displayGroups: ["Metadata", "MagicRes"]));
            CreateChild(new TableView("Spells",                 Model, ngc, displayGroups: ["Metadata", "Spells"]));
            CreateChild(new TableView("Weapons / Accessories",  Model, ngc, displayGroups: ["Metadata", "Equipment"]));
            CreateChild(new TableView("Specials",               Model, ngc, displayGroups: ["Metadata", "Specials"]));
            CreateChild(new TableView("Growth Curve Calc",      Model, ngc, displayGroups: ["Metadata", "CurveCalc"]));

            return Control;
        }

        public StatsTable Model { get; }
        public INameGetterContext NameGetterContext { get; }
    }
}
