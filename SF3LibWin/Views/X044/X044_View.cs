using System.Windows.Forms;
using SF3.Models.Files.X044;
using SF3.Win.Views.X1;

namespace SF3.Win.Views.X044 {
    public class X044_View : TabView {
        public X044_View(string name, IX044_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            if (Model.MonsterTable != null)
                CreateChild(new MonstersView("Monsters", Model.MonsterTable, ngc));

            CreateChild(new TechnicalView("Technical Info", Model));

            return Control;
        }

        public IX044_File Model { get; }
    }
}
