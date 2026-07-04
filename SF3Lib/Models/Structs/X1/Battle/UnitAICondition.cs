using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X1.Battle {
    public class UnitAICondition : Struct {
        private readonly int _zoneAddr;
        private readonly int _typeAddr;
        private readonly int _offAiIndexAddr;
        private readonly int _onAiIndexAddr;

        public UnitAICondition(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x04) {
            _zoneAddr       = Address + 0x00;
            _typeAddr       = Address + 0x01;
            _offAiIndexAddr = Address + 0x02;
            _onAiIndexAddr  = Address + 0x03;
        }

        [TableViewModelColumn(addressField: nameof(_zoneAddr), displayOrder: 0, displayFormat: "X2")]
        [BulkCopy]
        public byte Zone {
            get => Data.GetUInt8(_zoneAddr);
            set => Data.SetUInt8(_zoneAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_typeAddr), displayOrder: 1, displayFormat: "X2")]
        [BulkCopy]
        public byte OrderFlags {
            get => Data.GetUInt8(_typeAddr);
            set => Data.SetUInt8(_typeAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_offAiIndexAddr), displayOrder: 2, displayFormat: "X2")]
        [BulkCopy]
        public byte OffAIIndex {
            get => Data.GetUInt8(_offAiIndexAddr);
            set => Data.SetUInt8(_offAiIndexAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_onAiIndexAddr), displayOrder: 3, displayFormat: "X2")]
        [BulkCopy]
        public byte OnAIIndex {
            get => Data.GetUInt8(_onAiIndexAddr);
            set => Data.SetUInt8(_onAiIndexAddr, value);
        }

        public bool AlwaysCheck => (Zone & 0x80) != 0;
        public bool Exists => Zone != 0xFF;
    }
}
