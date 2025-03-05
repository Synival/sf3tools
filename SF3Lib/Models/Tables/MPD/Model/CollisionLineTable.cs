using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD.Model;

namespace SF3.Models.Tables.MPD.Model {
    public class CollisionLineTable : TerminatedTable<CollisionLine> {
        protected CollisionLineTable(IByteData data, string name, int address, int? maxSize) : base(data, name, address, maxSize) {
        }

        public static CollisionLineTable Create(IByteData data, string name, int address, int? maxSize) {
            var newTable = new CollisionLineTable(data, name, address, maxSize);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            return Load(
                (id, address) => new CollisionLine(Data, id, "Line" + id.ToString("D2"), address),
                (rows, lastRow) => lastRow.Point1IDIndex != 0xFFFF,
                false
            );
        }
    }
}
