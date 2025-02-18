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
            if (Model.LightPalette != null)
                CreateChild(new ColorTableView("Palette", Model.LightPalette, Model.NameGetterContext));
            if (Model.LightPosition != null)
                CreateChild(new ModelView("Direction", Model.LightPosition, ngc));

            return Control;
        }

        public IMPD_File Model { get; }
    }
}
