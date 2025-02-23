using System;
using SF3.ByteData;
using SF3.Models.Structs.X1.Battle;

namespace SF3.Models.Tables.X1.Battle {
    public class SlotTable : FixedSizeTable<Slot> {
        protected SlotTable(IByteData data, string name, int address, int size) : base(data, name, address, size) {
        }

        public static SlotTable Create(IByteData data, string name, int address, int size) {
            var newTable = new SlotTable(data, name, address, size);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            return Load((id, address) => {
                var name = (id < 12) ? ("CharacterSlot" + id.ToString("D2")) : ("EnemySlot" + (id - 12).ToString("D2"));
                return new Slot(Data, id, name, address);
            });
        }
    }
}
