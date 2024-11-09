using static STHAEditor.Forms.frmMain;

namespace STHAEditor.Models.Items {
    public class Item {
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

        public Item(int id, string text) {
            if (Globals.scenario == 1)
                offset = 0x00004000;

            ID = id;
            Name = text;

            var start = offset + (id * 128);
            x0 = start;
            x1 = start + 2;
            x2 = start + 4;
            x3 = start + 6;
            x4 = start + 8;
            x5 = start + 10;
            x6 = start + 12;
            x7 = start + 14;
            x8 = start + 16;
            x9 = start + 18;
            x10 = start + 20;
            x11 = start + 22;
            x12 = start + 24;
            x13 = start + 26;
            x14 = start + 28;
            x15 = start + 30;
            x16 = start + 32;
            x17 = start + 34;
            x18 = start + 36;
            x19 = start + 38;
            x20 = start + 40;
            x21 = start + 42;
            x22 = start + 44;
            x23 = start + 46;
            x24 = start + 48;
            x25 = start + 50;
            x26 = start + 52;
            x27 = start + 54;
            x28 = start + 56;
            x29 = start + 58;
            x30 = start + 60;
            x31 = start + 62;
            x32 = start + 64;
            x33 = start + 66;
            x34 = start + 68;
            x35 = start + 70;
            x36 = start + 72;
            x37 = start + 74;
            x38 = start + 76;
            x39 = start + 78;
            x40 = start + 80;
            x41 = start + 82;
            x42 = start + 84;
            x43 = start + 86;
            x44 = start + 88;
            x45 = start + 90;
            x46 = start + 92;
            x47 = start + 94;
            x48 = start + 96;
            x49 = start + 98;
            x50 = start + 100;
            x51 = start + 102;
            x52 = start + 104;
            x53 = start + 106;
            x54 = start + 108;
            x55 = start + 110;
            x56 = start + 112;
            x57 = start + 114;
            x58 = start + 116;
            x59 = start + 118;
            x60 = start + 120;
            x61 = start + 122;
            x62 = start + 124;
            x63 = start + 126;

            Address = offset + (id * 128);
        }

        public int ID { get; }
        public string Name { get; }
        public int Address { get; }

        public int X0Tile {
            get => FileEditor.getWord(x0);
            set => FileEditor.setWord(x0, value);
        }

        public int X1Tile {
            get => FileEditor.getWord(x1);
            set => FileEditor.setWord(x1, value);
        }

        public int X2Tile {
            get => FileEditor.getWord(x2);
            set => FileEditor.setWord(x2, value);
        }

        public int X3Tile {
            get => FileEditor.getWord(x3);
            set => FileEditor.setWord(x3, value);
        }

        public int X4Tile {
            get => FileEditor.getWord(x4);
            set => FileEditor.setWord(x4, value);
        }

        public int X5Tile {
            get => FileEditor.getWord(x5);
            set => FileEditor.setWord(x5, value);
        }

        public int X6Tile {
            get => FileEditor.getWord(x6);
            set => FileEditor.setWord(x6, value);
        }

        public int X7Tile {
            get => FileEditor.getWord(x7);
            set => FileEditor.setWord(x7, value);
        }

        public int X8Tile {
            get => FileEditor.getWord(x8);
            set => FileEditor.setWord(x8, value);
        }

        public int X9Tile {
            get => FileEditor.getWord(x9);
            set => FileEditor.setWord(x9, value);
        }

        public int X10Tile {
            get => FileEditor.getWord(x10);
            set => FileEditor.setWord(x10, value);
        }

        public int X11Tile {
            get => FileEditor.getWord(x11);
            set => FileEditor.setWord(x11, value);
        }

        public int X12Tile {
            get => FileEditor.getWord(x12);
            set => FileEditor.setWord(x12, value);
        }

        public int X13Tile {
            get => FileEditor.getWord(x13);
            set => FileEditor.setWord(x13, value);
        }

        public int X14Tile {
            get => FileEditor.getWord(x14);
            set => FileEditor.setWord(x14, value);
        }

        public int X15Tile {
            get => FileEditor.getWord(x15);
            set => FileEditor.setWord(x15, value);
        }

        public int X16Tile {
            get => FileEditor.getWord(x16);
            set => FileEditor.setWord(x16, value);
        }

        public int X17Tile {
            get => FileEditor.getWord(x17);
            set => FileEditor.setWord(x17, value);
        }

        public int X18Tile {
            get => FileEditor.getWord(x18);
            set => FileEditor.setWord(x18, value);
        }

        public int X19Tile {
            get => FileEditor.getWord(x19);
            set => FileEditor.setWord(x19, value);
        }

        public int X20Tile {
            get => FileEditor.getWord(x20);
            set => FileEditor.setWord(x20, value);
        }

        public int X21Tile {
            get => FileEditor.getWord(x21);
            set => FileEditor.setWord(x21, value);
        }

        public int X22Tile {
            get => FileEditor.getWord(x22);
            set => FileEditor.setWord(x22, value);
        }

        public int X23Tile {
            get => FileEditor.getWord(x23);
            set => FileEditor.setWord(x23, value);
        }

        public int X24Tile {
            get => FileEditor.getWord(x24);
            set => FileEditor.setWord(x24, value);
        }

        public int X25Tile {
            get => FileEditor.getWord(x25);
            set => FileEditor.setWord(x25, value);
        }

        public int X26Tile {
            get => FileEditor.getWord(x26);
            set => FileEditor.setWord(x26, value);
        }

        public int X27Tile {
            get => FileEditor.getWord(x27);
            set => FileEditor.setWord(x27, value);
        }

        public int X28Tile {
            get => FileEditor.getWord(x28);
            set => FileEditor.setWord(x28, value);
        }

        public int X29Tile {
            get => FileEditor.getWord(x29);
            set => FileEditor.setWord(x29, value);
        }

        public int X30Tile {
            get => FileEditor.getWord(x30);
            set => FileEditor.setWord(x30, value);
        }

        public int X31Tile {
            get => FileEditor.getWord(x31);
            set => FileEditor.setWord(x31, value);
        }

        public int X32Tile {
            get => FileEditor.getWord(x32);
            set => FileEditor.setWord(x32, value);
        }

        public int X33Tile {
            get => FileEditor.getWord(x33);
            set => FileEditor.setWord(x33, value);
        }

        public int X34Tile {
            get => FileEditor.getWord(x34);
            set => FileEditor.setWord(x34, value);
        }

        public int X35Tile {
            get => FileEditor.getWord(x35);
            set => FileEditor.setWord(x35, value);
        }

        public int X36Tile {
            get => FileEditor.getWord(x36);
            set => FileEditor.setWord(x36, value);
        }

        public int X37Tile {
            get => FileEditor.getWord(x37);
            set => FileEditor.setWord(x37, value);
        }

        public int X38Tile {
            get => FileEditor.getWord(x38);
            set => FileEditor.setWord(x38, value);
        }

        public int X39Tile {
            get => FileEditor.getWord(x39);
            set => FileEditor.setWord(x39, value);
        }

        public int X40Tile {
            get => FileEditor.getWord(x40);
            set => FileEditor.setWord(x40, value);
        }

        public int X41Tile {
            get => FileEditor.getWord(x41);
            set => FileEditor.setWord(x41, value);
        }

        public int X42Tile {
            get => FileEditor.getWord(x42);
            set => FileEditor.setWord(x42, value);
        }

        public int X43Tile {
            get => FileEditor.getWord(x43);
            set => FileEditor.setWord(x43, value);
        }

        public int X44Tile {
            get => FileEditor.getWord(x44);
            set => FileEditor.setWord(x44, value);
        }

        public int X45Tile {
            get => FileEditor.getWord(x45);
            set => FileEditor.setWord(x45, value);
        }

        public int X46Tile {
            get => FileEditor.getWord(x46);
            set => FileEditor.setWord(x46, value);
        }

        public int X47Tile {
            get => FileEditor.getWord(x47);
            set => FileEditor.setWord(x47, value);
        }

        public int X48Tile {
            get => FileEditor.getWord(x48);
            set => FileEditor.setWord(x48, value);
        }

        public int X49Tile {
            get => FileEditor.getWord(x49);
            set => FileEditor.setWord(x49, value);
        }

        public int X50Tile {
            get => FileEditor.getWord(x50);
            set => FileEditor.setWord(x50, value);
        }

        public int X51Tile {
            get => FileEditor.getWord(x51);
            set => FileEditor.setWord(x51, value);
        }

        public int X52Tile {
            get => FileEditor.getWord(x52);
            set => FileEditor.setWord(x52, value);
        }

        public int X53Tile {
            get => FileEditor.getWord(x53);
            set => FileEditor.setWord(x53, value);
        }

        public int X54Tile {
            get => FileEditor.getWord(x54);
            set => FileEditor.setWord(x54, value);
        }

        public int X55Tile {
            get => FileEditor.getWord(x55);
            set => FileEditor.setWord(x55, value);
        }

        public int X56Tile {
            get => FileEditor.getWord(x56);
            set => FileEditor.setWord(x56, value);
        }

        public int X57Tile {
            get => FileEditor.getWord(x57);
            set => FileEditor.setWord(x57, value);
        }

        public int X58Tile {
            get => FileEditor.getWord(x58);
            set => FileEditor.setWord(x58, value);
        }

        public int X59Tile {
            get => FileEditor.getWord(x59);
            set => FileEditor.setWord(x59, value);
        }

        public int X60Tile {
            get => FileEditor.getWord(x60);
            set => FileEditor.setWord(x60, value);
        }

        public int X61Tile {
            get => FileEditor.getWord(x61);
            set => FileEditor.setWord(x61, value);
        }

        public int X62Tile {
            get => FileEditor.getWord(x62);
            set => FileEditor.setWord(x62, value);
        }

        public int X63Tile {
            get => FileEditor.getWord(x63);
            set => FileEditor.setWord(x63, value);
        }
    }
}
