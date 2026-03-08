using System.Windows.Forms;
using SF3.Models.Files.X032;

namespace SF3.Win.Views.X032 {
    public class X032_View : TabView {
        public X032_View(string name, IX032_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            CreateChild(new TableView("Spell Icons", Model.SpellIconTable, ngc));

            CreateChild(new TechnicalView("Technical Info", Model));

            return Control;
        }

        public IX032_File Model { get; }
    }
}
