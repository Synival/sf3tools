using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X1 {
    public class Interactable : Struct {
        private readonly int _searchedAddr;
        private readonly int _eventNumberAddr;
        private readonly int _flagUsedAddr;
        private readonly int _unknown0x06Addr;
        private readonly int _eventTypeAddr;
        private readonly int _itemIDAddr;

        public Interactable(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x0C) {
            _searchedAddr    = Address + 0x00; // 2 bytes. how is searched. second by being 0x13 is a treasure. if this is 0xffff terminate 
            _eventNumberAddr = Address + 0x02; // 2 bytes
            _flagUsedAddr    = Address + 0x04; // 2 bytes
            _unknown0x06Addr = Address + 0x06; // 2 bytes
            _eventTypeAddr   = Address + 0x08; // 2 bytes
            _itemIDAddr      = Address + 0x0a; // 2 bytes
        }

        [TableViewModelColumn(displayOrder: 0, displayName: "Direction/Searched", displayFormat: "X4")]
        [BulkCopy]
        public int Searched {
            get => Data.GetWord(_searchedAddr);
            set => Data.SetWord(_searchedAddr, value);
        }

        [TableViewModelColumn(displayOrder: 1, displayFormat: "X2")]
        [BulkCopy]
        public int EventNumber {
            get => Data.GetWord(_eventNumberAddr);
            set => Data.SetWord(_eventNumberAddr, value);
        }

        [TableViewModelColumn(displayOrder: 2, displayFormat: "X4")]
        [BulkCopy]
        public int FlagUsed {
            get => Data.GetWord(_flagUsedAddr);
            set => Data.SetWord(_flagUsedAddr, value);
        }

        [TableViewModelColumn(displayOrder: 3, displayName: "+0x06", displayFormat: "X2")]
        [BulkCopy]
        public int Unknown0x06 {
            get => Data.GetWord(_unknown0x06Addr);
            set => Data.SetWord(_unknown0x06Addr, value);
        }

        [TableViewModelColumn(displayOrder: 4, displayName: "EventType/Code", displayFormat: "X4")]
        [BulkCopy]
        public int EventType {
            get => Data.GetWord(_eventTypeAddr);
            set => Data.SetWord(_eventTypeAddr, value);
        }

        [TableViewModelColumn(displayOrder: 5, displayName: "Item/Text/Code", displayFormat: "X4")]
        [BulkCopy]
        [NameGetter(NamedValueType.EventParameter, nameof(EventType))]
        public int EventParameter {
            get => Data.GetWord(_itemIDAddr);
            set => Data.SetWord(_itemIDAddr, value);
        }

        [TableViewModelColumn(displayOrder: 6, isReadOnly: true, displayFormat: "X2")]
        [BulkCopy]
        public string MPDTieInID
            => Data.GetWord(_eventNumberAddr) <= 0x0f
                ? (Data.GetWord(_eventNumberAddr) + 0x30).ToString("X2")
                : "";
    }
}
