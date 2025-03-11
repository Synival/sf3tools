using System.Collections.Generic;
using SF3.Models.Tables.X014;

namespace SF3.Models.Files.X014 {
    public interface IX014_File : IScenarioTableFile {
        MPDBattleSceneInfoTable MPDBattleSceneInfoTable { get; }
        Dictionary<int, TerrainBasedBattleSceneTable> TerrainBasedBattleSceneTablesByRamAddress { get; }
    }
}
