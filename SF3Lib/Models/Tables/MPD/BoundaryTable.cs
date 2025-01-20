using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD;

namespace SF3.Models.Tables.MPD {
    public class BoundaryTable : ResourceTable<Boundary> {
        protected BoundaryTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address) { }

        public static BoundaryTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new BoundaryTable(data, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Boundary(Data, id, name, address));
    }
}
