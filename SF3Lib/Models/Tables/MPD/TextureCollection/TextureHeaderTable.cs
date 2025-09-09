using SF3.ByteData;
using SF3.Models.Structs.MPD.TextureChunk;

namespace SF3.Models.Tables.MPD.TextureCollection {
    public class TextureHeaderTable : FixedSizeTable<TextureHeader> {
        protected TextureHeaderTable(IByteData data, string name, int address) : base(data, name, address, 1) {
        }

        public static TextureHeaderTable Create(IByteData data, string name, int address)
            => Create(() => new TextureHeaderTable(data, name, address));

        public override bool Load()
            => Load((id, address) => new TextureHeader(Data, id, "Header", address));
    }
}
