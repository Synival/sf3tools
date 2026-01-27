using System;
using CommonLib.Extensions;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Writer {
    public partial class MPD_Writer {
        public void WritePlaneChunks(IMPD_Planes planes, bool lastChunkIsReservedForModels) {
            // Write the background image.
            var bgImage = planes.GroundImage ?? planes.GroundTiledImage?.Tileset ?? planes.BackgroundImage;
            if (bgImage != null && bgImage.Width == 512 && bgImage.Height == 256 && bgImage.BytesPerPixel == 1) {
                var data = (bgImage == planes.GroundTiledImage?.Tileset) ? bgImage.ImageData8Bit.FromTiles(8, 8) : bgImage.ImageData8Bit.To1DArrayTransposed();
                WriteCompressedChunk(writer => writer.WriteBytes(data, 0x00000, 0x10000));
                WriteCompressedChunk(writer => writer.WriteBytes(data, 0x10000, 0x10000));
            }
            else {
                WriteEmptyChunk();
                WriteEmptyChunk();
            }

            // 1/2 of ground tile assignments are written here.
            if (planes.GroundTiledImage?.TileAssignment != null)
                WriteTileAssignmentChunk(planes.GroundTiledImage.TileAssignment, startBlock: 0, 8);
            else
                WriteEmptyChunk();

            // Write the background image.
            var fgImage = planes.SkyImage ?? planes.ForegroundTiledImage?.Tileset;
            if (fgImage != null && fgImage.Width == 512 && fgImage.Height == 256 && fgImage.BytesPerPixel == 1) {
                var data = (fgImage == planes.ForegroundTiledImage?.Tileset) ? fgImage.ImageData8Bit.FromTiles(8, 8) : fgImage.ImageData8Bit.To1DArrayTransposed();
                WriteCompressedChunk(writer => writer.WriteBytes(data, 0x00000, 0x10000));
                WriteCompressedChunk(writer => writer.WriteBytes(data, 0x10000, 0x10000));
            }
            else {
                WriteEmptyChunk();
                WriteEmptyChunk();
            }

            // TODO: This incredibly stupid scenario should be checked for. Maybe we throw if this ever happens?
            if (!lastChunkIsReservedForModels) {
                // 2/2 of ground tile assignments are written here.
                if (planes.GroundTiledImage?.TileAssignment != null)
                    WriteTileAssignmentChunk(planes.GroundTiledImage.TileAssignment, startBlock: 8, 16);
                // Foreground tile assignment shares the same chunk.
                else if (planes.ForegroundTiledImage?.TileAssignment != null)
                    WriteTileAssignmentChunk(planes.ForegroundTiledImage.TileAssignment, startBlock: 0, 1);
                else
                    WriteEmptyChunk();
            }
        }

        public void WriteTileAssignmentChunk(IMPD_PlaneTileAssignment tileAssignment, int startBlock, int stopBlock)
            => WriteCompressedChunk(writer => writer.WriteTileAssignmentChunkContent(tileAssignment, startBlock, stopBlock));

        public void WriteTileAssignmentChunkContent(IMPD_PlaneTileAssignment tileAssignment, int startBlock, int stopBlock) {
            for (int block = startBlock; block < stopBlock; block++) {
                var blockX = (block % 4) * 64;
                var blockY = (block / 4) * 64;

                var blockMaxX = Math.Min(tileAssignment.Width, blockX + 64);
                var blockMaxY = Math.Min(tileAssignment.Height, blockY + 64);

                for (int y = blockY; y < blockMaxY; y++) {
                    for (int x = blockX; x < blockMaxX; x++) {
                        (var tileX, var tileY) = tileAssignment[(byte) x, (byte) y];
                        WriteUShort((ushort) (((tileX & 0x3F) << 1) | (tileY << 7)));
                    }
                }
            }
        }
    }
}
