using System;
using System.Windows.Forms;
using SF3.Editors.MPD;
using SF3.Win.Controls;

namespace SF3.Win.Views.MPD {
    public class SurfaceView : TabView {
        public SurfaceView(string name, IMPD_Editor editor) : base(name) {
            Editor = editor;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            // TODO: this should be its own view
            var surfaceMapView = (Editor.TileSurfaceCharacterRows != null) ? new ControlView<SurfaceMapControl>("Viewer") : null;
            _ = CreateChild(surfaceMapView, autoFill: false);
            _surfaceMapControl = (SurfaceMapControl) (surfaceMapView?.Control);

            if (_surfaceMapControl != null) {
                _surfaceMapControlTab = (TabPage) _surfaceMapControl.Parent;
                TabControl.Selected += UpdateSurfaceMapControl;

                var textureData = Editor?.TileSurfaceCharacterRows?.Make2DTextureData();
                _surfaceMapControl.UpdateTextures(textureData, Editor.TextureChunks);
            }

            var ngc = Editor.NameGetterContext;

            _ = CreateChild(new TableView("Textures", Editor.TileSurfaceCharacterRows, ngc));
            _ = CreateChild(new TableView("Heightmap", Editor.TileSurfaceHeightmapRows, ngc));
            _ = CreateChild(new TableView("Height + Terrain Type", Editor.TileHeightTerrainRows, ngc));
            _ = CreateChild(new TableView("Object Locations", Editor.TileItemRows, ngc));

            return Control;
        }

        private TabPage _surfaceMapControlTab = null;
        private SurfaceMapControl _surfaceMapControl = null;

        void UpdateSurfaceMapControl(object sender, EventArgs eventArgs) {
            if (TabControl.SelectedTab != _surfaceMapControlTab)
                return;

            var textureData = Editor?.TileSurfaceCharacterRows?.Make2DTextureData();
            _surfaceMapControl.UpdateTextures(textureData, Editor.TextureChunks);
        }

        public override void Destroy() {
            if (_surfaceMapControl != null) {
                TabControl.Selected -= UpdateSurfaceMapControl;
                _surfaceMapControl = null;
                _surfaceMapControlTab = null;
            }

            base.Destroy();
        }

        public IMPD_Editor Editor { get; }
    }
}
