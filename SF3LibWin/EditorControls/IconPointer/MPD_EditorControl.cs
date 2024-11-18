using System.Windows.Forms;
using SF3.Editors.IconPointer;

namespace SF3.Win.EditorControls.MPD {
    public class IconPointerEditorControl : EditorControlContainer {
        public IconPointerEditorControl(string name, IconPointerEditor editor) : base(name) {
            Editor = editor;
        }

        public override Control Create() {
            base.Create();

            var ngc = Editor.NameGetterContext;
            CreateChild(new TableEditorControl("Item Icons",  Editor.ItemIconTable,  ngc));
            CreateChild(new TableEditorControl("Spell Icons", Editor.SpellIconTable, ngc));

            return Control;
        }

        public IconPointerEditor Editor { get; }
    }
}
