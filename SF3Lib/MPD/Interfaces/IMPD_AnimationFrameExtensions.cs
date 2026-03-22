using System;
using CommonLib.Extensions;
using CommonLib.Types;
using CommonLib.Utils;
using Newtonsoft.Json.Linq;
using SF3.Imaging;

namespace SF3.MPD.Interfaces {
    public static class IMPD_AnimationFrameExtensions {
        public static JObject ToJObject(this IMPD_AnimationFrame frame) {
            var imageData = (frame.PixelFormat == TexturePixelFormat.ABGR1555)
                ? frame.ImageData16Bit.To1DArrayTransposed().ToBytes()
                : frame.ImageData8Bit.To1DArrayTransposed();

            return new JObject {
                { "ImageData", Convert.ToBase64String(imageData) },
                { "Duration",  frame.Duration },
            };
        }
    }
}
