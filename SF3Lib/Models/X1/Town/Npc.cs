using CommonLib.Attributes;
using SF3.FileEditors;

namespace SF3.Models.X1.Town {
    public class Npc : Model {
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

        public Npc(IByteEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x18) {
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
            => Editor.GetWord(spriteID) > 0x0f && Editor.GetWord(spriteID) != 0xffff ? (ID + 0x3D).ToString("X") : "";

        [BulkCopy]
        public int SpriteID {
            get => Editor.GetWord(spriteID);
            set => Editor.SetWord(spriteID, value);
        }

        [BulkCopy]
        public int NpcUnknown {
            get => Editor.GetWord(unknown1);
            set => Editor.SetWord(unknown1, value);
        }

        [BulkCopy]
        public int NpcTable {
            get => Editor.GetDouble(table);
            set => Editor.SetDouble(table, value);
        }

        [BulkCopy]
        public int NpcXPos {
            get => Editor.GetWord(xPos);
            set => Editor.SetWord(xPos, value);
        }

        [BulkCopy]
        public int NpcZPos {
            get => Editor.GetWord(zPos);
            set => Editor.SetWord(zPos, value);
        }

        [BulkCopy]
        public int NpcDirection {
            get => Editor.GetWord(direction);
            set => Editor.SetWord(direction, value);
        }

        [BulkCopy]
        public int NpcUnknownA {
            get => Editor.GetWord(unknownA);
            set => Editor.SetWord(unknownA, value);
        }

        [BulkCopy]
        public int NpcUnknownC {
            get => Editor.GetWord(unknownC);
            set => Editor.SetWord(unknownC, value);
        }

        [BulkCopy]
        public int NpcUnknownE {
            get => Editor.GetWord(unknownE);
            set => Editor.SetWord(unknownE, value);
        }

        [BulkCopy]
        public int NpcUnknown12 {
            get => Editor.GetWord(unknown12);
            set => Editor.SetWord(unknown12, value);
        }

        [BulkCopy]
        public int NpcUnknown16 {
            get => Editor.GetWord(unknown16);
            set => Editor.SetWord(unknown16, value);
        }
    }
}
