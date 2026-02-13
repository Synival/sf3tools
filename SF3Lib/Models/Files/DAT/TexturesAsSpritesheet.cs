using System;
using CommonLib.Extensions;
using CommonLib.Imaging;
using CommonLib.Types;
using CommonLib.Utils;

namespace SF3.Models.Files.DAT {
    public class TexturesAsSpritesheet : ITextureData {
        public TexturesAsSpritesheet(IDAT_File file, Palette palette, bool zeroIsTransparent, int widthPerImage, int heightPerImage, int imagesPerRow)
            : this(file, TexturePixelFormat.Indexed8Bit, palette, zeroIsTransparent, widthPerImage, heightPerImage, imagesPerRow) {}

        public TexturesAsSpritesheet(IDAT_File file, bool zeroIsTransparent, int widthPerImage, int heightPerImage, int imagesPerRow)
            : this(file, TexturePixelFormat.ABGR1555, null, zeroIsTransparent, widthPerImage, heightPerImage, imagesPerRow) {}

        private TexturesAsSpritesheet(IDAT_File file, TexturePixelFormat pixelFormat, Palette palette, bool zeroIsTransparent, int widthPerImage, int heightPerImage, int imagesPerRow) {
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
            _textureDataBuffer.Invalidate();
            Invalidated?.Invoke(this, EventArgs.Empty);
        }

        public IDAT_File DAT_File { get; }
        public int WidthPerImage { get; }
        public int HeightPerImage { get; }
        public int ImagesPerRow { get; }
        public TexturePixelFormat PixelFormat { get; }
        public Palette Palette { get; }
        public int Width { get; }
        public int Height { get; private set; }
        public int ImageDataSize => Width * Height * BytesPerPixel;
        public int BytesPerPixel => PixelFormat.BytesPerPixel();
        public bool ZeroIsTransparent { get; }

        public byte[,] ImageData8Bit => _textureDataBuffer.GetOrCacheImageData8Bit(() => Create8BitImageData());

        // TODO: make settable!
        public void SetImageData8Bit(byte[,] data, Palette palette) {}

        public ushort[,] ImageData16Bit {
            get => _textureDataBuffer.GetOrCacheImageData16Bit(() => Create16BitImageData());
            // TODO: make settable!
            set {}
        }

        public byte[] BitmapDataARGB1555 => GetBitmapDataARGB1555(false);
        public byte[] BitmapDataARGB8888 => GetBitmapDataARGB8888(false);

        public string Hash => _textureDataBuffer.GetOrCacheHash(() => BitmapDataARGB1555.CreateTextureHash());

        // TODO: make settable!
        public bool CanSetImageData8Bit => false;
        // TODO: make settable!
        public bool CanSetImageData16Bit => false;

        public byte[] GetBitmapDataARGB1555(bool highlightEndcodes = false)
            => _textureDataBuffer.GetOrCacheBitmapDataARGB1555(() => BitmapUtils.ConvertABGR1555DataToARGB1555BitmapData(ImageData16Bit));
        public byte[] GetBitmapDataARGB8888(bool highlightEndcodes = false)
            => _textureDataBuffer.GetOrCacheBitmapDataARGB8888(() => BitmapUtils.ConvertIndexedDataToARGB8888BitmapData(ImageData8Bit, Palette, ZeroIsTransparent));

        // TODO: support!
        public string Validate8BitImageData(byte[,] data, Palette palette, int oldStoredSize, int newStoredSize) => "Not supported";
        // TODO: support!
        public string Validate16BitImageData(ushort[,] data, int oldStoredSize, int newStoredSize) => "Not supported";

        private byte[,] Create8BitImageData() {
            var data = new byte[Width, Height];

            int count = 0;
            foreach (var row in DAT_File.TextureTable) {
                var imageX = (count % ImagesPerRow) * WidthPerImage;
                var imageY = (count / ImagesPerRow) * HeightPerImage;

                var imageData = row.ImageData8Bit;
                if (imageData != null) {
                    var imageWidth  = row.Width;
                    var imageHeight = row.Height;

                    for (int y = 0; y < imageHeight && y < HeightPerImage; y++)
                        for (int x = 0; x < imageWidth && x < WidthPerImage; x++)
                            data[x + imageX, y + imageY] = imageData[x, y];
                }

                count++;
            }

            return data;
        }

        private ushort[,] Create16BitImageData() {
            var data = new ushort[Width, Height];

            int count = 0;
            foreach (var row in DAT_File.TextureTable) {
                var imageX = (count % ImagesPerRow) * WidthPerImage;
                var imageY = (count / ImagesPerRow) * HeightPerImage;

                var imageData = row.PixelFormat == TexturePixelFormat.ABGR1555
                    ? row.ImageData16Bit
                    : (row.ImageData8Bit?.To1DArray()?.ConvertIndexedToABGR1555(row.Palette)?.To2DArray(row.Width, row.Height));

                if (imageData != null) {
                    var imageWidth  = row.Width;
                    var imageHeight = row.Height;

                    for (int y = 0; y < imageHeight && y < HeightPerImage; y++)
                        for (int x = 0; x < imageWidth && x < WidthPerImage; x++)
                            data[x + imageX, y + imageY] = imageData[x, y];
                }

                count++;
            }

            return data;
        }

        private void UpdateHeight()
            => Height = (int) (Math.Ceiling(DAT_File.TextureTable.Length / (float) ImagesPerRow) * HeightPerImage);

        public event EventHandler Invalidated;

        private TextureDataBuffer _textureDataBuffer = new TextureDataBuffer();
    }
}
