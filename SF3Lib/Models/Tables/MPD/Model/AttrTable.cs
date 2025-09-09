using SF3.ByteData;
using SF3.Models.Structs.MPD.Model;

namespace SF3.Models.Tables.MPD.Model {
    public class AttrTable : FixedSizeTable<AttrModel> {
        protected AttrTable(IByteData data, string name, int address, int size) : base(data, name, address, size) {
        }

        public static AttrTable Create(IByteData data, string name, int address, int size)
            => CreateBase(() => new AttrTable(data, name, address, size));

        public override bool Load()
            => Load((id, address) => new AttrModel(Data, id, "ATTR" + id.ToString("D4"), address));
    }
}
