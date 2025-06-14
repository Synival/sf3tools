using System.Windows.Forms;
using SF3.Models.Files.X011;
using SF3.Win.Views.X1;

namespace SF3.Win.Views.X011 {
    public class X011_View : TabView {
        public X011_View(string name, IX011_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            base.Create();

            var ngc = Model.NameGetterContext;
            CreateChild(new TableView("Item Icons",  Model.ItemIconTable, ngc));
            CreateChild(new TableView("Spell Icons", Model.SpellIconTable, ngc));

            CreateChild(new TechnicalView("Technical Info", Model));

            return Control;
        }

        public IX011_File Model { get; }
    }
}
