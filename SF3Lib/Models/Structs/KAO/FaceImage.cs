using CommonLib.Attributes;
using CommonLib.Imaging;
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
        public override bool HasImage => Header.GetLayerOffset(Layer, Index) > 0;

        protected override int StructImageDataOffset {
            get {
                // Real offsets (0 or higher) need 0x222 added to them.
                // Negative offsets -- which are functional values -- stay as they are.
                var offset = Header.GetLayerOffset(Layer, Index);
                return offset >= 0 ? (offset + 0x222) : offset;
            }
            set {
                if (Layer == 0)
                    return;

                // Unapply the 0x222 for offsets when setting them.
                // If the range set is in range (0, 0x221), it's invalid; just use zero.
                // Negative offsets -- which are functional values -- stay as they are.
                var newOffset = (value >= 0x222) ? (value - 0x222) : (value > 0) ? 0 : value;
                Header.SetLayerOffset(Layer, Index, (short) newOffset);
            }
        }

        protected override Palette StructPalette {
            get => Chunk.Palette;
            set {}
        }

        protected override void OnImageUpdated() {}
    }
}
