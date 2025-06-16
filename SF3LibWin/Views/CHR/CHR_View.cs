using System.Windows.Forms;
using SF3.Models.Files.CHR;

namespace SF3.Win.Views.CHR {
    public class CHR_View : TabView {
        public CHR_View(string name, ICHR_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            if (Model.SpriteTable != null)
                CreateChild(new TableView("Sprites", Model.SpriteTable, ngc));
            if (Model.SpriteOffset1SetTable != null)
                CreateChild(new TableView("Sprite Offset 1 Tables", Model.SpriteOffset1SetTable, ngc));
            if (Model.SpriteOffset2SetTable != null)
                CreateChild(new TableView("Sprite Offset 2 Tables", Model.SpriteOffset2SetTable, ngc));

            return Control;
        }

        public ICHR_File Model { get; }
    }
}
