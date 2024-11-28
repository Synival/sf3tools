using System.Windows.Forms;
using SF3.Models.Files.IconPointer;

namespace SF3.Win.Views.MPD {
    public class IconPointerView : TabView {
        public IconPointerView(string name, IconPointerFile editor) : base(name) {
            Editor = editor;
        }

        public override Control Create() {
            base.Create();

            var ngc = Editor.NameGetterContext;
            CreateChild(new TableView("Item Icons",  Editor.ItemIconTable,  ngc));
            CreateChild(new TableView("Spell Icons", Editor.SpellIconTable, ngc));

            return Control;
        }

        public IconPointerFile Editor { get; }
    }
}
