using System;
using SF3.ByteData;
using SF3.Models.Structs.X1.Town;

namespace SF3.Models.Tables.X1.Town {
    public class EnterTable : ResourceTable<Enter> {
        protected EnterTable(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address, 100) {
        }

        public static EnterTable Create(IByteData data, string name, string resourceFile, int address) {
            var newTable = new EnterTable(data, name, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load(
                (id, name, address) => new Enter(Data, id, name, address),
                (rows, models) => models.Entered != 0xFFFF);
    }
}
