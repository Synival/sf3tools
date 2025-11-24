using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommonLib.Utils;
using SF3.Types;

namespace SF3.MPD {
    public partial class MPD_Writer {
        public void WriteChunks(IMPD mpd) {
            // Chunk[0] is always empty.
            WriteEmptyChunk();

            // TODO: check for this, and get memory mapping stuff!!
            // Chunk[1] is always models if it exists.
            // TODO: In Scenario 2+, this could be Chunk[20].
            if (!mpd.ModelCollections.TryGetValue(CollectionType.Primary, out var mc))
                WriteEmptyChunk();
            else
                WriteModelChunk(mc.Models, mc.ModelInstances, mpd.Flags.HighMemoryHasModels);

            // Chunk[2] is the surface model.
            // TODO: In Scenario 2+, this could be Chunk[20].
            if (mpd.Surface.HasModel)
                WriteSurfaceModelChunk(mpd.Surface);
            else
                WriteEmptyChunk();

            // TODO: Chunk[3] (animated texture frames)
            WriteEmptyChunk();

            // Chunk[4] is always empty.
            WriteEmptyChunk();

            // Chunk[5] is the surface (heightmap + gameplay, not the model for rendering).
            WriteSurfaceDataChunk(mpd.Surface);

            // Chunk[6, 7, 8, 9, 10] are all textures.
            IEnumerable<ITexture> GetTexturesForCollection(CollectionType collection) {
                if (!mpd.ModelCollections.TryGetValue(collection, out mc))
                    return new ITexture[0];
                return mc.Textures ?? new ITexture[0];
            }

            // In Scenario 1, Chunk[10] belongs to a different collection of textures. This is used for the Titan in Z_AS.MPD.
            if (mpd.Flags.HasChunk19Model) {
                WriteTextureChunks(GetTexturesForCollection(CollectionType.Primary), chunkCount: 4, startID: 0);
                WriteTextureChunks(GetTexturesForCollection(CollectionType.ExtraModel), chunkCount: 1, startID: 0);
            }
            else
                WriteTextureChunks(GetTexturesForCollection(CollectionType.Primary), chunkCount: 5, startID: 0);

            // Chunk[11, 12, 13] are textures for Chest1, Chest2, and Barrel.
            // (it's so silly that it works this way, lol)
            var chest1Textures = GetTexturesForCollection(CollectionType.MovableModels1);
            var chest2Textures = GetTexturesForCollection(CollectionType.MovableModels2);
            var barrelTextures = GetTexturesForCollection(CollectionType.MovableModels3);

            WriteTextureChunk(chest1Textures, 0, out _);
            WriteTextureChunk(chest2Textures, 0, out _);
            WriteTextureChunk(barrelTextures, 0, out _);

            // TODO: ground + skybox chunks
            // TODO: Chunk[19] for the Titan

            // TODO: actual chunks!!
            while (_currentChunks < 20)
                WriteEmptyChunk();
        }

        public void WriteEmptyChunk()
            => WriteUncompressedChunk(_ => {});

        private void WriteUncompressedChunk(Action<MPD_Writer> writerFunc) {
            // Write the address of this chunk in the chunk table.
            var currentChunkStart = CurrentOffset;
            AtOffset(0x2000 + _currentChunks * 0x08, curOffset => WriteMPDPointer((uint) curOffset));

            // Perform the actual writing.
            writerFunc(this);

            // Write the size of the chunk.
            AtOffset(0x2000 + _currentChunks * 0x08 + 0x04, curOffset => WriteUInt((uint) (curOffset - currentChunkStart)));

            // Get ready for the next chunk.
            WriteToAlignTo(4);
            _currentChunks++;
        }

        private void WriteCompressedChunk(Action<MPD_Writer> writerFunc) {
            // Write the address of this chunk in the chunk table.
            var currentChunkStart = CurrentOffset;
            AtOffset(0x2000 + _currentChunks * 0x08, curOffset => WriteMPDPointer((uint) curOffset));

            // Write to an uncompressed buffer.
            byte[] uncompressedData = null;
            using (var ms = new MemoryStream()) {
                writerFunc(new MPD_Writer(ms, Scenario));
                uncompressedData = ms.ToArray();
            }

            // Write the compressed data out.
            WriteBytes(Compression.CompressLZSS(uncompressedData));

            // Write the size of the chunk.
            AtOffset(0x2000 + _currentChunks * 0x08 + 0x04, curOffset => WriteUInt((uint) (curOffset - currentChunkStart)));

            // Get ready for the next chunk.
            WriteToAlignTo(4);
            _currentChunks++;
        }

        private int _currentChunks = 0;
    }
}
