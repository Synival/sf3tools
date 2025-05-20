using System.Collections.Generic;
using SF3.Models.Tables.X014;

namespace SF3.Models.Files.X014 {
    public interface IX014_File : IScenarioTableFile {
        CharacterBattleModelsSc1Table CharacterBattleModelsSc1Table { get; }
        CharacterBattleModelsSc2Table CharacterBattleModelsSc2Table { get; }
        CharacterBattleModelsSc3Table CharacterBattleModelsSc3Table { get; }
        MPDBattleSceneIdTable MPDBattleSceneIdTable { get; }
        MPDBattleSceneInfoTable MPDBattleSceneInfoTable { get; }
        Dictionary<int, TerrainBasedBattleSceneTable> TerrainBasedBattleSceneTablesByRamAddress { get; }
        Sc1BattleSceneFileIdTable BattleScenesByMapTable { get; }
        Sc1BattleSceneFileIdTable BattleScenesByTerrainTable { get; }
        Sc1BattleSceneFileIdTable BattleScenesOtherTable { get; }
    }
}
