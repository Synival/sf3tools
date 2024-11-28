using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.RawData;

namespace SF3.Models.Structs.X002 {
    public class StatBoost : Struct {
        private readonly int stat;

        public StatBoost(IRawData editor, int id, string name, int address)
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