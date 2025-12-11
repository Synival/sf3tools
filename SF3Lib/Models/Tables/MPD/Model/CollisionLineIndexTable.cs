using SF3.ByteData;
using SF3.Models.Structs.MPD.Model;

namespace SF3.Models.Tables.MPD.Model {
    public class CollisionLineIndexTable : TerminatedTable<CollisionLineIndex> {
        protected CollisionLineIndexTable(IByteData data, string name, int address, int blockX, int blockY) : base(data, name, address, 2, null) {
            BlockX = blockX;
            BlockY = blockY;
        }

        public int BlockX { get; }
        public int BlockY { get; }

        public static CollisionLineIndexTable Create(IByteData data, string name, int address, int blockX, int blockY)
            => Create(() => new CollisionLineIndexTable(data, name, address, blockX, blockY));

        public override bool Load() {
            return Load(
                (id, address) => new CollisionLineIndex(Data, id, "LineIndex" + id.ToString("D2"), address),
                (rows, lastRow) => lastRow.LineIndex != 0xFFFF,
                false
            );
        }
    }
}
