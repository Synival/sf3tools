using SF3.ByteData;

namespace SF3.Models.Tables.MPD {
    public class IgnoredTextureTable : TextureIDTable {
        protected IgnoredTextureTable(IByteData data, string name, int address, int terminatorSize, int? maxSize)
        : base(data, name, "IgnoreTex", address, terminatorSize, maxSize) {
        }

        public static IgnoredTextureTable Create(IByteData data, string name, int address, int terminatorSize, int? maxSize)
            => Create(() => new IgnoredTextureTable(data, name, address, terminatorSize, maxSize));
    }
}
