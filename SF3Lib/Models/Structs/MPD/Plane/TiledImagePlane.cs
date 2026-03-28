using System;
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
            TexturePixelFormat paletteType,
            Func<IPalette> paletteGetter,
            Action<IPalette> paletteSetter,
            bool zeroIsTransparent
        ) {
            TilesetDatas   = new IByteData[] { tilesetData1, tilesetData2 };
            TileAssignment = tileAssignment;
            PaletteType    = paletteType;
            PaletteGetter  = paletteGetter;
            PaletteSetter  = paletteSetter;
            ZeroIsTransparent = zeroIsTransparent;

            UpdateImages();
        }

        public void UpdateImages() {
            Tileset = new MultiChunkTextureIndexed(TilesetDatas, PaletteType, PaletteGetter, PaletteSetter, zeroIsTransparent: ZeroIsTransparent, isTiled: true);
            TiledImage = new InMemoryTextureData(MPD_TiledPlane.CreateTiledImageData(Tileset, TileAssignment), PaletteGetter(), zeroIsTransparent: ZeroIsTransparent, canSetImage: false, IndexedColorUpdateStrategy.MatchToExistingPalette);
        }

        public ITextureData Tileset { get; private set; }
        public IMPD_PlaneTileAssignment TileAssignment { get; private set; }
        public ITextureData TiledImage { get; private set; }

        public IByteData[] TilesetDatas { get; }
        public TexturePixelFormat PaletteType { get; }
        public Func<IPalette> PaletteGetter { get; }
        public Action<IPalette> PaletteSetter { get; }
        public bool ZeroIsTransparent { get; }
    }
}
