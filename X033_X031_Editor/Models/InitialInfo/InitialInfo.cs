using SF3.Attributes;
using SF3.Types;
using SF3.Values;

namespace SF3.X033_X031_Editor.Models.InitialInfos
{
    public class InitialInfo
    {
        private IX033_X031_FileEditor _fileEditor;

        //starting equipment table
        private int character;
        private int characterClass;
        private int level;
        private int sex;
        private int weapon; //2 bytes
        private int accessory; //2 bytes
        private int item1; //2 bytes
        private int item2; //2 bytes
        private int item3; //2 bytes
        private int item4; //2 bytes
        private int weapon1Type; //for exp
        private int weapon1Exp; //2 bytes
        private int weapon2Type; //for exp
        private int weapon2Exp; //2 bytes
        private int weapon3Type; //for exp
        private int weapon3Exp; //2 bytes
        private int weapon4Type; // for exp
        private int weapon4Exp; //2 bytes

        private int checkType;
        private int checkVersion2;

        private int address;
        private int offset;

        private int index;
        private string name;

        public InitialInfo(IX033_X031_FileEditor fileEditor, int id, string text)
        {
            _fileEditor = fileEditor;
            checkType = _fileEditor.GetByte(0x00000009); //if it's 0x07 we're in a x033.bin
            checkVersion2 = _fileEditor.GetByte(0x00000017); //if it's 0x7c we're in a x033.bin version 1.003 scn2

            if (Scenario == ScenarioType.Scenario1)
            {
                if (checkType == 0x07)
                {
                    offset = 0x00001d80; //scn1
                }
                else
                {
                    offset = 0x00001d50; //x031
                }
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                if (checkType == 0x07) //x033
                {
                    if (checkVersion2 == 0x8c)
                    {
                        offset = 0x00002e96; //scn2 ver 1.003
                    }
                    else
                    {
                        offset = 0x00002ebe; //scn2
                    }
                }
                else //x031
                {
                    if (checkVersion2 == 0x4c)
                    {
                        offset = 0x00002e5a; //scn2 ver 1.003
                    }
                    else
                    {
                        offset = 0x00002e6a;
                    }
                }
            }
            else if (Scenario == ScenarioType.Scenario3)
            {
                if (checkType == 0x07) //x033
                {
                    offset = 0x000054e6; //scn3
                }
                else
                {
                    offset = 0x000054aa;
                }
            }
            else if (Scenario == ScenarioType.PremiumDisk)
            {
                //Console.WriteLine(checkType);
                if (checkType == 0x07) //x033
                {
                    offset = 0x00005734; //pd
                }
                else
                {
                    //Console.WriteLine(checkType);
                    offset = 0x000056ec;
                }
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 0x20);

            character = start + 0x00;
            characterClass = start + 0x01;
            level = start + 0x02;
            sex = start + 0x03;
            weapon = start + 0x04; //2 bytes
            accessory = start + 0x06; //2 bytes
            item1 = start + 0x08; //2 bytes
            item2 = start + 0x0a; //2 bytes
            item3 = start + 0x0c; //2 bytes
            item4 = start + 0x0e; //2 bytes
            weapon1Type = start + 0x10; //for exp
            weapon1Exp = start + 0x12; //2 bytes
            weapon2Type = start + 0x14; //for exp
            weapon2Exp = start + 0x16; //2 bytes
            weapon3Type = start + 0x18; //for exp
            weapon3Exp = start + 0x1a; //2 bytes
            weapon4Type = start + 0x1c; // for exp
            weapon4Exp = start + 0x1e; //2 bytes

            address = offset + (id * 0x20);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int PresetID => index;

        [BulkCopyRowName]
        public string PresetName => name;

        [BulkCopy]
        public int CharacterE
        {
            get => _fileEditor.GetByte(character);
            set => _fileEditor.SetByte(character, (byte)value);
        }

        [BulkCopy]
        public CharacterClassValue CharacterClassE
        {
            get => new CharacterClassValue(_fileEditor.GetByte(characterClass));
            set => _fileEditor.SetByte(characterClass, (byte)value.Value);
        }

        [BulkCopy]
        public int Level
        {
            get => _fileEditor.GetByte(level);
            set => _fileEditor.SetByte(level, (byte)value);
        }

        [BulkCopy]
        public SexValue Sex
        {
            get => new SexValue(_fileEditor.GetByte(sex));
            set => _fileEditor.SetByte(sex, (byte)value.Value);
        }

        [BulkCopy]
        public ItemValue Weapon
        {
            get => new ItemValue(Scenario, _fileEditor.GetWord(weapon));
            set => _fileEditor.SetWord(weapon, value.Value);
        }

        [BulkCopy]
        public ItemValue Accessory
        {
            get => new ItemValue(Scenario, _fileEditor.GetWord(accessory));
            set => _fileEditor.SetWord(accessory, value.Value);
        }

        [BulkCopy]
        public ItemValue Item1
        {
            get => new ItemValue(Scenario, _fileEditor.GetWord(item1));
            set => _fileEditor.SetWord(item1, value.Value);
        }

        [BulkCopy]
        public ItemValue Item2
        {
            get => new ItemValue(Scenario, _fileEditor.GetWord(item2));
            set => _fileEditor.SetWord(item2, value.Value);
        }

        [BulkCopy]
        public ItemValue Item3
        {
            get => new ItemValue(Scenario, _fileEditor.GetWord(item3));
            set => _fileEditor.SetWord(item3, value.Value);
        }

        [BulkCopy]
        public ItemValue Item4
        {
            get => new ItemValue(Scenario, _fileEditor.GetWord(item4));
            set => _fileEditor.SetWord(item4, value.Value);
        }

        [BulkCopy]
        public WeaponTypeValue Weapon1Type
        {
            get => new WeaponTypeValue(_fileEditor.GetByte(weapon1Type));
            set => _fileEditor.SetByte(weapon1Type, (byte)value.Value);
        }

        [BulkCopy]
        public int Weapon1Exp
        {
            get => _fileEditor.GetWord(weapon1Exp);
            set => _fileEditor.SetWord(weapon1Exp, value);
        }

        [BulkCopy]
        public WeaponTypeValue Weapon2Type
        {
            get => new WeaponTypeValue(_fileEditor.GetByte(weapon2Type));
            set => _fileEditor.SetByte(weapon2Type, (byte)value.Value);
        }

        [BulkCopy]
        public int Weapon2Exp
        {
            get => _fileEditor.GetWord(weapon2Exp);
            set => _fileEditor.SetWord(weapon2Exp, value);
        }

        [BulkCopy]
        public WeaponTypeValue Weapon3Type
        {
            get => new WeaponTypeValue(_fileEditor.GetByte(weapon3Type));
            set => _fileEditor.SetByte(weapon3Type, (byte)value.Value);
        }

        [BulkCopy]
        public int Weapon3Exp
        {
            get => _fileEditor.GetWord(weapon3Exp);
            set => _fileEditor.SetWord(weapon3Exp, value);
        }

        [BulkCopy]
        public WeaponTypeValue Weapon4Type
        {
            get => new WeaponTypeValue(_fileEditor.GetByte(weapon4Type));
            set => _fileEditor.SetByte(weapon4Type, (byte)value.Value);
        }

        [BulkCopy]
        public int Weapon4Exp
        {
            get => _fileEditor.GetWord(weapon4Exp);
            set => _fileEditor.SetWord(weapon4Exp, value);
        }

        public int PresetAddress => (address);
    }
}
