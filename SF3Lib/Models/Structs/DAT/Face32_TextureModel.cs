using System;
using System.Drawing;
using CommonLib.Attributes;
using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Extensions;
using SF3.Types;

namespace SF3.Models.Structs.DAT {
    public class Face32_TextureModel : TextureModelBase {
        private readonly int _imageDataOffsetAddr;

        public Face32_TextureModel(IByteData data, int id, string name, int address, Palette palette, bool isCompressed)
        : base(data, id, name, address, 4, 32, 32, TexturePixelFormat.Palette1, palette, isCompressed, false) {
            _imageDataOffsetAddr = address + 0;
            _ = FetchAndCacheTexture();
        }

        public override int ImageDataOffset => Data.GetDouble(_imageDataOffsetAddr);
        public override bool HasImage => ImageDataOffset != -1;
        public override bool CanLoadImage => HasImage && !IsCompressed;

        [TableViewModelColumn(addressField: null, displayName: nameof(ImageDataOffset), displayOrder: 2, displayFormat: "X4")]
        public int ImageDataOffsetViewable {
            get => ImageDataOffset;
            set => Data.SetWord(_imageDataOffsetAddr, value);
        }

        public override void LoadImageAction(Image image, string filename) {
            if (image.Width != Width || image.Height != Height)
                throw new ArgumentException($"Incoming image dimensions ({image.Width}x{image.Height}) should be {Width}x{Height}");
            if (image.PixelFormat != System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
                throw new ArgumentException($"Incoming image pixel format ({image.PixelFormat}) should be 'Format8bppIndexed'");
            Texture = image.CreateTextureIndexed(CollectionType.Primary, 0, 0, 0, ZeroIsTransparent);
        }

        public override void LoadPaletteFromImage(ITexture texture) {
            // Palette can't be changed; it's shared between all images.
        }
    }
}
