using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.MPD.Model {
    public class CollisionLine : Struct {
        private readonly int _point1Addr;
        private readonly int _point2Addr;
        private readonly int _angleAddr;
        private readonly int _unknown0x06Addr;
        private readonly int _ifFlag2XXOffAddr;

        public CollisionLine(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x08) {
            _point1Addr   = Address + 0x00; // 2 bytes
            _point2Addr   = Address + 0x02; // 2 bytes
            _angleAddr    = Address + 0x04; // 2 bytes
            _unknown0x06Addr = Address + 0x06; // 1 byte
            _ifFlag2XXOffAddr = Address + 0x07; // 1 byte
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_point1Addr), displayOrder: 0, displayFormat: "X2")]
        public ushort Point1Index {
            get => (ushort) Data.GetWord(_point1Addr);
            set => Data.SetWord(_point1Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_point2Addr), displayOrder: 1, displayFormat: "X2")]
        public ushort Point2Index {
            get => (ushort) Data.GetWord(_point2Addr);
            set => Data.SetWord(_point2Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_angleAddr), displayOrder: 2, displayFormat: "X4")]
        public ushort Angle {
            get => (ushort) Data.GetWord(_angleAddr);
            set => Data.SetWord(_angleAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_unknown0x06Addr), displayOrder: 3, displayName: "+0x06 (Order?)", displayFormat: "X2")]
        public byte Unknown0x06 {
            get => (byte) Data.GetByte(_unknown0x06Addr);
            set => Data.SetByte(_unknown0x06Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_ifFlag2XXOffAddr), displayOrder: 4, displayFormat: "X2")]
        public byte IfFlagIn2XXOff {
            get => (byte) Data.GetByte(_ifFlag2XXOffAddr);
            set => Data.SetByte(_ifFlag2XXOffAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: null, displayOrder: 4.1f, displayFormat: "X3", minWidth: 200)]
        [NameGetter(NamedValueType.GameFlag)]
        public int? IfFlagOff {
            get {
                var flag200 = IfFlagIn2XXOff;
                return (flag200 == 0) ? (int?) null : flag200 + 0x200;
            }
        }
    }
}
