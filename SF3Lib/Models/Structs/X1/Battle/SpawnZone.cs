using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.RawData;

namespace SF3.Models.Structs.X1.Battle {
    public class SpawnZone : Struct {
        private readonly int unknown00;
        private readonly int unknown02;
        private readonly int unknown04;
        private readonly int unknown06;
        private readonly int unknown08;
        private readonly int unknown0A;
        private readonly int unknown0C;
        private readonly int unknown0E;
        private readonly int unknown10;

        public SpawnZone(IRawData editor, int id, string name, int address)
        : base(editor, id, name, address, 0x12) {
            unknown00 = Address;        // 2 bytes  
            unknown02 = Address + 0x02; // 2 bytes
            unknown04 = Address + 0x04; // 2 bytes
            unknown06 = Address + 0x06; // 2 bytes
            unknown08 = Address + 0x08; // 2 bytes
            unknown0A = Address + 0x0a; // 2 bytes
            unknown0C = Address + 0x0c; // 2 bytes
            unknown0E = Address + 0x0e; // 2 bytes
            unknown10 = Address + 0x10; // 2 bytes
        }

        [BulkCopy]
        public int UnknownAI00 {
            get => Editor.GetWord(unknown00);
            set => Editor.SetWord(unknown00, value);
        }

        [BulkCopy]
        public int UnknownAI02 {
            get => Editor.GetWord(unknown02);
            set => Editor.SetWord(unknown02, value);
        }

        [BulkCopy]
        public int UnknownAI04 {
            get => Editor.GetWord(unknown04);
            set => Editor.SetWord(unknown04, value);
        }

        [BulkCopy]
        public int UnknownAI06 {
            get => Editor.GetWord(unknown06);
            set => Editor.SetWord(unknown06, value);
        }

        [BulkCopy]
        public int UnknownAI08 {
            get => Editor.GetWord(unknown08);
            set => Editor.SetWord(unknown08, value);
        }

        [BulkCopy]
        public int UnknownAI0A {
            get => Editor.GetWord(unknown0A);
            set => Editor.SetWord(unknown0A, value);
        }

        [BulkCopy]
        public int UnknownAI0C {
            get => Editor.GetWord(unknown0C);
            set => Editor.SetWord(unknown0C, value);
        }

        [BulkCopy]
        public int UnknownAI0E {
            get => Editor.GetWord(unknown0E);
            set => Editor.SetWord(unknown0E, value);
        }

        [BulkCopy]
        public int UnknownAI10 {
            get => Editor.GetWord(unknown10);
            set => Editor.SetWord(unknown10, value);
        }
    }
}
