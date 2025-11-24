using System.Windows.Forms;
using SF3.Models.Files.MPD;

namespace SF3.Win.Views.MPD {
    public class SurfaceModelChunkView : TabView {
        public SurfaceModelChunkView(string name, SurfaceModelChunk model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            CreateChild(new TableView("Texture Flags + IDs", Model.TileTextureRowTable, ngc));

            return Control;
        }

        public SurfaceModelChunk Model { get; }
    }
}
