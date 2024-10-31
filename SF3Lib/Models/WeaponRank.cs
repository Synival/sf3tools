using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models {
    public class WeaponRank : IModel {
        private readonly int skill0;
        private readonly int skill1;
        private readonly int skill2;
        private readonly int skill3;

        public WeaponRank(ISF3FileEditor editor, int id, string name, int address) {
            Editor  = editor;
            Name    = name;
            ID      = id;
            Address = address;
            Size    = 0x04;

            skill0 = Address;     // 1 byte
            skill1 = Address + 1; // 1 byte
            skill2 = Address + 2; // 1 byte
            skill3 = Address + 3; // 1 byte
        }

        public IByteEditor Editor { get; }

        [BulkCopyRowName]
        public string Name { get; }
        public int ID { get; }
        public int Address { get; }
        public int Size { get; }

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
