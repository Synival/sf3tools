using System.Windows.Forms;
using SF3.Models.Files.X021;
using SF3.Win.Views.X1;

namespace SF3.Win.Views.X021 {
    public class X021_View : TabView {
        public X021_View(string name, IX021_File model) : base(name) {
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

        public IX021_File Model { get; }
    }
}
