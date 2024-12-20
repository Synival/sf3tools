using System;
using System.Drawing;
using System.Windows.Forms;
using SF3.Models.Files.MPD;

namespace SF3.Win.Views.MPD {
    public class SurfaceView : TabView {
        public SurfaceView(string name, IMPD_File model) : base(name) {
            Model = model;
            SurfaceMap3DView = new SurfaceMap3DView("Tiles (3D)", Model);
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            CreateChild(SurfaceMap3DView, (c) => {
                _surfaceMap3DControlTab = (TabPage) c.Parent;
                SurfaceMap3DView.UpdateMap();
            }, autoFill: true);

            TabControl.Selected += UpdateSurfaceMapControls;

            var ngc = Model.NameGetterContext;

            CreateChild(new TableView("Textures", Model.TileSurfaceCharacterRows, ngc));
            CreateChild(new TableView("Heightmap", Model.TileSurfaceHeightmapRows, ngc));
            CreateChild(new TableView("Height + Terrain Type", Model.TileHeightTerrainRows, ngc));
            CreateChild(new TableView("Object Locations", Model.TileItemRows, ngc));

            return Control;
        }

        private TabPage _surfaceMap3DControlTab = null;

        void UpdateSurfaceMapControls(object sender, EventArgs eventArgs) {
            if (TabControl.SelectedTab == _surfaceMap3DControlTab)
                SurfaceMap3DView?.UpdateMap();
        }

        public override void Destroy() {
            if (!IsCreated)
                return;

            if (TabControl != null)
                TabControl.Selected -= UpdateSurfaceMapControls;

            SurfaceMap3DView.Destroy();
            _surfaceMap3DControlTab = null;

            base.Destroy();
        }

        public IMPD_File Model { get; }
        public SurfaceMap3DView SurfaceMap3DView { get; }
    }
}
