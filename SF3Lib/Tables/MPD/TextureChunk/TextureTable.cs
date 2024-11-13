using SF3.StreamEditors;
using SF3.Models.MPD.TextureChunk;

namespace SF3.Tables.MPD.TextureChunk {
    public class TextureTable : Table<Texture> {
        public TextureTable(IByteEditor fileEditor, int address, int textureCount, int startId) : base(fileEditor, address) {
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
