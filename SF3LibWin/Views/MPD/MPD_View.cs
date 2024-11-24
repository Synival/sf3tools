using System;
using System.Windows.Forms;
using BrightIdeasSoftware;
using SF3.Editors.MPD;
using SF3.Models.MPD.TextureAnimation;
using SF3.Win.Controls;
using SF3.Win.Extensions;

namespace SF3.Win.Views.MPD {
    public class MPD_View : TabView {
        public MPD_View(string name, IMPD_Editor editor) : base(name) {
            Editor = editor;
        }

        public override Control Create() {
            base.Create();

            var ngc = Editor.NameGetterContext;

            var headerContainer = new TabView("Headers");
            _ = CreateChild(headerContainer);
            _ = headerContainer.CreateChild(new TableView("Header", Editor.Header, ngc));
            _ = headerContainer.CreateChild(new TableView("Chunk Header", Editor.ChunkHeader, ngc));
            _ = headerContainer.CreateChild(new TableView("Offset 1 Table", Editor.Offset1Table, ngc));
            _ = headerContainer.CreateChild(new TableView("Offset 2 Table", Editor.Offset2Table, ngc));
            _ = headerContainer.CreateChild(new TableView("Offset 3 Table", Editor.Offset3Table, ngc));
            _ = headerContainer.CreateChild(new TableView("Offset 4 Table", Editor.Offset4Table, ngc));

            var paletteContainer = new TabView("Palettes");
            _ = CreateChild(paletteContainer);
            for (var i = 0; i < Editor.Palettes.Length; i++)
                _ = paletteContainer.CreateChild(new TableView("Palette" + (i + 1).ToString(), Editor.Palettes[i], ngc));

            var surfaceContainer = new TabView("Surface");
            var surfaceTabControl = (TabControl) CreateChild(surfaceContainer);
            var surfaceMapView = (Editor.TileSurfaceCharacterRows != null) ? new ControlView<SurfaceMapControl>("Viewer") : null;
            _ = surfaceContainer.CreateChild(surfaceMapView, autoFill: false);
            var surfaceMapControl = (SurfaceMapControl) (surfaceMapView?.Control);

            if (surfaceMapControl != null) {
                var surfaceMapControlTab = (TabPage) surfaceMapControl.Parent;
                var textureData = Editor?.TileSurfaceCharacterRows?.Make2DTextureData();

                void updateSurfaceMapControl(object sender, EventArgs eventArgs) {
                    if (surfaceTabControl.SelectedTab == surfaceMapControlTab && textureData != null)
                        surfaceMapControl.UpdateTextures(textureData, Editor.TextureChunks);
                };

                surfaceTabControl.Selected += updateSurfaceMapControl;
                surfaceMapControl.UpdateTextures(textureData, Editor.TextureChunks);
            }

            _ = surfaceContainer.CreateChild(new TableView("Textures", Editor.TileSurfaceCharacterRows, ngc));
            _ = surfaceContainer.CreateChild(new TableView("Heightmap", Editor.TileSurfaceHeightmapRows, ngc));
            _ = surfaceContainer.CreateChild(new TableView("Height + Terrain Type", Editor.TileHeightTerrainRows, ngc));
            _ = surfaceContainer.CreateChild(new TableView("Object Locations", Editor.TileItemRows, ngc));

            var textureContainer = new TabView("Textures");
            _ = CreateChild(textureContainer);
            for (var i = 0; i < Editor.TextureChunks.Length; i++)
                _ = textureContainer.CreateChild(new TextureChunkView("Chunk " + (i + 6), Editor.TextureChunks[i]));

            _ = textureContainer.CreateChild(new TableView("Animations", Editor.TextureAnimations, ngc));
            _ = textureContainer.CreateChild(new TextureAnimFramesView("Anim. Frames", Editor, ngc));

            return Control;
        }

        public IMPD_Editor Editor { get; }
    }
}
