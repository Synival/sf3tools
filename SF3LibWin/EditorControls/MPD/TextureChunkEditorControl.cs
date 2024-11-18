using System.Windows.Forms;
using SF3.Editors.MPD;

namespace SF3.Win.EditorControls.MPD {
    public class TextureChunkEditorControl : EditorControlContainer {
        public TextureChunkEditorControl(string name, TextureChunkEditor editor) : base(name) {
            Editor = editor;
        }

        public override Control Create() {
            base.Create();

            var ngc = Editor.NameGetterContext;

            _ = CreateChild(new TableEditorControl("Header", Editor.HeaderTable, ngc));
            _ = CreateChild(new TableEditorControl("Textures", Editor.TextureTable, ngc));

            // TODO: texture viewer!!

            return Control;
        }

        public TextureChunkEditor Editor { get; }
    }
}
