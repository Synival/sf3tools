using System;
using System.Collections.Generic;
using CommonLib.Imaging;
using CommonLib.Types;
using SF3.Types;

namespace SF3.Imaging {
    public class MPD_MockAnimationFrame : IMPD_AnimationFrame, IDisposable {
        public MPD_MockAnimationFrame(IMPD_Texture texture) {
            _texture = texture;
            _texture.Invalidated += InvalidateSelfHandler;
        }

        private void InvalidateSelfHandler(object sender, EventArgs args)
            => this.Invalidated?.Invoke(sender, args);

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing) {
            if (!_disposedValue) {
                if (disposing)
                    _texture.Invalidated -= InvalidateSelfHandler;

                _disposedValue = true;
            }
        }

        public int Frame => 0;
        public int Duration => 0;

        public void SetImageData8Bit(byte[,] data, Palette palette)
            => _texture.SetImageData8Bit(data, palette);

        public byte[] GetBitmapDataARGB1555(bool highlightEndcodes = false)
            => _texture.GetBitmapDataARGB1555(highlightEndcodes);

        public byte[] GetBitmapDataARGB8888(bool highlightEndcodes = false)
            => _texture.GetBitmapDataARGB8888(highlightEndcodes);

        public string Validate8BitImageData(byte[,] data, Palette palette, int oldStoredSize, int newStoredSize)
            => _texture.Validate8BitImageData(data, palette, oldStoredSize, newStoredSize);

        public string Validate16BitImageData(ushort[,] data, int oldStoredSize, int newStoredSize)
            => _texture.Validate16BitImageData(data, oldStoredSize, newStoredSize);

        public MPD_CollectionType Collection => _texture.Collection;
        public int ID => _texture.ID;
        public Dictionary<TagKey, TagValue> Tags => _texture.Tags;
        public int BytesPerPixel => _texture.BytesPerPixel;
        public TexturePixelFormat PixelFormat => _texture.PixelFormat;
        public int Width => _texture.Width;
        public int Height => _texture.Height;
        public int ImageDataSize => _texture.ImageDataSize;
        public byte[,] ImageData8Bit => _texture.ImageData8Bit;
        public ushort[,] ImageData16Bit { get => _texture.ImageData16Bit; set => _texture.ImageData16Bit = value; }
        public byte[] BitmapDataARGB1555 => _texture.BitmapDataARGB1555;
        public byte[] BitmapDataARGB8888 => _texture.BitmapDataARGB8888;
        public string Hash => _texture.Hash;
        public Palette Palette => _texture.Palette;
        public bool ZeroIsTransparent => _texture.ZeroIsTransparent;
        public bool CanSetImageData8Bit => _texture.CanSetImageData8Bit;
        public bool CanSetImageData16Bit => _texture.CanSetImageData16Bit;
        public bool IsIgnored => false;

        private readonly IMPD_Texture _texture;
        private bool _disposedValue;

        public event EventHandler Invalidated;
    }
}
