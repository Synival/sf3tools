using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X002 {
    public class AttackResist : IModel {
        private readonly int attack;
        private readonly int resist;

        public AttackResist(ISF3FileEditor editor, int id, string name, int address) {
            Editor  = editor;
            Name    = name;
            ID      = id;
            Address = address;
            Size    = 0xD3;

            attack = Address;        // 1 byte
            resist = Address + 0xd2; // 1 byte
        }

        public IByteEditor Editor { get; }

        [BulkCopyRowName]
        public string Name { get; }
        public int ID { get; }
        public int Address { get; }
        public int Size { get; }

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
