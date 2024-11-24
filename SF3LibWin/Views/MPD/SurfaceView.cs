using System;
using System.Windows.Forms;
using SF3.Editors.MPD;

namespace SF3.Win.Views.MPD {
    public class SurfaceView : TabView {
        public SurfaceView(string name, IMPD_Editor editor) : base(name) {
            Editor = editor;
            SurfaceMapView = new SurfaceMapView("Viewer", Editor);
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            _ = CreateChild(SurfaceMapView, autoFill: false);
            if (SurfaceMapView.SurfaceMapControl != null) {
                _surfaceMapControlTab = (TabPage) SurfaceMapView.SurfaceMapControl.Parent;
                TabControl.Selected += UpdateSurfaceMapControl;
                SurfaceMapView.UpdateMap();
            }

            var ngc = Editor.NameGetterContext;

            _ = CreateChild(new TableView("Textures", Editor.TileSurfaceCharacterRows, ngc));
            _ = CreateChild(new TableView("Heightmap", Editor.TileSurfaceHeightmapRows, ngc));
            _ = CreateChild(new TableView("Height + Terrain Type", Editor.TileHeightTerrainRows, ngc));
            _ = CreateChild(new TableView("Object Locations", Editor.TileItemRows, ngc));

            return Control;
        }

        private TabPage _surfaceMapControlTab = null;

        void UpdateSurfaceMapControl(object sender, EventArgs eventArgs) {
            if (TabControl.SelectedTab == _surfaceMapControlTab)
                SurfaceMapView.UpdateMap();
        }

        public override void Destroy() {
            if (_surfaceMapControlTab != null) {
                TabControl.Selected -= UpdateSurfaceMapControl;
                _surfaceMapControlTab = null;
            }

            base.Destroy();
        }

        public IMPD_Editor Editor { get; }
        public SurfaceMapView SurfaceMapView { get; }
    }
}
