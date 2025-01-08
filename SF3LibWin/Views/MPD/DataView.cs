using System.Windows.Forms;
using SF3.Models.Files.MPD;

namespace SF3.Win.Views.MPD {
    public class DataView : TabView {
        public DataView(string name, IMPD_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            CreateChild(new MainTablesView("Main Tables", Model));
            CreateChild(new ChunksView("Chunks", Model));

            return Control;
        }

        public IMPD_File Model { get; }
    }
}