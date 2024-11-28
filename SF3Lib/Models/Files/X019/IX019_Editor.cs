using SF3.Models.Files;
using SF3.Models.Tables.X019;

namespace SF3.Models.Files.X019 {
    public interface IX019_Editor : IScenarioTableEditor {
        MonsterTable MonsterTable { get; }
    }
}
