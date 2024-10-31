using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X002 {
    public class Item : IModel {
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
        private readonly int offset;
        private readonly int checkVersion2;

        public Item(ISF3FileEditor editor, int id, string text) {
            Editor = editor;
            Name   = text;
            ID     = id;
            Size   = 0x18;

            checkVersion2 = Editor.GetByte(0x0000000B);

            if (editor.Scenario == ScenarioType.Scenario1) {
                offset = 0x00002b28; //scn1
                if (checkVersion2 == 0x10) //original jp
                    offset -= 0x0C;
            }
            else if (editor.Scenario == ScenarioType.Scenario2) {
                offset = 0x00002e9c; //scn2
                if (checkVersion2 == 0x2C)
                    offset -= 0x44;
            }
            else if (editor.Scenario == ScenarioType.Scenario3) {
                offset = 0x0000354c; //scn3
            }
            else {
                offset = 0x000035fc; //pd
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 24);
            PriceLocation = start; //2 bytes. only thing that is 2 bytes
            WeaponTypeLocation = start + 2; //1 byte
            EffectsEquipLocation = start + 3; //1 byte
            RequirementLocation = start + 4; //1 byte
            RangeLocation = start + 5; //1 byte
            AttackLocation = start + 6;
            DefenseLocation = start + 7;
            AttackUpRankLocation = start + 8;
            SpellUpRankLocation = start + 9;
            PhysicalAttributeLocation = start + 10; //actually effective type 1
            Unknown1Location = start + 11; //effective type 1 power
            MonsterTypeAttributeLocation = start + 12; //effective type 2 actually
            Unknown2Location = start + 13; //effective type 2 power
            StatType1Location = start + 14;
            StatUp1Location = start + 15;
            StatType2Location = start + 16;
            StatUp2Location = start + 17;
            StatType3Location = start + 18;
            StatUp3Location = start + 19;
            StatType4Location = start + 20;
            StatUp4Location = start + 21;
            SpellOnUseLocation = start + 22;
            SpellLvOnUseLocation = start + 23;
            Address = offset + (id * 0x18);
            //address = 0x0354c + (id * 0x18);
        }

        public IByteEditor Editor { get; }

        [BulkCopyRowName]
        public string Name { get; }
        public int ID { get; }
        public int Address { get; }
        public int Size { get; }

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

        [BulkCopy]
        [NameGetter(NamedValueType.StatType)]
        public int StatType1 {
            get => Editor.GetByte(StatType1Location);
            set => Editor.SetByte(StatType1Location, (byte) value);
        }

        [BulkCopy]
        public int StatUp1 {
            get => Editor.GetByte(StatUp1Location);
            set => Editor.SetByte(StatUp1Location, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.StatType)]
        public int StatType2 {
            get => Editor.GetByte(StatType2Location);
            set => Editor.SetByte(StatType2Location, (byte) value);
        }

        [BulkCopy]
        public int StatUp2 {
            get => Editor.GetByte(StatUp2Location);
            set => Editor.SetByte(StatUp2Location, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.StatType)]
        public int StatType3 {
            get => Editor.GetByte(StatType3Location);
            set => Editor.SetByte(StatType3Location, (byte) value);
        }

        [BulkCopy]
        public int StatUp3 {
            get => Editor.GetByte(StatUp3Location);
            set => Editor.SetByte(StatUp3Location, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.StatType)]
        public int StatType4 {
            get => Editor.GetByte(StatType4Location);
            set => Editor.SetByte(StatType4Location, (byte) value);
        }

        [BulkCopy]
        public int StatUp4 {
            get => Editor.GetByte(StatUp4Location);
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
