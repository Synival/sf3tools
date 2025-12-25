using System.Windows.Forms;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.Animation;

namespace SF3.Win.Views.MPD {
    public class AnimationFrameChunkView : TabView {
        public AnimationFrameChunkView(string name, AnimationFrameChunk model) : base(name) {
            Model = model;

            var ngc = Model.NameGetterContext;
            TextureTableView = new TextureStructBaseTableView<UniqueAnimationFrame>("Textures", Model.UniqueAnimationFrameTable, ngc);
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            CreateChild(TextureTableView);

            // Return the top-level control.
            return Control;
        }

        public AnimationFrameChunk Model { get; }
        public TextureStructBaseTableView<UniqueAnimationFrame> TextureTableView { get; }
    }
}
