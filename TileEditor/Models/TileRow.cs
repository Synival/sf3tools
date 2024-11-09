using SF3.FileEditors;
using SF3.Models;

namespace SF3.TileEditor.Models {
    public class TileRow : Model {
        //TILES
        private readonly int theSpellIcon;
        private readonly int realOffset;
        private readonly int x0;
        private readonly int x1;
        private readonly int x2;
        private readonly int x3;
        private readonly int x4;
        private readonly int x5;
        private readonly int x6;
        private readonly int x7;
        private readonly int x8;
        private readonly int x9;
        private readonly int x10;
        private readonly int x11;
        private readonly int x12;
        private readonly int x13;
        private readonly int x14;
        private readonly int x15;
        private readonly int x16;
        private readonly int x17;
        private readonly int x18;
        private readonly int x19;
        private readonly int x20;
        private readonly int x21;
        private readonly int x22;
        private readonly int x23;
        private readonly int x24;
        private readonly int x25;
        private readonly int x26;
        private readonly int x27;
        private readonly int x28;
        private readonly int x29;
        private readonly int x30;
        private readonly int x31;
        private readonly int x32;
        private readonly int x33;
        private readonly int x34;
        private readonly int x35;
        private readonly int x36;
        private readonly int x37;
        private readonly int x38;
        private readonly int x39;
        private readonly int x40;
        private readonly int x41;
        private readonly int x42;
        private readonly int x43;
        private readonly int x44;
        private readonly int x45;
        private readonly int x46;
        private readonly int x47;
        private readonly int x48;
        private readonly int x49;
        private readonly int x50;
        private readonly int x51;
        private readonly int x52;
        private readonly int x53;
        private readonly int x54;
        private readonly int x55;
        private readonly int x56;
        private readonly int x57;
        private readonly int x58;
        private readonly int x59;
        private readonly int x60;
        private readonly int x61;
        private readonly int x62;
        private readonly int x63;
        private readonly int offset;
        private readonly int sub;

        public TileRow(IByteEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 128) {
            x0  = Address;
            x1  = Address + 2;
            x2  = Address + 4;
            x3  = Address + 6;
            x4  = Address + 8;
            x5  = Address + 10;
            x6  = Address + 12;
            x7  = Address + 14;
            x8  = Address + 16;
            x9  = Address + 18;
            x10 = Address + 20;
            x11 = Address + 22;
            x12 = Address + 24;
            x13 = Address + 26;
            x14 = Address + 28;
            x15 = Address + 30;
            x16 = Address + 32;
            x17 = Address + 34;
            x18 = Address + 36;
            x19 = Address + 38;
            x20 = Address + 40;
            x21 = Address + 42;
            x22 = Address + 44;
            x23 = Address + 46;
            x24 = Address + 48;
            x25 = Address + 50;
            x26 = Address + 52;
            x27 = Address + 54;
            x28 = Address + 56;
            x29 = Address + 58;
            x30 = Address + 60;
            x31 = Address + 62;
            x32 = Address + 64;
            x33 = Address + 66;
            x34 = Address + 68;
            x35 = Address + 70;
            x36 = Address + 72;
            x37 = Address + 74;
            x38 = Address + 76;
            x39 = Address + 78;
            x40 = Address + 80;
            x41 = Address + 82;
            x42 = Address + 84;
            x43 = Address + 86;
            x44 = Address + 88;
            x45 = Address + 90;
            x46 = Address + 92;
            x47 = Address + 94;
            x48 = Address + 96;
            x49 = Address + 98;
            x50 = Address + 100;
            x51 = Address + 102;
            x52 = Address + 104;
            x53 = Address + 106;
            x54 = Address + 108;
            x55 = Address + 110;
            x56 = Address + 112;
            x57 = Address + 114;
            x58 = Address + 116;
            x59 = Address + 118;
            x60 = Address + 120;
            x61 = Address + 122;
            x62 = Address + 124;
            x63 = Address + 126;
        }

        public int X0Tile {
            get => Editor.GetWord(x0);
            set => Editor.SetWord(x0, value);
        }

        public int X1Tile {
            get => Editor.GetWord(x1);
            set => Editor.SetWord(x1, value);
        }

        public int X2Tile {
            get => Editor.GetWord(x2);
            set => Editor.SetWord(x2, value);
        }

        public int X3Tile {
            get => Editor.GetWord(x3);
            set => Editor.SetWord(x3, value);
        }

        public int X4Tile {
            get => Editor.GetWord(x4);
            set => Editor.SetWord(x4, value);
        }

        public int X5Tile {
            get => Editor.GetWord(x5);
            set => Editor.SetWord(x5, value);
        }

        public int X6Tile {
            get => Editor.GetWord(x6);
            set => Editor.SetWord(x6, value);
        }

        public int X7Tile {
            get => Editor.GetWord(x7);
            set => Editor.SetWord(x7, value);
        }

        public int X8Tile {
            get => Editor.GetWord(x8);
            set => Editor.SetWord(x8, value);
        }

        public int X9Tile {
            get => Editor.GetWord(x9);
            set => Editor.SetWord(x9, value);
        }

        public int X10Tile {
            get => Editor.GetWord(x10);
            set => Editor.SetWord(x10, value);
        }

        public int X11Tile {
            get => Editor.GetWord(x11);
            set => Editor.SetWord(x11, value);
        }

        public int X12Tile {
            get => Editor.GetWord(x12);
            set => Editor.SetWord(x12, value);
        }

        public int X13Tile {
            get => Editor.GetWord(x13);
            set => Editor.SetWord(x13, value);
        }

        public int X14Tile {
            get => Editor.GetWord(x14);
            set => Editor.SetWord(x14, value);
        }

        public int X15Tile {
            get => Editor.GetWord(x15);
            set => Editor.SetWord(x15, value);
        }

        public int X16Tile {
            get => Editor.GetWord(x16);
            set => Editor.SetWord(x16, value);
        }

        public int X17Tile {
            get => Editor.GetWord(x17);
            set => Editor.SetWord(x17, value);
        }

        public int X18Tile {
            get => Editor.GetWord(x18);
            set => Editor.SetWord(x18, value);
        }

        public int X19Tile {
            get => Editor.GetWord(x19);
            set => Editor.SetWord(x19, value);
        }

        public int X20Tile {
            get => Editor.GetWord(x20);
            set => Editor.SetWord(x20, value);
        }

        public int X21Tile {
            get => Editor.GetWord(x21);
            set => Editor.SetWord(x21, value);
        }

        public int X22Tile {
            get => Editor.GetWord(x22);
            set => Editor.SetWord(x22, value);
        }

        public int X23Tile {
            get => Editor.GetWord(x23);
            set => Editor.SetWord(x23, value);
        }

        public int X24Tile {
            get => Editor.GetWord(x24);
            set => Editor.SetWord(x24, value);
        }

        public int X25Tile {
            get => Editor.GetWord(x25);
            set => Editor.SetWord(x25, value);
        }

        public int X26Tile {
            get => Editor.GetWord(x26);
            set => Editor.SetWord(x26, value);
        }

        public int X27Tile {
            get => Editor.GetWord(x27);
            set => Editor.SetWord(x27, value);
        }

        public int X28Tile {
            get => Editor.GetWord(x28);
            set => Editor.SetWord(x28, value);
        }

        public int X29Tile {
            get => Editor.GetWord(x29);
            set => Editor.SetWord(x29, value);
        }

        public int X30Tile {
            get => Editor.GetWord(x30);
            set => Editor.SetWord(x30, value);
        }

        public int X31Tile {
            get => Editor.GetWord(x31);
            set => Editor.SetWord(x31, value);
        }

        public int X32Tile {
            get => Editor.GetWord(x32);
            set => Editor.SetWord(x32, value);
        }

        public int X33Tile {
            get => Editor.GetWord(x33);
            set => Editor.SetWord(x33, value);
        }

        public int X34Tile {
            get => Editor.GetWord(x34);
            set => Editor.SetWord(x34, value);
        }

        public int X35Tile {
            get => Editor.GetWord(x35);
            set => Editor.SetWord(x35, value);
        }

        public int X36Tile {
            get => Editor.GetWord(x36);
            set => Editor.SetWord(x36, value);
        }

        public int X37Tile {
            get => Editor.GetWord(x37);
            set => Editor.SetWord(x37, value);
        }

        public int X38Tile {
            get => Editor.GetWord(x38);
            set => Editor.SetWord(x38, value);
        }

        public int X39Tile {
            get => Editor.GetWord(x39);
            set => Editor.SetWord(x39, value);
        }

        public int X40Tile {
            get => Editor.GetWord(x40);
            set => Editor.SetWord(x40, value);
        }

        public int X41Tile {
            get => Editor.GetWord(x41);
            set => Editor.SetWord(x41, value);
        }

        public int X42Tile {
            get => Editor.GetWord(x42);
            set => Editor.SetWord(x42, value);
        }

        public int X43Tile {
            get => Editor.GetWord(x43);
            set => Editor.SetWord(x43, value);
        }

        public int X44Tile {
            get => Editor.GetWord(x44);
            set => Editor.SetWord(x44, value);
        }

        public int X45Tile {
            get => Editor.GetWord(x45);
            set => Editor.SetWord(x45, value);
        }

        public int X46Tile {
            get => Editor.GetWord(x46);
            set => Editor.SetWord(x46, value);
        }

        public int X47Tile {
            get => Editor.GetWord(x47);
            set => Editor.SetWord(x47, value);
        }

        public int X48Tile {
            get => Editor.GetWord(x48);
            set => Editor.SetWord(x48, value);
        }

        public int X49Tile {
            get => Editor.GetWord(x49);
            set => Editor.SetWord(x49, value);
        }

        public int X50Tile {
            get => Editor.GetWord(x50);
            set => Editor.SetWord(x50, value);
        }

        public int X51Tile {
            get => Editor.GetWord(x51);
            set => Editor.SetWord(x51, value);
        }

        public int X52Tile {
            get => Editor.GetWord(x52);
            set => Editor.SetWord(x52, value);
        }

        public int X53Tile {
            get => Editor.GetWord(x53);
            set => Editor.SetWord(x53, value);
        }

        public int X54Tile {
            get => Editor.GetWord(x54);
            set => Editor.SetWord(x54, value);
        }

        public int X55Tile {
            get => Editor.GetWord(x55);
            set => Editor.SetWord(x55, value);
        }

        public int X56Tile {
            get => Editor.GetWord(x56);
            set => Editor.SetWord(x56, value);
        }

        public int X57Tile {
            get => Editor.GetWord(x57);
            set => Editor.SetWord(x57, value);
        }

        public int X58Tile {
            get => Editor.GetWord(x58);
            set => Editor.SetWord(x58, value);
        }

        public int X59Tile {
            get => Editor.GetWord(x59);
            set => Editor.SetWord(x59, value);
        }

        public int X60Tile {
            get => Editor.GetWord(x60);
            set => Editor.SetWord(x60, value);
        }

        public int X61Tile {
            get => Editor.GetWord(x61);
            set => Editor.SetWord(x61, value);
        }

        public int X62Tile {
            get => Editor.GetWord(x62);
            set => Editor.SetWord(x62, value);
        }

        public int X63Tile {
            get => Editor.GetWord(x63);
            set => Editor.SetWord(x63, value);
        }
    }
}
