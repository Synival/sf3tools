using MPDLib;
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
        SurfaceCharacterRowTable SurfaceCharacterRows { get; }
        TileRowTable TileRows { get; }
        ItemTileRowTable ItemTileRows { get; }
    }
}
