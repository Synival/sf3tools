using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X002 {
    public class Preset : IModel {
        private readonly int spell;
        private readonly int weaponLv0;
        private readonly int weaponLv1;
        private readonly int weaponLv2;
        private readonly int weaponLv3;

        public Preset(ISF3FileEditor editor, int id, string name, int address) {
            Editor  = editor;
            Name    = name;
            ID      = id;
            Address = address;
            Size    = 0x05;

            spell     = Address;     // 2 bytes
            weaponLv0 = Address + 1; // 1 byte
            weaponLv1 = Address + 2; // 1 byte
            weaponLv2 = Address + 3; // 1 byte
            weaponLv3 = Address + 4; // 1 byte
        }

        public IByteEditor Editor { get; }

        [BulkCopyRowName]
        public string Name { get; }
        public int ID { get; }
        public int Address { get; }
        public int Size { get; }

        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int SpellID2 {
            get => Editor.GetByte(spell);
            set => Editor.SetByte(spell, (byte) value);
        }

        [BulkCopy]
        public int Weapon0 {
            get => Editor.GetByte(weaponLv0);
            set => Editor.SetByte(weaponLv0, (byte) value);
        }

        [BulkCopy]
        public int Weapon1 {
            get => Editor.GetByte(weaponLv1);
            set => Editor.SetByte(weaponLv1, (byte) value);
        }

        [BulkCopy]
        public int Weapon2 {
            get => Editor.GetByte(weaponLv2);
            set => Editor.SetByte(weaponLv2, (byte) value);
        }

        [BulkCopy]
        public int Weapon3 {
            get => Editor.GetByte(weaponLv3);
            set => Editor.SetByte(weaponLv3, (byte) value);
        }
    }
}
