using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD;

namespace SF3.Models.Tables.MPD {
    public class ColorTable : FixedSizeTable<ColorModel> {
        protected ColorTable(IByteData data, int address, int size) : base(data, address, size) {}

        public static ColorTable Create(IByteData data, int address, int colors) {
            var newTable = new ColorTable(data, address, colors);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, address) => new ColorModel(Data, id, "Color" + id.ToString("D3"), address));
    }
}
