using System.Collections.Generic;
using System.Linq;
using CommonLib.Extensions;
using CommonLib.Utils;

namespace SF3.MPD {
    public partial class MPD_Writer {
        public void WriteTextureChunks(List<ITexture> textures, int chunkCount) {
            var texturesByID = textures.ToDictionary(x => x.ID, x => x);
            int startID = 0;
            for (int i = 0; i < chunkCount; i++) {
                WriteTextureChunk(texturesByID, startID, out var textureCount);
                startID += textureCount;
            }
        }

        public void WriteTextureChunk(Dictionary<int /*id*/, ITexture> textures, int startID, out int textureCount) {
            int textureCountBigDumbLocal = 0;
            WriteCompressedChunk(writer => writer.WriteTextureChunkContent(textures, startID, out textureCountBigDumbLocal));
            textureCount = textureCountBigDumbLocal;
        }

        public void WriteTextureChunkContent(Dictionary<int /*id*/, ITexture> textures, int startID, out int textureCount) {
            // Figure out how many textures we can write here. Enforce a limit 0x10000 bytes worth of texture data.
            textureCount = 0;
            int totalTextureDataSize = 0;
            for (int id = startID; id < textures.Count && id < 0x100; id++) {
                if (!textures.TryGetValue(id, out var texture))
                    break;

                // Size of texture, also accounting for its entry in the table.
                // (Based on MPD analysis, this appears to be the limit.)
                var textureDataSize = 0x04 + texture.Width * texture.Height * texture.BytesPerPixel;

                if (totalTextureDataSize + textureDataSize >= 0x10000)
                    break;

                totalTextureDataSize += textureDataSize;
                textureCount++;

                // Stop here if the next texture's offset would be >= 0x10000, because that can't be stored in the table.
                if (totalTextureDataSize >= 0x10000)
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
                var texture = textures[i];
                WriteByte((byte) texture.Width);
                WriteByte((byte) texture.Height);
                WriteUShort(curTexOffset);

                curTexOffset += (ushort) (texture.Width * texture.Height * texture.BytesPerPixel);
            }

            // Write texture data.
            for (int i = startID; i < endID; i++) {
                var texture = textures[i];
                if (texture.BytesPerPixel == 1)
                    WriteBytes(texture.ImageData8Bit.To1DArrayTransposed());
                else
                    WriteBytes(texture.ImageData16Bit.To1DArrayTransposed().SelectMany(x => x.ToByteArray()).ToArray());
            }
        }
    }
}
