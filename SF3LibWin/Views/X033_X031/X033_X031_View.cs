using System.Windows.Forms;
using SF3.Models.Files.X033_X031;

namespace SF3.Win.Views.X033_X031 {
    public class X033_X031_View : TabView {
        public X033_X031_View(string name, IX033_X031_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            // TODO: tables!

            return Control;
        }

        public IX033_X031_File Model { get; }
    }
}
