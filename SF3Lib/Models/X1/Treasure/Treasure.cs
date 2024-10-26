using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X1.Treasure {
    public class Treasure {
        private readonly IX1_FileEditor _fileEditor;

        private readonly int searched;
        private readonly int eventNumber;
        private readonly int flagUsed;
        private readonly int unknown;
        private readonly int eventType;
        private readonly int itemID;

        //private int npcOffset;
        private readonly int offset;
        private readonly int sub;

        /*public int NPCTableAddress1
        {
            get => _fileEditor.GetDouble(npcOffset);
            set => _fileEditor.SetDouble(npcOffset, value);
        }

        public int NPCTableAddress2 => _fileEditor.GetDouble(NPCTableAddress1 - 0x0605F000);

        public int NPCTableAddress3 => _fileEditor.GetDouble(NPCTableAddress2 - 0x0605F000);*/

        public Treasure(IX1_FileEditor fileEditor, int id, string text) {
            _fileEditor = fileEditor;

            if (_fileEditor.IsBTL99) {
                offset = 0x0000000C; //btl99 initial pointer
                sub = 0x06060000;
                offset = _fileEditor.GetDouble(offset);
                offset -= sub;
            }
            else if (Scenario == ScenarioType.Scenario1) {
                offset = 0x0000000C; //scn1 initial pointer
                sub = 0x0605f000;
                offset = _fileEditor.GetDouble(offset);

                offset -= sub;
            }
            else if (Scenario == ScenarioType.Scenario2) {
                offset = 0x0000000C; //scn2 initial pointer
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset -= sub;
            }
            else if (Scenario == ScenarioType.Scenario3) {
                offset = 0x0000000C; //scn3 initial pointer
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset -= sub;
            }
            else if (Scenario == ScenarioType.PremiumDisk) {
                offset = 0x0000000C; //pd initial pointer
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset -= sub;
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            TreasureID = id;
            TreasureName = text;

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 0x0c);
            searched = start; //2 bytes. how is searched. second by being 0x13 is a treasure. if this is 0xffff terminate 
            eventNumber = start + 2;
            flagUsed = start + 4;
            unknown = start + 6;
            eventType = start + 8;
            itemID = start + 10;

            //unknown42 = start + 52;
            TreasureAddress = offset + (id * 0x0c);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int TreasureID { get; }
        public string TreasureName { get; }

        public string MPDTieIn
            => _fileEditor.GetWord(eventNumber) <= 0x0f
                ? (_fileEditor.GetWord(eventNumber) + 0x30).ToString("X")
                : "";

        public int Searched {
            get => _fileEditor.GetWord(searched);
            set => _fileEditor.SetWord(searched, value);
        }

        public int EventNumber {
            get => _fileEditor.GetWord(eventNumber);
            set => _fileEditor.SetWord(eventNumber, value);
        }

        public int FlagUse {
            get => _fileEditor.GetWord(flagUsed);
            set => _fileEditor.SetWord(flagUsed, value);
        }

        public int UnknownTreasure {
            get => _fileEditor.GetWord(unknown);
            set => _fileEditor.SetWord(unknown, value);
        }

        public int EventType {
            get => _fileEditor.GetWord(eventType);
            set => _fileEditor.SetWord(eventType, value);
        }

        public int TreasureItem {
            get => _fileEditor.GetWord(itemID);
            set => _fileEditor.SetWord(itemID, value);
        }

        public int TreasureAddress { get; }
    }
}
