using SF3.Models.Tables.Shared;

namespace SF3.Models.Files.X044 {
    public interface IX044_File : IScenarioTableFile {
        MonsterTable MonsterTable { get; }
    }
}
