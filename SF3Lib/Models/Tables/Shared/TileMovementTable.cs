using System;
using SF3.ByteData;
using SF3.Models.Structs.Shared;

namespace SF3.Models.Tables.Shared {
    public class TileMovementTable : ResourceTable<TileMovement> {
        protected TileMovementTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address) {
        }

        public static TileMovementTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new TileMovementTable(data, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new TileMovement(Data, id, name, address));

        public override int? MaxSize => 31;
    }
}
