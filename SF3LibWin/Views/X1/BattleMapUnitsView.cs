using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Models.Tables.X1.Battle;

namespace SF3.Win.Views.X1 {
    public class BattleMapUnitsView : TabView {
        public BattleMapUnitsView(string name, UnitTable model, INameGetterContext nameGetterContext) : base(name) {
            Model = model;
            NameGetterContext = nameGetterContext;
        }

        public override Control Create() {
            base.Create();

            var ngc = NameGetterContext;
            CreateChild(new TableView("Page 1",              Model, ngc, displayGroups: ["Metadata", "Page1"]));
            CreateChild(new TableView("Page 2",              Model, ngc, displayGroups: ["Metadata", "Page2"]));
            CreateChild(new TableView("Page 3 (Conditions)", Model, ngc, displayGroups: ["Metadata", "Page3"]));
            CreateChild(new TableView("Page 4 (AI)",         Model, ngc, displayGroups: ["Metadata", "Page4"]));
            CreateChild(new TableView("Page 5 (Flags)",      Model, ngc, displayGroups: ["Metadata", "Page5"]));

            return Control;
        }

        public UnitTable Model { get; }
        public INameGetterContext NameGetterContext { get; }
    }
}
