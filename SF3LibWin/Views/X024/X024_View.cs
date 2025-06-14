using System.Windows.Forms;
using SF3.Models.Files.X024;
using SF3.Win.Views.X1;

namespace SF3.Win.Views.X024 {
    public class X024_View : TabView {
        public X024_View(string name, IX024_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            if (Model.BlacksmithTable != null)
                CreateChild(new TableView("Blacksmith", Model.BlacksmithTable, ngc));

            CreateChild(new TechnicalView("Technical Info", Model));

            return Control;
        }

        public IX024_File Model { get; }
    }
}
