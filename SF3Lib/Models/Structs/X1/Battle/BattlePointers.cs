using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.RawEditors;

namespace SF3.Models.Structs.X1.Battle {
    public class BattlePointers : Struct {
        private readonly int battlePointer;

        public BattlePointers(IRawEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x04) {
            battlePointer = Address; // 2 bytes 
        }

        [BulkCopy]
        public int BattlePointer {
            get => Editor.GetDouble(battlePointer);
            set => Editor.SetDouble(battlePointer, value);
        }
    }
}
