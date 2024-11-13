using SF3.RawEditors;
using SF3.Models.MPD.TextureChunk;

namespace SF3.Tables.MPD.TextureChunk {
    public class TextureTable : Table<Texture> {
        public TextureTable(IRawEditor editor, int address, int textureCount, int startId) : base(editor, address) {
            MaxSize = textureCount;
            StartID = startId;
        }

        public override bool Load() {
            var size = new Texture(FileEditor, StartID, "Texture0", Address).Size;
            return LoadUntilMax((id, address) => {
                var nextImageDataOffset = (id + 1 >= MaxSize)
                    ? FileEditor.Data.Length
                    : new Texture(FileEditor, StartID + id + 1, "", address + size).ImageDataOffset;
                return new Texture(FileEditor, StartID + id, "Texture" + (StartID + id), address, nextImageDataOffset);
            });
        }

        public override int? MaxSize { get; }

        public int StartID { get; }
    }
}
