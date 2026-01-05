using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Attributes;
using CommonLib.Extensions;
using CommonLib.Imaging;
using CommonLib.Utils;
using SF3.ByteData;
using SF3.Imaging;
using SF3.Types;

namespace SF3.Models.Structs.KAO {
    public class FaceCompositeImage : Struct, ITextureData {
        public FaceCompositeImage(IByteData data, int id, int layer, int index, FaceChunk chunk)
        : base(data, id, "", 0 /* not applicable */, 0 /* not applicable */) {
            Layer = layer;
            Index = index;
            Chunk = chunk;
            LayerImage = Chunk.ImageTable.First(x => x.Layer == layer && x.Index == index);
            Name = LayerImage.Name;

            // Force an update if the data was ever modified.
            // TODO: This is a bit aggressive... maybe only update on relevant data changes?
            chunk.Data.Data.RangeModified += (s, e) => Invalidate();
        }

        [TableViewModelColumn(displayOrder: -2.9f, displayGroup: "Metadata")]
        public int Layer { get; }

        [TableViewModelColumn(displayOrder: -2.8f, displayGroup: "Metadata")]
        public int Index { get; }

        [TableViewModelColumn(displayOrder: 0, displayGroup: "Metadata")]
        public bool HasImage => Header.GetLayerOffset(Layer, Index) > 0;

        [TableViewModelColumn(displayOrder: 1)]
        public int? FrameRef => LayerImage.FrameRef;

        [TableViewModelColumn(displayOrder: 2)]
        public int? SubstituteFrameRef => LayerImage.SubstituteFrameRef;

        [TableViewModelColumn(displayOrder: 3, minWidth: 225)]
        public string Hash => _textureDataBuffer.GetOrCacheHash(() => BitmapDataARGB1555.CreateTextureHash());

        public FaceChunk Chunk { get; }
        public FaceHeader Header => Chunk.Header;
        public FaceImage LayerImage { get; }

        public void Invalidate() {
            _textureDataBuffer.Invalidate();
            Invalidated?.Invoke(this, EventArgs.Empty);
        }

        public byte[] GetBitmapDataARGB1555(bool highlightEndcodes = false)
            => _textureDataBuffer.GetOrCacheBitmapDataARGB1555(() => BitmapUtils.ConvertIndexedDataToARGB1555BitmapData(ImageData8Bit, Palette, true));

        public byte[] GetBitmapDataARGB8888(bool highlightEndcodes = false)
            => _textureDataBuffer.GetOrCacheBitmapDataARGB8888(() => BitmapUtils.ConvertIndexedDataToARGB8888BitmapData(ImageData8Bit, Palette, true));

        public string Validate8BitImageData(byte[,] data, Palette palette, int oldStoredSize, int newStoredSize) {
            if (!HasImage)
                return "No image available";
            return TextureDataValidators.IsSameDimensions(data, Width, Height);
        }

        public string Validate16BitImageData(ushort[,] data, int oldStoredSize, int newStoredSize)
            => "Image must be in 8-bit indexed format";

        private byte[,] GetCompositeImageData() {
            var baseImage = Chunk.ImageTable[0];
            if (baseImage?.ImageData8Bit == null)
                return null;

            var addImage = LayerImage;
            if (addImage.ImageDataOffset <= 0 || addImage?.ImageData8Bit == null)
                return null;

            return CreateCompositeImageData(baseImage, new FaceImage[] { addImage });
        }

        public static byte[,] CreateCompositeImageData(FaceImage baseImage, IEnumerable<FaceImage> addImages) {
            var compositeImageData = baseImage.ImageData8Bit.Clone() as byte[,];
            foreach (var addImage in addImages)
                AddFaceImageToData(compositeImageData, addImage);
            return compositeImageData;
        }

        public static (int X, int Y) GetFaceImageOffset(int baseWidth, int baseHeight, FaceImage addImage) {
            var addData   = addImage?.ImageData8Bit;
            var addWidth  = addData.GetLength(0);
            var addHeight = addData.GetLength(1);

            var offsetX = baseWidth / 2  - addWidth / 2  + addImage.X;
            var offsetY = baseHeight / 2 - addHeight / 2 + addImage.Y;

            return (offsetX, offsetY);
       }

        public static void AddFaceImageToData(byte[,] data, FaceImage image) {
            if (image?.ImageData8Bit == null)
                return;

            var width  = data.GetLength(0);
            var height = data.GetLength(1);

            var addData   = image?.ImageData8Bit;
            var addWidth  = addData.GetLength(0);
            var addHeight = addData.GetLength(1);

            var (offsetX, offsetY) = GetFaceImageOffset(width, height, image);
            var toY = offsetY;
            for (int fromY = 0; fromY < addHeight; fromY++, toY++) {
                var toX = offsetX;
                for (int fromX = 0; fromX < addWidth; fromX++, toX++) {
                    if (addData[fromX, fromY] != 0 && toX >= 0 && toX < width && toY >= 0 && toX < height)
                        data[toX, toY] = addData[fromX, fromY];
                }
            }
        }

        public int BytesPerPixel => 1;
        public TexturePixelFormat PixelFormat => TexturePixelFormat.Indexed8Bit;
        public int Width => Header.Width;
        public int Height => Header.Height;
        public int ImageDataSize => Width * Height * BytesPerPixel;

        public byte[,] ImageData8Bit => _textureDataBuffer.GetOrCacheImageData8Bit(() => GetCompositeImageData());

        public void SetImageData8Bit(byte[,] data, Palette palette) {
            var error = Validate8BitImageData(data, palette, 0, 0);
            if (error != null)
                throw new ArgumentException(error);

            var baseImage = Chunk.ImageTable[0];
            var layerImage = LayerImage;
            if (baseImage?.ImageData8Bit == null || layerImage?.ImageData8Bit == null)
                throw new InvalidOperationException("Cannot set empty image");

            var baseWidth   = baseImage.Width;
            var baseHeight  = baseImage.Width;
            var layerWidth  = layerImage.Width;
            var layerHeight = layerImage.Height;

            var baseData = baseImage.ImageData8Bit;
            var newData  = new byte[layerWidth, layerHeight];

            // The input data cannot have any zeroes, which represent transparency. If they exist, they're probably
            // intended to be black. Find the closest color in the palette to Color[0] to use as a replacement.
            data = ImageUtils.Create8BitImageDataWithoutTransparency(data, Chunk.Palette);

            // Only set differences
            var (offsetX, offsetY) = GetFaceImageOffset(baseWidth, baseHeight, layerImage);
            var (compareX, compareY) = (0, offsetY);
            for (int toY = 0; toY < layerHeight; toY++, compareY++) {
                compareX = offsetX;
                for (int toX = 0; toX < layerWidth; toX++, compareX++) {
                    if (compareX >= 0 && compareX < baseWidth && compareY >= 0 && compareY < baseHeight) {
                        if (data[compareX, compareY] != baseData[compareX, compareY])
                            newData[toX, toY] = data[compareX, compareY];
                    }
                }
            }

            layerImage.SetImageData8Bit(newData, layerImage.Palette);
            Invalidate();
        }

        public ushort[,] ImageData16Bit {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public byte[] BitmapDataARGB1555 => GetBitmapDataARGB1555(highlightEndcodes: false);
        public byte[] BitmapDataARGB8888 => GetBitmapDataARGB8888(highlightEndcodes: false);

        public Palette Palette => Chunk.Palette;
        public bool CanSetImageData8Bit => HasImage;
        public bool CanSetImageData16Bit => false;
        public bool ZeroIsTransparent => true;

        public event EventHandler Invalidated;

        private TextureDataBuffer _textureDataBuffer = new TextureDataBuffer();
    }
}
