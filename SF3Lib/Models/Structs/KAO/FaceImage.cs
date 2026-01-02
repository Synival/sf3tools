using System.Linq;
using CommonLib.Attributes;
using CommonLib.Imaging;
using CommonLib.Utils;
using SF3.ByteData;
using SF3.Models.Structs.Shared;
using SF3.Types;

namespace SF3.Models.Structs.KAO {
    public class FaceImage : TextureStructBase {
        public FaceImage(IByteData data, int id, int layer, int index, string name, FaceChunk chunk)
        : base(
            data, id, name, chunk.Header.GetLayerOffset(layer, index), chunk.Header.GetLayerWidth(layer) * chunk.Header.GetLayerHeight(layer),
            TexturePixelFormat.Palette1, isCompressed: false, zeroIsTransparent: true
        ) {
            Layer = layer;
            Index = index;
            Chunk = chunk;

            LoadImageData();

            // Force an update if the data was ever modified.
            // TODO: This is a bit aggressive... maybe only update on relevant data changes?
            chunk.Data.Data.RangeModified += (s, e) => InvalidateImage();
        }

        [TableViewModelColumn(displayOrder: -2.9f, displayGroup: "Metadata")]
        public int Layer { get; }

        [TableViewModelColumn(displayOrder: -2.8f, displayGroup: "Metadata")]
        public int Index { get; }

        [TableViewModelColumn(displayOrder: -2.7f, displayGroup: "Metadata")]
        public int? FrameRef => (Header.GetLayerOffset(Layer, Index) > 0) ? (int?) ID : null;

        [TableViewModelColumn(displayOrder: -2.6f, displayGroup: "Metadata")]
        public int? SubstituteFrameRef {
            get {
                var offset = Header.GetLayerOffset(Layer, Index);
                return offset < 0 ? (int?) -offset : null;
            }
        }

        public FaceImage ActualImage {
            get {
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
                    var otherImage = Chunk.ImageTable[frameRef];
                    if (otherImage.ImageDataOffset > 0)
                        return otherImage;
                }

                // No valid image.
                return null;
            }
        }

        public FaceChunk Chunk { get; }
        public FaceHeader Header => Chunk.Header;

        protected override int StructWidth {
            get => Header.GetLayerWidth(Layer);
            set => Header.SetLayerWidth(Layer, (ushort) value);
        }

        protected override int StructHeight {
            get => Header.GetLayerHeight(Layer);
            set => Header.SetLayerHeight(Layer, (ushort) value);
        }

        [TableViewModelColumn(displayOrder: 10, minWidth: 50)]
        public int X {
            get => Header.GetLayerX(Layer);
            set {
                if (Layer != 0)
                    Header.SetLayerX(Layer, value);
            }
        }

        [TableViewModelColumn(displayOrder: 11, minWidth: 50)]
        public int Y {
            get => Header.GetLayerY(Layer);
            set {
                if (Layer != 0)
                    Header.SetLayerY(Layer, value);
            }
        }

        public override bool CanLoadImage => HasImage;
        public override bool HasImage => Layer == 0 || Header.GetLayerOffset(Layer, Index) > 0;

        protected override int StructImageDataOffset {
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

        protected override Palette StructPalette {
            get => Chunk.Palette;
            set {
                if (Layer == 0)
                    Chunk.Palette = value;
            }
        }

        protected override void OnImageUpdated() {}

        protected override (byte[,], Palette) PreProcessIncomingImageData8Bit(byte[,] newData, Palette palette) {
            (newData, palette) = base.PreProcessIncomingImageData8Bit(newData, palette);

            // Replace the transparent color with the best match
            newData = ImageUtils.Create8BitImageDataWithoutTransparency(newData, palette);

            // Return our new data + palette pair.
            return (newData, palette);
        }
    }
}
