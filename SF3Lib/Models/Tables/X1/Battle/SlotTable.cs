using System;
using SF3.ByteData;
using SF3.Models.Structs.X1.Battle;

namespace SF3.Models.Tables.X1.Battle {
    public class SlotTable : ResourceTable<Slot> {
        protected SlotTable(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address, 256) {
        }

        public static SlotTable Create(IByteData data, string name, string resourceFile, int address) {
            var newTable = new SlotTable(data, name, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, name, address) => new Slot(Data, id, name, address));
    }
}
