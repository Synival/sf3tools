using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.CHR {
    public class Sprite : Struct {
        private readonly int _spriteIdAddr;
        private readonly int _widthAddr;
        private readonly int _heightAddr;
        private readonly int _directionsAddr;
        private readonly int _verticalOffsetAddr;
        private readonly int _unknown0x08Addr;
        private readonly int _collisionShadowDiameterAddr;
        private readonly int _promotionLevelAddr;
        private readonly int _paddingAddr;
        private readonly int _scaleAddr;
        private readonly int _frameTableOffsetAddr;
        private readonly int _animationTableOffsetAddr;

        public Sprite(IByteData data, int id, string name, int address, uint dataOffset) : base(data, id, name, address, 0x18) {
            DataOffset = dataOffset;

            _spriteIdAddr                = Address + 0x00; // 2 bytes
            _widthAddr                   = Address + 0x02; // 2 bytes
            _heightAddr                  = Address + 0x04; // 2 bytes
            _directionsAddr              = Address + 0x06; // 1 byte
            _verticalOffsetAddr          = Address + 0x07; // 1 byte
            _unknown0x08Addr             = Address + 0x08; // 1 byte
            _collisionShadowDiameterAddr = Address + 0x09; // 1 byte
            _promotionLevelAddr          = Address + 0x0A; // 1 byte
            _paddingAddr                 = Address + 0x0B; // 1 byte
            _scaleAddr                   = Address + 0x0C; // 4 bytes
            _frameTableOffsetAddr        = Address + 0x10; // 4 bytes
            _animationTableOffsetAddr    = Address + 0x14; // 4 bytes
        }

        public uint DataOffset { get; }

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

        [TableViewModelColumn(addressField: nameof(_directionsAddr), displayOrder: 3, displayFormat: "X2")]
        [BulkCopy]
        public byte Directions {
            get => (byte) Data.GetByte(_directionsAddr);
            set => Data.SetByte(_directionsAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_verticalOffsetAddr), displayOrder: 4, displayFormat: "X2")]
        [BulkCopy]
        public byte VerticalOffset {
            get => (byte) Data.GetByte(_verticalOffsetAddr);
            set => Data.SetByte(_verticalOffsetAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x08Addr), displayOrder: 5, displayName: "+0x08")]
        [BulkCopy]
        public byte Unknown0x08 {
            get => (byte) Data.GetByte(_unknown0x08Addr);
            set => Data.SetByte(_unknown0x08Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_collisionShadowDiameterAddr), displayOrder: 6, displayName: "Collision/Shadow Diameter")]
        [BulkCopy]
        public byte CollisionShadowDiameter {
            get => (byte) Data.GetByte(_collisionShadowDiameterAddr);
            set => Data.SetByte(_collisionShadowDiameterAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_promotionLevelAddr), displayOrder: 7, displayFormat: "X2")]
        [BulkCopy]
        public byte PromotionLevel {
            get => (byte) Data.GetByte(_promotionLevelAddr);
            set => Data.SetByte(_promotionLevelAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_paddingAddr), displayOrder: 8, displayFormat: "X2", displayName: "(Padding?)")]
        [BulkCopy]
        public byte Padding {
            get => (byte) Data.GetByte(_paddingAddr);
            set => Data.SetByte(_paddingAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_scaleAddr), displayOrder: 9, displayFormat: "X2")]
        [BulkCopy]
        public uint Scale {
            get => (uint) Data.GetDouble(_scaleAddr);
            set => Data.SetDouble(_scaleAddr, (int) value);
        }

        [TableViewModelColumn(addressField: nameof(_frameTableOffsetAddr), displayOrder: 10, displayFormat: "X2")]
        [BulkCopy]
        public uint FrameTableOffset {
            get => (uint) Data.GetDouble(_frameTableOffsetAddr);
            set => Data.SetDouble(_frameTableOffsetAddr, (int) value);
        }

        [TableViewModelColumn(addressField: nameof(_animationTableOffsetAddr), displayOrder: 11, displayFormat: "X2")]
        [BulkCopy]
        public uint AnimationTableOffset {
            get => (uint) Data.GetDouble(_animationTableOffsetAddr);
            set => Data.SetDouble(_animationTableOffsetAddr, (int) value);
        }
    }
}
