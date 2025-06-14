using System.Windows.Forms;
using SF3.Models.Files.X019;
using SF3.Win.Views.X1;

namespace SF3.Win.Views.X019 {
    public class X019_View : TabView {
        public X019_View(string name, IX019_File model) : base(name) {
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

        public IX019_File Model { get; }
    }
}
