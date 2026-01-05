using System;
using CommonLib.Imaging;
using CommonLib.Types;
using SF3.ByteData;
using SF3.Imaging;
using SF3.Models.Files.MPD;
using SF3.MPD;

namespace SF3.Models.Structs.MPD.Plane {
    public class TiledImagePlane : IMPD_TiledPlane {
        public TiledImagePlane(
            IByteData tilesetData1,
            IByteData tilesetData2,
            IMPD_PlaneTileAssignment tileAssignment,
            TexturePixelFormat paletteType,
            Func<Palette> paletteGetter,
            Action<Palette> paletteSetter,
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
            TiledImage = new TextureData(CreateTiledImageData(Tileset, TileAssignment), PaletteGetter(), zeroIsTransparent: ZeroIsTransparent, canSetImage: false);
        }

        private byte[,] CreateTiledImageData(ITextureData tilesetImage, IMPD_PlaneTileAssignment tileAssignment) {
            var outputImageData = new byte[tileAssignment.Width * 8, tileAssignment.Height * 8];
            var inputImageData  = tilesetImage.ImageData8Bit;

            for (int tileY = 0; tileY < tileAssignment.Height; tileY++) {
                var outputY = tileY * 8;
                for (int tileX = 0; tileX < tileAssignment.Width; tileX++) {
                    var outputX = tileX * 8;

                    var tilesetCoords = tileAssignment[(byte) tileX, (byte) tileY];
                    var inputX = tilesetCoords.X * 8;
                    var inputY = tilesetCoords.Y * 8;

                    for (int y = 0; y < 8; y++)
                        for (int x = 0; x < 8; x++)
                            outputImageData[outputX + x, outputY + y] = inputImageData[inputX + x, inputY + y];
                }
            }

            return outputImageData;
        }

        public ITextureData Tileset { get; private set; }
        public IMPD_PlaneTileAssignment TileAssignment { get; private set; }
        public ITextureData TiledImage { get; private set; }

        public IByteData[] TilesetDatas { get; }
        public TexturePixelFormat PaletteType { get; }
        public Func<Palette> PaletteGetter { get; }
        public Action<Palette> PaletteSetter { get; }
        public bool ZeroIsTransparent { get; }
    }
}
