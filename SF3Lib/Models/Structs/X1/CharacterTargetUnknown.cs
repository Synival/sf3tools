using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X1 {
    public class CharacterTargetUnknown : Struct {
        private readonly int _valueAddr;

        public CharacterTargetUnknown(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x01) {
            _valueAddr = Address + 0x00; // 1 byte
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0, displayFormat: "X2")]
        [NameGetter(NamedValueType.Character)]
        public byte CharacterID => (byte) ID;

        [TableViewModelColumn(addressField: nameof(_valueAddr), displayOrder: 1, displayFormat: "X2")]
        [BulkCopy]
        public byte Value {
            get => (byte) Data.GetByte(_valueAddr);
            set => Data.SetByte(_valueAddr, value);
        }
    }
}
