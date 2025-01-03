using SF3.Models.Structs.X1;
using SF3.Models.Tables;
using SF3.ByteData;
using System;

namespace SF3.Models.Tables.X1 {
    public class TreasureTable : Table<Treasure> {
        protected TreasureTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address) {
        }

        public static TreasureTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new TreasureTable(data, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => LoadFromResourceFile(
                (id, name, address) => new Treasure(Data, id, name, address),
                (rows, model) => model.Searched != 0xFFFF);

        public override int? MaxSize => 255;
    }
}
