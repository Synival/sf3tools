using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.ByteData;

namespace SF3.Models.Structs.X1.Battle {
    public class BattleHeader : Struct {
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

        public BattleHeader(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x0A) {
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
            get => Data.GetByte(unknown1);
            set => Data.SetByte(unknown1, (byte) value);
        }

        [BulkCopy]
        public int TableSize {
            get => Data.GetByte(tableSize);
            set => Data.SetByte(tableSize, (byte) value);
        }

        [BulkCopy]
        public int SizeUnknown2 {
            get => Data.GetByte(unknown2);
            set => Data.SetByte(unknown2, (byte) value);
        }

        [BulkCopy]
        public int SizeUnknown3 {
            get => Data.GetByte(unknown3);
            set => Data.SetByte(unknown3, (byte) value);
        }

        [BulkCopy]
        public int SizeUnknown4 {
            get => Data.GetByte(unknown4);
            set => Data.SetByte(unknown4, (byte) value);
        }

        [BulkCopy]
        public int SizeUnknown5 {
            get => Data.GetByte(unknown5);
            set => Data.SetByte(unknown5, (byte) value);
        }

        [BulkCopy]
        public int SizeUnknown6 {
            get => Data.GetByte(unknown6);
            set => Data.SetByte(unknown6, (byte) value);
        }

        [BulkCopy]
        public int SizeUnknown7 {
            get => Data.GetByte(unknown7);
            set => Data.SetByte(unknown7, (byte) value);
        }

        [BulkCopy]
        public int SizeUnknown8 {
            get => Data.GetByte(unknown8);
            set => Data.SetByte(unknown8, (byte) value);
        }

        [BulkCopy]
        public int SizeUnknown9 {
            get => Data.GetByte(unknown9);
            set => Data.SetByte(unknown9, (byte) value);
        }
    }
}
