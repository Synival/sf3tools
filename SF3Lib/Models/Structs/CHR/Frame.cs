using CommonLib.Attributes;
using CommonLib.Extensions;
using CommonLib.Utils;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.CHR {
    public class Frame : Struct {
        private readonly int _textureOffsetAddr;
        private int _compressedTextureSizeAddr;

        public Frame(IByteData data, int id, string name, int address, uint dataOffset, int width, int height, int spriteId, SpriteFrameDirection direction) : base(data, id, name, address, 0x04) {
            DataOffset = dataOffset;
            Width      = width;
            Height     = height;
            SpriteID   = spriteId;
            Direction  = direction;

            _textureOffsetAddr = Address + 0x00; // 4 bytes
            _compressedTextureSizeAddr = (TextureOffset != 0) ? (int) (TextureOffset + DataOffset) : 0;
            Texture =_compressedTextureSizeAddr != 0 ?  new TextureABGR1555(0, 0, 0, GetUncompressedTextureData()) : (ITexture) null;
        }

        public uint DataOffset { get; }

        [TableViewModelColumn(addressField: null, displayOrder: -0.3f, displayFormat: "X2", minWidth: 200)]
        [NameGetter(NamedValueType.Sprite)]
        public int SpriteID { get; }

        [TableViewModelColumn(addressField: null, displayOrder: -0.25f)]
        public SpriteFrameDirection Direction { get; }

        [TableViewModelColumn(displayOrder: -0.2f)]
        public int Width { get; }

        [TableViewModelColumn(displayOrder: -0.1f)]
        public int Height { get; }

        [TableViewModelColumn(displayOrder: 0, addressField: nameof(_textureOffsetAddr), displayFormat: "X4")]
        [BulkCopy]
        public uint TextureOffset {
            get => (uint) Data.GetDouble(_textureOffsetAddr);
            set {
                Data.SetDouble(_textureOffsetAddr, (int) value);
                _compressedTextureSizeAddr = (int) (TextureOffset + DataOffset);
            }
        }

        [TableViewModelColumn(displayOrder: 1, addressField: nameof(_compressedTextureSizeAddr), displayFormat: "X2")]
        [BulkCopy]
        public uint CompressedTextureSize {
            get => (uint) Data.GetDouble(_compressedTextureSizeAddr);
            set => Data.SetDouble(_compressedTextureSizeAddr, (int) value);
        }

        public ITexture Texture { get; private set; }

        private ushort[,] GetUncompressedTextureData() {
            try {
                var decompressedData = Compression.DecompressSpriteData(Data.Data.GetDataCopyOrReference(), (uint) _compressedTextureSizeAddr);
                if (decompressedData.Length != Width * Height)
                    return decompressedData.To2DArrayColumnMajor(1, decompressedData.Length);
                else
                    return decompressedData.To2DArrayColumnMajor(Width, Height);
            }
            catch {
                return new ushort[0, 0];
            }
        }
    }
}