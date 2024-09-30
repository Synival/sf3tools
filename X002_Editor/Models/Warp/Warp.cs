//using SF3.X002_Editor.Forms;
//using SF3.X002_Editor.Models;
using BrightIdeasSoftware;
using System;
using static SF3.X002_Editor.Forms.frmMain;

//using SF3.X002_Editor.Models.StatTypes;


namespace SF3.X002_Editor.Models.Warps
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


            if (Globals.scenario == 1)
            {

                offset = 0x000053cc; //scn1


            }



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

        public int WarpID
        {
            get
            {
                return index;
            }
        }
        public string WarpName
        {
            get
            {
                return name;
            }
        }

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






        public int WarpAddress
        {
            get
            {
                return (address);
            }
        }



    }
}
