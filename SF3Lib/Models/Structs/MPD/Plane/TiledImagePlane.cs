using CommonLib.Imaging;
using CommonLib.Types;
using SF3.ByteData;
using SF3.Models.Files.MPD;
using SF3.MPD.Interfaces;
using SF3.MPD.Project;

namespace SF3.Models.Structs.MPD.Plane {
    public class TiledImagePlane : IMPD_TiledPlane {
        public TiledImagePlane(
            IByteData tilesetData1,
            IByteData tilesetData2,
            IMPD_PlaneTileAssignment tileAssignment,
            IPalette palette,
            bool zeroIsTransparent
        ) {
            TilesetDatas   = new IByteData[] { tilesetData1, tilesetData2 };
            TileAssignment = tileAssignment;
            Palette        = palette;
            ZeroIsTransparent = zeroIsTransparent;

            UpdateImages();
        }

        public void UpdateImages() {
            Tileset = new MultiChunkTextureIndexed(TilesetDatas, isTiled: true, Palette, ZeroIsTransparent, ImageDataCanSet.CanSet8Bit, IndexedColorUpdateStrategy.UpdateExistingPalette);
            TiledImage = new InMemoryTextureData(MPD_TiledPlane.CreateTiledImageData(Tileset, TileAssignment), Palette, ZeroIsTransparent, ImageDataCanSet.Never, IndexedColorUpdateStrategy.MatchToExistingPalette);
        }

        public IByteData[] TilesetDatas { get; }
        public IMPD_PlaneTileAssignment TileAssignment { get; private set; }
        public IPalette Palette { get; }
        public bool ZeroIsTransparent { get; }

        public ITextureData Tileset { get; private set; }
        public ITextureData TiledImage { get; private set; }
    }
}
