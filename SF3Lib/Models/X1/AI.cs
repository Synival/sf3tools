using CommonLib.Attributes;
using SF3.FileEditors;

namespace SF3.Models.X1 {
    public class AI : Model {
        private readonly int targetX;
        private readonly int targetY;

        public AI(IByteEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x04) {
            targetX = Address;     // 2 bytes
            targetY = Address + 2; // 2 bytes
        }

        [BulkCopy]
        public int TargetX {
            get => Editor.GetWord(targetX);
            set => Editor.SetWord(targetX, value);
        }

        [BulkCopy]
        public int TargetY {
            get => Editor.GetWord(targetY);
            set => Editor.SetWord(targetY, value);
        }
    }
}
