using MPDLib;
using SF3.Tables.MPD;

namespace SF3.FileEditors {
    public interface IMPD_FileEditor : ISF3FileEditor {
        /// <summary>
        /// All data and functions for an MPD file
        /// </summary>
        MPDFile MPDFile { get; }

        /// <summary>
        /// Byte editor for decompressed data in Chunk 5
        /// </summary>
        IByteEditor Chunk5Editor { get; }

        HeaderTable Header { get; }
        TileRowTable TileRows { get; }
        ItemTileRowTable ItemTileRows { get; }
    }
}
