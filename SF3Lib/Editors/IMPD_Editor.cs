using MPDLib;
using SF3.RawEditors;
using SF3.Tables.MPD;
using SF3.Editors.MPD;

namespace SF3.Editors {
    public interface IMPD_Editor : IScenarioTableEditor {
        /// <summary>
        /// Byte editor for (de)compressed data for chunks
        /// </summary>
        IByteEditor[] ChunkEditors { get; }

        HeaderTable Header { get; }
        ColorTable[] Palettes { get; }
        ChunkHeaderTable ChunkHeader { get; }
        Chunk[] Chunks { get; }
        TileHeightmapRowTable TileHeightmapRows { get; }
        TileSurfaceCharacterRowTable TileSurfaceCharacterRows { get; }
        TileHeightRowTable TileHeightRows { get; }
        TileTerrainRowTable TileTerrainRows { get; }
        TileItemRowTable TileItemRows { get; }
        TextureChunkEditor[] TextureChunks { get; }
    }
}
