using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.RawData;

namespace SF3.Models.Structs.X1.Battle {
    public class AI : Struct {
        private readonly int targetX;
        private readonly int targetY;

        public AI(IRawData editor, int id, string name, int address)
        : base(editor, id, name, address, 0x04) {
            targetX = Address;     // 2 bytes
            targetY = Address + 2; // 2 bytes
        }

        [BulkCopy]
        public int TargetX {
            get => Data.GetWord(targetX);
            set => Data.SetWord(targetX, value);
        }

        [BulkCopy]
        public int TargetY {
            get => Data.GetWord(targetY);
            set => Data.SetWord(targetY, value);
        }
    }
}
