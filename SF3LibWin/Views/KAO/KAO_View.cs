using System.Windows.Forms;
using SF3.Models.Files.KAO;

namespace SF3.Win.Views.KAO {
    public class KAO_View : TabView {
        public KAO_View(string name, IKAO_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            // TODO: Show stuff!

            return Control;
        }

        public IKAO_File Model { get; }
    }
}
