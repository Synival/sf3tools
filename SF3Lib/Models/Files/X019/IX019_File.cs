using System.Collections.Generic;
using SF3.Models.Tables.Shared;

namespace SF3.Models.Files.X019 {
    public interface IX019_File : IScenarioTableFile, IMonsterTableFile {
        MonsterTable MonsterTable { get; }
    }
}
