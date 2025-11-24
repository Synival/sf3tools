using System;
using System.Drawing;
using System.Drawing.Imaging;
using CommonLib.Imaging;

namespace CommonLib.Extensions {
    public static class ImageExtensions {
        public static Bitmap CreateIndexedBitmap(this Image image) {
            if (image.PixelFormat != PixelFormat.Indexed)
                throw new ArgumentException($"Bitmap pixel format ({image.PixelFormat}) should be 'Format8bppIndexed'");

            var bitmap = new Bitmap(image.Width, image.Height, PixelFormat.Format8bppIndexed);
            using (var graphics = Graphics.FromImage(bitmap)) {
                graphics.DrawImage(image, 0, 0);
                graphics.Flush();
            }
            bitmap.SetPalette(image.GetPalette());
            return bitmap;
        }

        public static Bitmap CreateARGB8888Bitmap(this Image image) {
            var bitmap = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
            using (var graphics = Graphics.FromImage(bitmap)) {
                graphics.DrawImage(image, 0, 0);
                graphics.Flush();
            }
            return bitmap;
        }

        public static byte[] GetBitmapDataBGRA8888(this Image image) {
            if (image is Bitmap bitmap)
                return BitmapExtensions.GetBitmapDataBGRA8888(bitmap);
            else {
                using (bitmap = image.CreateARGB8888Bitmap())
                    return BitmapExtensions.GetBitmapDataBGRA8888(bitmap);
            }
        }

        public static ushort[] GetDataABGR1555(this Image image) {
            if (image is Bitmap bitmap)
                return BitmapExtensions.GetDataABGR1555(bitmap);
            else {
                using (bitmap = image.CreateARGB8888Bitmap())
                    return BitmapExtensions.GetDataABGR1555(bitmap);
            }
        }

        public static ushort[,] Get2DDataABGR1555(this Image image) {
            if (image is Bitmap bitmap)
                return BitmapExtensions.Get2DDataABGR1555(bitmap);
            else {
                using (bitmap = image.CreateARGB8888Bitmap())
                    return BitmapExtensions.Get2DDataABGR1555(bitmap);
            }
        }

        public static void SetPalette(this Image bitmap, Palette palette) {
            var outputPalette = bitmap.Palette;
            var palLen = Math.Min(256, palette.Channels.Length);

            for (int i = 0; i < palLen; ++i) {
                var inputColor = palette[i];
                outputPalette.Entries[i] = Color.FromArgb(inputColor.r, inputColor.g, inputColor.b);
            }
            bitmap.Palette = outputPalette;
        }

        public static Palette GetPalette(this Image bitmap) {
            var inputPalette = bitmap.Palette;
            var outputColors = new ushort[inputPalette.Entries.Length];

            for (int i = 0; i < outputColors.Length; ++i) {
                var inputColor = inputPalette.Entries[i];
                outputColors[i] = new PixelChannels { r = inputColor.R, g = inputColor.G, b = inputColor.B, a = inputColor.A }.ToABGR1555();
            }

            return new Palette(outputColors);
        }
    }
}
