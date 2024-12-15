using SF3.Models.Structs.MPD.TextureChunk;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.MPD.TextureChunk {
    public class TextureHeaderTable : Table<TextureHeader> {
        public TextureHeaderTable(IByteData data, int address) : base(data, address) {
        }

        public override bool Load()
            => LoadUntilMax((id, address) => new TextureHeader(Data, id, "Header", address));

        public override int? MaxSize => 1;
    }
}
