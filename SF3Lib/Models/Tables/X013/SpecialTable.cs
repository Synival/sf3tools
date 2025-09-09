using SF3.ByteData;
using SF3.Models.Structs.X013;

namespace SF3.Models.Tables.X013 {
    public class SpecialTable : ResourceTable<Special> {
        protected SpecialTable(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address, 256) {
        }

        public static SpecialTable Create(IByteData data, string name, string resourceFile, int address)
            => CreateBase(() => new SpecialTable(data, name, resourceFile, address));

        public override bool Load()
            => Load((id, name, address) => new Special(Data, id, name, address));
    }
}
