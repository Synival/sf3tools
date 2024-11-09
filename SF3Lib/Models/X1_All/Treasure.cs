using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X1_All {
    public class Treasure : Model {
        private readonly int searched;
        private readonly int eventNumber;
        private readonly int flagUsed;
        private readonly int unknown;
        private readonly int eventType;
        private readonly int itemID;

        public Treasure(IByteEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x0C) {
            searched    = Address; //2 bytes. how is searched. second by being 0x13 is a treasure. if this is 0xffff terminate 
            eventNumber = Address + 0x02;
            flagUsed    = Address + 0x04;
            unknown     = Address + 0x06;
            eventType   = Address + 0x08;
            itemID      = Address + 0x0a;
        }

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
        [NameGetter(NamedValueType.EventParameter, nameof(EventType))]
        public int EventParameter {
            get => Editor.GetWord(itemID);
            set => Editor.SetWord(itemID, value);
        }
    }
}
