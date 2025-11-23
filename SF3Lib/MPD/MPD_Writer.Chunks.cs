using System;
using System.IO;
using System.Linq;
using CommonLib.Utils;
using SF3.Models.Files.MPD;
using SF3.Types;

namespace SF3.MPD {
    public partial class MPD_Writer {
        public void WriteChunks(IMPD_File mpd) {
            // Chunk[0] is always empty.
            WriteEmptyChunk();

            // TODO: check for this, and get memory mapping stuff!!
            // Chunk[1] is always models if it exists.
            // TODO: In Scenario 2+, this could be Chunk[20].
            var mc = mpd.ModelCollections.FirstOrDefault(x => x?.CollectionType == ModelCollectionType.PrimaryModels);
            if (mc == null)
                WriteEmptyChunk();
            else
                WriteModelChunk(mc.GetSGLModels(), mc.GetModelInstances(), mpd.Flags.HighMemoryHasModels);

            // Chunk[2] is the surface model.
            // TODO: In Scenario 2+, this could be Chunk[20].
            if (mpd.SurfaceModel == null)
                WriteEmptyChunk();
            else
                WriteSurfaceModelChunk(mpd.Tiles);

            // TODO: Chunk[3] (animated texture frames)
            WriteEmptyChunk();

            // Chunk[4] is always empty.
            WriteEmptyChunk();

            // Chunk[5] is the surface (heightmap + gameplay, not the model for rendering).
            WriteSurfaceChunk(mpd.Tiles);

            // Chunk[6, 7, 8, 9, 10] are all textures.
            // In Scenario 1, Chunk[10] belongs to a different collection of textures. This is used for the Titan in Z_AS.MPD.
            bool IdBelongsToTextureCollection(int id, TextureCollectionType collection) {
                var min = (int) collection * TextureCollection.IDsPerCollectionType;
                return (id >= min && id < min + TextureCollection.IDsPerCollectionType);
            }
            if (mpd.Flags.HasChunk19Model) {
                WriteTextureChunks(mpd.Textures.Where(x => IdBelongsToTextureCollection(x.ID, TextureCollectionType.PrimaryTextures)).ToArray(), chunkCount: 4, startID: 0);
                WriteTextureChunks(mpd.Textures.Where(x => IdBelongsToTextureCollection(x.ID, TextureCollectionType.Chunk19ModelTextures)).ToArray(), chunkCount: 1, startID: 0);
            }
            else
                WriteTextureChunks(mpd.Textures.Where(x => IdBelongsToTextureCollection(x.ID, TextureCollectionType.PrimaryTextures)).ToArray(), chunkCount: 5, startID: 0);

            // Chunk[11, 12, 13] are textures for Chest1, Chest2, and Barrel.
            // (it's so silly that it works this way, lol)
            var chest1Textures = mpd.Textures.Where(x => IdBelongsToTextureCollection(x.ID, TextureCollectionType.MovableModels1)).ToArray();
            var chest2Textures = mpd.Textures.Where(x => IdBelongsToTextureCollection(x.ID, TextureCollectionType.MovableModels2)).ToArray();
            var barrelTextures = mpd.Textures.Where(x => IdBelongsToTextureCollection(x.ID, TextureCollectionType.MovableModels3)).ToArray();

            WriteTextureChunk(chest1Textures, 0, out _);
            WriteTextureChunk(chest2Textures, 0, out _);
            WriteTextureChunk(barrelTextures, 0, out _);

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
