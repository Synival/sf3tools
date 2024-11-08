using CommonLib.Attributes;
using SF3.FileEditors;

namespace SF3.Models {
    public class StatusEffect : Model {
        private readonly int luck0;
        private readonly int unknown0;
        private readonly int luck1;
        private readonly int unknown1;
        private readonly int luck2;
        private readonly int unknown2;
        private readonly int luck3;
        private readonly int unknown3;
        private readonly int luck4;
        private readonly int unknown4;
        private readonly int luck5;
        private readonly int unknown5;
        private readonly int luck6;
        private readonly int unknown6;
        private readonly int luck7;
        private readonly int unknown7;
        private readonly int luck8;
        private readonly int unknown8;
        private readonly int luck9;
        private readonly int unknown9;
        public StatusEffect(IByteEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x18) {
            luck0    = Address;
            unknown0 = Address + 0x01;
            luck1    = Address + 0x02;
            unknown1 = Address + 0x03;
            luck2    = Address + 0x04;
            unknown2 = Address + 0x05;
            luck3    = Address + 0x06;
            unknown3 = Address + 0x07;
            luck4    = Address + 0x08;
            unknown4 = Address + 0x09;
            luck5    = Address + 0x0A;
            unknown5 = Address + 0x0B;
            luck6    = Address + 0x0C;
            unknown6 = Address + 0x0D;
            luck7    = Address + 0x0E;
            unknown7 = Address + 0x0F;
            luck8    = Address + 0x10;
            unknown8 = Address + 0x11;
            luck9    = Address + 0x12;
            unknown9 = Address + 0x13;
        }

        [BulkCopy]
        public int StatusLuck0 {
            get => Editor.GetByte(luck0);
            set => Editor.SetByte(luck0, (byte) value);
        }

        [BulkCopy]
        public int StatusUnknown0 {
            get => Editor.GetByte(unknown0);
            set => Editor.SetByte(unknown0, (byte) value);
        }

        [BulkCopy]
        public int StatusLuck1 {
            get => Editor.GetByte(luck1);
            set => Editor.SetByte(luck1, (byte) value);
        }

        [BulkCopy]
        public int StatusUnknown1 {
            get => Editor.GetByte(unknown1);
            set => Editor.SetByte(unknown1, (byte) value);
        }

        [BulkCopy]
        public int StatusLuck2 {
            get => Editor.GetByte(luck2);
            set => Editor.SetByte(luck2, (byte) value);
        }

        [BulkCopy]
        public int StatusUnknown2 {
            get => Editor.GetByte(unknown2);
            set => Editor.SetByte(unknown2, (byte) value);
        }

        [BulkCopy]
        public int StatusLuck3 {
            get => Editor.GetByte(luck3);
            set => Editor.SetByte(luck3, (byte) value);
        }

        [BulkCopy]
        public int StatusUnknown3 {
            get => Editor.GetByte(unknown3);
            set => Editor.SetByte(unknown3, (byte) value);
        }

        [BulkCopy]
        public int StatusLuck4 {
            get => Editor.GetByte(luck4);
            set => Editor.SetByte(luck4, (byte) value);
        }

        [BulkCopy]
        public int StatusUnknown4 {
            get => Editor.GetByte(unknown4);
            set => Editor.SetByte(unknown4, (byte) value);
        }

        [BulkCopy]
        public int StatusLuck5 {
            get => Editor.GetByte(luck5);
            set => Editor.SetByte(luck5, (byte) value);
        }

        [BulkCopy]
        public int StatusUnknown5 {
            get => Editor.GetByte(unknown5);
            set => Editor.SetByte(unknown5, (byte) value);
        }

        [BulkCopy]
        public int StatusLuck6 {
            get => Editor.GetByte(luck6);
            set => Editor.SetByte(luck6, (byte) value);
        }

        [BulkCopy]
        public int StatusUnknown6 {
            get => Editor.GetByte(unknown6);
            set => Editor.SetByte(unknown6, (byte) value);
        }

        [BulkCopy]
        public int StatusLuck7 {
            get => Editor.GetByte(luck7);
            set => Editor.SetByte(luck7, (byte) value);
        }

        [BulkCopy]
        public int StatusUnknown7 {
            get => Editor.GetByte(unknown7);
            set => Editor.SetByte(unknown7, (byte) value);
        }

        [BulkCopy]
        public int StatusLuck8 {
            get => Editor.GetByte(luck8);
            set => Editor.SetByte(luck8, (byte) value);
        }

        [BulkCopy]
        public int StatusUnknown8 {
            get => Editor.GetByte(unknown8);
            set => Editor.SetByte(unknown8, (byte) value);
        }

        [BulkCopy]
        public int StatusLuck9 {
            get => Editor.GetByte(luck9);
            set => Editor.SetByte(luck9, (byte) value);
        }

        [BulkCopy]
        public int StatusUnknown9 {
            get => Editor.GetByte(unknown9);
            set => Editor.SetByte(unknown9, (byte) value);
        }
    }
}
