using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X1.Battle {
    public class AITargetPosition : Struct {
        private readonly int targetX;
        private readonly int targetY;

        public AITargetPosition(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x04) {
            targetX = Address;     // 2 bytes
            targetY = Address + 2; // 2 bytes
        }

        [TableViewModelColumn(displayOrder: 0)]
        [BulkCopy]
        public int TargetX {
            get => Data.GetWord(targetX);
            set => Data.SetWord(targetX, value);
        }

        [TableViewModelColumn(displayOrder: 1)]
        [BulkCopy]
        public int TargetY {
            get => Data.GetWord(targetY);
            set => Data.SetWord(targetY, value);
        }
    }
}
