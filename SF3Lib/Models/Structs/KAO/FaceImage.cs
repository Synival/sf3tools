using System;
using CommonLib.Attributes;
using CommonLib.Extensions;
using CommonLib.Imaging;
using CommonLib.Types;
using CommonLib.Utils;
using SF3.ByteData;

namespace SF3.Models.Structs.KAO {
    public class FaceImage : Struct, ITextureData {
        public FaceImage(IByteData data, int id, int layer, int index, string name, FaceChunk face)
        : base(data, id, name, 0 /* not applicable */, 0 /* not applicable */) {
            Layer = layer;
            Index = index;
            Face  = face;

            // Force an update if the data was ever modified.
            // TODO: This is a bit aggressive... maybe only update on relevant data changes?
            face.Data.Data.RangeModified += (s, e) => InvalidateImage();
        }

        public void InvalidateImage() {
            _textureDataCache.Invalidate();
            Invalidated?.Invoke(this, EventArgs.Empty);
        }

        public FaceImage GetActualImage() {
            // The base image is always itself.
            if (Layer == 0)
                return this;

            // Positive offsets are itself, zero offsets are 'no image'.
            var offset = ImageDataOffset;
            if (offset > 0)
                return this;
            else if (offset == 0)
                return null;

            // Negative offsets reference a different layer.
            // The frame referenced must be between (1, 9) inclusive and cannot reference the same frame.
            // If the frame referenced is *also* a referencing frame, it's not valid.
            var frameRef = -offset;
            var sameFrameRef = Layer * 3 + Index + 1;
            if (frameRef >= 1 && frameRef <= 9 && frameRef != sameFrameRef) {
                var otherImage = Face.ImageTable[frameRef];
                if (otherImage.ImageDataOffset > 0)
                    return otherImage;
            }

            // No valid image.
            return null;
        }

        public FaceChunk Face { get; }
        public FaceHeader Header => Face.Header;

        [TableViewModelColumn(displayOrder: -2.9f, displayGroup: "Metadata")]
        public int Layer { get; }

        [TableViewModelColumn(displayOrder: -2.8f, displayGroup: "Metadata")]
        public int Index { get; }

        [TableViewModelColumn(displayOrder: 0, minWidth: 50)]
        public int Width {
            get => Header.GetLayerWidth(Layer);
            set => Header.SetLayerWidth(Layer, (ushort) value);
        }

        [TableViewModelColumn(displayOrder: 1, minWidth: 50)]
        public int Height {
            get => Header.GetLayerHeight(Layer);
            set => Header.SetLayerHeight(Layer, (ushort) value);
        }

        [TableViewModelColumn(displayOrder: 2, displayFormat: "X4")]
        public int ImageDataSize => Width * Height * BytesPerPixel;

        [TableViewModelColumn(displayOrder: 3, displayName: "RelX", minWidth: 50)]
        public int RelativeX {
            get => Header.GetLayerRelativeX(Layer);
            set {
                if (Layer != 0)
                    Header.SetLayerRelativeX(Layer, value);
            }
        }

        [TableViewModelColumn(displayOrder: 4, displayName: "RelY", minWidth: 50)]
        public int RelativeY {
            get => Header.GetLayerRelativeY(Layer);
            set {
                if (Layer != 0)
                    Header.SetLayerRelativeY(Layer, value);
            }
        }

        [TableViewModelColumn(displayOrder: 4.1f, minWidth: 50)]
        public int X {
            get => Header.GetLayerX(Layer);
            set {
                if (Layer != 0)
                    Header.SetLayerX(Layer, value);
            }
        }

        [TableViewModelColumn(displayOrder: 4.2f, minWidth: 50)]
        public int Y {
            get => Header.GetLayerY(Layer);
            set {
                if (Layer != 0)
                    Header.SetLayerY(Layer, value);
            }
        }

        [TableViewModelColumn(displayOrder: 5, displayFormat: "-X4")]
        public int ImageDataOffset {
            get {
                if (Layer == 0)
                    return 0x222;

                // Real offsets (0 or higher) need 0x222 added to them.
                // Negative offsets -- which are functional values -- stay as they are.
                var offset = Header.GetLayerOffset(Layer, Index);
                return offset > 0 ? (offset + 0x222) : offset;
            }
            set {
                if (Layer == 0)
                    return;

                // Unapply the 0x222 for offsets when setting them.
                // If the range set is in range (0, 0x221), it's invalid; just use zero.
                // Negative offsets -- which are functional values -- stay as they are.
                var newOffset = (value > 0x222) ? (value - 0x222) : (value > 0) ? 0 : value;
                Header.SetLayerOffset(Layer, Index, (short) newOffset);
            }
        }

        [TableViewModelColumn(displayOrder: 6)]
        public bool HasImage => Layer == 0 || Header.GetLayerOffset(Layer, Index) > 0;

        [TableViewModelColumn(displayOrder: 7)]
        public int? FrameRef => (Header.GetLayerOffset(Layer, Index) > 0) ? (int?) ID : null;

        [TableViewModelColumn(displayOrder: 8)]
        public int? SubstituteFrameRef {
            get {
                var offset = Header.GetLayerOffset(Layer, Index);
                return offset < 0 ? (int?) -offset : null;
            }
        }

        [TableViewModelColumn(displayOrder: 9, minWidth: 225)]
        public string Hash => _textureDataCache.GetOrCacheHash(() => BitmapDataARGB1555.CreateTextureHash());

        public byte[] GetBitmapDataARGB1555(bool highlightEndcodes = false)
            => _textureDataCache.GetOrCacheBitmapDataARGB1555(() => BitmapUtils.ConvertIndexedDataToARGB1555BitmapData(ImageData8Bit, Palette, true));

        public byte[] GetBitmapDataARGB8888(bool highlightEndcodes = false)
            => _textureDataCache.GetOrCacheBitmapDataARGB8888(() => BitmapUtils.ConvertIndexedDataToARGB8888BitmapData(ImageData8Bit, Palette, true));

        public string Validate8BitImageData(byte[,] data, IPalette palette, int? oldStoredSize, int? newStoredSize)
            => TextureDataValidators.IsSameDimensions(data, Width, Height);

        public string Validate16BitImageData(ushort[,] data, int? oldStoredSize, int? newStoredSize)
            => throw new NotSupportedException();

        public int BytesPerPixel => 1;
        public TexturePixelFormat PixelFormat => TexturePixelFormat.Indexed8Bit;

        public byte[,] ImageData8Bit
            => _textureDataCache.GetOrCacheImageData8Bit(() => HasImage ? Data.GetDataCopyAt(ImageDataOffset, Width * Height).To2DArrayColumnMajor(Width, Height) : null);

        public void SetImageData8Bit(byte[,] data, IPalette palette) {
            var error = Validate8BitImageData(data, palette, ImageDataSize, data.GetLength(0) * data.GetLength(1));
            if (error != null)
                throw new ArgumentException(error);

            // Replacing the base image has some special qualities: no transparency, and update the palette.
            if (Layer == 0) {
                data = ImageUtils.Create8BitImageDataWithoutTransparency(data, palette);
                Palette = palette;
            }
            else
                data = ImageUtils.GetImageDataConformingToPalette(data, palette, Palette);

            _textureDataCache.Invalidate();
            Data.Data.SetDataAtTo(ImageDataOffset, data.GetLength(0) * data.GetLength(1), data.To1DArrayTransposed());
            _textureDataCache.SetImageData8Bit(data);
            Invalidated?.Invoke(this, EventArgs.Empty);
        }

        public ushort[,] ImageData16Bit {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public byte[] BitmapDataARGB1555 => GetBitmapDataARGB1555(false);
        public byte[] BitmapDataARGB8888 => GetBitmapDataARGB8888(false);

        public IPalette Palette {
            get => Face.Palette;
            set {
                if (Layer == 0)
                    Face.Palette = value;
            }
        }

        public bool ZeroIsTransparent => true;
        public bool CanSetImageData8Bit => HasImage;
        public bool CanSetImageData16Bit => false;

        private TextureDataCache _textureDataCache = new TextureDataCache();

        public event EventHandler Invalidated;
    }
}
