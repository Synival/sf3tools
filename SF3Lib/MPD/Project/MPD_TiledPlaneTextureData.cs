using CommonLib.Imaging;
using CommonLib.Types;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Project {
    public class MPD_TiledPlaneTextureData : CachedTextureData {
        public MPD_TiledPlaneTextureData(ITextureData tileset, IMPD_PlaneTileAssignment tileAssignment, IPalette palette, bool zeroIsTransparent)
        : this(new MPD_TiledPlaneTextureDataSource(tileset, tileAssignment), palette, zeroIsTransparent) {
        }

        private MPD_TiledPlaneTextureData(MPD_TiledPlaneTextureDataSource dataSource, IPalette palette, bool zeroIsTransparent)
        : base(dataSource.TileAssignment.Width * 8, dataSource.TileAssignment.Height * 8, TexturePixelFormat.Indexed8Bit, palette, zeroIsTransparent, ImageDataCanSet.Never, dataSource, IndexedColorUpdateStrategy.DontUpdate) {
        }
    }
}
