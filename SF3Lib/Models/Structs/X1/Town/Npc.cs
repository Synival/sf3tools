using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X1.Town {
    public class Npc : Struct {
        private readonly int spriteID;
        private readonly int unknown1;
        private readonly int table;
        private readonly int xPos;
        private readonly int zPos;
        private readonly int direction;
        private readonly int unknownA;
        private readonly int unknownC;
        private readonly int unknownE;
        private readonly int unknown12;
        private readonly int unknown16;

        public Npc(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x18) {
            spriteID  = Address;        // 2 bytes. how is searched. second by being 0x13 is a treasure. if this is 0xffff terminate 
            unknown1  = Address + 0x02; // unknown + 0x02
            table     = Address + 0x04; // 2 bytes
            xPos      = Address + 0x08; // 2 bytes
            unknownA  = Address + 0x0a; // 2 bytes
            unknownC  = Address + 0x0c; // 2 bytes
            unknownE  = Address + 0x0e; // 2 bytes
            zPos      = Address + 0x10; // 2 bytes
            unknown12 = Address + 0x12; // 2 bytes
            direction = Address + 0x14; // 2 bytes
            unknown16 = Address + 0x16; // 2 bytes
        }

        [TableViewModelColumn(displayOrder: 0, displayFormat: "X3")]
        [BulkCopy]
        public int SpriteID {
            get => Data.GetWord(spriteID);
            set => Data.SetWord(spriteID, value);
        }

        [TableViewModelColumn(displayOrder: 1, displayName: "+0x02", displayFormat: "X4")]
        [BulkCopy]
        public int NpcUnknown {
            get => Data.GetWord(unknown1);
            set => Data.SetWord(unknown1, value);
        }

        [TableViewModelColumn(displayOrder: 2, displayName: "MovementTable?", isPointer: true)]
        [BulkCopy]
        public int NpcTable {
            get => Data.GetDouble(table);
            set => Data.SetDouble(table, value);
        }

        [TableViewModelColumn(displayOrder: 3, displayName: "xPos")]
        [BulkCopy]
        public int NpcXPos {
            get => Data.GetWord(xPos);
            set => Data.SetWord(xPos, value);
        }

        [TableViewModelColumn(displayOrder: 4, displayName: "+0x0A", displayFormat: "X2")]
        [BulkCopy]
        public int NpcUnknownA {
            get => Data.GetWord(unknownA);
            set => Data.SetWord(unknownA, value);
        }

        [TableViewModelColumn(displayOrder: 5, displayName: "+0x0C", displayFormat: "X2")]
        [BulkCopy]
        public int NpcUnknownC {
            get => Data.GetWord(unknownC);
            set => Data.SetWord(unknownC, value);
        }

        [TableViewModelColumn(displayOrder: 6, displayName: "+0x0E", displayFormat: "X2")]
        [BulkCopy]
        public int NpcUnknownE {
            get => Data.GetWord(unknownE);
            set => Data.SetWord(unknownE, value);
        }

        [TableViewModelColumn(displayOrder: 7, displayName: "zPos")]
        [BulkCopy]
        public int NpcZPos {
            get => Data.GetWord(zPos);
            set => Data.SetWord(zPos, value);
        }

        [TableViewModelColumn(displayOrder: 8, displayName: "0x12", displayFormat: "X2")]
        [BulkCopy]
        public int NpcUnknown12 {
            get => Data.GetWord(unknown12);
            set => Data.SetWord(unknown12, value);
        }

        [TableViewModelColumn(displayOrder: 9, displayName: "Direction", displayFormat: "X4")]
        [BulkCopy]
        public int NpcDirection {
            get => Data.GetWord(direction);
            set => Data.SetWord(direction, value);
        }

        [TableViewModelColumn(displayOrder: 10, displayName: "+0x16", displayFormat: "X2")]
        [BulkCopy]
        public int NpcUnknown16 {
            get => Data.GetWord(unknown16);
            set => Data.SetWord(unknown16, value);
        }

        [TableViewModelColumn(displayOrder: 11, displayName: "TiedToEventNumber")]
        public string NpcTieIn
            => Data.GetWord(spriteID) > 0x0f && Data.GetWord(spriteID) != 0xffff ? (ID + 0x3D).ToString("X") : "";
    }
}
