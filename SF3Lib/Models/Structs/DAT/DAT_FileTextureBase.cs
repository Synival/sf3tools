using CommonLib.Types;
using SF3.ByteData;
using SF3.Models.Structs.Shared;

namespace SF3.Models.Structs.DAT {
    public abstract class DAT_FileTextureBase : FixedSizeTextureStructBase {
        public DAT_FileTextureBase(IByteData data, int id, string name, int address, int size, int width, int height, TexturePixelFormat pixelFormat, bool isCompressed, bool zeroIsTransparent) : base(data, id, name, address, size, width, height, pixelFormat, isCompressed, zeroIsTransparent) {
        }
    }
}
