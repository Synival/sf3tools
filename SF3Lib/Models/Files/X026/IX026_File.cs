using SF3.Models.Tables.Shared;

namespace SF3.Models.Files.X026 {
    public interface IX026_File : IScenarioTableFile {
        SpellIconTable SpellIconTable { get; }
        ItemIconTable ItemIconTable { get; }
    }
}
