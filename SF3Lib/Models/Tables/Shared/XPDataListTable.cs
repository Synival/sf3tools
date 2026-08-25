using SF3.ByteData;
using SF3.Models.Structs.Shared;

namespace SF3.Models.Tables.Shared {
    public class XPDataListTable : TerminatedTable<XPDataListStruct> {
        protected XPDataListTable(IByteData data, string name, int address)
        : base(data, name, address, terminatedBytes: 4, maxSize: 1000) {
        }

        public static XPDataListTable Create(IByteData data, string name, int address)
            => Create(() => new XPDataListTable(data, name, address));

        public override bool Load() {
            return Load(
                (id, addr) => new XPDataListStruct(Data, id, "XPDATA_List_" + id.ToString("D3"), addr),
                (rows, prevRow) => prevRow.XPDataListOffset != -1,
                false
            );
        }
    }
}
