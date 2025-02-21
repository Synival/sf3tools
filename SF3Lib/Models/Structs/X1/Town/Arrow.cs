using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X1.Town {
    public class Arrow : Struct {
        private readonly int unknown0; //2 byte
        private readonly int textID; //2 byte
        private readonly int unknown4; //2 byte
        private readonly int warpInMPD; //2 byte
        private readonly int unknown8; //2 byte
        private readonly int unknownA; //2 byte

        public Arrow(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x0c) {
            unknown0  = Address; //2 bytes. how is searched. second by being 0x13 is a treasure. if this is 0xffff terminate 
            textID    = Address + 0x02;
            unknown4  = Address + 0x04;
            warpInMPD = Address + 0x06;
            unknown8  = Address + 0x08;
            unknownA  = Address + 0x0a;
        }

        [TableViewModelColumn(displayOrder: 0, displayName: "+0x00", displayFormat: "X2")]
        [BulkCopy]
        public int ArrowUnknown0 {
            get => Data.GetWord(unknown0);
            set => Data.SetWord(unknown0, value);
        }

        [TableViewModelColumn(displayOrder: 1, displayName: "TextID", displayFormat: "X4")]
        [BulkCopy]
        public int ArrowText {
            get => Data.GetWord(textID);
            set => Data.SetWord(textID, value);
        }

        [TableViewModelColumn(displayOrder: 2, displayName: "+0x04", displayFormat: "X2")]
        [BulkCopy]
        public int ArrowUnknown4 {
            get => Data.GetWord(unknown4);
            set => Data.SetWord(unknown4, value);
        }

        [TableViewModelColumn(displayOrder: 3, displayName: "PointToWarpMPD", displayFormat: "X2")]
        [BulkCopy]
        public int ArrowWarp {
            get => Data.GetWord(warpInMPD);
            set => Data.SetWord(warpInMPD, value);
        }

        [TableViewModelColumn(displayOrder: 4, displayName: "+0x08", displayFormat: "X2")]
        [BulkCopy]
        public int ArrowUnknown8 {
            get => Data.GetWord(unknown8);
            set => Data.SetWord(unknown8, value);
        }

        [TableViewModelColumn(displayOrder: 5, displayName: "+0x0A", displayFormat: "X2")]
        [BulkCopy]
        public int ArrowUnknownA {
            get => Data.GetWord(unknownA);
            set => Data.SetWord(unknownA, value);
        }
    }
}
