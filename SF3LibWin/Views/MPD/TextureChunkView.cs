using System.Windows.Forms;
using SF3.Models.Files.MPD;

namespace SF3.Win.Views.MPD {
    public class TextureChunkView : TabView {
        public TextureChunkView(string name, TextureChunk model) : base(name) {
            Model = model;

            var ngc = Model.NameGetterContext;
            HeaderView       = new TableView("Header", Model.TextureHeaderTable, ngc);
            TextureTableView = new TextureTableView("Textures", Model.TextureTable, ngc);
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            CreateChild(HeaderView);
            CreateChild(TextureTableView);

            // Return the top-level control.
            return Control;
        }

        public TextureChunk Model { get; }
        public TableView HeaderView { get; }
        public TextureTableView TextureTableView { get; }
    }
}
