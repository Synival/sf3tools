using System.Linq;
using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.CHR {
    public class SpriteHeader : Struct {
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

        public SpriteHeader(IByteData data, int id, string name, int address, uint dataOffset, bool isInCHP)
        : base(data, id, name, address, 0x18) {
            DataOffset = dataOffset;
            IsInCHP    = isInCHP;

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

        public bool IsValid() {
            return (
                SpriteID == 0xFFFF &&
                Enumerable.SequenceEqual(new byte[Size - 4], Data.GetDataCopyAt(Address + 4, Size - 4))
            )
            || (
                SpriteID != 0xFFFF &&
                SpriteID  < 0x0800 &&
                Width  < 0x0200 &&
                Height < 0x0200 &&
                Width  >      0 &&
                Height >      0 &&
                FrameTableOffset < 0xC0000 &&
                AnimationTableOffset < 0xC0000 &&
                Scale > 0x00500 &&
                Scale < 0x30000 &&
                Directions > 0
            );
        }

        [TableViewModelColumn(displayOrder: 0, displayFormat: "X2")]
        public uint DataOffset { get; }
        public bool IsInCHP { get; }

        [TableViewModelColumn(addressField: nameof(_spriteIdAddr), displayOrder: 0.1f, displayFormat: "X2", minWidth: 200)]
        [NameGetter(NamedValueType.Sprite)]
        [BulkCopy]
        public ushort SpriteID {
            get => Data.GetUInt16(_spriteIdAddr);
            set => Data.SetUInt16(_spriteIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_widthAddr), displayOrder: 1)]
        [BulkCopy]
        public ushort Width {
            get => Data.GetUInt16(_widthAddr);
            set => Data.SetUInt16(_widthAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_heightAddr), displayOrder: 2)]
        [BulkCopy]
        public ushort Height {
            get => Data.GetUInt16(_heightAddr);
            set => Data.SetUInt16(_heightAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_directionsAddr), displayOrder: 3)]
        [NameGetter(NamedValueType.SpriteDirectionCount)]
        [BulkCopy]
        public byte Directions {
            get => Data.GetUInt8(_directionsAddr);
            set => Data.SetUInt8(_directionsAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_verticalOffsetAddr), displayOrder: 4, displayFormat: "X2")]
        [BulkCopy]
        public byte VerticalOffset {
            get => Data.GetUInt8(_verticalOffsetAddr);
            set => Data.SetUInt8(_verticalOffsetAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x08Addr), displayOrder: 5, displayName: "+0x08")]
        [BulkCopy]
        public byte Unknown0x08 {
            get => Data.GetUInt8(_unknown0x08Addr);
            set => Data.SetUInt8(_unknown0x08Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_collisionShadowDiameterAddr), displayOrder: 6, displayName: "Collision/Shadow Diameter")]
        [BulkCopy]
        public byte CollisionShadowDiameter {
            get => Data.GetUInt8(_collisionShadowDiameterAddr);
            set => Data.SetUInt8(_collisionShadowDiameterAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_promotionLevelAddr), displayOrder: 7, displayFormat: "X2")]
        [BulkCopy]
        public byte PromotionLevel {
            get => Data.GetUInt8(_promotionLevelAddr);
            set => Data.SetUInt8(_promotionLevelAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_paddingAddr), displayOrder: 8, displayFormat: "X2", displayName: "(Padding?)")]
        [BulkCopy]
        public byte Padding {
            get => Data.GetUInt8(_paddingAddr);
            set => Data.SetUInt8(_paddingAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_scaleAddr), displayOrder: 9, displayFormat: "X2")]
        [BulkCopy]
        public uint Scale {
            get => Data.GetUInt32(_scaleAddr);
            set => Data.SetUInt32(_scaleAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_frameTableOffsetAddr), displayOrder: 10, displayFormat: "X2")]
        [BulkCopy]
        public uint FrameTableOffset {
            get => Data.GetUInt32(_frameTableOffsetAddr);
            set => Data.SetUInt32(_frameTableOffsetAddr, value);
        }

        [TableViewModelColumn(displayOrder: 10.1f, displayName: "FrameTableOff (In File)", displayFormat: "X2")]
        public uint FrameTableOffsetInFile {
            get => FrameTableOffset + DataOffset;
            set => FrameTableOffset = value - DataOffset;
        }

        [TableViewModelColumn(addressField: nameof(_animationTableOffsetAddr), displayOrder: 11, displayFormat: "X2")]
        [BulkCopy]
        public uint AnimationTableOffset {
            get => Data.GetUInt32(_animationTableOffsetAddr);
            set => Data.SetUInt32(_animationTableOffsetAddr, value);
        }

        [TableViewModelColumn(displayOrder: 11.1f, displayName: "AniTableOff (In File)", displayFormat: "X2", visibilityProperty: nameof(IsInCHP))]
        public uint AnimationTableOffsetInFile {
            get => AnimationTableOffset + DataOffset;
            set => AnimationTableOffset = value - DataOffset;
        }

        // TODO: Not actually part of the header! Only here for views.
        [TableViewModelColumn(displayOrder: 12, displayFormat: "X4", displayName: "TotalCompressedFramesSize (Derived)", isReadOnly: true)]
        public uint TotalCompressedFramesSize { get; set; }
    }
}
