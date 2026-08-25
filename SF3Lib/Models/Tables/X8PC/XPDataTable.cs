using SF3.ByteData;
using SF3.Models.Structs.X8PC;

namespace SF3.Models.Tables.X8PC {
    public class PC_SGL_Model_XPDataTable : TerminatedTable<PC_SGL_Model_XPDataStruct> {
        protected PC_SGL_Model_XPDataTable(IByteData data, string name, int address, PolyChar polyChar)
        : base(data, name, address, terminatedBytes: 4, maxSize: 1000) {
            PolyChar = polyChar;
        }

        public static PC_SGL_Model_XPDataTable Create(IByteData data, string name, int address, PolyChar polyChar)
            => Create(() => new PC_SGL_Model_XPDataTable(data, name, address, polyChar));

        public override bool Load() {
            return Load(
                (id, addr) => new PC_SGL_Model_XPDataStruct(Data, id, "XPDATA_" + id.ToString("D3"), addr, PolyChar),
                (rows, prevRow) => {
                    var data = prevRow.Data.GetInt32(prevRow.Address);
                    return data != -1;
                },
                false
            );
        }

        public PolyChar PolyChar { get; }
    }
}
