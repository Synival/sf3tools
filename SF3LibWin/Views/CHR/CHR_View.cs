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
                foreach (var sprite in Model.SpriteTable)
                    CreateChild(new SpriteView(sprite.SpriteName, sprite));

            return Control;
        }

        public ICHR_File Model { get; }
    }
}
