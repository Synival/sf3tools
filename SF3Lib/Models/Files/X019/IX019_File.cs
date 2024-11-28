using SF3.Models.Files;
using SF3.Models.Tables.X019;

namespace SF3.Models.Files.X019 {
    public interface IX019_File : IScenarioTableFile {
        MonsterTable MonsterTable { get; }
    }
}
