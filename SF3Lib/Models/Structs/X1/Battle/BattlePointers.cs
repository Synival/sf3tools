using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X1.Battle {
    public class BattlePointers : Struct {
        private readonly int battlePointer;

        public BattlePointers(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x04) {
            battlePointer = Address; // 2 bytes 
        }

        [TableViewModelColumn(displayOrder: 0, displayName: "Pointer", isPointer: true)]
        [BulkCopy]
        public int BattlePointer {
            get => Data.GetDouble(battlePointer);
            set => Data.SetDouble(battlePointer, value);
        }
    }
}
