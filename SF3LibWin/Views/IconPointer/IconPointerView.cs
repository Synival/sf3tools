using System.Windows.Forms;
using SF3.Models.Files.IconPointer;

namespace SF3.Win.Views.IconPointer {
    public class IconPointerView : TabView {
        public IconPointerView(string name, IIconPointerFile model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            base.Create();

            var ngc = Model.NameGetterContext;
            CreateChild(new TableView("Item Icons",  Model.ItemIconTable, ngc));
            CreateChild(new TableView("Spell Icons", Model.SpellIconTable, ngc));

            return Control;
        }

        public IIconPointerFile Model { get; }
    }
}
