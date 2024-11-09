using CommonLib.Attributes;
using SF3.FileEditors;

namespace SF3.Models.X013 {
    public class Soulfail : Model {
        private readonly int expLost;

        public Soulfail(IByteEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x01) {
            expLost = Address; // 1 bytes
        }

        [BulkCopy]
        public int ExpLost {
            get => Editor.GetByte(expLost);
            set => Editor.SetByte(expLost, (byte) value);
        }
    }
}
