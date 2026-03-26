using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using CommonLib.Imaging;
using CommonLib.Utils;

namespace CommonLib.Extensions {
    public static class ImageExtensions {
        public static Bitmap CreateIndexedBitmap(this Image image) {
            if (image.PixelFormat != PixelFormat.Format8bppIndexed)
                throw new ArgumentException($"Bitmap pixel format ({image.PixelFormat}) should be 'Format8bppIndexed'");

            // For now, only bitmaps are supported, which is incredibly stupid because we're just duplicating a bitmap here...
            // ...but I can't find any reasonable way to get the 8-bit image data from anything other than a Bitmap.
            var imageAsBitmap = image as Bitmap;
            if (imageAsBitmap == null)
                throw new ArgumentException("Only bitmaps are supported");

            var bitmap = new Bitmap(image.Width, image.Height, PixelFormat.Format8bppIndexed);
            bitmap.SetPalette(image.GetPalette());

            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
            Marshal.Copy(imageAsBitmap.GetBitmapDataIndexed(), 0, bitmapData.Scan0, bitmap.Width * bitmap.Height);
            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }

        public static Bitmap CreateARGB8888Bitmap(this Image image, bool zeroIsTransparent = false) {
            var bitmap = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);

            // Try to get raw data if possible.
            byte[] newData = null;
            if (image is Bitmap imageAsBitmap) {
                switch (imageAsBitmap.PixelFormat) {
                    case PixelFormat.Format8bppIndexed:
                        newData = BitmapUtils.ConvertIndexedDataToARGB8888BitmapData(imageAsBitmap.GetBitmapDataIndexed(), imageAsBitmap.GetPalette(), zeroIsTransparent);
                        break;

                    case PixelFormat.Format32bppArgb:
                        newData = imageAsBitmap.GetBitmapDataARGB8888(zeroIsTransparent);
                        break;
                }
            }

            // We found data -- copy it in.
            if (newData != null) {
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
                Marshal.Copy(newData, 0, bitmapData.Scan0, bitmap.Width * bitmap.Height * 4);
                bitmap.UnlockBits(bitmapData);
            }
            // As a fallback, use Graphics.DrawImage().
            else {
                using (var graphics = Graphics.FromImage(bitmap)) {
                    graphics.DrawImage(image, 0, 0);
                    graphics.Flush();
                }
            }

            return bitmap;
        }

        public static byte[] GetBitmapDataIndexed(this Image image)
            => Get1DDataIndexed(image);

        public static byte[] GetBitmapDataARGB8888(this Image image, bool zeroIsTransparent = false) {
            if (image is Bitmap bitmap)
                return BitmapExtensions.GetBitmapDataARGB8888(bitmap, zeroIsTransparent);
            else {
                using (bitmap = image.CreateARGB8888Bitmap())
                    return BitmapExtensions.GetBitmapDataARGB8888(bitmap, zeroIsTransparent);
            }
        }

        public static byte[] Get1DDataIndexed(this Image image) {
            if (image is Bitmap bitmap)
                return BitmapExtensions.Get1DDataIndexed(bitmap);
            else {
                using (bitmap = image.CreateIndexedBitmap())
                    return BitmapExtensions.Get1DDataIndexed(bitmap);
            }
        }

        public static byte[,] Get2DDataIndexed(this Image image) {
            if (image is Bitmap bitmap)
                return BitmapExtensions.Get2DDataIndexed(bitmap);
            else {
                using (bitmap = image.CreateIndexedBitmap())
                    return BitmapExtensions.Get2DDataIndexed(bitmap);
            }
        }

        public static ushort[] Get1DDataABGR1555(this Image image, bool zeroIsTransparent = false) {
            if (image is Bitmap bitmap)
                return BitmapExtensions.Get1DDataABGR1555(bitmap, zeroIsTransparent);
            else {
                using (bitmap = image.CreateARGB8888Bitmap(zeroIsTransparent))
                    return BitmapExtensions.Get1DDataABGR1555(bitmap, zeroIsTransparent);
            }
        }

        public static ushort[,] Get2DDataABGR1555(this Image image, bool zeroIsTransparent = false) {
            if (image is Bitmap bitmap)
                return BitmapExtensions.Get2DDataABGR1555(bitmap, zeroIsTransparent);
            else {
                using (bitmap = image.CreateARGB8888Bitmap(zeroIsTransparent))
                    return BitmapExtensions.Get2DDataABGR1555(bitmap, zeroIsTransparent);
            }
        }

        public static void SetPalette(this Image image, IPalette palette) {
            var outputPalette = image.Palette;
            var palLen = Math.Min(256, palette.Colors.Length);

            for (int i = 0; i < palLen; ++i) {
                var inputColor = palette[i];
                outputPalette.Entries[i] = Color.FromArgb(inputColor.R, inputColor.G, inputColor.B);
            }
            image.Palette = outputPalette;
        }

        public static Palette GetPalette(this Image image) {
            var inputPalette = image.Palette;
            var outputColors = new ushort[inputPalette.Entries.Length];

            for (int i = 0; i < outputColors.Length; ++i) {
                var inputColor = inputPalette.Entries[i];
                outputColors[i] = new PixelChannels { R = inputColor.R, G = inputColor.G, B = inputColor.B, A = inputColor.A }.ToABGR1555();
            }

            return new Palette(outputColors);
        }
    }
}
