using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.RawData;

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

        [BulkCopy]
        public int NoSpecial {
            get => Data.GetByte(noSpecial);
            set => Data.SetByte(noSpecial, (byte) value);
        }

        [BulkCopy]
        public int OneSpecial {
            get => Data.GetByte(oneSpecial);
            set => Data.SetByte(oneSpecial, (byte) value);
        }

        [BulkCopy]
        public int TwoSpecial {
            get => Data.GetByte(twoSpecial);
            set => Data.SetByte(twoSpecial, (byte) value);
        }

        [BulkCopy]
        public int ThreeSpecial {
            get => Data.GetByte(threeSpecial);
            set => Data.SetByte(threeSpecial, (byte) value);
        }

        [BulkCopy]
        public int FourSpecial {
            get => Data.GetByte(fourSpecial);
            set => Data.SetByte(fourSpecial, (byte) value);
        }

        [BulkCopy]
        public int FiveSpecial {
            get => Data.GetByte(fiveSpecial);
            set => Data.SetByte(fiveSpecial, (byte) value);
        }
    }
}
