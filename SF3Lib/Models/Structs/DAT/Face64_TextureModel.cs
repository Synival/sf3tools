using System.Drawing;
using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Extensions;
using SF3.Types;

namespace SF3.Models.Structs.DAT {
    public class Face64_TextureModel : TextureModelBase {
        public Face64_TextureModel(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 64 * 64 * 2, 64, 64, TexturePixelFormat.ABGR1555, null, false, false) {
            _ = FetchAndCacheTexture();
        }

        public override int ImageDataOffset => Address;
        public override bool HasImage => true;
        public override bool CanLoadImage => true;

        [TableViewModelColumn(addressField: null, displayOrder: 2, displayFormat: "X4")]
        public int ImageDataOffsetViewable => ImageDataOffset;

        public override void LoadImageAction(Image image, string filename) {
            // TODO: better error handling
            if (image.Width != Width || image.Height != Height)
                return;
            Texture = image.CreateTextureABGR1555(CollectionType.Primary, 0, 0, 0);
        }

        public override void LoadPaletteFromImage(ITexture texture) {
            // Nothing to do; there shouldn't ever be a palette to load.
        }
    }
}
