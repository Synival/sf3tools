using System;
using SF3.ByteData;
using SF3.Models.Structs.X023;

namespace SF3.Models.Tables.X023 {
    public class ShopPointerTable : TerminatedTable<ShopPointer> {
        protected ShopPointerTable(IByteData data, string name, int address) : base(data, name, address, 4, 300) {
        }

        public static ShopPointerTable Create(IByteData data, string name, int address) {
            var newTable = new ShopPointerTable(data, name, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, address) => new ShopPointer(Data, id, $"{nameof(ShopPointer)}{id:D2}", address),
                (rows, prev) => prev.Shop != 0,
                false);
    }
}
