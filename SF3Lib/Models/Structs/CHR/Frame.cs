using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.Extensions;
using CommonLib.Utils;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.CHR {
    public class Frame : Struct {
        private static readonly HashSet<string> s_brokenTybaltHashes = new HashSet<string>() {
            "19711da96a96d8a92d866ba15b252d1f",
            "b1c69203445eb35716c46f31085ecc32",
            "df4a1a48135b5a38302b697b3f6f61af",
            "ff47e55687d5524c138c75f5f4e61c5c"
        };

        private readonly int _textureOffsetAddr;

        public Frame(IByteData data, int id, string name, int address, uint dataOffset, int width, int height, int spriteId, SpriteFrameDirection direction) : base(data, id, name, address, 0x04) {
            DataOffset = dataOffset;
            Width      = width;
            Height     = height;
            SpriteID   = spriteId;
            Direction  = direction;

            _textureOffsetAddr = Address + 0x00; // 4 bytes
            var texData = TextureOffset != 0 ? GetUncompressedTextureData() : null;
            Texture = TextureOffset != 0 ?  new TextureABGR1555(0, 0, 0, texData) : (ITexture) null;

            // There's one specific Tybalt sprite with some problems.
            if (texData != null && ErrorWhileDecompressing != null && s_brokenTybaltHashes.Contains(Texture.Hash))
                Texture = new TextureABGR1555(0, 0, 0, texData.To1DArrayTransposed().To2DArrayColumnMajor(48, 40));
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
            set => Data.SetDouble(_textureOffsetAddr, (int) value);
        }

        public ITexture Texture { get; private set; }

        [TableViewModelColumn(displayOrder: 1)]
        public string ErrorWhileDecompressing { get; private set; } = null;

        private ushort[,] GetUncompressedTextureData() {
            try {
                var decompressedData = Compression.DecompressSpriteData(Data.Data.GetDataCopyOrReference(), TextureOffset + DataOffset);
                if (decompressedData.Length != Width * Height) {
                    ErrorWhileDecompressing = $"Length {decompressedData.Length} != {Width * Height} ({Width}x{Height})";

                    if (decompressedData.Length % Width == 0)
                        return decompressedData.To2DArrayColumnMajor(Width, decompressedData.Length / Width);
                    else
                        return decompressedData.To2DArrayColumnMajor(1, decompressedData.Length);
                }
                else {
                    ErrorWhileDecompressing = null;
                    return decompressedData.To2DArrayColumnMajor(Width, Height);
                }
            }
            catch {
                return new ushort[0, 0];
            }
        }
    }
}