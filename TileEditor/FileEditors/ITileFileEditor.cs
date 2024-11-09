using SF3.FileEditors;
using SF3.TileEditor.Tables;

namespace SF3.TileEditor.FileEditors {
    public interface ITileFileEditor : ISF3FileEditor {
        TileRowTable TileRows { get; }
        ItemTileRowTable ItemTileRows { get; }
    }
}
