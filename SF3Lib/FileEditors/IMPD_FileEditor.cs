using MPDLib;
using SF3.Tables.MPD;

namespace SF3.FileEditors {
    public interface IMPD_FileEditor : ISF3FileEditor {
        /// <summary>
        /// Entries for all chunks in the MPD file
        /// </summary>
        ChunkCollection Chunks { get; }

        /// <summary>
        /// Byte editor for decompressed data in Chunk 5
        /// </summary>
        IByteEditor Chunk5Editor { get; }

        HeaderTable Header { get; }
        TileRowTable TileRows { get; }
        ItemTileRowTable ItemTileRows { get; }
    }
}
