using System.Collections.Generic;
using CommonLib;
using SF3.Models.Files;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD;
using SF3.Models.Tables.MPD.TextureAnimation;
using SF3.RawData;

namespace SF3.Models.Files.MPD {
    public interface IMPD_File : IScenarioTableFile {
        /// <summary>
        /// Recompresses compressed chunks, updating the ChunkHeader however necessary.
        /// All of the file's data will be updated to contain the new recompressed data.
        /// This could result in a different data size.
        /// </summary>
        /// <param name="onlyModified">Only perform updates for modified compressed chunks.</param>
        /// <returns>'true' on success, otherwise 'false'.</returns>
        bool Recompress(bool onlyModified);

        /// <summary>
        /// Byte data for (de)compressed data for chunks
        /// </summary>
        IChunkData[] ChunkData { get; }
        IChunkData SurfaceChunkData { get; }

        MPDHeaderTable MPDHeader { get; }
        ColorTable[] Palettes { get; }
        ChunkHeaderTable ChunkHeader { get; }
        UnknownUInt16Table Offset1Table { get; }
        UnknownUInt32Table Offset2Table { get; }
        UnknownUInt16Table Offset3Table { get; }
        Offset4Table Offset4Table { get; }

        TextureAnimationTable TextureAnimations { get; }

        /// <summary>
        /// The compressed data for each texture in Chunk3, keyed by its offset.
        /// </summary>
        Dictionary<uint, CompressedData> Chunk3Frames { get; }

        Chunk[] Chunks { get; }

        Chunk SurfaceChunk { get; }
        TileSurfaceCharacterRowTable TileSurfaceCharacterRows { get; }
        TileSurfaceHeightmapRowTable TileSurfaceHeightmapRows { get; }
        TileHeightTerrainRowTable TileHeightTerrainRows { get; }
        TileItemRowTable TileItemRows { get; }

        MPD_FileTextureChunk[] TextureChunks { get; }
    }
}
