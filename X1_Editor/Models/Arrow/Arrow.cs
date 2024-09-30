using BrightIdeasSoftware;
using SF3.Editor;
using System;
using static SF3.X1_Editor.Forms.frmMain;

namespace SF3.X1_Editor.Models.Arrows
{
    public class Arrow
    {
        private int unknown0; //2 byte
        private int textID; //2 byte
        private int unknown4; //2 byte
        private int warpInMPD; //2 byte
        private int unknown8; //2 byte
        private int unknownA; //2 byte

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

        public Arrow(int id, string text)
        {
            if (Globals.scenario == 2)
            {
                offset = 0x00000060; //scn2 initial pointer
                sub = 0x0605e000;
                offset = FileEditor.getDouble(offset);
                offset = offset - sub;
            }
            else if (Globals.scenario == 3)
            {
                offset = 0x00000060; //scn3 initial pointer
                sub = 0x0605e000;
                offset = FileEditor.getDouble(offset);
                offset = offset - sub;
            }
            else if (Globals.scenario == 4)
            {
                offset = 0x00000060; //pd initial pointer
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

            int start = offset + (id * 0x0c);
            unknown0 = start; //2 bytes. how is searched. second by being 0x13 is a treasure. if this is 0xffff terminate 
            textID = start + 0x02;
            unknown4 = start + 0x04;
            warpInMPD = start + 0x06;
            unknown8 = start + 0x08;
            unknownA = start + 0x0a;

            //unknown42 = start + 52;
            address = offset + (id * 0x0c);
            //address = 0x0354c + (id * 0x18);
        }

        public int ArrowID => index;
        public string ArrowName => name;

        public int ArrowUnknown0
        {
            get => FileEditor.getWord(unknown0);
            set => FileEditor.setWord(unknown0, value);
        }

        public int ArrowText
        {
            get => FileEditor.getWord(textID);
            set => FileEditor.setWord(textID, value);
        }

        public int ArrowUnknown4
        {
            get => FileEditor.getWord(unknown4);
            set => FileEditor.setWord(unknown4, value);
        }

        public int ArrowWarp
        {
            get => FileEditor.getWord(warpInMPD);
            set => FileEditor.setWord(warpInMPD, value);
        }

        public int ArrowUnknown8
        {
            get => FileEditor.getWord(unknown8);
            set => FileEditor.setWord(unknown8, value);
        }

        public int ArrowUnknownA
        {
            get => FileEditor.getWord(unknownA);
            set => FileEditor.setWord(unknownA, value);
        }

        public int ArrowAddress => (address);
    }
}
