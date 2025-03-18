using System.Windows.Forms;
using SF3.Models.Files.X013;

namespace SF3.Win.Views.X013 {
    public class X013_View : TabView {
        public X013_View(string name, IX013_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            // TODO: tables!

            return Control;
        }

        public IX013_File Model { get; }
    }
}
