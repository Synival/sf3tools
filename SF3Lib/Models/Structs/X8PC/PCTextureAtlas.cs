using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Extensions;
using CommonLib.Imaging;
using CommonLib.Types;
using SF3.ThirdParty.TexturePacker;
using SF3.ThirdParty.TexturePacker.Extensions;

namespace SF3.Models.Structs.X8PC {
    public class PCTextureAtlas : CachedTextureDataBase {
        private class MockTexture : InMemoryTextureData, ITexture {
            public MockTexture(int id, ushort[,] data)
            : base(data, ImageDataCanSet.Never, IndexedColorUpdateStrategy.DontUpdate) {
                TextureID = id;
            }

            public int TextureCollectionID => 0;
            public int TextureID { get; }
        }

        public PCTextureAtlas(Dictionary<int, TextureAtlas> atlasesByModelID) {
            var bitmaps = atlasesByModelID.Select(x => x.Value.CreateBitmap().Trim(ignoreTopLeft: false, clampToPow2: false)).ToArray();
            var textures = bitmaps.Select((x, i) => new MockTexture(i, x.Get2DDataABGR1555()))
                .OrderByDescending(x => x.Height)
                .ToArray();

            _subAtlasesByModelID = atlasesByModelID;
            _textureAtlas = new TextureAtlas(textures, padding: 0, tryRotate: true, sortBySize: false, minWidth: 320);
            var dimensions = _textureAtlas.GetDimensions();
            _width = dimensions.Width;
            _height = dimensions.Height;
        }

        public override TexturePixelFormat PixelFormat { get => TexturePixelFormat.ABGR1555; set => throw new NotSupportedException(); }
        public override int Width { get => _width; set => throw new NotSupportedException(); }
        public override int Height { get => _height; set => throw new NotSupportedException(); }
        public override IPalette Palette { get => null; set => throw new NotSupportedException(); }
        public override bool ZeroIsTransparent { get => false; set => throw new NotSupportedException(); }
        public override ImageDataCanSet CanSetImageData { get => ImageDataCanSet.CanSet16Bit; set => throw new NotSupportedException(); }
        protected override byte[,] FetchImageData8Bit() => null;
        public override void SetImageData8Bit(byte[,] data, IPalette palette) => throw new NotSupportedException();

        protected override ushort[,] FetchImageData16Bit() {
            var data = _textureAtlas.CreateBitmap().Trim(ignoreTopLeft: false, clampToPow2: false, ignoreWidth: true).Get2DDataABGR1555();
            _width = data.GetLength(0);
            _height = data.GetLength(1);
            return data;
        }

        protected override void SetImageData16Bit(ushort[,] data) {
            foreach (var sub in _subAtlasesByModelID) {
                var modelId = sub.Key;
                var atlas = sub.Value;

                ushort idx = 0;
                foreach (var node in atlas.GetAllNodes()) {
                    // TODO: this is all temporary test stuff.
                    var tex = node.Texture;
                    var newData = new ushort[tex.Width, tex.Height];

                    float r = (idx >> 0) % 32;
                    float g = modelId % 32;
                    float b = (idx >> 5) % 32;
                    var color = new PixelChannels() { R = (byte) (r / 31.0 * 255.0), G = (byte) (g / 31.0 * 255.0), B = (byte) (b / 31.0 * 255.0), A = 255 }.ToABGR1555();

                    System.Diagnostics.Debug.WriteLine($"{color:X4}");

                    unsafe {
                        fixed (ushort* ptr = &newData[0, 0]) {
                            Span<ushort> newDataSpan = new Span<ushort>(ptr, newData.Length);
                            newDataSpan.Fill(color);
                        }
                    }
                    tex.ImageData16Bit = newData;
                    idx++;
                }
            }
        }

        private readonly Dictionary<int, TextureAtlas> _subAtlasesByModelID;
        private readonly TextureAtlas _textureAtlas;
        private int _width;
        private int _height;
    }
}
