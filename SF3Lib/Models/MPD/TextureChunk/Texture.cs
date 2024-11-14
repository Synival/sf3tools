using System;
using CommonLib.Attributes;
using SF3.RawEditors;
using SF3.Types;

namespace SF3.Models.MPD.TextureChunk {
    public class Texture : Model {
        private readonly int widthAddress;
        private readonly int heightAddress;
        private readonly int imageDataOffsetAddress;

        public Texture(IRawEditor editor, int id, string name, int address, int? nextImageDataOffset = null)
        : base(editor, id, name, address, GlobalSize) {
            widthAddress           = Address;     // 1 byte
            heightAddress          = Address + 1; // 1 byte
            imageDataOffsetAddress = Address + 2; // 2 bytes

            if (nextImageDataOffset.HasValue && Width > 0 && Height > 0) {
                int imageDataSize = nextImageDataOffset.Value - ImageDataOffset;
                double bytesPerPixel = ((double) imageDataSize) / (double) Width / (double) Height;
                if (bytesPerPixel == 2.00)
                    AssumedPixelFormat = TexturePixelFormat.ABGR1555;
                else if (bytesPerPixel == 1.00)
                    AssumedPixelFormat = TexturePixelFormat.UnknownPalette;
                else {
                    try {
                        throw new ArgumentException("Unhandled bytes per pixel: " + bytesPerPixel.ToString());
                    }
                    catch {}
                    AssumedPixelFormat = TexturePixelFormat.ABGR1555;
                }
            }
            else
                AssumedPixelFormat = TexturePixelFormat.ABGR1555;

            _readyForImageData = true;
            UpdateImageData();
        }

        public static int GlobalSize => 0x04;

        private bool _readyForImageData = false;

        private bool UpdateImageData() {
            if (!_readyForImageData)
                return false;

            try {
                if (AssumedPixelFormat == TexturePixelFormat.ABGR1555) {
                    var data = new ushort[Width, Height];
                    var off = ImageDataOffset;
                    for (var y = 0; y < Height; y++) {
                        for (var x = 0; x < Width; x++) {
                            var texPixel = (ushort) Editor.GetWord(off);
                            off += 2;
                            data[x, y] = texPixel;
                        }
                    }

                    ImageData8Bit = null;
                    ImageData16Bit = data;
                }
                else {
                    var data = new byte[Width, Height];
                    var off = ImageDataOffset;
                    for (var y = 0; y < Height; y++) {
                        for (var x = 0; x < Width; x++) {
                            var texPixel = (byte) Editor.GetByte(off++);
                            data[x, y] = texPixel;
                        }
                    }

                    ImageData8Bit = data;
                    ImageData16Bit = null;
                }

                return true;
            }
            catch {
                return false;   
            }
        }

        public TexturePixelFormat AssumedPixelFormat { get; }

        [BulkCopy]
        public int Width {
            get => Editor.GetByte(widthAddress);
            set {
                Editor.SetByte(widthAddress, (byte) value);
                UpdateImageData();
            }
        }

        [BulkCopy]
        public int Height {
            get => Editor.GetByte(heightAddress);
            set {
                Editor.SetByte(heightAddress, (byte) value);
                UpdateImageData();
            }
        }

        [BulkCopy]
        public int ImageDataOffset {
            get => Editor.GetWord(imageDataOffsetAddress);
            set {
                Editor.SetWord(imageDataOffsetAddress, value);
                UpdateImageData();
            }
        }

        private ushort[,] _imageData16Bit = null;

        public ushort[,] ImageData16Bit {
            get => _imageData16Bit;
            set {
                if (_imageData16Bit != value) {
                    _imageData16Bit = value;
                    if (value == null) {
                        BitmapDataARGB1555 = null;
                        return;
                    }

                    var imageDataBytes = new byte[value.GetLength(0) * value.GetLength(1) * 2];
                    int pos = 0;
                    for (int y = 0; y < value.GetLength(1); y++) {
                        for (int x = 0; x < value.GetLength(0); x++) {
                            // Swap red and blue channels.
                            var lowerChannel = value[x, y] & 0x001F;
                            var upperChannel = value[x, y] & 0x7F00;
                            var newBits = (value[x, y] & ~0x7C1F) | (lowerChannel << 10) | (upperChannel >> 10);
                            imageDataBytes[pos++] = (byte) ((newBits & 0x00FF));
                            imageDataBytes[pos++] = (byte) ((newBits & 0xFF00) >> 8);
                        }
                    }

                    BitmapDataARGB1555 = imageDataBytes;
                }
            }
        }

        private byte[,] _imageData8Bit = null;

        public byte[,] ImageData8Bit {
            get => _imageData8Bit;
            set {
                if (_imageData8Bit != value) {
                    _imageData8Bit = value;
                    if (value == null) {
                        BitmapDataPalette = null;
                        return;
                    }

                    var imageDataBytes = new byte[value.GetLength(0) * value.GetLength(1)];
                    int pos = 0;
                    for (int y = 0; y < value.GetLength(1); y++)
                        for (int x = 0; x < value.GetLength(0); x++)
                            imageDataBytes[pos++] = value[x, y];
                    BitmapDataPalette = imageDataBytes;
                }
            }
        }

        /// <summary>
        /// Image data for 16-bit ARGB1555 format.
        /// </summary>
        public byte[] BitmapDataARGB1555 { get; private set; } = null;

        /// <summary>
        /// Image data for 8-bit palette format.
        /// </summary>
        public byte[] BitmapDataPalette { get; private set; } = null;
    }
}
