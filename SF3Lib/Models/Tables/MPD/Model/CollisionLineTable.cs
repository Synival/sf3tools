using System.Collections.Generic;
using SF3.ByteData;
using SF3.Models.Structs.MPD.Model;

namespace SF3.Models.Tables.MPD.Model {
    public class CollisionLineTable : FixedSizeTable<CollisionLine> {
        protected CollisionLineTable(IByteData data, string name, int address, int size, CollisionPointTable pointTable, IEnumerable<CollisionLineIndexTable> blockLines)
        : base(data, name, address, size) {
            PointTable = pointTable;
            BlockLines = blockLines;
        }

        public static CollisionLineTable Create(IByteData data, string name, int address, int size, CollisionPointTable pointTable, IEnumerable<CollisionLineIndexTable> blockLines)
            => Create(() => new CollisionLineTable(data, name, address, size, pointTable, blockLines));

        public override bool Load()
            => Load((id, address) => new CollisionLine(Data, id, "Line" + id.ToString("D3"), address, PointTable, BlockLines));

        public CollisionPointTable PointTable { get; }
        public IEnumerable<CollisionLineIndexTable> BlockLines { get; }
    }
}
