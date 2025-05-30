using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X033_X031 {
    public class InitialInfo : Struct {
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

        public InitialInfo(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x20) {
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

        [TableViewModelColumn(displayOrder: 0, minWidth: 100, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Character)]
        public int CharacterID {
            get => Data.GetByte(character);
            set => Data.SetByte(character, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 1, displayName: "Class", minWidth: 150, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.CharacterClass)]
        public int CharacterClass {
            get => Data.GetByte(characterClass);
            set => Data.SetByte(characterClass, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 2)]
        [BulkCopy]
        public int Level {
            get => Data.GetByte(level);
            set => Data.SetByte(level, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 3, minWidth: 100, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Sex)]
        public int Sex {
            get => Data.GetByte(sex);
            set => Data.SetByte(sex, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 4, minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public int Weapon {
            get => Data.GetWord(weapon);
            set => Data.SetWord(weapon, value);
        }

        [TableViewModelColumn(displayOrder: 5, minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public int Accessory {
            get => Data.GetWord(accessory);
            set => Data.SetWord(accessory, value);
        }

        [TableViewModelColumn(displayOrder: 6, minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public int Item1 {
            get => Data.GetWord(item1);
            set => Data.SetWord(item1, value);
        }

        [TableViewModelColumn(displayOrder: 7, minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public int Item2 {
            get => Data.GetWord(item2);
            set => Data.SetWord(item2, value);
        }

        [TableViewModelColumn(displayOrder: 8, minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public int Item3 {
            get => Data.GetWord(item3);
            set => Data.SetWord(item3, value);
        }

        [TableViewModelColumn(displayOrder: 9, minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public int Item4 {
            get => Data.GetWord(item4);
            set => Data.SetWord(item4, value);
        }

        [TableViewModelColumn(displayOrder: 10, minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.WeaponType)]
        public int Weapon1Type {
            get => Data.GetByte(weapon1Type);
            set => Data.SetByte(weapon1Type, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 11)]
        [BulkCopy]
        public int Weapon1Exp {
            get => Data.GetWord(weapon1Exp);
            set => Data.SetWord(weapon1Exp, value);
        }

        [TableViewModelColumn(displayOrder: 12, minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.WeaponType)]
        public int Weapon2Type {
            get => Data.GetByte(weapon2Type);
            set => Data.SetByte(weapon2Type, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 13)]
        [BulkCopy]
        public int Weapon2Exp {
            get => Data.GetWord(weapon2Exp);
            set => Data.SetWord(weapon2Exp, value);
        }

        [TableViewModelColumn(displayOrder: 14, minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.WeaponType)]
        public int Weapon3Type {
            get => Data.GetByte(weapon3Type);
            set => Data.SetByte(weapon3Type, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 15)]
        [BulkCopy]
        public int Weapon3Exp {
            get => Data.GetWord(weapon3Exp);
            set => Data.SetWord(weapon3Exp, value);
        }

        [TableViewModelColumn(displayOrder: 16, minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.WeaponType)]
        public int Weapon4Type {
            get => Data.GetByte(weapon4Type);
            set => Data.SetByte(weapon4Type, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 17)]
        [BulkCopy]
        public int Weapon4Exp {
            get => Data.GetWord(weapon4Exp);
            set => Data.SetWord(weapon4Exp, value);
        }
    }
}
