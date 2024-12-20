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

            CreateChild(new HeadersView ("Headers",  Model));
            CreateChild(new PalettesView("Palettes", Model));
            CreateChild(new SurfaceView ("Surface",  Model));
            CreateChild(new TexturesView("Textures", Model));

            return Control;
        }

        public IMPD_File Model { get; }
    }
}
