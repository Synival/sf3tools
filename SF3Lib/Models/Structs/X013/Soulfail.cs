using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.RawData;

namespace SF3.Models.Structs.X013 {
    public class Soulfail : Struct {
        private readonly int expLost;

        public Soulfail(IRawData editor, int id, string name, int address)
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
