using System;
using SF3.ByteData;
using SF3.Models.Structs.X002;

namespace SF3.Models.Tables.X002 {
    public class ItemTable : ResourceTable<Item> {
        protected ItemTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address, 300) {
        }

        public static ItemTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new ItemTable(data, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, name, address) => new Item(Data, id, name, address));
    }
}
