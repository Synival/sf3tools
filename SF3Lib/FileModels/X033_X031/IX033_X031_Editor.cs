using SF3.Tables.X033_X031;

namespace SF3.FileModels.X033_X031 {
    public interface IX033_X031_Editor : IScenarioTableEditor {
        StatsTable StatsTable { get; }
        InitialInfoTable InitialInfoTable { get; }
        WeaponLevelTable WeaponLevelTable { get; }
    }
}
