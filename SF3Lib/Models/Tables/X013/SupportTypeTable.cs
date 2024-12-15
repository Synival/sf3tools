using SF3.Models.Structs.X013;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.X013 {
    public class SupportTypeTable : Table<SupportType> {
        public SupportTypeTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new SupportType(Data, id, name, address));

        public override int? MaxSize => 120;
    }
}
