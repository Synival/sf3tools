using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.CHR {
    public class Sprite : Struct {
        private readonly int _spriteIdAddr;
        private readonly int _widthAddr;
        private readonly int _heightAddr;
        private readonly int _unknown0x06Addr;
        private readonly int _unknown0x07Addr;
        private readonly int _unknown0x08Addr;
        private readonly int _unknown0x09Addr;
        private readonly int _unknown0x0AAddr;
        private readonly int _unknown0x0BAddr;
        private readonly int _scaleAddr;
        private readonly int _unknown0x10Addr;
        private readonly int _unknown0x14Addr;

        public Sprite(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x18) {
             _spriteIdAddr    = Address + 0x00; // 2 bytes
             _widthAddr       = Address + 0x02; // 2 bytes
             _heightAddr      = Address + 0x04; // 2 bytes
             _unknown0x06Addr = Address + 0x06; // 1 byte
             _unknown0x07Addr = Address + 0x07; // 1 byte
             _unknown0x08Addr = Address + 0x08; // 1 byte
             _unknown0x09Addr = Address + 0x09; // 1 byte
             _unknown0x0AAddr = Address + 0x0A; // 1 byte
             _unknown0x0BAddr = Address + 0x0B; // 1 byte
             _scaleAddr       = Address + 0x0C; // 4 bytes
             _unknown0x10Addr = Address + 0x10; // 4 bytes
             _unknown0x14Addr = Address + 0x14; // 4 bytes
        }

        [TableViewModelColumn(addressField: nameof(_spriteIdAddr), displayOrder: 0, displayFormat: "X2", minWidth: 200)]
        [NameGetter(NamedValueType.Sprite)]
        [BulkCopy]
        public int SpriteId {
            get => Data.GetWord(_spriteIdAddr);
            set => Data.SetWord(_spriteIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_widthAddr), displayOrder: 1)]
        [BulkCopy]
        public ushort Width {
            get => (ushort) Data.GetWord(_widthAddr);
            set => Data.SetWord(_widthAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_heightAddr), displayOrder: 2)]
        [BulkCopy]
        public ushort Height {
            get => (ushort) Data.GetWord(_heightAddr);
            set => Data.SetWord(_heightAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x06Addr), displayOrder: 3, displayFormat: "X2", displayName: "+0x06")]
        [BulkCopy]
        public byte Unknown0x06 {
            get => (byte) Data.GetByte(_unknown0x06Addr);
            set => Data.SetByte(_unknown0x06Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x07Addr), displayOrder: 4, displayFormat: "X2", displayName: "+0x07")]
        [BulkCopy]
        public byte Unknown0x07 {
            get => (byte) Data.GetByte(_unknown0x07Addr);
            set => Data.SetByte(_unknown0x07Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x08Addr), displayOrder: 5, displayFormat: "X2", displayName: "+0x08")]
        [BulkCopy]
        public byte Unknown0x08 {
            get => (byte) Data.GetByte(_unknown0x08Addr);
            set => Data.SetByte(_unknown0x08Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x09Addr), displayOrder: 6, displayFormat: "X2", displayName: "+0x09")]
        [BulkCopy]
        public byte Unknown0x09 {
            get => (byte) Data.GetByte(_unknown0x09Addr);
            set => Data.SetByte(_unknown0x09Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x0AAddr), displayOrder: 7, displayFormat: "X2", displayName: "+0x0A")]
        [BulkCopy]
        public byte Unknown0x0A {
            get => (byte) Data.GetByte(_unknown0x0AAddr);
            set => Data.SetByte(_unknown0x0AAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x0BAddr), displayOrder: 8, displayFormat: "X2", displayName: "+0x0B")]
        [BulkCopy]
        public byte Unknown0x0B {
            get => (byte) Data.GetByte(_unknown0x0BAddr);
            set => Data.SetByte(_unknown0x0BAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_scaleAddr), displayOrder: 9, displayFormat: "X2")]
        [BulkCopy]
        public uint Scale {
            get => (uint) Data.GetDouble(_scaleAddr);
            set => Data.SetDouble(_scaleAddr, (int) value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x10Addr), displayOrder: 10, displayFormat: "X2", displayName: "+0x10")]
        [BulkCopy]
        public uint Unknown0x10 {
            get => (uint) Data.GetDouble(_unknown0x10Addr);
            set => Data.SetDouble(_unknown0x10Addr, (int) value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x14Addr), displayOrder: 11, displayFormat: "X2", displayName: "+0x14")]
        [BulkCopy]
        public uint Unknown0x14 {
            get => (uint) Data.GetDouble(_unknown0x14Addr);
            set => Data.SetDouble(_unknown0x14Addr, (int) value);
        }
    }
}
