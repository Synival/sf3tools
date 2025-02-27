using System;
using SF3.ByteData;
using SF3.Models.Structs.Shared;

namespace SF3.Models.Tables.Shared {
    public class TileMovementTable : ResourceTable<TileMovement> {
        protected TileMovementTable(IByteData data, string name, string resourceFile, int address, bool hasExtra) : base(data, name, resourceFile, address, hasExtra ? 16 : 13) {
            HasExtra = hasExtra;
        }

        public static TileMovementTable Create(IByteData data, string name, string resourceFile, int address, bool hasExtra) {
            var newTable = new TileMovementTable(data, name, resourceFile, address, hasExtra);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, name, address) => new TileMovement(Data, id, name, address));

        public bool HasExtra { get; }
    }
}
