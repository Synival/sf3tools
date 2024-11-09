using CommonLib.Attributes;
using SF3.FileEditors;

namespace SF3.Models.X002 {
    public class WeaponRank : Model {
        private readonly int skill0;
        private readonly int skill1;
        private readonly int skill2;
        private readonly int skill3;

        public WeaponRank(IByteEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x04) {
            skill0 = Address;     // 1 byte
            skill1 = Address + 1; // 1 byte
            skill2 = Address + 2; // 1 byte
            skill3 = Address + 3; // 1 byte
        }

        [BulkCopy]
        public int Skill0 {
            get => Editor.GetByte(skill0);
            set => Editor.SetByte(skill0, (byte) value);
        }

        [BulkCopy]
        public int Skill1 {
            get => Editor.GetByte(skill1);
            set => Editor.SetByte(skill1, (byte) value);
        }

        [BulkCopy]
        public int Skill2 {
            get => Editor.GetByte(skill2);
            set => Editor.SetByte(skill2, (byte) value);
        }

        [BulkCopy]
        public int Skill3 {
            get => Editor.GetByte(skill3);
            set => Editor.SetByte(skill3, (byte) value);
        }
    }
}
