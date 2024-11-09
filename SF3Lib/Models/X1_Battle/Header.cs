using CommonLib.Attributes;
using SF3.FileEditors;

namespace SF3.Models.X1_Battle {
    public class Header : Model {
        private readonly int unknown1;
        private readonly int tableSize;
        private readonly int unknown2;
        private readonly int unknown3;
        private readonly int unknown4;
        private readonly int unknown5;
        private readonly int unknown6;
        private readonly int unknown7;
        private readonly int unknown8;
        private readonly int unknown9;

        public Header(IByteEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x0A) {
            unknown1  = Address;     // 1 byte
            tableSize = Address + 1; // 1 byte
            unknown2  = Address + 2; // 1 byte
            unknown3  = Address + 3; // 1 byte
            unknown4  = Address + 4; // 1 byte
            unknown5  = Address + 5;
            unknown6  = Address + 6;
            unknown7  = Address + 7;
            unknown8  = Address + 8;
            unknown9  = Address + 9;
        }

        [BulkCopy]
        public int SizeUnknown1 {
            get => Editor.GetByte(unknown1);
            set => Editor.SetByte(unknown1, (byte) value);
        }

        [BulkCopy]
        public int TableSize {
            get => Editor.GetByte(tableSize);
            set => Editor.SetByte(tableSize, (byte) value);
        }

        [BulkCopy]
        public int SizeUnknown2 {
            get => Editor.GetByte(unknown2);
            set => Editor.SetByte(unknown2, (byte) value);
        }

        [BulkCopy]
        public int SizeUnknown3 {
            get => Editor.GetByte(unknown3);
            set => Editor.SetByte(unknown3, (byte) value);
        }

        [BulkCopy]
        public int SizeUnknown4 {
            get => Editor.GetByte(unknown4);
            set => Editor.SetByte(unknown4, (byte) value);
        }

        [BulkCopy]
        public int SizeUnknown5 {
            get => Editor.GetByte(unknown5);
            set => Editor.SetByte(unknown5, (byte) value);
        }

        [BulkCopy]
        public int SizeUnknown6 {
            get => Editor.GetByte(unknown6);
            set => Editor.SetByte(unknown6, (byte) value);
        }

        [BulkCopy]
        public int SizeUnknown7 {
            get => Editor.GetByte(unknown7);
            set => Editor.SetByte(unknown7, (byte) value);
        }

        [BulkCopy]
        public int SizeUnknown8 {
            get => Editor.GetByte(unknown8);
            set => Editor.SetByte(unknown8, (byte) value);
        }

        [BulkCopy]
        public int SizeUnknown9 {
            get => Editor.GetByte(unknown9);
            set => Editor.SetByte(unknown9, (byte) value);
        }
    }
}
