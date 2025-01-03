using System.Windows.Forms;
using SF3.Models.Files.MPD;

namespace SF3.Win.Views.MPD {
    public class SurfaceView : TabView {
        public SurfaceView(string name, IMPD_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;

            CreateChild(new TableView("Textures",              Model.SurfaceModel?.CharacterRowTable, ngc));
            CreateChild(new TableView("Heightmap",             Model.TileSurfaceHeightmapRows, ngc));
            CreateChild(new TableView("Height + Terrain Type", Model.TileHeightTerrainRows, ngc));
            CreateChild(new TableView("Object Locations",      Model.TileItemRows, ngc));

            return Control;
        }

        public IMPD_File Model { get; }
    }
}
