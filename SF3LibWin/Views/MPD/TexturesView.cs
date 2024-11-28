using System.Windows.Forms;
using SF3.Models.Files.MPD;

namespace SF3.Win.Views.MPD {
    public class TexturesView : TabView {
        public TexturesView(string name, IMPD_File editor) : base(name) {
            Editor = editor;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Editor.NameGetterContext;
            for (var i = 0; i < Editor.TextureChunks.Length; i++)
                _ = CreateChild(new TextureChunkView("Chunk " + (i + 6), Editor.TextureChunks[i]));
            _ = CreateChild(new TableView("Animations", Editor.TextureAnimations, ngc));
            _ = CreateChild(new TextureAnimFramesView("Anim. Frames", Editor, ngc));

            return Control;
        }

        public IMPD_File Editor { get; }
    }
}
