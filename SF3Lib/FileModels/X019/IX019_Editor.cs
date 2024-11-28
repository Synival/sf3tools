using SF3.TableModels.X019;

namespace SF3.FileModels.X019 {
    public interface IX019_Editor : IScenarioTableEditor {
        MonsterTable MonsterTable { get; }
    }
}
