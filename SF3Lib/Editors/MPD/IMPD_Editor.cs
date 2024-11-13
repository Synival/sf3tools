using CommonLib;
using SF3.RawEditors;
using SF3.Tables.MPD;

namespace SF3.Editors.MPD {
    public interface IMPD_Editor : IScenarioTableEditor {
        /// <summary>
        /// Byte editor for (de)compressed data for chunks
        /// </summary>
        IByteEditor[] ChunkEditors { get; }

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
