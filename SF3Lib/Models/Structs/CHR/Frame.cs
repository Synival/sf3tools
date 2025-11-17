using System;
using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.Extensions;
using CommonLib.Utils;
using SF3.ByteData;
using SF3.Sprites;
using SF3.Types;

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

        public Frame(IByteData data, int id, string name, int address, uint dataOffset, int width, int height, int spriteIndex, int spriteId) : base(data, id, name, address, 0x04) {
            DataOffset = dataOffset;
            Width      = width;
            Height     = height;
            SpriteIndex = spriteIndex;
            SpriteID   = spriteId;

            _textureOffsetAddr = Address + 0x00; // 4 bytes

            if (TextureOffset != 0) {
                var texData = GetUncompressedTextureData();
                Texture = new TextureABGR1555(0, 0, 0, texData);
                var hash = Texture.Hash.ToLower();

                // There's one specific Tybalt sprite with some problems.
                if (ErrorWhileDecompressing != null && s_brokenTybaltHashes.Contains(hash)) {
                    Texture = new TextureABGR1555(0, 0, 0, texData.To1DArrayTransposed().To2DArrayColumnMajor(48, 40));
                    Width  = Texture.Width;
                    Height = Texture.Height;
                }

                FrameRefs = SpriteResources.GetFrameRefsByImageHash(hash);
            }
        }

        public uint DataOffset { get; }

        [TableViewModelColumn(addressField: null, displayOrder: -0.4f, displayFormat: "X2")]
        public int SpriteIndex { get; }

        [TableViewModelColumn(addressField: null, displayOrder: -0.3f, displayFormat: "X2", minWidth: 200)]
        [NameGetter(NamedValueType.Sprite)]
        public int SpriteID { get; }

        [TableViewModelColumn(displayOrder: -0.25f, minWidth: 175)]
        public string SpriteName
            => FrameRefs?.GetAggergateSpriteName();

        [TableViewModelColumn(displayOrder: -0.2f)]
        public int Width { get; private set; }

        [TableViewModelColumn(displayOrder: -0.1f)]
        public int Height { get; private set; }

        [TableViewModelColumn(displayOrder: 0, minWidth: 200)]
        public string FrameName
            => FrameRefs?.GetAggregateFrameGroupName();

        [TableViewModelColumn(displayOrder: 0.1f)]
        public SpriteFrameDirection Direction
            => FrameRefs?.GetUniqueFrameDirection() ?? SpriteFrameDirection.Unset;

        [TableViewModelColumn(displayOrder: 0.2f, addressField: nameof(_textureOffsetAddr), displayFormat: "X4")]
        [BulkCopy]
        public uint TextureOffset {
            get => (uint) Data.GetDouble(_textureOffsetAddr);
            set => Data.SetDouble(_textureOffsetAddr, (int) value);
        }

        [TableViewModelColumn(displayOrder: 0.5f, displayFormat: "X4")]
        public uint TextureBitstreamOffset => TextureOffset + (uint) Data.GetDouble((int) (TextureOffset + DataOffset));

        [TableViewModelColumn(displayOrder: 0.6f, displayFormat: "X4")]
        public uint TextureEndOffset => TextureOffset + TextureCompressedSize;

        [TableViewModelColumn(displayOrder: 0.7f, displayFormat: "X2")]
        public uint TextureCompressedSize { get; private set; } = 0;

        [TableViewModelColumn(displayOrder: 0.8f, displayFormat: "X2")]
        public uint TextureDecompressedSize { get; private set; } = 0;

        public ITexture Texture { get; private set; }

        private ushort[,] GetUncompressedTextureData() {
            try {
                var decompressedData = Compression.DecompressSpriteData(Data.GetDataCopyOrReference(), TextureOffset + DataOffset, out var size);
                TextureCompressedSize = size;
                TextureDecompressedSize = (uint) decompressedData.Length * 2;

                // If the correct size couldn't be determined, make some educated guesses.
                // (There are errors in the CHR files from time to time)
                if (decompressedData.Length != Width * Height) {
                    ErrorWhileDecompressing = $"decompressedData.Length ({decompressedData.Length}) != {Width * Height} ({Width}x{Height})";

                    // Perform width/height corrections to this frame.
                    if (decompressedData.Length % Width == 0)
                        Height = decompressedData.Length / Width;
                    else if (decompressedData.Length % Height == 0)
                        Width = decompressedData.Length / Height;
                    else {
                        var lengthRoot2 = Math.Sqrt(decompressedData.Length);
                        if (lengthRoot2 == (int) lengthRoot2) {
                            Width = (int) lengthRoot2;
                            Height = (int) lengthRoot2;
                        }
                        else {
                            Width = 1;
                            Height = decompressedData.Length;
                        }
                    }
                }
                // No errors -- decompress normally.
                else
                    ErrorWhileDecompressing = null;

                return decompressedData.To2DArrayColumnMajor(Width, Height);
            }
            catch {
                return new ushort[0, 0];
            }
        }

        public FrameRefSet FrameRefs { get; }
 
        [TableViewModelColumn(displayOrder: 1, minWidth: 200)]
        public string TextureHash => Texture?.Hash;

        [TableViewModelColumn(displayOrder: 5, minWidth: 200)]
        public string ErrorWhileDecompressing { get; private set; } = null;
    }
}