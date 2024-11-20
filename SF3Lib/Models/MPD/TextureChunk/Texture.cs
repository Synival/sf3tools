using System;
using CommonLib.Attributes;
using CommonLib.Utils;
using SF3.Attributes;
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
                    CachedImageData8Bit = null;
                    CachedImageData16Bit = ImageData16Bit;
                }
                else {
                    CachedImageData8Bit = ImageData8Bit;
                    CachedImageData16Bit = null;
                }

                return true;
            }
            catch {
                return false;   
            }
        }

        [BulkCopy]
        [ViewModelData(displayOrder: 0)]
        public int Width {
            get => Editor.GetByte(widthAddress);
            set {
                Editor.SetByte(widthAddress, (byte) value);
                UpdateImageData();
            }
        }

        [BulkCopy]
        [ViewModelData(displayOrder: 1)]
        public int Height {
            get => Editor.GetByte(heightAddress);
            set {
                Editor.SetByte(heightAddress, (byte) value);
                UpdateImageData();
            }
        }

        [BulkCopy]
        [ViewModelData(displayOrder: 2, displayFormat: "X4")]
        public int ImageDataOffset {
            get => Editor.GetWord(imageDataOffsetAddress);
            set {
                Editor.SetWord(imageDataOffsetAddress, value);
                UpdateImageData();
            }
        }

        [ViewModelData(displayName: "(Assumed) Pixel Format", displayOrder: 3)]
        public TexturePixelFormat AssumedPixelFormat { get; }

        public ushort[,] ImageData16Bit {
            protected get {
                var data = new ushort[Width, Height];
                var off = ImageDataOffset;
                for (var y = 0; y < Height; y++) {
                    for (var x = 0; x < Width; x++) {
                        var texPixel = (ushort) Editor.GetWord(off);
                        off += 2;
                        data[x, y] = texPixel;
                    }
                }
                return data;
            }
            set {
                if (value.GetLength(0) != Width || value.GetLength(1) != Height)
                    throw new ArgumentException("Incoming data dimensions must match specified width/height");

                var off = ImageDataOffset;
                for (var y = 0; y < Height; y++) {
                    for (var x = 0; x < Width; x++) {
                        Editor.SetWord(off, value[x, y]);
                        off += 2;
                    }
                }

                CachedImageData16Bit = value;
            }
        }

        private ushort[,] _cachedImageData16Bit = null;

        public ushort[,] CachedImageData16Bit {
            get => (ushort[,]) _cachedImageData16Bit.Clone();
            private set {
                if (_cachedImageData16Bit != value) {
                    _cachedImageData16Bit = value;
                    if (value == null) {
                        CachedBitmapDataARGB1555 = null;
                        return;
                    }

                    var imageDataBytes = new byte[value.GetLength(0) * value.GetLength(1) * 2];
                    int pos = 0;
                    for (int y = 0; y < value.GetLength(1); y++) {
                        for (int x = 0; x < value.GetLength(0); x++) {
                            var newBits = PixelConversion.ARGB1555toABGR1555(value[x, y]);
                            imageDataBytes[pos++] = (byte) ((newBits & 0x00FF));
                            imageDataBytes[pos++] = (byte) ((newBits & 0xFF00) >> 8);
                        }
                    }

                    CachedBitmapDataARGB1555 = imageDataBytes;
                }
            }
        }

        protected byte[,] ImageData8Bit {
            get {
                var data = new byte[Width, Height];
                var off = ImageDataOffset;
                for (var y = 0; y < Height; y++) {
                    for (var x = 0; x < Width; x++) {
                        var texPixel = (byte) Editor.GetByte(off++);
                        data[x, y] = texPixel;
                    }
                }
                return data;
            }
            set {
                if (value.GetLength(0) != Width || value.GetLength(1) != Height)
                    throw new ArgumentException("Incoming data dimensions must match specified width/height");

                var off = ImageDataOffset;
                for (var y = 0; y < Height; y++)
                    for (var x = 0; x < Width; x++)
                        Editor.SetByte(off++, value[x, y]);

                CachedImageData8Bit = value;
            }
        }

        private byte[,] _cachedImageData8Bit = null;

        public byte[,] CachedImageData8Bit {
            get => (byte[,]) _cachedImageData8Bit.Clone();
            set {
                if (_cachedImageData8Bit != value) {
                    _cachedImageData8Bit = value;
                    if (value == null) {
                        CachedBitmapDataPalette = null;
                        return;
                    }

                    var imageDataBytes = new byte[value.GetLength(0) * value.GetLength(1)];
                    int pos = 0;
                    for (int y = 0; y < value.GetLength(1); y++)
                        for (int x = 0; x < value.GetLength(0); x++)
                            imageDataBytes[pos++] = value[x, y];
                    CachedBitmapDataPalette = imageDataBytes;
                }
            }
        }

        /// <summary>
        /// Image data for 16-bit ARGB1555 format.
        /// </summary>
        public byte[] CachedBitmapDataARGB1555 { get; private set; } = null;

        /// <summary>
        /// Image data for 8-bit palette format.
        /// </summary>
        public byte[] CachedBitmapDataPalette { get; private set; } = null;
    }
}
