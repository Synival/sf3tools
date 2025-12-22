using System.Windows.Forms;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.Animation;

namespace SF3.Win.Views.MPD {
    public class TextureAnimationFrameChunkView : TabView {
        public TextureAnimationFrameChunkView(string name, TextureAnimationFrameChunk model) : base(name) {
            Model = model;

            var ngc = Model.NameGetterContext;
            TextureTableView = new TextureStructBaseTableView<UniqueTextureAnimationFrame>("Textures", Model.UniqueTextureAnimationFrameTable, ngc);
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            CreateChild(TextureTableView);

            // Return the top-level control.
            return Control;
        }

        public TextureAnimationFrameChunk Model { get; }
        public TextureStructBaseTableView<UniqueTextureAnimationFrame> TextureTableView { get; }
    }
}
