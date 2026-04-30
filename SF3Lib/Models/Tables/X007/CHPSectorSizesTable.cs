using SF3.ByteData;
using SF3.Models.Structs.X007;

namespace SF3.Models.Tables.X007 {
    public class CHPSectorSizesTable : ResourceTable<CHPSectorSizes> {
        protected CHPSectorSizesTable(IByteData data, string name, string resourceFile, int address, int? maxSize = 0)
        : base(data, name, resourceFile, address, maxSize) {
        }

        public static CHPSectorSizesTable Create(IByteData data, string name, string resourceFile, int address, int? maxSize = 0)
            => Create(() => new CHPSectorSizesTable(data, name, resourceFile, address, maxSize));

        public override bool Load()
            => Load((id, name, address) => new CHPSectorSizes(Data, id, name, address));
    }
}
