using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD.Surface;

namespace SF3.Models.Tables.MPD.Surface {
    public class EventIDRowTable : Table<EventIDRow> {
        protected EventIDRowTable(IByteData data, int address) : base(data, address) {
        }

        public static EventIDRowTable Create(IByteData data, int address) {
            var newTable = new EventIDRowTable(data, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            var size = new EventIDRow(Data, 0, "", Address).Size;
            return LoadUntilMax((id, address) => new EventIDRow(Data, id, "Y" + id.ToString("D2"), Address + id * size));
        }

        public override int? MaxSize => 64;
    }
}
