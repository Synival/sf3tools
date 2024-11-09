using SF3.FileEditors;
using STHAEditor.Models.Items;
using STHAEditor.Models.Presets;

namespace SF3.TileEditor {
    public interface ITileFileEditor : ISF3FileEditor {
        TileRowTable TileRows { get; }
        ItemTileRowTable ItemTileRows { get; }
    }
}
