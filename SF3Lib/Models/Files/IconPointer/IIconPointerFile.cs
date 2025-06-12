using SF3.Models.Tables.Shared;

namespace SF3.Models.Files.IconPointer {
    public interface IIconPointerFile : IScenarioTableFile {
        SpellIconTable SpellIconTable { get; }
        ItemIconTable ItemIconTable { get; }
    }
}
