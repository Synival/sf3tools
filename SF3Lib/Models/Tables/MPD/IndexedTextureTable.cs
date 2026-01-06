using SF3.ByteData;

namespace SF3.Models.Tables.MPD {
    public class IndexedTextureTable : TextureIDTable {
        protected IndexedTextureTable(IByteData data, string name, int address, int terminatorSize, int? maxSize)
        : base(data, name, "IndexedTex", address, terminatorSize, maxSize) {
        }

        public static IndexedTextureTable Create(IByteData data, string name, int address, int terminatorSize, int? maxSize)
            => Create(() => new IndexedTextureTable(data, name, address, terminatorSize, maxSize));
    }
}
