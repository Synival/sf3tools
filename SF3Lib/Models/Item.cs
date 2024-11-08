using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models {
    public class Item : Model {
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

        public Item(IByteEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x18) {
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

        [BulkCopy]
        public int Price {
            get => Editor.GetWord(PriceLocation);
            set => Editor.SetWord(PriceLocation, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.WeaponType)]
        public int WeaponType {
            get => Editor.GetByte(WeaponTypeLocation);
            set => Editor.SetByte(WeaponTypeLocation, (byte) value);
        }

        [BulkCopy]
        public int EffectsEquip {
            get => Editor.GetByte(EffectsEquipLocation);
            set => Editor.SetByte(EffectsEquipLocation, (byte) value);
        }

        public bool Cursed {
            get => Editor.GetBit(EffectsEquipLocation, 1);
            set => Editor.SetBit(EffectsEquipLocation, 1, value);
        }

        public bool CanCrack {
            get => Editor.GetBit(EffectsEquipLocation, 2);
            set => Editor.SetBit(EffectsEquipLocation, 2, value);
        }

        public bool HealingItem {
            get => Editor.GetBit(EffectsEquipLocation, 3);
            set => Editor.SetBit(EffectsEquipLocation, 3, value);
        }

        public bool CannotUnequip {
            get => Editor.GetBit(EffectsEquipLocation, 4);
            set => Editor.SetBit(EffectsEquipLocation, 4, value);
        }

        public bool Rare {
            get => Editor.GetBit(EffectsEquipLocation, 5);
            set => Editor.SetBit(EffectsEquipLocation, 5, value);
        }

        public bool FakeRare //shows rare message when selling, but does not add to deals
        {
            get => Editor.GetBit(EffectsEquipLocation, 6);
            set => Editor.SetBit(EffectsEquipLocation, 6, value);
        }

        public bool HealingItem2 //higher tier healing has this
        {
            get => Editor.GetBit(EffectsEquipLocation, 7);
            set => Editor.SetBit(EffectsEquipLocation, 7, value);
        }

        [BulkCopy]
        public int Requirements {
            get => Editor.GetByte(RequirementLocation);
            set => Editor.SetByte(RequirementLocation, (byte) value);
        }

        public bool RequiredPromo {
            get => Editor.GetBit(RequirementLocation, 1);
            set => Editor.SetBit(RequirementLocation, 1, value);
        }

        public bool RequiredPromo2 //apostle of light
        {
            get => Editor.GetBit(RequirementLocation, 2);
            set => Editor.SetBit(RequirementLocation, 2, value);
        }

        public bool RequiredHero //Synbios, Medion, Julian, Gracia, Cyclops
        {
            get => Editor.GetBit(RequirementLocation, 3);
            set => Editor.SetBit(RequirementLocation, 3, value);
        }

        public bool RequiredMale {
            get => Editor.GetBit(RequirementLocation, 4);
            set => Editor.SetBit(RequirementLocation, 4, value);
        }

        public bool RequiredFemale {
            get => Editor.GetBit(RequirementLocation, 5);
            set => Editor.SetBit(RequirementLocation, 5, value);
        }

        [BulkCopy]
        public int Range {
            get => Editor.GetByte(RangeLocation);
            set => Editor.SetByte(RangeLocation, (byte) value);
        }

        [BulkCopy]
        public int Attack {
            get => Editor.GetByte(AttackLocation);
            set => Editor.SetByte(AttackLocation, (byte) value);
        }

        [BulkCopy]
        public int Defense {
            get => Editor.GetByte(DefenseLocation);
            set => Editor.SetByte(DefenseLocation, (byte) value);
        }

        [BulkCopy]
        public int AttackRank {
            get => Editor.GetByte(AttackUpRankLocation);
            set => Editor.SetByte(AttackUpRankLocation, (byte) value);
        }

        [BulkCopy]
        public int SpellRank {
            get => Editor.GetByte(SpellUpRankLocation);
            set => Editor.SetByte(SpellUpRankLocation, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.EffectiveType)]
        public int PhysicalAttribute {
            get => Editor.GetByte(PhysicalAttributeLocation);
            set => Editor.SetByte(PhysicalAttributeLocation, (byte) value);
        }

        [BulkCopy]
        public int Unknown1 {
            get => Editor.GetByte(Unknown1Location);
            set => Editor.SetByte(Unknown1Location, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.EffectiveType)]
        public int MonsterType {
            get => Editor.GetByte(MonsterTypeAttributeLocation);
            set => Editor.SetByte(MonsterTypeAttributeLocation, (byte) value);
        }

        [BulkCopy]
        public int Unknown2 {
            get => Editor.GetByte(Unknown2Location);
            set => Editor.SetByte(Unknown2Location, (byte) value);
        }

        int conditionallySignedStatUp(int type, int value)
            => (type == (int) StatUpType.Special || type == (int) StatUpType.Spell) ? value : (sbyte) value;

        [BulkCopy]
        [NameGetter(NamedValueType.StatType)]
        public int StatType1 {
            get => Editor.GetByte(StatType1Location);
            set => Editor.SetByte(StatType1Location, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.StatUpValueType, nameof(StatType1))]
        public int StatUp1 {
            get => conditionallySignedStatUp(StatType1, Editor.GetByte(StatUp1Location));
            set => Editor.SetByte(StatUp1Location, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.StatType)]
        public int StatType2 {
            get => Editor.GetByte(StatType2Location);
            set => Editor.SetByte(StatType2Location, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.StatUpValueType, nameof(StatType2))]
        public int StatUp2 {
            get => conditionallySignedStatUp(StatType2, Editor.GetByte(StatUp2Location));
            set => Editor.SetByte(StatUp2Location, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.StatType)]
        public int StatType3 {
            get => Editor.GetByte(StatType3Location);
            set => Editor.SetByte(StatType3Location, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.StatUpValueType, nameof(StatType3))]
        public int StatUp3 {
            get => conditionallySignedStatUp(StatType3, Editor.GetByte(StatUp3Location));
            set => Editor.SetByte(StatUp3Location, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.StatType)]
        public int StatType4 {
            get => Editor.GetByte(StatType4Location);
            set => Editor.SetByte(StatType4Location, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.StatUpValueType, nameof(StatType4))]
        public int StatUp4 {
            get => conditionallySignedStatUp(StatType4, Editor.GetByte(StatUp4Location));
            set => Editor.SetByte(StatUp4Location, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int SpellUse {
            get => Editor.GetByte(SpellOnUseLocation);
            set => Editor.SetByte(SpellOnUseLocation, (byte) value);
        }

        [BulkCopy]
        public int SpellUseLv {
            get => Editor.GetByte(SpellLvOnUseLocation);
            set => Editor.SetByte(SpellLvOnUseLocation, (byte) value);
        }
    }
}
