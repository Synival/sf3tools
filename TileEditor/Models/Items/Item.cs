//using STHAEditor.Forms;
//using STHAEditor.Models;
using static STHAEditor.Forms.frmMain;

//using STHAEditor.Models.StatTypes;


namespace STHAEditor.Models.Items
{
    public class Item
    {

        //TILES
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



        //int pointerValue;

        private int address;
        //private int npcOffset;
        private int offset;
        private int sub;

        private int index;
        private string name;

        /*public int NPCTableAddress1
        {
            get => FileEditor.getDouble(npcOffset);
            set => FileEditor.setDouble(npcOffset, value);
        }

        public int NPCTableAddress2 => FileEditor.getDouble(NPCTableAddress1 - 0x0605F000);

        public int NPCTableAddress3 => FileEditor.getDouble(NPCTableAddress2 - 0x0605F000);*/







        public Item(int id, string text)
        {
            if (Globals.scenario == 1)
            {
                /*offset = 0x00000030; //scn1 initial pointer
                sub = 0x06068000;
                offset = FileEditor.getDouble(offset);
                offset = offset - sub; //pointer

                realOffset = 0xFF8E;*/
                offset = 0x00004000;
            }



            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;






            //int start = 0x354c + (id * 24);

            int start = offset + (id * 128);
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
            

            //theSpellIcon = start; //2 bytes  
            //unknown42 = start + 52;
            address = offset + (id * 128);
            //address = 0x0354c + (id * 0x18);

        }

        public int ID
        {
            get
            {
                return index;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
        }

        public int X0Tile
        {
            get
            {
                return FileEditor.getWord(x0);
            }
            set
            {
                FileEditor.setWord(x0, value);
            }
        }

        public int X1Tile
        {
            get
            {
                return FileEditor.getWord(x1);
            }
            set
            {
                FileEditor.setWord(x1, value);
            }
        }

        public int X2Tile
        {
            get
            {
                return FileEditor.getWord(x2);
            }
            set
            {
                FileEditor.setWord(x2, value);
            }
        }

        public int X3Tile
        {
            get
            {
                return FileEditor.getWord(x3);
            }
            set
            {
                FileEditor.setWord(x3, value);
            }
        }

        public int X4Tile
        {
            get
            {
                return FileEditor.getWord(x4);
            }
            set
            {
                FileEditor.setWord(x4, value);
            }
        }

        public int X5Tile
        {
            get
            {
                return FileEditor.getWord(x5);
            }
            set
            {
                FileEditor.setWord(x5, value);
            }
        }

        public int X6Tile
        {
            get
            {
                return FileEditor.getWord(x6);
            }
            set
            {
                FileEditor.setWord(x6, value);
            }
        }

        public int X7Tile
        {
            get
            {
                return FileEditor.getWord(x7);
            }
            set
            {
                FileEditor.setWord(x7, value);
            }
        }

        public int X8Tile
        {
            get
            {
                return FileEditor.getWord(x8);
            }
            set
            {
                FileEditor.setWord(x8, value);
            }
        }

        public int X9Tile
        {
            get
            {
                return FileEditor.getWord(x9);
            }
            set
            {
                FileEditor.setWord(x9, value);
            }
        }

        public int X10Tile
        {
            get
            {
                return FileEditor.getWord(x10);
            }
            set
            {
                FileEditor.setWord(x10, value);
            }
        }

        public int X11Tile
        {
            get
            {
                return FileEditor.getWord(x11);
            }
            set
            {
                FileEditor.setWord(x11, value);
            }
        }

        public int X12Tile
        {
            get
            {
                return FileEditor.getWord(x12);
            }
            set
            {
                FileEditor.setWord(x12, value);
            }
        }

        public int X13Tile
        {
            get
            {
                return FileEditor.getWord(x13);
            }
            set
            {
                FileEditor.setWord(x13, value);
            }
        }

        public int X14Tile
        {
            get
            {
                return FileEditor.getWord(x14);
            }
            set
            {
                FileEditor.setWord(x14, value);
            }
        }

        public int X15Tile
        {
            get
            {
                return FileEditor.getWord(x15);
            }
            set
            {
                FileEditor.setWord(x15, value);
            }
        }

        public int X16Tile
        {
            get
            {
                return FileEditor.getWord(x16);
            }
            set
            {
                FileEditor.setWord(x16, value);
            }
        }

        public int X17Tile
        {
            get
            {
                return FileEditor.getWord(x17);
            }
            set
            {
                FileEditor.setWord(x17, value);
            }
        }

        public int X18Tile
        {
            get
            {
                return FileEditor.getWord(x18);
            }
            set
            {
                FileEditor.setWord(x18, value);
            }
        }

        public int X19Tile
        {
            get
            {
                return FileEditor.getWord(x19);
            }
            set
            {
                FileEditor.setWord(x19, value);
            }
        }

        public int X20Tile
        {
            get
            {
                return FileEditor.getWord(x20);
            }
            set
            {
                FileEditor.setWord(x20, value);
            }
        }

        public int X21Tile
        {
            get
            {
                return FileEditor.getWord(x21);
            }
            set
            {
                FileEditor.setWord(x21, value);
            }
        }

        public int X22Tile
        {
            get
            {
                return FileEditor.getWord(x22);
            }
            set
            {
                FileEditor.setWord(x22, value);
            }
        }

        public int X23Tile
        {
            get
            {
                return FileEditor.getWord(x23);
            }
            set
            {
                FileEditor.setWord(x23, value);
            }
        }

        public int X24Tile
        {
            get
            {
                return FileEditor.getWord(x24);
            }
            set
            {
                FileEditor.setWord(x24, value);
            }
        }

        public int X25Tile
        {
            get
            {
                return FileEditor.getWord(x25);
            }
            set
            {
                FileEditor.setWord(x25, value);
            }
        }

        public int X26Tile
        {
            get
            {
                return FileEditor.getWord(x26);
            }
            set
            {
                FileEditor.setWord(x26, value);
            }
        }

        public int X27Tile
        {
            get
            {
                return FileEditor.getWord(x27);
            }
            set
            {
                FileEditor.setWord(x27, value);
            }
        }

        public int X28Tile
        {
            get
            {
                return FileEditor.getWord(x28);
            }
            set
            {
                FileEditor.setWord(x28, value);
            }
        }

        public int X29Tile
        {
            get
            {
                return FileEditor.getWord(x29);
            }
            set
            {
                FileEditor.setWord(x29, value);
            }
        }

        public int X30Tile
        {
            get
            {
                return FileEditor.getWord(x30);
            }
            set
            {
                FileEditor.setWord(x30, value);
            }
        }

        public int X31Tile
        {
            get
            {
                return FileEditor.getWord(x31);
            }
            set
            {
                FileEditor.setWord(x31, value);
            }
        }

        public int X32Tile
        {
            get
            {
                return FileEditor.getWord(x32);
            }
            set
            {
                FileEditor.setWord(x32, value);
            }
        }

        public int X33Tile
        {
            get
            {
                return FileEditor.getWord(x33);
            }
            set
            {
                FileEditor.setWord(x33, value);
            }
        }

        public int X34Tile
        {
            get
            {
                return FileEditor.getWord(x34);
            }
            set
            {
                FileEditor.setWord(x34, value);
            }
        }


        public int X35Tile
        {
            get
            {
                return FileEditor.getWord(x35);
            }
            set
            {
                FileEditor.setWord(x35, value);
            }
        }

        public int X36Tile
        {
            get
            {
                return FileEditor.getWord(x36);
            }
            set
            {
                FileEditor.setWord(x36, value);
            }
        }

        public int X37Tile
        {
            get
            {
                return FileEditor.getWord(x37);
            }
            set
            {
                FileEditor.setWord(x37, value);
            }
        }

        public int X38Tile
        {
            get
            {
                return FileEditor.getWord(x38);
            }
            set
            {
                FileEditor.setWord(x38, value);
            }
        }

        public int X39Tile
        {
            get
            {
                return FileEditor.getWord(x39);
            }
            set
            {
                FileEditor.setWord(x39, value);
            }
        }

        public int X40Tile
        {
            get
            {
                return FileEditor.getWord(x40);
            }
            set
            {
                FileEditor.setWord(x40, value);
            }
        }

        public int X41Tile
        {
            get
            {
                return FileEditor.getWord(x41);
            }
            set
            {
                FileEditor.setWord(x41, value);
            }
        }

        public int X42Tile
        {
            get
            {
                return FileEditor.getWord(x42);
            }
            set
            {
                FileEditor.setWord(x42, value);
            }
        }

        public int X43Tile
        {
            get
            {
                return FileEditor.getWord(x43);
            }
            set
            {
                FileEditor.setWord(x43, value);
            }
        }

        public int X44Tile
        {
            get
            {
                return FileEditor.getWord(x44);
            }
            set
            {
                FileEditor.setWord(x44, value);
            }
        }

        public int X45Tile
        {
            get
            {
                return FileEditor.getWord(x45);
            }
            set
            {
                FileEditor.setWord(x45, value);
            }
        }

        public int X46Tile
        {
            get
            {
                return FileEditor.getWord(x46);
            }
            set
            {
                FileEditor.setWord(x46, value);
            }
        }

        public int X47Tile
        {
            get
            {
                return FileEditor.getWord(x47);
            }
            set
            {
                FileEditor.setWord(x47, value);
            }
        }

        public int X48Tile
        {
            get
            {
                return FileEditor.getWord(x48);
            }
            set
            {
                FileEditor.setWord(x48, value);
            }
        }

        public int X49Tile
        {
            get
            {
                return FileEditor.getWord(x49);
            }
            set
            {
                FileEditor.setWord(x49, value);
            }
        }

        public int X50Tile
        {
            get
            {
                return FileEditor.getWord(x50);
            }
            set
            {
                FileEditor.setWord(x50, value);
            }
        }

        public int X51Tile
        {
            get
            {
                return FileEditor.getWord(x51);
            }
            set
            {
                FileEditor.setWord(x51, value);
            }
        }

        public int X52Tile
        {
            get
            {
                return FileEditor.getWord(x52);
            }
            set
            {
                FileEditor.setWord(x52, value);
            }
        }

        public int X53Tile
        {
            get
            {
                return FileEditor.getWord(x53);
            }
            set
            {
                FileEditor.setWord(x53, value);
            }
        }

        public int X54Tile
        {
            get
            {
                return FileEditor.getWord(x54);
            }
            set
            {
                FileEditor.setWord(x54, value);
            }
        }

        public int X55Tile
        {
            get
            {
                return FileEditor.getWord(x55);
            }
            set
            {
                FileEditor.setWord(x55, value);
            }
        }

        public int X56Tile
        {
            get
            {
                return FileEditor.getWord(x56);
            }
            set
            {
                FileEditor.setWord(x56, value);
            }
        }

        public int X57Tile
        {
            get
            {
                return FileEditor.getWord(x57);
            }
            set
            {
                FileEditor.setWord(x57, value);
            }
        }

        public int X58Tile
        {
            get
            {
                return FileEditor.getWord(x58);
            }
            set
            {
                FileEditor.setWord(x58, value);
            }
        }

        public int X59Tile
        {
            get
            {
                return FileEditor.getWord(x59);
            }
            set
            {
                FileEditor.setWord(x59, value);
            }
        }

        public int X60Tile
        {
            get
            {
                return FileEditor.getWord(x60);
            }
            set
            {
                FileEditor.setWord(x60, value);
            }
        }

        public int X61Tile
        {
            get
            {
                return FileEditor.getWord(x61);
            }
            set
            {
                FileEditor.setWord(x61, value);
            }
        }

        public int X62Tile
        {
            get
            {
                return FileEditor.getWord(x62);
            }
            set
            {
                FileEditor.setWord(x62, value);
            }
        }

        public int X63Tile
        {
            get
            {
                return FileEditor.getWord(x63);
            }
            set
            {
                FileEditor.setWord(x63, value);
            }
        }

        public int Address
        {
            get
            {
                return (address);
            }
        }



    }
}
