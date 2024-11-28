using SF3.Models.Files;
using SF3.Models.Tables.IconPointer;

namespace SF3.Models.Files.IconPointer {
    public interface IIconPointerEditor : IScenarioTableEditor {
        SpellIconTable SpellIconTable { get; }
        ItemIconTable ItemIconTable { get; }
    }
}
