using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X013 {
    public class Soulmate : Model {
        private readonly int chance;

        public Soulmate(IByteEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x01) {
            chance = Address; // 2 bytes
        }

        [BulkCopy]
        public int Chance {
            get => Editor.GetByte(chance);
            set => Editor.SetByte(chance, (byte) value);
        }
    }
}
