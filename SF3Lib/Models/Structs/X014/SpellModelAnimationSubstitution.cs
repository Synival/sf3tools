using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X014 {
    public class SpellModelAnimationSubstitution : Struct {
        private readonly int _characterIdAddr;
        private readonly int _spellIdAddr;
        private readonly int _spellLvAddr;
        private readonly int _animationIdAddr;

        public SpellModelAnimationSubstitution(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x06) {
            _characterIdAddr = Address + 0x00; // 2 bytes
            _spellIdAddr     = Address + 0x02; // 1 byte
            _spellLvAddr     = Address + 0x03; // 1 byte
            _animationIdAddr = Address + 0x04; // 2 bytes
        }

        [TableViewModelColumn(addressField: nameof(_characterIdAddr), displayOrder: 0, displayFormat: "X2", minWidth: 100)]
        [NameGetter(NamedValueType.Character)]
        [BulkCopy]
        public short CharacterID {
            get => Data.GetInt16(_characterIdAddr);
            set => Data.SetInt16(_characterIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_spellIdAddr), displayOrder: 1, displayFormat: "X2", minWidth: 150)]
        [NameGetter(NamedValueType.Spell)]
        [BulkCopy]
        public byte SpellID {
            get => (byte) Data.GetUInt8(_spellIdAddr);
            set => Data.SetUInt8(_spellIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_spellLvAddr), displayOrder: 2, displayFormat: "X2")]
        [BulkCopy]
        public byte SpellLv {
            get => (byte) Data.GetUInt8(_spellLvAddr);
            set => Data.SetUInt8(_spellLvAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_animationIdAddr), displayOrder: 3, displayFormat: "X2")]
        public short AnimationId {
            get => Data.GetInt16(_animationIdAddr);
            set => Data.SetInt16(_animationIdAddr, value);
        }
    }
}
