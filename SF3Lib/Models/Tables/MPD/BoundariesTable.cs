using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD;

namespace SF3.Models.Tables.MPD {
    public class BoundariesTable : Table<Boundaries> {
        protected BoundariesTable(IByteData data, int address) : base(data, address) { }

        public static BoundariesTable Create(IByteData data, int address) {
            var newTable = new BoundariesTable(data, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => LoadUntilMax((id, address) => new Boundaries(Data, id, "Boundaries", address));

        public override int? MaxSize => 1;
    }
}
