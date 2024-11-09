using CommonLib.Attributes;
using SF3.FileEditors;

namespace SF3.Models.X1.Town {
    public class Enter : Model {
        private readonly int enterID;   // 2 bytes
        private readonly int unknown2;  // 2 bytes
        private readonly int xPos;      // 2 bytes
        private readonly int unknown6;  // 2 bytes
        private readonly int zPos;      // 2 bytes
        private readonly int direction; // 2 bytes
        private readonly int camera;    // 2 bytes
        private readonly int unknownE;  // 2 bytes

        public Enter(IByteEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x10) {
            enterID   = Address;        // 2 bytes. how is searched. second by being 0x13 is a treasure. if this is 0xffff terminate 
            unknown2  = Address + 0x02; // unknown+0x02
            xPos      = Address + 0x04;
            unknown6  = Address + 0x06;
            zPos      = Address + 0x08;
            direction = Address + 0x0a;
            camera    = Address + 0x0c;
            unknownE  = Address + 0x0e;
        }

        [BulkCopy]
        public int Entered {
            get => Editor.GetWord(enterID);
            set => Editor.SetWord(enterID, value);
        }

        [BulkCopy]
        public int EnterUnknown2 {
            get => Editor.GetWord(unknown2);
            set => Editor.SetWord(unknown2, value);
        }

        [BulkCopy]
        public int EnterXPos {
            get => Editor.GetWord(xPos);
            set => Editor.SetWord(xPos, value);
        }

        [BulkCopy]
        public int EnterUnknown6 {
            get => Editor.GetWord(unknown6);
            set => Editor.SetWord(unknown6, value);
        }

        [BulkCopy]
        public int EnterZPos {
            get => Editor.GetWord(zPos);
            set => Editor.SetWord(zPos, value);
        }

        [BulkCopy]
        public int EnterDirection {
            get => Editor.GetWord(direction);
            set => Editor.SetWord(direction, value);
        }

        [BulkCopy]
        public int EnterCamera {
            get => Editor.GetWord(camera);
            set => Editor.SetWord(camera, value);
        }

        [BulkCopy]
        public int EnterUnknownE {
            get => Editor.GetWord(unknownE);
            set => Editor.SetWord(unknownE, value);
        }
    }
}
