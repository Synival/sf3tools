using System;
using CommonLib.Extensions;
using CommonLib.Types;
using CommonLib.Utils;
using Newtonsoft.Json.Linq;

namespace CommonLib.Imaging {
    /// <summary>
    /// A texture whose image data exists solely in memory.
    /// </summary>
    public class InMemoryTextureData : CachedTextureData, ITextureData {
        public InMemoryTextureData(ITextureData original, Palette palette)
        : base(
            original.Width,
            original.Height,
            original.PixelFormat,
            palette,
            original.ZeroIsTransparent,
            original.CanSetImageData8Bit || original.CanSetImageData16Bit,
            new DummyTextureDataSource()
        ) {
            if (PixelFormat == TexturePixelFormat.Indexed8Bit)
                _ = _textureDataCache.SetImageData8Bit(original.ImageData8Bit);
            if (PixelFormat == TexturePixelFormat.ABGR1555)
                _ = _textureDataCache.SetImageData16Bit(original.ImageData16Bit);
        }

        public InMemoryTextureData(byte[,] data, Palette palette, bool zeroIsTransparent, bool canSetImage)
        : base(
            data?.GetLength(0) ?? 0,
            data?.GetLength(1) ?? 0,
            TexturePixelFormat.Indexed8Bit,
            palette,
            zeroIsTransparent,
            canSetImage,
            new DummyTextureDataSource()
        ) {
            if (data != null)
                _ = _textureDataCache.SetImageData8Bit(data);
        }

        public InMemoryTextureData(ushort[,] data, bool canSetImage)
        : base(
            data?.GetLength(0) ?? 0,
            data?.GetLength(1) ?? 0,
            TexturePixelFormat.ABGR1555,
            palette: null,
            zeroIsTransparent: false,
            canSetImage,
            new DummyTextureDataSource()
        ) {
            if (data != null)
                _ = _textureDataCache.SetImageData16Bit(data);
        }

        public InMemoryTextureData(int width, int height, TexturePixelFormat pixelFormat, Palette palette, bool zeroIsTransparent, bool canSetImage)
        : base(
            width,
            height,
            pixelFormat,
            palette,
            zeroIsTransparent,
            canSetImage,
            new DummyTextureDataSource()
        ) {
        }

        public static InMemoryTextureData FromJToken(JToken token, bool zeroIsTransparent, Palette palette, bool canSetImage)
            => new InMemoryTextureData((JObject) token, zeroIsTransparent, palette, canSetImage);
        protected InMemoryTextureData(JObject jObject, bool zeroIsTransparent, Palette palette, bool canSetImage)
        : base(
            (int) jObject["Width"],
            (int) jObject["Height"],
            (TexturePixelFormat) Enum.Parse(typeof(TexturePixelFormat), (string) jObject["PixelFormat"]),
            palette,
            zeroIsTransparent,
            canSetImage,
            new DummyTextureDataSource()
        ) {
            var imageDataBase64 = (string) jObject["ImageData"];
            if (PixelFormat == TexturePixelFormat.ABGR1555)
                _ = _textureDataCache.SetImageData16Bit(Convert.FromBase64String(imageDataBase64).ToUShorts().To2DArrayColumnMajor(Width, Height));
            else
                _ = _textureDataCache.SetImageData8Bit(Convert.FromBase64String(imageDataBase64).To2DArrayColumnMajor(Width, Height));
        }

        public static InMemoryTextureData FromJToken(JToken token, int width, int height, TexturePixelFormat pixelFormat, bool zeroIsTransparent, Palette palette, bool canSetImage)
            => new InMemoryTextureData(token, width, height, pixelFormat, zeroIsTransparent, palette, canSetImage);
        protected InMemoryTextureData(JToken token, int width, int height, TexturePixelFormat pixelFormat, bool zeroIsTransparent, Palette palette, bool canSetImage)
        : base(
            width,
            height,
            pixelFormat,
            palette,
            zeroIsTransparent,
            canSetImage,
            new DummyTextureDataSource()
        ) {
            var imageDataBase64 = (string) token;
            if (PixelFormat == TexturePixelFormat.ABGR1555)
                _ = _textureDataCache.SetImageData16Bit(Convert.FromBase64String(imageDataBase64).ToUShorts().To2DArrayColumnMajor(Width, Height));
            else
                _ = _textureDataCache.SetImageData8Bit(Convert.FromBase64String(imageDataBase64).To2DArrayColumnMajor(Width, Height));
        }
    }
}
