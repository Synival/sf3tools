using SF3.Models.Structs.Shared;
using SF3.Models.Tables.Shared;

namespace SF3.Models.Files.X031 {
    public interface IX031_File : IScenarioTableFile {
        StatsTable StatsTable { get; }
        StatGrowthStatisticsTable StatGrowthStatistics { get; }
        InitialInfoTable InitialInfoTable { get; }
        WeaponLevel WeaponLevelExp { get; }
    }
}
