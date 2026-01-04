using System.Windows.Forms;
using SF3.Models.Files.X017;

namespace SF3.Win.Views.X017 {
    public class X017_View : TabView {
        public X017_View(string name, IX017_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            CreateChild(new TechnicalView("Technical Info", Model));

            return Control;
        }

        public IX017_File Model { get; }
    }
}
