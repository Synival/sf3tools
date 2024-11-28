using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.RawData;

namespace SF3.Models.Structs.X1.Battle {
    public class BattlePointers : Struct {
        private readonly int battlePointer;

        public BattlePointers(IRawData editor, int id, string name, int address)
        : base(editor, id, name, address, 0x04) {
            battlePointer = Address; // 2 bytes 
        }

        [BulkCopy]
        public int BattlePointer {
            get => Data.GetDouble(battlePointer);
            set => Data.SetDouble(battlePointer, value);
        }
    }
}
