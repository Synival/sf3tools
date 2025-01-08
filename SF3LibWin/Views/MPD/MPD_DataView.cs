using System.Windows.Forms;
using SF3.Models.Files.MPD;

namespace SF3.Win.Views.MPD {
    public class MPD_DataView : TabView {
        public MPD_DataView(string name, IMPD_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            CreateChild(new MPD_MainTablesView("Main Tables", Model));
            CreateChild(new MPD_ChunksView("Chunks", Model));

            return Control;
        }

        public IMPD_File Model { get; }
    }
}