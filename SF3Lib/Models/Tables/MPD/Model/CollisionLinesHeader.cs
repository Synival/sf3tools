using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Models.Structs;

namespace SF3.Models.Tables.MPD.Model {
    public class CollisionLinesHeader : Struct {
        private readonly int _pointsOffsetAddr;
        private readonly int _linesOffsetAddr;

        public CollisionLinesHeader(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x08) {
            _pointsOffsetAddr = Address + 0x00; // 4 bytes
            _linesOffsetAddr  = Address + 0x04; // 4 bytes
        }

        [TableViewModelColumn(displayOrder: 0, isPointer: true, minWidth: 80)]
        [BulkCopy]
        public int PointsOffset {
            get => Data.GetDouble(_pointsOffsetAddr);
            set => Data.SetDouble(_pointsOffsetAddr, value);
        }

        [TableViewModelColumn(displayOrder: 0, isPointer: true, minWidth: 80)]
        [BulkCopy]
        public int LinesOffset {
            get => Data.GetDouble(_linesOffsetAddr);
            set => Data.SetDouble(_linesOffsetAddr, value);
        }
    }
}
