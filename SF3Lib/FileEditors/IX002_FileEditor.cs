using SF3.Tables.X002.AttackResist;
using SF3.Tables.X002.Items;
using SF3.Tables.X002.LoadedOverride;
using SF3.Tables.X002.Loading;
using SF3.Tables.X002.Preset;
using SF3.Tables.X002.Spells;
using SF3.Tables.X002.StatBoost;
using SF3.Tables.X002.Warp;
using SF3.Tables.X002.WeaponRank;

namespace SF3.FileEditors {
    public interface IX002_FileEditor : ISF3FileEditor {
        ItemList ItemList { get; }
        SpellList SpellList { get; }
        PresetList PresetList { get; }
        LoadList LoadList { get; }
        StatList StatList { get; }
        WeaponRankList WeaponRankList { get; }
        AttackResistList AttackResistList { get; }
        WarpList WarpList { get; }
        LoadedOverrideList LoadedOverrideList { get; }
    }
}
