using SF3.Models.Structs.MPD;
using SF3.Models.Tables;
using SF3.ByteData;
using System;

namespace SF3.Models.Tables.MPD {
    public class TileSurfaceHeightmapRowTable : Table<TileSurfaceHeightmapRow> {
        protected TileSurfaceHeightmapRowTable(IByteData data, int address) : base(data, address) {
        }

        public static TileSurfaceHeightmapRowTable Create(IByteData data, int address) {
            var newTable = new TileSurfaceHeightmapRowTable(data, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            var size = new TileSurfaceHeightmapRow(Data, 0, "", Address).Size;
            return LoadUntilMax((id, address) => new TileSurfaceHeightmapRow(Data, id, "Y" + id.ToString("D2"), Address + (63 - id) * size));
        }

        public override int? MaxSize => 64;
    }
}
