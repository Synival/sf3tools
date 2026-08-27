using SF3.ByteData;
using SF3.Models.Structs.X8PC;

namespace SF3.Models.Tables.X8PC {
    public class PC_XPDataListTable : TerminatedTable<PC_XPDataListStruct> {
        protected PC_XPDataListTable(IByteData data, string name, int address, PolyChar polyChar)
        : base(data, name, address, terminatedBytes: 4, maxSize: 1000) {
            PolyChar = polyChar;
        }

        public static PC_XPDataListTable Create(IByteData data, string name, int address, PolyChar polyChar)
            => Create(() => new PC_XPDataListTable(data, name, address, polyChar));

        public override bool Load() {
            return Load(
                (id, addr) => new PC_XPDataListStruct(Data, id, "XPDATA_List_" + id.ToString("D3"), addr, PolyChar),
                (rows, prevRow) => prevRow.XPDataListOffset != -1,
                false
            );
        }

        public PolyChar PolyChar { get; }
    }
}
