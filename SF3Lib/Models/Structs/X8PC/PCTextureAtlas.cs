using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CommonLib.Extensions;
using CommonLib.Imaging;
using CommonLib.Types;
using SF3.ThirdParty.TexturePacker;

namespace SF3.Models.Structs.X8PC {
    public class PCTextureAtlas : CachedTextureDataBase, IDisposable {
        private class SubAtlasTexture : CachedTextureDataBase, ITexture, IDisposable {
            public SubAtlasTexture(int textureId, TextureAtlas atlas) {
                TextureID    = textureId;
                TextureAtlas = atlas;

                // Pre-cache the image data which builds the trimmed atlas bitmap. Set the width and height based on that.
                _ = ImageData16Bit;

                var textures = TextureAtlas.GetAllNodes().Where(x => x.Texture != null).Select(x => x.Texture).ToArray();
                foreach (var tex in textures)
                    tex.Invalidated += OnAtlasTextureInvalidate;
            }

            public int TextureCollectionID => 0;
            public int TextureID { get; }
            public TextureAtlas TextureAtlas { get; }

            private int _width;
            private int _height;

            // Unsupported features.
            public override IPalette Palette { get => null; set => throw new NotSupportedException(); }
            public override bool ZeroIsTransparent { get => false; set => throw new NotSupportedException(); }
            public override void SetImageData8Bit(byte[,] data, IPalette palette) => throw new NotSupportedException();
            protected override byte[,] FetchImageData8Bit() => throw new NotSupportedException();
            protected override void SetImageData16Bit(ushort[,] data) => throw new NotSupportedException();

            // Read-only features.
            public override TexturePixelFormat PixelFormat { get => TexturePixelFormat.ABGR1555; set => throw new NotSupportedException(); }
            public override int Width { get => _width; set => throw new NotSupportedException(); }
            public override int Height { get => _height; set => throw new NotSupportedException(); }
            public override ImageDataCanSet CanSetImageData { get => ImageDataCanSet.CanSet16Bit; set => throw new NotSupportedException(); }

            // Fully supported features.
            protected override ushort[,] FetchImageData16Bit() {
                ushort[,] data;
                using (var bitmap = TextureAtlas.CreateBitmap(onlyTextures: true))
                    data = bitmap.Get2DDataAtABGR1555(TextureAtlas.GetDimensions(onlyTextures: true));
                _width  = data.GetLength(0);
                _height = data.GetLength(1);
                return data;
            }

            private bool disposedValue;
            protected virtual void Dispose(bool disposing) {
                if (!disposedValue) {
                    if (disposing) {
                        var textures = TextureAtlas.GetAllNodes().Where(x => x.Texture != null).Select(x => x.Texture).ToArray();
                        foreach (var tex in textures)
                            tex.Invalidated -= OnAtlasTextureInvalidate;
                    }
                    disposedValue = true;
                }
            }

            private void OnAtlasTextureInvalidate(object sender, EventArgs args)
                => Invalidate();

            public void Dispose() {
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }
        }

        public PCTextureAtlas(Dictionary<int, TextureAtlas> atlasesByModelID, IPalette attrPalette) {
            _subAtlasesByModelID = atlasesByModelID;
            AttrPalette = attrPalette;

            var textures = atlasesByModelID
                .Select(x => new SubAtlasTexture(x.Key, x.Value))
                .OrderByDescending(x => x.Height)
                .ToArray();

            _subAtlasTextures = textures;
            foreach (var tex in _subAtlasTextures)
                tex.Invalidated += OnSubAtlasTextureInvalidated;

            _textureAtlas = new TextureAtlas(textures, padding: 0, tryRotate: false, sortBySize: false, minWidth: 320);

            // Pre-cache the image data which builds the trimmed atlas bitmap. Set the width and height based on that.
            _ = ImageData16Bit;

            Add16BitValidator((texData, _1, _2) => TextureDataValidators.IsSameDimensions(texData, Width, Height));
        }

        private void OnSubAtlasTextureInvalidated(object sender, EventArgs args)
            => Invalidate();

        public override TexturePixelFormat PixelFormat { get => TexturePixelFormat.ABGR1555; set => throw new NotSupportedException(); }
        public override int Width { get => _width; set => throw new NotSupportedException(); }
        public override int Height { get => _height; set => throw new NotSupportedException(); }
        public override IPalette Palette { get => null; set => throw new NotSupportedException(); }
        public override bool ZeroIsTransparent { get => false; set => throw new NotSupportedException(); }
        public override ImageDataCanSet CanSetImageData { get => ImageDataCanSet.CanSet16Bit; set => throw new NotSupportedException(); }
        protected override byte[,] FetchImageData8Bit() => null;
        public override void SetImageData8Bit(byte[,] data, IPalette palette) => throw new NotSupportedException();

        protected override ushort[,] FetchImageData16Bit() {
            // TODO: Why do we have to force an even width here??? This breaks if we don't, but why???
            var bitmapDimensions = _textureAtlas.GetDimensions(onlyTextures: true, forceEvenWidth: true);

            // Enforce minimum width and extra pixels for the palette.
            bitmapDimensions.Width = Math.Max(16, bitmapDimensions.Width);
            bitmapDimensions.Height += PaletteHeightForWidth(bitmapDimensions.Width);

            // Get the image data.
            ushort[,] data;
            using (var bitmap = new Bitmap(bitmapDimensions.Width, bitmapDimensions.Height)) {
                _textureAtlas.DrawPackedNodes(bitmap);
                data = bitmap.Get2DDataAtABGR1555(bitmapDimensions) ?? new ushort[16, 1];
            }

            // Cache the width/height.
            _width  = data.GetLength(0);
            _height = data.GetLength(1);

            // Add the palette to the bottom.
            if (AttrPalette != null) {
                var paletteColorCount = AttrPalette.ColorCount;
                for (int i = 0; i < paletteColorCount; i++)
                    data[i % _width, (i / _width) + _height - 1] = AttrPalette[i].ToABGR1555();
            }

            return data;
        }

        private int PaletteHeightForWidth(int width)
            => (AttrPalette == null) ? 0 : (AttrPalette.ColorCount + (width - 1)) / width;

        protected override void SetImageData16Bit(ushort[,] data) {
            if (data != null) {
                var error = Validate16BitImageData(data, ImageData16Bit.Length, data.Length);
                if (error != null)
                    throw new ArgumentException(error);
            }

            var metaAtlasNodes = _textureAtlas.GetAllNodes().OrderBy(x => x.Texture.TextureID).ToArray();

            // Don't auto-invalidate ourselves; do it manually after.
            using (InvalidateGuard()) {
                // Prevent invalidation of all subatlas textures to prevent excessive event-firing.
                var texScopeGuards = _subAtlasTextures.Select(x => (Tex: x, Guard: x.InvalidateGuard())).ToArray();

                try {
                    int metaAtlasIdx = 0;
                    foreach (var sub in _subAtlasesByModelID) {
                        var metaAtlasNode = metaAtlasNodes[metaAtlasIdx++];
                        var metaAtlasNodeRect = metaAtlasNode.Rect;

                        var modelId = sub.Key;
                        var atlas = sub.Value;

                        foreach (var node in atlas.GetAllNodes().OrderBy(x => x.Texture.TextureID).ToArray()) {
                            var tex = node.Texture;
                            var newData = new ushort[tex.Width, tex.Height];

                            var rect = node.Rect;
                            var (imageX, imageY) = (metaAtlasNodeRect.Left + rect.Left, metaAtlasNodeRect.Top + rect.Top);

                            var rotateCount = (node.Rotated ? 1 : 0) + (metaAtlasNode.Rotated ? 1 : 0);
                            int width  = tex.Width;
                            int height = tex.Height;

                            for (int y = 0; y < height; y++) {
                                for (int x = 0; x < width; x++) {
                                    var px = imageX + ((rotateCount == 0) ? x : (rotateCount == 1) ? (height - y - 1) : (width - x - 1));
                                    var py = imageY + ((rotateCount == 0) ? y : (rotateCount == 1) ? x : (height - y - 1));

                                    var pixel = data[px, py];
                                    var channels = PixelConversion.ABGR1555toChannels(pixel);
                                    newData[x, y] = channels.ToABGR1555();
                                }
                            }

                            tex.ImageData16Bit = newData;
                        }
                    }
                }
                finally {
                    foreach (var tsg in texScopeGuards) {
                        tsg.Guard.Dispose();
                        tsg.Tex.Invalidate();
                    }
                }

                // Update the palette at the bottom.
                if (AttrPalette != null) {
                    var paletteColorCount = AttrPalette.ColorCount;
                    var newPalette = new PixelChannels[paletteColorCount];
                    for (int i = 0; i < paletteColorCount; i++)
                        newPalette[i] = PixelConversion.ABGR1555toChannels(data[i % _width, (i / _width) + _height - 1]);
                    AttrPalette.Replace(newPalette);
                }
            }

            Invalidate();
        }

        private readonly Dictionary<int, TextureAtlas> _subAtlasesByModelID;
        public IPalette AttrPalette { get; }

        private readonly SubAtlasTexture[] _subAtlasTextures;
        private readonly TextureAtlas _textureAtlas;
        private int _width;
        private int _height;

        private bool disposedValue;
        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                foreach (var tex in _subAtlasTextures) {
                    tex.Invalidated -= OnSubAtlasTextureInvalidated;
                    tex.Dispose();
                }
                _textureAtlas.Dispose();
                disposedValue = true;
            }
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
