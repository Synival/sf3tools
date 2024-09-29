//using STHAEditor.Forms;
//using STHAEditor.Models;
using static STHAEditor.Forms.frmMain;

//using STHAEditor.Models.StatTypes;


namespace STHAEditor.Models.UnknownAI
{
    public class UnknownAI
    {
        private int unknown00;
        private int unknown02;
        private int unknown04;
        private int unknown06;
        private int unknown08;
        private int unknown0A;
        private int unknown0C;
        private int unknown0E;
        private int unknown10;
        //private int unknown42;


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







        public UnknownAI(int id, string text)
        {
            if (Globals.scenario == 1)
            {
                offset = 0x00000018; //scn1 initial pointer
                sub = 0x0605f000;
                offset = FileEditor.getDouble(offset);
                offset = offset - sub; //first pointer
                offset = FileEditor.getDouble(offset);
                offset = offset - sub + Globals.map; //second pointer


                offset = FileEditor.getDouble(offset);

                if(offset != 0)
                {
                    offset = offset - sub; //third pointer

                    offset = offset + 10;
                    offset = offset + 0xEa0;
                }
                else
                {
                    Globals.map = 0;
                    offset = 0x00000018; //scn1 initial pointer
                    sub = 0x0605f000;
                    offset = FileEditor.getDouble(offset);
                    offset = offset - sub; //first pointer
                    offset = FileEditor.getDouble(offset);
                    offset = offset - sub + Globals.map; //second pointer
                    offset = FileEditor.getDouble(offset);
                    offset = offset - sub; //third pointer

                    offset = offset + 10;
                    offset = offset + 0xEa0;
                }



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
            }
            else if (Globals.scenario == 2)
            {

                offset = 0x00000024; //scn2 initial pointer
                sub = 0x0605e000;
                offset = FileEditor.getDouble(offset);
                offset = offset - sub; //first pointer
                offset = FileEditor.getDouble(offset);
                offset = offset - sub + Globals.map; //second pointer

                offset = FileEditor.getDouble(offset);
                if (offset != 0)
                {
                    offset = offset - sub; //third pointer

                    offset = offset + 10;
                    offset = offset + 0xa90;
                }
                else
                {
                    Globals.map = 4;
                    offset = 0x00000024; //scn2 initial pointer
                    sub = 0x0605e000;
                    offset = FileEditor.getDouble(offset);
                    offset = offset - sub; //first pointer
                    offset = FileEditor.getDouble(offset);
                    offset = offset - sub + Globals.map; //second pointer
                    offset = FileEditor.getDouble(offset);
                    offset = offset - sub; //third pointer

                    offset = offset + 10;
                    offset = offset + 0xa90;
                }

                /*offset = 0x00000024; //scn2 initial pointer
                npcOffset = offset;
                npcOffset = FileEditor.getDouble(offset);
                sub = 0x0605e000;
                offset = npcOffset - sub + 4; //second pointer
                npcOffset = FileEditor.getDouble(offset);
                offset = npcOffset - sub; //third pointer
                //offset value should now point to where npc placements are
                */

            }
            else if (Globals.scenario == 3)
            {
                offset = 0x00000024; //scn3 initial pointer
                sub = 0x0605e000;
                offset = FileEditor.getDouble(offset);
                offset = offset - sub; //first pointer
                offset = FileEditor.getDouble(offset);
                offset = offset - sub + Globals.map; //second pointer

                offset = FileEditor.getDouble(offset);
                if (offset != 0)
                {
                    offset = offset - sub; //third pointer

                    offset = offset + 10;
                    offset = offset + 0xa90;
                }
                else
                {
                    Globals.map = 8;
                    offset = 0x00000024; //scn3 initial pointer
                    sub = 0x0605e000;
                    offset = FileEditor.getDouble(offset);
                    offset = offset - sub; //first pointer
                    offset = FileEditor.getDouble(offset);
                    offset = offset - sub + Globals.map; //second pointer
                    offset = FileEditor.getDouble(offset);
                    offset = offset - sub; //third pointer

                    offset = offset + 10;
                    offset = offset + 0xa90;
                }
            }
            else if (Globals.scenario == 4)
            {
                offset = 0x00000024; //pd initial pointer
                sub = 0x0605e000;
                offset = FileEditor.getDouble(offset);
                offset = offset - sub; //first pointer
                offset = FileEditor.getDouble(offset);
                offset = offset - sub + Globals.map; //second pointer
                offset = FileEditor.getDouble(offset);
                if (offset != 0)
                {
                    offset = offset - sub; //third pointer

                    offset = offset + 10;
                    offset = offset + 0xa90;
                }
                else
                {
                    Globals.map = 0;
                    offset = 0x00000024; //pd initial pointer
                    sub = 0x0605e000;
                    offset = FileEditor.getDouble(offset);
                    offset = offset - sub; //first pointer
                    offset = FileEditor.getDouble(offset);
                    offset = offset - sub + Globals.map; //second pointer
                    offset = FileEditor.getDouble(offset);
                    offset = offset - sub; //third pointer

                    offset = offset + 10;
                    offset = offset + 0xa90;
                }
            }
            else if (Globals.scenario == 5)
            {
                offset = 0x00000018; //BTL99 initial pointer
                sub = 0x06060000;
                offset = FileEditor.getDouble(offset);
                offset = offset - sub; //first pointer
                offset = FileEditor.getDouble(offset);
                offset = offset - sub; //second pointer
                offset = FileEditor.getDouble(offset);
                offset = offset - sub; //third pointer

                offset = offset + 10;
                offset = offset + 0xea0;
            }



            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;






            //int start = 0x354c + (id * 24);

            int start = offset + (id * 0x12);
            unknown00 = start; //2 bytes  
            unknown02 = start + 2; //2 byte
            unknown04 = start + 4; //2 byte
            unknown06 = start + 6; //2 byte
            unknown08 = start + 8; 
            unknown0A = start + 0x0a; 
            unknown0C = start + 0x0c; //2 byte
            unknown0E = start + 0x0e; 
            unknown10 = start + 0x10;
            //unknown42 = start + 52;
            address = offset + (id * 0x12);
            //address = 0x0354c + (id * 0x18);

        }

        public int UnknownAIID
        {
            get
            {
                return index;
            }
        }
        public string UnknownAIName
        {
            get
            {
                return name;
            }
        }

        public int UnknownAI00
        {
            get
            {
                return FileEditor.getWord(unknown00);
            }
            set
            {
                FileEditor.setWord(unknown00, value);
            }
        }

        public int UnknownAI02
        {
            get
            {
                return FileEditor.getWord(unknown02);
            }
            set
            {
                FileEditor.setWord(unknown02, value);
            }
        }

        public int UnknownAI04
        {
            get
            {
                return FileEditor.getWord(unknown04);
            }
            set
            {
                FileEditor.setWord(unknown04, value);
            }
        }

        public int UnknownAI06
        {
            get
            {
                return FileEditor.getWord(unknown06);
            }
            set
            {
                FileEditor.setWord(unknown06, value);
            }
        }

        public int UnknownAI08
        {
            get
            {
                return FileEditor.getWord(unknown08);
            }
            set
            {
                FileEditor.setWord(unknown08, value);
            }
        }

        public int UnknownAI0A
        {
            get
            {
                return FileEditor.getWord(unknown0A);
            }
            set
            {
                FileEditor.setWord(unknown0A, value);
            }
        }

        public int UnknownAI0C
        {
            get
            {
                return FileEditor.getWord(unknown0C);
            }
            set
            {
                FileEditor.setWord(unknown0C, value);
            }
        }

        public int UnknownAI0E
        {
            get
            {
                return FileEditor.getWord(unknown0E);
            }
            set
            {
                FileEditor.setWord(unknown0E, value);
            }
        }

        public int UnknownAI10
        {
            get
            {
                return FileEditor.getWord(unknown10);
            }
            set
            {
                FileEditor.setWord(unknown10, value);
            }
        }

        public int UnknownAIAddress
        {
            get
            {
                return (address);
            }
        }



    }
}
