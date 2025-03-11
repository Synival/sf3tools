using System.Windows.Forms;
using SF3.Models.Files.X005;

namespace SF3.Win.Views.X005 {
    public class X005_View : TabView {
        public X005_View(string name, IX005_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            // TODO: tables

            return Control;
        }

        public IX005_File Model { get; }
    }
}
