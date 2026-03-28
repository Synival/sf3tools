using System;
using System.Collections.Generic;
using CommonLib.Extensions;
using CommonLib.Types;
using CommonLib.Utils;

namespace CommonLib.Imaging {
    /// <summary>
    /// Core functionality of a cached TextureData. How the data itself is retrieved and set is left to the
    /// implmenetation, but the caching mechanism is still present.
    /// </summary>
    public abstract class CachedTextureDataBase : ITextureData {
        public delegate string Validator8Bit(byte[,] data, IPalette palette, int? oldStoredSize, int? newStoredSize);
        public delegate string Validator16Bit(ushort[,] data, int? oldStoredSize, int? newStoredSize);

        public byte[] GetBitmapDataARGB1555(bool highlightEndcodes = false) {
            if (BytesPerPixel == 1)
                return _textureDataCache.GetOrCacheBitmapDataARGB1555(() => BitmapUtils.ConvertIndexedDataToARGB1555BitmapData(ImageData8Bit, Palette, ZeroIsTransparent));
            else if (highlightEndcodes)
                return _textureDataCache.GetOrCacheBitmapDataARGB1555_Endcodes(() => BitmapUtils.ConvertABGR1555DataToARGB1555BitmapData(ImageData16Bit, true));
            else
                return _textureDataCache.GetOrCacheBitmapDataARGB1555(() => BitmapUtils.ConvertABGR1555DataToARGB1555BitmapData(ImageData16Bit, false));
        }

        public byte[] GetBitmapDataARGB8888(bool highlightEndcodes = false) {
            if (BytesPerPixel == 1)
                return _textureDataCache.GetOrCacheBitmapDataARGB8888(() => BitmapUtils.ConvertIndexedDataToARGB8888BitmapData(ImageData8Bit, Palette, ZeroIsTransparent));
            else if (highlightEndcodes)
                return _textureDataCache.GetOrCacheBitmapDataARGB8888_Endcodes(() => BitmapUtils.ConvertABGR1555DataToARGB8888BitmapData(ImageData16Bit, true));
            else
                return _textureDataCache.GetOrCacheBitmapDataARGB8888(() => BitmapUtils.ConvertABGR1555DataToARGB8888BitmapData(ImageData16Bit, false));
        }

        public void Invalidate(bool sendEvent = true) {
            if (_invalidateGuard <= 0) {
                _textureDataCache.Invalidate();
                if (sendEvent)
                    Invalidated?.Invoke(this, EventArgs.Empty);
            }
        }

        protected void InvokeInvalidatedEvent()
            => Invalidated?.Invoke(this, EventArgs.Empty);

        public virtual string Validate8BitImageData(byte[,] data, IPalette palette, int? oldStoredSize, int? newStoredSize) {
            if (!CanSetImageData.HasFlag(ImageDataCanSet.CanSet8Bit))
                return "Not supported";
            foreach (var validator in _validators8Bit) {
                var error = validator(data, palette, oldStoredSize, newStoredSize);
                if (error != null)                
                    return error;
            }
            return null;
        }

        public virtual string Validate16BitImageData(ushort[,] data, int? oldStoredSize, int? newStoredSize) {
            if (!CanSetImageData.HasFlag(ImageDataCanSet.CanSet16Bit))
                return "Not supported";
            foreach (var validator in _validators16Bit) {
                var error = validator(data, oldStoredSize, newStoredSize);
                if (error != null)                
                    return error;
            }
            return null;
        }

        public void Add8BitValidator(Validator8Bit v)
            => _validators8Bit.Add(v);

        public void Add16BitValidator(Validator16Bit v)
            => _validators16Bit.Add(v);

        public ScopeGuard InvalidateGuard()
            => new ScopeGuard(() => _invalidateGuard++, () => _invalidateGuard--);

        public byte[] BitmapDataARGB1555 => GetBitmapDataARGB1555(false);
        public byte[] BitmapDataARGB8888 => GetBitmapDataARGB8888(false);

        public byte[,] ImageData8Bit => _textureDataCache.GetOrCacheImageData8Bit(FetchImageData8Bit);
        public ushort[,] ImageData16Bit { 
            get => _textureDataCache.GetOrCacheImageData16Bit(FetchImageData16Bit);
            set => SetImageData16Bit(value);
        }

        public string Hash => _textureDataCache.GetOrCacheHash(() => BitmapDataARGB1555.CreateTextureHash());
        public int BytesPerPixel => PixelFormat.BytesPerPixel();
        public int ImageDataSize => Width * Height * BytesPerPixel;

        public abstract TexturePixelFormat PixelFormat { get; set; }
        public abstract int Width { get; set; }
        public abstract int Height { get; set; }
        public abstract IPalette Palette { get; set; }
        public abstract bool ZeroIsTransparent { get; set; }
        public abstract ImageDataCanSet CanSetImageData { get; set; }

        protected abstract byte[,] FetchImageData8Bit();
        public abstract void SetImageData8Bit(byte[,] data, IPalette palette);
        protected abstract ushort[,] FetchImageData16Bit();
        protected abstract void SetImageData16Bit(ushort[,] data);

        private int _invalidateGuard = 0;
        protected TextureDataCache _textureDataCache    = new TextureDataCache();
        protected List<Validator8Bit> _validators8Bit   = new List<Validator8Bit>();
        protected List<Validator16Bit> _validators16Bit = new List<Validator16Bit>();

        public event EventHandler Invalidated;
    }
}
