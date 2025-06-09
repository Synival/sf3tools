using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X033_X031 {
    public class InitialInfo : Struct {
        //starting equipment table
        private readonly int _characterIdAddr;
        private readonly int _characterClassAddr;
        private readonly int _levelAddr;
        private readonly int _sexAddr;
        private readonly int _weaponAddr; //2 bytes
        private readonly int _accessoryAddr; //2 bytes
        private readonly int _item1Addr; //2 bytes
        private readonly int _item2Addr; //2 bytes
        private readonly int _item3Addr; //2 bytes
        private readonly int _item4Addr; //2 bytes
        private readonly int _weapon1TypeAddr; //for exp
        private readonly int _weapon1ExpAddr; //2 bytes
        private readonly int _weapon2TypeAddr; //for exp
        private readonly int _weapon2ExpAddr; //2 bytes
        private readonly int _weapon3TypeAddr; //for exp
        private readonly int _weapon3ExpAddr; //2 bytes
        private readonly int _weapon4TypeAddr; // for exp
        private readonly int _weapon4ExpAddr; //2 bytes

        public InitialInfo(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x20) {
            _characterIdAddr    = Address + 0x00;
            _characterClassAddr = Address + 0x01;
            _levelAddr          = Address + 0x02;
            _sexAddr            = Address + 0x03;
            _weaponAddr         = Address + 0x04; // 2 bytes
            _accessoryAddr      = Address + 0x06; // 2 bytes
            _item1Addr          = Address + 0x08; // 2 bytes
            _item2Addr          = Address + 0x0a; // 2 bytes
            _item3Addr          = Address + 0x0c; // 2 bytes
            _item4Addr          = Address + 0x0e; // 2 bytes
            _weapon1TypeAddr    = Address + 0x10; // 2 bytes
            _weapon1ExpAddr     = Address + 0x12; // 2 bytes
            _weapon2TypeAddr    = Address + 0x14; // 2 bytes
            _weapon2ExpAddr     = Address + 0x16; // 2 bytes
            _weapon3TypeAddr    = Address + 0x18; // 2 bytes
            _weapon3ExpAddr     = Address + 0x1a; // 2 bytes
            _weapon4TypeAddr    = Address + 0x1c; // 2 bytes
            _weapon4ExpAddr     = Address + 0x1e; // 2 bytes
        }

        [TableViewModelColumn(addressField: nameof(_characterIdAddr), displayOrder: 0, minWidth: 100, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Character)]
        public int CharacterID {
            get => Data.GetByte(_characterIdAddr);
            set => Data.SetByte(_characterIdAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_characterClassAddr), displayOrder: 1, displayName: "Class", minWidth: 150, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.CharacterClass)]
        public int CharacterClass {
            get => Data.GetByte(_characterClassAddr);
            set => Data.SetByte(_characterClassAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_levelAddr), displayOrder: 2)]
        [BulkCopy]
        public int Level {
            get => Data.GetByte(_levelAddr);
            set => Data.SetByte(_levelAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_sexAddr), displayOrder: 3, minWidth: 100, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Sex)]
        public int Sex {
            get => Data.GetByte(_sexAddr);
            set => Data.SetByte(_sexAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_weaponAddr), displayOrder: 4, minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public int Weapon {
            get => Data.GetWord(_weaponAddr);
            set => Data.SetWord(_weaponAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_accessoryAddr), displayOrder: 5, minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public int Accessory {
            get => Data.GetWord(_accessoryAddr);
            set => Data.SetWord(_accessoryAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_item1Addr), displayOrder: 6, minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public int Item1 {
            get => Data.GetWord(_item1Addr);
            set => Data.SetWord(_item1Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_item2Addr), displayOrder: 7, minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public int Item2 {
            get => Data.GetWord(_item2Addr);
            set => Data.SetWord(_item2Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_item3Addr), displayOrder: 8, minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public int Item3 {
            get => Data.GetWord(_item3Addr);
            set => Data.SetWord(_item3Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_item4Addr), displayOrder: 9, minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public int Item4 {
            get => Data.GetWord(_item4Addr);
            set => Data.SetWord(_item4Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_weapon1TypeAddr), displayOrder: 10, minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.WeaponType)]
        public int Weapon1Type {
            get => Data.GetByte(_weapon1TypeAddr);
            set => Data.SetByte(_weapon1TypeAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_weapon1ExpAddr), displayOrder: 11)]
        [BulkCopy]
        public int Weapon1Exp {
            get => Data.GetWord(_weapon1ExpAddr);
            set => Data.SetWord(_weapon1ExpAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_weapon2TypeAddr), displayOrder: 12, minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.WeaponType)]
        public int Weapon2Type {
            get => Data.GetByte(_weapon2TypeAddr);
            set => Data.SetByte(_weapon2TypeAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_weapon2ExpAddr), displayOrder: 13)]
        [BulkCopy]
        public int Weapon2Exp {
            get => Data.GetWord(_weapon2ExpAddr);
            set => Data.SetWord(_weapon2ExpAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_weapon3TypeAddr), displayOrder: 14, minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.WeaponType)]
        public int Weapon3Type {
            get => Data.GetByte(_weapon3TypeAddr);
            set => Data.SetByte(_weapon3TypeAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_weapon3ExpAddr), displayOrder: 15)]
        [BulkCopy]
        public int Weapon3Exp {
            get => Data.GetWord(_weapon3ExpAddr);
            set => Data.SetWord(_weapon3ExpAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_weapon4TypeAddr), displayOrder: 16, minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.WeaponType)]
        public int Weapon4Type {
            get => Data.GetByte(_weapon4TypeAddr);
            set => Data.SetByte(_weapon4TypeAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_weapon4ExpAddr), displayOrder: 17)]
        [BulkCopy]
        public int Weapon4Exp {
            get => Data.GetWord(_weapon4ExpAddr);
            set => Data.SetWord(_weapon4ExpAddr, value);
        }
    }
}
