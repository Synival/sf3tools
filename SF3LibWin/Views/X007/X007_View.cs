using System.Windows.Forms;
using SF3.Models.Files.X007;
using SF3.Models.Tables.X007;

namespace SF3.Win.Views.X007 {
    public class X007_View : TabView {
        public X007_View(string name, IX007_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;

            if (Model.CHPSectorSizesTables != null)
                CreateChild(new TableArrayView<CHPSectorSizesTable>("CHP Sector + Sizes", Model.CHPSectorSizesTables, ngc));

            CreateChild(new TechnicalView("Technical Info", Model));

            return Control;
        }

        public IX007_File Model { get; }
    }
}
