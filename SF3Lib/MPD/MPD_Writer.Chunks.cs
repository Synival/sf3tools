using System;
using System.Collections.Generic;
using System.IO;
using CommonLib.Extensions;
using CommonLib.Utils;
using SF3.Imaging;
using SF3.Types;

namespace SF3.MPD {
    public partial class MPD_Writer {
        public void WriteChunks(IMPD mpd, byte[] chunk3Data) {
            bool allowIndexedTextures = Scenario >= ScenarioType.Scenario3;

            // Chunk[0] is always empty.
            WriteEmptyChunk();

            // Chunk[1] is always models for Scenario 1, but only low-memory models for Scenario 2+.
            var primaryMc = mpd.ModelCollections.TryGetValue(MPD_CollectionType.Primary, out var mcOut) ? mcOut : null;
            var extraMc   = mpd.ModelCollections.TryGetValue(MPD_CollectionType.ExtraModels, out mcOut) ? mcOut : null;

            if (primaryMc != null && mpd.Flags.ModelChunkIndex == 1)
                WriteModelChunk(primaryMc.Models, primaryMc.ModelInstances, mpd.Collisions, mpd.Flags.ModelsMemoryLocation == MemoryLocationType.HighMemory);
            else if (extraMc != null && mpd.Flags.Bit_0x4000_HasExtraChunk1ModelWithChunk21Textures)
                WriteModelChunk(primaryMc.Models, primaryMc.ModelInstances, mpd.Collisions, isHighMemory: false);
            else
                WriteEmptyChunk();

            // Chunk[2] is the surface model, but sometimes Chunk[20] for Scenario 2+.
            if (mpd.Surface.HasModel && mpd.Flags.SurfaceModelChunkIndex == 2)
                WriteSurfaceModelChunk(mpd.Surface);
            else
                WriteEmptyChunk();

            // Chunk[3] has animated texture frames, built earlier when building the table.
            if (chunk3Data != null)
                WriteDataChunk(chunk3Data);
            else
                WriteEmptyChunk();

            // Chunk[4] is always empty.
            WriteEmptyChunk();

            // Chunk[5] is the surface (heightmap + gameplay, not the model for rendering).
            WriteSurfaceDataChunk(mpd.Surface);

            // Chunk[6, 7, 8, 9, 10] are all textures.
            IEnumerable<IMPD_AnimatableTexture> GetTexturesForCollection(MPD_CollectionType collection) {
                if (!mpd.ModelCollections.TryGetValue(collection, out var mc))
                    return new IMPD_AnimatableTexture[0];
                return mc.Textures ?? new IMPD_AnimatableTexture[0].ToEnumerableWithLength();
            }

            // In Scenario 1, Chunk[10] belongs to a different collection of textures. This is used for the Titan in Z_AS.MPD.
            if (mpd.Flags.Bit_0x0080_HasChunk19ModelWithChunk10Textures) {
                WriteTextureChunks(GetTexturesForCollection(MPD_CollectionType.Primary),     chunkCount: 4, startID: 0, allowIndexed: allowIndexedTextures);
                WriteTextureChunks(GetTexturesForCollection(MPD_CollectionType.ExtraModels), chunkCount: 1, startID: 0, allowIndexed: false);
            }
            else
                WriteTextureChunks(GetTexturesForCollection(MPD_CollectionType.Primary), chunkCount: 5, startID: 0, allowIndexed: allowIndexedTextures);

            // Chunk[11, 12, 13] are textures for Chest1, Chest2, and Barrel.
            // (it's so silly that it works this way, lol)
            var chest1Textures = GetTexturesForCollection(MPD_CollectionType.Chest);
            var chest2Textures = GetTexturesForCollection(MPD_CollectionType.LockedChest);
            var barrelTextures = GetTexturesForCollection(MPD_CollectionType.Barrel);

            WriteTextureChunk(chest1Textures, 0, out _, allowIndexed: false);
            WriteTextureChunk(chest2Textures, 0, out _, allowIndexed: false);
            WriteTextureChunk(barrelTextures, 0, out _, allowIndexed: false);

            // Ground + sky chunks.
            WritePlaneChunks(mpd.Planes, mpd.Flags.Bit_0x0080_HasChunk19ModelWithChunk10Textures);

            // In Scenario 1, Chunk[19] is the Titan model used in Z_AS.MPD.
            if (mpd.Flags.Bit_0x0080_HasChunk19ModelWithChunk10Textures) {
                if (mpd.ModelCollections.TryGetValue(MPD_CollectionType.ExtraModels, out var extraModels))
                    WriteModelChunk(extraModels.Models, extraModels.ModelInstances, null, isHighMemory: true);
                else
                    WriteEmptyChunk();
            }

            // Scenario 2+ has two more chunks.
            if (Scenario >= ScenarioType.Scenario2) {
                if (primaryMc != null && mpd.Flags.ModelsMemoryLocation == MemoryLocationType.HighMemory)
                    WriteModelChunk(primaryMc.Models, primaryMc.ModelInstances, mpd.Collisions, isHighMemory: true);
                else if (mpd.Surface.HasModel && mpd.Flags.SurfaceModelChunkIndex == 20)
                    WriteSurfaceModelChunk(mpd.Surface);
                else
                    WriteEmptyChunk();

                if (extraMc != null && mpd.Flags.Bit_0x4000_HasExtraChunk1ModelWithChunk21Textures) {
                    var extraTextures = GetTexturesForCollection(MPD_CollectionType.ExtraModels);
                    WriteTextureChunk(extraTextures, 0, out _, allowIndexed: allowIndexedTextures);
                }
                else
                    WriteEmptyChunk();
            }
        }

        public void WriteEmptyChunk()
            => WriteUncompressedChunk(_ => {});

        public void WriteDataChunk(byte[] data)
            => WriteUncompressedChunk(_ => WriteBytes(data));

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
