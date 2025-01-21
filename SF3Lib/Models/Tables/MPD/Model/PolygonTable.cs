using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD.Model;

namespace SF3.Models.Tables.MPD.Model {
    public class PolygonTable : FixedSizeTable<PolygonModel> {
        protected PolygonTable(IByteData data, string name, int address, int size) : base(data, name, address, size) {
        }

        public static PolygonTable Create(IByteData data, string name, int address, int size) {
            var newTable = new PolygonTable(data, name, address, size);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, address) => new PolygonModel(Data, id, "POLYGON" + id.ToString("D4"), address));
    }
}
