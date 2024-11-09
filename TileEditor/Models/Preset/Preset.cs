using static STHAEditor.Forms.frmMain;

namespace STHAEditor.Models.Presets {
    public class Preset {
        //Item Tiles
        private readonly int theItemIcon;

        private readonly int offset;
        private readonly int sub;

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

        public Preset(int id, string text) {
            if (Globals.scenario == 1)
                offset = 0x00006000;

            SizeID = id;
            SizeName = text;
            SizeAddress = offset + (id * 64);

            var start = offset + (id * 64);
            x0 = start;
            x1 = start + 1;
            x2 = start + 2;
            x3 = start + 3;
            x4 = start + 4;
            x5 = start + 5;
            x6 = start + 6;
            x7 = start + 7;
            x8 = start + 8;
            x9 = start + 9;
            x10 = start + 10;
            x11 = start + 11;
            x12 = start + 12;
            x13 = start + 13;
            x14 = start + 14;
            x15 = start + 15;
            x16 = start + 16;
            x17 = start + 17;
            x18 = start + 18;
            x19 = start + 19;
            x20 = start + 20;
            x21 = start + 21;
            x22 = start + 22;
            x23 = start + 23;
            x24 = start + 24;
            x25 = start + 25;
            x26 = start + 26;
            x27 = start + 27;
            x28 = start + 28;
            x29 = start + 29;
            x30 = start + 30;
            x31 = start + 31;
            x32 = start + 32;
            x33 = start + 33;
            x34 = start + 34;
            x35 = start + 35;
            x36 = start + 36;
            x37 = start + 37;
            x38 = start + 38;
            x39 = start + 39;
            x40 = start + 40;
            x41 = start + 41;
            x42 = start + 42;
            x43 = start + 43;
            x44 = start + 44;
            x45 = start + 45;
            x46 = start + 46;
            x47 = start + 47;
            x48 = start + 48;
            x49 = start + 49;
            x50 = start + 50;
            x51 = start + 51;
            x52 = start + 52;
            x53 = start + 53;
            x54 = start + 54;
            x55 = start + 55;
            x56 = start + 56;
            x57 = start + 57;
            x58 = start + 58;
            x59 = start + 59;
            x60 = start + 60;
            x61 = start + 61;
            x62 = start + 62;
            x63 = start + 63;
        }

        public int SizeID { get; }
        public string SizeName { get; }
        public int SizeAddress { get; }

        public int X0Treasure {
            get => FileEditor.getByte(x0);
            set => FileEditor.setByte(x0, (byte) value);
        }

        public int X1Treasure {
            get => FileEditor.getByte(x1);
            set => FileEditor.setByte(x1, (byte) value);
        }

        public int X2Treasure {
            get => FileEditor.getByte(x2);
            set => FileEditor.setByte(x2, (byte) value);
        }

        public int X3Treasure {
            get => FileEditor.getByte(x3);
            set => FileEditor.setByte(x3, (byte) value);
        }

        public int X4Treasure {
            get => FileEditor.getByte(x4);
            set => FileEditor.setByte(x4, (byte) value);
        }

        public int X5Treasure {
            get => FileEditor.getByte(x5);
            set => FileEditor.setByte(x5, (byte) value);
        }

        public int X6Treasure {
            get => FileEditor.getByte(x6);
            set => FileEditor.setByte(x6, (byte) value);
        }

        public int X7Treasure {
            get => FileEditor.getByte(x7);
            set => FileEditor.setByte(x7, (byte) value);
        }

        public int X8Treasure {
            get => FileEditor.getByte(x8);
            set => FileEditor.setByte(x8, (byte) value);
        }

        public int X9Treasure {
            get => FileEditor.getByte(x9);
            set => FileEditor.setByte(x9, (byte) value);
        }

        public int X10Treasure {
            get => FileEditor.getByte(x10);
            set => FileEditor.setByte(x10, (byte) value);
        }

        public int X11Treasure {
            get => FileEditor.getByte(x11);
            set => FileEditor.setByte(x11, (byte) value);
        }

        public int X12Treasure {
            get => FileEditor.getByte(x12);
            set => FileEditor.setByte(x12, (byte) value);
        }

        public int X13Treasure {
            get => FileEditor.getByte(x13);
            set => FileEditor.setByte(x13, (byte) value);
        }

        public int X14Treasure {
            get => FileEditor.getByte(x14);
            set => FileEditor.setByte(x14, (byte) value);
        }

        public int X15Treasure {
            get => FileEditor.getByte(x15);
            set => FileEditor.setByte(x15, (byte) value);
        }

        public int X16Treasure {
            get => FileEditor.getByte(x16);
            set => FileEditor.setByte(x16, (byte) value);
        }

        public int X17Treasure {
            get => FileEditor.getByte(x17);
            set => FileEditor.setByte(x17, (byte) value);
        }

        public int X18Treasure {
            get => FileEditor.getByte(x18);
            set => FileEditor.setByte(x18, (byte) value);
        }

        public int X19Treasure {
            get => FileEditor.getByte(x19);
            set => FileEditor.setByte(x19, (byte) value);
        }

        public int X20Treasure {
            get => FileEditor.getByte(x20);
            set => FileEditor.setByte(x20, (byte) value);
        }

        public int X21Treasure {
            get => FileEditor.getByte(x21);
            set => FileEditor.setByte(x21, (byte) value);
        }

        public int X22Treasure {
            get => FileEditor.getByte(x22);
            set => FileEditor.setByte(x22, (byte) value);
        }

        public int X23Treasure {
            get => FileEditor.getByte(x23);
            set => FileEditor.setByte(x23, (byte) value);
        }

        public int X24Treasure {
            get => FileEditor.getByte(x24);
            set => FileEditor.setByte(x24, (byte) value);
        }

        public int X25Treasure {
            get => FileEditor.getByte(x25);
            set => FileEditor.setByte(x25, (byte) value);
        }

        public int X26Treasure {
            get => FileEditor.getByte(x26);
            set => FileEditor.setByte(x26, (byte) value);
        }

        public int X27Treasure {
            get => FileEditor.getByte(x27);
            set => FileEditor.setByte(x27, (byte) value);
        }

        public int X28Treasure {
            get => FileEditor.getByte(x28);
            set => FileEditor.setByte(x28, (byte) value);
        }

        public int X29Treasure {
            get => FileEditor.getByte(x29);
            set => FileEditor.setByte(x29, (byte) value);
        }

        public int X30Treasure {
            get => FileEditor.getByte(x30);
            set => FileEditor.setByte(x30, (byte) value);
        }

        public int X31Treasure {
            get => FileEditor.getByte(x31);
            set => FileEditor.setByte(x31, (byte) value);
        }

        public int X32Treasure {
            get => FileEditor.getByte(x32);
            set => FileEditor.setByte(x32, (byte) value);
        }

        public int X33Treasure {
            get => FileEditor.getByte(x33);
            set => FileEditor.setByte(x33, (byte) value);
        }

        public int X34Treasure {
            get => FileEditor.getByte(x34);
            set => FileEditor.setByte(x34, (byte) value);
        }

        public int X35Treasure {
            get => FileEditor.getByte(x35);
            set => FileEditor.setByte(x35, (byte) value);
        }

        public int X36Treasure {
            get => FileEditor.getByte(x36);
            set => FileEditor.setByte(x36, (byte) value);
        }

        public int X37Treasure {
            get => FileEditor.getByte(x37);
            set => FileEditor.setByte(x37, (byte) value);
        }

        public int X38Treasure {
            get => FileEditor.getByte(x38);
            set => FileEditor.setByte(x38, (byte) value);
        }

        public int X39Treasure {
            get => FileEditor.getByte(x39);
            set => FileEditor.setByte(x39, (byte) value);
        }

        public int X40Treasure {
            get => FileEditor.getByte(x40);
            set => FileEditor.setByte(x40, (byte) value);
        }

        public int X41Treasure {
            get => FileEditor.getByte(x41);
            set => FileEditor.setByte(x41, (byte) value);
        }

        public int X42Treasure {
            get => FileEditor.getByte(x42);
            set => FileEditor.setByte(x42, (byte) value);
        }

        public int X43Treasure {
            get => FileEditor.getByte(x43);
            set => FileEditor.setByte(x43, (byte) value);
        }

        public int X44Treasure {
            get => FileEditor.getByte(x44);
            set => FileEditor.setByte(x44, (byte) value);
        }

        public int X45Treasure {
            get => FileEditor.getByte(x45);
            set => FileEditor.setByte(x45, (byte) value);
        }

        public int X46Treasure {
            get => FileEditor.getByte(x46);
            set => FileEditor.setByte(x46, (byte) value);
        }

        public int X47Treasure {
            get => FileEditor.getByte(x47);
            set => FileEditor.setByte(x47, (byte) value);
        }

        public int X48Treasure {
            get => FileEditor.getByte(x48);
            set => FileEditor.setByte(x48, (byte) value);
        }

        public int X49Treasure {
            get => FileEditor.getByte(x49);
            set => FileEditor.setByte(x49, (byte) value);
        }

        public int X50Treasure {
            get => FileEditor.getByte(x50);
            set => FileEditor.setByte(x50, (byte) value);
        }

        public int X51Treasure {
            get => FileEditor.getByte(x51);
            set => FileEditor.setByte(x51, (byte) value);
        }

        public int X52Treasure {
            get => FileEditor.getByte(x52);
            set => FileEditor.setByte(x52, (byte) value);
        }

        public int X53Treasure {
            get => FileEditor.getByte(x53);
            set => FileEditor.setByte(x53, (byte) value);
        }

        public int X54Treasure {
            get => FileEditor.getByte(x54);
            set => FileEditor.setByte(x54, (byte) value);
        }

        public int X55Treasure {
            get => FileEditor.getByte(x55);
            set => FileEditor.setByte(x55, (byte) value);
        }

        public int X56Treasure {
            get => FileEditor.getByte(x56);
            set => FileEditor.setByte(x56, (byte) value);
        }

        public int X57Treasure {
            get => FileEditor.getByte(x57);
            set => FileEditor.setByte(x57, (byte) value);
        }

        public int X58Treasure {
            get => FileEditor.getByte(x58);
            set => FileEditor.setByte(x58, (byte) value);
        }

        public int X59Treasure {
            get => FileEditor.getByte(x59);
            set => FileEditor.setByte(x59, (byte) value);
        }

        public int X60Treasure {
            get => FileEditor.getByte(x60);
            set => FileEditor.setByte(x60, (byte) value);
        }

        public int X61Treasure {
            get => FileEditor.getByte(x61);
            set => FileEditor.setByte(x61, (byte) value);
        }

        public int X62Treasure {
            get => FileEditor.getByte(x62);
            set => FileEditor.setByte(x62, (byte) value);
        }

        public int X63Treasure {
            get => FileEditor.getByte(x63);
            set => FileEditor.setByte(x63, (byte) value);
        }
    }
}
