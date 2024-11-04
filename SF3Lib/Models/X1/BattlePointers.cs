using CommonLib.Attributes;
using SF3.FileEditors;

namespace SF3.Models.X1 {
    public class BattlePointers : Model {
        private readonly int battlePointer;

        public BattlePointers(IByteEditor editor, int id, string name, int address)
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
