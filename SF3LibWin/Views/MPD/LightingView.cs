using System.Windows.Forms;
using SF3.Models.Files.MPD;

namespace SF3.Win.Views.MPD {
    public class LightingView : TabView {
        public LightingView(string name, IMPD_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            if (Model.LightPaletteColorTable != null)
                CreateChild(new ColorTableView("Palette", Model.LightPaletteColorTable, Model.NameGetterContext));
            if (Model.LightPosition != null)
                CreateChild(new DataModelView("Direction", Model.LightPosition, ngc));

            return Control;
        }

        public IMPD_File Model { get; }
    }
}
