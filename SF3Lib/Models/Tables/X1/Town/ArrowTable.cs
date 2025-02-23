using System;
using SF3.ByteData;
using SF3.Models.Structs.X1.Town;

namespace SF3.Models.Tables.X1.Town {
    public class ArrowTable : TerminatedTable<Arrow> {
        protected ArrowTable(IByteData data, string name, int address) : base(data, name, address, 100) {
        }

        public static ArrowTable Create(IByteData data, string name, int address) {
            var newTable = new ArrowTable(data, name, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load(
                (id, address) => new Arrow(Data, id, "Arrow" + id.ToString("D2"), address),
                (rows, model) => model.ArrowUnknown0 != 0xFFFF,
                false);
    }
}
