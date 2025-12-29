using System;
using System.Linq;
using CommonLib.Arrays;
using CommonLib.Extensions;
using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Images;
using SF3.Types;

namespace SF3.Models.Structs.KAO {
    public class FaceCompositeImage : Struct, ITextureData {
        public FaceCompositeImage(IByteData data, int id, int layer, int index, string name, FaceChunk chunk)
        : base(data, id, name, 0 /* not applicable */, chunk.Header.Width * chunk.Header.Height) {
            Layer = layer;
            Index = index;
            Chunk = chunk;

            UpdateCompositeImageData();
            _textureData = new TextureData(
                _compositeImageData, 0, chunk.Header.Width, chunk.Header.Height,
                TexturePixelFormat.Palette1, chunk.Palette, isCompressed: false, zeroIsTransparent: true, canSetImage: true
            );

            _textureData.Invalidated += (s, e) => {
                UpdateCompositeImageData();
                this.Invalidated?.Invoke(s, e);
            };
        }

        public int Layer { get; }
        public int Index { get; }
        public FaceChunk Chunk { get; }
        public FaceHeader Header => Chunk.Header;

        public void SetImageData8Bit(byte[,] data, Palette palette) {
            // TODO: Fancy composite setting thing
        }

        public byte[] GetBitmapDataARGB1555(bool highlightEndcodes = false) => _textureData.GetBitmapDataARGB1555(highlightEndcodes);
        public byte[] GetBitmapDataARGB8888(bool highlightEndcodes = false) => _textureData.GetBitmapDataARGB8888(highlightEndcodes);
        public string Validate8BitImageData(byte[,] data, Palette palette, int oldStoredSize, int newStoredSize) => Validate8BitImageData(data, palette, oldStoredSize, newStoredSize);
        public string Validate16BitImageData(ushort[,] data, int oldStoredSize, int newStoredSize) => Validate16BitImageData(data, oldStoredSize, newStoredSize);

        private void UpdateCompositeImageData() {
            if (_compositeImageData == null)
                _compositeImageData = new ByteArray(Header.Width * Header.Height);

            var baseImage = Chunk.ImageTable[0];

            var baseImageData = baseImage.ImageData8Bit;
            var baseWidth  = baseImageData.GetLength(0);
            var baseHeight = baseImageData.GetLength(1);

            _compositeImageData.SetDataTo(baseImageData.To1DArrayTransposed());

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
                    var dataAddr = baseWidth * toY + toX;

                    for (int fromX = 0; fromX < addWidth; fromX++, toX++, dataAddr++) {
                        if (dataToAdd[fromX, fromY] != 0)
                            _compositeImageData[dataAddr] = dataToAdd[fromX, fromY];
                    }
                }
            }
        }

        public int BytesPerPixel => 1;
        public TexturePixelFormat PixelFormat => TexturePixelFormat.Palette1;
        public int Width => Header.Width;
        public int Height => Header.Height;
        public byte[,] ImageData8Bit => _textureData.ImageData8Bit;

        public ushort[,] ImageData16Bit {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public byte[] BitmapDataARGB1555 => _textureData.BitmapDataARGB1555;
        public byte[] BitmapDataARGB8888 => _textureData.BitmapDataARGB8888;
        public string Hash => _textureData.Hash;
        public Palette Palette => Chunk.Palette;
        public bool CanSetImageData8Bit => true;
        public bool CanSetImageData16Bit => false;

        private ByteArray _compositeImageData;
        private TextureData _textureData;

        public event EventHandler Invalidated;
    }
}
