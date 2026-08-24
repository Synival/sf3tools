using SF3.ByteData;
using SF3.Models.Structs.Shared.SGL;

namespace SF3.Models.Tables.Shared.SGL {
    public class XPDataTable : TerminatedTable<XPDataStruct> {
        protected XPDataTable(IByteData data, string name, int address)
        : base(data, name, address, terminatedBytes: 4, maxSize: 1000) {
        }

        public static XPDataTable Create(IByteData data, string name, int address)
            => Create(() => new XPDataTable(data, name, address));

        public override bool Load() {
            return Load(
                (id, addr) => new XPDataStruct(Data, id, "XPData" + id.ToString("D3"), addr),
                (rows, prevRow) => {
                    var data = prevRow.Data.GetInt32(prevRow.Address);
                    return data != -1;
                },
                false
            );
        }
    }
}
