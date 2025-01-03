using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X013 {
    public class SpecialChance : Struct {
        private readonly int twoSpecials2;
        private readonly int threeSpecials3;
        private readonly int threeSpecials2;
        private readonly int fourSpecials4;
        private readonly int fourSpecials3;
        private readonly int fourSpecials2;

        public SpecialChance(IByteData data, int id, string name, int address, bool hasLargeTable)
        : base(data, id, name, address, hasLargeTable ? 0x4a : 0x3a) {
            if (hasLargeTable) {
                twoSpecials2   = Address + 0x01; // 1 byte
                threeSpecials3 = Address + 0x15; // 1 byte
                threeSpecials2 = Address + 0x1d; // 1 byte
                fourSpecials4  = Address + 0x31; // 1 byte
                fourSpecials3  = Address + 0x3d; // 1 byte
                fourSpecials2  = Address + 0x49; // 1 byte
            }
            else {
                twoSpecials2   = Address + 0x01; // 1 byte
                threeSpecials3 = Address + 0x0f; // 1 byte
                threeSpecials2 = Address + 0x19; // 1 byte
                fourSpecials4  = Address + 0x21; // 1 byte
                fourSpecials3  = Address + 0x2d; // 1 byte
                fourSpecials2  = Address + 0x39; // 1 byte
            }
        }

        [BulkCopy]
        public int TwoSpecials2 {
            get => Data.GetByte(twoSpecials2);
            set => Data.SetByte(twoSpecials2, (byte) value);
        }

        [BulkCopy]
        public int ThreeSpecials3 {
            get => Data.GetByte(threeSpecials3);
            set => Data.SetByte(threeSpecials3, (byte) value);
        }

        [BulkCopy]
        public int ThreeSpecials2 {
            get => Data.GetByte(threeSpecials2);
            set => Data.SetByte(threeSpecials2, (byte) value);
        }

        [BulkCopy]
        public int FourSpecials4 {
            get => Data.GetByte(fourSpecials4);
            set => Data.SetByte(fourSpecials4, (byte) value);
        }

        [BulkCopy]
        public int FourSpecials3 {
            get => Data.GetByte(fourSpecials3);
            set => Data.SetByte(fourSpecials3, (byte) value);
        }

        [BulkCopy]
        public int FourSpecials2 {
            get => Data.GetByte(fourSpecials2);
            set => Data.SetByte(fourSpecials2, (byte) value);
        }
    }
}
