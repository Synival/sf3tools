using CommonLib.Types;
using SF3.ByteData;

namespace SF3.Models.Structs.Shared {
    public abstract class FixedSizeTextureStructBase : TextureStructBase {
        public FixedSizeTextureStructBase(IByteData data, int id, string name, int address, int size,
            int width, int height, TexturePixelFormat pixelFormat, bool isCompressed, bool zeroIsTransparent,
            IndexedColorUpdateStrategy indexedUpdateStrategy)
        : base(data, id, name, address, size, pixelFormat, isCompressed, zeroIsTransparent, indexedUpdateStrategy) {
            _width  = width;
            _height = height;
        }

        private int _width;
        protected override int StructWidth { get => _width; set {} }

        private int _height;
        protected override int StructHeight { get => _height; set {} }
    }
}
