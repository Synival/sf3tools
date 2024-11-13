using CommonLib.Attributes;
using SF3.RawEditors;
using SF3.Models;

namespace SF3.Models.X002 {
    public class StatBoost : Model {
        private readonly int stat;

        public StatBoost(IRawEditor editor, int id, string name, int address)
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
