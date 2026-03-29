using CommonLib.Imaging;
using CommonLib.Types;
using SF3.ByteData;

namespace SF3.Models.Files.MPD {
    public class MultiChunkTextureData : CachedTextureData {
        public MultiChunkTextureData(
            IByteData[] datas, bool isTiled, IPalette palette, bool zeroIsTransparent, ImageDataCanSet canSetImage, IndexedColorUpdateStrategy indexedUpdateStrategy)
        : this(palette, zeroIsTransparent, canSetImage, new MultiChunkTextureDataSource(datas, isTiled), indexedUpdateStrategy)
        {}

        private MultiChunkTextureData(IPalette palette, bool zeroIsTransparent, ImageDataCanSet canSetImage, MultiChunkTextureDataSource dataSource, IndexedColorUpdateStrategy indexedUpdateStrategy)
        : base(dataSource.Width, dataSource.Height, TexturePixelFormat.Indexed8Bit, palette, zeroIsTransparent, canSetImage, dataSource, indexedUpdateStrategy) {
            Add8BitValidator((data, _2, _3, _4) => TextureDataValidators.IsSameDimensions(data, Width, Height));
        }
    }
}
