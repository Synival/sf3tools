using CommonLib.Attributes;
using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.DAT {
    public class Face32_TextureModel : TextureModelBase {
        private readonly int _imageDataOffsetAddr;

        public Face32_TextureModel(IByteData data, int id, string name, int address, Palette palette, bool isCompressed)
        : base(data, id, name, address, 4, 32, 32, TexturePixelFormat.Palette1, palette, isCompressed) {
            _imageDataOffsetAddr = address + 0;
            _ = FetchAndCacheTexture();
        }

        public override int ImageDataOffset => Data.GetDouble(_imageDataOffsetAddr);
        public override bool HasImage => ImageDataOffset != -1;

        [TableViewModelColumn(addressField: null, displayName: nameof(ImageDataOffset), displayOrder: 2, displayFormat: "X4")]
        public int ImageDataOffsetViewable {
            get => ImageDataOffset;
            set => Data.SetWord(_imageDataOffsetAddr, value);
        }
    }
}
