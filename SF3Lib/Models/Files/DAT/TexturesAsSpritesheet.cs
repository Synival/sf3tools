using System;
using System.Collections.Generic;
using System.Drawing;
using CommonLib.Extensions;
using CommonLib.Imaging;
using CommonLib.Types;
using CommonLib.Utils;

namespace SF3.Models.Files.DAT {
    public class TexturesAsSpritesheet : ITextureData {
        public TexturesAsSpritesheet(IDAT_File file, IPalette palette, bool zeroIsTransparent, int widthPerImage, int heightPerImage, int imagesPerRow)
            : this(file, TexturePixelFormat.Indexed8Bit, palette, zeroIsTransparent, widthPerImage, heightPerImage, imagesPerRow) {}

        public TexturesAsSpritesheet(IDAT_File file, bool zeroIsTransparent, int widthPerImage, int heightPerImage, int imagesPerRow)
            : this(file, TexturePixelFormat.ABGR1555, null, zeroIsTransparent, widthPerImage, heightPerImage, imagesPerRow) {}

        private TexturesAsSpritesheet(IDAT_File file, TexturePixelFormat pixelFormat, IPalette palette, bool zeroIsTransparent, int widthPerImage, int heightPerImage, int imagesPerRow) {
            DAT_File       = file;
            WidthPerImage  = widthPerImage;
            HeightPerImage = heightPerImage;
            ImagesPerRow   = imagesPerRow;

            PixelFormat    = pixelFormat;
            Palette        = palette;
            Width          = imagesPerRow * widthPerImage;
            ZeroIsTransparent = zeroIsTransparent;

            UpdateHeight();

            // Invalidate this image if ANY data has changed.
            file.Data.Data.RangeModified += (s, e) => Invalidate();
        }

        public void Invalidate() {
            _textureDataCache.Invalidate();
            Invalidated?.Invoke(this, EventArgs.Empty);
        }

        public IDAT_File DAT_File { get; }
        public int WidthPerImage { get; }
        public int HeightPerImage { get; }
        public int ImagesPerRow { get; }
        public TexturePixelFormat PixelFormat { get; }
        public IPalette Palette { get; }
        public int Width { get; }
        public int Height { get; private set; }
        public int ImageDataSize => Width * Height * BytesPerPixel;
        public int BytesPerPixel => PixelFormat.BytesPerPixel();
        public bool ZeroIsTransparent { get; }

        public byte[,] ImageData8Bit => _textureDataCache.GetOrCacheImageData8Bit(() => Create8BitImageData());

        public void SetImageData8Bit(byte[,] data, IPalette palette) {
            var error = Validate8BitImageData(data, palette, null, null);
            if (error != null)
                throw new ArgumentException(error);

            var images = ImportSpritesheet(data);
            DAT_File.ReplaceImages8Bit(images, palette, MinimalChangesWhenSetting);
        }

        public ushort[,] ImageData16Bit {
            get => _textureDataCache.GetOrCacheImageData16Bit(() => Create16BitImageData());
            set {
                var error = Validate16BitImageData(value, null, null);
                if (error != null)
                    throw new ArgumentException(error);

                var images = ImportSpritesheet(value);
                DAT_File.ReplaceImages16Bit(images, MinimalChangesWhenSetting);
            }
        }

        private T[][,] ImportSpritesheet<T>(T[,] data) where T : struct {
            int columns = data.GetLength(0) / WidthPerImage;
            int rows    = data.GetLength(1) / HeightPerImage;

            int imageIndex = 0;
            var imageList = new List<T[,]>();
            for (int y = 0; y < rows; y++) {
                for (int x = 0; x < columns; x++) {
                    if (ProcessImage(data, imageIndex, x * WidthPerImage, y * HeightPerImage, out var image)) {
                        imageIndex++;
                        imageList.Add(image);
                    }
                }
            }

            if (imageList.Count != DAT_File.TextureTable.Count)
                throw new ArgumentException($"Imported spritesheet has {imageList.Count} image(s), but it should have {DAT_File.TextureTable.Count}");

            return imageList.ToArray();
        }

        private bool ProcessImage<T>(T[,] allData, int imageIndex, int offsetX, int offsetY, out T[,] imageOut) where T : struct {
            var imageData = new T[WidthPerImage, HeightPerImage];
            for (int y = 0; y < HeightPerImage; y++)
                for (int x = 0; x < WidthPerImage; x++)
                    imageData[x, y] = allData[x + offsetX, y + offsetY];

            if (ImageIsNull(imageData)) {
                imageOut = null;
                return true;
            }
            else if (ImageIsOutOfBounds(imageData)) {
                imageOut = null;
                return false;
            }
            else {
                imageOut = imageData;
                return true;
            }
        }

        // Returns 'true' if all the pixels along the edge have the same color value.
        private bool ImageIsCoded<T>(T[,] data, out int width, out int height, out T color1, out T color2) {
            width  = data.GetLength(0);
            height = data.GetLength(1);
            color1 = data[0, 0];
            color2 = data[2, 1];

            // For anything less than 6x6, the encoding can't be distinguished.
            if (width < 6 || height < 6)
                return false;
            if (color1.Equals(color2))
                return false;

            for (int x = 0; x < width; x++)
                if (!data[x, 0].Equals(color1) || !data[x, height - 1].Equals(color1))
                    return false;

            for (int y = 1; y < height - 1; y++)
                if (!data[0, y].Equals(color1) || !data[width - 1, y].Equals(color1))
                    return false;

            return true;
        }

        // Returns 'true' if the image has a border and a big X through it.
        private bool ImageIsNull<T>(T[,] data) where T : struct {
            if (!ImageIsCoded(data, out var width, out var height, out var color1, out var color2))
                return false;

            for (int y = 1; y < height - 1; y++)
                for (int x = 1; x < width - 1; x++)
                    if (!data[x, y].Equals((x == y || (width - x - 1) == y) ? color1 : color2))
                        return false;

            return true;
        }

        // Returns 'true' if the image has a border with a cross-hatch pattern inside.
        private bool ImageIsOutOfBounds<T>(T[,] data) where T : struct {
            if (!ImageIsCoded(data, out var width, out var height, out var color1, out var color2))
                return false;

            for (int y = 1; y < height - 1; y++)
                for (int x = 1; x < width - 1; x++)
                    if (!data[x, y].Equals(((x + y) % 2 == 0) ? color1 : color2))
                        return false;

            return true;
        }

        public byte[] BitmapDataARGB1555 => GetBitmapDataARGB1555(false);
        public byte[] BitmapDataARGB8888 => GetBitmapDataARGB8888(false);

        public string Hash => _textureDataCache.GetOrCacheHash(() => BitmapDataARGB1555.CreateTextureHash());

        public bool CanSetImageData8Bit  => DAT_File.CanReplaceImages8Bit;
        public bool CanSetImageData16Bit => DAT_File.CanReplaceImages16Bit;

        public byte[] GetBitmapDataARGB1555(bool highlightEndcodes = false)
            => _textureDataCache.GetOrCacheBitmapDataARGB1555(() => (PixelFormat == TexturePixelFormat.Indexed8Bit)
                ? BitmapUtils.ConvertIndexedDataToARGB1555BitmapData(ImageData8Bit, Palette, ZeroIsTransparent)
                : BitmapUtils.ConvertABGR1555DataToARGB1555BitmapData(ImageData16Bit)
            );

        public byte[] GetBitmapDataARGB8888(bool highlightEndcodes = false)
            => _textureDataCache.GetOrCacheBitmapDataARGB8888(() => (PixelFormat == TexturePixelFormat.Indexed8Bit)
                ? BitmapUtils.ConvertIndexedDataToARGB8888BitmapData(ImageData8Bit, Palette, ZeroIsTransparent)
                : BitmapUtils.ConvertABGR1555DataToARGB8888BitmapData(ImageData16Bit)
            );

        public string Validate8BitImageData(byte[,] data, IPalette palette, int? oldStoredSize, int? newStoredSize) {
            if (data.GetLength(0) % WidthPerImage != 0)
                return $"Image width ({data.GetLength(0)}) must be a multiple of {WidthPerImage}";
            if (data.GetLength(1) % HeightPerImage != 0)
                return $"Image height ({data.GetLength(1)}) must be a multiple of {HeightPerImage}";

            return null;
        }

        public string Validate16BitImageData(ushort[,] data, int? oldStoredSize, int? newStoredSize) {
            if (data.GetLength(0) % WidthPerImage != 0)
                return $"Image width ({data.GetLength(0)}) must be a multiple of {WidthPerImage}";
            if (data.GetLength(1) % HeightPerImage != 0)
                return $"Image height ({data.GetLength(1)}) must be a multiple of {HeightPerImage}";

            return null;
        }

        private byte[,] Create8BitImageData() {
            var imageData = new byte[Width, Height];
            var widthPerImage  = WidthPerImage;
            var heightPerImage = HeightPerImage;

            int count = 0;
            int rowMax = (int) (Math.Ceiling((float) DAT_File.TextureTable.Count / ImagesPerRow) * ImagesPerRow);
            var backColor = ZeroIsTransparent ? (byte) 0 : (byte) Palette.GetClosestIndex(false, new PixelChannels() { R = 64, G = 64, B = 64 });
            var foreColor = (byte) Palette.GetClosestIndex(ZeroIsTransparent, new PixelChannels() { R = 128, G = 128, B = 128 });

            for (int i = 0; i < rowMax; i++) {
                var imageX = (count % ImagesPerRow) * widthPerImage;
                var imageY = (count / ImagesPerRow) * heightPerImage;

                var item = (i < DAT_File.TextureTable.Count) ? DAT_File.TextureTable[i] : null;
                var itemImageData = item?.ImageData8Bit;

                // Copy images that exist to the spritesheet.
                if (itemImageData != null) {
                    var imageWidth  = item.Width;
                    var imageHeight = item.Height;

                    for (int y = 0; y < imageHeight && y < heightPerImage; y++)
                        for (int x = 0; x < imageWidth && x < widthPerImage; x++)
                            imageData[x + imageX, y + imageY] = itemImageData[x, y];
                }
                // Otherwise, use special images to denote some meaning of why there isn't an image there.
                else {
                    var drawRect = new Rectangle(imageX, imageY, widthPerImage - 1, heightPerImage - 1);

                    // Mark non-existant images with a big X
                    if (item != null) {
                        if (!ZeroIsTransparent)
                            FillBox(imageData, backColor, drawRect);
                        DrawX(imageData, foreColor, drawRect);
                    }
                    // Mark images out of range with a cross-hatch pattern
                    else
                        DrawCrossHatch(imageData, foreColor, backColor, drawRect);

                    // Draw a box around these special squares.
                    DrawBox(imageData, foreColor, drawRect);
                }

                count++;
            }

            return imageData;
        }

        private ushort[,] Create16BitImageData() {
            var imageData = new ushort[Width, Height];
            var widthPerImage  = WidthPerImage;
            var heightPerImage = HeightPerImage;

            int count = 0;
            int rowMax = (int) (Math.Ceiling((float) DAT_File.TextureTable.Count / ImagesPerRow) * ImagesPerRow);
            var backColor = new PixelChannels() { R =  64, G =  64, B =  64, A = 255 }.ToABGR1555();
            var foreColor = new PixelChannels() { R = 128, G = 128, B = 128, A = 255 }.ToABGR1555();

            for (int i = 0; i < rowMax; i++) {
                var imageX = (count % ImagesPerRow) * widthPerImage;
                var imageY = (count / ImagesPerRow) * heightPerImage;

                var item = (i < DAT_File.TextureTable.Count) ? DAT_File.TextureTable[i] : null;
                var itemImageData = (item?.PixelFormat == TexturePixelFormat.ABGR1555)
                    ? item?.ImageData16Bit
                    : (item?.ImageData8Bit?.To1DArray()?.ConvertIndexedToABGR1555(item.Palette, ZeroIsTransparent)?.To2DArray(item.Width, item.Height));

                // Copy images that exist to the spritesheet.
                if (itemImageData != null) {
                    var imageWidth  = item.Width;
                    var imageHeight = item.Height;

                    for (int y = 0; y < imageHeight && y < heightPerImage; y++)
                        for (int x = 0; x < imageWidth && x < widthPerImage; x++)
                            imageData[x + imageX, y + imageY] = itemImageData[x, y];
                }
                // Otherwise, use special images to denote some meaning of why there isn't an image there.
                else {
                    var drawRect = new Rectangle(imageX, imageY, widthPerImage - 1, heightPerImage - 1);

                    // Mark non-existant images with a big X
                    if (item != null)
                        DrawX(imageData, foreColor, drawRect);
                    // Mark images out of range with a cross-hatch pattern
                    else
                        DrawCrossHatch(imageData, foreColor, backColor, drawRect);

                    // Draw a box around these special squares.
                    DrawBox(imageData, foreColor, drawRect);
                }

                count++;
            }

            return imageData;
        }

        private void FillBox<T>(T[,] imageData, T color, Rectangle rect) {
            for (int y = 0; y <= rect.Height; y++)
                for (int x = 0; x <= rect.Width; x++)
                    imageData[rect.X + x, rect.Y + y] = color;
        }

        private void DrawX<T>(T[,] imageData, T color, Rectangle rect) {
            DrawLine(imageData, color, rect.Left,  rect.Top, rect.Right, rect.Bottom);
            DrawLine(imageData, color, rect.Right, rect.Top, rect.Left,  rect.Bottom); 
        }

        private void DrawLine<T>(T[,] imageData, T color, int x1, int y1, int x2, int y2) {
            int sign = 1;
            if (x1 > x2) {
                (x1, x2) = (x2, x1);
                sign = -sign;
            }
            if (y1 > y2) {
                (y1, y2) = (y2, y1);
                sign = -sign;
            }
            var width  = x2 - x1;
            var height = y2 - y1;

            if (width > height) {
                var yStep = height / width * sign;
                var yTop = (sign == 1) ? y1 : y2;
                for (int x = 0; x <= width; x++)
                    imageData[x + x1, (int) Math.Round((float) x * yStep + yTop)] = color;
            }
            else {
                var xStep = width / height * sign;
                var xTop = (sign == 1) ? x1 : x2;
                for (int y = 0; y <= height; y++)
                    imageData[(int) Math.Round((float) y * xStep) + xTop, y + y1] = color;
            }
        }

        private void DrawCrossHatch<T>(T[,] imageData, T color1, T color2, Rectangle rect) {
            for (int y = 0; y <= rect.Height; y++)
                for (int x = 0; x <= rect.Width; x++)
                    imageData[rect.X + x, rect.Y + y] = ((x + y) % 2 == 0) ? color1 : color2;
        }

        private void DrawBox<T>(T[,] imageData, T color, Rectangle rect) {
            for (int y = 0; y <= rect.Height; y++) {
                imageData[rect.Left,  rect.Y + y] = color;
                imageData[rect.Right, rect.Y + y] = color;
            }
            for (int x = 0; x <= rect.Width; x++) {
                imageData[rect.X + x, rect.Top]    = color;
                imageData[rect.X + x, rect.Bottom] = color;
            }
        }

        private void UpdateHeight()
            => Height = (int) (Math.Ceiling(DAT_File.TextureTable.Count / (float) ImagesPerRow) * HeightPerImage);

        public bool MinimalChangesWhenSetting { get; set; } = false;

        private TextureDataCache _textureDataCache = new TextureDataCache();

        public event EventHandler Invalidated;
    }
}
