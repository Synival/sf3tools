using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X1 {
    public class Treasure : Struct {
        private readonly int searched;
        private readonly int eventNumber;
        private readonly int flagUsed;
        private readonly int unknown;
        private readonly int eventType;
        private readonly int itemID;

        public Treasure(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x0C) {
            searched    = Address; //2 bytes. how is searched. second by being 0x13 is a treasure. if this is 0xffff terminate 
            eventNumber = Address + 0x02;
            flagUsed    = Address + 0x04;
            unknown     = Address + 0x06;
            eventType   = Address + 0x08;
            itemID      = Address + 0x0a;
        }

        public string MPDTieIn
            => Data.GetWord(eventNumber) <= 0x0f
                ? (Data.GetWord(eventNumber) + 0x30).ToString("X")
                : "";

        [BulkCopy]
        public int Searched {
            get => Data.GetWord(searched);
            set => Data.SetWord(searched, value);
        }

        [BulkCopy]
        public int EventNumber {
            get => Data.GetWord(eventNumber);
            set => Data.SetWord(eventNumber, value);
        }

        [BulkCopy]
        public int FlagUse {
            get => Data.GetWord(flagUsed);
            set => Data.SetWord(flagUsed, value);
        }

        [BulkCopy]
        public int UnknownTreasure {
            get => Data.GetWord(unknown);
            set => Data.SetWord(unknown, value);
        }

        [BulkCopy]
        public int EventType {
            get => Data.GetWord(eventType);
            set => Data.SetWord(eventType, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.EventParameter, nameof(EventType))]
        public int EventParameter {
            get => Data.GetWord(itemID);
            set => Data.SetWord(itemID, value);
        }
    }
}
