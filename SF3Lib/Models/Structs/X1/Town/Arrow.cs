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
        public int Unknown0x00 {
            get => Data.GetWord(_unknown0x00Addr);
            set => Data.SetWord(_unknown0x00Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_textIDAddr), displayOrder: 1, displayFormat: "X4")]
        [BulkCopy]
        public int TextID {
            get => Data.GetWord(_textIDAddr);
            set => Data.SetWord(_textIDAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_ifFlagOffAddr), displayOrder: 2, displayFormat: "X3")]
        [BulkCopy]
        [NameGetter(NamedValueType.GameFlag)]
        public int IfFlagOff {
            get => Data.GetWord(_ifFlagOffAddr);
            set => Data.SetWord(_ifFlagOffAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_pointToWarpMPDAddr), displayOrder: 3, displayFormat: "X2")]
        [BulkCopy]
        public int PointToWarpMPD {
            get => Data.GetWord(_pointToWarpMPDAddr);
            set => Data.SetWord(_pointToWarpMPDAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x08Addr), displayOrder: 4, displayName: "+0x08", displayFormat: "X2")]
        [BulkCopy]
        public int Unknown0x08 {
            get => Data.GetWord(_unknown0x08Addr);
            set => Data.SetWord(_unknown0x08Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x0AAddr), displayOrder: 5, displayName: "+0x0A", displayFormat: "X2")]
        [BulkCopy]
        public int Unknown0x0A {
            get => Data.GetWord(_unknown0x0AAddr);
            set => Data.SetWord(_unknown0x0AAddr, value);
        }
    }
}
