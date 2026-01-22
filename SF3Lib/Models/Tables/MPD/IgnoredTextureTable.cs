using SF3.ByteData;

namespace SF3.Models.Tables.MPD {
    public class IgnoredTextureTable : TextureIDTable {
        protected IgnoredTextureTable(IByteData data, string name, int address, int? readUntil, int? maxSize)
        : base(data, name, "IgnoreTex", GetRealAddress(data, address, readUntil), 2, maxSize) {
            IsDummiedOut = Address != address;
        }

        public static int GetRealAddress(IByteData data, int address, int? readUntil) {
            return (readUntil.HasValue && readUntil - address > 2 && data.GetWord(address) == 0xFFFF && data.GetWord(readUntil.Value - 2) == 0xFFFF)
                ? address + 2 : address;
        }

        public static IgnoredTextureTable Create(IByteData data, string name, int address, int? readUntil, int? maxSize)
            => Create(() => new IgnoredTextureTable(data, name, address, readUntil, maxSize));

        /// <summary>
        /// When true, the gradient is serialized, but "dummied-out" with a preceeding
        /// 0xFFFF that prevents it from loading.
        /// </summary>
        public bool IsDummiedOut { get; }
    }
}
