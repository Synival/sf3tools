using System;
using System.Drawing;
using System.Windows.Forms;
using SF3.Models.Files.MPD;

namespace SF3.Win.Views.MPD {
    public class SurfaceView : TabView {
        public SurfaceView(string name, IMPD_File model) : base(name) {
            Model = model;
        }

        public class NoScrollTabPage : TabPage {
            public NoScrollTabPage(string text) : base(text) { }
            protected override Point ScrollToControl(Control control) => DisplayRectangle.Location;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            SurfaceMapView = new SurfaceMapView("Tiles", Model);
            _ = CreateCustomChild(SurfaceMapView, autoFill: false, (name) => new NoScrollTabPage(name) { AutoScroll = true });
            _surfaceMapControlTab = (TabPage) SurfaceMapView.SurfaceMapControl.Parent;
            TabControl.Selected += UpdateSurfaceMapControl;
            SurfaceMapView.UpdateMap();

            var ngc = Model.NameGetterContext;

            _ = CreateChild(new TableView("Textures", Model.TileSurfaceCharacterRows, ngc));
            _ = CreateChild(new TableView("Heightmap", Model.TileSurfaceHeightmapRows, ngc));
            _ = CreateChild(new TableView("Height + Terrain Type", Model.TileHeightTerrainRows, ngc));
            _ = CreateChild(new TableView("Object Locations", Model.TileItemRows, ngc));

            return Control;
        }

        private TabPage _surfaceMapControlTab = null;

        void UpdateSurfaceMapControl(object sender, EventArgs eventArgs) {
            if (TabControl.SelectedTab == _surfaceMapControlTab)
                SurfaceMapView?.UpdateMap();
        }

        public override void Destroy() {
            if (SurfaceMapView != null) {
                TabControl.Selected -= UpdateSurfaceMapControl;
                _surfaceMapControlTab = null;
                SurfaceMapView = null;
            }
            base.Destroy();
        }

        public IMPD_File Model { get; }
        public SurfaceMapView SurfaceMapView { get; private set; }
    }
}
