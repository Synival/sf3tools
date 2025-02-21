using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X1.Town {
    public class Enter : Struct {
        private readonly int enterID;   // 2 bytes
        private readonly int unknown2;  // 2 bytes
        private readonly int xPos;      // 2 bytes
        private readonly int unknown6;  // 2 bytes
        private readonly int zPos;      // 2 bytes
        private readonly int direction; // 2 bytes
        private readonly int camera;    // 2 bytes
        private readonly int unknownE;  // 2 bytes

        public Enter(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x10) {
            enterID   = Address;        // 2 bytes. how is searched. second by being 0x13 is a treasure. if this is 0xffff terminate 
            unknown2  = Address + 0x02; // unknown+0x02
            xPos      = Address + 0x04;
            unknown6  = Address + 0x06;
            zPos      = Address + 0x08;
            direction = Address + 0x0a;
            camera    = Address + 0x0c;
            unknownE  = Address + 0x0e;
        }

        [TableViewModelColumn(displayOrder: 0, displayName: "Scene#", displayFormat: "X2")]
        [BulkCopy]
        public int Entered {
            get => Data.GetWord(enterID);
            set => Data.SetWord(enterID, value);
        }

        [TableViewModelColumn(displayOrder: 1, displayName: "+0x02", displayFormat: "X2")]
        [BulkCopy]
        public int EnterUnknown2 {
            get => Data.GetWord(unknown2);
            set => Data.SetWord(unknown2, value);
        }

        [TableViewModelColumn(displayOrder: 2, displayName: "xPos")]
        [BulkCopy]
        public int EnterXPos {
            get => Data.GetWord(xPos);
            set => Data.SetWord(xPos, value);
        }

        [TableViewModelColumn(displayOrder: 3, displayName: "+0x06", displayFormat: "X2")]
        [BulkCopy]
        public int EnterUnknown6 {
            get => Data.GetWord(unknown6);
            set => Data.SetWord(unknown6, value);
        }

        [TableViewModelColumn(displayOrder: 4, displayName: "zPos")]
        [BulkCopy]
        public int EnterZPos {
            get => Data.GetWord(zPos);
            set => Data.SetWord(zPos, value);
        }

        [TableViewModelColumn(displayOrder: 5, displayName: "Direction", displayFormat: "X4")]
        [BulkCopy]
        public int EnterDirection {
            get => Data.GetWord(direction);
            set => Data.SetWord(direction, value);
        }

        [TableViewModelColumn(displayOrder: 6, displayName: "Camera", displayFormat: "X4")]
        [BulkCopy]
        public int EnterCamera {
            get => Data.GetWord(camera);
            set => Data.SetWord(camera, value);
        }

        [TableViewModelColumn(displayOrder: 7, displayName: "+0x0E", displayFormat: "X2")]
        [BulkCopy]
        public int EnterUnknownE {
            get => Data.GetWord(unknownE);
            set => Data.SetWord(unknownE, value);
        }
    }
}
