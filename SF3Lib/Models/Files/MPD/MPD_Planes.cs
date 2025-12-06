using System.Linq;
using SF3.MPD;
using SF3.Types;

namespace SF3.Models.Files.MPD {
    public class MPD_Planes : IMPD_Planes {
        public MPD_Planes(IMPD_File mpdFile) {
            MPD_File = mpdFile;
            UpdateImages();
        }

        public void UpdateImages() {
            ITexture groundImage          = null;
            ITexture groundTileset        = null;
            IMPD_PlaneTileAssignment groundTileAssignment = null;
            ITexture groundTiledImage     = null;
            ITexture backgroundImage      = null;

            ITexture skyBoxImage          = null;
            ITexture foregroundTileset    = null;
            IMPD_PlaneTileAssignment foregroundTileAssignment = null;
            ITexture foregroundTiledImage = null;

            if (MPD_File.GroundImageChunkDatas?.Any() == true) {
                try {
                    var palette = MPD_File.CreatePalette(0);
                    groundImage = new MultiChunkTextureIndexed(MPD_File.GroundImageChunkDatas.Select(x => x.DecompressedData).ToArray(), TexturePixelFormat.Palette1, palette);
                }
                catch {
                    // TODO: what to do here??
                }
            }

            if (MPD_File.GroundTilesetChunkDatas?.Any() == true && MPD_File.GroundTileAssignmentChunks?.Length == 2) {
                var palette = MPD_File.CreatePalette(0);
                groundTileset = new MultiChunkTextureIndexed(MPD_File.GroundTilesetChunkDatas.Select(x => x.DecompressedData).ToArray(), TexturePixelFormat.Palette1, palette, true);
                groundTileAssignment = new MPD_GroundPlaneTileAssignment(
                    MPD_File.GroundTileAssignmentChunks[0].PlaneTileTextureRowTable,
                    MPD_File.GroundTileAssignmentChunks[1].PlaneTileTextureRowTable
                );

                var tiledGroundImageData = CreateTiledImageData(groundTileset, groundTileAssignment);
                groundTiledImage = new TextureIndexed(0, 0, 0, 0, tiledGroundImageData, TexturePixelFormat.Palette1, palette, false);
            }

            if (MPD_File.SkyBoxChunkDatas?.Any() == true)
                skyBoxImage = new MultiChunkTextureIndexed(MPD_File.SkyBoxChunkDatas.Select(x => x.DecompressedData).ToArray(), TexturePixelFormat.Palette2, MPD_File.CreatePalette(1));

            if (MPD_File.BackgroundChunkDatas?.Any() == true)
                backgroundImage = new MultiChunkTextureIndexed(MPD_File.BackgroundChunkDatas.Select(x => x.DecompressedData).ToArray(), TexturePixelFormat.Palette1, MPD_File.CreatePalette(0));

            if (MPD_File.ForegroundTileChunkDatas?.Any() == true && MPD_File.ForegroundTileAssignmentChunk != null) {
                var palette = MPD_File.CreatePalette(1);
                foregroundTileset = new MultiChunkTextureIndexed(MPD_File.ForegroundTileChunkDatas.Select(x => x.DecompressedData).ToArray(), TexturePixelFormat.Palette1, palette, true);
                foregroundTileAssignment = new MPD_ForegroundPlaneTileAssignment(MPD_File.ForegroundTileAssignmentChunk.PlaneTileTextureRowTable);

                var foregroundImageData = CreateTiledImageData(foregroundTileset, foregroundTileAssignment);
                foregroundTiledImage = new TextureIndexed(0, 0, 0, 0, foregroundImageData, TexturePixelFormat.Palette2, palette, true);
            }

            GroundImage          = groundImage;
            GroundTileset        = groundTileset;
            GroundTileAssignment = groundTileAssignment;
            GroundTiledImage     = groundTiledImage;
            BackgroundImage      = backgroundImage;

            SkyBoxImage          = skyBoxImage;
            ForegroundTileset    = foregroundTileset;
            ForegroundTileAssignment = foregroundTileAssignment;
            ForegroundTiledImage = foregroundTiledImage;
        }

        private byte[,] CreateTiledImageData(ITexture tilesetImage, IMPD_PlaneTileAssignment tileAssignment) {
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

        public IMPD_File MPD_File { get; }

        public ITexture GroundImage { get; private set; }
        public ITexture GroundTileset { get; private set; }
        public IMPD_PlaneTileAssignment GroundTileAssignment { get; private set; }
        public ITexture GroundTiledImage { get; private set; }
        public ITexture BackgroundImage { get; private set; }

        public ITexture SkyBoxImage { get; private set; }
        public ITexture ForegroundTileset { get; private set; }
        public IMPD_PlaneTileAssignment ForegroundTileAssignment { get; private set; }
        public ITexture ForegroundTiledImage { get; private set; }
    }
}
