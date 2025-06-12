using System;
using SF3.ByteData;
using SF3.Models.Structs.X023;

namespace SF3.Models.Tables.X023 {
    public class ShopHaggleTable : TerminatedTable<ShopHaggle> {
        protected ShopHaggleTable(IByteData data, string name, int address) : base(data, name, address, 4, 300) {
        }

        public static ShopHaggleTable Create(IByteData data, string name, int address) {
            var newTable = new ShopHaggleTable(data, name, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, address) => new ShopHaggle(Data, id, $"{nameof(ShopHaggle)}{id:D2}", address),
                (rows, prev) => prev.Item != 0,
                false);
    }
}
