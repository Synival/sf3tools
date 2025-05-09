using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD.Surface;

namespace SF3.Models.Tables.MPD.Surface {
    public class HeightTerrainRowTable : FixedSizeTable<HeightTerrainRow> {
        protected HeightTerrainRowTable(IByteData data, string name, int address) : base(data, name, address, 64) {
        }

        public static HeightTerrainRowTable Create(IByteData data, string name, int address) {
            var newTable = new HeightTerrainRowTable(data, name, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            var size = new HeightTerrainRow(Data, 0, "", Address).Size;
            return Load((id, address) => new HeightTerrainRow(Data, id, "Y" + id.ToString("D2"), Address + id * size));
        }
    }
}
