using System;
using System.Collections.Generic;
using CommonLib.Imaging;
using CommonLib.Types;
using CommonLib.Utils;
using Newtonsoft.Json.Linq;

namespace CommonLib.Extensions {
    public static class ITextureDataExtensions {
        public static JObject ToJObject(this ITextureData texture, bool includePalette) {
            var imageData = (texture.PixelFormat == TexturePixelFormat.ABGR1555)
                ? texture.ImageData16Bit.To1DArrayTransposed().ToByteArray()
                : texture.ImageData8Bit.To1DArrayTransposed();

            var properties = new List<JProperty>() {
                new JProperty("PixelFormat", texture.PixelFormat.ToString()),
                new JProperty("Width",       texture.Width),
                new JProperty("Height",      texture.Height),
                new JProperty("ImageData",   Convert.ToBase64String(imageData)),
            };

            if (includePalette)
                properties.Add(new JProperty("Palette", texture.Palette?.ToJArray()));

            return new JObject(properties.ToArray());
        }

        public static JValue ToJValue(this ITextureData texture) {
            var imageData = (texture.PixelFormat == TexturePixelFormat.ABGR1555)
                ? texture.ImageData16Bit.To1DArrayTransposed().ToByteArray()
                : texture.ImageData8Bit.To1DArrayTransposed();

            return new JValue(Convert.ToBase64String(imageData));
        }
    }
}
