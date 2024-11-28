using SF3.TableModels.Shared;
using SF3.TableModels.X002;

namespace SF3.FileModels.X002 {
    public interface IX002_Editor : IScenarioTableEditor {
        ItemTable ItemTable { get; }
        SpellTable SpellTable { get; }
        WeaponSpellTable WeaponSpellTable { get; }
        LoadingTable LoadingTable { get; }
        StatBoostTable StatBoostTable { get; }
        WeaponRankTable WeaponRankTable { get; }
        AttackResistTable AttackResistTable { get; }
        WarpTable WarpTable { get; }
        LoadedOverrideTable LoadedOverrideTable { get; }
    }
}
