using System;
using System.Windows.Forms;
using SF3.FileModels.MPD;
using SF3.Win.Controls;

namespace SF3.Win.Views.MPD {
    public class MPD_View : TabView {
        public MPD_View(string name, IMPD_Editor editor) : base(name) {
            Editor = editor;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            _ = CreateChild(new HeadersView("Headers", Editor));
            _ = CreateChild(new PalettesView("Palettes", Editor));
            _ = CreateChild(new SurfaceView("Surface", Editor));
            _ = CreateChild(new TexturesView("Textures", Editor));

            return Control;
        }

        public IMPD_Editor Editor { get; }
    }
}
