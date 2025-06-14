using System.Windows.Forms;
using SF3.Models.Files.X027;
using SF3.Models.Tables.Shared;
using SF3.Win.Views.X1;

namespace SF3.Win.Views.X027 {
    public class X027_View : TabView {
        public X027_View(string name, IX027_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            if (Model.BlacksmithTables?.Length > 0)
                CreateChild(new TableArrayView<BlacksmithTable>("Blacksmith", Model.BlacksmithTables, ngc));

            CreateChild(new TechnicalView("Technical Info", Model));

            return Control;
        }

        public IX027_File Model { get; }
    }
}
