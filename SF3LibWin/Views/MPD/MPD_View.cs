using System.Windows.Forms;
using SF3.Models.Files.MPD;

namespace SF3.Win.Views.MPD {
    public class MPD_View : TabView {
        public MPD_View(string name, IMPD_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            _ = CreateChild(new HeadersView("Headers", Model));
            _ = CreateChild(new PalettesView("Palettes", Model));
            _ = CreateChild(new SurfaceView("Surface", Model));
            _ = CreateChild(new TexturesView("Textures", Model));

            return Control;
        }

        public IMPD_File Model { get; }
    }
}
