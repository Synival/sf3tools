using System.Windows.Forms;
using SF3.Models.Files.MPD;

namespace SF3.Win.Views.MPD {
    public class TexturesView : TabView {
        public TexturesView(string name, IMPD_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            for (var i = 0; i < Model.TextureChunks.Length; i++)
                CreateChild(new TextureChunkView("Chunk " + (i + 6), Model.TextureChunks[i]));
            CreateChild(new TextureAnimationsView("Animations", Model, ngc));
            CreateChild(new TextureAnimFramesView("Anim. Frames", Model, ngc));

            return Control;
        }

        public IMPD_File Model { get; }
    }
}
