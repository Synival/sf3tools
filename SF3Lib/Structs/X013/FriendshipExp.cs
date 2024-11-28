using CommonLib.Attributes;
using SF3.RawEditors;

namespace SF3.Structs.X013 {
    public class FriendshipExp : Struct {
        private readonly int sLvl0;
        private readonly int sLvl1;
        private readonly int sLvl2;
        private readonly int sLvl3;
        private readonly int sLvl4;

        public FriendshipExp(IRawEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x04) {
            sLvl0 = Address;     // 1 byte
            sLvl1 = Address + 1; // 1 byte
            sLvl2 = Address + 2; // 1 byte
            sLvl3 = Address + 3; // 1 byte
            sLvl4 = Address + 4; // 1 byte
        }

        [BulkCopy]
        public int SLvl0 {
            get => Editor.GetByte(sLvl0);
            set => Editor.SetByte(sLvl0, (byte) value);
        }

        [BulkCopy]
        public int SLvl1 {
            get => Editor.GetByte(sLvl1);
            set => Editor.SetByte(sLvl1, (byte) value);
        }

        [BulkCopy]
        public int SLvl2 {
            get => Editor.GetByte(sLvl2);
            set => Editor.SetByte(sLvl2, (byte) value);
        }

        [BulkCopy]
        public int SLvl3 {
            get => Editor.GetByte(sLvl3);
            set => Editor.SetByte(sLvl3, (byte) value);
        }

        [BulkCopy]
        public int SLvl4 {
            get => Editor.GetByte(sLvl4);
            set => Editor.SetByte(sLvl4, (byte) value);
        }
    }
}
