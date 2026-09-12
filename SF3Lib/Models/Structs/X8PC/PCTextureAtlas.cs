using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Extensions;
using CommonLib.Imaging;
using CommonLib.Types;
using SF3.ThirdParty.TexturePacker;
using SF3.ThirdParty.TexturePacker.Extensions;

namespace SF3.Models.Structs.X8PC {
    public class PCTextureAtlas : CachedTextureDataBase, IDisposable {
        private class SubAtlasTexture : CachedTextureDataBase, ITexture, IDisposable {
            public SubAtlasTexture(int textureId, TextureAtlas atlas) {
                TextureID    = textureId;
                TextureAtlas = atlas;
                var dimensions = TextureAtlas.GetDimensions();

                // Pre-cache the image data which builds the trimmed atlas bitmap. Set the width and height based on that.
                var imageData = ImageData16Bit;
                _width  = imageData.GetLength(0);
                _height = imageData.GetLength(1);

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
            protected override ushort[,] FetchImageData16Bit() => TextureAtlas.CreateBitmap().Trim(ignoreTopLeft: false, clampToPow2: false).Get2DDataABGR1555();

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

        public PCTextureAtlas(Dictionary<int, TextureAtlas> atlasesByModelID) {
            _subAtlasesByModelID = atlasesByModelID;

            var textures = atlasesByModelID.Select(x => new SubAtlasTexture(x.Key, x.Value))
                .OrderByDescending(x => x.Height)
                .ToArray();

            _subAtlasTextures = textures;
            foreach (var tex in _subAtlasTextures)
                tex.Invalidated += OnSubAtlasTextureInvalidated;

            _textureAtlas = new TextureAtlas(textures, padding: 0, tryRotate: true, sortBySize: false, minWidth: 320);
            var dimensions = _textureAtlas.GetDimensions();
            _width = dimensions.Width;
            _height = dimensions.Height;

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
            var data = _textureAtlas.CreateBitmap()?.Trim(ignoreTopLeft: false, clampToPow2: false, ignoreWidth: true)?.Get2DDataABGR1555() ?? new ushort[0, 0];
            _width = data.GetLength(0);
            _height = data.GetLength(1);
            return data;
        }

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

                            int xx = 1, xy = 0;
                            int yx = 0, yy = 1;

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
            }

            Invalidate();
        }

        private readonly Dictionary<int, TextureAtlas> _subAtlasesByModelID;
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
