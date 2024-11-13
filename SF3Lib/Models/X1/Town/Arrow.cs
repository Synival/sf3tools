using CommonLib.Attributes;
using SF3.RawEditors;

namespace SF3.Models.X1.Town {
    public class Arrow : Model {
        private readonly int unknown0; //2 byte
        private readonly int textID; //2 byte
        private readonly int unknown4; //2 byte
        private readonly int warpInMPD; //2 byte
        private readonly int unknown8; //2 byte
        private readonly int unknownA; //2 byte

        public Arrow(IRawEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x0c) {
            unknown0  = Address; //2 bytes. how is searched. second by being 0x13 is a treasure. if this is 0xffff terminate 
            textID    = Address + 0x02;
            unknown4  = Address + 0x04;
            warpInMPD = Address + 0x06;
            unknown8  = Address + 0x08;
            unknownA  = Address + 0x0a;
        }

        [BulkCopy]
        public int ArrowUnknown0 {
            get => Editor.GetWord(unknown0);
            set => Editor.SetWord(unknown0, value);
        }

        [BulkCopy]
        public int ArrowText {
            get => Editor.GetWord(textID);
            set => Editor.SetWord(textID, value);
        }

        [BulkCopy]
        public int ArrowUnknown4 {
            get => Editor.GetWord(unknown4);
            set => Editor.SetWord(unknown4, value);
        }

        [BulkCopy]
        public int ArrowWarp {
            get => Editor.GetWord(warpInMPD);
            set => Editor.SetWord(warpInMPD, value);
        }

        [BulkCopy]
        public int ArrowUnknown8 {
            get => Editor.GetWord(unknown8);
            set => Editor.SetWord(unknown8, value);
        }

        [BulkCopy]
        public int ArrowUnknownA {
            get => Editor.GetWord(unknownA);
            set => Editor.SetWord(unknownA, value);
        }
    }
}
