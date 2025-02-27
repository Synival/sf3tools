using System.Windows.Forms;
using SF3.Models.Files.X012;

namespace SF3.Win.Views.X012 {
    public class X012_View : TabView {
        public X012_View(string name, IX012_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            base.Create();

            return Control;
        }

        public IX012_File Model { get; }
    }
}
