using SF3.Tables.X019;

namespace SF3.Editors {
    public interface IX019_FileEditor : IScenarioTableEditor {
        MonsterTable MonsterTable { get; }
    }
}
