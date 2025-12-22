using SF3.ByteData;
using SF3.Models.Structs.MPD;

namespace SF3.Models.Tables.MPD {
    public class ColorTable : FixedSizeTable<Color> {
        protected ColorTable(IByteData data, string name, int address, int size) : base(data, name, address, size) {}

        public static ColorTable Create(IByteData data, string name, int address, int size)
            => Create(() => new ColorTable(data, name, address, size));

        public override bool Load()
            => Load((id, address) => new Color(Data, id, "Color" + id.ToString("D3"), address));
    }
}
