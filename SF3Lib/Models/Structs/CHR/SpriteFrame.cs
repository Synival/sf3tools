using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.CHR {
    public class SpriteFrame : Struct {
        private readonly int _textureOffsetAddr;

        public SpriteFrame(IByteData data, int id, string name, int address, uint dataOffset) : base(data, id, name, address, 0x04) {
            DataOffset = dataOffset;

            _textureOffsetAddr = Address + 0x00; // 4 bytes
            _textureSizeAddr = (int) (TextureOffset + DataOffset);
        }

        public uint DataOffset { get; }

        private int _textureSizeAddr;

        [TableViewModelColumn(displayOrder: 0, addressField: nameof(_textureOffsetAddr), displayFormat: "X4")]
        [BulkCopy]
        public uint TextureOffset {
            get => (uint) Data.GetDouble(_textureOffsetAddr);
            set {
                Data.SetDouble(_textureOffsetAddr, (int) value);
                _textureSizeAddr = (int) (TextureOffset + DataOffset);
            }
        }

        [TableViewModelColumn(displayOrder: 1, addressField: nameof(_textureSizeAddr), displayFormat: "X2")]
        [BulkCopy]
        public uint TextureSize {
            get => (uint) Data.GetDouble(_textureSizeAddr);
            set => Data.SetDouble(_textureSizeAddr, (int) value);
        }
    }
}
