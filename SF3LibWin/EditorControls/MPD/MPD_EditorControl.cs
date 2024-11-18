using System.Windows.Forms;
using SF3.Editors.MPD;

namespace SF3.Win.EditorControls.MPD {
    public class MPD_EditorControl : EditorControlContainer {
        public MPD_EditorControl(string name, MPD_Editor editor) : base(name) {
            Editor = editor;
        }

        public override Control Create() {
            base.Create();

            var ngc = Editor.NameGetterContext;

            var headerContainer = new EditorControlContainer("Headers");
            _ = CreateChild(headerContainer);
            _ = headerContainer.CreateChild(new TableEditorControl("Header", Editor.Header, ngc));
            _ = headerContainer.CreateChild(new TableEditorControl("Chunk Header", Editor.ChunkHeader, ngc));

            var paletteContainer = new EditorControlContainer("Palettes");
            _ = CreateChild(paletteContainer);
            for (var i = 0; i < Editor.Palettes.Length; i++)
                _ = paletteContainer.CreateChild(new TableEditorControl("Palette" + (i + 1).ToString(), Editor.Palettes[i], ngc));

            var surfaceContainer = new EditorControlContainer("Surface");
            _ = CreateChild(surfaceContainer);
            _ = surfaceContainer.CreateChild(new TableEditorControl("Textures", Editor.TileSurfaceCharacterRows, ngc));
            _ = surfaceContainer.CreateChild(new TableEditorControl("Heightmap", Editor.TileSurfaceHeightmapRows, ngc));
            _ = surfaceContainer.CreateChild(new TableEditorControl("Height + Terrain Type", Editor.TileHeightTerrainRows, ngc));
            _ = surfaceContainer.CreateChild(new TableEditorControl("Object Locations", Editor.TileItemRows, ngc));

            var textureContainer = new EditorControlContainer("Textures");
            _ = CreateChild(textureContainer);
            for (var i = 0; i < Editor.TextureChunks.Length; i++)
                _ = textureContainer.CreateChild(new TextureChunkEditorControl("Chunk " + (i + 6), Editor.TextureChunks[i]));

            return Control;
        }

        public MPD_Editor Editor { get; }
    }
}
