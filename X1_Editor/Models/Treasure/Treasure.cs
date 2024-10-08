using BrightIdeasSoftware;
using SF3.Editor;
using SF3.Types;
using System;

namespace SF3.X1_Editor.Models.Treasures
{
    public class Treasure
    {
        private IFileEditor _fileEditor;

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
            get => _fileEditor.GetDouble(npcOffset);
            set => _fileEditor.SetDouble(npcOffset, value);
        }

        public int NPCTableAddress2 => _fileEditor.GetDouble(NPCTableAddress1 - 0x0605F000);

        public int NPCTableAddress3 => _fileEditor.GetDouble(NPCTableAddress2 - 0x0605F000);*/

        public Treasure(IFileEditor fileEditor, ScenarioType scenario, int id, string text)
        {
            _fileEditor = fileEditor;
            Scenario = scenario;

            if (Scenario == ScenarioType.Scenario1)
            {
                offset = 0x0000000C; //scn1 initial pointer
                sub = 0x0605f000;
                offset = _fileEditor.GetDouble(offset);

                offset = offset - sub;
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                offset = 0x0000000C; //scn2 initial pointer
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub;
            }
            else if (Scenario == ScenarioType.Scenario3)
            {
                offset = 0x0000000C; //scn3 initial pointer
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub;
            }
            else if (Scenario == ScenarioType.PremiumDisk)
            {
                offset = 0x0000000C; //pd initial pointer
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub;
            }
            else if (Scenario == ScenarioType.Other)
            {
                offset = 0x0000000C; //btl99 initial pointer
                sub = 0x06060000;
                offset = _fileEditor.GetDouble(offset);
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

        public ScenarioType Scenario { get; }
        public int TreasureID => index;
        public string TreasureName => name;

        public string MPDTieIn
        {
            get
            {
                if (FileEditor.GetWord(eventNumber) <= 0x0f)
                {
                    return (FileEditor.GetWord(eventNumber) + 0x30).ToString("X");
                }
                else
                {
                    return "";
                }
            }
        }

        public int Searched
        {
            get => FileEditor.GetWord(searched);
            set => FileEditor.SetWord(searched, value);
        }

        public int EventNumber
        {
            get => FileEditor.GetWord(eventNumber);
            set => FileEditor.SetWord(eventNumber, value);
        }

        public int FlagUse
        {
            get => FileEditor.GetWord(flagUsed);
            set => FileEditor.SetWord(flagUsed, value);
        }

        public int UnknownTreasure
        {
            get => FileEditor.GetWord(unknown);
            set => FileEditor.SetWord(unknown, value);
        }

        public int EventType
        {
            get => FileEditor.GetWord(eventType);
            set => FileEditor.SetWord(eventType, value);
        }

        public int TreasureItem
        {
            get => FileEditor.GetWord(itemID);
            set => FileEditor.SetWord(itemID, value);
        }

        public int TreasureAddress => (address);
    }
}
