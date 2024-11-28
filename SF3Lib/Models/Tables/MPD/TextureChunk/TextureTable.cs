using SF3.Models.Structs.MPD.TextureChunk;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.MPD.TextureChunk {
    public class TextureTable : Table<Texture> {
        public TextureTable(IRawData editor, int address, int textureCount, int startId) : base(editor, address) {
            MaxSize = textureCount;
            StartID = startId;
        }

        public override bool Load() {
            var size = Texture.GlobalSize;
            return LoadUntilMax((id, address) => {
                var nextImageDataOffset = id + 1 >= MaxSize
                    ? Editor.Size
                    : Editor.GetWord(address + size + 2);
                return new Texture(Editor, StartID + id, "Texture" + (StartID + id).ToString("D3"), address, nextImageDataOffset);
            });
        }

        public override int? MaxSize { get; }

        public int StartID { get; }
    }
}
