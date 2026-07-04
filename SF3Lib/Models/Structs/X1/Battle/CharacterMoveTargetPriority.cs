using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X1.Battle {
    public class CharacterMoveTargetPriority : Struct {
        private readonly int _characterIdAddr;

        public CharacterMoveTargetPriority(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x01) {
            _characterIdAddr = Address;
        }

        [TableViewModelColumn(addressField: nameof(_characterIdAddr), displayOrder: 0, displayFormat: "X2", minWidth: 100)]
        [NameGetter(NamedValueType.Character)]
        [BulkCopy]
        public byte CharacterID {
            get => (byte) Data.GetUInt8(_characterIdAddr);
            set => Data.SetUInt8(_characterIdAddr, value);
        }
    }
}
