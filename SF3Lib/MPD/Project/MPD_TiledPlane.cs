using CommonLib.Extensions;
using CommonLib.Imaging;
using CommonLib.Types;
using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Project {
    public class MPD_TiledPlane : IMPD_TiledPlane {
        public MPD_TiledPlane(IMPD_TiledPlane original, IPalette palette) {
            if (original.Tileset != null) {
                Tileset = new InMemoryTextureData(original.Tileset, palette, IndexedColorUpdateStrategy.UpdateExistingPalette);
                ((InMemoryTextureData) Tileset).Add8BitValidator((data, _, _1, _2) => TextureDataValidators.IsSameDimensions(data, 512, 256));
            }
            if (original.TileAssignment != null)
                TileAssignment = new MPD_PlaneTileAssignment(original.TileAssignment);
            if (original.TiledImage != null)
                TiledImage = new InMemoryTextureData(original.TiledImage, palette, IndexedColorUpdateStrategy.UpdateExistingPalette);
        }

        public static MPD_TiledPlane FromJToken(JToken token, IPalette palette, int tilesWidth, int tilesHeight)
            => new MPD_TiledPlane(token, palette, tilesWidth, tilesHeight);
        private MPD_TiledPlane(JToken token, IPalette palette, int tilesWidth, int tilesHeight) {
            var jObject = (JObject) token;

            Tileset = jObject.GetValueIfExists("Tileset",
                t => InMemoryTextureData.FromJToken(t, 512, 256, TexturePixelFormat.Indexed8Bit, false, palette, true, IndexedColorUpdateStrategy.UpdateExistingPalette));
            if (Tileset != null)
                ((InMemoryTextureData) Tileset).Add8BitValidator((data, _, _1, _2) => TextureDataValidators.IsSameDimensions(data, 512, 256));

            TileAssignment = jObject.GetValueIfExists("TileAssignment",
                t => MPD_PlaneTileAssignment.FromJToken(t, tilesWidth, tilesHeight));

            if (Tileset != null && TileAssignment != null)
                TiledImage = new InMemoryTextureData(CreateTiledImageData(Tileset, TileAssignment), palette, zeroIsTransparent: false, canSetImage: false, IndexedColorUpdateStrategy.UpdateExistingPalette);
        }

        public static byte[,] CreateTiledImageData(ITextureData tilesetImage, IMPD_PlaneTileAssignment tileAssignment) {
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

        public ITextureData Tileset { get; set; }
        public IMPD_PlaneTileAssignment TileAssignment { get; set; }
        public ITextureData TiledImage { get; set; }
    }
}
