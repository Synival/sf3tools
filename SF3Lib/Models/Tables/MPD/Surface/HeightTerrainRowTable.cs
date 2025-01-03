using SF3.ByteData;
using System;
using SF3.Models.Structs.MPD.Surface;

namespace SF3.Models.Tables.MPD.Surface {
    public class HeightTerrainRowTable : Table<HeightTerrainRow> {
        protected HeightTerrainRowTable(IByteData data, int address) : base(data, address) {
        }

        public static HeightTerrainRowTable Create(IByteData data, int address) {
            var newTable = new HeightTerrainRowTable(data, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            var size = new HeightTerrainRow(Data, 0, "", Address).Size;
            return LoadUntilMax((id, address) => new HeightTerrainRow(Data, id, "Y" + id.ToString("D2"), Address + (63 - id) * size));
        }

        public override int? MaxSize => 64;
    }
}
