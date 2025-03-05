using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD.Model {
    public class CollisionLine : Struct {
        private readonly int _point1IdAddr;
        private readonly int _point2IdAddr;
        private readonly int _angleAddr;
        private readonly int _unknownAddr;

        public CollisionLine(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x08) {
            _point1IdAddr  = Address + 0x00; // 2 bytes
            _point2IdAddr  = Address + 0x02; // 2 bytes
            _angleAddr     = Address + 0x04; // 2 bytes
            _unknownAddr   = Address + 0x06; // 2 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0, displayFormat: "X2")]
        public ushort Point1IDIndex {
            get => (ushort) Data.GetWord(_point1IdAddr);
            set => Data.SetWord(_point1IdAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 1, displayFormat: "X2")]
        public ushort Point2IDIndex {
            get => (ushort) Data.GetWord(_point2IdAddr);
            set => Data.SetWord(_point2IdAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 2, displayFormat: "X4")]
        public ushort Angle {
            get => (ushort) Data.GetWord(_angleAddr);
            set => Data.SetWord(_angleAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 3, displayFormat: "X4")]
        public ushort Unknown {
            get => (ushort) Data.GetWord(_unknownAddr);
            set => Data.SetWord(_unknownAddr, value);
        }
    }
}
