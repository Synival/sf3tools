using SF3.Models.Structs.MPD;
using SF3.Models.Tables;
using SF3.ByteData;
using System;

namespace SF3.Models.Tables.MPD {
    public class TileHeightTerrainRowTable : Table<TileHeightTerrainRow> {
        protected TileHeightTerrainRowTable(IByteData data, int address) : base(data, address) {
        }

        public static TileHeightTerrainRowTable Create(IByteData data, int address) {
            var newTable = new TileHeightTerrainRowTable(data, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            var size = new TileHeightTerrainRow(Data, 0, "", Address).Size;
            return LoadUntilMax((id, address) => new TileHeightTerrainRow(Data, id, "Y" + id.ToString("D2"), Address + (63 - id) * size));
        }

        public override int? MaxSize => 64;
    }
}
