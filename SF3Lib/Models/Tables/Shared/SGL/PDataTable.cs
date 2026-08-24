using SF3.ByteData;
using SF3.Models.Structs.Shared.SGL;

namespace SF3.Models.Tables.Shared.SGL {
    public class PDataTable : TerminatedTable<PDataStruct> {
        protected PDataTable(IByteData data, string name, int address)
        : base(data, name, address, terminatedBytes: 4, maxSize: 1000) {
        }

        public static PDataTable Create(IByteData data, string name, int address)
            => Create(() => new PDataTable(data, name, address));

        public override bool Load() {
            return Load(
                (id, addr) => new PDataStruct(Data, id, "PData" + id.ToString("D3"), addr),
                (rows, prevRow) => {
                    var data = prevRow.Data.GetInt32(prevRow.Address);
                    return data != -1;
                },
                false
            );
        }
    }
}
