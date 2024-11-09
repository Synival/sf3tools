using CommonLib.Attributes;
using SF3.FileEditors;

namespace SF3.Models.X1_Battle {
    public class CustomMovement : Model {
        private readonly int unknown00;
        private readonly int xPos1;
        private readonly int zPos1;
        private readonly int xPos2;
        private readonly int zPos2;
        private readonly int xPos3;
        private readonly int zPos3;
        private readonly int xPos4;
        private readonly int zPos4;
        private readonly int ending;

        public CustomMovement(IByteEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x16) {
            unknown00 = Address;      // 2 bytes
            xPos1     = Address +  2; // 2 bytes
            zPos1     = Address +  4; // 2 bytes
            xPos2     = Address +  6; // 2 bytes
            zPos2     = Address +  8; // 2 bytes
            xPos3     = Address + 10; // 2 bytes
            zPos3     = Address + 12; // 2 bytes
            xPos4     = Address + 14; // 2 bytes
            zPos4     = Address + 16; // 2 bytes
            ending    = Address + 18; // 4 bytes
        }

        [BulkCopy]
        public int CustomMovementUnknown {
            get => Editor.GetWord(unknown00);
            set => Editor.SetWord(unknown00, value);
        }

        [BulkCopy]
        public int CustomMovementX1 {
            get => Editor.GetWord(xPos1);
            set => Editor.SetWord(xPos1, value);
        }

        [BulkCopy]
        public int CustomMovementZ1 {
            get => Editor.GetWord(zPos1);
            set => Editor.SetWord(zPos1, value);
        }

        [BulkCopy]
        public int CustomMovementX2 {
            get => Editor.GetWord(xPos2);
            set => Editor.SetWord(xPos2, value);
        }

        [BulkCopy]
        public int CustomMovementZ2 {
            get => Editor.GetWord(zPos2);
            set => Editor.SetWord(zPos2, value);
        }

        [BulkCopy]
        public int CustomMovementX3 {
            get => Editor.GetWord(xPos3);
            set => Editor.SetWord(xPos3, value);
        }

        [BulkCopy]
        public int CustomMovementZ3 {
            get => Editor.GetWord(zPos3);
            set => Editor.SetWord(zPos3, value);
        }

        [BulkCopy]
        public int CustomMovementX4 {
            get => Editor.GetWord(xPos4);
            set => Editor.SetWord(xPos4, value);
        }

        [BulkCopy]
        public int CustomMovementZ4 {
            get => Editor.GetWord(zPos4);
            set => Editor.SetWord(zPos4, value);
        }

        [BulkCopy]
        public int CustomMovementEnd {
            get => Editor.GetDouble(ending);
            set => Editor.SetDouble(ending, value);
        }
    }
}
