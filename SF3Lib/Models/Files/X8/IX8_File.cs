using SF3.Models.Structs.X8;

namespace SF3.Models.Files.X8 {
    public interface IX8_File : IScenarioTableFile {
        BattleModelHeader Header { get; }
    }
}
