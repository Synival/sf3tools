using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X013 {
    public class SpellAnimationSubstitution : Struct {
        private readonly int _spellIdAddr;
        private readonly int _spellLevelAddr;
        private readonly int _subAnimationAddr;
        private readonly int _replacementSpellID;

        public SpellAnimationSubstitution(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x04) {
            _spellIdAddr        = Address + 0x00; // 1 byte
            _spellLevelAddr     = Address + 0x01; // 1 byte
            _subAnimationAddr   = Address + 0x02; // 1 byte
            _replacementSpellID = Address + 0x03; // 1 byte
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_spellIdAddr), displayOrder: 0, minWidth: 150, displayFormat: "X2")]
        [NameGetter(NamedValueType.Spell)]
        public int SpellID {
            get => Data.GetByte(_spellIdAddr);
            set => Data.SetByte(_spellIdAddr, (byte) value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_spellLevelAddr), displayOrder: 1, displayFormat: "X2")]
        public byte SpellLevel {
            get => (byte) Data.GetByte(_spellLevelAddr);
            set => Data.SetByte(_spellLevelAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_subAnimationAddr), displayOrder: 2, displayFormat: "X2")]
        public byte SubAnimation {
            get => (byte) Data.GetByte(_subAnimationAddr);
            set => Data.SetByte(_subAnimationAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_replacementSpellID), displayOrder: 3, minWidth: 200, displayFormat: "X2")]
        [NameGetter(NamedValueType.Spell)]
        public int ReplacementSpellID {
            get => Data.GetByte(_replacementSpellID);
            set => Data.SetByte(_replacementSpellID, (byte) value);
        }
    }
}
