using SF3.Tables.X002;

namespace SF3.FileEditors {
    public interface IX002_FileEditor : ISF3FileEditor {
        ItemTable ItemList { get; }
        SpellTable SpellList { get; }
        PresetTable PresetList { get; }
        LoadTable LoadList { get; }
        StatTable StatList { get; }
        WeaponRankTable WeaponRankList { get; }
        AttackResistTable AttackResistList { get; }
        WarpTable WarpList { get; }
        LoadedOverrideTable LoadedOverrideList { get; }
    }
}
