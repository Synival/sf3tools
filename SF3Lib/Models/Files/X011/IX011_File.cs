using SF3.Models.Tables.Shared;

namespace SF3.Models.Files.X011 {
    public interface IX011_File : IScenarioTableFile {
        SpellIconTable SpellIconTable { get; }
        ItemIconTable ItemIconTable { get; }
    }
}
