using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD.Model {
    public class CollisionLine : Struct {
        private readonly int _point1Addr;
        private readonly int _point2Addr;
        private readonly int _angleAddr;
        private readonly int _unknown1Addr;
        private readonly int _unknown2Addr;

        public CollisionLine(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x08) {
            _point1Addr   = Address + 0x00; // 2 bytes
            _point2Addr   = Address + 0x02; // 2 bytes
            _angleAddr    = Address + 0x04; // 2 bytes
            _unknown1Addr = Address + 0x06; // 1 byte
            _unknown2Addr = Address + 0x07; // 1 byte
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0, displayFormat: "X2")]
        public ushort Point1Index {
            get => (ushort) Data.GetWord(_point1Addr);
            set => Data.SetWord(_point1Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 1, displayFormat: "X2")]
        public ushort Point2Index {
            get => (ushort) Data.GetWord(_point2Addr);
            set => Data.SetWord(_point2Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 2, displayFormat: "X4")]
        public ushort Angle {
            get => (ushort) Data.GetWord(_angleAddr);
            set => Data.SetWord(_angleAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 3, displayFormat: "X2")]
        public byte Unknown1 {
            get => (byte) Data.GetByte(_unknown1Addr);
            set => Data.SetByte(_unknown1Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 4, displayFormat: "X2")]
        public byte Unknown2 {
            get => (byte) Data.GetByte(_unknown2Addr);
            set => Data.SetByte(_unknown2Addr, value);
        }
    }
}
