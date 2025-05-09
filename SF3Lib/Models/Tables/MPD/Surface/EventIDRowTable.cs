using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD.Surface;

namespace SF3.Models.Tables.MPD.Surface {
    public class EventIDRowTable : FixedSizeTable<EventIDRow> {
        protected EventIDRowTable(IByteData data, string name, int address) : base(data, name, address, 64) {
        }

        public static EventIDRowTable Create(IByteData data, string name, int address) {
            var newTable = new EventIDRowTable(data, name, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            var size = new EventIDRow(Data, 0, "", Address).Size;
            return Load((id, address) => new EventIDRow(Data, id, "Y" + id.ToString("D2"), Address + id * size));
        }
    }
}
