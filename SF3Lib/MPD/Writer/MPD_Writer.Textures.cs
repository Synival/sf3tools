using System.Collections.Generic;
using System.Linq;
using CommonLib.Extensions;
using CommonLib.Imaging;
using CommonLib.Utils;
using SF3.Imaging;

namespace SF3.MPD.Writer {
    public partial class MPD_Writer {
        public void WriteTextureChunks(IEnumerable<IMPD_Texture> textures, int chunkCount, int startID, bool allowIndexed, int? chunkSizeLimit = null) {
            if (textures == null)
                textures = new IMPD_Texture[0];

            var sortedTextures = textures
                .OrderBy(x => x.ID)
                .ToArray();

            for (int i = 0; i < chunkCount; i++) {
                WriteTextureChunk(sortedTextures, startID, out var textureCount, allowIndexed, chunkSizeLimit);
                startID += textureCount;
            }
        }

        public void WriteTextureChunk(IEnumerable<ITextureData> sortedTextures, int startID, out int textureCount, bool allowIndexed, int? chunkSizeLimit = null) {
            if (sortedTextures == null) {
                WriteEmptyChunk();
                textureCount = 0;
                return;
            }

            int textureCountBigDumbLocal = 0;
            WriteCompressedChunk(writer => writer.WriteTextureChunkContent(sortedTextures.ToArray(), startID, out textureCountBigDumbLocal, allowIndexed, chunkSizeLimit));
            textureCount = textureCountBigDumbLocal;
        }

        public void WriteTextureChunkContent(ITextureData[] sortedTextures, int startID, out int textureCount, bool allowIndexed, int? chunkSizeLimit = null) {
            // Figure out how many textures we can write here. Enforce a default limit 0xFFFF bytes worth of texture data.
            textureCount = 0;
            chunkSizeLimit = chunkSizeLimit ?? 0xFFFF;

            // (4 bytes for the initial header)
            int totalTextureDataSize = 0x04;

            for (int id = startID; id < sortedTextures.Count(); id++) {
                var texture = sortedTextures[id];

                // Size of texture, also accounting for its entry in the table.
                // (Based on MPD analysis, this appears to be the limit.)
                var textureDataSize = 0x04 + texture.Width * texture.Height * (allowIndexed ? texture.BytesPerPixel : 2);

                if (totalTextureDataSize + textureDataSize > chunkSizeLimit)
                    break;

                totalTextureDataSize += textureDataSize;
                textureCount++;

                // Stop here if the next texture's offset would be >= 0xFFFF, because that can't be stored in the table.
                if (totalTextureDataSize > chunkSizeLimit)
                    break;
            }

            // Write the texture ID table.
            // (There's a limit of 0x100 textures, which means these should be bytes, but they're stored as shorts anyway.)
            WriteUShort((byte) textureCount /* texture count */);
            WriteUShort((byte) startID /* texture start id */);

            // Determine where texture offsets are going to be, which is after the initial header (4 bytes) and the
            // width+height+offset table (4 bytes per texture).
            var curTexOffset = (ushort) (0x04 + textureCount * 0x04);

            // Write texture width+height+offset table.
            var endID = startID + textureCount;
            for (int i = startID; i < endID; i++) {
                var texture = sortedTextures[i];
                WriteByte((byte) texture.Width);
                WriteByte((byte) texture.Height);
                WriteUShort(curTexOffset);

                curTexOffset += (ushort) (texture.Width * texture.Height * (allowIndexed ? texture.BytesPerPixel : 2));
            }

            // Write texture data.
            if (allowIndexed) {
                for (int i = startID; i < endID; i++) {
                    var texture = sortedTextures[i];
                    var imageData = (texture.BytesPerPixel == 1)
                        ? texture.ImageData8Bit.To1DArrayTransposed()
                        : texture.ImageData16Bit.To1DArrayTransposed().ToBytes();
                    WriteBytes(imageData);
                }
            }
            else {
                for (int i = startID; i < endID; i++) {
                    var texture = sortedTextures[i];
                    var imageData = (texture.BytesPerPixel == 1)
                        ? texture.ImageData8Bit.To1DArrayTransposed().ConvertIndexedToABGR1555(texture.Palette, zeroIsTransparent: true)
                        : texture.ImageData16Bit.To1DArrayTransposed();
                    WriteBytes(imageData.ToBytes());
                }
            }
        }
    }
}
