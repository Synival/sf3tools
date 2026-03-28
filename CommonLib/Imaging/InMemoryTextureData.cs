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
        public InMemoryTextureData(ITextureData original, IPalette palette, ImageDataCanSet canSet, IndexedColorUpdateStrategy indexedUpdateStrategy)
        : base(
            original.Width,
            original.Height,
            original.PixelFormat,
            palette,
            original.ZeroIsTransparent,
            canSet,
            new DummyTextureDataSource(),
            indexedUpdateStrategy
        ) {
            if (PixelFormat == TexturePixelFormat.Indexed8Bit)
                _ = _textureDataCache.SetImageData8Bit(original.ImageData8Bit);
            if (PixelFormat == TexturePixelFormat.ABGR1555)
                _ = _textureDataCache.SetImageData16Bit(original.ImageData16Bit);
        }

        public InMemoryTextureData(byte[,] data, IPalette palette, bool zeroIsTransparent, ImageDataCanSet canSetImage, IndexedColorUpdateStrategy indexedUpdateStrategy)
        : base(
            data?.GetLength(0) ?? 0,
            data?.GetLength(1) ?? 0,
            TexturePixelFormat.Indexed8Bit,
            palette,
            zeroIsTransparent,
            canSetImage,
            new DummyTextureDataSource(),
            indexedUpdateStrategy
        ) {
            if (data != null)
                _ = _textureDataCache.SetImageData8Bit(data);
        }

        public InMemoryTextureData(ushort[,] data, ImageDataCanSet canSetImage, IndexedColorUpdateStrategy indexedUpdateStrategy)
        : base(
            data?.GetLength(0) ?? 0,
            data?.GetLength(1) ?? 0,
            TexturePixelFormat.ABGR1555,
            palette: null,
            zeroIsTransparent: false,
            canSetImage,
            new DummyTextureDataSource(),
            indexedUpdateStrategy
        ) {
            if (data != null)
                _ = _textureDataCache.SetImageData16Bit(data);
        }

        public InMemoryTextureData(int width, int height, TexturePixelFormat pixelFormat, IPalette palette, bool zeroIsTransparent, ImageDataCanSet canSetImage, IndexedColorUpdateStrategy indexedUpdateStrategy)
        : base(
            width,
            height,
            pixelFormat,
            palette,
            zeroIsTransparent,
            canSetImage,
            new DummyTextureDataSource(),
            indexedUpdateStrategy
        ) {
        }

        public static InMemoryTextureData FromJToken(JToken token, bool zeroIsTransparent, IPalette palette, ImageDataCanSet canSetImage, IndexedColorUpdateStrategy paletteUpdateStrategy)
            => new InMemoryTextureData((JObject) token, zeroIsTransparent, palette, canSetImage, paletteUpdateStrategy);
        protected InMemoryTextureData(JObject jObject, bool zeroIsTransparent, IPalette palette, ImageDataCanSet canSetImage, IndexedColorUpdateStrategy indexedUpdateStrategy)
        : base(
            (int) jObject["Width"],
            (int) jObject["Height"],
            (TexturePixelFormat) Enum.Parse(typeof(TexturePixelFormat), (string) jObject["PixelFormat"]),
            palette,
            zeroIsTransparent,
            canSetImage,
            new DummyTextureDataSource(),
            indexedUpdateStrategy
        ) {
            var imageDataBase64 = (string) jObject["ImageData"];
            if (PixelFormat == TexturePixelFormat.ABGR1555)
                _ = _textureDataCache.SetImageData16Bit(Convert.FromBase64String(imageDataBase64).ToUShorts().To2DArrayColumnMajor(Width, Height));
            else
                _ = _textureDataCache.SetImageData8Bit(Convert.FromBase64String(imageDataBase64).To2DArrayColumnMajor(Width, Height));
        }

        public static InMemoryTextureData FromJToken(JToken token, int width, int height, TexturePixelFormat pixelFormat, bool zeroIsTransparent, IPalette palette, ImageDataCanSet canSetImage, IndexedColorUpdateStrategy paletteUpdateStrategy)
            => new InMemoryTextureData(token, width, height, pixelFormat, zeroIsTransparent, palette, canSetImage, paletteUpdateStrategy);
        protected InMemoryTextureData(JToken token, int width, int height, TexturePixelFormat pixelFormat, bool zeroIsTransparent, IPalette palette, ImageDataCanSet canSetImage, IndexedColorUpdateStrategy indexedUpdateStrategy)
        : base(
            width,
            height,
            pixelFormat,
            palette,
            zeroIsTransparent,
            canSetImage,
            new DummyTextureDataSource(),
            indexedUpdateStrategy
        ) {
            var imageDataBase64 = (string) token;
            if (PixelFormat == TexturePixelFormat.ABGR1555)
                _ = _textureDataCache.SetImageData16Bit(Convert.FromBase64String(imageDataBase64).ToUShorts().To2DArrayColumnMajor(Width, Height));
            else
                _ = _textureDataCache.SetImageData8Bit(Convert.FromBase64String(imageDataBase64).To2DArrayColumnMajor(Width, Height));
        }
    }
}
