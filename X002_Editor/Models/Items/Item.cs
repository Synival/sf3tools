using SF3.Editor;
using SF3.Editor.Values;
using static SF3.X002_Editor.Forms.frmMain;

namespace SF3.X002_Editor.Models.Items
{
    public class Item
    {
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

        public Item(int id, string text)
        {
            checkVersion2 = FileEditor.getByte(0x0000000B);

            if (Globals.scenario == 1)
            {
                offset = 0x00002b28; //scn1
            }
            else if (Globals.scenario == 2)
            {
                offset = 0x00002e9c; //scn2
                if (checkVersion2 == 0x2C)
                {
                    offset = offset - 0x44;
                }
            }
            else if (Globals.scenario == 3)
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

        public int ID => index;
        public string Name => name;

        public int Price
        {
            get => FileEditor.getWord(PriceLocation);
            set => FileEditor.setWord(PriceLocation, value);
        }
        public WeaponTypeValue WeaponType
        {
            get => new WeaponTypeValue(FileEditor.getByte(WeaponTypeLocation));
            set => FileEditor.setByte(WeaponTypeLocation, (byte)value.Value);
        }
        public int EffectsEquip
        {
            get => FileEditor.getByte(EffectsEquipLocation);
            set => FileEditor.setByte(EffectsEquipLocation, (byte)value);
        }
        public int Requirements
        {
            get => FileEditor.getByte(RequirementLocation);
            set => FileEditor.setByte(RequirementLocation, (byte)value);
        }
        public int Range
        {
            get => FileEditor.getByte(RangeLocation);
            set => FileEditor.setByte(RangeLocation, (byte)value);
        }
        public int Attack
        {
            get => FileEditor.getByte(AttackLocation);
            set => FileEditor.setByte(AttackLocation, (byte)value);
        }
        public int Defense
        {
            get => FileEditor.getByte(DefenseLocation);
            set => FileEditor.setByte(DefenseLocation, (byte)value);
        }
        public int AttackRank
        {
            get => FileEditor.getByte(AttackUpRankLocation);
            set => FileEditor.setByte(AttackUpRankLocation, (byte)value);
        }
        public int SpellRank
        {
            get => FileEditor.getByte(SpellUpRankLocation);
            set => FileEditor.setByte(SpellUpRankLocation, (byte)value);
        }
        public int PhysicalAttribute
        {
            get => FileEditor.getByte(PhysicalAttributeLocation);
            set => FileEditor.setByte(PhysicalAttributeLocation, (byte)value);
        }

        public int Unknown1
        {
            get => FileEditor.getByte(Unknown1Location);
            set => FileEditor.setByte(Unknown1Location, (byte)value);
        }
        public int MonsterType
        {
            get => FileEditor.getByte(MonsterTypeAttributeLocation);
            set => FileEditor.setByte(MonsterTypeAttributeLocation, (byte)value);
        }
        public int Unknown2
        {
            get => FileEditor.getByte(Unknown2Location);
            set => FileEditor.setByte(Unknown2Location, (byte)value);
        }
        public int StatType1
        {
            get => FileEditor.getByte(StatType1Location);
            set => FileEditor.setByte(StatType1Location, (byte)value);
        }
        public int StatUp1
        {
            get => FileEditor.getByte(StatUp1Location);
            set => FileEditor.setByte(StatUp1Location, (byte)value);
        }
        public int StatType2
        {
            get => FileEditor.getByte(StatType2Location);
            set => FileEditor.setByte(StatType2Location, (byte)value);
        }
        public int StatUp2
        {
            get => FileEditor.getByte(StatUp2Location);
            set => FileEditor.setByte(StatUp2Location, (byte)value);
        }
        public int StatType3
        {
            get => FileEditor.getByte(StatType3Location);
            set => FileEditor.setByte(StatType3Location, (byte)value);
        }
        public int StatUp3
        {
            get => FileEditor.getByte(StatUp3Location);
            set => FileEditor.setByte(StatUp3Location, (byte)value);
        }
        public int StatType4
        {
            get => FileEditor.getByte(StatType4Location);
            set => FileEditor.setByte(StatType4Location, (byte)value);
        }
        public int StatUp4
        {
            get => FileEditor.getByte(StatUp4Location);
            set => FileEditor.setByte(StatUp4Location, (byte)value);
        }
        public int SpellUse
        {
            get => FileEditor.getByte(SpellOnUseLocation);
            set => FileEditor.setByte(SpellOnUseLocation, (byte)value);
        }
        public int SpellUseLv
        {
            get => FileEditor.getByte(SpellLvOnUseLocation);
            set => FileEditor.setByte(SpellLvOnUseLocation, (byte)value);
        }

        public int Address => (address);
    }
}
