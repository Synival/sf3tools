using System;
using SF3.ByteData;
using SF3.Models.Structs.X1.Town;

namespace SF3.Models.Tables.X1.Town {
    public class ArrowTable : ResourceTable<Arrow> {
        protected ArrowTable(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address, 100) {
        }

        public static ArrowTable Create(IByteData data, string name, string resourceFile, int address) {
            var newTable = new ArrowTable(data, name, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load(
                (id, name, address) => new Arrow(Data, id, name, address),
                (rows, model) => model.ArrowUnknown0 != 0xFFFF);
    }
}
