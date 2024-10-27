using SF3.Tables;
using SF3.Tables.X002;

namespace SF3.FileEditors {
    public interface IX002_FileEditor : ISF3FileEditor {
        ItemTable ItemTable { get; }
        SpellTable SpellTable { get; }
        PresetTable PresetTable { get; }
        LoadTable LoadTable { get; }
        StatTable StatTable { get; }
        WeaponRankTable WeaponRankTable { get; }
        AttackResistTable AttackResistTable { get; }
        WarpTable WarpTable { get; }
        LoadedOverrideTable LoadedOverrideTable { get; }
    }
}
