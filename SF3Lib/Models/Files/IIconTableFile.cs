using SF3.Models.Tables.Shared;
using SF3.Types;

namespace SF3.Models.Files {
    public interface IIconTableFile {
        ScenarioType Scenario { get; }
        SpellIconTable SpellIconTable { get; }
        ItemIconTable ItemIconTable { get; }
    }
}
