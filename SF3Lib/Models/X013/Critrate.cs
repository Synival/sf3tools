using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X013 {
    public class Critrate : Model {
        private readonly int noSpecial;
        private readonly int oneSpecial;
        private readonly int twoSpecial;
        private readonly int threeSpecial;
        private readonly int fourSpecial;
        private readonly int fiveSpecial;

        public Critrate(IByteEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x08) {
            noSpecial    = Address;     // 1 byte
            oneSpecial   = Address + 1; // 1 byte
            twoSpecial   = Address + 2; // 1 byte
            threeSpecial = Address + 3; // 1 byte
            fourSpecial  = Address + 4;
            fiveSpecial  = Address + 5;
        }

        [BulkCopy]
        public int NoSpecial {
            get => Editor.GetByte(noSpecial);
            set => Editor.SetByte(noSpecial, (byte) value);
        }

        [BulkCopy]
        public int OneSpecial {
            get => Editor.GetByte(oneSpecial);
            set => Editor.SetByte(oneSpecial, (byte) value);
        }

        [BulkCopy]
        public int TwoSpecial {
            get => Editor.GetByte(twoSpecial);
            set => Editor.SetByte(twoSpecial, (byte) value);
        }

        [BulkCopy]
        public int ThreeSpecial {
            get => Editor.GetByte(threeSpecial);
            set => Editor.SetByte(threeSpecial, (byte) value);
        }

        [BulkCopy]
        public int FourSpecial {
            get => Editor.GetByte(fourSpecial);
            set => Editor.SetByte(fourSpecial, (byte) value);
        }

        [BulkCopy]
        public int FiveSpecial {
            get => Editor.GetByte(fiveSpecial);
            set => Editor.SetByte(fiveSpecial, (byte) value);
        }
    }
}
