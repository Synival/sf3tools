using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD.Model;

namespace SF3.Models.Tables.MPD.Model {
    public class CollisionLineIndexTable : TerminatedTable<CollisionLineIndex> {
        protected CollisionLineIndexTable(IByteData data, string name, int address) : base(data, name, address) {
        }

        public static CollisionLineIndexTable Create(IByteData data, string name, int address) {
            var newTable = new CollisionLineIndexTable(data, name, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            return Load(
                (id, address) => new CollisionLineIndex(Data, id, "LineIndex" + id.ToString("D2"), address),
                (rows, lastRow) => lastRow.LineIndex != 0xFFFF,
                false
            );
        }
    }
}
