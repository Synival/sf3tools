using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X002 {
    public class Item : Struct {
        private readonly int _priceAddr;
        private readonly int _weaponEqTypeAddr;
        private readonly int _effectsEquipAddr;
        private readonly int _requirementsAddr;
        private readonly int _rangeAddr;
        private readonly int _attackAddr;
        private readonly int _defenseAddr;
        private readonly int _attackRankAddr;
        private readonly int _spellRankAddr;
        private readonly int _effective1TypeAddr;
        private readonly int _effective1PowAddr;
        private readonly int _effective2TypeAddr;
        private readonly int _effective2PowAddr;
        private readonly int _stat1TypeAddr;
        private readonly int _stat1ModAddr;
        private readonly int _stat2TypeAddr;
        private readonly int _stat2ModAddr;
        private readonly int _stat3TypeAddr;
        private readonly int _stat3ModAddr;
        private readonly int _stat4TypeAddr;
        private readonly int _stat4ModAddr;
        private readonly int _useSpellAddr;
        private readonly int _useSpellLvAddr;

        public Item(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x18) {
            _priceAddr          = Address + 0x00; // 2 bytes. only thing that is 2 bytes
            _weaponEqTypeAddr   = Address + 0x02; // 1 byte
            _effectsEquipAddr   = Address + 0x03; // 1 byte
            _requirementsAddr   = Address + 0x04; // 1 byte
            _rangeAddr          = Address + 0x05; // 1 byte
            _attackAddr         = Address + 0x06; // 1 byte
            _defenseAddr        = Address + 0x07; // 1 byte
            _attackRankAddr     = Address + 0x08; // 1 byte
            _spellRankAddr      = Address + 0x09; // 1 byte
            _effective1TypeAddr = Address + 0x0A; // 1 byte
            _effective1PowAddr  = Address + 0x0B; // 1 byte
            _effective2TypeAddr = Address + 0x0C; // 1 byte
            _effective2PowAddr  = Address + 0x0D; // 1 byte
            _stat1TypeAddr      = Address + 0x0E; // 1 byte
            _stat1ModAddr       = Address + 0x0F; // 1 byte
            _stat2TypeAddr      = Address + 0x10; // 1 byte
            _stat2ModAddr       = Address + 0x11; // 1 byte
            _stat3TypeAddr      = Address + 0x12; // 1 byte
            _stat3ModAddr       = Address + 0x13; // 1 byte
            _stat4TypeAddr      = Address + 0x14; // 1 byte
            _stat4ModAddr       = Address + 0x15; // 1 byte
            _useSpellAddr       = Address + 0x16; // 1 byte
            _useSpellLvAddr     = Address + 0x17; // 1 byte
        }

        [TableViewModelColumn(addressField: nameof(_priceAddr), displayOrder: 0, displayGroup: "Stats")]
        [BulkCopy]
        public int Price {
            get => Data.GetWord(_priceAddr);
            set => Data.SetWord(_priceAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_weaponEqTypeAddr), displayOrder: 1, displayName: "Weapon/Eq Type", minWidth: 120, displayFormat: "X2", displayGroup: "Stats")]
        [BulkCopy]
        [NameGetter(NamedValueType.WeaponType)]
        public int WeaponEqType {
            get => Data.GetByte(_weaponEqTypeAddr);
            set => Data.SetByte(_weaponEqTypeAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_effectsEquipAddr), displayOrder: 25, displayFormat: "X2", displayGroup: "FlagsDebug")]
        [BulkCopy]
        public int EffectsEquip {
            get => Data.GetByte(_effectsEquipAddr);
            set => Data.SetByte(_effectsEquipAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 23.1f, displayGroup: "Flags")]
        public bool Cursed {
            get => Data.GetBit(_effectsEquipAddr, 1);
            set => Data.SetBit(_effectsEquipAddr, 1, value);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 23.2f, displayGroup: "Flags")]
        public bool CanCrack {
            get => Data.GetBit(_effectsEquipAddr, 2);
            set => Data.SetBit(_effectsEquipAddr, 2, value);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 23.3f, displayGroup: "Flags")]
        public bool HealingItem {
            get => Data.GetBit(_effectsEquipAddr, 3);
            set => Data.SetBit(_effectsEquipAddr, 3, value);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 23.4f, displayGroup: "Flags")]
        public bool CannotUnequip {
            get => Data.GetBit(_effectsEquipAddr, 4);
            set => Data.SetBit(_effectsEquipAddr, 4, value);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 23.5f, displayGroup: "Flags")]
        public bool Rare {
            get => Data.GetBit(_effectsEquipAddr, 5);
            set => Data.SetBit(_effectsEquipAddr, 5, value);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 23.6f, displayGroup: "Flags")]
        public bool FakeRare //shows rare message when selling, but does not add to deals
        {
            get => Data.GetBit(_effectsEquipAddr, 6);
            set => Data.SetBit(_effectsEquipAddr, 6, value);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 23.7f, displayGroup: "Flags")]
        public bool HealingItem2 //higher tier healing has this
        {
            get => Data.GetBit(_effectsEquipAddr, 7);
            set => Data.SetBit(_effectsEquipAddr, 7, value);
        }

        [TableViewModelColumn(addressField: nameof(_requirementsAddr), displayOrder: 26, displayFormat: "X2", displayGroup: "FlagsDebug")]
        [BulkCopy]
        public int Requirements {
            get => Data.GetByte(_requirementsAddr);
            set => Data.SetByte(_requirementsAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 24.1f, displayGroup: "Flags")]
        public bool PromotedOnly {
            get => Data.GetBit(_requirementsAddr, 1);
            set => Data.SetBit(_requirementsAddr, 1, value);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 24.2f, displayGroup: "Flags")]
        public bool Promoted2Only {
            get => Data.GetBit(_requirementsAddr, 2);
            set => Data.SetBit(_requirementsAddr, 2, value);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 24.3f, displayGroup: "Flags")]
        public bool HeroOnly { // Synbios, Medion, Julian, Gracia, Cyclops
            get => Data.GetBit(_requirementsAddr, 3);
            set => Data.SetBit(_requirementsAddr, 3, value);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 24.4f, displayGroup: "Flags")]
        public bool MaleOnly {
            get => Data.GetBit(_requirementsAddr, 4);
            set => Data.SetBit(_requirementsAddr, 4, value);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 24.5f, displayGroup: "Flags")]
        public bool FemaleOnly {
            get => Data.GetBit(_requirementsAddr, 5);
            set => Data.SetBit(_requirementsAddr, 5, value);
        }

        [TableViewModelColumn(addressField: nameof(_rangeAddr), displayOrder: 4.0f, displayFormat: "X2", displayGroup: "Stats")]
        [BulkCopy]
        public byte Range {
            get => (byte) Data.GetByte(_rangeAddr);
            set => Data.SetByte(_rangeAddr, value);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 4.05f, displayFormat: "X1", displayGroup: "Stats")]
        public int RangeMax {
            get => (Range & 0xF0) >> 4;
            set => Range = (byte) ((Range & 0x0F) | ((value & 0x0F) << 4));
        }

        [TableViewModelColumn(addressField: null, displayOrder: 4.1f, displayFormat: "X1", displayGroup: "Stats")]
        public int RangeMin {
            get => Range & 0x0F;
            set => Range = (byte) ((Range & 0xF0) | (value & 0x0F));
        }

        [TableViewModelColumn(addressField: nameof(_attackAddr), displayOrder: 5, displayGroup: "Stats")]
        [BulkCopy]
        public int Attack {
            get => Data.GetByte(_attackAddr);
            set => Data.SetByte(_attackAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_defenseAddr), displayOrder: 6, displayGroup: "Stats")]
        [BulkCopy]
        public int Defense {
            get => Data.GetByte(_defenseAddr);
            set => Data.SetByte(_defenseAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_attackRankAddr), displayOrder: 7, displayGroup: "Stats")]
        [BulkCopy]
        public int AttackRank {
            get => Data.GetByte(_attackRankAddr);
            set => Data.SetByte(_attackRankAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_spellRankAddr), displayOrder: 8, displayGroup: "Stats")]
        [BulkCopy]
        public int SpellRank {
            get => Data.GetByte(_spellRankAddr);
            set => Data.SetByte(_spellRankAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_effective1TypeAddr), displayOrder: 9, displayFormat: "X2", minWidth: 110, displayGroup: "Stats")]
        [BulkCopy]
        [NameGetter(NamedValueType.EffectiveType)]
        public int Effective1Type {
            get => Data.GetByte(_effective1TypeAddr);
            set => Data.SetByte(_effective1TypeAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_effective1PowAddr), displayOrder: 10, displayFormat: "X2", displayGroup: "Stats")]
        [BulkCopy]
        public int Effective1Pow {
            get => Data.GetByte(_effective1PowAddr);
            set => Data.SetByte(_effective1PowAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_effective2TypeAddr), displayOrder: 11, displayFormat: "X2", minWidth: 110, displayGroup: "Stats")]
        [BulkCopy]
        [NameGetter(NamedValueType.EffectiveType)]
        public int Effective2Type {
            get => Data.GetByte(_effective2TypeAddr);
            set => Data.SetByte(_effective2TypeAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_effective2PowAddr), displayOrder: 12, displayFormat: "X2", displayGroup: "Stats")]
        [BulkCopy]
        public int Effective2Pow {
            get => Data.GetByte(_effective2PowAddr);
            set => Data.SetByte(_effective2PowAddr, (byte) value);
        }

        private NamedValueType? ValueTypeForStat(int type) {
            switch ((StatUpType) type) {
                case StatUpType.Special:
                    return NamedValueType.Special;
                case StatUpType.Spell:
                    return NamedValueType.Spell;
                default:
                    return null;
            }
        }

        private int ConditionallySignedStatUp(int type, int value)
            => type == (int) StatUpType.Special || type == (int) StatUpType.Spell ? value : (sbyte) value;

        [TableViewModelColumn(addressField: nameof(_stat1TypeAddr), displayOrder: 13, minWidth: 90, displayFormat: "X2", displayGroup: "Bonuses")]
        [BulkCopy]
        [NameGetter(NamedValueType.StatType)]
        public int Stat1Type {
            get => Data.GetByte(_stat1TypeAddr);
            set => Data.SetByte(_stat1TypeAddr, (byte) value);
        }

        public NamedValueType? Stat1ValueType => ValueTypeForStat(Stat1Type);

        [TableViewModelColumn(addressField: nameof(_stat1ModAddr), displayOrder: 14, minWidth: 100, displayFormat: "X2", displayGroup: "Bonuses")]
        [BulkCopy]
        [NameGetter(NamedValueType.ConditionalType, nameof(Stat1ValueType))]
        public int Stat1Mod {
            get => ConditionallySignedStatUp(Stat1Type, Data.GetByte(_stat1ModAddr));
            set => Data.SetByte(_stat1ModAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_stat2TypeAddr), displayOrder: 15, minWidth: 90, displayFormat: "X2", displayGroup: "Bonuses")]
        [BulkCopy]
        [NameGetter(NamedValueType.StatType)]
        public int Stat2Type {
            get => Data.GetByte(_stat2TypeAddr);
            set => Data.SetByte(_stat2TypeAddr, (byte) value);
        }

        public NamedValueType? Stat2ValueType => ValueTypeForStat(Stat2Type);

        [TableViewModelColumn(addressField: nameof(_stat2ModAddr), displayOrder: 16, minWidth: 100, displayFormat: "X2", displayGroup: "Bonuses")]
        [BulkCopy]
        [NameGetter(NamedValueType.ConditionalType, nameof(Stat2ValueType))]
        public int Stat2Mod {
            get => ConditionallySignedStatUp(Stat2Type, Data.GetByte(_stat2ModAddr));
            set => Data.SetByte(_stat2ModAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_stat3TypeAddr), displayOrder: 17, minWidth: 90, displayFormat: "X2", displayGroup: "Bonuses")]
        [BulkCopy]
        [NameGetter(NamedValueType.StatType)]
        public int Stat3Type {
            get => Data.GetByte(_stat3TypeAddr);
            set => Data.SetByte(_stat3TypeAddr, (byte) value);
        }

        public NamedValueType? Stat3ValueType => ValueTypeForStat(Stat3Type);

        [TableViewModelColumn(addressField: nameof(_stat3ModAddr), displayOrder: 18, minWidth: 100, displayFormat: "X2", displayGroup: "Bonuses")]
        [BulkCopy]
        [NameGetter(NamedValueType.ConditionalType, nameof(Stat3ValueType))]
        public int Stat3Mod {
            get => ConditionallySignedStatUp(Stat3Type, Data.GetByte(_stat3ModAddr));
            set => Data.SetByte(_stat3ModAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_stat4TypeAddr), displayOrder: 19, minWidth: 90, displayFormat: "X2", displayGroup: "Bonuses")]
        [BulkCopy]
        [NameGetter(NamedValueType.StatType)]
        public int Stat4Type {
            get => Data.GetByte(_stat4TypeAddr);
            set => Data.SetByte(_stat4TypeAddr, (byte) value);
        }

        public NamedValueType? Stat4ValueType => ValueTypeForStat(Stat4Type);

        [TableViewModelColumn(addressField: nameof(_stat4ModAddr), displayOrder: 20, minWidth: 130, displayFormat: "X2", displayGroup: "Bonuses")]
        [BulkCopy]
        [NameGetter(NamedValueType.ConditionalType, nameof(Stat4ValueType))]
        public int Stat4Mod {
            get => ConditionallySignedStatUp(Stat4Type, Data.GetByte(_stat4ModAddr));
            set => Data.SetByte(_stat4ModAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_useSpellAddr), displayOrder: 21, minWidth: 130, displayFormat: "X2", displayGroup: "Stats")]
        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int UseSpell {
            get => Data.GetByte(_useSpellAddr);
            set => Data.SetByte(_useSpellAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_useSpellLvAddr), displayOrder: 22, displayGroup: "Stats")]
        [BulkCopy]
        public int UseSpellLv {
            get => Data.GetByte(_useSpellLvAddr);
            set => Data.SetByte(_useSpellLvAddr, (byte) value);
        }
    }
}
