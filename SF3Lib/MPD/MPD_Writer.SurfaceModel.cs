namespace SF3.MPD {
    public partial class MPD_Writer {
        private static int c_surfaceModelBlockCount = 0x100;

        public void WriteSurfaceModelChunk(IMPD_Tile[,] tiles)
            => WriteUncompressedChunk(writer => writer.WriteSurfaceModelChunkContent(tiles));

        public void WriteSurfaceModelChunkContent(IMPD_Tile[,] tiles) {
/*
            0x0000 - 0x2000: TileTextureRowTable    | Textures
            0x2000 - 0xB600: VertexNormalBlockTable | Normals
            0xB600 - 0xCF00: VertexHeightBlockTable | Heightmap
*/

            // The surface model is stored as 256 4x4 blocks, in row major order.
            // There are 16 columns of blocks and 16 rows of blocks.

            // 0x10 words (2 bytes) for 0x100 blocks. Total 0x2000 bytes.
            for (int block = 0; block < c_surfaceModelBlockCount; block++) {
                int blockX = (block % 0x10) * 4;
                int blockY = (block / 0x10) * 4;
                for (int y = 0; y < 4; y++) {
                    for (int x = 0; x < 4; x++) {
                        var tile = tiles[x + blockX, y + blockY];
                        WriteUShort((ushort) ((tile.ModelTextureFlags << 8) | tile.ModelTextureID));
                    }
                }
            }

            // TODO: the rest of the owl
            const int remainingBytes = 0xCF00 - 0x2000;
            WriteBytes(new byte[remainingBytes]);
        }
    }
}
