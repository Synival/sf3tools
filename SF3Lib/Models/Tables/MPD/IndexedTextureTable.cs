using SF3.ByteData;

namespace SF3.Models.Tables.MPD {
    public class IndexedTextureTable : TextureIDTable {
        protected IndexedTextureTable(IByteData data, string name, int address, int? maxSize)
        : base(data, name, "IndexedTex", address, 4, maxSize) {
        }

        public static IndexedTextureTable Create(IByteData data, string name, int address, int? maxSize)
            => Create(() => new IndexedTextureTable(data, name, address, maxSize));
    }
}
