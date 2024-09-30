using BrightIdeasSoftware;
using System;
using static SF3.X1_Editor.Forms.frmMain;

namespace SF3.X1_Editor.Models.Enters
{
    public class Enter
    {
        private int enterID; //2 byte
        private int unknown2; //2 byte
        private int xPos; //2 byte
        private int unknown6; //2 byte
        private int zPos; //2 byte
        private int direction; //2 byte
        private int camera; //2 byte
        private int unknownE; //2 byte

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

        public Enter(int id, string text)
        {
            if (Globals.scenario == 1)
            {
                offset = 0x00000024; //scn1 initial pointer
                sub = 0x0605f000;
                offset = FileEditor.getDouble(offset);

                offset = offset - sub;

            }
            else if (Globals.scenario == 2)
            {
                offset = 0x00000030; //scn2 initial pointer
                sub = 0x0605e000;
                offset = FileEditor.getDouble(offset);
                offset = offset - sub;

            }
            else if (Globals.scenario == 3)
            {
                offset = 0x00000030; //scn3 initial pointer
                sub = 0x0605e000;
                offset = FileEditor.getDouble(offset);
                offset = offset - sub;

            }
            else if (Globals.scenario == 4)
            {
                offset = 0x00000030; //pd initial pointer
                sub = 0x0605e000;
                offset = FileEditor.getDouble(offset);
                offset = offset - sub;
            }
            /*
            else if (Globals.scenario == 5)
            {
                offset = 0x00000030; //btl99 initial pointer
                sub = 0x06060000;
                offset = FileEditor.getDouble(offset);
                offset = offset - sub;

            }*/

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 0x10);
            enterID = start; //2 bytes. how is searched. second by being 0x13 is a treasure. if this is 0xffff terminate 
            unknown2 = start + 0x02; //unknown+0x02
            xPos = start + 0x04;
            unknown6 = start + 0x06;
            zPos = start + 0x08;
            direction = start + 0x0a;
            camera = start + 0x0c;
            unknownE = start + 0x0e;

            //unknown42 = start + 52;
            address = offset + (id * 0x10);
            //address = 0x0354c + (id * 0x18);

        }

        public int EnterID
        {
            get
            {
                return index;
            }
        }
        public string EnterName
        {
            get
            {
                return name;
            }
        }

        public int Entered
        {
            get
            {
                return FileEditor.getWord(enterID);
            }
            set
            {
                FileEditor.setWord(enterID, value);
            }
        }

        public int EnterUnknown2
        {
            get
            {
                return FileEditor.getWord(unknown2);
            }
            set
            {
                FileEditor.setWord(unknown2, value);
            }
        }

        public int EnterXPos
        {
            get
            {
                return FileEditor.getWord(xPos);
            }
            set
            {
                FileEditor.setWord(xPos, value);
            }
        }

        public int EnterUnknown6
        {
            get
            {
                return FileEditor.getWord(unknown6);
            }
            set
            {
                FileEditor.setWord(unknown6, value);
            }
        }

        public int EnterZPos
        {
            get
            {
                return FileEditor.getWord(zPos);
            }
            set
            {
                FileEditor.setWord(zPos, value);
            }
        }

        public int EnterDirection
        {
            get
            {
                return FileEditor.getWord(direction);
            }
            set
            {
                FileEditor.setWord(direction, value);
            }
        }

        public int EnterCamera
        {
            get
            {
                return FileEditor.getWord(camera);
            }
            set
            {
                FileEditor.setWord(camera, value);
            }
        }

        public int EnterUnknownE
        {
            get
            {
                return FileEditor.getWord(unknownE);
            }
            set
            {
                FileEditor.setWord(unknownE, value);
            }
        }

        public int EnterAddress
        {
            get
            {
                return (address);
            }
        }

    }
}
