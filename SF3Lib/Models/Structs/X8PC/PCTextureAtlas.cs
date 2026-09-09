using System;
using System.Collections.Generic;
using System.Drawing;
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
            var textures = bitmaps.Select((x, i) => new MockTexture(i, x.Get2DDataABGR1555())).ToArray();

            if (textures.Length > 0) {
                var textureAtlas = new TextureAtlas(textures, padding: 0, tryRotate: true, sortBySize: true, minWidth: 320);
                _textureBitmap = textureAtlas.CreateBitmap().Trim(ignoreTopLeft: false, clampToPow2: false, ignoreWidth: true);
            }
            else {
                _textureBitmap = null;
            }
        }

        private Bitmap _textureBitmap;

        public override TexturePixelFormat PixelFormat { get => TexturePixelFormat.ABGR1555; set => throw new NotSupportedException(); }
        public override int Width { get => _textureBitmap?.Width ?? 16; set => throw new NotSupportedException(); }
        public override int Height { get => _textureBitmap?.Height ?? 16; set => throw new NotSupportedException(); }
        public override IPalette Palette { get => null; set => throw new NotSupportedException(); }
        public override bool ZeroIsTransparent { get => false; set => throw new NotSupportedException(); }
        public override ImageDataCanSet CanSetImageData { get => ImageDataCanSet.CanSet16Bit; set => throw new NotSupportedException(); }
        protected override byte[,] FetchImageData8Bit() => null;
        public override void SetImageData8Bit(byte[,] data, IPalette palette) => throw new NotSupportedException();

        protected override ushort[,] FetchImageData16Bit() => _textureBitmap?.Get2DDataABGR1555() ?? new ushort[Width, Height];

        protected override void SetImageData16Bit(ushort[,] data) {
            // TODO: everything!
        }
    }
}
