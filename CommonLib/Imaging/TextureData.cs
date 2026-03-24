using System;
using CommonLib.Extensions;
using CommonLib.Types;
using CommonLib.Utils;
using Newtonsoft.Json.Linq;

namespace CommonLib.Imaging {
    public class TextureData : TextureDataStandard, ITextureData {
        public TextureData(ITextureData original, Palette palette)
        : base(
            original.Width,
            original.Height,
            original.PixelFormat,
            palette,
            original.ZeroIsTransparent,
            original.CanSetImageData8Bit || original.CanSetImageData16Bit,
            new NullTextureDataSource()
        ) {
            if (PixelFormat == TexturePixelFormat.Indexed8Bit)
                _ = _textureDataBuffer.SetImageData8Bit(original.ImageData8Bit);
            if (PixelFormat == TexturePixelFormat.ABGR1555)
                _ = _textureDataBuffer.SetImageData16Bit(original.ImageData16Bit);
        }

        public TextureData(byte[,] data, Palette palette, bool zeroIsTransparent, bool canSetImage)
        : base(
            data?.GetLength(0) ?? 0,
            data?.GetLength(1) ?? 0,
            TexturePixelFormat.Indexed8Bit,
            palette,
            zeroIsTransparent,
            canSetImage,
            new NullTextureDataSource()
        ) {
            if (data != null)
                _ = _textureDataBuffer.SetImageData8Bit(data);
        }

        public TextureData(ushort[,] data, bool canSetImage)
        : base(
            data?.GetLength(0) ?? 0,
            data?.GetLength(1) ?? 0,
            TexturePixelFormat.ABGR1555,
            palette: null,
            zeroIsTransparent: false,
            canSetImage,
            new NullTextureDataSource()
        ) {
            if (data != null)
                _ = _textureDataBuffer.SetImageData16Bit(data);
        }

        public TextureData(int width, int height, TexturePixelFormat pixelFormat, Palette palette, bool zeroIsTransparent, bool canSetImage)
        : base(
            width,
            height,
            pixelFormat,
            palette,
            zeroIsTransparent,
            canSetImage,
            new NullTextureDataSource()
        ) {
        }

        public static TextureData FromJToken(JToken token, bool zeroIsTransparent, Palette palette, bool canSetImage)
            => new TextureData((JObject) token, zeroIsTransparent, palette, canSetImage);
        protected TextureData(JObject jObject, bool zeroIsTransparent, Palette palette, bool canSetImage)
        : base(
            (int) jObject["Width"],
            (int) jObject["Height"],
            (TexturePixelFormat) Enum.Parse(typeof(TexturePixelFormat), (string) jObject["PixelFormat"]),
            palette,
            zeroIsTransparent,
            canSetImage,
            new NullTextureDataSource()
        ) {
            var imageDataBase64 = (string) jObject["ImageData"];
            if (PixelFormat == TexturePixelFormat.ABGR1555)
                _ = _textureDataBuffer.SetImageData16Bit(Convert.FromBase64String(imageDataBase64).ToUShorts().To2DArrayColumnMajor(Width, Height));
            else
                _ = _textureDataBuffer.SetImageData8Bit(Convert.FromBase64String(imageDataBase64).To2DArrayColumnMajor(Width, Height));
        }

        public static TextureData FromJToken(JToken token, int width, int height, TexturePixelFormat pixelFormat, bool zeroIsTransparent, Palette palette, bool canSetImage)
            => new TextureData(token, width, height, pixelFormat, zeroIsTransparent, palette, canSetImage);
        protected TextureData(JToken token, int width, int height, TexturePixelFormat pixelFormat, bool zeroIsTransparent, Palette palette, bool canSetImage)
        : base(
            width,
            height,
            pixelFormat,
            palette,
            zeroIsTransparent,
            canSetImage,
            new NullTextureDataSource()
        ) {
            var imageDataBase64 = (string) token;
            if (PixelFormat == TexturePixelFormat.ABGR1555)
                _ = _textureDataBuffer.SetImageData16Bit(Convert.FromBase64String(imageDataBase64).ToUShorts().To2DArrayColumnMajor(Width, Height));
            else
                _ = _textureDataBuffer.SetImageData8Bit(Convert.FromBase64String(imageDataBase64).To2DArrayColumnMajor(Width, Height));
        }

        public override void SetImageData8Bit(byte[,] data, Palette palette) {
            var error = Validate8BitImageData(data, palette, null, null);
            if (error != null)
                throw new ArgumentException(error);

            Invalidate(sendEvent: false);
            using (InvalidateGuard()) {
                PixelFormat = TexturePixelFormat.Indexed8Bit;
                Width = data.GetLength(0);
                Height = data.GetLength(1);
                _ = _textureDataBuffer.SetImageData8Bit(data);
                Palette = palette;
            }
            InvokeInvalidatedEvent();
        }

        protected override void SetImageData16Bit(ushort[,] data) {
            data = data.Clone() as ushort[,];
            data.FixSaturnTransparency(useEndCodes: true);

            var error = Validate16BitImageData(data, null, null);
            if (error != null)
                throw new ArgumentException(error);

            Invalidate(sendEvent: false);
            using (InvalidateGuard()) {
                PixelFormat = TexturePixelFormat.ABGR1555;
                Width = data.GetLength(0);
                Height = data.GetLength(1);
                _ = _textureDataBuffer.SetImageData16Bit(data);
            }
            InvokeInvalidatedEvent();
        }
    }
}
