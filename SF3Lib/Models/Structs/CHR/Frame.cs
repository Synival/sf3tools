using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.Extensions;
using CommonLib.Utils;
using SF3.ByteData;
using SF3.Types;
using SF3.Utils;

namespace SF3.Models.Structs.CHR {
    public class Frame : Struct {
        // There are four specific images in Scenario 2 that aren't read correctly for some reason.
        // They're listed at 40x40, but they're actually 48x40.
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

            if (TextureOffset != 0) {
                var texData = GetUncompressedTextureData();
                Texture = new TextureABGR1555(0, 0, 0, texData);
                var hash = Texture.Hash.ToLower();

                // There's one specific Tybalt sprite with some problems.
                if (ErrorWhileDecompressing != null && s_brokenTybaltHashes.Contains(hash))
                    Texture = new TextureABGR1555(0, 0, 0, texData.To1DArrayTransposed().To2DArrayColumnMajor(48, 40));

                var frameTextureInfo = SpriteFrameTextueUtils.GetFrameTextureInfoByHash(hash);
                TextureHash   = hash;
                SpriteName    = frameTextureInfo.SpriteName;
                AnimationName = frameTextureInfo.AnimationName;
            }
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

        private ushort[,] GetUncompressedTextureData() {
            try {
                var decompressedData = Compression.DecompressSpriteData(Data.Data.GetDataCopyOrReference(), TextureOffset + DataOffset);
                if (decompressedData.Length != Width * Height) {
                    ErrorWhileDecompressing = $"decompressedData.Length ({decompressedData.Length}) != {Width * Height} ({Width}x{Height})";

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

        [TableViewModelColumn(displayOrder: 1, minWidth: 200)]
        public string TextureHash { get; }

        [TableViewModelColumn(displayOrder: 2, minWidth: 150)]
        public string SpriteName { get; }

        [TableViewModelColumn(displayOrder: 3)]
        public string AnimationName { get; }

        [TableViewModelColumn(displayOrder: 4, minWidth: 200)]
        public string ErrorWhileDecompressing { get; private set; } = null;
    }
}