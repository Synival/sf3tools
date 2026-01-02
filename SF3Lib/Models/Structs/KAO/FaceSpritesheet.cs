using System;
using System.Security.Cryptography;
using CommonLib.Imaging;
using CommonLib.Utils;
using SF3.Images;
using SF3.Types;

namespace SF3.Models.Structs.KAO {
    public class FaceSpritesheet : ITextureData {
        public FaceSpritesheet(FaceChunk face) {
            Face = face;

            // Invalidate this image if ANY data has changed.
            face.Data.Data.RangeModified += (s, e) => Invalidate();
        }

        public void Invalidate() {
            _textureDataBuffer.Invalidate();
            Invalidated?.Invoke(this, EventArgs.Empty);
        }

        public FaceChunk Face { get; }
        public FaceHeader Header => Face.Header;
        public Palette Palette => Face.Palette;

        public int BytesPerPixel => 1;
        public TexturePixelFormat PixelFormat => TexturePixelFormat.Palette1;
        public int Width => Header.Width * 6;
        public int Height => Header.Height * 2;

        public byte[,] ImageData8Bit {
            get {
                if (_textureDataBuffer.ImageData8Bit == null)
                    _textureDataBuffer.ImageData8Bit = GetImageData();
                return _textureDataBuffer.ImageData8Bit;
            }
        }

        public void SetImageData8Bit(byte[,] data, Palette palette) {
            // TODO: validate incoming image
            // TODO: build decomposited frames
            // TODO: rebuild entire data structure (this one's a doozy!)
        }

        public ushort[,] ImageData16Bit {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public byte[] BitmapDataARGB1555 => GetBitmapDataARGB1555(false);
        public byte[] BitmapDataARGB8888 => GetBitmapDataARGB8888(false);

        public string Hash {
            get {
                if (_textureDataBuffer.Hash == null && BitmapDataARGB1555 != null) {
                    using (var md5 = MD5.Create())
                        _textureDataBuffer.Hash = BitConverter.ToString(md5.ComputeHash(BitmapDataARGB1555)).Replace("-", "").ToLower();
                }
                return _textureDataBuffer.Hash;
            }
        }

        public bool ZeroIsTransparent => true;

        // TODO: allow setting 8-bit data!
        public bool CanSetImageData8Bit => false;
        public bool CanSetImageData16Bit => false;

        public byte[] GetBitmapDataARGB1555(bool highlightEndcodes = false) {
            if (_textureDataBuffer.BitmapDataARGB1555 == null)
                _textureDataBuffer.BitmapDataARGB1555 = BitmapUtils.ConvertIndexedDataToARGB1555BitmapData(ImageData8Bit, Palette, ZeroIsTransparent);
            return _textureDataBuffer.BitmapDataARGB1555;
        }

        public byte[] GetBitmapDataARGB8888(bool highlightEndcodes = false) {
            if (_textureDataBuffer.BitmapDataARGB8888 == null)
                _textureDataBuffer.BitmapDataARGB8888 = BitmapUtils.ConvertIndexedDataToARGB1555BitmapData(ImageData8Bit, Palette, ZeroIsTransparent);
            return _textureDataBuffer.BitmapDataARGB8888;
        }

        public string Validate8BitImageData(byte[,] data, Palette palette, int oldStoredSize, int newStoredSize) {
            // TODO: proper validation!
            return null;
        }

        public string Validate16BitImageData(ushort[,] data, int oldStoredSize, int newStoredSize) => "Not supported";

        private byte[,] GetImageData() {
            int width  = Header.Width;
            int height = Header.Height;

            // 2D array of 2D arrays: [imageX, imageY][pixelX, pixelY]
            var frameData = new byte[6, 2][,];
            var baseImage = Face.ImageTable[0].ImageData8Bit;

            var palette = Face.Palette;
            var lightestColor = (byte) palette.GetLightestIndex(zeroIsTransparent: true);

            // Build blink frames (top 3 images).
            void BuildImageRow(int imageStartIndex, int imageStopIndex, int columnStart, int row) {
                int count = imageStopIndex - imageStartIndex;

                for (int i = 0; i < count; ++i) {
                    var image = Face.ImageTable[i + imageStartIndex];
                    var col = columnStart + i;

                    // For "substitute" frame references, make an image where the number of pixels represents the reference value.
                    if (image.SubstituteFrameRef.HasValue) {
                        frameData[col, row] = new byte[width, height];
                        for (int x = 0; x < (image.SubstituteFrameRef ?? 0); x++)
                            frameData[col, row][x, 0] = lightestColor;
                        continue;
                    }

                    // Get the actual image used here. If there truly is none, do nothing.
                    var actualImage = image.ActualImage;
                    if (actualImage == null)
                        continue;

                    frameData[col, row] = ImageUtils.Create8BitImageDataWithoutTransparency(baseImage.Clone() as byte[,], palette);
                    FaceCompositeImage.AddFaceImageToData(frameData[col, row], actualImage);
                }
            }

            // Add base image.
            frameData[0, 0] = baseImage.Clone() as byte[,];

            // Build blinking and talking images.
            BuildImageRow(1,  4, 3, 0);
            BuildImageRow(4, 10, 0, 1);

            // Copy images into one big image.
            var completeData = new byte[width * 6, height * 2];
            for (int y = 0; y < 2; y++) {
                for (int x = 0; x < 6; x++) {
                    var data = frameData[x, y];
                    if (data == null)
                        continue;
                    for (int subY = 0; subY < height; subY++)
                        for (int subX = 0; subX < width; subX++)
                            completeData[x * width + subX, y * height + subY] = data[subX, subY];
                }
            }

            return completeData;
        }

        public event EventHandler Invalidated;

        private TextureDataBuffer _textureDataBuffer = new TextureDataBuffer();
    }
}
