using System;
using SF3.ByteData;
using SF3.Models.Structs.X023;

namespace SF3.Models.Tables.X023 {
    public class ShopAutoDealsPointerTable : TerminatedTable<ShopAutoDealsPointer> {
        protected ShopAutoDealsPointerTable(IByteData data, string name, int address, int? hasFlagOffset) : base(data, name, address, 4, 300) {
            HasFlagOffset = hasFlagOffset;
        }

        public static ShopAutoDealsPointerTable Create(IByteData data, string name, int address, int? hasFlagOffset) {
            var newTable = new ShopAutoDealsPointerTable(data, name, address, hasFlagOffset);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, address) => new ShopAutoDealsPointer(Data, id, $"{nameof(ShopAutoDealsPointer)}{id:D2}", address, HasFlagOffset),
                (rows, prev) => prev.ShopAutoDeals != 0,
                false);

        public int? HasFlagOffset { get; }
    }
}
