using MPDLib;
using SF3.RawEditors;
using SF3.Models.MPD.TextureChunk;
using SF3.Tables.MPD;

namespace SF3.FileEditors {
    public interface IMPD_FileEditor : ISF3FileEditor {
        /// <summary>
        /// Byte editor for (de)compressed data for chunks
        /// </summary>
        IRawEditor[] ChunkEditors { get; }

        HeaderTable Header { get; }
        ColorTable[] Palettes { get; }
        ChunkHeaderTable ChunkHeader { get; }
        Chunk[] Chunks { get; }
        TileHeightmapRowTable TileHeightmapRows { get; }
        TileSurfaceCharacterRowTable TileSurfaceCharacterRows { get; }
        TileHeightRowTable TileHeightRows { get; }
        TileTerrainRowTable TileTerrainRows { get; }
        TileItemRowTable TileItemRows { get; }
        TextureChunk[] TextureChunks { get; }
    }
}
