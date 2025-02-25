using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X1.Town {
    public class Npc : Struct {
        private readonly int _spriteIDAddr;
        private readonly int _unknown0x02Addr;
        private readonly int _movementTableAddr;
        private readonly int _xPosAddr;
        private readonly int _zPosAddr;
        private readonly int _directionAddr;
        private readonly int _unknown0x0AAddr;
        private readonly int _unknown0x0CAddr;
        private readonly int _unknown0x0EAddr;
        private readonly int _unknown0x12Addr;
        private readonly int _unknown0x16Addr;

        public Npc(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x18) {
            _spriteIDAddr      = Address + 0x00; // 2 bytes. how is searched. second by being 0x13 is a treasure. if this is 0xffff terminate 
            _unknown0x02Addr   = Address + 0x02; // unknown + 0x02
            _movementTableAddr = Address + 0x04; // 2 bytes
            _xPosAddr          = Address + 0x08; // 2 bytes
            _unknown0x0AAddr   = Address + 0x0a; // 2 bytes
            _unknown0x0CAddr   = Address + 0x0c; // 2 bytes
            _unknown0x0EAddr   = Address + 0x0e; // 2 bytes
            _zPosAddr          = Address + 0x10; // 2 bytes
            _unknown0x12Addr   = Address + 0x12; // 2 bytes
            _directionAddr     = Address + 0x14; // 2 bytes
            _unknown0x16Addr   = Address + 0x16; // 2 bytes
        }

        [TableViewModelColumn(displayOrder: 0, displayFormat: "X3")]
        [BulkCopy]
        public int SpriteID {
            get => Data.GetWord(_spriteIDAddr);
            set => Data.SetWord(_spriteIDAddr, value);
        }

        [TableViewModelColumn(displayOrder: 1, displayName: "+0x02", displayFormat: "X4")]
        [BulkCopy]
        public int Unknown0x02 {
            get => Data.GetWord(_unknown0x02Addr);
            set => Data.SetWord(_unknown0x02Addr, value);
        }

        [TableViewModelColumn(displayOrder: 2, displayName: "MovementTable?", isPointer: true)]
        [BulkCopy]
        public int MovementTable {
            get => Data.GetDouble(_movementTableAddr);
            set => Data.SetDouble(_movementTableAddr, value);
        }

        [TableViewModelColumn(displayOrder: 3, displayName: "xPos")]
        [BulkCopy]
        public int XPos {
            get => Data.GetWord(_xPosAddr);
            set => Data.SetWord(_xPosAddr, value);
        }

        [TableViewModelColumn(displayOrder: 4, displayName: "+0x0A", displayFormat: "X2")]
        [BulkCopy]
        public int Unknown0x0A {
            get => Data.GetWord(_unknown0x0AAddr);
            set => Data.SetWord(_unknown0x0AAddr, value);
        }

        [TableViewModelColumn(displayOrder: 5, displayName: "+0x0C", displayFormat: "X2")]
        [BulkCopy]
        public int Unknown0x0C {
            get => Data.GetWord(_unknown0x0CAddr);
            set => Data.SetWord(_unknown0x0CAddr, value);
        }

        [TableViewModelColumn(displayOrder: 6, displayName: "+0x0E", displayFormat: "X2")]
        [BulkCopy]
        public int Unknown0x0E {
            get => Data.GetWord(_unknown0x0EAddr);
            set => Data.SetWord(_unknown0x0EAddr, value);
        }

        [TableViewModelColumn(displayOrder: 7, displayName: "zPos")]
        [BulkCopy]
        public int ZPos {
            get => Data.GetWord(_zPosAddr);
            set => Data.SetWord(_zPosAddr, value);
        }

        [TableViewModelColumn(displayOrder: 8, displayName: "0x12", displayFormat: "X2")]
        [BulkCopy]
        public int Unknown0x12 {
            get => Data.GetWord(_unknown0x12Addr);
            set => Data.SetWord(_unknown0x12Addr, value);
        }

        [TableViewModelColumn(displayOrder: 9, displayName: "Direction", displayFormat: "X4")]
        [BulkCopy]
        public int Direction {
            get => Data.GetWord(_directionAddr);
            set => Data.SetWord(_directionAddr, value);
        }

        [TableViewModelColumn(displayOrder: 10, displayName: "+0x16", displayFormat: "X2")]
        [BulkCopy]
        public int Unknown0x16 {
            get => Data.GetWord(_unknown0x16Addr);
            set => Data.SetWord(_unknown0x16Addr, value);
        }

        [TableViewModelColumn(displayOrder: 11)]
        public string TiedToEventNumber
            => Data.GetWord(_spriteIDAddr) > 0x0f && Data.GetWord(_spriteIDAddr) != 0xffff ? (ID + 0x3D).ToString("X") : "";
    }
}
