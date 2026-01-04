using System.Windows.Forms;
using SF3.Models.Files.X035;

namespace SF3.Win.Views.X035 {
    public class X035_View : TabView {
        public X035_View(string name, IX035_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            CreateChild(new TechnicalView("Technical Info", Model));

            return Control;
        }

        public IX035_File Model { get; }
    }
}
