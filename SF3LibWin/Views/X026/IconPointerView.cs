using System.Windows.Forms;
using SF3.Models.Files.X026;

namespace SF3.Win.Views.X026 {
    public class X026_View : TabView {
        public X026_View(string name, IX026_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            base.Create();

            var ngc = Model.NameGetterContext;
            CreateChild(new TableView("Item Icons",  Model.ItemIconTable, ngc));
            CreateChild(new TableView("Spell Icons", Model.SpellIconTable, ngc));

            return Control;
        }

        public IX026_File Model { get; }
    }
}
