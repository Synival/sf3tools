using SF3.Models.Structs.X8PC;

namespace SF3.Models.Files.X8PC {
    public interface IX8PC_File : IScenarioTableFile {
        BattleModelHeader Header { get; }
    }
}
