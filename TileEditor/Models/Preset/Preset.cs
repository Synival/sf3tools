//using STHAEditor.Forms;
//using STHAEditor.Models;
using static STHAEditor.Forms.frmMain;

//using STHAEditor.Models.StatTypes;


namespace STHAEditor.Models.Presets
{
    public class Preset
    {
        //Item Tiles
        private int theItemIcon;

        private int offset;
        private int address;

        private int index;
        private string name;
        private int sub;

        private int theSpellIcon;
        private int realOffset;
        private int x0;
        private int x1;
        private int x2;
        private int x3;
        private int x4;
        private int x5;
        private int x6;
        private int x7;
        private int x8;
        private int x9;
        private int x10;
        private int x11;
        private int x12;
        private int x13;
        private int x14;
        private int x15;
        private int x16;
        private int x17;
        private int x18;
        private int x19;
        private int x20;
        private int x21;
        private int x22;
        private int x23;
        private int x24;
        private int x25;
        private int x26;
        private int x27;
        private int x28;
        private int x29;
        private int x30;
        private int x31;
        private int x32;
        private int x33;
        private int x34;
        private int x35;
        private int x36;
        private int x37;
        private int x38;
        private int x39;
        private int x40;
        private int x41;
        private int x42;
        private int x43;
        private int x44;
        private int x45;
        private int x46;
        private int x47;
        private int x48;
        private int x49;
        private int x50;
        private int x51;
        private int x52;
        private int x53;
        private int x54;
        private int x55;
        private int x56;
        private int x57;
        private int x58;
        private int x59;
        private int x60;
        private int x61;
        private int x62;
        private int x63;

        public Preset(int id, string text)
        {
            if (Globals.scenario == 1)
            {
            /*
            offset = 0x0000003C; //scn1 initial pointer
            sub = 0x06068000;
            offset = FileEditor.getDouble(offset);
            offset = offset - sub; //pointer*/

            /*
            offset = 0x00000018; //scn1 initial pointer
            npcOffset = offset;
            npcOffset = FileEditor.getDouble(offset);
            sub = 0x0605f000;
            offset = npcOffset - sub; //second pointer
            npcOffset = FileEditor.getDouble(offset);
            offset = npcOffset - sub; //third pointer
            //offset value should now point to where npc placements are
            */
            offset = 0x00006000;
            }
               

            index = id;
            name = text;






            //int start = 0x354c + (id * 24);

            //int start = offset + (id * 0x04);
            //theItemIcon = start; //1 bytes


            //address = offset + (id * 0x04);
            //address = 0x0354c + (id * 0x18);

            int start = offset + (id * 64);
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


            //theSpellIcon = start; //2 bytes  
            //unknown42 = start + 52;
            address = offset + (id * 64);





        }

        public int SizeID
        {
            get
            {
                return index;
            }
        }
        public string SizeName
        {
            get
            {
                return name;
            }
        }

        public int X0Treasure
        {
            get
            {
                return FileEditor.getByte(x0);
            }
            set
            {
                FileEditor.setByte(x0, (byte)value);
            }
        }

        public int X1Treasure
        {
            get
            {
                return FileEditor.getByte(x1);
            }
            set
            {
                FileEditor.setByte(x1, (byte)value);
            }
        }

        public int X2Treasure
        {
            get
            {
                return FileEditor.getByte(x2);
            }
            set
            {
                FileEditor.setByte(x2, (byte)value);
            }
        }

        public int X3Treasure
        {
            get
            {
                return FileEditor.getByte(x3);
            }
            set
            {
                FileEditor.setByte(x3, (byte)value);
            }
        }

        public int X4Treasure
        {
            get
            {
                return FileEditor.getByte(x4);
            }
            set
            {
                FileEditor.setByte(x4, (byte)value);
            }
        }

        public int X5Treasure
        {
            get
            {
                return FileEditor.getByte(x5);
            }
            set
            {
                FileEditor.setByte(x5, (byte)value);
            }
        }

        public int X6Treasure
        {
            get
            {
                return FileEditor.getByte(x6);
            }
            set
            {
                FileEditor.setByte(x6, (byte)value);
            }
        }

        public int X7Treasure
        {
            get
            {
                return FileEditor.getByte(x7);
            }
            set
            {
                FileEditor.setByte(x7, (byte)value);
            }
        }

        public int X8Treasure
        {
            get
            {
                return FileEditor.getByte(x8);
            }
            set
            {
                FileEditor.setByte(x8, (byte)value);
            }
        }

        public int X9Treasure
        {
            get
            {
                return FileEditor.getByte(x9);
            }
            set
            {
                FileEditor.setByte(x9, (byte)value);
            }
        }

        public int X10Treasure
        {
            get
            {
                return FileEditor.getByte(x10);
            }
            set
            {
                FileEditor.setByte(x10, (byte)value);
            }
        }

        public int X11Treasure
        {
            get
            {
                return FileEditor.getByte(x11);
            }
            set
            {
                FileEditor.setByte(x11, (byte)value);
            }
        }

        public int X12Treasure
        {
            get
            {
                return FileEditor.getByte(x12);
            }
            set
            {
                FileEditor.setByte(x12, (byte)value);
            }
        }

        public int X13Treasure
        {
            get
            {
                return FileEditor.getByte(x13);
            }
            set
            {
                FileEditor.setByte(x13, (byte)value);
            }
        }

        public int X14Treasure
        {
            get
            {
                return FileEditor.getByte(x14);
            }
            set
            {
                FileEditor.setByte(x14, (byte)value);
            }
        }

        public int X15Treasure
        {
            get
            {
                return FileEditor.getByte(x15);
            }
            set
            {
                FileEditor.setByte(x15, (byte)value);
            }
        }

        public int X16Treasure
        {
            get
            {
                return FileEditor.getByte(x16);
            }
            set
            {
                FileEditor.setByte(x16, (byte)value);
            }
        }

        public int X17Treasure
        {
            get
            {
                return FileEditor.getByte(x17);
            }
            set
            {
                FileEditor.setByte(x17, (byte)value);
            }
        }

        public int X18Treasure
        {
            get
            {
                return FileEditor.getByte(x18);
            }
            set
            {
                FileEditor.setByte(x18, (byte)value);
            }
        }

        public int X19Treasure
        {
            get
            {
                return FileEditor.getByte(x19);
            }
            set
            {
                FileEditor.setByte(x19, (byte)value);
            }
        }

        public int X20Treasure
        {
            get
            {
                return FileEditor.getByte(x20);
            }
            set
            {
                FileEditor.setByte(x20, (byte)value);
            }
        }

        public int X21Treasure
        {
            get
            {
                return FileEditor.getByte(x21);
            }
            set
            {
                FileEditor.setByte(x21, (byte)value);
            }
        }

        public int X22Treasure
        {
            get
            {
                return FileEditor.getByte(x22);
            }
            set
            {
                FileEditor.setByte(x22, (byte)value);
            }
        }

        public int X23Treasure
        {
            get
            {
                return FileEditor.getByte(x23);
            }
            set
            {
                FileEditor.setByte(x23, (byte)value);
            }
        }


        public int X24Treasure
        {
            get
            {
                return FileEditor.getByte(x24);
            }
            set
            {
                FileEditor.setByte(x24, (byte)value);
            }
        }

        public int X25Treasure
        {
            get
            {
                return FileEditor.getByte(x25);
            }
            set
            {
                FileEditor.setByte(x25, (byte)value);
            }
        }

        public int X26Treasure
        {
            get
            {
                return FileEditor.getByte(x26);
            }
            set
            {
                FileEditor.setByte(x26, (byte)value);
            }
        }

        public int X27Treasure
        {
            get
            {
                return FileEditor.getByte(x27);
            }
            set
            {
                FileEditor.setByte(x27, (byte)value);
            }
        }

        public int X28Treasure
        {
            get
            {
                return FileEditor.getByte(x28);
            }
            set
            {
                FileEditor.setByte(x28, (byte)value);
            }
        }

        public int X29Treasure
        {
            get
            {
                return FileEditor.getByte(x29);
            }
            set
            {
                FileEditor.setByte(x29, (byte)value);
            }
        }

        public int X30Treasure
        {
            get
            {
                return FileEditor.getByte(x30);
            }
            set
            {
                FileEditor.setByte(x30, (byte)value);
            }
        }

        public int X31Treasure
        {
            get
            {
                return FileEditor.getByte(x31);
            }
            set
            {
                FileEditor.setByte(x31, (byte)value);
            }
        }

        public int X32Treasure
        {
            get
            {
                return FileEditor.getByte(x32);
            }
            set
            {
                FileEditor.setByte(x32, (byte)value);
            }
        }

        public int X33Treasure
        {
            get
            {
                return FileEditor.getByte(x33);
            }
            set
            {
                FileEditor.setByte(x33, (byte)value);
            }
        }

        public int X34Treasure
        {
            get
            {
                return FileEditor.getByte(x34);
            }
            set
            {
                FileEditor.setByte(x34, (byte)value);
            }
        }

        public int X35Treasure
        {
            get
            {
                return FileEditor.getByte(x35);
            }
            set
            {
                FileEditor.setByte(x35, (byte)value);
            }
        }

        public int X36Treasure
        {
            get
            {
                return FileEditor.getByte(x36);
            }
            set
            {
                FileEditor.setByte(x36, (byte)value);
            }
        }

        public int X37Treasure
        {
            get
            {
                return FileEditor.getByte(x37);
            }
            set
            {
                FileEditor.setByte(x37, (byte)value);
            }
        }

        public int X38Treasure
        {
            get
            {
                return FileEditor.getByte(x38);
            }
            set
            {
                FileEditor.setByte(x38, (byte)value);
            }
        }

        public int X39Treasure
        {
            get
            {
                return FileEditor.getByte(x39);
            }
            set
            {
                FileEditor.setByte(x39, (byte)value);
            }
        }

        public int X40Treasure
        {
            get
            {
                return FileEditor.getByte(x40);
            }
            set
            {
                FileEditor.setByte(x40, (byte)value);
            }
        }

        public int X41Treasure
        {
            get
            {
                return FileEditor.getByte(x41);
            }
            set
            {
                FileEditor.setByte(x41, (byte)value);
            }
        }

        public int X42Treasure
        {
            get
            {
                return FileEditor.getByte(x42);
            }
            set
            {
                FileEditor.setByte(x42, (byte)value);
            }
        }

        public int X43Treasure
        {
            get
            {
                return FileEditor.getByte(x43);
            }
            set
            {
                FileEditor.setByte(x43, (byte)value);
            }
        }

        public int X44Treasure
        {
            get
            {
                return FileEditor.getByte(x44);
            }
            set
            {
                FileEditor.setByte(x44, (byte)value);
            }
        }

        public int X45Treasure
        {
            get
            {
                return FileEditor.getByte(x45);
            }
            set
            {
                FileEditor.setByte(x45, (byte)value);
            }
        }

        public int X46Treasure
        {
            get
            {
                return FileEditor.getByte(x46);
            }
            set
            {
                FileEditor.setByte(x46, (byte)value);
            }
        }

        public int X47Treasure
        {
            get
            {
                return FileEditor.getByte(x47);
            }
            set
            {
                FileEditor.setByte(x47, (byte)value);
            }
        }

        public int X48Treasure
        {
            get
            {
                return FileEditor.getByte(x48);
            }
            set
            {
                FileEditor.setByte(x48, (byte)value);
            }
        }

        public int X49Treasure
        {
            get
            {
                return FileEditor.getByte(x49);
            }
            set
            {
                FileEditor.setByte(x49, (byte)value);
            }
        }

        public int X50Treasure
        {
            get
            {
                return FileEditor.getByte(x50);
            }
            set
            {
                FileEditor.setByte(x50, (byte)value);
            }
        }

        public int X51Treasure
        {
            get
            {
                return FileEditor.getByte(x51);
            }
            set
            {
                FileEditor.setByte(x51, (byte)value);
            }
        }

        public int X52Treasure
        {
            get
            {
                return FileEditor.getByte(x52);
            }
            set
            {
                FileEditor.setByte(x52, (byte)value);
            }
        }

        public int X53Treasure
        {
            get
            {
                return FileEditor.getByte(x53);
            }
            set
            {
                FileEditor.setByte(x53, (byte)value);
            }
        }

        public int X54Treasure
        {
            get
            {
                return FileEditor.getByte(x54);
            }
            set
            {
                FileEditor.setByte(x54, (byte)value);
            }
        }

        public int X55Treasure
        {
            get
            {
                return FileEditor.getByte(x55);
            }
            set
            {
                FileEditor.setByte(x55, (byte)value);
            }
        }

        public int X56Treasure
        {
            get
            {
                return FileEditor.getByte(x56);
            }
            set
            {
                FileEditor.setByte(x56, (byte)value);
            }
        }

        public int X57Treasure
        {
            get
            {
                return FileEditor.getByte(x57);
            }
            set
            {
                FileEditor.setByte(x57, (byte)value);
            }
        }

        public int X58Treasure
        {
            get
            {
                return FileEditor.getByte(x58);
            }
            set
            {
                FileEditor.setByte(x58, (byte)value);
            }
        }

        public int X59Treasure
        {
            get
            {
                return FileEditor.getByte(x59);
            }
            set
            {
                FileEditor.setByte(x59, (byte)value);
            }
        }

        public int X60Treasure
        {
            get
            {
                return FileEditor.getByte(x60);
            }
            set
            {
                FileEditor.setByte(x60, (byte)value);
            }
        }

        public int X61Treasure
        {
            get
            {
                return FileEditor.getByte(x61);
            }
            set
            {
                FileEditor.setByte(x61, (byte)value);
            }
        }

        public int X62Treasure
        {
            get
            {
                return FileEditor.getByte(x62);
            }
            set
            {
                FileEditor.setByte(x62, (byte)value);
            }
        }

        public int X63Treasure
        {
            get
            {
                return FileEditor.getByte(x63);
            }
            set
            {
                FileEditor.setByte(x63, (byte)value);
            }
        }


        public int SizeAddress
        {
            get
            {
                return (address);
            }
        }



    }
}
