using System;
using CommonLib.Extensions;
using CommonLib.Types;
using CommonLib.Utils;
using Newtonsoft.Json.Linq;

namespace CommonLib.Imaging {
    public class TextureData : TextureDataStandard, ITextureData {
        public TextureData(ITextureData original, Palette palette)
        : base(original.Width, original.Height, original.PixelFormat, original.ZeroIsTransparent) {
            CanSetImage        = original.CanSetImageData8Bit || original.CanSetImageData16Bit;
            _palette           = palette;

            if (PixelFormat == TexturePixelFormat.Indexed8Bit)
                _textureDataBuffer.SetImageData8Bit(original.ImageData8Bit);
            if (PixelFormat == TexturePixelFormat.ABGR1555)
                _textureDataBuffer.SetImageData16Bit(original.ImageData16Bit);
        }

        public TextureData(byte[,] data, Palette palette, bool zeroIsTransparent, bool canSetImage)
        : base(data?.GetLength(0) ?? 0, data?.GetLength(1) ?? 0, TexturePixelFormat.Indexed8Bit, zeroIsTransparent) {
            if (data != null)
                _textureDataBuffer.SetImageData8Bit(data);

            _palette    = palette;
            CanSetImage = canSetImage;
        }

        public TextureData(ushort[,] data, bool canSetImage)
        : base(data?.GetLength(0) ?? 0, data?.GetLength(1) ?? 0, TexturePixelFormat.ABGR1555, zeroIsTransparent: false) {
            if (data != null)
                _textureDataBuffer.SetImageData16Bit(data);

            CanSetImage = canSetImage;
        }

        public TextureData(int width, int height, TexturePixelFormat pixelFormat, Palette palette, bool zeroIsTransparent, bool canSetImage)
        : base(width, height, pixelFormat, zeroIsTransparent) {
            _palette    = palette;
            CanSetImage = canSetImage;
        }

        public static TextureData FromJToken(JToken token, bool zeroIsTransparent, Palette palette, bool canSetImage)
            => new TextureData((JObject) token, zeroIsTransparent, palette, canSetImage);
        protected TextureData(JObject jObject, bool zeroIsTransparent, Palette palette, bool canSetImage)
        : base(
            (int) jObject["Width"],
            (int) jObject["Height"],
            (TexturePixelFormat) Enum.Parse(typeof(TexturePixelFormat),
            (string) jObject["PixelFormat"]), zeroIsTransparent
        ) {
            _palette    = palette;
            CanSetImage = canSetImage;

            var imageDataBase64 = (string) jObject["ImageData"];
            if (PixelFormat == TexturePixelFormat.ABGR1555)
                _textureDataBuffer.SetImageData16Bit(Convert.FromBase64String(imageDataBase64).ToUShorts().To2DArrayColumnMajor(Width, Height));
            else {
                _textureDataBuffer.SetImageData8Bit(Convert.FromBase64String(imageDataBase64).To2DArrayColumnMajor(Width, Height));
                _palette = Palette;
            }
        }

        public static TextureData FromJToken(JToken token, int width, int height, TexturePixelFormat pixelFormat, bool zeroIsTransparent, Palette palette, bool canSetImage)
            => new TextureData(token, width, height, pixelFormat, zeroIsTransparent, palette, canSetImage);
        protected TextureData(JToken token, int width, int height, TexturePixelFormat pixelFormat, bool zeroIsTransparent, Palette palette, bool canSetImage)
        : base(width, height, pixelFormat, zeroIsTransparent) {
            _palette    = palette;
            CanSetImage = canSetImage;

            var imageDataBase64 = (string) token;
            if (PixelFormat == TexturePixelFormat.ABGR1555)
                _textureDataBuffer.SetImageData16Bit(Convert.FromBase64String(imageDataBase64).ToUShorts().To2DArrayColumnMajor(Width, Height));
            else {
                _textureDataBuffer.SetImageData8Bit(Convert.FromBase64String(imageDataBase64).To2DArrayColumnMajor(Width, Height));
                _palette = Palette;
            }
        }

        private Palette _palette;
        public override Palette Palette {
            get => _palette;
            set {
                if (SetPalette(value))
                    Invalidate();
            }
        }

        public override bool CanSetImageData8Bit => CanSetImage;
        public override bool CanSetImageData16Bit => CanSetImage;
        public virtual bool CanSetImage { get; set; }

        protected override byte[,] FetchImageData8Bit() {
            if (BytesPerPixel != 1)
                throw new InvalidOperationException();

            // Nothing to fetch; if it's not set, it's not set.
            return null;
        }

        public override void SetImageData8Bit(byte[,] data, Palette palette) {
            var newWidth = data.GetLength(0);
            var newHeight = data.GetLength(1);

            var error = Validate8BitImageData(data, palette, ImageDataSize, newWidth * newHeight);
            if (error != null)
                throw new ArgumentException(error);

            SetPixelFormatInternal(TexturePixelFormat.Indexed8Bit, invalidate: false);
            SetDimensionsInternal(newWidth, newHeight, invalidate: false);

            Invalidate(sendEvent: false);
            using (new ScopeGuard(() => _invalidateGuard++, () => _invalidateGuard--)) {
                _ = _textureDataBuffer.SetImageData8Bit(data);
                SetPalette(palette);
            }

            InvokeInvalidatedEvent();
        }

        protected override ushort[,] FetchImageData16Bit() {
            if (BytesPerPixel != 2)
                throw new InvalidOperationException();

            // Nothing to fetch; if it's not set, it's not set.
            return null;
        }

        protected override void SetImageData16Bit(ushort[,] data) {
            var newWidth = data.GetLength(0);
            var newHeight = data.GetLength(1);

            var error = Validate16BitImageData(data, ImageDataSize, newWidth * newHeight * 2);
            if (error != null)
                throw new ArgumentException(error);

            PixelFormat = TexturePixelFormat.ABGR1555;
            Width  = newWidth;
            Height = newHeight;

            Invalidate(sendEvent: false);
            using (new ScopeGuard(() => _invalidateGuard++, () => _invalidateGuard--)) {
                var newData = data.Clone() as ushort[,];
                newData.FixSaturnTransparency(useEndCodes: true);
                _ = _textureDataBuffer.SetImageData16Bit(newData);
            }

            InvokeInvalidatedEvent();
        }

        public virtual bool SetPalette(Palette palette) {
            if (_palette == palette)
                return false;
            _palette = palette;
            return true;
        }
    }
}
