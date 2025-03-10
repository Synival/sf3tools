using System.Windows.Forms;
using SF3.Models.Files.X014;

namespace SF3.Win.Views.X014 {
    public class X014_View : TabView {
        public X014_View(string name, IX014_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            // TODO: make tables!

            return Control;
        }

        public IX014_File Model { get; }
    }
}
