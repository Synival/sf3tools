using SF3.Models.Structs.Shared;
using SF3.Models.Tables.Shared;

namespace SF3.Models.Files.X033 {
    public interface IX033_File : IScenarioTableFile {
        StatsTable StatsTable { get; }
        InitialInfoTable InitialInfoTable { get; }
        WeaponLevel WeaponLevelExp { get; }
    }
}
