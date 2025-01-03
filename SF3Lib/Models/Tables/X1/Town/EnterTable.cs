using SF3.Models.Structs.X1.Town;
using SF3.Models.Tables;
using SF3.ByteData;
using System;

namespace SF3.Models.Tables.X1.Town {
    public class EnterTable : Table<Enter> {
        protected EnterTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address) {
        }

        public static EnterTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new EnterTable(data, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => LoadFromResourceFile(
                (id, name, address) => new Enter(Data, id, name, address),
                (rows, models) => models.Entered != 0xFFFF);

        public override int? MaxSize => 100;
    }
}
