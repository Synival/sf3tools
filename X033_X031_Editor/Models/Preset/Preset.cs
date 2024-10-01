using SF3.Editor;
using SF3.Editor.Values;
using System;
using static SF3.X033_X031_Editor.Forms.frmMain;

namespace SF3.X033_X031_Editor.Models.Presets
{
    public class Preset
    {
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

        public Preset(int id, string text)
        {
            checkType = FileEditor.getByte(0x00000009); //if it's 0x07 we're in a x033.bin
            checkVersion2 = FileEditor.getByte(0x00000017); //if it's 0x7c we're in a x033.bin version 1.003 scn2

            if (Globals.scenario == 1)
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
            else if (Globals.scenario == 2)
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
            else if (Globals.scenario == 3)
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
            else if (Globals.scenario == 4)
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

        public int PresetID => index;
        public string PresetName => name;

        public int CharacterE
        {
            get => FileEditor.getByte(character);
            set => FileEditor.setByte(character, (byte)value);
        }

        public CharacterClassValue CharacterClassE
        {
            get => new CharacterClassValue(FileEditor.getByte(characterClass));
            set => FileEditor.setByte(characterClass, (byte)value.Value);
        }

        public int Level
        {
            get => FileEditor.getByte(level);
            set => FileEditor.setByte(level, (byte)value);
        }

        public SexValue Sex
        {
            get => new SexValue(FileEditor.getByte(sex));
            set => FileEditor.setByte(sex, (byte)value.Value);
        }

        public int Weapon
        {
            get => FileEditor.getWord(weapon);
            set => FileEditor.setWord(weapon, value);
        }

        public int Accessory
        {
            get => FileEditor.getWord(accessory);
            set => FileEditor.setWord(accessory, value);
        }

        public int Item1
        {
            get => FileEditor.getWord(item1);
            set => FileEditor.setWord(item1, value);
        }

        public int Item2
        {
            get => FileEditor.getWord(item2);
            set => FileEditor.setWord(item2, value);
        }
        public int Item3
        {
            get => FileEditor.getWord(item3);
            set => FileEditor.setWord(item3, value);
        }
        public int Item4
        {
            get => FileEditor.getWord(item4);
            set => FileEditor.setWord(item4, value);
        }

        public WeaponTypeValue Weapon1Type
        {
            get => new WeaponTypeValue(FileEditor.getByte(weapon1Type));
            set => FileEditor.setByte(weapon1Type, (byte)value.Value);
        }

        public int Weapon1Exp
        {
            get => FileEditor.getWord(weapon1Exp);
            set => FileEditor.setWord(weapon1Exp, value);
        }

        public WeaponTypeValue Weapon2Type
        {
            get => new WeaponTypeValue(FileEditor.getByte(weapon2Type));
            set => FileEditor.setByte(weapon2Type, (byte)value.Value);
        }

        public int Weapon2Exp
        {
            get => FileEditor.getWord(weapon2Exp);
            set => FileEditor.setWord(weapon2Exp, value);
        }
        public WeaponTypeValue Weapon3Type
        {
            get => new WeaponTypeValue(FileEditor.getByte(weapon3Type));
            set => FileEditor.setByte(weapon3Type, (byte)value.Value);
        }

        public int Weapon3Exp
        {
            get => FileEditor.getWord(weapon3Exp);
            set => FileEditor.setWord(weapon3Exp, value);
        }
        public WeaponTypeValue Weapon4Type
        {
            get => new WeaponTypeValue(FileEditor.getByte(weapon4Type));
            set => FileEditor.setByte(weapon4Type, (byte)value.Value);
        }

        public int Weapon4Exp
        {
            get => FileEditor.getWord(weapon4Exp);
            set => FileEditor.setWord(weapon4Exp, value);
        }

        public int PresetAddress => (address);
    }
}
