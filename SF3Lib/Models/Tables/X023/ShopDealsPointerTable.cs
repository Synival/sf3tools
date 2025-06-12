using System;
using SF3.ByteData;
using SF3.Models.Structs.X023;

namespace SF3.Models.Tables.X023 {
    public class ShopDealsPointerTable : TerminatedTable<ShopDealsPointer> {
        protected ShopDealsPointerTable(IByteData data, string name, int address) : base(data, name, address, 4, 300) {
        }

        public static ShopDealsPointerTable Create(IByteData data, string name, int address) {
            var newTable = new ShopDealsPointerTable(data, name, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, address) => new ShopDealsPointer(Data, id, $"{nameof(ShopDealsPointer)}{id:D2}", address),
                (rows, prev) => prev.ShopDeals != 0,
                false);
    }
}
