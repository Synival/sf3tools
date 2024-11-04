using CommonLib.Attributes;
using SF3.FileEditors;

namespace SF3.Models {
    public class CritMod : Model {
        private readonly int advantage;
        private readonly int disadvantage;
        private readonly int offset;
        private readonly int checkVersion2;

        public CritMod(IByteEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x12) {
            advantage    = Address + 0x01; // 1 byte
            disadvantage = Address + 0x11; // 1 byte
        }

        [BulkCopy]
        public int Advantage {
            get => Editor.GetByte(advantage);
            set => Editor.SetByte(advantage, (byte) value);
        }

        [BulkCopy]
        public int Disadvantage {
            get => Editor.GetByte(disadvantage);
            set => Editor.SetByte(disadvantage, (byte) value);
        }
    }
}
