using System;
using System.Linq;
using SF3.ByteData;
using SF3.Models.Tables.MPD.Plane;
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
            ITexture groundTiledImage     = null;
            ITexture skyBoxImage          = null;
            ITexture backgroundImage      = null;
            ITexture foregroundTileset    = null;
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

            if (MPD_File.GroundTilesetChunkDatas?.Any() == true && MPD_File.GroundTileAssignmentChunks?.Any() == true) {
                var palette = MPD_File.CreatePalette(0);
                groundTileset = new MultiChunkTextureIndexed(MPD_File.GroundTilesetChunkDatas.Select(x => x.DecompressedData).ToArray(), TexturePixelFormat.Palette1, palette, true);

                var tiledGroundImageData = CreateTiledImageData(groundTileset, MPD_File.GroundTileAssignmentChunks.Select(x => x.PlaneTileTextureRowTable).ToArray(), 4);
                groundTiledImage = new TextureIndexed(0, 0, 0, 0, tiledGroundImageData, TexturePixelFormat.Palette1, palette, false);
            }

            if (MPD_File.SkyBoxChunkDatas?.Any() == true)
                skyBoxImage = new MultiChunkTextureIndexed(MPD_File.SkyBoxChunkDatas.Select(x => x.DecompressedData).ToArray(), TexturePixelFormat.Palette2, MPD_File.CreatePalette(1));

            if (MPD_File.BackgroundChunkDatas?.Any() == true)
                backgroundImage = new MultiChunkTextureIndexed(MPD_File.BackgroundChunkDatas.Select(x => x.DecompressedData).ToArray(), TexturePixelFormat.Palette1, MPD_File.CreatePalette(0));

            if (MPD_File.ForegroundTileChunkDatas?.Any() == true && MPD_File.ForegroundTileAssignmentChunk != null) {
                var palette = MPD_File.CreatePalette(1);
                foregroundTileset = new MultiChunkTextureIndexed(MPD_File.ForegroundTileChunkDatas.Select(x => x.DecompressedData).ToArray(), TexturePixelFormat.Palette1, palette, true);

                var foregroundImageData = CreateTiledImageData(foregroundTileset, new PlaneTileTextureRowTable[] { MPD_File.ForegroundTileAssignmentChunk.PlaneTileTextureRowTable}, 1);
                foregroundTiledImage = new TextureIndexed(0, 0, 0, 0, foregroundImageData, TexturePixelFormat.Palette2, palette, true);
            }

            GroundImage          = groundImage;
            GroundTileset        = groundTileset;
            GroundTiledImage     = groundTiledImage;
            SkyBoxImage          = skyBoxImage;
            BackgroundImage      = backgroundImage;
            ForegroundTileset    = foregroundTileset;
            ForegroundTiledImage = foregroundTiledImage;
        }

        private byte[,] CreateTiledImageData(ITexture tiledGroundTileImage, PlaneTileTextureRowTable[] tileMaps, int blockCountX) {
            const int c_tilesPerBlockX = 64;

            var tileImageData = tiledGroundTileImage.ImageData8Bit;
            var tileImageDataWidth  = tileImageData.GetLength(0);
            var tileImageDataHeight = tileImageData.GetLength(1);

            var tileCountX = tileImageDataWidth / 8;
            var tileCountY = tileImageDataHeight / 8;
            var tileCount = tileCountX * tileCountY;

            var imageTileCountX = c_tilesPerBlockX * blockCountX;
            var imageTileCountY = tileMaps.Sum(x => x.Length);
            var outputImage = new byte[imageTileCountX * 8, imageTileCountY * 8];

            // Precalculations for tile lookups
            var tileInputX = new int[tileCount];
            var tileInputY = new int[tileCount];
            int pos = 0;
            for (int y = 0; y < tileCountY; y++) {
                for (int x = 0; x < tileCountX; x++) {
                    tileInputX[pos]   = x * 8;
                    tileInputY[pos++] = y * 8;
                }
            }

/*
            var tileDataPosMap = new int[imageTileCountX, imageTileCountY];
            var tileAssignmentMap = new (int TilesetX, int TilesetY)[imageTileCountX, imageTileCountY];
            int blockXMax = blockCountX * tileSize;
            int tile = 0, tileInBlock = 0, blockX = 0, blockY = 0, tileInBlockX = 0, tileInBlockY = 0;
            foreach (var tileMap in tileMaps) {
                var data = tileMap.GetDataCopyOrReference();
                for (var dataPos = 0; dataPos < data.Length - 1; tile++, tileInBlock++, tileInBlockX++) {
                    // Reset some tile locations when we've reached the end of a block.
                    if (tileInBlock == tilesPerBlock) {
                        tileInBlock = 0;
                        tileInBlockX = 0;
                        tileInBlockY = 0;

                        // Move ahead one block, wrapping when blockXMax is reached.
                        blockX += tileSize;
                        if (blockX == blockXMax) {
                            blockX = 0;
                            blockY += tileSize;
                        }
                    }
                    // Make sure that tileInBlockX wraps.
                    else if (tileInBlockX == tileSize) {
                        tileInBlockX = 0;
                        tileInBlockY++;
                    }

                    tileDataPosMap[tileInBlockX + blockX, tileInBlockY + blockY] = dataPos;

                    var tileIndexValue = (data[dataPos] << 8) + data[dataPos + 1];
                    dataPos += 2;
                    var tileIndex = tileIndexValue / 2;
                    if (tileIndex >= tileCount) {
                        System.Diagnostics.Debug.WriteLine($"{dataPos:X4}: {tileIndex}");
                        continue;
                    }
                    if (tileIndexValue % 2 == 1)
                        ; // Wow!

                    tileAssignmentMap[tileInBlockX + blockX, tileInBlockY + blockY] = (tileIndex % tileCountX, tileIndex / tileCountX);
                }
            }
*/
            for (int tileY = 0; tileY < imageTileCountY; tileY++) {
                var tileMap = tileMaps[tileY / 128];
                var tileMapRow = tileMap[tileY % 128];
                for (int tileX = 0; tileX < imageTileCountX; tileX++) {
                    var tileIndex = tileMapRow[tileX] / 2;
                    var inputX = tileInputX[tileIndex];
                    var inputY = tileInputY[tileIndex];

                    var outputX = tileX * 8;
                    var outputY = tileY * 8;

                    for (int y = 0; y < 8; y++)
                        for (int x = 0; x < 8; x++)
                            outputImage[outputX + x, outputY + y] = tileImageData[inputX + x, inputY + y];
                }
            }

            return outputImage;
        }

        public IMPD_File MPD_File { get; }

        public ITexture GroundImage { get; private set; }
        public ITexture GroundTileset { get; private set; }
        public ITexture GroundTiledImage { get; private set; }
        public ITexture BackgroundImage { get; private set; }

        public ITexture SkyBoxImage { get; private set; }
        public ITexture ForegroundTileset { get; private set; }
        public ITexture ForegroundTiledImage { get; private set; }
    }
}
