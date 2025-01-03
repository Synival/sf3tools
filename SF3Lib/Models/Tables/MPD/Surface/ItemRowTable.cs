using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD.Surface;

namespace SF3.Models.Tables.MPD.Surface {
    public class ItemRowTable : Table<ItemRow> {
        protected ItemRowTable(IByteData data, int address) : base(data, address) {
        }

        public static ItemRowTable Create(IByteData data, int address) {
            var newTable = new ItemRowTable(data, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            var size = new ItemRow(Data, 0, "", Address).Size;
            return LoadUntilMax((id, address) => new ItemRow(Data, id, "Y" + id.ToString("D2"), Address + (63 - id) * size));
        }

        public override int? MaxSize => 64;
    }
}
