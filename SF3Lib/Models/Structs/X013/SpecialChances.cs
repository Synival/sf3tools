using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X013 {
    public class SpecialChances : Struct {
        private readonly int _twoSpecials2Addr;
        private readonly int _threeSpecials3Addr;
        private readonly int _threeSpecials2Addr;
        private readonly int _fourSpecials4Addr;
        private readonly int _fourSpecials3Addr;
        private readonly int _fourSpecials2Addr;

        public SpecialChances(IByteData data, int id, string name, int address, bool hasLargeTable)
        : base(data, id, name, address, hasLargeTable ? 0x4a : 0x3a) {
            if (hasLargeTable) {
                _twoSpecials2Addr   = Address + 0x01; // 1 byte
                _threeSpecials3Addr = Address + 0x15; // 1 byte
                _threeSpecials2Addr = Address + 0x1d; // 1 byte
                _fourSpecials4Addr  = Address + 0x31; // 1 byte
                _fourSpecials3Addr  = Address + 0x3d; // 1 byte
                _fourSpecials2Addr  = Address + 0x49; // 1 byte
            }
            else {
                _twoSpecials2Addr   = Address + 0x01; // 1 byte
                _threeSpecials3Addr = Address + 0x0f; // 1 byte
                _threeSpecials2Addr = Address + 0x19; // 1 byte
                _fourSpecials4Addr  = Address + 0x21; // 1 byte
                _fourSpecials3Addr  = Address + 0x2d; // 1 byte
                _fourSpecials2Addr  = Address + 0x39; // 1 byte
            }
        }

        [TableViewModelColumn(addressField: nameof(_twoSpecials2Addr), displayOrder: 0)]
        [BulkCopy]
        public byte TwoSpecials2 {
            get => Data.GetUInt8(_twoSpecials2Addr);
            set => Data.SetUInt8(_twoSpecials2Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_threeSpecials3Addr), displayOrder: 1)]
        [BulkCopy]
        public byte ThreeSpecials3 {
            get => Data.GetUInt8(_threeSpecials3Addr);
            set => Data.SetUInt8(_threeSpecials3Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_threeSpecials2Addr), displayOrder: 2)]
        [BulkCopy]
        public byte ThreeSpecials2 {
            get => Data.GetUInt8(_threeSpecials2Addr);
            set => Data.SetUInt8(_threeSpecials2Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_fourSpecials4Addr), displayOrder: 3)]
        [BulkCopy]
        public byte FourSpecials4 {
            get => Data.GetUInt8(_fourSpecials4Addr);
            set => Data.SetUInt8(_fourSpecials4Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_fourSpecials3Addr), displayOrder: 4)]
        [BulkCopy]
        public byte FourSpecials3 {
            get => Data.GetUInt8(_fourSpecials3Addr);
            set => Data.SetUInt8(_fourSpecials3Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_fourSpecials2Addr), displayOrder: 5)]
        [BulkCopy]
        public byte FourSpecials2 {
            get => Data.GetUInt8(_fourSpecials2Addr);
            set => Data.SetUInt8(_fourSpecials2Addr, value);
        }
    }
}
