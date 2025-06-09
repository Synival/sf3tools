using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X1 {
    public class CharacterTargetPriority : Struct {
        private readonly int _characterIdAddr;

        public CharacterTargetPriority(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x01) {
            _characterIdAddr = Address;
        }

        [TableViewModelColumn(addressField: nameof(_characterIdAddr), displayOrder: 0, displayFormat: "X2")]
        [NameGetter(NamedValueType.Character)]
        [BulkCopy]
        public byte CharacterID {
            get => (byte) Data.GetByte(_characterIdAddr);
            set => Data.SetByte(_characterIdAddr, value);
        }
    }
}
