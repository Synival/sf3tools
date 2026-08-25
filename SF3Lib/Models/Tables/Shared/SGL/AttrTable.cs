using SF3.ByteData;
using SF3.Models.Structs.Shared.SGL;

namespace SF3.Models.Tables.Shared.SGL {
    public class AttrTable : FixedSizeTable<AttrStruct> {
        protected AttrTable(IByteData data, string name, int address, int size) : base(data, name, address, size) {
        }

        public static AttrTable Create(IByteData data, string name, int address, int size)
            => Create(() => new AttrTable(data, name, address, size));

        public override bool Load()
            => Load((id, address) => new AttrStruct(Data, id, "ATTR_" + id.ToString("D4"), address));
    }
}
