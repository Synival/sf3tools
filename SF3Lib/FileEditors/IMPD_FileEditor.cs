using SF3.Tables.MPD;

namespace SF3.FileEditors {
    public interface IMPD_FileEditor : ISF3FileEditor {
        HeaderTable Header { get; }
        TileRowTable TileRows { get; }
        ItemTileRowTable ItemTileRows { get; }
    }
}
