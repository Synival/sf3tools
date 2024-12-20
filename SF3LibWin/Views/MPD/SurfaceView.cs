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

            SurfaceMap2DView = new SurfaceMap2DView("Tiles (2D)", Model);
            _ = CreateCustomChild(SurfaceMap2DView, autoFill: false, (name) => new NoScrollTabPage(name) { AutoScroll = true });
            _surfaceMap2DControlTab = (TabPage) SurfaceMap2DView.SurfaceMapControl.Parent;
            SurfaceMap2DView.UpdateMap();

            SurfaceMap3DView = new SurfaceMap3DView("Tiles (3D)", Model);
            _ = CreateChild(SurfaceMap3DView, autoFill: true);
            _surfaceMap3DControlTab = (TabPage) SurfaceMap3DView.SurfaceMapControl.Parent;
            SurfaceMap3DView.UpdateMap();

            TabControl.Selected += UpdateSurfaceMapControls;

            var ngc = Model.NameGetterContext;

            CreateChild(new TableView("Textures", Model.TileSurfaceCharacterRows, ngc));
            CreateChild(new TableView("Heightmap", Model.TileSurfaceHeightmapRows, ngc));
            CreateChild(new TableView("Height + Terrain Type", Model.TileHeightTerrainRows, ngc));
            CreateChild(new TableView("Object Locations", Model.TileItemRows, ngc));

            return Control;
        }

        private TabPage _surfaceMap2DControlTab = null;
        private TabPage _surfaceMap3DControlTab = null;

        void UpdateSurfaceMapControls(object sender, EventArgs eventArgs) {
            if (TabControl.SelectedTab == _surfaceMap2DControlTab)
                SurfaceMap2DView?.UpdateMap();
            if (TabControl.SelectedTab == _surfaceMap3DControlTab)
                SurfaceMap3DView?.UpdateMap();
        }

        public override void Destroy() {
            if (TabControl != null)
                TabControl.Selected -= UpdateSurfaceMapControls;

            if (SurfaceMap2DView != null) {
                _surfaceMap2DControlTab = null;
                SurfaceMap2DView = null;
            }

            if (SurfaceMap3DView != null) {
                _surfaceMap3DControlTab = null;
                SurfaceMap3DView = null;
            }

            base.Destroy();
        }

        public IMPD_File Model { get; }
        public SurfaceMap2DView SurfaceMap2DView { get; private set; }
        public SurfaceMap3DView SurfaceMap3DView { get; private set; }
    }
}
