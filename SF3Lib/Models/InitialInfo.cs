using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.FileEditors;
using SF3.NamedValues;
using SF3.Types;

namespace SF3.Models {
    public class InitialInfo : IModel {
        //starting equipment table
        private readonly int character;
        private readonly int characterClass;
        private readonly int level;
        private readonly int sex;
        private readonly int weapon; //2 bytes
        private readonly int accessory; //2 bytes
        private readonly int item1; //2 bytes
        private readonly int item2; //2 bytes
        private readonly int item3; //2 bytes
        private readonly int item4; //2 bytes
        private readonly int weapon1Type; //for exp
        private readonly int weapon1Exp; //2 bytes
        private readonly int weapon2Type; //for exp
        private readonly int weapon2Exp; //2 bytes
        private readonly int weapon3Type; //for exp
        private readonly int weapon3Exp; //2 bytes
        private readonly int weapon4Type; // for exp
        private readonly int weapon4Exp; //2 bytes

        public InitialInfo(IByteEditor fileEditor, int id, string name, int address, ScenarioType scenario) {
            Editor   = fileEditor;
            ID       = id;
            Name     = name;
            Address  = address;
            Size     = 0x20;

            Scenario = scenario;

            character      = Address + 0x00;
            characterClass = Address + 0x01;
            level          = Address + 0x02;
            sex            = Address + 0x03;
            weapon         = Address + 0x04; // 2 bytes
            accessory      = Address + 0x06; // 2 bytes
            item1          = Address + 0x08; // 2 bytes
            item2          = Address + 0x0a; // 2 bytes
            item3          = Address + 0x0c; // 2 bytes
            item4          = Address + 0x0e; // 2 bytes
            weapon1Type    = Address + 0x10; // 2 bytes
            weapon1Exp     = Address + 0x12; // 2 bytes
            weapon2Type    = Address + 0x14; // 2 bytes
            weapon2Exp     = Address + 0x16; // 2 bytes
            weapon3Type    = Address + 0x18; // 2 bytes
            weapon3Exp     = Address + 0x1a; // 2 bytes
            weapon4Type    = Address + 0x1c; // 2 bytes
            weapon4Exp     = Address + 0x1e; // 2 bytes
        }

        public IByteEditor Editor { get; }
        public ScenarioType Scenario { get; }
        [BulkCopyRowName]
        public string Name { get; }
        public int ID { get; }
        public int Address { get; }
        public int Size { get; }

        private NameAndInfo GetCharacterClassName(int value) => ValueNames.GetCharacterClassName(value);
        private NameAndInfo GetSexName(int value) => ValueNames.GetSexName(value);
        private NameAndInfo GetItemName(int value) => ValueNames.GetItemName(Scenario, value);
        private NameAndInfo GetWeaponTypeName(int value) => ValueNames.GetWeaponTypeName(value);

        [BulkCopy]
        public int CharacterE {
            get => Editor.GetByte(character);
            set => Editor.SetByte(character, (byte) value);
        }

        [BulkCopy]
        [NameGetter(nameof(GetCharacterClassName))]
        public int CharacterClassE {
            get => Editor.GetByte(characterClass);
            set => Editor.SetByte(characterClass, (byte) value);
        }

        [BulkCopy]
        public int Level {
            get => Editor.GetByte(level);
            set => Editor.SetByte(level, (byte) value);
        }

        [BulkCopy]
        [NameGetter(nameof(GetSexName))]
        public int Sex {
            get => Editor.GetByte(sex);
            set => Editor.SetByte(sex, (byte) value);
        }

        [BulkCopy]
        [NameGetter(nameof(GetItemName))]
        public int Weapon {
            get => Editor.GetWord(weapon);
            set => Editor.SetWord(weapon, value);
        }

        [BulkCopy]
        [NameGetter(nameof(GetItemName))]
        public int Accessory {
            get => Editor.GetWord(accessory);
            set => Editor.SetWord(accessory, value);
        }

        [BulkCopy]
        [NameGetter(nameof(GetItemName))]
        public int Item1 {
            get => Editor.GetWord(item1);
            set => Editor.SetWord(item1, value);
        }

        [BulkCopy]
        [NameGetter(nameof(GetItemName))]
        public int Item2 {
            get => Editor.GetWord(item2);
            set => Editor.SetWord(item2, value);
        }

        [BulkCopy]
        [NameGetter(nameof(GetItemName))]
        public int Item3 {
            get => Editor.GetWord(item3);
            set => Editor.SetWord(item3, value);
        }

        [BulkCopy]
        [NameGetter(nameof(GetItemName))]
        public int Item4 {
            get => Editor.GetWord(item4);
            set => Editor.SetWord(item4, value);
        }

        [BulkCopy]
        [NameGetter(nameof(GetWeaponTypeName))]
        public int Weapon1Type {
            get => Editor.GetByte(weapon1Type);
            set => Editor.SetByte(weapon1Type, (byte) value);
        }

        [BulkCopy]
        public int Weapon1Exp {
            get => Editor.GetWord(weapon1Exp);
            set => Editor.SetWord(weapon1Exp, value);
        }

        [BulkCopy]
        [NameGetter(nameof(GetWeaponTypeName))]
        public int Weapon2Type {
            get => Editor.GetByte(weapon2Type);
            set => Editor.SetByte(weapon2Type, (byte) value);
        }

        [BulkCopy]
        public int Weapon2Exp {
            get => Editor.GetWord(weapon2Exp);
            set => Editor.SetWord(weapon2Exp, value);
        }

        [BulkCopy]
        [NameGetter(nameof(GetWeaponTypeName))]
        public int Weapon3Type {
            get => Editor.GetByte(weapon3Type);
            set => Editor.SetByte(weapon3Type, (byte) value);
        }

        [BulkCopy]
        public int Weapon3Exp {
            get => Editor.GetWord(weapon3Exp);
            set => Editor.SetWord(weapon3Exp, value);
        }

        [BulkCopy]
        [NameGetter(nameof(GetWeaponTypeName))]
        public int Weapon4Type {
            get => Editor.GetByte(weapon4Type);
            set => Editor.SetByte(weapon4Type, (byte) value);
        }

        [BulkCopy]
        public int Weapon4Exp {
            get => Editor.GetWord(weapon4Exp);
            set => Editor.SetWord(weapon4Exp, value);
        }
    }
}
