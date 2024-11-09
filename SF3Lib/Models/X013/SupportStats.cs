using CommonLib.Attributes;
using SF3.FileEditors;

namespace SF3.Models.X013 {
    public class SupportStats : Model {
        private readonly int sLvlStat1;
        private readonly int sLvlStat2;
        private readonly int sLvlStat3;
        private readonly int sLvlStat4;

        public SupportStats(IByteEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x04) {
            sLvlStat1 = Address;     // 1 byte
            sLvlStat2 = Address + 1; // 1 byte
            sLvlStat3 = Address + 2; // 1 byte
            sLvlStat4 = Address + 3; // 1 byte
        }

        [BulkCopy]
        public int SLvlStat1 {
            get => Editor.GetByte(sLvlStat1);
            set => Editor.SetByte(sLvlStat1, (byte) value);
        }

        [BulkCopy]
        public int SLvlStat2 {
            get => Editor.GetByte(sLvlStat2);
            set => Editor.SetByte(sLvlStat2, (byte) value);
        }

        [BulkCopy]
        public int SLvlStat3 {
            get => Editor.GetByte(sLvlStat3);
            set => Editor.SetByte(sLvlStat3, (byte) value);
        }

        [BulkCopy]
        public int SLvlStat4 {
            get => Editor.GetByte(sLvlStat4);
            set => Editor.SetByte(sLvlStat4, (byte) value);
        }
    }
}
