using SF3.Models.Tables.Shared;

namespace SF3.Models.Files.X021 {
    public interface IX021_File : IScenarioTableFile {
        SpellIconTable SpellIconTable { get; }
        ItemIconTable ItemIconTable { get; }
    }
}
