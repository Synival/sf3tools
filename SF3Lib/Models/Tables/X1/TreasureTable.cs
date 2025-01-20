using System;
using SF3.ByteData;
using SF3.Models.Structs.X1;

namespace SF3.Models.Tables.X1 {
    public class TreasureTable : ResourceTable<Treasure> {
        protected TreasureTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address, 255) {
        }

        public static TreasureTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new TreasureTable(data, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load(
                (id, name, address) => new Treasure(Data, id, name, address),
                (rows, model) => model.Searched != 0xFFFF);
    }
}
