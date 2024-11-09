using SF3.Tables;

namespace SF3.FileEditors {
    public interface IMPD_FileEditor : ISF3FileEditor {
        TileRowTable TileRows { get; }
        ItemTileRowTable ItemTileRows { get; }
    }
}
