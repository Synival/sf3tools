using System;
using SF3.ByteData;
using SF3.Models.Structs.X1.Battle;

namespace SF3.Models.Tables.X1.Battle {
    public class CustomMovementTable : ResourceTable<CustomMovement> {
        protected CustomMovementTable(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address, 130) {
        }

        public static CustomMovementTable Create(IByteData data, string name, string resourceFile, int address) {
            var newTable = new CustomMovementTable(data, name, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, name, address) => new CustomMovement(Data, id, name, address));
    }
}
