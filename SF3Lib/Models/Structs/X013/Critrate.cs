using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X013 {
    public class Critrate : Struct {
        private readonly int _noSpecialAddr;
        private readonly int _oneSpecialAddr;
        private readonly int _twoSpecialAddr;
        private readonly int _threeSpecialAddr;
        private readonly int _fourSpecialAddr;
        private readonly int _fiveSpecialAddr;

        public Critrate(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x08) {
            _noSpecialAddr    = Address;     // 1 byte
            _oneSpecialAddr   = Address + 1; // 1 byte
            _twoSpecialAddr   = Address + 2; // 1 byte
            _threeSpecialAddr = Address + 3; // 1 byte
            _fourSpecialAddr  = Address + 4;
            _fiveSpecialAddr  = Address + 5;
        }

        [TableViewModelColumn(addressField: nameof(_noSpecialAddr), displayOrder: 0, displayName: "Have 0 Specials")]
        [BulkCopy]
        public byte NoSpecial {
            get => Data.GetUInt8(_noSpecialAddr);
            set => Data.SetUInt8(_noSpecialAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_oneSpecialAddr), displayOrder: 1, displayName: "Have 1 Special")]
        [BulkCopy]
        public byte OneSpecial {
            get => Data.GetUInt8(_oneSpecialAddr);
            set => Data.SetUInt8(_oneSpecialAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_twoSpecialAddr), displayOrder: 2, displayName: "Have 2 Specials")]
        [BulkCopy]
        public byte TwoSpecial {
            get => Data.GetUInt8(_twoSpecialAddr);
            set => Data.SetUInt8(_twoSpecialAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_threeSpecialAddr), displayOrder: 3, displayName: "Have 3 Specials")]
        [BulkCopy]
        public byte ThreeSpecial {
            get => Data.GetUInt8(_threeSpecialAddr);
            set => Data.SetUInt8(_threeSpecialAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_fourSpecialAddr), displayOrder: 4, displayName: "Have 4 Specials")]
        [BulkCopy]
        public byte FourSpecial {
            get => Data.GetUInt8(_fourSpecialAddr);
            set => Data.SetUInt8(_fourSpecialAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_fiveSpecialAddr), displayOrder: 5, displayName: "Have 5 Specials")]
        [BulkCopy]
        public byte FiveSpecial {
            get => Data.GetUInt8(_fiveSpecialAddr);
            set => Data.SetUInt8(_fiveSpecialAddr, value);
        }
    }
}
