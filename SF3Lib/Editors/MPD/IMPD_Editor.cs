using CommonLib;
using SF3.RawEditors;
using SF3.Tables.MPD;

namespace SF3.Editors.MPD {
    public interface IMPD_Editor : IScenarioTableEditor {
        /// <summary>
        /// Recompresses compressed chunks, updating the ChunkHeader however necessary.
        /// All of the editor's data will be updated to contain the new recompressed data.
        /// This could result in a different data size.
        /// </summary>
        /// <param name="onlyModified">Only perform updates for modified compressed chunks.</param>
        /// <returns>'true' on success, otherwise 'false'.</returns>
        bool Recompress(bool onlyModified);

        /// <summary>
        /// Byte editor for (de)compressed data for chunks
        /// </summary>
        IByteEditor[] ChunkEditors { get; }

        /// <summary>
        /// All assigned child editors, including both CompressedEditors and ChunkEditors.
        /// </summary>
        IByteEditor[] ChildEditors { get; }

        HeaderTable Header { get; }
        ColorTable[] Palettes { get; }
        ChunkHeaderTable ChunkHeader { get; }
        Chunk[] Chunks { get; }
        TileSurfaceCharacterRowTable TileSurfaceCharacterRows { get; }
        TileSurfaceHeightmapRowTable TileSurfaceHeightmapRows { get; }
        TileHeightTerrainRowTable TileHeightTerrainRows { get; }
        TileItemRowTable TileItemRows { get; }
        TextureChunkEditor[] TextureChunks { get; }
    }
}
