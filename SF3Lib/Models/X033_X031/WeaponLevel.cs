using CommonLib.Attributes;
using SF3.FileEditors;

namespace SF3.Models.X033_X031 {
    public class WeaponLevel : Model {
        private readonly int level1;
        private readonly int level2;
        private readonly int level3;
        private readonly int level4;

        public WeaponLevel(IByteEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x11) {
            level1 = Address + 0x02; // 4 bytes
            level2 = Address + 0x06; // 4 bytes
            level3 = Address + 0x0a; // 4 bytes
            level4 = Address + 0x0e; // 4 bytes
        }

        [BulkCopy]
        public int WLevel1 {
            get => Editor.GetWord(level1);
            set => Editor.SetWord(level1, value);
        }

        [BulkCopy]
        public int WLevel2 {
            get => Editor.GetWord(level2);
            set => Editor.SetWord(level2, value);
        }

        [BulkCopy]
        public int WLevel3 {
            get => Editor.GetWord(level3);
            set => Editor.SetWord(level3, value);
        }

        [BulkCopy]
        public int WLevel4 {
            get => Editor.GetWord(level4);
            set => Editor.SetWord(level4, value);
        }
    }
}
