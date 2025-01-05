using System.Windows.Forms;
using SF3.Models.Files.MPD;

namespace SF3.Win.Views.MPD {
    public class PalettesView : TabView {
        public PalettesView(string name, IMPD_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;
            CreateChild(new ColorTableView("Light Palette", Model.LightPalette, Model.NameGetterContext));

            for (var i = 0; i < Model.TexturePalettes.Length; i++)
                if (Model.TexturePalettes[i] != null)
                    CreateChild(new ColorTableView("Texture Palette " + (i + 1).ToString(), Model.TexturePalettes[i], Model.NameGetterContext));

            return Control;
        }

        public IMPD_File Model { get; }
    }
}
