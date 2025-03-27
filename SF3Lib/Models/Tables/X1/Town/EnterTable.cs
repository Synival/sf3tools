using System;
using SF3.ByteData;
using SF3.Models.Structs.X1.Town;

namespace SF3.Models.Tables.X1.Town {
    public class EnterTable : TerminatedTable<Enter> {
        protected EnterTable(IByteData data, string name, int address)
        : base(data, name, address, 2, 100) {
        }

        public static EnterTable Create(IByteData data, string name, int address) {
            var newTable = new EnterTable(data, name, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load(
                (id, address) => new Enter(Data, id, "Enter" + id.ToString("X2"), address),
                (rows, models) => models.SceneID != 0xFFFF,
                false);
    }
}
