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

        [TableViewModelColumn(displayOrder: 0)]
        [BulkCopy]
        public int TwoSpecials2 {
            get => Data.GetByte(_twoSpecials2Addr);
            set => Data.SetByte(_twoSpecials2Addr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 1)]
        [BulkCopy]
        public int ThreeSpecials3 {
            get => Data.GetByte(_threeSpecials3Addr);
            set => Data.SetByte(_threeSpecials3Addr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 2)]
        [BulkCopy]
        public int ThreeSpecials2 {
            get => Data.GetByte(_threeSpecials2Addr);
            set => Data.SetByte(_threeSpecials2Addr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 3)]
        [BulkCopy]
        public int FourSpecials4 {
            get => Data.GetByte(_fourSpecials4Addr);
            set => Data.SetByte(_fourSpecials4Addr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 4)]
        [BulkCopy]
        public int FourSpecials3 {
            get => Data.GetByte(_fourSpecials3Addr);
            set => Data.SetByte(_fourSpecials3Addr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 5)]
        [BulkCopy]
        public int FourSpecials2 {
            get => Data.GetByte(_fourSpecials2Addr);
            set => Data.SetByte(_fourSpecials2Addr, (byte) value);
        }
    }
}
