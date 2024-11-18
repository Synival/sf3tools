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

            _ = CreateChild(new TableEditorControl("Header", Editor.Header, ngc));
            for (var i = 0; i < Editor.Palettes.Length; i++)
                _ = CreateChild(new TableEditorControl("Palette" + (i + 1).ToString(), Editor.Palettes[i], ngc));
            _ = CreateChild(new TableEditorControl("Chunk Header", Editor.ChunkHeader, ngc));
            _ = CreateChild(new TableEditorControl("Surface Characters", Editor.TileSurfaceCharacterRows, ngc));
            _ = CreateChild(new TableEditorControl("Surface Heightmap", Editor.TileSurfaceHeightmapRows, ngc));
            _ = CreateChild(new TableEditorControl("Tile Height + Terrain", Editor.TileHeightTerrainRows, ngc));
            _ = CreateChild(new TableEditorControl("Object Locations", Editor.TileItemRows, ngc));

            for (var i = 0; i < Editor.TextureChunks.Length; i++) {
                // TODO: make TextureChunkEditorControl!
                // CreateChild(new TextureChunkEditorControl("Textures " + (i + 1), Editor.TextureChunk[i], ngc));
            }

            return Control;
        }

        public MPD_Editor Editor { get; }
    }
}
