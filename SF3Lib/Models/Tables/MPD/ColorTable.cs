using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD;

namespace SF3.Models.Tables.MPD {
    public class ColorTable : Table<ColorModel> {
        protected ColorTable(IByteData data, int address, int colors) : base(data, address) {
            MaxSize = colors;
        }

        public static ColorTable Create(IByteData data, int address, int colors) {
            var newTable = new ColorTable(data, address, colors);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => LoadUntilMax((id, address) => new ColorModel(Data, id, "Color" + id.ToString("D3"), address));

        public override int? MaxSize { get; }
    }
}
