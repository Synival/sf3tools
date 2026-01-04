using System.Windows.Forms;
using SF3.Models.Files.X018;

namespace SF3.Win.Views.X018 {
    public class X018_View : TabView {
        public X018_View(string name, IX018_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            CreateChild(new TechnicalView("Technical Info", Model));

            return Control;
        }

        public IX018_File Model { get; }
    }
}
