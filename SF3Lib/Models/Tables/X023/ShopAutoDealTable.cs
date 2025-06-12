using System;
using SF3.ByteData;
using SF3.Models.Structs.X023;

namespace SF3.Models.Tables.X023 {
    public class ShopAutoDealTable : TerminatedTable<ShopAutoDeal> {
        protected ShopAutoDealTable(IByteData data, string name, int address, bool hasFlag) : base(data, name, address, 4, 300) {
            HasFlag = hasFlag;
        }

        public static ShopAutoDealTable Create(IByteData data, string name, int address, bool hasFlag) {
            var newTable = new ShopAutoDealTable(data, name, address, hasFlag);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, address) => new ShopAutoDeal(Data, id, $"{nameof(ShopAutoDeal)}{id:D2}", address, HasFlag),
                (rows, prev) => prev.Item != 0,
                false);

        public bool HasFlag { get; }

    }
}
