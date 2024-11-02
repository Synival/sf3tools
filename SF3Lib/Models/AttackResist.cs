using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models {
    public class AttackResist : Model {
        private readonly int attack;
        private readonly int resist;

        public AttackResist(ISF3FileEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0xD3) {
            attack = Address;        // 1 byte
            resist = Address + 0xd2; // 1 byte
        }

        [BulkCopy]
        public int Attack {
            get => Editor.GetByte(attack);
            set => Editor.SetByte(attack, (byte) value);
        }

        [BulkCopy]
        public int Resist {
            get => Editor.GetByte(resist);
            set => Editor.SetByte(resist, (byte) value);
        }
    }
}
