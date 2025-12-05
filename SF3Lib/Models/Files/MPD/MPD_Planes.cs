using System;
using System.Linq;
using SF3.ByteData;
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

            if (MPD_File.GroundTilesetChunkDatas?.Any() == true && MPD_File.GroundTileAssignmentChunkDatas?.Any() == true) {
                var palette = MPD_File.CreatePalette(0);
                groundTileset = new MultiChunkTextureIndexed(MPD_File.GroundTilesetChunkDatas.Select(x => x.DecompressedData).ToArray(), TexturePixelFormat.Palette1, palette, true);

                var tiledGroundImageData = CreateTiledImageData(groundTileset, MPD_File.GroundTileAssignmentChunkDatas.Select(x => x.DecompressedData).ToArray(), 64, 4);
                groundTiledImage = new TextureIndexed(0, 0, 0, 0, tiledGroundImageData, TexturePixelFormat.Palette1, palette, false);
            }

            if (MPD_File.SkyBoxChunkDatas?.Any() == true)
                skyBoxImage = new MultiChunkTextureIndexed(MPD_File.SkyBoxChunkDatas.Select(x => x.DecompressedData).ToArray(), TexturePixelFormat.Palette2, MPD_File.CreatePalette(1));

            if (MPD_File.BackgroundChunkDatas?.Any() == true)
                backgroundImage = new MultiChunkTextureIndexed(MPD_File.BackgroundChunkDatas.Select(x => x.DecompressedData).ToArray(), TexturePixelFormat.Palette1, MPD_File.CreatePalette(0));

            if (MPD_File.ForegroundTileChunkDatas?.Any() == true && MPD_File.ForegroundTileAssignmentChunkData != null) {
                var palette = MPD_File.CreatePalette(1);
                foregroundTileset = new MultiChunkTextureIndexed(MPD_File.ForegroundTileChunkDatas.Select(x => x.DecompressedData).ToArray(), TexturePixelFormat.Palette1, palette, true);

                var foregroundImageData = CreateTiledImageData(foregroundTileset, new IByteData[] { MPD_File.ForegroundTileAssignmentChunkData.DecompressedData }, 64, 1);
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

        private byte[,] CreateTiledImageData(ITexture tiledGroundTileImage, IByteData[] tileMaps, int tileSize, int blockCountX) {
            int tilesPerBlock = tileSize * tileSize;

            // Count the number of tiles (they're 16 bits, so divide the byte count by 2)
            var tileMapTileCount = tileMaps.Sum(x => x.Length) / 2;

            var blockCountYf = (float) tileMapTileCount / tilesPerBlock / blockCountX;

            var tileImageData = tiledGroundTileImage.ImageData8Bit;
            var tileImageDataWidth  = tileImageData.GetLength(0);
            var tileImageDataHeight = tileImageData.GetLength(1);

            var tileCountX = tileImageDataWidth / 8;
            var tileCountY = tileImageDataHeight / 8;
            var tileCount = tileCountX * tileCountY;
            var outputImage = new byte[tileSize * blockCountX * 8, (int) Math.Ceiling(tileSize * blockCountYf) * 8];

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

                    var tileIndex = ((data[dataPos++] << 8) + data[dataPos++]) / 2;
                    if (tileIndex >= tileCount) {
                        System.Diagnostics.Debug.WriteLine($"{dataPos:X4}: {tileIndex}");
                        continue;
                    }

                    var inputX = tileInputX[tileIndex];
                    var inputY = tileInputY[tileIndex];

                    var outputX = (tileInBlockX + blockX) * 8;
                    var outputY = (tileInBlockY + blockY) * 8;

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
