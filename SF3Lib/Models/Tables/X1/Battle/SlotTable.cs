using SF3.Models.Structs.X1.Battle;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.X1.Battle {
    public class SlotTable : Table<Slot> {
        public SlotTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Slot(Data, id, name, address));

        public override int? MaxSize => 256;

    }
}
