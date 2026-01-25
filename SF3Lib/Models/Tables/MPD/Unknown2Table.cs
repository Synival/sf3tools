using SF3.ByteData;

namespace SF3.Models.Tables.MPD {
    public class Unknown2Table : UnknownUInt16Table {
        protected Unknown2Table(IByteData data, string name, int address, int count, bool isDummiedOut)
        : base(data, name, address, count, null) {
            IsDummiedOut = isDummiedOut;
        }

        public static Unknown2Table Create(IByteData data, string name, int address, int count, bool isDummiedOut)
            => Create(() => new Unknown2Table(data, name, address, count, isDummiedOut));

        /// <summary>
        /// When true, the Unknown2 table is serialized, but "dummied-out" with a preceeding
        /// 0xFFFF that prevents it from loading.
        /// </summary>
        public bool IsDummiedOut { get; }
    }
}
