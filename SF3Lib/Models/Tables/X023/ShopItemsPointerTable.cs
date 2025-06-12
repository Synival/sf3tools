using System;
using SF3.ByteData;
using SF3.Models.Structs.X023;

namespace SF3.Models.Tables.X023 {
    public class ShopItemsPointerTable : TerminatedTable<ShopItemsPointer> {
        protected ShopItemsPointerTable(IByteData data, string name, int address) : base(data, name, address, 4, 300) {
        }

        public static ShopItemsPointerTable Create(IByteData data, string name, int address) {
            var newTable = new ShopItemsPointerTable(data, name, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, address) => new ShopItemsPointer(Data, id, $"{nameof(ShopItemsPointer)}{id:D2}", address),
                (rows, prev) => prev.ShopItems != 0,
                false);
    }
}
