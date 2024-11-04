using CommonLib.Attributes;
using SF3.FileEditors;

namespace SF3.Models {
    public class StatBoost : Model {
        private readonly int stat;

        public StatBoost(IByteEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x01) {
            stat = Address; // 1 byte
        }

        [BulkCopy]
        public int Stat {
            get => Editor.GetByte(stat);
            set => Editor.SetByte(stat, (byte) value);
        }
    }
}
