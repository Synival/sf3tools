using SF3.Models.Structs.MPD;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.MPD {
    public class ColorTable : Table<ColorModel> {
        public ColorTable(IByteData data, int address, int colors) : base(data, address) {
            MaxSize = colors;
        }

        public override bool Load()
            => LoadUntilMax((id, address) => new ColorModel(Data, id, "Color" + id.ToString("D3"), address));

        public override int? MaxSize { get; }
    }
}
