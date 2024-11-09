using MPDLib;
using SF3.Models.MPD.TextureChunk;
using SF3.Tables.MPD;

namespace SF3.FileEditors {
    public interface IMPD_FileEditor : ISF3FileEditor {
        /// <summary>
        /// Entries for all chunks in the MPD file
        /// </summary>
        ChunkCollection Chunks { get; }

        /// <summary>
        /// Byte editor for (de)compressed data for chunks
        /// </summary>
        IByteEditor[] ChunkEditors { get; }

        HeaderTable Header { get; }
        TileHeightmapRowTable TileHeightmapRows { get; }
        TileSurfaceCharacterRowTable TileSurfaceCharacterRows { get; }
        TileHeightRowTable TileHeightRows { get; }
        TileTerrainRowTable TileTerrainRows { get; }
        TileItemRowTable TileItemRows { get; }
        TextureChunk[] TextureChunks { get; }
    }
}
