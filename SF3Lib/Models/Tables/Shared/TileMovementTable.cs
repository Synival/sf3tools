using System;
using SF3.ByteData;
using SF3.Models.Structs.Shared;

namespace SF3.Models.Tables.Shared {
    public class TileMovementTable : ResourceTable<TileMovement> {
        protected TileMovementTable(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address, 31) {
        }

        public static TileMovementTable Create(IByteData data, string name, string resourceFile, int address) {
            var newTable = new TileMovementTable(data, name, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, name, address) => new TileMovement(Data, id, name, address));
    }
}
