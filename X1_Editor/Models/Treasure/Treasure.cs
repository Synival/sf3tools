using BrightIdeasSoftware;
using System;
using static SF3.X1_Editor.Forms.frmMain;

namespace SF3.X1_Editor.Models.Treasures
{
    public class Treasure
    {
        private int searched;
        private int eventNumber;
        private int flagUsed;
        private int unknown;
        private int eventType;
        private int itemID;

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

        public Treasure(int id, string text)
        {
            if (Globals.scenario == 1)
            {
                offset = 0x0000000C; //scn1 initial pointer
                sub = 0x0605f000;
                offset = FileEditor.getDouble(offset);

                offset = offset - sub;

            }
            else if (Globals.scenario == 2)
            {
                offset = 0x0000000C; //scn2 initial pointer
                sub = 0x0605e000;
                offset = FileEditor.getDouble(offset);
                offset = offset - sub;

            }
            else if (Globals.scenario == 3)
            {
                offset = 0x0000000C; //scn3 initial pointer
                sub = 0x0605e000;
                offset = FileEditor.getDouble(offset);
                offset = offset - sub;

            }
            else if (Globals.scenario == 4)
            {
                offset = 0x0000000C; //pd initial pointer
                sub = 0x0605e000;
                offset = FileEditor.getDouble(offset);
                offset = offset - sub;
            }
            else if (Globals.scenario == 5)
            {
                offset = 0x0000000C; //btl99 initial pointer
                sub = 0x06060000;
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

            int start = offset + (id * 0x0c);
            searched = start; //2 bytes. how is searched. second by being 0x13 is a treasure. if this is 0xffff terminate 
            eventNumber = start + 2;
            flagUsed = start + 4;
            unknown = start + 6;
            eventType = start + 8;
            itemID = start + 10;

            //unknown42 = start + 52;
            address = offset + (id * 0x0c);
            //address = 0x0354c + (id * 0x18);

        }

        public int TreasureID => index;
        public string TreasureName => name;

        public string MPDTieIn
        {
            get
            {
                if (FileEditor.getWord(eventNumber) <= 0x0f)
                {
                    return (FileEditor.getWord(eventNumber) + 0x30).ToString("X");
                }
                else
                {
                    return "";
                }

            }
        }

        public int Searched
        {
            get
            {
                return FileEditor.getWord(searched);
            }
            set
            {
                FileEditor.setWord(searched, value);
            }
        }

        public int EventNumber
        {
            get
            {
                return FileEditor.getWord(eventNumber);
            }
            set
            {
                FileEditor.setWord(eventNumber, value);
            }
        }

        public int FlagUse
        {
            get
            {
                return FileEditor.getWord(flagUsed);
            }
            set
            {
                FileEditor.setWord(flagUsed, value);
            }
        }

        public int UnknownTreasure
        {
            get
            {
                return FileEditor.getWord(unknown);
            }
            set
            {
                FileEditor.setWord(unknown, value);
            }
        }

        public int EventType
        {
            get
            {
                return FileEditor.getWord(eventType);
            }
            set
            {
                FileEditor.setWord(eventType, value);
            }
        }

        public int TreasureItem
        {
            get
            {
                return FileEditor.getWord(itemID);
            }
            set
            {
                FileEditor.setWord(itemID, value);
            }
        }

        public int TreasureAddress => (address);
    }
}
