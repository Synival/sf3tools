using SF3.ByteData;
using SF3.Models.Structs.X013;

namespace SF3.Models.Tables.X013 {
    public class SupportTypeTable : ResourceTable<SupportType> {
        protected SupportTypeTable(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address, 120) {
        }

        public static SupportTypeTable Create(IByteData data, string name, string resourceFile, int address)
            => CreateBase(() => new SupportTypeTable(data, name, resourceFile, address));

        public override bool Load()
            => Load((id, name, address) => new SupportType(Data, id, name, address));
    }
}
