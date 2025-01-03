using CommonLib.Attributes;
using SF3.Models.Structs;
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
            table     = Address + 0x04;
            xPos      = Address + 0x08;
            unknownA  = Address + 0x0a;
            unknownC  = Address + 0x0c;
            unknownE  = Address + 0x0e;
            zPos      = Address + 0x10;
            unknown12 = Address + 0x12;
            direction = Address + 0x14;
            unknown16 = Address + 0x16;
        }

        public string NpcTieIn
            => Data.GetWord(spriteID) > 0x0f && Data.GetWord(spriteID) != 0xffff ? (ID + 0x3D).ToString("X") : "";

        [BulkCopy]
        public int SpriteID {
            get => Data.GetWord(spriteID);
            set => Data.SetWord(spriteID, value);
        }

        [BulkCopy]
        public int NpcUnknown {
            get => Data.GetWord(unknown1);
            set => Data.SetWord(unknown1, value);
        }

        [BulkCopy]
        public int NpcTable {
            get => Data.GetDouble(table);
            set => Data.SetDouble(table, value);
        }

        [BulkCopy]
        public int NpcXPos {
            get => Data.GetWord(xPos);
            set => Data.SetWord(xPos, value);
        }

        [BulkCopy]
        public int NpcZPos {
            get => Data.GetWord(zPos);
            set => Data.SetWord(zPos, value);
        }

        [BulkCopy]
        public int NpcDirection {
            get => Data.GetWord(direction);
            set => Data.SetWord(direction, value);
        }

        [BulkCopy]
        public int NpcUnknownA {
            get => Data.GetWord(unknownA);
            set => Data.SetWord(unknownA, value);
        }

        [BulkCopy]
        public int NpcUnknownC {
            get => Data.GetWord(unknownC);
            set => Data.SetWord(unknownC, value);
        }

        [BulkCopy]
        public int NpcUnknownE {
            get => Data.GetWord(unknownE);
            set => Data.SetWord(unknownE, value);
        }

        [BulkCopy]
        public int NpcUnknown12 {
            get => Data.GetWord(unknown12);
            set => Data.SetWord(unknown12, value);
        }

        [BulkCopy]
        public int NpcUnknown16 {
            get => Data.GetWord(unknown16);
            set => Data.SetWord(unknown16, value);
        }
    }
}
