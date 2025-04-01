using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X013 {
    public class StatusEffect : Struct {
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
        public StatusEffect(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x18) {
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

        [TableViewModelColumn(displayOrder: 0, displayName: "Luck 0 Chance%")]
        [BulkCopy]
        public int StatusLuck0 {
            get => Data.GetByte(luck0);
            set => Data.SetByte(luck0, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 0.5f, displayName: "Luck 0 Unknown")]
        [BulkCopy]
        public int StatusUnknown0 {
            get => Data.GetByte(unknown0);
            set => Data.SetByte(unknown0, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 1, displayName: "Luck 1 Chance%")]
        [BulkCopy]
        public int StatusLuck1 {
            get => Data.GetByte(luck1);
            set => Data.SetByte(luck1, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 1.5f, displayName: "Luck 1 Unknown")]
        [BulkCopy]
        public int StatusUnknown1 {
            get => Data.GetByte(unknown1);
            set => Data.SetByte(unknown1, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 2, displayName: "Luck 2 Chance%")]
        [BulkCopy]
        public int StatusLuck2 {
            get => Data.GetByte(luck2);
            set => Data.SetByte(luck2, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 2.5f, displayName: "Luck 2 Unknown")]
        [BulkCopy]
        public int StatusUnknown2 {
            get => Data.GetByte(unknown2);
            set => Data.SetByte(unknown2, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 3, displayName: "Luck 3 Chance%")]
        [BulkCopy]
        public int StatusLuck3 {
            get => Data.GetByte(luck3);
            set => Data.SetByte(luck3, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 3.5f, displayName: "Luck 3 Unknown")]
        [BulkCopy]
        public int StatusUnknown3 {
            get => Data.GetByte(unknown3);
            set => Data.SetByte(unknown3, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 4, displayName: "Luck 4 Chance%")]
        [BulkCopy]
        public int StatusLuck4 {
            get => Data.GetByte(luck4);
            set => Data.SetByte(luck4, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 4.5f, displayName: "Luck 4 Unknown")]
        [BulkCopy]
        public int StatusUnknown4 {
            get => Data.GetByte(unknown4);
            set => Data.SetByte(unknown4, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 5, displayName: "Luck 5 Chance%")]
        [BulkCopy]
        public int StatusLuck5 {
            get => Data.GetByte(luck5);
            set => Data.SetByte(luck5, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 5.5f, displayName: "Luck 5 Unknown")]
        [BulkCopy]
        public int StatusUnknown5 {
            get => Data.GetByte(unknown5);
            set => Data.SetByte(unknown5, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 6, displayName: "Luck 6 Chance%")]
        [BulkCopy]
        public int StatusLuck6 {
            get => Data.GetByte(luck6);
            set => Data.SetByte(luck6, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 6.5f, displayName: "Luck 6 Unknown")]
        [BulkCopy]
        public int StatusUnknown6 {
            get => Data.GetByte(unknown6);
            set => Data.SetByte(unknown6, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 7, displayName: "Luck 7 Chance%")]
        [BulkCopy]
        public int StatusLuck7 {
            get => Data.GetByte(luck7);
            set => Data.SetByte(luck7, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 7.5f, displayName: "Luck 7 Unknown")]
        [BulkCopy]
        public int StatusUnknown7 {
            get => Data.GetByte(unknown7);
            set => Data.SetByte(unknown7, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 8, displayName: "Luck 8 Chance%")]
        [BulkCopy]
        public int StatusLuck8 {
            get => Data.GetByte(luck8);
            set => Data.SetByte(luck8, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 8.5f, displayName: "Luck 8 Unknown")]
        [BulkCopy]
        public int StatusUnknown8 {
            get => Data.GetByte(unknown8);
            set => Data.SetByte(unknown8, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 9, displayName: "Luck 9 Chance%")]
        [BulkCopy]
        public int StatusLuck9 {
            get => Data.GetByte(luck9);
            set => Data.SetByte(luck9, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 9.5f, displayName: "Luck 9 Unknown")]
        [BulkCopy]
        public int StatusUnknown9 {
            get => Data.GetByte(unknown9);
            set => Data.SetByte(unknown9, (byte) value);
        }
    }
}
