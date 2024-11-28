using SF3.Models.Files;
using SF3.Models.Tables.IconPointer;

namespace SF3.Models.Files.IconPointer {
    public interface IIconPointerFile : IScenarioTableFile {
        SpellIconTable SpellIconTable { get; }
        ItemIconTable ItemIconTable { get; }
    }
}
