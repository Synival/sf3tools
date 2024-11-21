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
            var surfaceMapControl = (Editor.TileSurfaceCharacterRows != null) ? new SurfaceMapControl() : null;
            _ = surfaceContainer.CreateChild("Viewer", surfaceMapControl, autoFill: false);

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
            var framesControl = (ObjectListView) textureContainer.CreateChild(new TableView("Anim. Frames", Editor.TextureAnimFrames, ngc));

            // TODO: extract everything here to do with showing this editor into its own utility function!
            var framesTabPage = framesControl?.Parent;

            // Add a texture viewer on the right side of the 'Textures' tab.
            if (framesTabPage != null) {
                var textureViewer = new TextureControl();
                textureViewer.Dock = DockStyle.Right;
                framesTabPage.Controls.Add(textureViewer);

                void OnTextureChanged(object sender, EventArgs e) {
                    var item = (OLVListItem) framesControl.SelectedItem;
                    var frame = (FrameModel) item?.RowObject;
                    textureViewer.TextureImage = (frame == null || Editor.TextureAnimFrameEditors[frame.ID] == null)
                        ? (System.Drawing.Image) null
                        : frame.CreateBitmap(Editor.TextureAnimFrameEditors[frame.ID].DecompressedEditor);
                };

                framesControl.ItemSelectionChanged += (s, e) => OnTextureChanged(s, e);
            }


            return Control;
        }

        public IMPD_Editor Editor { get; }
    }
}
