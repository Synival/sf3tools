using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X013 {
    public class Critrate : Struct {
        private readonly int noSpecial;
        private readonly int oneSpecial;
        private readonly int twoSpecial;
        private readonly int threeSpecial;
        private readonly int fourSpecial;
        private readonly int fiveSpecial;

        public Critrate(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x08) {
            noSpecial    = Address;     // 1 byte
            oneSpecial   = Address + 1; // 1 byte
            twoSpecial   = Address + 2; // 1 byte
            threeSpecial = Address + 3; // 1 byte
            fourSpecial  = Address + 4;
            fiveSpecial  = Address + 5;
        }

        [TableViewModelColumn(displayOrder: 0, displayName: "Have 0 Specials")]
        [BulkCopy]
        public int NoSpecial {
            get => Data.GetByte(noSpecial);
            set => Data.SetByte(noSpecial, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 1, displayName: "Have 1 Special")]
        [BulkCopy]
        public int OneSpecial {
            get => Data.GetByte(oneSpecial);
            set => Data.SetByte(oneSpecial, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 2, displayName: "Have 2 Specials")]
        [BulkCopy]
        public int TwoSpecial {
            get => Data.GetByte(twoSpecial);
            set => Data.SetByte(twoSpecial, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 3, displayName: "Have 3 Specials")]
        [BulkCopy]
        public int ThreeSpecial {
            get => Data.GetByte(threeSpecial);
            set => Data.SetByte(threeSpecial, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 4, displayName: "Have 4 Specials")]
        [BulkCopy]
        public int FourSpecial {
            get => Data.GetByte(fourSpecial);
            set => Data.SetByte(fourSpecial, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 5, displayName: "Have 5 Specials")]
        [BulkCopy]
        public int FiveSpecial {
            get => Data.GetByte(fiveSpecial);
            set => Data.SetByte(fiveSpecial, (byte) value);
        }
    }
}
