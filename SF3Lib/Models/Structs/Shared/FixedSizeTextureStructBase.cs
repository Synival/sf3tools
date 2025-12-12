using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.Shared {
    public abstract class FixedSizeTextureStructBase : TextureStructBase {
        public FixedSizeTextureStructBase(IByteData data, int id, string name, int address, int size,
            int width, int height, TexturePixelFormat pixelFormat, bool isCompressed, bool zeroIsTransparent)
        : base(data, id, name, address, size, pixelFormat, isCompressed, zeroIsTransparent) {
            _width  = width;
            _height = height;
        }

        private int _width;
        [TableViewModelColumn(addressField: null, displayOrder: 0)]
        public override int Width { get => _width; set {} }

        private int _height;
        [TableViewModelColumn(addressField: null, displayOrder: 1)]
        public override int Height { get => _height; set {} }
    }
}
