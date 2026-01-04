using System.Windows.Forms;
using SF3.Models.Files.X016;

namespace SF3.Win.Views.X016 {
    public class X016_View : TabView {
        public X016_View(string name, IX016_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            CreateChild(new TechnicalView("Technical Info", Model));

            return Control;
        }

        public IX016_File Model { get; }
    }
}
