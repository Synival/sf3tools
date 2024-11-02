using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X1 {
    public class Treasure : IModel {
        private readonly int searched;
        private readonly int eventNumber;
        private readonly int flagUsed;
        private readonly int unknown;
        private readonly int eventType;
        private readonly int itemID;

        public Treasure(IX1_FileEditor editor, int id, string name) {
            Editor = editor;
            Name   = name;
            ID     = id;
            Size   = 0x0c;

            int offset = 0;
            int sub;

            if (editor.IsBTL99) {
                offset = 0x0000000C; //btl99 initial pointer
                sub = 0x06060000;
                offset = Editor.GetDouble(offset);
                offset -= sub;
            }
            else if (editor.Scenario == ScenarioType.Scenario1) {
                offset = 0x0000000C; //scn1 initial pointer
                sub = 0x0605f000;
                offset = Editor.GetDouble(offset);

                offset -= sub;
            }
            else if (editor.Scenario == ScenarioType.Scenario2) {
                offset = 0x0000000C; //scn2 initial pointer
                sub = 0x0605e000;
                offset = Editor.GetDouble(offset);
                offset -= sub;
            }
            else if (editor.Scenario == ScenarioType.Scenario3) {
                offset = 0x0000000C; //scn3 initial pointer
                sub = 0x0605e000;
                offset = Editor.GetDouble(offset);
                offset -= sub;
            }
            else if (editor.Scenario == ScenarioType.PremiumDisk) {
                offset = 0x0000000C; //pd initial pointer
                sub = 0x0605e000;
                offset = Editor.GetDouble(offset);
                offset -= sub;
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

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

        public IByteEditor Editor { get; }

        [BulkCopyRowName]
        public string Name { get; }
        public int ID { get; }
        public int Address { get; }
        public int Size { get; }

        public string MPDTieIn
            => Editor.GetWord(eventNumber) <= 0x0f
                ? (Editor.GetWord(eventNumber) + 0x30).ToString("X")
                : "";

        [BulkCopy]
        public int Searched {
            get => Editor.GetWord(searched);
            set => Editor.SetWord(searched, value);
        }

        [BulkCopy]
        public int EventNumber {
            get => Editor.GetWord(eventNumber);
            set => Editor.SetWord(eventNumber, value);
        }

        [BulkCopy]
        public int FlagUse {
            get => Editor.GetWord(flagUsed);
            set => Editor.SetWord(flagUsed, value);
        }

        [BulkCopy]
        public int UnknownTreasure {
            get => Editor.GetWord(unknown);
            set => Editor.SetWord(unknown, value);
        }

        [BulkCopy]
        public int EventType {
            get => Editor.GetWord(eventType);
            set => Editor.SetWord(eventType, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public int TreasureItem {
            get => Editor.GetWord(itemID);
            set => Editor.SetWord(itemID, value);
        }

        public int TreasureAddress { get; }
    }
}
