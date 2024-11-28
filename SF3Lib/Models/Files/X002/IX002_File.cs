using SF3.Models.Files;
using SF3.Models.Tables.Shared;
using SF3.Models.Tables.X002;

namespace SF3.Models.Files.X002 {
    public interface IX002_File : IScenarioTableFile {
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
