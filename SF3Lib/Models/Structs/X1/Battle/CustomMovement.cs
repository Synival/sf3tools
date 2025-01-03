using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X1.Battle {
    public class CustomMovement : Struct {
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

        public CustomMovement(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x16) {
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
            get => Data.GetWord(unknown00);
            set => Data.SetWord(unknown00, value);
        }

        [BulkCopy]
        public int CustomMovementX1 {
            get => Data.GetWord(xPos1);
            set => Data.SetWord(xPos1, value);
        }

        [BulkCopy]
        public int CustomMovementZ1 {
            get => Data.GetWord(zPos1);
            set => Data.SetWord(zPos1, value);
        }

        [BulkCopy]
        public int CustomMovementX2 {
            get => Data.GetWord(xPos2);
            set => Data.SetWord(xPos2, value);
        }

        [BulkCopy]
        public int CustomMovementZ2 {
            get => Data.GetWord(zPos2);
            set => Data.SetWord(zPos2, value);
        }

        [BulkCopy]
        public int CustomMovementX3 {
            get => Data.GetWord(xPos3);
            set => Data.SetWord(xPos3, value);
        }

        [BulkCopy]
        public int CustomMovementZ3 {
            get => Data.GetWord(zPos3);
            set => Data.SetWord(zPos3, value);
        }

        [BulkCopy]
        public int CustomMovementX4 {
            get => Data.GetWord(xPos4);
            set => Data.SetWord(xPos4, value);
        }

        [BulkCopy]
        public int CustomMovementZ4 {
            get => Data.GetWord(zPos4);
            set => Data.SetWord(zPos4, value);
        }

        [BulkCopy]
        public int CustomMovementEnd {
            get => Data.GetDouble(ending);
            set => Data.SetDouble(ending, value);
        }
    }
}
