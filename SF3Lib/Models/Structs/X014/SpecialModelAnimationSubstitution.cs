using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X014 {
    public class SpecialModelAnimationSubstitution : Struct {
        private readonly int _characterIdAddr;
        private readonly int _specialIdAddr;
        private readonly int _animationIdAddr;

        public SpecialModelAnimationSubstitution(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x04) {
            _characterIdAddr = Address + 0x00; // 2 bytes
            _specialIdAddr   = Address + 0x02; // 1 byte
            _animationIdAddr = Address + 0x03; // 1 byte
        }

        [TableViewModelColumn(addressField: nameof(_characterIdAddr), displayOrder: 0, displayFormat: "X2", minWidth: 100)]
        [NameGetter(NamedValueType.Character)]
        [BulkCopy]
        public short CharacterID {
            get => Data.GetInt16(_characterIdAddr);
            set => Data.SetInt16(_characterIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_specialIdAddr), displayOrder: 1, displayFormat: "X2", minWidth: 200)]
        [NameGetter(NamedValueType.Special)]
        [BulkCopy]
        public byte SpecialID {
            get => (byte) Data.GetUInt8(_specialIdAddr);
            set => Data.SetUInt8(_specialIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_animationIdAddr), displayOrder: 2, displayFormat: "X2")]
        public byte AnimationID {
            get => (byte) Data.GetUInt8(_animationIdAddr);
            set => Data.SetUInt8(_animationIdAddr, value);
        }
    }
}
