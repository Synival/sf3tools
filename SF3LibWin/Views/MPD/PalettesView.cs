using System.Windows.Forms;
using SF3.Models.Files.MPD;

namespace SF3.Win.Views.MPD {
    public class PalettesView : TabView {
        public PalettesView(string name, IMPD_Editor editor) : base(name) {
            Editor = editor;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;
            for (var i = 0; i < Editor.Palettes.Length; i++)
                _ = CreateChild(new TableView("Palette" + (i + 1).ToString(), Editor.Palettes[i], Editor.NameGetterContext));
            return Control;
        }

        public IMPD_Editor Editor { get; }
    }
}
