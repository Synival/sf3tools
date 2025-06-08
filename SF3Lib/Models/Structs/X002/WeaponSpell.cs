using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X002 {
    public class WeaponSpell : Struct {
        private readonly int _spellAddr;
        private readonly int _weaponLv0Addr;
        private readonly int _weaponLv1Addr;
        private readonly int _weaponLv2Addr;
        private readonly int _weaponLv3Addr;

        public WeaponSpell(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x05) {
            _spellAddr     = Address;     // 1 byte
            _weaponLv0Addr = Address + 1; // 1 byte
            _weaponLv1Addr = Address + 2; // 1 byte
            _weaponLv2Addr = Address + 3; // 1 byte
            _weaponLv3Addr = Address + 4; // 1 byte
        }

        [TableViewModelColumn(addressField: nameof(_spellAddr), displayOrder: 0, minWidth: 100, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int Spell {
            get => Data.GetByte(_spellAddr);
            set => Data.SetByte(_spellAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_weaponLv0Addr), displayOrder: 1)]
        [BulkCopy]
        public int WeaponLv0 {
            get => Data.GetByte(_weaponLv0Addr);
            set => Data.SetByte(_weaponLv0Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_weaponLv1Addr), displayOrder: 2)]
        [BulkCopy]
        public int WeaponLv1 {
            get => Data.GetByte(_weaponLv1Addr);
            set => Data.SetByte(_weaponLv1Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_weaponLv2Addr), displayOrder: 3)]
        [BulkCopy]
        public int WeaponLv2 {
            get => Data.GetByte(_weaponLv2Addr);
            set => Data.SetByte(_weaponLv2Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_weaponLv3Addr), displayOrder: 4)]
        [BulkCopy]
        public int WeaponLv3 {
            get => Data.GetByte(_weaponLv3Addr);
            set => Data.SetByte(_weaponLv3Addr, (byte) value);
        }
    }
}
