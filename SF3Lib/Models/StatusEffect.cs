using CommonLib.Attributes;
using SF3.FileEditors;

namespace SF3.Models {
    public class StatusEffect : Model {
        private readonly int luck0;
        private readonly int luck1;
        private readonly int luck2;
        private readonly int luck3;
        private readonly int luck4;
        private readonly int luck5;
        private readonly int luck6;
        private readonly int luck7;
        private readonly int luck8;
        private readonly int luck9;

        public StatusEffect(IByteEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x18) {
            luck0 = Address;
            luck1 = Address + 0x02;
            luck2 = Address + 0x04;
            luck3 = Address + 0x06;
            luck4 = Address + 0x08;
            luck5 = Address + 0x0A;
            luck6 = Address + 0x0C;
            luck7 = Address + 0x0E;
            luck8 = Address + 0x10;
            luck9 = Address + 0x12;
        }

        [BulkCopy]
        public int StatusLuck0 {
            get => Editor.GetByte(luck0);
            set => Editor.SetByte(luck0, (byte) value);
        }

        [BulkCopy]
        public int StatusLuck1 {
            get => Editor.GetByte(luck1);
            set => Editor.SetByte(luck1, (byte) value);
        }

        [BulkCopy]
        public int StatusLuck2 {
            get => Editor.GetByte(luck2);
            set => Editor.SetByte(luck2, (byte) value);
        }

        [BulkCopy]
        public int StatusLuck3 {
            get => Editor.GetByte(luck3);
            set => Editor.SetByte(luck3, (byte) value);
        }

        [BulkCopy]
        public int StatusLuck4 {
            get => Editor.GetByte(luck4);
            set => Editor.SetByte(luck4, (byte) value);
        }

        [BulkCopy]
        public int StatusLuck5 {
            get => Editor.GetByte(luck5);
            set => Editor.SetByte(luck5, (byte) value);
        }

        [BulkCopy]
        public int StatusLuck6 {
            get => Editor.GetByte(luck6);
            set => Editor.SetByte(luck6, (byte) value);
        }

        [BulkCopy]
        public int StatusLuck7 {
            get => Editor.GetByte(luck7);
            set => Editor.SetByte(luck7, (byte) value);
        }

        [BulkCopy]
        public int StatusLuck8 {
            get => Editor.GetByte(luck8);
            set => Editor.SetByte(luck8, (byte) value);
        }

        [BulkCopy]
        public int StatusLuck9 {
            get => Editor.GetByte(luck9);
            set => Editor.SetByte(luck9, (byte) value);
        }
    }
}
