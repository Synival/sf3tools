using CommonLib.Attributes;
using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Models.Structs.Shared;
using SF3.Types;

namespace SF3.Models.Structs.DAT {
    public class Face32_Texture : FixedSizeTextureStructBase {
        private readonly int _imageDataOffsetAddr;

        public Face32_Texture(IByteData data, int id, string name, int address, Palette palette, bool isCompressed)
        : base(data, id, name, address, 4, 32, 32, TexturePixelFormat.Palette1, isCompressed, false) {
            _palette = palette;
            _imageDataOffsetAddr = address + 0;
            LoadImageData();
        }

        public override int ImageDataOffset {
            get => Data.GetDouble(_imageDataOffsetAddr);
            set {
                Data.SetDouble(_imageDataOffsetAddr, value);
                InvalidateImage();
            }
        }

        public override bool HasImage => ImageDataOffset != -1;
        public override bool CanLoadImage => HasImage && !IsCompressed;

        [TableViewModelColumn(addressField: null, displayName: nameof(ImageDataOffset), displayOrder: 2, displayFormat: "X4")]
        public int ImageDataOffsetViewable {
            get => ImageDataOffset;
            set => Data.SetWord(_imageDataOffsetAddr, value);
        }

        public override void OnSetImageData() {}

        private readonly Palette _palette;
        public override Palette Palette { get => _palette; protected set {} }
    }
}
