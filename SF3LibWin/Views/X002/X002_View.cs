using System.Windows.Forms;
using SF3.Models.Files.X002;

namespace SF3.Win.Views.X002 {
    public class X002_View : TabView {
        public X002_View(string name, IX002_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            // TODO: tables!

            return Control;
        }

        public IX002_File Model { get; }
    }
}
