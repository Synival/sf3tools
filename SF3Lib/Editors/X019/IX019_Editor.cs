using SF3.Tables.X019;

namespace SF3.Editors.X019 {
    public interface IX019_Editor : IScenarioTableEditor {
        MonsterTable MonsterTable { get; }
    }
}