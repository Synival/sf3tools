using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X1 {
    public class Treasure : Struct {
        private readonly int searched;
        private readonly int eventNumber;
        private readonly int flagUsed;
        private readonly int unknown1;
        private readonly int eventType;
        private readonly int itemID;

        public Treasure(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x0C) {
            searched    = Address;        // 2 bytes. how is searched. second by being 0x13 is a treasure. if this is 0xffff terminate 
            eventNumber = Address + 0x02; // 2 bytes
            flagUsed    = Address + 0x04; // 2 bytes
            unknown1    = Address + 0x06; // 2 bytes
            eventType   = Address + 0x08; // 2 bytes
            itemID      = Address + 0x0a; // 2 bytes
        }

        [TableViewModelColumn(displayOrder: 0, displayName: "Direction/Searched", displayFormat: "X4")]
        [BulkCopy]
        public int Searched {
            get => Data.GetWord(searched);
            set => Data.SetWord(searched, value);
        }

        [TableViewModelColumn(displayOrder: 1, displayFormat: "X2")]
        [BulkCopy]
        public int EventNumber {
            get => Data.GetWord(eventNumber);
            set => Data.SetWord(eventNumber, value);
        }

        [TableViewModelColumn(displayOrder: 2, displayFormat: "X4")]
        [BulkCopy]
        public int FlagUsed {
            get => Data.GetWord(flagUsed);
            set => Data.SetWord(flagUsed, value);
        }


        [TableViewModelColumn(displayOrder: 3, displayFormat: "X2")]
        [BulkCopy]
        public int Unknown1 {
            get => Data.GetWord(unknown1);
            set => Data.SetWord(unknown1, value);
        }

        [TableViewModelColumn(displayOrder: 4, displayName: "EventType/Code", displayFormat: "X4")]
        [BulkCopy]
        public int EventType {
            get => Data.GetWord(eventType);
            set => Data.SetWord(eventType, value);
        }

        [TableViewModelColumn(displayOrder: 5, displayName: "Item/Text/Code", displayFormat: "X4")]
        [BulkCopy]
        [NameGetter(NamedValueType.EventParameter, nameof(EventType))]
        public int EventParameter {
            get => Data.GetWord(itemID);
            set => Data.SetWord(itemID, value);
        }

        [TableViewModelColumn(displayOrder: 6, isReadOnly: true, displayFormat: "X2")]
        [BulkCopy]
        public string MPDTieInID
            => Data.GetWord(eventNumber) <= 0x0f
                ? (Data.GetWord(eventNumber) + 0x30).ToString("X2")
                : "";
    }
}
