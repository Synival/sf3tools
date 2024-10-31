using SF3.Tables;

namespace SF3.FileEditors {
    public interface IX002_FileEditor : ISF3FileEditor {
        ItemTable ItemTable { get; }
        SpellTable SpellTable { get; }
        PresetTable PresetTable { get; }
        LoadingTable LoadingTable { get; }
        StatBoostTable StatBoostTable { get; }
        WeaponRankTable WeaponRankTable { get; }
        AttackResistTable AttackResistTable { get; }
        WarpTable WarpTable { get; }
        LoadedOverrideTable LoadedOverrideTable { get; }
    }
}
