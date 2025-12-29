using System;
using System.Linq;
using CommonLib.Attributes;
using CommonLib.Imaging;
using CommonLib.Utils;
using SF3.ByteData;
using SF3.Images;
using SF3.Types;

namespace SF3.Models.Structs.KAO {
    public class FaceCompositeImage : Struct, ITextureData {
        public FaceCompositeImage(IByteData data, int id, int layer, int index, string name, FaceChunk chunk)
        : base(data, id, name, 0 /* not applicable */, 0 /* not applicable */) {
            Layer = layer;
            Index = index;
            Chunk = chunk;
        }

        [TableViewModelColumn(displayOrder: -2.9f, displayGroup: "Metadata")]
        public int Layer { get; }

        [TableViewModelColumn(displayOrder: -2.8f, displayGroup: "Metadata")]
        public int Index { get; }

        public FaceChunk Chunk { get; }
        public FaceHeader Header => Chunk.Header;

        public void SetImageData8Bit(byte[,] data, Palette palette) {
            // TODO: Fancy composite setting thing
            Invalidated?.Invoke(this, EventArgs.Empty);
        }

        public byte[] GetBitmapDataARGB1555(bool highlightEndcodes = false)
            => BitmapUtils.ConvertIndexedDataToARGB1555BitmapData(ImageData8Bit, Palette, true);

        public byte[] GetBitmapDataARGB8888(bool highlightEndcodes = false)
            => BitmapUtils.ConvertIndexedDataToARGB8888BitmapData(ImageData8Bit, Palette, true);

        // TODO: Implement!
        public string Validate8BitImageData(byte[,] data, Palette palette, int oldStoredSize, int newStoredSize)
            => "Not implemented";

        // TODO: Implement!
        public string Validate16BitImageData(ushort[,] data, int oldStoredSize, int newStoredSize)
            => "Not implemented";

        private byte[,] GetCompositeImageData() {
            var baseImage = Chunk.ImageTable[0];

            var baseImageData = baseImage.ImageData8Bit;
            var baseWidth  = baseImageData.GetLength(0);
            var baseHeight = baseImageData.GetLength(1);

            var compositeImageData = baseImageData.Clone() as byte[,];

            var addImage = Chunk.ImageTable.FirstOrDefault(x => x.Layer == Layer && x.Index == Index);
            var dataToAdd = addImage?.ImageData8Bit;
            if (dataToAdd != null) {
                var addWidth  = dataToAdd.GetLength(0);
                var addHeight = dataToAdd.GetLength(1);

                var offsetX = baseWidth / 2  - addWidth / 2  + addImage.X;
                var offsetY = baseHeight / 2 - addHeight / 2 + addImage.Y;

                var toY = offsetY;
                for (int fromY = 0; fromY < addHeight; fromY++, toY++) {
                    var toX = offsetX;
                    for (int fromX = 0; fromX < addWidth; fromX++, toX++) {
                        if (dataToAdd[fromX, fromY] != 0)
                            compositeImageData[toX, toY] = dataToAdd[fromX, fromY];
                    }
                }
            }

            return compositeImageData;
        }

        public int BytesPerPixel => 1;
        public TexturePixelFormat PixelFormat => TexturePixelFormat.Palette1;
        public int Width => Header.Width;
        public int Height => Header.Height;

        public byte[,] ImageData8Bit => GetCompositeImageData();

        public ushort[,] ImageData16Bit {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public byte[] BitmapDataARGB1555 => GetBitmapDataARGB1555(highlightEndcodes: false);
        public byte[] BitmapDataARGB8888 => GetBitmapDataARGB8888(highlightEndcodes: false);
        public string Hash => "Not Implemented";
        public Palette Palette => Chunk.Palette;
        public bool CanSetImageData8Bit => true;
        public bool CanSetImageData16Bit => false;

        public event EventHandler Invalidated;
    }
}
