﻿using System;
using System.Collections.Generic;
using System.Linq;
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

        public Frame(IByteData data, int id, string name, int address, uint dataOffset, int width, int height, int spriteIndex, int spriteId, SpriteFrameDirection direction) : base(data, id, name, address, 0x04) {
            DataOffset = dataOffset;
            Width      = width;
            Height     = height;
            SpriteIndex = spriteIndex;
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

                FrameInfo = SpriteFrameTextueUtils.GetFrameTextureInfoByHash(hash, Texture.Width, Texture.Height);

                if (Direction != SpriteFrameDirection.None) {
                    var ds = Direction.ToString();
                    var dc = FrameInfo.DirectionCounts;
                    dc[ds] = dc.ContainsKey(ds) ? dc[ds] + 1 : 1;
                }
            }
        }

        public uint DataOffset { get; }

        [TableViewModelColumn(addressField: null, displayOrder: -0.4f, displayFormat: "X2")]
        public int SpriteIndex { get; }

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

                // If the correct size couldn't be determined, make some educated guesses.
                // (There are errors in the CHR files from time to time)
                if (decompressedData.Length != Width * Height) {
                    ErrorWhileDecompressing = $"decompressedData.Length ({decompressedData.Length}) != {Width * Height} ({Width}x{Height})";

                    if (decompressedData.Length % Width == 0)
                        return decompressedData.To2DArrayColumnMajor(Width, decompressedData.Length / Width);
                    if (decompressedData.Length % Height == 0)
                        return decompressedData.To2DArrayColumnMajor(decompressedData.Length / Height, Height);

                    var lengthRoot2 = Math.Sqrt(decompressedData.Length);
                    if (lengthRoot2 == (int) lengthRoot2)
                        return decompressedData.To2DArrayColumnMajor((int) lengthRoot2, (int) lengthRoot2);

                    return decompressedData.To2DArrayColumnMajor(1, decompressedData.Length);
                }

                // No errors -- decompress normally.
                ErrorWhileDecompressing = null;
                return decompressedData.To2DArrayColumnMajor(Width, Height);
            }
            catch {
                return new ushort[0, 0];
            }
        }

        public FrameTextureInfo FrameInfo { get; }
 
        [TableViewModelColumn(displayOrder: 1, minWidth: 200)]
        public string TextureHash => Texture?.Hash;

        [TableViewModelColumn(displayOrder: 2, minWidth: 175)]
        public string SpriteName => FrameInfo?.SpriteName;

        [TableViewModelColumn(displayOrder: 3)]
        public string AnimationName => FrameInfo?.AnimationName;

        [TableViewModelColumn(displayOrder: 4)]
        public string Directions => FrameInfo?.DirectionsString;

        [TableViewModelColumn(displayOrder: 5, minWidth: 200)]
        public string ErrorWhileDecompressing { get; private set; } = null;
    }
}