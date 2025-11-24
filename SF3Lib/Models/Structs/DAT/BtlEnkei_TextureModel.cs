using System;
using System.Drawing;
using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.DAT {
    public class BtlEnkei_TextureModel : BtlEnkei_TextureModelBase {
        private readonly int _paletteImageOffsetAddr;
        private readonly int _paletteImageSizeAddr;
        private readonly int _loadSizeAddr;
        private readonly int _paddingAddr;

        public BtlEnkei_TextureModel(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x10, data.GetDouble(address), fetchImmediately: false
        ) {
            _paletteImageOffsetAddr = Address + 0x00; // 4 bytes
            _paletteImageSizeAddr   = Address + 0x04; // 4 bytes
            _loadSizeAddr           = Address + 0x08; // 4 bytes
            _paddingAddr            = Address + 0x0C; // 4 bytes

            _ = FetchAndCacheTexture();
        }

        public override int ImageDataOffset => HasImage ? (PaletteImageOffset + 0x200) : 0;
        public override bool HasImage => PaletteImageOffset != 0;
        public override bool CanLoadImage => HasImage;
        public override int PaletteOffset => PaletteImageOffset;

        [TableViewModelColumn(addressField: nameof(_paletteImageOffsetAddr), displayOrder: -0.5f, isPointer: true)]
        public int PaletteImageOffset {
            get => Data.GetDouble(_paletteImageOffsetAddr);
            set => Data.SetDouble(_paletteImageOffsetAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_paletteImageSizeAddr), displayOrder: -0.4f, displayFormat: "X4")]
        public int PaletteImageSize {
            get => Data.GetDouble(_paletteImageSizeAddr);
            set => Data.SetDouble(_paletteImageSizeAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_loadSizeAddr), displayOrder: -0.3f, displayFormat: "X4")]
        public int LoadSize {
            get => Data.GetDouble(_loadSizeAddr);
            set => Data.SetDouble(_loadSizeAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_paddingAddr), displayOrder: -0.2f, displayFormat: "X4")]
        public int Padding {
            get => Data.GetDouble(_paddingAddr);
            set => Data.SetDouble(_paddingAddr, value);
        }

        public override void LoadImageAction(Image image, string filename) {
            base.LoadImageAction(image, filename);
            PaletteImageSize = 0x200 + StoredImageDataSize;
            LoadSize = ((PaletteImageSize + 0x7FF) / 0x800) * 0x800;
        }
    }
}
