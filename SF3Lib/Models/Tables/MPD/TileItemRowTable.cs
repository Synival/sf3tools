using SF3.Models.Structs.MPD;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.MPD {
    public class TileItemRowTable : Table<TileItemRow> {
        protected TileItemRowTable(IByteData data, int address) : base(data, address) {
        }

        public static TileItemRowTable Create(IByteData data, int address) {
            var newTable = new TileItemRowTable(data, address);
            newTable.Load();
            return newTable;
        }

        public override bool Load() {
            var size = new TileItemRow(Data, 0, "", Address).Size;
            return LoadUntilMax((id, address) => new TileItemRow(Data, id, "Y" + id.ToString("D2"), Address + (63 - id) * size));
        }

        public override int? MaxSize => 64;
    }
}
