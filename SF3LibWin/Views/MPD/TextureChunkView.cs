using System.Windows.Forms;
using SF3.Models.Files.MPD.Objects;

namespace SF3.Win.Views.MPD {
    public class TextureChunkView : TabView {
        public TextureChunkView(string name, TextureCollection model) : base(name) {
            Model = model;

            var ngc = Model.NameGetterContext;
            HeaderView   = new TableView("Header", Model.TextureHeaderTable, ngc);
            TexturesView = new TextureTableView("Textures", Model.TextureTable, ngc);
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            CreateChild(HeaderView);
            CreateChild(TexturesView);

            // Return the top-level control.
            return Control;
        }

        public TextureCollection Model { get; }
        public TableView HeaderView { get; }
        public TextureTableView TexturesView { get; }
    }
}
