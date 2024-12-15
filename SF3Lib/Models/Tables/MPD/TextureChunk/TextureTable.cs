using SF3.Models.Structs.MPD.TextureChunk;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.MPD.TextureChunk {
    public class TextureTable : Table<TextureModel> {
        public TextureTable(IByteData data, int address, int textureCount, int startId) : base(data, address) {
            MaxSize = textureCount;
            StartID = startId;
        }

        public override bool Load() {
            var size = TextureModel.GlobalSize;
            return LoadUntilMax((id, address) => {
                var nextImageDataOffset = id + 1 >= MaxSize
                    ? Data.Length
                    : Data.GetWord(address + size + 2);
                return new TextureModel(Data, StartID + id, "Texture" + (StartID + id).ToString("D3"), address, nextImageDataOffset);
            });
        }

        public override int? MaxSize { get; }

        public int StartID { get; }
    }
}
