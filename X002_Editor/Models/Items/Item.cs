using SF3.Editor;
using SF3.Types;
using SF3.Values;

namespace SF3.X002_Editor.Models.Items
{
    public class Item
    {
        private IFileEditor _fileEditor;

        private int PriceLocation;
        private int WeaponTypeLocation;
        private int EffectsEquipLocation;
        private int RequirementLocation;
        private int RangeLocation;
        private int AttackLocation;
        private int DefenseLocation;
        private int AttackUpRankLocation;
        private int SpellUpRankLocation;
        private int PhysicalAttributeLocation;
        private int Unknown1Location;
        private int MonsterTypeAttributeLocation;
        private int Unknown2Location;
        private int StatType1Location;
        private int StatUp1Location;
        private int StatType2Location;
        private int StatUp2Location;
        private int StatType3Location;
        private int StatUp3Location;
        private int StatType4Location;
        private int StatUp4Location;
        private int SpellOnUseLocation;
        private int SpellLvOnUseLocation;
        private int address;
        private int offset;
        private int checkVersion2;

        private int index;
        private string name;

        public Item(IFileEditor fileEditor, ScenarioType scenario, int id, string text)
        {
            _fileEditor = fileEditor;
            Scenario = scenario;

            checkVersion2 = FileEditor.GetByte(0x0000000B);

            if (Scenario == ScenarioType.Scenario1)
            {
                offset = 0x00002b28; //scn1
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                offset = 0x00002e9c; //scn2
                if (checkVersion2 == 0x2C)
                {
                    offset = offset - 0x44;
                }
            }
            else if (Scenario == ScenarioType.Scenario3)
            {
                offset = 0x0000354c; //scn3
            }
            else
                offset = 0x000035fc; //pd

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 24);
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
            address = offset + (id * 0x18);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario { get; }
        public int ID => index;
        public string Name => name;

        public int Price
        {
            get => _fileEditor.GetWord(PriceLocation);
            set => _fileEditor.SetWord(PriceLocation, value);
        }
        public WeaponTypeValue WeaponType
        {
            get => new WeaponTypeValue(FileEditor.GetByte(WeaponTypeLocation));
            set => FileEditor.SetByte(WeaponTypeLocation, (byte)value.Value);
        }
        public int EffectsEquip
        {
            get => FileEditor.GetByte(EffectsEquipLocation);
            set => FileEditor.SetByte(EffectsEquipLocation, (byte)value);
        }

        public bool Cursed
        {
            get => _fileEditor.GetBit(EffectsEquipLocation, 1);
            set => _fileEditor.SetBit(EffectsEquipLocation, 1, value);
        }

        public bool CanCrack
        {
            get => _fileEditor.GetBit(EffectsEquipLocation, 2);
            set => _fileEditor.SetBit(EffectsEquipLocation, 2, value);
        }

        public bool HealingItem
        {
            get => _fileEditor.GetBit(EffectsEquipLocation, 3);
            set => _fileEditor.SetBit(EffectsEquipLocation, 3, value);
        }

        public bool CannotUnequip
        {
            get => _fileEditor.GetBit(EffectsEquipLocation, 4);
            set => _fileEditor.SetBit(EffectsEquipLocation, 4, value);
        }

        public bool Rare
        {
            get => _fileEditor.GetBit(EffectsEquipLocation, 5);
            set => _fileEditor.SetBit(EffectsEquipLocation, 5, value);
        }

        public bool FakeRare //shows rare message when selling, but does not add to deals
        {
            get => _fileEditor.GetBit(EffectsEquipLocation, 6);
            set => _fileEditor.SetBit(EffectsEquipLocation, 6, value);
        }

        public bool HealingItem2 //higher tier healing has this
        {
            get => _fileEditor.GetBit(EffectsEquipLocation, 7);
            set => _fileEditor.SetBit(EffectsEquipLocation, 7, value);
        }

        public int Requirements
        {
            get => FileEditor.GetByte(RequirementLocation);
            set => FileEditor.SetByte(RequirementLocation, (byte)value);
        }

        public bool RequiredPromo
        {
            get => _fileEditor.GetBit(RequirementLocation, 1);
            set => _fileEditor.SetBit(RequirementLocation, 1, value);
        }

        public bool RequiredPromo2 //apostle of light
        {
            get => _fileEditor.GetBit(RequirementLocation, 2);
            set => _fileEditor.SetBit(RequirementLocation, 2, value);
        }

        public bool RequiredHero //Synbios, Medion, Julian, Gracia, Cyclops
        {
            get => _fileEditor.GetBit(RequirementLocation, 3);
            set => _fileEditor.SetBit(RequirementLocation, 3, value);
        }

        public bool RequiredMale
        {
            get => _fileEditor.GetBit(RequirementLocation, 4);
            set => _fileEditor.SetBit(RequirementLocation, 4, value);
        }

        public bool RequiredFemale
        {
            get => _fileEditor.GetBit(RequirementLocation, 5);
            set => _fileEditor.SetBit(RequirementLocation, 5, value);
        }


        public int Range
        {
            get => FileEditor.GetByte(RangeLocation);
            set => FileEditor.SetByte(RangeLocation, (byte)value);
        }
        public int Attack
        {
            get => FileEditor.GetByte(AttackLocation);
            set => FileEditor.SetByte(AttackLocation, (byte)value);
        }
        public int Defense
        {
            get => FileEditor.GetByte(DefenseLocation);
            set => FileEditor.SetByte(DefenseLocation, (byte)value);
        }
        public int AttackRank
        {
            get => FileEditor.GetByte(AttackUpRankLocation);
            set => FileEditor.SetByte(AttackUpRankLocation, (byte)value);
        }
        public int SpellRank
        {
            get => FileEditor.GetByte(SpellUpRankLocation);
            set => FileEditor.SetByte(SpellUpRankLocation, (byte)value);
        }
        public int PhysicalAttribute
        {
            get => FileEditor.GetByte(PhysicalAttributeLocation);
            set => FileEditor.SetByte(PhysicalAttributeLocation, (byte)value);
        }

        public int Unknown1
        {
            get => FileEditor.GetByte(Unknown1Location);
            set => FileEditor.SetByte(Unknown1Location, (byte)value);
        }
        public int MonsterType
        {
            get => FileEditor.GetByte(MonsterTypeAttributeLocation);
            set => FileEditor.SetByte(MonsterTypeAttributeLocation, (byte)value);
        }
        public int Unknown2
        {
            get => FileEditor.GetByte(Unknown2Location);
            set => FileEditor.SetByte(Unknown2Location, (byte)value);
        }
        public int StatType1
        {
            get => FileEditor.GetByte(StatType1Location);
            set => FileEditor.SetByte(StatType1Location, (byte)value);
        }
        public int StatUp1
        {
            get => FileEditor.GetByte(StatUp1Location);
            set => FileEditor.SetByte(StatUp1Location, (byte)value);
        }
        public int StatType2
        {
            get => FileEditor.GetByte(StatType2Location);
            set => FileEditor.SetByte(StatType2Location, (byte)value);
        }
        public int StatUp2
        {
            get => FileEditor.GetByte(StatUp2Location);
            set => FileEditor.SetByte(StatUp2Location, (byte)value);
        }
        public int StatType3
        {
            get => FileEditor.GetByte(StatType3Location);
            set => FileEditor.SetByte(StatType3Location, (byte)value);
        }
        public int StatUp3
        {
            get => FileEditor.GetByte(StatUp3Location);
            set => FileEditor.SetByte(StatUp3Location, (byte)value);
        }
        public int StatType4
        {
            get => FileEditor.GetByte(StatType4Location);
            set => FileEditor.SetByte(StatType4Location, (byte)value);
        }
        public int StatUp4
        {
            get => FileEditor.GetByte(StatUp4Location);
            set => FileEditor.SetByte(StatUp4Location, (byte)value);
        }
        public int SpellUse
        {
            get => FileEditor.GetByte(SpellOnUseLocation);
            set => FileEditor.SetByte(SpellOnUseLocation, (byte)value);
        }
        public int SpellUseLv
        {
            get => FileEditor.GetByte(SpellLvOnUseLocation);
            set => FileEditor.SetByte(SpellLvOnUseLocation, (byte)value);
        }

        public int Address => (address);
    }
}
