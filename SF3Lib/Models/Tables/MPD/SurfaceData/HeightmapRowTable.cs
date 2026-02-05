using SF3.ByteData;
using SF3.Models.Structs.MPD.Surface;

namespace SF3.Models.Tables.MPD.Surface {
    public class HeightmapRowTable : FixedSizeTable<HeightmapRow> {
        protected HeightmapRowTable(IByteData data, string name, int address) : base(data, name, address, 64) {
        }

        public static HeightmapRowTable Create(IByteData data, string name, int address)
            => Create(() => new HeightmapRowTable(data, name, address));

        public override bool Load() {
            var size = new HeightmapRow(Data, 0, "", Address).Size;
            return Load((id, address) => new HeightmapRow(Data, id, "Y" + id.ToString("D2"), Address + id * size));
        }
    }
}
