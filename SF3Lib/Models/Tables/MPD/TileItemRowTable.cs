using SF3.Models.Structs.MPD;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.MPD {
    public class TileItemRowTable : Table<TileItemRow> {
        public TileItemRowTable(IRawData data, int address) : base(data, address) {
        }

        public override bool Load() {
            var size = new TileItemRow(Data, 0, "", Address).Size;
            return LoadUntilMax((id, address) => new TileItemRow(Data, id, "Y" + id.ToString("D2"), Address + (63 - id) * size));
        }

        public override int? MaxSize => 64;
    }
}
