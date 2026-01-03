using System;
using System.Linq;
using CommonLib.Arrays;
using CommonLib.Extensions;
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

        public byte[,] ImageData8Bit => _textureDataBuffer.GetOrCacheImageData8Bit(() => GetImageData());

        private class DecomposedImageBoundary {
            public DecomposedImageBoundary(int x1, int y1, int x2, int y2) {
                X1 = x1;
                Y1 = y1;
                X2 = x2;
                Y2 = y2;
                Width = x2 - x1;
                Height = y2 - y1;
            }

            public readonly int X1, Y1, X2, Y2, Width, Height;
        }

        public void SetImageData8Bit(byte[,] sheetData, Palette palette) {
            var sheetWidth  = sheetData.GetLength(0);
            var sheetHeight = sheetData.GetLength(1);

            if (sheetWidth % 6 != 0)
                throw new ArgumentException($"Sheet width ({sheetWidth}) must be divisible by 6");
            if (sheetHeight % 2 != 0)
                throw new ArgumentException($"Sheet width ({sheetHeight}) must be divisible by 2");

            var width  = sheetWidth / 6;
            var height = sheetHeight / 2;

            // Remove transparency.
            sheetData = ImageUtils.Create8BitImageDataWithoutTransparency(sheetData, palette);

            // Get composite images.
            byte[,] GetCompositeImageData(int col, int row) {
                var sheetX = col * width;
                var sheetY = row * height;

                var imageData = new byte[width, height];
                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                        imageData[x, y] = sheetData[x + sheetX, y + sheetY];

                return imageData;
            }

            // Base image in the upper-left corner...
            var baseImageData = GetCompositeImageData(0, 0);

            // ...composite blink images in the upper-right...
            var blinkImageDatas = new byte[][,] {
                GetCompositeImageData(3, 0),
                GetCompositeImageData(4, 0),
                GetCompositeImageData(5, 0),
            };

            // ...and composite talk images along the bottom row.
            var talkImageDatas = new byte[][,] {
                GetCompositeImageData(0, 1),
                GetCompositeImageData(1, 1),
                GetCompositeImageData(2, 1),
                GetCompositeImageData(3, 1),
                GetCompositeImageData(4, 1),
                GetCompositeImageData(5, 1),
            };

            // Figure out which images are actually just references to others.
            int? GetFrameRef(byte[,] imageData) {
                var imageData1d = imageData.To1DArrayTransposed();

                int color1 = imageData1d[0];
                int color2 = -1;
                int color1Count = 1;

                // Encoding should be a string of color1 followed by a string of color2.
                // If a third color is present, or color1 appears after color2 does, it's invalid.
                for (int i = 1; i < imageData1d.Length; ++i) {
                    if (color2 == -1) {
                        if (imageData1d[i] == color1)
                            color1Count++;
                        else
                            color2 = imageData1d[i];
                    }
                    else if (imageData1d[i] != color2)
                        return null;
                }

                if (color1Count == imageData1d.Length)
                    return 0;
                else if (color1Count <= 9)
                    return color1Count;
                else
                    return null;
            }

            var blinkFrameRefs = blinkImageDatas.Select(x => GetFrameRef(x)).ToArray();
            var talkFrameRefs  = talkImageDatas.Select(x => GetFrameRef(x)).ToArray();

            // For images that aren't frame references, get the decomposited image data.
            void DecomposeImageData(byte[,] imageData) {
                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                        if (imageData[x, y] == baseImageData[x, y])
                            imageData[x, y] = 0;
            }

            for (int i = 0; i < 3; ++i)
                if (!blinkFrameRefs[i].HasValue)
                    DecomposeImageData(blinkImageDatas[i]);

            for (int i = 0; i < 6; ++i)
                if (!talkFrameRefs[i].HasValue)
                    DecomposeImageData(talkImageDatas[i]);

            // Get boundaries of all composite images.
            DecomposedImageBoundary GetDecomposedImageBoundaries(byte[,] imageData) {
                int x1 = -1, y1 = -1, x2 = -1, y2 = -1;
                for (int y = 0; y < height; y++) {
                    for (int x = 0; x < width; x++) {
                        if (imageData[x, y] != 0) {
                            if (x1 == -1) {
                                x1 = x2 = x;
                                y1 = y2 = y;
                            }
                            else {
                                x1 = Math.Min(x1, x);
                                y1 = Math.Min(y1, y);
                                x2 = Math.Max(x2, x);
                                y2 = Math.Max(y2, y);
                            }
                        }
                    }
                }

                return x1 == -1 ? null : new DecomposedImageBoundary(x1, y1, x2 + 1, y2 + 1);
            }

            var blinkImageBoundaries = blinkImageDatas.Select((x, i) => blinkFrameRefs[i].HasValue ? null : GetDecomposedImageBoundaries(x)).ToArray();
            var talkImageBoundaries  = talkImageDatas .Select((x, i) => talkFrameRefs[i].HasValue  ? null : GetDecomposedImageBoundaries(x)).ToArray();

            // Figure out the max boundaries for cropping.
            DecomposedImageBoundary GetLayerBoundary(DecomposedImageBoundary[] imageBoundaries) {
                int x1 = -1, y1 = -1, x2 = -1, y2 = -1;
                for (int i = 0; i < imageBoundaries.Length; i++) {
                    var ib = imageBoundaries[i];
                    if (ib == null)
                        continue;
                    else if (x1 == -1) {
                        x1 = ib.X1;
                        y1 = ib.Y1;
                        x2 = ib.X2;
                        y2 = ib.Y2;
                    }
                    else {
                        x1 = Math.Min(x1, ib.X1);
                        y1 = Math.Min(y1, ib.Y1);
                        x2 = Math.Max(x2, ib.X2);
                        y2 = Math.Max(y2, ib.Y2);
                    }
                }

                if (x1 == -1)
                    return null;

                // Don't allow odd widths.
                if ((x2 - x1) % 2 == 1) {
                    if (x2 == width)
                        x1--;
                    else
                        x2++;
                }

                // Don't allow odd heights.
                if ((y2 - y1) % 2 == 1) {
                    if (y2 == height)
                        y1--;
                    else
                        y2++;
                }

                return new DecomposedImageBoundary(x1, y1, x2, y2);
            }

            var blinkBoundaries = GetLayerBoundary(blinkImageBoundaries);
            var talkBoundaries  = GetLayerBoundary(talkImageBoundaries);

            // If all boundaries are null (i.e, no differences at all in the entire layer), mark frame refs as null.
            if (blinkBoundaries == null)
                for (int i = 0; i < 3; ++i)
                    blinkFrameRefs[i] = 0;
            if (talkBoundaries == null)
                for (int i = 0; i < 6; ++i)
                    talkFrameRefs[i] = 0;

            // We have all the info we need -- start building new face data. We're going to write the entire thing in one go.
            // First, figure out how big this thing is going to be.
            // TODO: KAO_Writer?
            var uniqueBlinkImageCount = blinkFrameRefs.Count(x => !x.HasValue);
            var uniqueTalkImageCount  = talkFrameRefs.Count(x => !x.HasValue);

            var totalBytes = 0x222 + (width * height);
            if (uniqueBlinkImageCount > 0)
                totalBytes += blinkBoundaries.Width * blinkBoundaries.Height * uniqueBlinkImageCount;
            if (uniqueTalkImageCount > 0)
                totalBytes += talkBoundaries.Width * talkBoundaries.Height * uniqueTalkImageCount;

            var newData = new ByteData.ByteData(new ByteArray(totalBytes));
            var newDataBytes = newData.GetDataCopyOrReference();

            // Write the header.
            newData.SetWord(0x00, width);
            newData.SetWord(0x02, height);

            // (skip offsets for now)

            newData.SetWord(0x16, (blinkBoundaries == null) ? 0 : blinkBoundaries.Width);
            newData.SetWord(0x18, (blinkBoundaries == null) ? 0 : blinkBoundaries.Height);
            newData.SetWord(0x1A, (talkBoundaries  == null) ? 0 : talkBoundaries.Width);
            newData.SetWord(0x1C, (talkBoundaries  == null) ? 0 : talkBoundaries.Height);

            newData.SetByte(0x1E, (byte) ((blinkBoundaries == null) ? 0 : (blinkBoundaries.X1 - (width  - blinkBoundaries.Width)  / 2)));
            newData.SetByte(0x1F, (byte) ((blinkBoundaries == null) ? 0 : (blinkBoundaries.Y1 - (height - blinkBoundaries.Height) / 2)));
            newData.SetByte(0x20, (byte) ((talkBoundaries  == null) ? 0 : (talkBoundaries.X1  - (width  - talkBoundaries.Width)   / 2)));
            newData.SetByte(0x21, (byte) ((talkBoundaries  == null) ? 0 : (talkBoundaries.Y1  - (height - talkBoundaries.Height)  / 2)));

            var paletteMax = Math.Min(0x100, palette.Channels.Length);
            for (int i = 0; i < paletteMax; i++)
                newData.SetWord(0x22 + i * 2, palette.Channels[i].ToABGR1555());

            int imageDataOffset = 0x222;
            void WriteImageData(byte[,] imageData, DecomposedImageBoundary boundary) {
                for (int y = boundary.Y1; y < boundary.Y2; y++)
                    for (int x = boundary.X1; x < boundary.X2; x++)
                        newDataBytes[imageDataOffset++] = imageData[x, y];
            }

            WriteImageData(baseImageData, new DecomposedImageBoundary(0, 0, width, height));

            int headerOffset = 0x04;
            void WriteLayerImage(byte[,] imageData, int? frameRef, DecomposedImageBoundary boundary) {
                if (frameRef.HasValue)
                    newData.SetWord(headerOffset, -frameRef.Value);
                else {
                    newData.SetWord(headerOffset, imageDataOffset - 0x222);
                    WriteImageData(imageData, boundary);
                }
                headerOffset += 0x02;
            }

            for (int i = 0; i < 3; i++)
                WriteLayerImage(blinkImageDatas[i], blinkFrameRefs[i], blinkBoundaries);
            for (int i = 0; i < 6; i++)
                WriteLayerImage(talkImageDatas[i], talkFrameRefs[i], talkBoundaries);

            // Here we go...!
            Face.Data.SetDataTo(newDataBytes);
        }

        public ushort[,] ImageData16Bit {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public byte[] BitmapDataARGB1555 => GetBitmapDataARGB1555(false);
        public byte[] BitmapDataARGB8888 => GetBitmapDataARGB8888(false);

        public string Hash => _textureDataBuffer.GetOrCacheHash(() => BitmapDataARGB1555.CreateTextureHash());

        public bool ZeroIsTransparent => true;

        public bool CanSetImageData8Bit => true;
        public bool CanSetImageData16Bit => false;

        public byte[] GetBitmapDataARGB1555(bool highlightEndcodes = false)
            => _textureDataBuffer.GetOrCacheBitmapDataARGB1555(() => BitmapUtils.ConvertIndexedDataToARGB1555BitmapData(ImageData8Bit, Palette, ZeroIsTransparent));

        public byte[] GetBitmapDataARGB8888(bool highlightEndcodes = false)
            => _textureDataBuffer.GetOrCacheBitmapDataARGB8888(() => BitmapUtils.ConvertIndexedDataToARGB8888BitmapData(ImageData8Bit, Palette, ZeroIsTransparent));

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
                    var image = Face.CompositeImageTable[i + imageStartIndex];
                    var col = columnStart + i;

                    // For "substitute" frame references, make an image where the number of pixels represents the reference value.
                    if (image.SubstituteFrameRef.HasValue) {
                        frameData[col, row] = new byte[width, height];
                        for (int x = 0; x < (image.SubstituteFrameRef ?? 0); x++)
                            frameData[col, row][x, 0] = lightestColor;
                        continue;
                    }

                    // Get the actual image used here. If there truly is none, do nothing.
                    if (!image.HasImage)
                        continue;
                    frameData[col, row] = image.ImageData8Bit;
                }
            }

            // Add base image.
            frameData[0, 0] = baseImage;

            // Build blinking and talking images.
            BuildImageRow(0, 3, 3, 0);
            BuildImageRow(3, 9, 0, 1);

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
