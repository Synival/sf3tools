using BrightIdeasSoftware;
using SF3.X1_Editor.Models.UnknownAI;
using System;
using static SF3.X1_Editor.Forms.frmMain;

namespace SF3.X1_Editor.Models.Warps
{
    public class Warp
    {
        private int unknown1;
        private int unknown2;
        private int type;
        private int map;

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

        public Warp(int id, string text)
        {
            //no scn1 for this

            if (Globals.scenario == 2)
            {
                offset = 0x00000018; //scn2 initial pointer
                sub = 0x0605e000;
                offset = FileEditor.getDouble(offset);
                offset = offset - sub;

            }
            else if (Globals.scenario == 3)
            {
                offset = 0x00000018; //scn3 initial pointer
                sub = 0x0605e000;
                offset = FileEditor.getDouble(offset);
                offset = offset - sub;

            }
            else if (Globals.scenario == 4)
            {
                offset = 0x00000018; //pd initial pointer
                sub = 0x0605e000;
                offset = FileEditor.getDouble(offset);
                offset = offset - sub;
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 0x04);
            unknown1 = start;
            unknown2 = start + 1;
            type = start + 2;
            map = start + 3;

            //unknown42 = start + 52;
            address = offset + (id * 0x04);
            //address = 0x0354c + (id * 0x18);

        }

        public int WarpID => index;
        public string WarpName => name;

        public int WarpUnknown1
        {
            get
            {
                return FileEditor.getByte(unknown1);
            }
            set
            {
                FileEditor.setByte(unknown1, (byte)value);
            }
        }

        public int WarpUnknown2
        {
            get
            {
                return FileEditor.getByte(unknown2);
            }
            set
            {
                FileEditor.setByte(unknown2, (byte)value);
            }
        }

        public int WarpType
        {
            get
            {
                return FileEditor.getByte(type);
            }
            set
            {
                FileEditor.setByte(type, (byte)value);
            }
        }

        public int WarpMap
        {
            get
            {
                return FileEditor.getByte(map);
            }
            set
            {
                FileEditor.setByte(map, (byte)value);
            }
        }

        public int WarpAddress => (address);
    }
}
