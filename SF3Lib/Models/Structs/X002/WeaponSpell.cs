using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X002 {
    public class WeaponSpell : Struct {
        private readonly int spell;
        private readonly int weaponLv0;
        private readonly int weaponLv1;
        private readonly int weaponLv2;
        private readonly int weaponLv3;

        public WeaponSpell(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x05) {
            spell     = Address;     // 2 bytes
            weaponLv0 = Address + 1; // 1 byte
            weaponLv1 = Address + 2; // 1 byte
            weaponLv2 = Address + 3; // 1 byte
            weaponLv3 = Address + 4; // 1 byte
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int SpellID2 {
            get => Data.GetByte(spell);
            set => Data.SetByte(spell, (byte) value);
        }

        [BulkCopy]
        public int Weapon0 {
            get => Data.GetByte(weaponLv0);
            set => Data.SetByte(weaponLv0, (byte) value);
        }

        [BulkCopy]
        public int Weapon1 {
            get => Data.GetByte(weaponLv1);
            set => Data.SetByte(weaponLv1, (byte) value);
        }

        [BulkCopy]
        public int Weapon2 {
            get => Data.GetByte(weaponLv2);
            set => Data.SetByte(weaponLv2, (byte) value);
        }

        [BulkCopy]
        public int Weapon3 {
            get => Data.GetByte(weaponLv3);
            set => Data.SetByte(weaponLv3, (byte) value);
        }
    }
}
