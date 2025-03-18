using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X002 {
    public class Item : Struct {
        private readonly int PriceLocation;
        private readonly int WeaponTypeLocation;
        private readonly int EffectsEquipLocation;
        private readonly int RequirementLocation;
        private readonly int RangeLocation;
        private readonly int AttackLocation;
        private readonly int DefenseLocation;
        private readonly int AttackUpRankLocation;
        private readonly int SpellUpRankLocation;
        private readonly int PhysicalAttributeLocation;
        private readonly int Unknown1Location;
        private readonly int MonsterTypeAttributeLocation;
        private readonly int Unknown2Location;
        private readonly int StatType1Location;
        private readonly int StatUp1Location;
        private readonly int StatType2Location;
        private readonly int StatUp2Location;
        private readonly int StatType3Location;
        private readonly int StatUp3Location;
        private readonly int StatType4Location;
        private readonly int StatUp4Location;
        private readonly int SpellOnUseLocation;
        private readonly int SpellLvOnUseLocation;

        public Item(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x18) {
            PriceLocation                = Address;     // 2 bytes. only thing that is 2 bytes
            WeaponTypeLocation           = Address + 2; // 1 byte
            EffectsEquipLocation         = Address + 3; // 1 byte
            RequirementLocation          = Address + 4; // 1 byte
            RangeLocation                = Address + 5; // 1 byte
            AttackLocation               = Address + 6;
            DefenseLocation              = Address + 7;
            AttackUpRankLocation         = Address + 8;
            SpellUpRankLocation          = Address + 9;
            PhysicalAttributeLocation    = Address + 10; // actually effective type 1
            Unknown1Location             = Address + 11; // effective type 1 power
            MonsterTypeAttributeLocation = Address + 12; // effective type 2 actually
            Unknown2Location             = Address + 13; // effective type 2 power
            StatType1Location            = Address + 14;
            StatUp1Location              = Address + 15;
            StatType2Location            = Address + 16;
            StatUp2Location              = Address + 17;
            StatType3Location            = Address + 18;
            StatUp3Location              = Address + 19;
            StatType4Location            = Address + 20;
            StatUp4Location              = Address + 21;
            SpellOnUseLocation           = Address + 22;
            SpellLvOnUseLocation         = Address + 23;
        }

        [TableViewModelColumn(displayOrder: 0, displayGroup: "Stats")]
        [BulkCopy]
        public int Price {
            get => Data.GetWord(PriceLocation);
            set => Data.SetWord(PriceLocation, value);
        }

        [TableViewModelColumn(displayOrder: 1, displayName: "Weapon/Eq Type", minWidth: 120, displayGroup: "Stats")]
        [BulkCopy]
        [NameGetter(NamedValueType.WeaponType)]
        public int WeaponType {
            get => Data.GetByte(WeaponTypeLocation);
            set => Data.SetByte(WeaponTypeLocation, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 23.9f, displayFormat: "X2", displayGroup: "FlagsDebug")]
        [BulkCopy]
        public int EffectsEquip {
            get => Data.GetByte(EffectsEquipLocation);
            set => Data.SetByte(EffectsEquipLocation, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 23.1f, displayGroup: "Flags")]
        public bool Cursed {
            get => Data.GetBit(EffectsEquipLocation, 1);
            set => Data.SetBit(EffectsEquipLocation, 1, value);
        }

        [TableViewModelColumn(displayOrder: 23.2f, displayGroup: "Flags")]
        public bool CanCrack {
            get => Data.GetBit(EffectsEquipLocation, 2);
            set => Data.SetBit(EffectsEquipLocation, 2, value);
        }

        [TableViewModelColumn(displayOrder: 23.3f, displayGroup: "Flags")]
        public bool HealingItem {
            get => Data.GetBit(EffectsEquipLocation, 3);
            set => Data.SetBit(EffectsEquipLocation, 3, value);
        }

        [TableViewModelColumn(displayOrder: 23.4f, displayGroup: "Flags")]
        public bool CannotUnequip {
            get => Data.GetBit(EffectsEquipLocation, 4);
            set => Data.SetBit(EffectsEquipLocation, 4, value);
        }

        [TableViewModelColumn(displayOrder: 23.5f, displayGroup: "Flags")]
        public bool Rare {
            get => Data.GetBit(EffectsEquipLocation, 5);
            set => Data.SetBit(EffectsEquipLocation, 5, value);
        }

        [TableViewModelColumn(displayOrder: 23.6f, displayGroup: "Flags")]
        public bool FakeRare //shows rare message when selling, but does not add to deals
        {
            get => Data.GetBit(EffectsEquipLocation, 6);
            set => Data.SetBit(EffectsEquipLocation, 6, value);
        }

        [TableViewModelColumn(displayOrder: 23.7f, displayGroup: "Flags")]
        public bool HealingItem2 //higher tier healing has this
        {
            get => Data.GetBit(EffectsEquipLocation, 7);
            set => Data.SetBit(EffectsEquipLocation, 7, value);
        }

        [TableViewModelColumn(displayOrder: 26, displayFormat: "X2", displayGroup: "FlagsDebug")]
        [BulkCopy]
        public int Requirements {
            get => Data.GetByte(RequirementLocation);
            set => Data.SetByte(RequirementLocation, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 24.1f, displayGroup: "Flags")]
        public bool RequiredPromo {
            get => Data.GetBit(RequirementLocation, 1);
            set => Data.SetBit(RequirementLocation, 1, value);
        }

        [TableViewModelColumn(displayOrder: 24.2f, displayGroup: "Flags")]
        public bool RequiredPromo2 //apostle of light
        {
            get => Data.GetBit(RequirementLocation, 2);
            set => Data.SetBit(RequirementLocation, 2, value);
        }

        [TableViewModelColumn(displayOrder: 24.3f, displayName: "Hero Only", displayGroup: "Flags")]
        public bool RequiredHero //Synbios, Medion, Julian, Gracia, Cyclops
        {
            get => Data.GetBit(RequirementLocation, 3);
            set => Data.SetBit(RequirementLocation, 3, value);
        }

        [TableViewModelColumn(displayOrder: 24.4f, displayName: "Male", displayGroup: "Flags")]
        public bool RequiredMale {
            get => Data.GetBit(RequirementLocation, 4);
            set => Data.SetBit(RequirementLocation, 4, value);
        }

        [TableViewModelColumn(displayOrder: 24.5f, displayName: "Female", displayGroup: "Flags")]
        public bool RequiredFemale {
            get => Data.GetBit(RequirementLocation, 5);
            set => Data.SetBit(RequirementLocation, 5, value);
        }

        [TableViewModelColumn(displayOrder: 4, displayName: "Range (max/min)", displayFormat: "X2", displayGroup: "Stats")]
        [BulkCopy]
        public int Range {
            get => Data.GetByte(RangeLocation);
            set => Data.SetByte(RangeLocation, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 5, displayGroup: "Stats")]
        [BulkCopy]
        public int Attack {
            get => Data.GetByte(AttackLocation);
            set => Data.SetByte(AttackLocation, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 6, displayGroup: "Stats")]
        [BulkCopy]
        public int Defense {
            get => Data.GetByte(DefenseLocation);
            set => Data.SetByte(DefenseLocation, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 7, displayGroup: "Stats")]
        [BulkCopy]
        public int AttackRank {
            get => Data.GetByte(AttackUpRankLocation);
            set => Data.SetByte(AttackUpRankLocation, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 8, displayGroup: "Stats")]
        [BulkCopy]
        public int SpellRank {
            get => Data.GetByte(SpellUpRankLocation);
            set => Data.SetByte(SpellUpRankLocation, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 9, displayName: "Effective Type 1", displayFormat: "X2", minWidth: 110, displayGroup: "Stats")]
        [BulkCopy]
        [NameGetter(NamedValueType.EffectiveType)]
        public int PhysicalAttribute {
            get => Data.GetByte(PhysicalAttributeLocation);
            set => Data.SetByte(PhysicalAttributeLocation, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 10, displayName: "Effective Pow 1", displayFormat: "X2", displayGroup: "Stats")]
        [BulkCopy]
        public int Unknown1 {
            get => Data.GetByte(Unknown1Location);
            set => Data.SetByte(Unknown1Location, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 11, displayName: "Effective Type 2", displayFormat: "X2", minWidth: 110, displayGroup: "Stats")]
        [BulkCopy]
        [NameGetter(NamedValueType.EffectiveType)]
        public int MonsterType {
            get => Data.GetByte(MonsterTypeAttributeLocation);
            set => Data.SetByte(MonsterTypeAttributeLocation, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 12, displayName: "Effective Pow 2", displayFormat: "X2", displayGroup: "Stats")]
        [BulkCopy]
        public int Unknown2 {
            get => Data.GetByte(Unknown2Location);
            set => Data.SetByte(Unknown2Location, (byte) value);
        }

        int conditionallySignedStatUp(int type, int value)
            => type == (int) StatUpType.Special || type == (int) StatUpType.Spell ? value : (sbyte) value;

        [TableViewModelColumn(displayOrder: 13, minWidth: 90, displayGroup: "Bonuses")]
        [BulkCopy]
        [NameGetter(NamedValueType.StatType)]
        public int StatType1 {
            get => Data.GetByte(StatType1Location);
            set => Data.SetByte(StatType1Location, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 14, minWidth: 100, displayGroup: "Bonuses")]
        [BulkCopy]
        [NameGetter(NamedValueType.StatUpValueType, nameof(StatType1))]
        public int StatUp1 {
            get => conditionallySignedStatUp(StatType1, Data.GetByte(StatUp1Location));
            set => Data.SetByte(StatUp1Location, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 15, minWidth: 90, displayGroup: "Bonuses")]
        [BulkCopy]
        [NameGetter(NamedValueType.StatType)]
        public int StatType2 {
            get => Data.GetByte(StatType2Location);
            set => Data.SetByte(StatType2Location, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 16, minWidth: 100, displayGroup: "Bonuses")]
        [BulkCopy]
        [NameGetter(NamedValueType.StatUpValueType, nameof(StatType2))]
        public int StatUp2 {
            get => conditionallySignedStatUp(StatType2, Data.GetByte(StatUp2Location));
            set => Data.SetByte(StatUp2Location, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 17, minWidth: 90, displayGroup: "Bonuses")]
        [BulkCopy]
        [NameGetter(NamedValueType.StatType)]
        public int StatType3 {
            get => Data.GetByte(StatType3Location);
            set => Data.SetByte(StatType3Location, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 18, minWidth: 100, displayGroup: "Bonuses")]
        [BulkCopy]
        [NameGetter(NamedValueType.StatUpValueType, nameof(StatType3))]
        public int StatUp3 {
            get => conditionallySignedStatUp(StatType3, Data.GetByte(StatUp3Location));
            set => Data.SetByte(StatUp3Location, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 19, minWidth: 90, displayGroup: "Bonuses")]
        [BulkCopy]
        [NameGetter(NamedValueType.StatType)]
        public int StatType4 {
            get => Data.GetByte(StatType4Location);
            set => Data.SetByte(StatType4Location, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 20, minWidth: 130, displayGroup: "Bonuses")]
        [BulkCopy]
        [NameGetter(NamedValueType.StatUpValueType, nameof(StatType4))]
        public int StatUp4 {
            get => conditionallySignedStatUp(StatType4, Data.GetByte(StatUp4Location));
            set => Data.SetByte(StatUp4Location, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 21, minWidth: 130, displayGroup: "Stats")]
        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int SpellUse {
            get => Data.GetByte(SpellOnUseLocation);
            set => Data.SetByte(SpellOnUseLocation, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 22, displayGroup: "Stats")]
        [BulkCopy]
        public int SpellUseLv {
            get => Data.GetByte(SpellLvOnUseLocation);
            set => Data.SetByte(SpellLvOnUseLocation, (byte) value);
        }
    }
}
