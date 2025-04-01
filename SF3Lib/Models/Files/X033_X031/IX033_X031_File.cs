using SF3.Models.Structs.X033_X031;
using SF3.Models.Tables.X033_X031;

namespace SF3.Models.Files.X033_X031 {
    public interface IX033_X031_File : IScenarioTableFile {
        StatsTable StatsTable { get; }
        InitialInfoTable InitialInfoTable { get; }
        WeaponLevel WeaponLevelExp { get; }
    }
}
