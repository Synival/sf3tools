using SF3.RawEditors;
using SF3.Models.MPD.TextureChunk;

namespace SF3.Tables.MPD.TextureChunk {
    public class TextureTable : Table<Texture> {
        public TextureTable(IRawEditor editor, int address, int textureCount, int startId) : base(editor, address) {
            MaxSize = textureCount;
            StartID = startId;
        }

        public override bool Load() {
            var size = new Texture(Editor, StartID, "Texture0", Address).Size;
            return LoadUntilMax((id, address) => {
                var nextImageDataOffset = (id + 1 >= MaxSize)
                    ? Editor.Data.Length
                    : new Texture(Editor, StartID + id + 1, "", address + size).ImageDataOffset;
                return new Texture(Editor, StartID + id, "Texture" + (StartID + id), address, nextImageDataOffset);
            });
        }

        public override int? MaxSize { get; }

        public int StartID { get; }
    }
}
