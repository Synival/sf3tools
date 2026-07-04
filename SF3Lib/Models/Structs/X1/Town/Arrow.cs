using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X1.Town {
    public class Arrow : Struct {
        private readonly int _unknown0x00Addr;    // 2 bytes
        private readonly int _textIDAddr;         // 2 bytes
        private readonly int _ifFlagOffAddr;      // 2 bytes
        private readonly int _pointToWarpMPDAddr; // 2 bytes
        private readonly int _unknown0x08Addr;    // 2 bytes
        private readonly int _unknown0x0AAddr;    // 2 bytes

        public Arrow(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x0c) {
            _unknown0x00Addr    = Address + 0x00; // 2 bytes
            _textIDAddr         = Address + 0x02; // 2 bytes
            _ifFlagOffAddr      = Address + 0x04; // 2 bytes
            _pointToWarpMPDAddr = Address + 0x06; // 2 bytes
            _unknown0x08Addr    = Address + 0x08; // 2 bytes
            _unknown0x0AAddr    = Address + 0x0a; // 2 bytes
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x00Addr), displayOrder: 0, displayName: "+0x00", displayFormat: "X2")]
        [BulkCopy]
        public ushort Unknown0x00 {
            get => Data.GetUInt16(_unknown0x00Addr);
            set => Data.SetUInt16(_unknown0x00Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_textIDAddr), displayOrder: 1, displayFormat: "X4")]
        [BulkCopy]
        public ushort TextID {
            get => Data.GetUInt16(_textIDAddr);
            set => Data.SetUInt16(_textIDAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_ifFlagOffAddr), displayOrder: 2, displayFormat: "X3")]
        [BulkCopy]
        [NameGetter(NamedValueType.GameFlag)]
        public ushort IfFlagOff {
            get => Data.GetUInt16(_ifFlagOffAddr);
            set => Data.SetUInt16(_ifFlagOffAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_pointToWarpMPDAddr), displayOrder: 3, displayFormat: "X2")]
        [BulkCopy]
        public ushort PointToWarpMPD {
            get => Data.GetUInt16(_pointToWarpMPDAddr);
            set => Data.SetUInt16(_pointToWarpMPDAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x08Addr), displayOrder: 4, displayName: "+0x08", displayFormat: "X2")]
        [BulkCopy]
        public ushort Unknown0x08 {
            get => Data.GetUInt16(_unknown0x08Addr);
            set => Data.SetUInt16(_unknown0x08Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x0AAddr), displayOrder: 5, displayName: "+0x0A", displayFormat: "X2")]
        [BulkCopy]
        public ushort Unknown0x0A {
            get => Data.GetUInt16(_unknown0x0AAddr);
            set => Data.SetUInt16(_unknown0x0AAddr, value);
        }
    }
}
