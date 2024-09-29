//using SF3.X1_Editor.Forms;
//using SF3.X1_Editor.Models;
using BrightIdeasSoftware;
using System;
using static SF3.X1_Editor.Forms.frmMain;

//using SF3.X1_Editor.Models.StatTypes;


namespace SF3.X1_Editor.Models.Npcs
{
    public class Npc
    {

        private int spriteID;
        private int unknown1;
        private int table;
        private int xPos;
        private int zPos;
        private int direction;
        private int unknownA;
        private int unknownC;
        private int unknownE;
        private int unknown12;
        private int unknown16;




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







        public Npc(int id, string text)
        {
            if (Globals.scenario == 1)
            {
                offset = 0x00000018; //scn1 initial pointer
                sub = 0x0605f000;
                offset = FileEditor.getDouble(offset);
                
                offset = offset - sub; 
                
            }
            else if (Globals.scenario == 2)
            {

                offset = 0x00000024; //scn2 initial pointer
                sub = 0x0605e000;
                offset = FileEditor.getDouble(offset);
                offset = offset - sub;


            }
            else if (Globals.scenario == 3)
            {
                offset = 0x00000024; //scn3 initial pointer
                sub = 0x0605e000;
                offset = FileEditor.getDouble(offset);
                offset = offset - sub;


            }
            else if (Globals.scenario == 4)
            {
                offset = 0x00000024; //pd initial pointer
                sub = 0x0605e000;
                offset = FileEditor.getDouble(offset);
                offset = offset - sub;
            }/*
            else if (Globals.scenario == 5)
            {

                offset = 0x00000024; //btl99 initial pointer
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

            int start = offset + (id * 0x18);
            spriteID = start; //2 bytes. how is searched. second by being 0x13 is a treasure. if this is 0xffff terminate 
            unknown1 = start + 0x02; //unknown+0x02
            table = start + 0x04;
            xPos = start + 0x08;
            unknownA = start + 0x0a;
            unknownC = start + 0x0c;
            unknownE = start + 0x0e;
            zPos = start + 0x10;
            unknown12 = start + 0x12;
            direction = start + 0x14;
            unknown16 = start + 0x16;






            //unknown42 = start + 52;
            address = offset + (id * 0x18);
            //address = 0x0354c + (id * 0x18);

        }

        public int NpcID
        {
            get
            {
                return index;
            }
        }
        public string NpcName
        {
            get
            {
                return name;
            }
        }

        public string NpcTieIn
        {
            /*get
            {
                return index + 0x3D;
            }*/

            get
            {
                if ((FileEditor.getWord(spriteID) > 0x0f) && (FileEditor.getWord(spriteID) != 0xffff))
                {
                    return (index + 0x3D).ToString("X");
                }
                else
                {
                    return "";
                }


            }
        }

        public int SpriteID
        {
            get
            {
                return FileEditor.getWord(spriteID);
            }
            set
            {
                FileEditor.setWord(spriteID, value);
            }
        }

        public int NpcUnknown
        {
            get
            {
                return FileEditor.getWord(unknown1);
            }
            set
            {
                FileEditor.setWord(unknown1, value);
            }
        }


        public int NpcTable
        {
            get
            {
                return FileEditor.getDouble(table);
            }
            set
            {
                FileEditor.setDouble(table, value);
            }
        }

        public int NpcXPos
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

        public int NpcZPos
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

        public int NpcDirection
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

        public int NpcUnknownA
        {
            get
            {
                return FileEditor.getWord(unknownA);
            }
            set
            {
                FileEditor.setWord(unknownA, value);
            }
        }

        public int NpcUnknownC
        {
            get
            {
                return FileEditor.getWord(unknownC);
            }
            set
            {
                FileEditor.setWord(unknownC, value);
            }
        }

        public int NpcUnknownE
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

        public int NpcUnknown12
        {
            get
            {
                return FileEditor.getWord(unknown12);
            }
            set
            {
                FileEditor.setWord(unknown12, value);
            }
        }

        public int NpcUnknown16
        {
            get
            {
                return FileEditor.getWord(unknown16);
            }
            set
            {
                FileEditor.setWord(unknown16, value);
            }
        }






        public int NpcAddress
        {
            get
            {
                return (address);
            }
        }



    }
}
